package com.farsitel.bazaar.callback;

public interface ConsumeCallback {
    void onSuccess();
    void onFailure(String message, String stackTrace);
}
