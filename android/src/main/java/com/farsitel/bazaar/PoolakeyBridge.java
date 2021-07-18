package com.farsitel.bazaar;

import android.app.Activity;

import com.farsitel.bazaar.callback.ConnectionCallback;
import com.farsitel.bazaar.callback.PaymentCallback;

import ir.cafebazaar.poolakey.Payment;

public class PoolakeyBridge {

    public static Payment connect(
            Activity activity,
            String rsaPublicKey,
            ConnectionCallback callback
    ) {
        return PoolakeyJavaBridge.INSTANCE.connect(activity, rsaPublicKey, callback);
    }

    public static void purchaseProduct(
            Activity activity,
            String rsaPublicKey,
            String productId,
            String payload,
            PaymentCallback paymentCallback
    ) {
        CallbackHolder.INSTANCE.setPaymentCallback(paymentCallback);
        PaymentActivity.start(
                activity,
                PaymentActivity.Command.PurchaseProduct,
                rsaPublicKey,
                productId,
                payload
        );
    }

    public static void subscribeProduct(
            Activity activity,
            String rsaPublicKey,
            String productId,
            String payload,
            PaymentCallback paymentCallback
    ) {
        CallbackHolder.INSTANCE.setPaymentCallback(paymentCallback);
        PaymentActivity.start(
                activity,
                PaymentActivity.Command.Subscribe,
                rsaPublicKey,
                productId,
                payload
        );
    }
}
