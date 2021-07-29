package com.farsitel.bazaar;

import android.app.Activity;
import android.util.Log;

import com.farsitel.bazaar.callback.ConnectionCallback;
import com.farsitel.bazaar.callback.ConsumeCallback;
import com.farsitel.bazaar.callback.OwnedProductsCallback;
import com.farsitel.bazaar.callback.PaymentCallback;
import com.farsitel.bazaar.callback.SKUDetailsCallback;

import java.lang.reflect.Field;
import java.util.Arrays;

public class PoolakeyBridge {
    public static final String TAG = "PoolakeyBridge";
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
        PoolakeyKotlinBridge.INSTANCE.connect(getCurrentActivity(), rsaPublicKey, callback);
    }

    public void disconnect() {
        PoolakeyKotlinBridge.INSTANCE.disconnect();
    }

    public void getSkuDetails(String type, String productIds, SKUDetailsCallback callback) {
        PoolakeyKotlinBridge.INSTANCE.getSkuDetails(type, Arrays.asList(productIds.split(",")), callback);
    }

    public void getOwnedProducts(String type, OwnedProductsCallback callback) {
        PoolakeyKotlinBridge.INSTANCE.getOwnedProducts(type, callback);
    }

    public void purchase(String type, String productId, String payload, String dynamicPriceToken, PaymentCallback callback) {
        PaymentActivity.Command cmd = PaymentActivity.Command.PurchaseProduct;
        if (!type.equalsIgnoreCase("inApp"))
            cmd = PaymentActivity.Command.Subscribe;
        PoolakeyKotlinBridge.INSTANCE.startActivity(getCurrentActivity(), cmd, callback, productId, payload, dynamicPriceToken);
    }

    public void consume(String token, ConsumeCallback callback) {
        PoolakeyKotlinBridge.INSTANCE.consume(token, callback);
    }
}
