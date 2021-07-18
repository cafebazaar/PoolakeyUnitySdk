package com.farsitel.bazaar

import android.app.Activity
import android.content.Context
import com.farsitel.bazaar.callback.ConnectionCallback
import com.farsitel.bazaar.callback.ConsumeCallback
import com.farsitel.bazaar.callback.PaymentCallback
import com.farsitel.bazaar.callback.SKUDetailsCallback
import ir.cafebazaar.poolakey.Connection
import ir.cafebazaar.poolakey.ConnectionState
import ir.cafebazaar.poolakey.Payment
import ir.cafebazaar.poolakey.config.PaymentConfiguration
import ir.cafebazaar.poolakey.config.SecurityCheck

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
            connectionFailed{ t -> callback.onFailure(t.message, t.stackTrace.joinToString { "\n" }) }
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


    fun getSkuDetails(type: String, productId: String, callback: SKUDetailsCallback) {
        if (connection.getState() != ConnectionState.Connected) {
            callback.onFailure("Connection not found.", "In order to getting ske details, connect to Poolakey!")
            return
        }
        when (type) {
            "inApp" ->
                payment.getInAppSkuDetails(skuIds = listOf(productId)) {
                    getSkuDetailsSucceed(callback::onSuccess)
                    getSkuDetailsFailed{ t -> callback.onFailure(t.message, t.stackTrace.joinToString { "\n" }) }
                }
            else ->
                payment.getSubscriptionSkuDetails(skuIds = listOf(productId)) {
                    getSkuDetailsSucceed(callback::onSuccess)
                    getSkuDetailsFailed{ t -> callback.onFailure(t.message, t.stackTrace.joinToString { "\n" }) }
                }
        }
    }

    fun startActivity(
        activity: Activity,
        command: PaymentActivity.Command,
        callback: PaymentCallback,
        productId: String,
        payload: String
    ) {
        if (connection.getState() != ConnectionState.Connected) {
            callback.onFailure("Connection not found.", "In order to purchasing, connect to Poolakey!")
            return
        }
        PaymentActivity.start(
            activity,
            command,
            productId,
            callback,
            payload
        )
    }

    fun consume(purchaseToken: String, callback: ConsumeCallback) {
        if (connection.getState() != ConnectionState.Connected) {
            callback.onFailure("Connection not found.", "In order to consumption, connect to Poolakey!")
            return
        }
        payment.consumeProduct(purchaseToken) {
            consumeSucceed(callback::onSuccess)
            consumeFailed { t -> callback.onFailure(t.message, t.stackTrace.joinToString { "\n" }) }
        }
    }
}