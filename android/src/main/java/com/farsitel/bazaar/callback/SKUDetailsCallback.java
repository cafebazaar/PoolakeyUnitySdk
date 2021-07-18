package com.farsitel.bazaar.callback;

import org.jetbrains.annotations.NotNull;

import java.util.List;

import ir.cafebazaar.poolakey.entity.SkuDetails;

public interface SKUDetailsCallback {
    void onSuccess(List<SkuDetails> it);
    void onFailure(String message, String stackTrace);
}
