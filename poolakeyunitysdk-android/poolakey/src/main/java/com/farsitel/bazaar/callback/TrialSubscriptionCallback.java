package com.farsitel.bazaar.callback;

public interface TrialSubscriptionCallback {
    void onSuccess(boolean isAvailable, int trialPeriodDays);

    void onFailure(String message, String stackTrace);
}
