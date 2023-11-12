package com.sample.android.trivialdrivesample;

import androidx.appcompat.app.AppCompatActivity;
import androidx.lifecycle.MutableLiveData;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.TextView;

import com.farsitel.bazaar.PaymentActivity;
import com.farsitel.bazaar.PoolakeyBridge;
import com.farsitel.bazaar.PoolakeyKotlinBridge;
import com.farsitel.bazaar.callback.ConsumeCallback;
import com.farsitel.bazaar.callback.PaymentCallback;

public class MainActivity extends AppCompatActivity {
    private TextView textView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        textView = findViewById(R.id.textView);
        MutableLiveData<String> logLiveData = getApp().getLiveData();
        logLiveData.observe(this, this::log);
    }

    public void purchase(View view) {
        PoolakeyKotlinBridge.INSTANCE.startActivity(this, PaymentActivity.Command.PurchaseProduct, new PaymentCallback() {
            @Override
            public void onStart() {
                getApp().log("onPurchaseStart");
            }

            @Override
            public void onCancel() {
                getApp().log("onPurchaseCancel");
            }

            @Override
            public void onSuccess(String orderId, String purchaseToken, String payload, String packageName, int purchaseState, long purchaseTime, String productId, String originalJson, String dataSignature) {
                getApp().log("onPurchaseSuccess " + orderId);
                consume(purchaseToken);
            }

            @Override
            public void onFailure(String message, String stackTrace) {
                getApp().log("onPurchaseFailure " + message);
            }
        }, "PaymentTest1000", "", "");
    }

    public void consume(String purchaseToken) {
        PoolakeyKotlinBridge.INSTANCE.consume(purchaseToken, new ConsumeCallback() {
            @Override
            public void onSuccess() {
                getApp().log("onConsumeSuccess");
            }

            @Override
            public void onFailure(String message, String stackTrace) {
                getApp().log("onConsumeFailure " + message);

            }
        });
    }

    void log(String message) {
        Log.i(PoolakeyBridge.TAG, message);
        textView.setText(message);
    }

    private App getApp() {
        return (App) getApplication();
    }
}