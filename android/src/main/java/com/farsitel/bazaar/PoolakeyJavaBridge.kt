package com.farsitel.bazaar

import android.content.Context
import com.farsitel.bazaar.callback.ConnectionCallback
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
    }
}