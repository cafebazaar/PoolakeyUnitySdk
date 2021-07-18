package com.farsitel.bazaar.callback;

import org.jetbrains.annotations.NotNull;

import java.util.List;

import ir.cafebazaar.poolakey.entity.SkuDetails;

public interface SKUDetailsCallback {

    default void onSuccess(List<SkuDetails> it) {
    }

    default void onFailure(@NotNull Throwable throwable) {
    }
}
