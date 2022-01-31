package com.farsitel.bazaar;

import android.app.Activity;
import android.util.Log;

import androidx.appcompat.app.AppCompatActivity;

import com.farsitel.bazaar.callback.ConnectionCallback;
import com.farsitel.bazaar.callback.ConsumeCallback;
import com.farsitel.bazaar.callback.PaymentCallback;
import com.farsitel.bazaar.callback.PurchasesCallback;
import com.farsitel.bazaar.callback.SKUDetailsCallback;

import java.lang.reflect.Field;
import java.util.Arrays;

public class PoolakeyBridge {
    public static final String TAG = "Poolakey";
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

    public String getVersion() {
        return BuildConfig.POOLAKEY_VERSION;
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

    public void getPurchases(String type, PurchasesCallback callback) {
        PoolakeyKotlinBridge.INSTANCE.getPurchases(type, callback);
    }

    public void purchase(String type, String productId, String payload, String dynamicPriceToken, PaymentCallback callback) {
        PoolakeyKotlinBridge.INSTANCE.purchase((AppCompatActivity) getCurrentActivity(), type, productId, payload, dynamicPriceToken, callback);
    }

    public void consume(String token, ConsumeCallback callback) {
        PoolakeyKotlinBridge.INSTANCE.consume(token, callback);
    }
}
