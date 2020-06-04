using UnityEngine;

public class TradPlusAndroidOfferWall
{
    private readonly AndroidJavaObject _OfferWallPlugin;


    public TradPlusAndroidOfferWall(string adUnitId)
    {
        _OfferWallPlugin = new AndroidJavaObject("com.tradplus.ads.unity.OfferWallPlugin", adUnitId);
    }


    public void RequestOfferWall()
    {
        _OfferWallPlugin.Call("request");
    }


    public void ShowOfferWall()
    {
        _OfferWallPlugin.Call("show");
    }

    public void ShowOfferWallConfirmUWSAd()
    {
        _OfferWallPlugin.Call("confirmUWSAd");
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
