package com.farsitel.bazaar.callback;

public interface ConnectionCallback {

    void onConnect();

    void onDisconnect();

    void onFailure();
}