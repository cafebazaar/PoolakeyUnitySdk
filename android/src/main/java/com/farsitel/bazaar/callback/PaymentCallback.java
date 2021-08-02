package com.farsitel.bazaar.callback;

public interface PaymentCallback {
    void onStart();
    void onCancel();
    void onSuccess(String orderId, String purchaseToken, String payload, String packageName, int purchaseState, long purchaseTime, String productId, String originalJson, String dataSignature);
    void onFailure(String message, String stackTrace);
}
