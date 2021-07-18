package com.farsitel.bazaar

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import androidx.fragment.app.FragmentActivity
import ir.cafebazaar.poolakey.Connection
import ir.cafebazaar.poolakey.Payment
import ir.cafebazaar.poolakey.config.PaymentConfiguration
import ir.cafebazaar.poolakey.config.SecurityCheck
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
        PoolakeyJavaBridge.payment.purchaseProduct(
                this@PaymentActivity,
                PurchaseRequest(productId!!, REQUEST_CODE, payload)
        ) {
            purchaseFlowBegan {
                // Bazaar's billing screen has opened successfully
            }
            failedToBeginFlow { throwable ->
                // Failed to open Bazaar's billing screen
                CallbackHolder.paymentCallback?.onFailure(throwable)
                finish()
            }
        }
    }

    private fun subscribeProduct() {
        PoolakeyJavaBridge.payment.subscribeProduct(
                this@PaymentActivity,
                PurchaseRequest(productId!!, REQUEST_CODE, payload)
        ) {
            purchaseFlowBegan {
                // Bazaar's billing screen has opened successfully
            }
            failedToBeginFlow { throwable ->
                // Failed to open Bazaar's billing screen
                CallbackHolder.paymentCallback?.onFailure(throwable)
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
        PoolakeyJavaBridge.payment.onActivityResult(requestCode, resultCode, data) {
            purchaseSucceed { purchaseEntity ->
                // User purchased the product
                CallbackHolder.paymentCallback?.onSuccess(purchaseEntity)
                finish()
            }
            purchaseCanceled {
                // User canceled the purchase
                CallbackHolder.paymentCallback?.onCancel()
                finish()
            }
            purchaseFailed { throwable ->
                CallbackHolder.paymentCallback?.onFailure(throwable)
                finish()
            }
        }
    }

    companion object {

        private const val REQUEST_CODE: Int = 1000
        private const val KEY_PRODUCT_ID = "productId"
        private const val KEY_PAYLOAD = "payload"
        private const val KEY_COMMAND = "command"

        @JvmStatic
        fun start(
                activity: Activity,
                command: Command,
                productId: String,
                payload: String?
        ) {
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