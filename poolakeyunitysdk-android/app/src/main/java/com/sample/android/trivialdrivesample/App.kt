package com.sample.android.trivialdrivesample

import android.app.Application
import androidx.lifecycle.MutableLiveData
import com.farsitel.bazaar.PoolakeyKotlinBridge.connect
import com.farsitel.bazaar.PoolakeyKotlinBridge.getSkuDetails
import com.farsitel.bazaar.callback.ConnectionCallback
import com.farsitel.bazaar.callback.SKUDetailsCallback
import ir.cafebazaar.poolakey.entity.SkuDetails

class App : Application() {

    val liveData = MutableLiveData<String>()

    override fun onCreate() {
        super.onCreate()

        connect(applicationContext, null, object : ConnectionCallback {
            override fun onConnect() {
                log("onConnect")
                getSKUDetails()
            }

            override fun onDisconnect() {
                log("onDisconnect")
            }

            override fun onFailure(message: String, stackTrace: String) {
                log("onFailure $message")
            }
        })
    }

    private fun getSKUDetails() {
        getSkuDetails(
            "inapp",
            listOf("gas","premium","trial_subscription","infinite_gas_monthly","dynamic_price"),
            object : SKUDetailsCallback {
                override fun onSuccess(data: List<SkuDetails?>) {
                    log("onSkuDetailsSuccess $data")
                }

                override fun onFailure(message: String, stackTrace: String) {
                    log("onSkuDetailsFailure $message")
                }
            })
    }

    fun log(text: String) {
        liveData.postValue(liveData.value.orEmpty() + "\n" + text)
    }
}