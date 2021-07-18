package com.farsitel.bazaar;

import android.app.Activity;
import android.util.Log;

import com.farsitel.bazaar.callback.ConnectionCallback;
import com.farsitel.bazaar.callback.PaymentCallback;

import java.lang.reflect.Field;

public class PoolakeyBridge {
    private static final String TAG = "PoolakeyBridge";
    private static PoolakeyBridge instance;

    private Class<?> mUnityPlayerClass;
    private Field mUnityPlayerActivityField;

    private PoolakeyBridge() {
        try {
            // Using reflection to remove reference to Unity library.
            mUnityPlayerClass = Class.forName("com.unity3d.player.UnityPlayer");
            mUnityPlayerActivityField = mUnityPlayerClass.getField("currentActivity");
        } catch (ClassNotFoundException e) {
            Log.i(TAG, "Could not find UnityPlayer class: " + e.getMessage());
        } catch (NoSuchFieldException e) {
            Log.i(TAG, "Could not find currentActivity field: " + e.getMessage());
        } catch (Exception e) {
            Log.i(TAG, "Unknown exception occurred locating UnitySendMessage(): " + e.getMessage());
        }
    }

    public static PoolakeyBridge getInstance() {
        if (instance == null) {
            instance = new PoolakeyBridge();
        }
        return instance;
    }

    private Activity getCurrentActivity() {
        if (mUnityPlayerActivityField != null)
            try {
                Activity activity = (Activity) mUnityPlayerActivityField.get(mUnityPlayerClass);
                if (activity == null)
                    Log.e(TAG, "The Unity Activity does not exist. This could be due to a low memory situation");
                return activity;
            } catch (Exception e) {
                Log.i(TAG, "Error getting currentActivity: " + e.getMessage());
            }
        return null;
    }

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
