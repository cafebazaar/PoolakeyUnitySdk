package com.farsitel.bazaar

import android.app.Activity
import android.content.Context
import com.farsitel.bazaar.callback.SKUDetailsCallback
import com.farsitel.bazaar.callback.ConnectionCallback
import com.farsitel.bazaar.callback.ConsumeCallback
import com.farsitel.bazaar.callback.PaymentCallback
import ir.cafebazaar.poolakey.Connection
import ir.cafebazaar.poolakey.ConnectionState
import ir.cafebazaar.poolakey.Payment
import ir.cafebazaar.poolakey.config.PaymentConfiguration
import ir.cafebazaar.poolakey.config.SecurityCheck

object PoolakeyJavaBridge {
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
            connectionFailed {
                callback.onFailure()
            }
            connectionSucceed {
                callback.onConnect()
            }
            disconnected {
                callback.onDisconnect()
            }
        }
    }

    fun startActivity(
        activity: Activity,
        command: PaymentActivity.Command,
        paymentCallback: PaymentCallback,
        productId: String,
        payload: String
    ) {
        CallbackHolder.paymentCallback = paymentCallback;
        if (connection.getState() != ConnectionState.Connected) {
//            paymentCallback.onFailure(throw Exception(message :"") )
            return
        }
        PaymentActivity.start(
            activity,
            command,
            productId,
            payload
        )
    }

    fun getSubscriptionSkuDetails(productId: String, callback: SKUDetailsCallback) {
        if (connection.getState() != ConnectionState.Connected) {
            return
        }
        payment.getSubscriptionSkuDetails(skuIds = listOf(productId)) {
            getSkuDetailsSucceed(callback::onSuccess)
            getSkuDetailsFailed(callback::onFailure)
        }
    }

    fun getPurchaseSkuDetails(productId: String, callback: SKUDetailsCallback) {
        if (connection.getState() != ConnectionState.Connected) {
            return
        }
        payment.getInAppSkuDetails(skuIds = listOf(productId)) {
            getSkuDetailsSucceed(callback::onSuccess)
            getSkuDetailsFailed(callback::onFailure)
        }
    }

    fun consume(purchaseToken: String, callback: ConsumeCallback) {
        if (connection.getState() != ConnectionState.Connected) {
            return
        }
        payment.consumeProduct(purchaseToken) {
            consumeSucceed(callback::onSuccess)
            consumeFailed (callback::onFailure)
        }
    }
}