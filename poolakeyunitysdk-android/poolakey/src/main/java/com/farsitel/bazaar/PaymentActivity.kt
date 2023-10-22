package com.farsitel.bazaar

import android.content.Context
import android.content.Intent
import android.os.Bundle
import androidx.fragment.app.FragmentActivity
import com.farsitel.bazaar.callback.PaymentCallback
import ir.cafebazaar.poolakey.exception.DynamicPriceNotSupportedException
import ir.cafebazaar.poolakey.request.PurchaseRequest
import java.security.InvalidParameterException

class PaymentActivity : FragmentActivity() {
    private var productId: String? = null
    private var payload: String? = null
    private var dynamicPriceToken: String? = null
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

    private fun initArgs() {
        productId = intent.extras?.getString(KEY_PRODUCT_ID)
        payload = intent.extras?.getString(KEY_PAYLOAD)
        dynamicPriceToken = intent.extras?.getString(KEY_DYNAMIC_PRICE_TOKEN)
        command = Command.valueOf(requireNotNull(intent.extras?.getString(KEY_COMMAND)))
    }

    private fun purchaseProduct() {
        PoolakeyKotlinBridge.payment.purchaseProduct(
            activityResultRegistry,
            PurchaseRequest(productId!!, payload, dynamicPriceToken)
        ) {
            purchaseFlowBegan {
                // Bazaar's billing screen has opened successfully
                paymentCallback?.onStart()
            }
            failedToBeginFlow {
                // Failed to open Bazaar's billing screen
                if (it is DynamicPriceNotSupportedException) {
                    dynamicPriceToken = null
                    purchaseProduct()
                } else {
                    paymentCallback?.onFailure(
                        it.localizedMessage,
                        it.stackTrace.joinToString("\n")
                    )
                }
                finish()
            }
            purchaseSucceed {
                // User purchased the product
                paymentCallback?.onSuccess(
                    it.orderId,
                    it.purchaseToken,
                    it.payload,
                    it.packageName,
                    it.purchaseState.ordinal,
                    it.purchaseTime,
                    it.productId,
                    it.originalJson,
                    it.dataSignature
                )
                finish()
            }
            purchaseCanceled {
                // User canceled the purchase
                paymentCallback?.onCancel()
                finish()

            }
            purchaseFailed {
                paymentCallback?.onFailure(it.localizedMessage, it.stackTrace.joinToString("\n"))
                finish()
            }
        }
    }

    private fun subscribeProduct() {
        PoolakeyKotlinBridge.payment.subscribeProduct(
            activityResultRegistry,
            PurchaseRequest(productId!!, payload, dynamicPriceToken)
        ) {
            purchaseFlowBegan {
                // Bazaar's billing screen has opened successfully
                paymentCallback?.onStart()
            }
            failedToBeginFlow { throwable ->
                // Failed to open Bazaar's billing screen
                if (throwable is DynamicPriceNotSupportedException) {
                    dynamicPriceToken = null
                    subscribeProduct()
                } else {
                    paymentCallback?.onFailure(
                        throwable.message,
                        throwable.stackTrace.joinToString("\n")
                    )
                    finish()
                }
            }
            purchaseSucceed {
                // User purchased the product
                paymentCallback?.onSuccess(
                    it.orderId,
                    it.purchaseToken,
                    it.payload,
                    it.packageName,
                    it.purchaseState.ordinal,
                    it.purchaseTime,
                    it.productId,
                    it.originalJson,
                    it.dataSignature
                )
                finish()
            }
            purchaseCanceled {
                // User canceled the purchase
                paymentCallback?.onCancel()
                finish()

            }
            purchaseFailed {
                paymentCallback?.onFailure(it.localizedMessage, it.stackTrace.joinToString("\n"))
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
        private const val KEY_DYNAMIC_PRICE_TOKEN = "dynamicPriceToken"
        private const val KEY_COMMAND = "command"
        var paymentCallback: PaymentCallback? = null

        @JvmStatic
        fun start(
            activity: Context,
            command: Command,
            productId: String,
            callback: PaymentCallback,
            payload: String?,
            dynamicPriceToken: String?
        ) {
            paymentCallback = callback
            val intent = Intent(activity, PaymentActivity::class.java)
            intent.putExtra(KEY_PRODUCT_ID, productId)
            intent.putExtra(KEY_PAYLOAD, payload)
            intent.putExtra(KEY_DYNAMIC_PRICE_TOKEN, dynamicPriceToken)
            intent.putExtra(KEY_COMMAND, command.name)
            activity.startActivity(intent)
        }
    }

    enum class Command {
        PurchaseProduct,
        Subscribe
    }
}