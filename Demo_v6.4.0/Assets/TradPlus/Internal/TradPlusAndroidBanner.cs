using System.Diagnostics.CodeAnalysis;
using UnityEngine;


public class TradPlusAndroidBanner
{
    private readonly AndroidJavaObject _bannerPlugin;


	public TradPlusAndroidBanner(string adUnitId)
    {
		_bannerPlugin = new AndroidJavaObject("com.tradplus.ads.unity.BannerUnityPlugin", adUnitId);
    }

    //加载广告
    [SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
	public void CreateBanner(TradPlus.AdPosition position)
    {
        _bannerPlugin.Call("createBanner", (int) position);
    }

    //隐藏or显示广告
    public void ShowBanner(bool shouldShow)
    {
        _bannerPlugin.Call("hideBanner", !shouldShow);
    }

    //销毁广告
    public void DestroyBanner()
    {
        _bannerPlugin.Call("destroyBanner");
    }

    //进入广告场景
    public void ShowBannerConfirmUWSAd()
    {
        _bannerPlugin.Call("entryAdScenario");
    }

    //进入广告场景(新)
    public void BannerEntryAdScenario()
    {
        _bannerPlugin.Call("entryAdScenario");
    }
}
