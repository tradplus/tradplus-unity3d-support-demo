using UnityEngine;

public class FluteAndroidInterstitial
{
    private readonly AndroidJavaObject _interstitialPlugin;


	public FluteAndroidInterstitial(string adUnitId)
    {
		_interstitialPlugin = new AndroidJavaObject("com.flute.ads.unity.InterstitialUnityPlugin", adUnitId);
    }


    public void RequestInterstitialAd(string keywords = "", string userDataKeywords = "")
    {
        Debug.Log("_interstitialPlugin.Call\n");
       _interstitialPlugin.Call("request", keywords, userDataKeywords);
    }


    public void ShowInterstitialAd()
    {
        _interstitialPlugin.Call("show");
    }


    public bool IsInterstitialReady {
        get { return _interstitialPlugin.Call<bool>("isReady"); }
    }


    public void DestroyInterstitialAd()
    {
        _interstitialPlugin.Call("destroy");
    }
}
