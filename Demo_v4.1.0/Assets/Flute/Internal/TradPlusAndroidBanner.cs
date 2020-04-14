using System.Diagnostics.CodeAnalysis;
using UnityEngine;


public class TradPlusAndroidBanner
{
    private readonly AndroidJavaObject _bannerPlugin;


    public TradPlusAndroidBanner(string adUnitId)
    {
        _bannerPlugin = new AndroidJavaObject("com.tradplus.ads.unity.BannerUnityPlugin", adUnitId);
    }


    [SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
    public void CreateBanner(TradPlus.AdPosition position)
    {
        _bannerPlugin.Call("createBanner", (int)position);
    }


    public void ShowBanner(bool shouldShow)
    {
        _bannerPlugin.Call("hideBanner", !shouldShow);
    }


    public void RefreshBanner(string keywords, string userDataKeywords = "")
    {
        _bannerPlugin.Call("refreshBanner", keywords, userDataKeywords);
    }


    public void DestroyBanner()
    {
        _bannerPlugin.Call("destroyBanner");
    }


    public void SetAutorefresh(bool enabled)
    {
        _bannerPlugin.Call("setAutorefreshEnabled", enabled);
    }


    public void ForceRefresh()
    {
        _bannerPlugin.Call("forceRefresh");
    }
}
