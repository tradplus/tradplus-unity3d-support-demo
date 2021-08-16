using UnityEngine;

public class TradPlusAndroidOfferWall
{
    private readonly AndroidJavaObject _OfferWallPlugin;


    public TradPlusAndroidOfferWall(string adUnitId)
    {
        _OfferWallPlugin = new AndroidJavaObject("com.tradplus.ads.unity.OfferWallPlugin", adUnitId);
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
    public void ShowOfferWallConfirmUWSAd()
    {
        _OfferWallPlugin.Call("entryAdScenario");
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

    //Check广告缓存数是否已达配置上限
    public bool IsOfferWallAllReady
    {
        get { return _OfferWallPlugin.Call<bool>("isAllReady"); }
    }

}
