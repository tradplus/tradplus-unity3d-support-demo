using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
public class FluteEventListener : MonoBehaviour
{
    [SerializeField]
    private FluteDemoGUI _demoGUI;


    private void Awake()
    {
        if (_demoGUI == null)
            _demoGUI = GetComponent<FluteDemoGUI>();

        if (_demoGUI != null) return;
        Debug.LogError("Missing reference to FluteDemoGUI.  Please fix in the editor.");
        Destroy(this);
    }


    private void OnEnable()
    {

        TradPlusManager.OnSdkInitializedEvent += OnSdkInitializedEvent;

        //Banner
        TradPlusManager.OnAdLoaded += OnAdLoaded;
        TradPlusManager.OnAdLoadFailed += OnAdLoadFailed;
        TradPlusManager.OnAdClicked += OnAdClicked;
        TradPlusManager.OnAdClosed += OnAdClosed;
        TradPlusManager.OnAdImpression += OnAdImpression;

        TradPlusManager.OnAdAllLoaded += OnAdAllLoaded;
        TradPlusManager.OneLayerLoaded += OneLayerLoaded;
        TradPlusManager.OneLayerLoadFailed += OneLayerLoadFailed;
        TradPlusManager.OnLoadAdStart += OnLoadAdStart;
        TradPlusManager.OnBiddingStart += OnBiddingStart;
        TradPlusManager.OnBiddingEnd += OnBiddingEnd;


        //原生广告
        TradPlusManager.OnNativeAdLoaded += OnNativeAdLoaded;
        TradPlusManager.OnNativeAdLoadFailed += OnNativeAdLoadFailed;
        TradPlusManager.OnNativeAdClicked += OnNativeAdClicked;
        TradPlusManager.OnNativeAdClosed += OnNativeAdClosed;
        TradPlusManager.OnNativeAdImpression += OnNativeAdImpression;

        TradPlusManager.OnNativeAdAllLoaded += OnNativeAdAllLoaded;
        TradPlusManager.OneNativeLayerLoaded += OneNativeLayerLoaded;
        TradPlusManager.OneNativeLayerLoadFailed += OneNativeLayerLoadFailed;
        TradPlusManager.OnNativeLoadAdStart += OnNativeLoadAdStart;
        TradPlusManager.OnNativeBiddingStart += OnNativeBiddingStart;
        TradPlusManager.OnNativeBiddingEnd += OnNativeBiddingEnd;


        //原生横幅广告
        TradPlusManager.OnNativeBannerAdLoaded += OnNativeBannerAdLoaded;
        TradPlusManager.OnNativeBannerAdLoadFailed += OnNativeBannerAdLoadFailed;
        TradPlusManager.OnNativeBannerAdClicked += OnNativeBannerAdClicked;
        TradPlusManager.OnNativeBannerAdClosed += OnNativeBannerAdClosed;
        TradPlusManager.OnNativeBannerAdImpression += OnNativeBannerAdImpression;

        TradPlusManager.OnNativeBannerAdAllLoaded += OnNativeAdAllLoaded;
        TradPlusManager.OneNativeBannerLayerLoaded += OneNativeBannerLayerLoaded;
        TradPlusManager.OneNativeBannerLayerLoadFailed += OneNativeBannerLayerLoadFailed;
        TradPlusManager.OnNativeBannerLoadAdStart += OnNativeBannerLoadAdStart;
        TradPlusManager.OnNativeBannerBiddingStart += OnNativeBannerBiddingStart;
        TradPlusManager.OnNativeBannerBiddingEnd += OnNativeBannerBiddingEnd;

        //插屏广告
        TradPlusManager.OnInterstitialAdLoaded += OnInterstitialAdLoaded;
        TradPlusManager.OnInterstitialAdFailed += OnInterstitialAdFailed;
        TradPlusManager.OnInterstitialAdImpression += OnInterstitialAdImpression;
        TradPlusManager.OnInterstitialAdClicked += OnInterstitialAdClicked;
        TradPlusManager.OnInterstitialAdClosed += OnInterstitialAdClosed;

        TradPlusManager.OnInterstitialAdAllLoaded += OnInterstitialAdAllLoaded;
        TradPlusManager.OneInterstitialLayerLoaded += OneInterstitialLayerLoaded;
        TradPlusManager.OneInterstitialLayerLoadFailed += OneInterstitialLayerLoadFailed;
        TradPlusManager.OnInterstitialLoadAdStart += OnInterstitialLoadAdStart;
        TradPlusManager.OnInterstitialBiddingStart += OnInterstitialBiddingStart;
        TradPlusManager.OnInterstitialBiddingEnd += OnInterstitialBiddingEnd;

        //激励视频广告
        TradPlusManager.OnRewardedVideoAdLoaded += OnRewardedVideoAdLoaded;
        TradPlusManager.OnRewardedVideoAdFailed += OnRewardedVideoAdFailed;
        TradPlusManager.OnRewardedVideoAdImpression += OnRewardedVideoAdImpression;
        TradPlusManager.OnRewardedVideoAdClicked += OnRewardedVideoAdClicked;
        TradPlusManager.OnRewardedVideoAdClosed += OnRewardedVideoAdClosed;
        TradPlusManager.OnRewardedVideoAdReward += OnRewardedVideoAdReward;
        TradPlusManager.OnRewardedVideoAdVideoError += OnRewardedVideoAdVideoError;

        TradPlusManager.OnRewardedVideoAdAllLoaded += OnRewardedVideoAdAllLoaded;
        TradPlusManager.OneRewardedVideoLayerLoaded += OneRewardedVideoLayerLoaded;
        TradPlusManager.OneRewardedVideoLayerLoadFailed += OneRewardedVideoLayerLoadFailed;
        TradPlusManager.OnRewardedVideoLoadAdStart += OnRewardedVideoLoadAdStart;
        TradPlusManager.OnRewardedVideoBiddingStart += OnRewardedVideoBiddingStart;
        TradPlusManager.OnRewardedVideoBiddingEnd += OnRewardedVideoBiddingEnd;

        //积分墙广告
        TradPlusManager.OnOfferWallAdLoaded += OnOfferWallAdLoaded;
        TradPlusManager.OnOfferWallAdFailed += OnOfferWallAdFailed;
        TradPlusManager.OnOfferWallAdImpression += OnOfferWallAdImpression;
        TradPlusManager.OnOfferWallAdClicked += OnOfferWallAdClicked;
        TradPlusManager.OnOfferWallAdClosed += OnOfferWallAdClosed;
        TradPlusManager.OnOfferWallAdReward += OnOfferWallAdReward;

        TradPlusManager.OnOfferWallAdAllLoaded += OnOfferWallAdAllLoaded;
        TradPlusManager.OneOfferWallLayerLoaded += OneOfferWallLayerLoaded;
        TradPlusManager.OneOfferWallLayerLoadFailed += OneOfferWallLayerLoadFailed;
        TradPlusManager.OnOfferWallLoadAdStart += OnOfferWallLoadAdStart;
        TradPlusManager.OnOfferWallBiddingStart += OnOfferWallBiddingStart;
        TradPlusManager.OnOfferWallBiddingEnd += OnOfferWallBiddingEnd;


        //GDPR
        TradPlusManager.OnGDPRSuccessEvent += OnGDPRSuccessEvent;
        TradPlusManager.OnGDPRFailedEvent += OnGDPRFailedEvent;
    }


    private void OnDisable()
    {
        // Remove all event handlers
        TradPlusManager.OnSdkInitializedEvent -= OnSdkInitializedEvent;

        // Banner
        TradPlusManager.OnAdLoaded -= OnAdLoaded;
        TradPlusManager.OnAdLoadFailed -= OnAdLoadFailed;
        TradPlusManager.OnAdClicked -= OnAdClicked;
        TradPlusManager.OnAdClosed -= OnAdClosed;
        TradPlusManager.OnAdImpression -= OnAdImpression;

        TradPlusManager.OnAdAllLoaded -= OnAdAllLoaded;
        TradPlusManager.OneLayerLoaded -= OneLayerLoaded;
        TradPlusManager.OneLayerLoadFailed -= OneLayerLoadFailed;
        TradPlusManager.OnLoadAdStart -= OnLoadAdStart;
        TradPlusManager.OnBiddingStart -= OnBiddingStart;
        TradPlusManager.OnBiddingEnd -= OnBiddingEnd;


        // Native 
        TradPlusManager.OnNativeAdLoaded -= OnNativeAdLoaded;
        TradPlusManager.OnNativeAdLoadFailed -= OnNativeAdLoadFailed;
        TradPlusManager.OnNativeAdClicked -= OnNativeAdClicked;
        TradPlusManager.OnNativeAdClosed -= OnNativeAdClosed;
        TradPlusManager.OnNativeAdImpression -= OnNativeAdImpression;

        TradPlusManager.OnNativeAdAllLoaded -= OnNativeAdAllLoaded;
        TradPlusManager.OneNativeLayerLoaded -= OneNativeLayerLoaded;
        TradPlusManager.OneNativeLayerLoadFailed -= OneNativeLayerLoadFailed;
        TradPlusManager.OnNativeLoadAdStart -= OnNativeLoadAdStart;
        TradPlusManager.OnNativeBiddingStart -= OnNativeBiddingStart;
        TradPlusManager.OnNativeBiddingEnd -= OnNativeBiddingEnd;


        //原生横幅广告
        TradPlusManager.OnNativeBannerAdLoaded -= OnNativeBannerAdLoaded;
        TradPlusManager.OnNativeBannerAdLoadFailed -= OnNativeBannerAdLoadFailed;
        TradPlusManager.OnNativeBannerAdClicked -= OnNativeBannerAdClicked;
        TradPlusManager.OnNativeBannerAdClosed -= OnNativeBannerAdClosed;
        TradPlusManager.OnNativeBannerAdImpression -= OnNativeBannerAdImpression;

        TradPlusManager.OnNativeBannerAdAllLoaded -= OnNativeAdAllLoaded;
        TradPlusManager.OneNativeBannerLayerLoaded -= OneNativeBannerLayerLoaded;
        TradPlusManager.OneNativeBannerLayerLoadFailed -= OneNativeBannerLayerLoadFailed;
        TradPlusManager.OnNativeBannerLoadAdStart -= OnNativeBannerLoadAdStart;
        TradPlusManager.OnNativeBannerBiddingStart -= OnNativeBannerBiddingStart;
        TradPlusManager.OnNativeBannerBiddingEnd -= OnNativeBannerBiddingEnd;

        //插屏广告
        TradPlusManager.OnInterstitialAdLoaded -= OnInterstitialAdLoaded;
        TradPlusManager.OnInterstitialAdFailed -= OnInterstitialAdFailed;
        TradPlusManager.OnInterstitialAdImpression -= OnInterstitialAdImpression;
        TradPlusManager.OnInterstitialAdClicked -= OnInterstitialAdClicked;
        TradPlusManager.OnInterstitialAdClosed -= OnInterstitialAdClosed;

        TradPlusManager.OnInterstitialAdAllLoaded -= OnInterstitialAdAllLoaded;
        TradPlusManager.OneInterstitialLayerLoaded -= OneInterstitialLayerLoaded;
        TradPlusManager.OneInterstitialLayerLoadFailed -= OneInterstitialLayerLoadFailed;
        TradPlusManager.OnInterstitialLoadAdStart -= OnInterstitialLoadAdStart;
        TradPlusManager.OnInterstitialBiddingStart -= OnInterstitialBiddingStart;
        TradPlusManager.OnInterstitialBiddingEnd -= OnInterstitialBiddingEnd;


        //激励视频广告
        TradPlusManager.OnRewardedVideoAdLoaded -= OnRewardedVideoAdLoaded;
        TradPlusManager.OnRewardedVideoAdFailed -= OnRewardedVideoAdFailed;
        TradPlusManager.OnRewardedVideoAdImpression -= OnRewardedVideoAdImpression;
        TradPlusManager.OnRewardedVideoAdClicked -= OnRewardedVideoAdClicked;
        TradPlusManager.OnRewardedVideoAdClosed -= OnRewardedVideoAdClosed;
        TradPlusManager.OnRewardedVideoAdReward -= OnRewardedVideoAdReward;
        TradPlusManager.OnRewardedVideoAdVideoError -= OnRewardedVideoAdVideoError;

        TradPlusManager.OnRewardedVideoAdAllLoaded -= OnRewardedVideoAdAllLoaded;
        TradPlusManager.OneRewardedVideoLayerLoaded -= OneRewardedVideoLayerLoaded;
        TradPlusManager.OneRewardedVideoLayerLoadFailed -= OneRewardedVideoLayerLoadFailed;
        TradPlusManager.OnRewardedVideoLoadAdStart -= OnRewardedVideoLoadAdStart;
        TradPlusManager.OnRewardedVideoBiddingStart -= OnRewardedVideoBiddingStart;
        TradPlusManager.OnRewardedVideoBiddingEnd -= OnRewardedVideoBiddingEnd;

        //积分墙广告
        TradPlusManager.OnOfferWallAdLoaded -= OnOfferWallAdLoaded;
        TradPlusManager.OnOfferWallAdFailed -= OnOfferWallAdFailed;
        TradPlusManager.OnOfferWallAdImpression -= OnOfferWallAdImpression;
        TradPlusManager.OnOfferWallAdClicked -= OnOfferWallAdClicked;
        TradPlusManager.OnOfferWallAdClosed -= OnOfferWallAdClosed;
        TradPlusManager.OnOfferWallAdReward -= OnOfferWallAdReward;

        TradPlusManager.OnOfferWallAdAllLoaded -= OnOfferWallAdAllLoaded;
        TradPlusManager.OneOfferWallLayerLoaded -= OneOfferWallLayerLoaded;
        TradPlusManager.OneOfferWallLayerLoadFailed -= OneOfferWallLayerLoadFailed;
        TradPlusManager.OnOfferWallLoadAdStart -= OnOfferWallLoadAdStart;
        TradPlusManager.OnOfferWallBiddingStart -= OnOfferWallBiddingStart;
        TradPlusManager.OnOfferWallBiddingEnd -= OnOfferWallBiddingEnd;
        ////GDPR
        TradPlusManager.OnGDPRSuccessEvent -= OnGDPRSuccessEvent;
        TradPlusManager.OnGDPRFailedEvent -= OnGDPRFailedEvent;
    }


    private void OnSdkInitializedEvent(string appId)
    {
        Debug.Log("OnSdkInitializedEvent: " + appId);
        _demoGUI.SdkInitialized();
    }

    //GDPR
    private void OnGDPRSuccessEvent(string appId)
    {
        Debug.Log("onGDPRSuccessEvent: " + appId);
        _demoGUI.GDPRSuccess(appId);
    }

    private void OnGDPRFailedEvent(string appId)
    {
        Debug.Log("onGDPRFailedEvent: " + appId);
        _demoGUI.GDPRFailed(appId);
    }


    // Native 
    private void OnNativeAdLoaded(string adUnitId, float height)
    {
        Debug.Log("OnNativeAdLoaded: " + adUnitId + " height: " + height);
    }

    private void OnNativeAdLoadFailed(string adUnitId, string error)
    {
        Debug.Log("OnNativeAdLoadFailed: " + adUnitId + "error :"+ error);
    }

    private void OnNativeAdClicked(string adUnitId)
    {
        Debug.Log("OnNativeAdClicked: " + adUnitId);
    }

    private void OnNativeAdClosed(string adUnitId)
    {
        Debug.Log("OnNativeAdClosed: " + adUnitId);
    }

    private void OnNativeAdImpression(string adUnitId)
    {
        Debug.Log("OnNativeAdImpression: " + adUnitId);
    }

    private void OnNativeAdAllLoaded(bool isLoadedSucces, string adUnitId)
    {
        Debug.Log("OnNativeAdAllLoaded: " + isLoadedSucces + " adUnitId: " + adUnitId);

    }

    private void OneNativeLayerLoadFailed(string tpAdInfo, string error)
    {
        Debug.Log("OneNativeLayerLoadFailed: " + tpAdInfo);
    }

    private void OneNativeLayerLoaded(string tpAdInfo)
    {
        Debug.Log("OneNativeLayerLoaded: " + tpAdInfo);
    }

    private void OnNativeLoadAdStart(string tpAdInfo)
    {
        Debug.Log("OnNativeLoadAdStart: " + tpAdInfo);
    }

    private void OnNativeBiddingStart(string tpAdInfo)
    {
        Debug.Log("OnNativeBiddingStart: " + tpAdInfo);
    }

    private void OnNativeBiddingEnd(string tpAdInfo)
    {
        Debug.Log("OnNativeBiddingEnd: " + tpAdInfo);
    }

    // NativeBanner
    private void OnNativeBannerAdLoaded(string adUnitId, float height)
    {
        Debug.Log("OnNativeBannerAdLoaded: " + adUnitId + " height: " + height);
    }

    private void OnNativeBannerAdLoadFailed(string adUnitId, string error)
    {
        Debug.Log("OnNativeBannerAdLoadFailed: " + adUnitId + "error :" + error);
    }

    private void OnNativeBannerAdClicked(string adUnitId)
    {
        Debug.Log("OnNativeBannerAdClicked: " + adUnitId);
    }

    private void OnNativeBannerAdClosed(string adUnitId)
    {
        Debug.Log("OnNativeBannerAdClosed: " + adUnitId);
    }

    private void OnNativeBannerAdImpression(string adUnitId)
    {
        Debug.Log("OnNativeBannerAdImpression: " + adUnitId);
    }

    private void OnNativeBannerAdAllLoaded(bool isLoadedSucces, string adUnitId)
    {
        Debug.Log("OnNativeBannerAdAllLoaded: " + isLoadedSucces + " adUnitId: " + adUnitId);

    }

    private void OneNativeBannerLayerLoadFailed(string tpAdInfo, string error)
    {
        Debug.Log("OneNativeBannerLayerLoadFailed: " + tpAdInfo);
    }

    private void OneNativeBannerLayerLoaded(string tpAdInfo)
    {
        Debug.Log("OneNativeBannerLayerLoaded: " + tpAdInfo);
    }

    private void OnNativeBannerLoadAdStart(string tpAdInfo)
    {
        Debug.Log("OnNativeBannerLoadAdStart: " + tpAdInfo);
    }

    private void OnNativeBannerBiddingStart(string tpAdInfo)
    {
        Debug.Log("OnNativeBannerBiddingStart: " + tpAdInfo);
    }

    private void OnNativeBannerBiddingEnd(string tpAdInfo)
    {
        Debug.Log("OnNativeBannerBiddingEnd: " + tpAdInfo);
    }

    private void AdFailed(string adUnitId, string action, string error)
    {
        var errorMsg = "Failed to " + action + " ad unit " + adUnitId;
        if (!string.IsNullOrEmpty(error))
            errorMsg += ": " + error;
        Debug.LogError(errorMsg);
        _demoGUI.UpdateStatusLabel("Error: " + errorMsg);
    }


    // Banner Events
    private void OnAdLoaded(string tpAdInfo, float height)
    {
        Debug.Log("Banner请求一次广告，第一个源加载成功==========OnAdLoadedEvent: " + tpAdInfo + " height: " + height);
        _demoGUI.BannerLoaded(tpAdInfo, height);
    }

    private void OnAdLoadFailed(string tpAdInfo, string error)
    {
        Debug.Log("Banner请求一次广告，配置的所有源加载失败==========onAdLoadFailed: " + tpAdInfo + " error: " + error);
        AdFailed(tpAdInfo, "load banner", error);
    }

    private void OnAdClicked(string tpAdInfo)
    {
        Debug.Log("Banner广告源被点击==========onAdClicked: " + tpAdInfo);
       
    }

    private void OnAdImpression(string tpAdInfo)
    {
        Debug.Log("Banner广告源展示==========onAdImpression: " + tpAdInfo);
       
    }

    private void OnAdClosed(string tpAdInfo)
    {
        Debug.Log("Banner广告源被关闭（部分三方5回调）==========OnAdCollapsedEvent: " + tpAdInfo);
    }

    private void OnAdAllLoaded(bool isLoadedSucces, string adUnitId)
    { 
        Debug.Log("Banner请求一次广告，所有配置走完 是否有可用广告==========onAdAllLoaded: " + isLoadedSucces + " adUnitId: " + adUnitId);
    }

    private void OneLayerLoadFailed(string tpAdInfo, string error)
    {
        Debug.Log("Banner请求一次广告，每个加载失败的广告源==========oneLayerLoadFailed: " + tpAdInfo);
    }

    private void OneLayerLoaded(string tpAdInfo)
    {
        Debug.Log("Banner请求一次广告，每个加载成功的广告源==========oneLayerLoaded: " + tpAdInfo);
    }

    private void OnLoadAdStart(string tpAdInfo)
    {
        Debug.Log("Banner请求广告开始==========onLoadAdStart: " + tpAdInfo);
    }

    private void OnBiddingStart(string tpAdInfo)
    {
        Debug.Log("Banner Bidding开始==========onBiddingStart: " + tpAdInfo);
    }

    private void OnBiddingEnd(string tpAdInfo)
    {
        Debug.Log("Banner Bidding结束==========onBiddingEnd: " + tpAdInfo);
    }


    // Interstitial Events
    private void OnInterstitialAdLoaded(string adUnitId)
    {
        Debug.Log("onInterstitialAdLoaded: " + adUnitId);
        _demoGUI.AdLoaded(adUnitId);
    }


    private void OnInterstitialAdFailed(string adUnitId, string error)
    {
        Debug.Log("OnInterstitialAdFailed: " + adUnitId);
        AdFailed(adUnitId, "load onInterstitialAdFailed", error);
    }


    private void OnInterstitialAdImpression(string adUnitId)
    {
        Debug.Log("onInterstitialAdImpression: " + adUnitId);
    }


    private void OnInterstitialAdClicked(string adUnitId)
    {
        Debug.Log("OnInterstitialAdClicked: " + adUnitId);
    }


    private void OnInterstitialAdClosed(string adUnitId)
    {
        Debug.Log("onInterstitialAdClosed: " + adUnitId);
        //_demoGUI.AdDismissed(adUnitId);
    }

    private void OnInterstitialAdAllLoaded(bool isLoadedSucces, string adUnitId)
    {
        Debug.Log("onInterstitialAdAllLoaded: " + adUnitId + " isLoadedSucces:" + isLoadedSucces);
    }

    private void OneInterstitialLayerLoadFailed(string tpAdInfo, string error)
    {
        Debug.Log("oneInterstitialLayerLoadFailed: " + tpAdInfo);
    }

    private void OneInterstitialLayerLoaded(string tpAdInfo)
    {
        Debug.Log("oneInterstitialLayerLoaded: " + tpAdInfo);
    }

    private void OnInterstitialLoadAdStart(string tpAdInfo)
    {
        Debug.Log("onInterstitialLoadAdStart: " + tpAdInfo);
    }

    private void OnInterstitialBiddingStart(string tpAdInfo)
    {
        Debug.Log("onInterstitialBiddingStart: " + tpAdInfo);
    }

    private void OnInterstitialBiddingEnd(string tpAdInfo)
    {
        Debug.Log("onInterstitialBiddingEnd: " + tpAdInfo);
    }



    // Rewarded Video Events
    private void OnRewardedVideoAdLoaded(string adUnitId)
    {
        Debug.Log("onRewardedVideoAdLoaded: " + adUnitId);
        _demoGUI.AdLoaded(adUnitId);
    }


    private void OnRewardedVideoAdFailed(string adUnitId, string error)
    {
        Debug.Log("OnRewardedVideoAdFailed: " + adUnitId);
        AdFailed(adUnitId, "onRewardedVideoAdFailed", error);
    }


    private void OnRewardedVideoAdImpression(string adUnitId)
    {
        Debug.Log("onRewardedVideoAdImpression: " + adUnitId);
    }


    private void OnRewardedVideoAdClicked(string adUnitId)
    {
        Debug.Log("onRewardedVideoAdClicked: " + adUnitId);
    }


    private void OnRewardedVideoAdClosed(string adUnitId)
    {
        Debug.Log("onRewardedVideoAdClosed: " + adUnitId);
        _demoGUI.AdDismissed(adUnitId);

    }

    private void OnRewardedVideoAdReward(string adUnitId)
    {
        Debug.Log("onRewardedVideoAdReward: " + adUnitId);
    }

    private void OnRewardedVideoAdVideoError(string adUnitId)
    {
        Debug.Log("onRewardedVideoAdVideoError: " + adUnitId);
    }


    private void OnRewardedVideoAdAllLoaded(bool isLoadedSucces, string adUnitId)
    {
        Debug.Log("onRewardedVideoAdAllLoaded: " + adUnitId + "OnRewardedVideoAllLoadedEvent:: " + " isLoadedSucces:" + isLoadedSucces);
    }

    private void OneRewardedVideoLayerLoadFailed(string tpAdInfo, string error)
    {
        Debug.Log("oneRewardedVideoLayerLoadFailed: " + tpAdInfo);
    }

    private void OneRewardedVideoLayerLoaded(string tpAdInfo)
    {
        Debug.Log("oneRewardedVideoLayerLoaded: " + tpAdInfo);
    }

    private void OnRewardedVideoLoadAdStart(string tpAdInfo)
    {
        Debug.Log("onRewardedVideoLoadAdStart: " + tpAdInfo);
    }

    private void OnRewardedVideoBiddingStart(string tpAdInfo)
    {
        Debug.Log("onRewardedVideoBiddingStart: " + tpAdInfo);
    }

    private void OnRewardedVideoBiddingEnd(string tpAdInfo)
    {
        Debug.Log("onRewardedVideoBiddingEnd: " + tpAdInfo);
    }


    //OfferWall Events
    private void OnOfferWallAdLoaded(string adUnitId)
    {
        Debug.Log("onOfferWallAdLoaded: " + adUnitId);
        _demoGUI.AdLoaded(adUnitId);
    }


    private void OnOfferWallAdFailed(string adUnitId, string error)
    {
        AdFailed(adUnitId, "onOfferWallAdFailedv", error);
    }


    private void OnOfferWallAdImpression(string adUnitId)
    {
        Debug.Log("onOfferWallAdImpression: " + adUnitId);
    }


    private void OnOfferWallAdClicked(string adUnitId)
    {
        Debug.Log("OnOfferWallAdClicked: " + adUnitId);
    }


    private void OnOfferWallAdClosed(string adUnitId)
    {
        Debug.Log("onOfferWallAdClosed: " + adUnitId);
        _demoGUI.AdDismissed(adUnitId);

    }

    private void OnOfferWallAdReward(string adUnitId)
    {
        Debug.Log("onOfferWallAdReward: " + adUnitId);
    }

    private void OnOfferWallAdVideoError(string adUnitId)
    {
        Debug.Log("onOfferWallAdVideoError: " + adUnitId);
    }


    private void OnOfferWallAdAllLoaded(bool isLoadedSucces, string adUnitId)
    {
        Debug.Log("onOfferWallAdAllLoaded: " + adUnitId + "OnRewardedVideoAllLoadedEvent:: " + " isLoadedSucces:" + isLoadedSucces);
    }

    private void OneOfferWallLayerLoadFailed(string tpAdInfo, string error)
    {
        Debug.Log("oneOfferWallLayerLoadFailed: " + tpAdInfo);
    }

    private void OneOfferWallLayerLoaded(string tpAdInfo)
    {
        Debug.Log("oneOfferWallLayerLoaded: " + tpAdInfo);
    }

    private void OnOfferWallLoadAdStart(string tpAdInfo)
    {
        Debug.Log("onOfferWallLoadAdStart: " + tpAdInfo);
    }

    private void OnOfferWallBiddingStart(string tpAdInfo)
    {
        Debug.Log("onOfferWallBiddingStart: " + tpAdInfo);
    }

    private void OnOfferWallBiddingEnd(string tpAdInfo)
    {
        Debug.Log("onOfferWallBiddingEnd: " + tpAdInfo);
    }
}
