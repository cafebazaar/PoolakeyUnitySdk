package com.farsitel.bazaar

import android.app.Activity
import android.content.Context
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AppCompatActivity
import com.farsitel.bazaar.callback.*
import ir.cafebazaar.poolakey.Connection
import ir.cafebazaar.poolakey.ConnectionState
import ir.cafebazaar.poolakey.Payment
import ir.cafebazaar.poolakey.config.PaymentConfiguration
import ir.cafebazaar.poolakey.config.SecurityCheck
import ir.cafebazaar.poolakey.entity.PurchaseInfo
import ir.cafebazaar.poolakey.entity.SkuDetails
import ir.cafebazaar.poolakey.request.PurchaseRequest

object PoolakeyKotlinBridge {
    lateinit var payment: Payment
    lateinit var connection: Connection

    fun connect(context: Context, rsaPublicKey: String?, callback: ConnectionCallback) {
        val securityCheck = if (rsaPublicKey != null) {
            SecurityCheck.Enable(rsaPublicKey)
        } else {
            SecurityCheck.Disable
        }
        val paymentConfig = PaymentConfiguration(localSecurityCheck = securityCheck)
        payment = Payment(context = context, config = paymentConfig)
        connection = payment.connect {
            connectionFailed { throwable ->
                callback.onFailure(
                    throwable.message,
                    throwable.stackTrace.joinToString { "\n" })
            }
            connectionSucceed {
                callback.onConnect()
            }
            disconnected {
                callback.onDisconnect()
            }
        }
    }

    fun disconnect() {
        connection.disconnect();
    }


    fun getSkuDetails(type: String, productIds: List<String>, callback: SKUDetailsCallback) {
        if (connection.getState() != ConnectionState.Connected) {
            callback.onFailure(
                "Connection not found.",
                "In order to getting ske details, connect to Poolakey!"
            )
            return
        }
        when (type) {
//            "all" -> {
//                val skuDetailsList: MutableList<SkuDetails> = mutableListOf()
//                payment.getInAppSkuDetails(productIds) {
//                    getSkuDetailsSucceed { list ->
//                        skuDetailsList.addAll(list)
//                        payment.getSubscriptionSkuDetails(productIds) {
//                            getSkuDetailsSucceed { list ->
//                                skuDetailsList.addAll(list)
//                                callback.onSuccess(skuDetailsList)
//                            }
//                            getSkuDetailsFailed { throwable ->
//                                callback.onFailure(
//                                    throwable.message,
//                                    throwable.stackTrace.joinToString { "\n" })
//                            }
//
//                        }
//                        getSkuDetailsFailed { throwable ->
//                            callback.onFailure(
//                                throwable.message,
//                                throwable.stackTrace.joinToString { "\n" })
//
//                        }
//                    }
//                }
//            }
            "inApp", "all" ->
                payment.getInAppSkuDetails(skuIds = productIds) {
                    getSkuDetailsSucceed(callback::onSuccess)
                    getSkuDetailsFailed { throwable ->
                        callback.onFailure(
                            throwable.message,
                            throwable.stackTrace.joinToString { "\n" })
                    }
                }
            else ->
                payment.getSubscriptionSkuDetails(skuIds = productIds) {
                    getSkuDetailsSucceed(callback::onSuccess)
                    getSkuDetailsFailed { throwable ->
                        callback.onFailure(
                            throwable.message,
                            throwable.stackTrace.joinToString { "\n" })
                    }
                }
        }
    }

    fun getPurchases(type: String, callback: PurchasesCallback) {
        if (connection.getState() != ConnectionState.Connected) {
            callback.onFailure(
                "Connection not found.",
                "In order to getting purchases, connect to Poolakey!"
            )
            return
        }
        when (type) {
            "all" -> {
                val purchasesList: MutableList<PurchaseInfo> = mutableListOf()
                payment.getPurchasedProducts {
                    querySucceed { list ->
                        purchasesList.addAll(list)
                        payment.getSubscribedProducts {
                            querySucceed { list ->
                                purchasesList.addAll(list)
                                callback.onSuccess(purchasesList)
                            }
                            queryFailed { throwable ->
                                callback.onFailure(
                                    throwable.message,
                                    throwable.stackTrace.joinToString { "\n" })
                            }
                        }
                        queryFailed { throwable ->
                            callback.onFailure(
                                throwable.message,
                                throwable.stackTrace.joinToString { "\n" })
                        }
                    }
                }
            }
            "inApp" -> payment.getPurchasedProducts {
                querySucceed(callback::onSuccess)
                queryFailed { throwable ->
                    callback.onFailure(
                        throwable.message,
                        throwable.stackTrace.joinToString { "\n" })
                }
            }
            else ->
                payment.getSubscribedProducts {
                    querySucceed(callback::onSuccess)
                    queryFailed { throwable ->
                        callback.onFailure(
                            throwable.message,
                            throwable.stackTrace.joinToString { "\n" })
                    }
                }
        }
    }

    fun purchase(
        activity: AppCompatActivity,
        type: String,
        productId: String,
        payload: String,
        dynamicPriceToken: String,
        callback: PaymentCallback
    ) {
        payment.purchaseProduct(
            registry = activity.activityResultRegistry,
            request = PurchaseRequest(productId!!, payload, dynamicPriceToken)
        ) {
            purchaseFlowBegan {
                callback.onStart();
            }
            failedToBeginFlow {
                // bazaar need to update, in this case we only launch purchase without discount
                callback.onFailure(it.localizedMessage, it.stackTrace.joinToString("\n"));
            }
            purchaseSucceed {
                callback.onSuccess(
                    it.orderId,
                    it.purchaseToken,
                    it.payload,
                    it.packageName,
                    it.purchaseState.ordinal,
                    it.purchaseTime,
                    it.productId,
                    it.originalJson,
                    it.dataSignature
                );
            }
            purchaseCanceled {
                callback.onCancel();
            }
            purchaseFailed {
                callback.onFailure(it.localizedMessage, it.stackTrace.joinToString("\n"))
            }

        }

    }

    fun consume(purchaseToken: String, callback: ConsumeCallback) {
        if (connection.getState() != ConnectionState.Connected) {
            callback.onFailure(
                "Connection not found.",
                "In order to consumption, connect to Poolakey!"
            )
            return
        }
        payment.consumeProduct(purchaseToken) {
            consumeSucceed(callback::onSuccess)
            consumeFailed { throwable ->
                callback.onFailure(
                    throwable.message,
                    throwable.stackTrace.joinToString { "\n" })
            }
        }
    }
}