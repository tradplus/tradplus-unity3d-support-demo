using System.Diagnostics.CodeAnalysis;
using UnityEngine;


public class FluteAndroidBanner
{
    private readonly AndroidJavaObject _bannerPlugin;


	public FluteAndroidBanner(string adUnitId)
    {
		_bannerPlugin = new AndroidJavaObject("com.flute.ads.unity.BannerUnityPlugin", adUnitId);
    }


    [SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
	public void CreateBanner(Flute.AdPosition position)
    {
        _bannerPlugin.Call("createBanner", (int) position);
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
