package com.farsitel.bazaar.callback;

import com.farsitel.bazaar.CallbackHolder;

import org.jetbrains.annotations.NotNull;

import ir.cafebazaar.poolakey.entity.PurchaseInfo;

public interface PaymentCallback {

    default void onSuccess(PurchaseInfo purchaseEntity) {
        CallbackHolder.INSTANCE.setPaymentCallback(null);
    }

    default void onCancel() {
        CallbackHolder.INSTANCE.setPaymentCallback(null);
    }

    default void onFailure(@NotNull Throwable throwable) {
        CallbackHolder.INSTANCE.setPaymentCallback(null);
    }
}
