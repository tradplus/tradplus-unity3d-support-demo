using UnityEngine;
using System.Collections.Generic;
using TradPlusInternal.ThirdParty.MiniJSON;

public class TradPlusAndroidInterstitial
{
    private readonly AndroidJavaObject _interstitialPlugin;


	public TradPlusAndroidInterstitial(string adUnitId)
    {
		_interstitialPlugin = new AndroidJavaObject("com.tradplus.ads.unity.InterstitialUnityPlugin", adUnitId);
    }

    //请求广告
    public void RequestInterstitialAd(bool autoReload = true)
    {
        Debug.Log("_interstitialPlugin.Call");
        _interstitialPlugin.Call("request",autoReload);
    }

    // V740支持
    public void SetInterstitialCustomParams(Dictionary<string, object> map)
    {
        _interstitialPlugin.Call("setCustomParams", Json.Serialize(map));
    }

    //强制Reload
    public void ReloadInterstitialAd()
    {
        _interstitialPlugin.Call("reloadInterstitialAd");
    }

    //展示广告
    public void ShowInterstitialAd()
    {
        _interstitialPlugin.Call("show");
    }

    //展示广告（广告场景ID）
    public void ShowInterstitialAd(string adSceneId)
    {
        _interstitialPlugin.Call("show", adSceneId);
    }

    //进入广告位所在界面时调用
    public void ShowInterstitialConfirmUWSAd()
    {
        _interstitialPlugin.Call("entryAdScenario");
    }

    //进入广告位所在界面时调用
    public void InterstitialEntryAdScenario()
    {
        _interstitialPlugin.Call("entryAdScenario");
    }

    //进入广告位所在界面时调用（广告场景ID）
    public void InterstitialEntryAdScenario(string adSceneId)
    {
        _interstitialPlugin.Call("entryAdScenario", adSceneId);
    }

    //check是否有可用广告
    public bool IsInterstitialReady 
    {
        get { return _interstitialPlugin.Call<bool>("isReady"); }
    }

    // V740 支持
    public void InterstitialOnDestroy()
    {
        _interstitialPlugin.Call("onDestroy");
    }

}
