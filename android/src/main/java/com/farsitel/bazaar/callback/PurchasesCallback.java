package com.farsitel.bazaar.callback;

import java.util.List;

import ir.cafebazaar.poolakey.entity.PurchaseInfo;
import ir.cafebazaar.poolakey.entity.SkuDetails;

public interface PurchasesCallback {
    void onSuccess(List<PurchaseInfo> data);
    void onFailure(String message, String stackTrace);
}
