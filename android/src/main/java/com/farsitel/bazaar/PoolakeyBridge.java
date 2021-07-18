package com.farsitel.bazaar;

import android.app.Activity;
import android.util.Log;

import com.farsitel.bazaar.callback.ConsumeCallback;
import com.farsitel.bazaar.callback.SKUDetailsCallback;
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

    public void connect(String rsaPublicKey, ConnectionCallback callback) {
        PoolakeyJavaBridge.INSTANCE.connect(getCurrentActivity(), rsaPublicKey, callback);
    }

    public void getPurchaseSkuDetails(String productId, SKUDetailsCallback callback) {
        PoolakeyJavaBridge.INSTANCE.getPurchaseSkuDetails(productId, callback);
    }
    public void getSubscriptionSkuDetails(String productId, SKUDetailsCallback callback) {
        PoolakeyJavaBridge.INSTANCE.getSubscriptionSkuDetails(productId, callback);
    }

    public void purchase(String productId, String payload, PaymentCallback callback) {
        PoolakeyJavaBridge.INSTANCE.startActivity(getCurrentActivity(), PaymentActivity.Command.PurchaseProduct, callback, productId, payload);
    }
    public void subscribe(String productId, String payload, PaymentCallback callback) {
        PoolakeyJavaBridge.INSTANCE.startActivity(getCurrentActivity(), PaymentActivity.Command.Subscribe, callback, productId, payload);
    }

    public void consume(String token, ConsumeCallback callback) {
        PoolakeyJavaBridge.INSTANCE.consume(token, callback);
    }
}
