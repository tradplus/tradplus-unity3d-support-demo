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
        _OfferWallPlugin.Call("confirmUWSAd");
    }

    //进入广告位所在界面时调用（广告场景ID）
    public void ShowOfferWallConfirmUWSAd(string adSceneId)
    {
        _OfferWallPlugin.Call("confirmUWSAd", adSceneId);
    }

    public bool IsOfferWallReady
    {
        get { return _OfferWallPlugin.Call<bool>("isReady"); }
    }


    public void DestroyOfferWall()
    {
        _OfferWallPlugin.Call("destroy");
    }

}
