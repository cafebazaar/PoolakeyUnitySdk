package com.farsitel.bazaar.callback;

import org.jetbrains.annotations.NotNull;

import ir.cafebazaar.poolakey.entity.PurchaseInfo;
import ir.cafebazaar.poolakey.entity.PurchaseState;

public interface PaymentCallback {

    void onSuccess(String orderId, String purchaseToken, String payload, String packageName, int purchaseState, long purchaseTime, String productId, String originalJson, String dataSignature);
    void onFailure(String message, String stackTrace);
    void onCancel();
}
