package com.farsitel.bazaar

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import androidx.fragment.app.FragmentActivity
import com.farsitel.bazaar.callback.PaymentCallback
import ir.cafebazaar.poolakey.request.PurchaseRequest
import java.security.InvalidParameterException

class PaymentActivity : FragmentActivity() {
    private var productId: String? = null
    private var payload: String? = null
    private var command: Command? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        initArgs()
        when (command) {
            Command.PurchaseProduct -> purchaseProduct()
            Command.Subscribe -> subscribeProduct()
            else -> throw InvalidParameterException("Undefined command: $command")
        }
    }

    private fun purchaseProduct() {
        PoolakeyKotlinBridge.payment.purchaseProduct(
            this@PaymentActivity,
            PurchaseRequest(productId!!, REQUEST_CODE, payload)
        ) {
            purchaseFlowBegan {
                // Bazaar's billing screen has opened successfully
            }
            failedToBeginFlow { t ->
                // Failed to open Bazaar's billing screen
                paymentCallback?.onFailure(t.message, t.stackTrace.joinToString("\n"))
                finish()
            }
        }
    }

    private fun subscribeProduct() {
        PoolakeyKotlinBridge.payment.subscribeProduct(
            this@PaymentActivity,
            PurchaseRequest(productId!!, REQUEST_CODE, payload)
        ) {
            purchaseFlowBegan {
                // Bazaar's billing screen has opened successfully
            }
            failedToBeginFlow { t ->
                // Failed to open Bazaar's billing screen
                paymentCallback?.onFailure(t.message, t.stackTrace.joinToString("\n"))
                finish()
            }
        }
    }

    private fun initArgs() {
        productId = intent.extras?.getString(KEY_PRODUCT_ID)
        payload = intent.extras?.getString(KEY_PAYLOAD)
        command = Command.valueOf(requireNotNull(intent.extras?.getString(KEY_COMMAND)))
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        PoolakeyKotlinBridge.payment.onActivityResult(requestCode, resultCode, data) {
            purchaseSucceed { p ->
                // User purchased the product
                paymentCallback?.onSuccess(
                    p.orderId,
                    p.purchaseToken,
                    p.payload,
                    p.packageName,
                    p.purchaseState.ordinal,
                    p.purchaseTime,
                    p.productId,
                    p.originalJson,
                    p.dataSignature
                )
                finish()
            }
            purchaseCanceled {
                // User canceled the purchase
                paymentCallback?.onCancel()
                finish()
            }
            purchaseFailed { t ->
                paymentCallback?.onFailure(t.message, t.stackTrace.joinToString("\n"))
                finish()
            }
        }
    }

    override fun onDestroy() {
        super.onDestroy();
        paymentCallback = null
    }

    companion object {

        private const val REQUEST_CODE: Int = 1000
        private const val KEY_PRODUCT_ID = "productId"
        private const val KEY_PAYLOAD = "payload"
        private const val KEY_COMMAND = "command"
        var paymentCallback: PaymentCallback? = null

        @JvmStatic
        fun start(
            activity: Activity,
            command: Command,
            productId: String,
            callback: PaymentCallback,
            payload: String?
        ) {
            paymentCallback = callback
            val intent = Intent(activity, PaymentActivity::class.java)
            intent.putExtra(KEY_PRODUCT_ID, productId)
            intent.putExtra(KEY_PAYLOAD, payload)
            intent.putExtra(KEY_COMMAND, command.name)
            activity.startActivity(intent)
        }
    }

    enum class Command {
        PurchaseProduct,
        Subscribe
    }
}