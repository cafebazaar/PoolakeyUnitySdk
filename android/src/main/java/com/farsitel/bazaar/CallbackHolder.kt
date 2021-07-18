package com.farsitel.bazaar

import com.farsitel.bazaar.callback.ConsumeCallback
import com.farsitel.bazaar.callback.PaymentCallback

object CallbackHolder {
    var paymentCallback: PaymentCallback? = null
    var consumeCallback: ConsumeCallback? = null
}