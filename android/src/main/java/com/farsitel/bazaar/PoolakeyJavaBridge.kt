package com.farsitel.bazaar

import android.content.Context
import com.farsitel.bazaar.callback.ConnectionCallback
import ir.cafebazaar.poolakey.Payment
import ir.cafebazaar.poolakey.config.PaymentConfiguration
import ir.cafebazaar.poolakey.config.SecurityCheck

object PoolakeyJavaBridge {
    fun connect(context: Context, rsaPublicKey: String?, callback: ConnectionCallback): Payment {
        val securityCheck = if (rsaPublicKey != null) {
            SecurityCheck.Enable(rsaPublicKey)
        } else {
            SecurityCheck.Disable
        }
        val paymentConfig = PaymentConfiguration(localSecurityCheck = securityCheck)
        val payment = Payment(context = context, config = paymentConfig)
        payment.connect {
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
        return payment
    }
}