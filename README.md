<img src="https://github.com/cafebazaar/PoolakeyUnitySdk/blob/main/images/Poolakey-unity.jpg?raw=true"/><br/>

[........... راهنمــــــای فـــــــارسی ..........](https://github.com/cafebazaar/PoolakeyUnitySdk/blob/main/README_FA.md)


### Unity SDK for [Poolakey](https://github.com/cafebazaar/Poolakey) ( New Cafebazaar IAP ).<br/><br/>

Easy payment :

```c#
var connectionResult = await payment.Connect();
var purchaseResult = await payment.Purchase("productID");
var consumeResult = await payment.Consume(purchaseResult.data.purchaseToken);
```
><b>Attention!</b>
>
> Keep in mind, Poolay only works on Unity 2020 and above.

<br/>
## How to use?
Import the unity packge provided and you are good to go. You can also check the sample provided with one of the packages and see the wiki pages to get familiarised with the methods and procedurs of SDK.
For more information regarding the usage of Poolakey, please check out the [wiki](https://github.com/cafebazaar/PoolakeyUnitySdk/wiki) page.
