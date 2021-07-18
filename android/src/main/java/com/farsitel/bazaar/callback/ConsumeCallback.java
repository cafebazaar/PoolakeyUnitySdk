package com.farsitel.bazaar.callback;

import org.jetbrains.annotations.NotNull;

public interface ConsumeCallback {
    default void onSuccess() {
    }

    default void onFailure(@NotNull Throwable throwable) {
    }
}
