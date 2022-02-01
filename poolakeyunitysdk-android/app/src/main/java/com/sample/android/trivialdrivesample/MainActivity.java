package com.sample.android.trivialdrivesample;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.TextView;

import com.farsitel.bazaar.PaymentActivity;
import com.farsitel.bazaar.PoolakeyBridge;
import com.farsitel.bazaar.PoolakeyKotlinBridge;
import com.farsitel.bazaar.callback.ConnectionCallback;
import com.farsitel.bazaar.callback.ConsumeCallback;
import com.farsitel.bazaar.callback.PaymentCallback;
import com.farsitel.bazaar.callback.PurchasesCallback;
import com.farsitel.bazaar.callback.SKUDetailsCallback;

import java.util.Arrays;
import java.util.List;

import ir.cafebazaar.poolakey.entity.SkuDetails;

public class MainActivity extends AppCompatActivity {
    private TextView textView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        textView = findViewById(R.id.textView);

        PoolakeyKotlinBridge.INSTANCE.connect(getApplicationContext(), null, new ConnectionCallback() {
            @Override
            public void onConnect() {
                log("onConnect");
                getSKUDetails();
            }

            @Override
            public void onDisconnect() {
                log("onDisconnect");
            }

            @Override
            public void onFailure(String message, String stackTrace) {
                log("onFailure " + message);
            }
        });
    }

    private void getSKUDetails() {
        PoolakeyKotlinBridge.INSTANCE.getSkuDetails("inapp", Arrays.asList("gas,premium".split(",")), new SKUDetailsCallback() {
            @Override
            public void onSuccess(List<SkuDetails> data) {
                log("onSkuDetailsSuccess " + data.toString());
            }

            @Override
            public void onFailure(String message, String stackTrace) {
                log("onSkuDetailsFailure " + message);
            }
        });
    }

    public void purchase(View view) {
        PoolakeyKotlinBridge.INSTANCE.startActivity(this, PaymentActivity.Command.PurchaseProduct, new PaymentCallback() {
            @Override
            public void onStart() {
                log("onPurchaseStart");
            }

            @Override
            public void onCancel() {
                log("onPurchaseCancel");
            }

            @Override
            public void onSuccess(String orderId, String purchaseToken, String payload, String packageName, int purchaseState, long purchaseTime, String productId, String originalJson, String dataSignature) {
                log("onPurchaseSuccess " + orderId);
                consume(purchaseToken);
            }

            @Override
            public void onFailure(String message, String stackTrace) {
                log("onPurchaseFailure " + message);
            }
        }, "gas", "", "");
    }

    public void consume(String purchaseToken) {
        PoolakeyKotlinBridge.INSTANCE.consume(purchaseToken, new ConsumeCallback() {
            @Override
            public void onSuccess() {
                log("onConsumeSuccess");
            }

            @Override
            public void onFailure(String message, String stackTrace) {
                log("onConsumeFailure " + message);

            }
        });
    }

    void log(String message) {
        Log.i(PoolakeyBridge.TAG, message);
        textView.append(message + "\n");
    }
}