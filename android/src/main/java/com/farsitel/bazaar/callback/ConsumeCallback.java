package com.farsitel.bazaar.callback;

import org.jetbrains.annotations.NotNull;

public interface ConsumeCallback {
    void onSuccess();
    void onFailure(String message, String stackTrace);
}
