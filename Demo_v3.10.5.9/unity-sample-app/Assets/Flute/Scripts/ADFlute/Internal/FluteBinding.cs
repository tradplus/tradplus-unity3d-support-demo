using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using FluteInternal.ThirdParty.MiniJSON;

[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
public class FluteBinding
{
    public Flute.Reward SelectedReward;

    private readonly string _adUnitId;
    public FluteBinding(string adUnitId)
    {
        _adUnitId = adUnitId;
        SelectedReward = new Flute.Reward { Label = string.Empty };
    }

    public void CreateBanner(Flute.AdPosition position)
    {
        _fluteCreateBanner((int) position, _adUnitId);
    }

    public void DestroyBanner()
    {
        _fluteDestroyBanner(_adUnitId);
    }

    public void ShowBanner(bool shouldShow)
    {
        _fluteShowBanner(_adUnitId, shouldShow);
    }

    public void RequestInterstitialAd()
    {
        _fluteRequestInterstitialAd(_adUnitId);
    }
		
    public bool IsInterstitialReady {
        get { return _fluteIsInterstitialReady(_adUnitId); }
    }
		
    public void ShowInterstitialAd()
    {
        _fluteShowInterstitialAd(_adUnitId);
    }

    public void DestroyInterstitialAd()
    {
        _fluteDestroyInterstitialAd(_adUnitId);
    }

    public void RequestRewardedVideo()
    {
		_fluteRequestRewardedVideo(_adUnitId);
    }

    // Queries if a rewarded video ad has been loaded for the given ad unit id.
    public bool HasRewardedVideo()
    {
		return _fluteHasRewardedVideo(_adUnitId);
    }

    // If a rewarded video ad is loaded this will take over the screen and show the ad
    public void ShowRewardedVideo()
    {
		_fluteShowRewardedVideo(_adUnitId);
    }
		
    public void DestroyRewardedVideo()
    {
        _fluteDestroyRewardedVideo(_adUnitId);
    }
    #region DllImports
#if ENABLE_IL2CPP && UNITY_ANDROID
    // IL2CPP on Android scrubs DllImports, so we need to provide stubs to unblock compilation
    private static void _fluteCreateBanner(int position, string adUnitId) { }
    private static void _fluteDestroyBanner(string adUnitId) {}
    private static void _fluteShowBanner(string adUnitId, bool shouldShow) {}
    private static void _fluteRefreshBanner(string adUnitId, string keywords, string userDataKeywords) {}
    private static void _fluteSetAutorefreshEnabled(string adUnitId, bool enabled) {}
    private static void _fluteForceRefresh(string adUnitId) {}
    private static void _fluteRequestInterstitialAd(string adUnitId) {}
    private static bool _fluteIsInterstitialReady(string adUnitId) { return false; }
    private static void _fluteShowInterstitialAd(string adUnitId) {}
    private static void _fluteDestroyInterstitialAd(string adUnitId) {}
    private static void _fluteRequestRewardedVideo(string adUnitId, string json, string keywords,
                                                   string userDataKeywords, double latitude, double longitude,
                                                   string customerId) {}
    private static bool _FluteHasRewardedVideo(string adUnitId) { return false; }
    private static string _FluteGetAvailableRewards(string adUnitId) { return null; }
    private static void _fluteShowRewardedVideo(string adUnitId, string currencyName, int currencyAmount,
                                                string customData) {}
#else
    [DllImport("__Internal")]
    private static extern void _fluteCreateBanner(int position, string adUnitId);


    [DllImport("__Internal")]
    private static extern void _fluteDestroyBanner(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _fluteShowBanner(string adUnitId, bool shouldShow);

    [DllImport("__Internal")]
	private static extern void _fluteRequestInterstitialAd(string adUnitId);


    [DllImport("__Internal")]
    private static extern bool _fluteIsInterstitialReady(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _fluteShowInterstitialAd(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _fluteDestroyInterstitialAd(string adUnitId);

   [DllImport("__Internal")]
	private static extern void _fluteRequestRewardedVideo(string adUnitId);


    [DllImport("__Internal")]
    private static extern bool _fluteHasRewardedVideo(string adUnitId);


    [DllImport("__Internal")]
    private static extern void _fluteShowRewardedVideo(string adUnitId);

   [DllImport("__Internal")]
    private static extern void _fluteDestroyRewardedVideo(string adUnitId);

#endif
    #endregion
}
