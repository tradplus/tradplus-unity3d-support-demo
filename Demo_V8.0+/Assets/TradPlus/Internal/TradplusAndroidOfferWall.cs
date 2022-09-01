using System;
using System.Collections.Generic;
using UnityEngine;
using TradPlusInternal.ThirdParty.MiniJSON;

// Android 8101 新增
public class TradPlusAndroidOfferWall
{
    private readonly AndroidJavaObject _OfferWallPlugin;


    public TradPlusAndroidOfferWall(string adUnitId)
    {
        _OfferWallPlugin = new AndroidJavaObject("com.tradplus.ads.unity.OfferWallPlugin", adUnitId);
    }

    // V820支持
    public void SetOfferWallCustomParams(Dictionary<string, object> map)
    {
        _OfferWallPlugin.Call("setCustomParams", Json.Serialize(map));
    }

    // V820支持
    public void SetOfferWallUserId(string userId)
    {
        _OfferWallPlugin.Call("setOfferWallUserId", userId);
    }

    //请求广告
    public void RequestOfferWall()
    {
        _OfferWallPlugin.Call("request");
    }

    //展示广告
    public void ShowOfferWall()
    {
        _OfferWallPlugin.Call("show");
    }

    //展示广告（广告场景ID）
    public void ShowOfferWall(string adSceneId)
    {
        _OfferWallPlugin.Call("show", adSceneId);
    }

    //进入广告位所在界面时调用
    public void OfferWallEntryAdScenario()
    {
        _OfferWallPlugin.Call("entryAdScenario");
    }

    //进入广告位所在界面时调用（广告场景ID）
    public void OfferWallEntryAdScenario(string adSceneId)
    {
        _OfferWallPlugin.Call("entryAdScenario", adSceneId);
    }

    //Check是否有可用广告
    public bool IsOfferWallReady
    {
        get { return _OfferWallPlugin.Call<bool>("isReady"); }
    }


    //查询总额
    public void GetCurrencyBalance()
    {
        _OfferWallPlugin.Call("getCurrencyBalance");
    }

    //消耗积分
    public void SpendCurrency(int count)
    {
        _OfferWallPlugin.Call("spendCurrency",count);
    }

    //增加积分
    public void AwardCurrency(int amount)
    {
        _OfferWallPlugin.Call("awardCurrency", amount);
    }

    //释放资源
    public void OfferWallOnDestroy()
    {
        _OfferWallPlugin.Call("onDestroy");
    }
}
