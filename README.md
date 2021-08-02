<img src="https://github.com/manjav/PoolakeyUnitySdk/blob/main/images/Poolakey-unity.jpg?raw=true"/><br/>

[........... راهنمــــــای فـــــــارسی ..........](https://github.com/manjav/PoolakeyUnitySdk/blob/main/README_FA.md)


### Unity SDK for [Poolakey](https://github.com/cafebazaar/Poolakey) ( New Cafebazaar IAP ).<br/><br/>

Easy payment :

```c#
var connectionResult = await payment.Connect();
var purchaseResult = await payment.Purchase("productID");
var consumeResult = await payment.Consume(purchaseResult.data.purchaseToken);
```
<br/><br/>
## How to use?
For more information regarding the usage of Poolakey, please check out the [wiki](https://github.com/manjav/PoolakeyUnitySdk/wiki) page.