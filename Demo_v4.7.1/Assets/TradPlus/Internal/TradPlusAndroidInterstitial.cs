using UnityEngine;

public class TradPlusAndroidInterstitial
{
    private readonly AndroidJavaObject _interstitialPlugin;


	public TradPlusAndroidInterstitial(string adUnitId)
    {
		_interstitialPlugin = new AndroidJavaObject("com.tradplus.ads.unity.InterstitialUnityPlugin", adUnitId);
    }

    //请求广告
    public void RequestInterstitialAd(bool autoReload = false)
    {
        Debug.Log("_interstitialPlugin.Call");
        _interstitialPlugin.Call("request",autoReload);
    }

    public void loadForcedlyInterstitial()
    {
        Debug.Log("_interstitialPlugin.Call");
        _interstitialPlugin.Call("loadForcedly");
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
        _interstitialPlugin.Call("confirmUWSAd");
    }

    //进入广告位所在界面时调用（广告场景ID）
    public void ShowInterstitialConfirmUWSAd(string adSceneId)
    {
        _interstitialPlugin.Call("confirmUWSAd", adSceneId);
    }

    //check广告是否准备好
    public bool IsInterstitialReady {
        get { return _interstitialPlugin.Call<bool>("isReady"); }
    }

    //销毁广告
    public void DestroyInterstitialAd()
    {
        _interstitialPlugin.Call("destroy");
    }
}
