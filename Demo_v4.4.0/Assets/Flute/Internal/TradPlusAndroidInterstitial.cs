using UnityEngine;

public class TradPlusAndroidInterstitial
{
    private readonly AndroidJavaObject _interstitialPlugin;


	public TradPlusAndroidInterstitial(string adUnitId)
    {
		_interstitialPlugin = new AndroidJavaObject("com.tradplus.ads.unity.InterstitialUnityPlugin", adUnitId);
    }


    public void RequestInterstitialAd(bool autoReload = false)
    {
        Debug.Log("_interstitialPlugin.Call");
        _interstitialPlugin.Call("request",autoReload);
    }


    public void ShowInterstitialAd()
    {
        _interstitialPlugin.Call("show");
    }


    public void ShowInterstitialConfirmUWSAd()
    {
        _interstitialPlugin.Call("confirmUWSAd");
    }

    public bool IsInterstitialReady {
        get { return _interstitialPlugin.Call<bool>("isReady"); }
    }


    public void DestroyInterstitialAd()
    {
        _interstitialPlugin.Call("destroy");
    }
}
