package com.farsitel.bazaar.callback;

import java.util.List;

import ir.cafebazaar.poolakey.entity.SkuDetails;

public interface SKUDetailsCallback {
    void onSuccess(List<SkuDetails> data);
    void onFailure(String message, String stackTrace);
}
