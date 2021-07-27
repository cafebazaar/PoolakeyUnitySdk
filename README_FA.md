<img src="https://github.com/manjav/PoolakeyUnitySdk/blob/main/images/Poolakey-unity.jpg?raw=true"/><br/>

[........... راهنمــــــای فـــــــارسی ..........](https://github.com/manjav/PoolakeyUnitySdk/blob/main/README_FA.md)


### [کتابخانه پولکی](https://github.com/cafebazaar/Poolakey) ( سیستم جدید و ساده پرداخت درون‌برنامه کافه بازار ).<br/><br/>

پیاده سازی بسیار آسان :

```c#
var connectionResult = await payment.Connect();
var purchaseResult = await payment.Purchase("productID");
var consumeResult = await payment.Consume(purchaseResult.data.purchaseToken);
```
<br/><br/>
## چطوری شروع کنم؟
برای اینکه اطلاعات بیشتری از نحوه استفاده کتابخانه به دست بیاری به [ویکی](https://github.com/manjav/PoolakeyUnitySdk/wiki) مراجعه کن.