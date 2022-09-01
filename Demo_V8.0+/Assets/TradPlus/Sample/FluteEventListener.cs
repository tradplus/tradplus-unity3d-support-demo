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
        TradPlusManager.OnBiddingStart += OnBiddingStart;
        TradPlusManager.OnBiddingEnd += OnBiddingEnd;
        TradPlusManager.OnAdShowFailed += OnAdShowFailed;

        TradPlusManager.OneLayerStartLoad += OneLayerStartLoad;
        TradPlusManager.OnAdStartLoad += OnAdStartLoad;

        TradPlusManager.OnDownloadStart += OnDownloadStart;
        TradPlusManager.OnDownloadUpdate += OnDownloadUpdate;
        TradPlusManager.OnDownloadPause += OnDownloadPause;
        TradPlusManager.OnDownloadFinish += OnDownloadFinish;
        TradPlusManager.OnDownloadFail += OnDownloadFail;
        TradPlusManager.OnInstalled += OnInstalled;


        //原生广告
        TradPlusManager.OnNativeAdLoaded += OnNativeAdLoaded;
        TradPlusManager.OnNativeAdLoadFailed += OnNativeAdLoadFailed;
        TradPlusManager.OnNativeAdClicked += OnNativeAdClicked;
        TradPlusManager.OnNativeAdClosed += OnNativeAdClosed;
        TradPlusManager.OnNativeAdImpression += OnNativeAdImpression;

        TradPlusManager.OnNativeAdAllLoaded += OnNativeAdAllLoaded;
        TradPlusManager.OneNativeLayerLoaded += OneNativeLayerLoaded;
        TradPlusManager.OneNativeLayerLoadFailed += OneNativeLayerLoadFailed;
        TradPlusManager.OnNativeBiddingStart += OnNativeBiddingStart;
        TradPlusManager.OnNativeBiddingEnd += OnNativeBiddingEnd;
        TradPlusManager.OnNativeAdShowFailed += OnNativeAdShowFailed;

        TradPlusManager.OneNativeLayerStartLoad += OneNativeLayerStartLoad;
        TradPlusManager.OnNativeAdStartLoad += OnNativeAdStartLoad;

        TradPlusManager.OnNativeAdVideoPlayStart += OnNativeAdVideoPlayStart;
        TradPlusManager.OnNativeAdVideoPlayEnd += OnNativeAdVideoPlayEnd;

        TradPlusManager.OnNativeDownloadStart += OnNativeDownloadStart;
        TradPlusManager.OnNativeDownloadUpdate += OnNativeDownloadUpdate;
        TradPlusManager.OnNativeDownloadPause += OnNativeDownloadPause;
        TradPlusManager.OnNativeDownloadFinish += OnNativeDownloadFinish;
        TradPlusManager.OnNativeDownloadFail += OnNativeDownloadFail;
        TradPlusManager.OnNativeInstalled += OnNativeInstalled;


        //原生横幅广告
        TradPlusManager.OnNativeBannerAdLoaded += OnNativeBannerAdLoaded;
        TradPlusManager.OnNativeBannerAdLoadFailed += OnNativeBannerAdLoadFailed;
        TradPlusManager.OnNativeBannerAdClicked += OnNativeBannerAdClicked;
        TradPlusManager.OnNativeBannerAdClosed += OnNativeBannerAdClosed;
        TradPlusManager.OnNativeBannerAdImpression += OnNativeBannerAdImpression;

        TradPlusManager.OnNativeBannerAdAllLoaded += OnNativeAdAllLoaded;
        TradPlusManager.OneNativeBannerLayerLoaded += OneNativeBannerLayerLoaded;
        TradPlusManager.OneNativeBannerLayerLoadFailed += OneNativeBannerLayerLoadFailed;
        TradPlusManager.OnNativeBannerBiddingStart += OnNativeBannerBiddingStart;
        TradPlusManager.OnNativeBannerBiddingEnd += OnNativeBannerBiddingEnd;
        TradPlusManager.OnNativeBannerAdShowFailed += OnNativeBannerAdShowFailed;

        TradPlusManager.OneNativeBannerLayerStartLoad += OneNativeBannerLayerStartLoad;
        TradPlusManager.OnNativeBannerAdStartLoad += OnNativeBannerAdStartLoad;

        TradPlusManager.OnNativeBannerDownloadStart += OnNativeBannerDownloadStart;
        TradPlusManager.OnNativeBannerDownloadUpdate += OnNativeBannerDownloadUpdate;
        TradPlusManager.OnNativeBannerDownloadPause += OnNativeBannerDownloadPause;
        TradPlusManager.OnNativeBannerDownloadFinish += OnNativeBannerDownloadFinish;
        TradPlusManager.OnNativeBannerDownloadFail += OnNativeBannerDownloadFail;
        TradPlusManager.OnNativeBannerInstalled += OnNativeBannerInstalled;


        //插屏广告
        TradPlusManager.OnInterstitialAdLoaded += OnInterstitialAdLoaded;
        TradPlusManager.OnInterstitialAdFailed += OnInterstitialAdFailed;
        TradPlusManager.OnInterstitialAdImpression += OnInterstitialAdImpression;
        TradPlusManager.OnInterstitialAdClicked += OnInterstitialAdClicked;
        TradPlusManager.OnInterstitialAdClosed += OnInterstitialAdClosed;

        TradPlusManager.OnInterstitialAdAllLoaded += OnInterstitialAdAllLoaded;
        TradPlusManager.OneInterstitialLayerLoaded += OneInterstitialLayerLoaded;
        TradPlusManager.OneInterstitialLayerLoadFailed += OneInterstitialLayerLoadFailed;
        TradPlusManager.OnInterstitialBiddingStart += OnInterstitialBiddingStart;
        TradPlusManager.OnInterstitialBiddingEnd += OnInterstitialBiddingEnd;
        TradPlusManager.OnInterstitialAdVideoError += OnInterstitialAdVideoError;

        TradPlusManager.OneInterstitialLayerStartLoad += OneInterstitialLayerStartLoad;
        TradPlusManager.OnInterstitialAdStartLoad += OnInterstitialAdStartLoad;

        TradPlusManager.OnInterstitialVideoPlayStart += OnInterstitialVideoPlayStart;
        TradPlusManager.OnInterstitialVideoPlayEnd += OnInterstitialVideoPlayEnd;

        TradPlusManager.OnInterstitialDownloadStart += OnInterstitialDownloadStart;
        TradPlusManager.OnInterstitialDownloadUpdate += OnInterstitialDownloadUpdate;
        TradPlusManager.OnInterstitialDownloadPause += OnInterstitialDownloadPause;
        TradPlusManager.OnInterstitialDownloadFinish += OnInterstitialDownloadFinish;
        TradPlusManager.OnInterstitialDownloadFail += OnInterstitialDownloadFail;
        TradPlusManager.OnInterstitialInstalled += OnInterstitialInstalled;

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
        TradPlusManager.OnRewardedVideoBiddingStart += OnRewardedVideoBiddingStart;
        TradPlusManager.OnRewardedVideoBiddingEnd += OnRewardedVideoBiddingEnd;
        TradPlusManager.OnRewardedVideoPlayStart += OnRewardedVideoPlayStart;
        TradPlusManager.OnRewardedVideoPlayEnd += OnRewardedVideoPlayEnd;

        TradPlusManager.OneRewardedVideoLayerStartLoad += OneRewardedVideoLayerStartLoad;
        TradPlusManager.OnRewardedVideoAdStartLoad += OnRewardedVideoAdStartLoad;

        //TradPlusManager.OnRewardedVideoPlayStart += OnRewardedVideoPlayStart;
        //TradPlusManager.OnRewardedVideoPlayEnd += OnRewardedVideoPlayEnd;

        TradPlusManager.OnRewardedVideoAgainImpression += OnRewardedVideoAgainImpression;
        TradPlusManager.OnRewardedVideoAgainVideoStart += OnRewardedVideoAgainVideoStart;
        TradPlusManager.OnRewardedVideoAgainVideoEnd += OnRewardedVideoAgainVideoEnd;
        TradPlusManager.OnRewardedVideoAgainVideoClicked += OnRewardedVideoAgainVideoClicked;
        TradPlusManager.OnRewardedVideoPlayAgainReward += OnRewardedVideoPlayAgainReward;
        TradPlusManager.OnRewardedVideoDownloadStart += OnRewardedVideoDownloadStart;

        TradPlusManager.OnRewardedVideoDownloadStart += OnRewardedVideoDownloadStart;
        TradPlusManager.OnRewardedVideoDownloadUpdate += OnRewardedVideoDownloadUpdate;
        TradPlusManager.OnRewardedVideoDownloadPause += OnRewardedVideoDownloadPause;
        TradPlusManager.OnRewardedVideoDownloadFinish += OnRewardedVideoDownloadFinish;
        TradPlusManager.OnRewardedVideoDownloadFail += OnRewardedVideoDownloadFail;
        TradPlusManager.OnRewardedVideoInstalled += OnRewardedVideoInstalled;

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
        TradPlusManager.OneOfferWallLayerStartLoad += OneOfferWallLayerStartLoad;
        TradPlusManager.OnAwardCurrencySuccess += OnAwardCurrencySuccess;
        TradPlusManager.OnSpendCurrencySuccess += OnSpendCurrencySuccess;
        TradPlusManager.OnCurrencyBalanceSuccess += OnCurrencyBalanceSuccess;
        TradPlusManager.OnAwardCurrencyFailed += OnAwardCurrencyFailed;
        TradPlusManager.OnSpendCurrencyFailed += OnSpendCurrencyFailed;
        TradPlusManager.OnCurrencyBalanceFailed += OnCurrencyBalanceFailed;
        TradPlusManager.OnSetUserIdSuccess += OnSetUserIdSuccess;
        TradPlusManager.OnSetUserIdFailed += OnSetUserIdFailed;
        TradPlusManager.OnOfferWallAdShowFailed += OnOfferWallAdShowFailed;
        TradPlusManager.OnOfferWallAdStartLoad += OnOfferWallAdStartLoad;

        //GDPR
        TradPlusManager.OnGDPRSuccessEvent += OnGDPRSuccessEvent;
        TradPlusManager.OnGDPRFailedEvent += OnGDPRFailedEvent;

        //地区查询
        TradPlusManager.OnCheckCurrentAreaSuccess += OnCheckCurrentAreaSuccess;
        TradPlusManager.OnCheckCurrentAreaFailed += OnCheckCurrentAreaFailed;
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
        TradPlusManager.OnBiddingStart -= OnBiddingStart;
        TradPlusManager.OnBiddingEnd -= OnBiddingEnd;
        TradPlusManager.OnAdShowFailed -= OnAdShowFailed;

        TradPlusManager.OneLayerStartLoad -= OneLayerStartLoad;
        TradPlusManager.OnAdStartLoad -= OnAdStartLoad;

        TradPlusManager.OnDownloadStart -= OnDownloadStart;
        TradPlusManager.OnDownloadUpdate -= OnDownloadUpdate;
        TradPlusManager.OnDownloadPause -= OnDownloadPause;
        TradPlusManager.OnDownloadFinish -= OnDownloadFinish;
        TradPlusManager.OnDownloadFail -= OnDownloadFail;
        TradPlusManager.OnInstalled -= OnInstalled;


        // Native 
        TradPlusManager.OnNativeAdLoaded -= OnNativeAdLoaded;
        TradPlusManager.OnNativeAdLoadFailed -= OnNativeAdLoadFailed;
        TradPlusManager.OnNativeAdClicked -= OnNativeAdClicked;
        TradPlusManager.OnNativeAdClosed -= OnNativeAdClosed;
        TradPlusManager.OnNativeAdImpression -= OnNativeAdImpression;

        TradPlusManager.OnNativeAdAllLoaded -= OnNativeAdAllLoaded;
        TradPlusManager.OneNativeLayerLoaded -= OneNativeLayerLoaded;
        TradPlusManager.OneNativeLayerLoadFailed -= OneNativeLayerLoadFailed;
        TradPlusManager.OnNativeBiddingStart -= OnNativeBiddingStart;
        TradPlusManager.OnNativeBiddingEnd -= OnNativeBiddingEnd;
        TradPlusManager.OnNativeAdShowFailed -= OnNativeAdShowFailed;

        TradPlusManager.OneNativeLayerStartLoad -= OneNativeLayerStartLoad;
        TradPlusManager.OnNativeAdStartLoad -= OnNativeAdStartLoad;

        TradPlusManager.OnNativeDownloadStart -= OnNativeDownloadStart;
        TradPlusManager.OnNativeDownloadUpdate -= OnNativeDownloadUpdate;
        TradPlusManager.OnNativeDownloadPause -= OnNativeDownloadPause;
        TradPlusManager.OnNativeDownloadFinish -= OnNativeDownloadFinish;
        TradPlusManager.OnNativeDownloadFail -= OnNativeDownloadFail;
        TradPlusManager.OnNativeInstalled -= OnNativeInstalled;


        //原生横幅广告
        TradPlusManager.OnNativeBannerAdLoaded -= OnNativeBannerAdLoaded;
        TradPlusManager.OnNativeBannerAdLoadFailed -= OnNativeBannerAdLoadFailed;
        TradPlusManager.OnNativeBannerAdClicked -= OnNativeBannerAdClicked;
        TradPlusManager.OnNativeBannerAdClosed -= OnNativeBannerAdClosed;
        TradPlusManager.OnNativeBannerAdImpression -= OnNativeBannerAdImpression;

        TradPlusManager.OnNativeBannerAdAllLoaded -= OnNativeAdAllLoaded;
        TradPlusManager.OneNativeBannerLayerLoaded -= OneNativeBannerLayerLoaded;
        TradPlusManager.OneNativeBannerLayerLoadFailed -= OneNativeBannerLayerLoadFailed;
        TradPlusManager.OnNativeBannerBiddingStart -= OnNativeBannerBiddingStart;
        TradPlusManager.OnNativeBannerBiddingEnd -= OnNativeBannerBiddingEnd;
        TradPlusManager.OnNativeBannerAdShowFailed -= OnNativeBannerAdShowFailed;

        TradPlusManager.OneNativeBannerLayerStartLoad -= OneNativeBannerLayerStartLoad;
        TradPlusManager.OnNativeBannerAdStartLoad -= OnNativeBannerAdStartLoad;

        TradPlusManager.OnNativeBannerDownloadStart -= OnNativeBannerDownloadStart;
        TradPlusManager.OnNativeBannerDownloadUpdate -= OnNativeBannerDownloadUpdate;
        TradPlusManager.OnNativeBannerDownloadPause -= OnNativeBannerDownloadPause;
        TradPlusManager.OnNativeBannerDownloadFinish -= OnNativeBannerDownloadFinish;
        TradPlusManager.OnNativeBannerDownloadFail -= OnNativeBannerDownloadFail;
        TradPlusManager.OnNativeBannerInstalled -= OnNativeBannerInstalled;

        //插屏广告
        TradPlusManager.OnInterstitialAdLoaded -= OnInterstitialAdLoaded;
        TradPlusManager.OnInterstitialAdFailed -= OnInterstitialAdFailed;
        TradPlusManager.OnInterstitialAdImpression -= OnInterstitialAdImpression;
        TradPlusManager.OnInterstitialAdClicked -= OnInterstitialAdClicked;
        TradPlusManager.OnInterstitialAdClosed -= OnInterstitialAdClosed;

        TradPlusManager.OnInterstitialAdAllLoaded -= OnInterstitialAdAllLoaded;
        TradPlusManager.OneInterstitialLayerLoaded -= OneInterstitialLayerLoaded;
        TradPlusManager.OneInterstitialLayerLoadFailed -= OneInterstitialLayerLoadFailed;
        TradPlusManager.OnInterstitialBiddingStart -= OnInterstitialBiddingStart;
        TradPlusManager.OnInterstitialBiddingEnd -= OnInterstitialBiddingEnd;
        TradPlusManager.OnInterstitialAdVideoError -= OnInterstitialAdVideoError;

        TradPlusManager.OneInterstitialLayerStartLoad -= OneInterstitialLayerStartLoad;
        TradPlusManager.OnInterstitialAdStartLoad -= OnInterstitialAdStartLoad;

        TradPlusManager.OnInterstitialDownloadStart -= OnInterstitialDownloadStart;
        TradPlusManager.OnInterstitialDownloadUpdate -= OnInterstitialDownloadUpdate;
        TradPlusManager.OnInterstitialDownloadPause -= OnInterstitialDownloadPause;
        TradPlusManager.OnInterstitialDownloadFinish -= OnInterstitialDownloadFinish;
        TradPlusManager.OnInterstitialDownloadFail -= OnInterstitialDownloadFail;
        TradPlusManager.OnInterstitialInstalled -= OnInterstitialInstalled;

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
        TradPlusManager.OnRewardedVideoBiddingStart -= OnRewardedVideoBiddingStart;
        TradPlusManager.OnRewardedVideoBiddingEnd -= OnRewardedVideoBiddingEnd;
        TradPlusManager.OnRewardedVideoPlayStart += OnRewardedVideoPlayStart;
        TradPlusManager.OnRewardedVideoPlayEnd += OnRewardedVideoPlayEnd;

        TradPlusManager.OneRewardedVideoLayerStartLoad -= OneRewardedVideoLayerStartLoad;
        TradPlusManager.OnRewardedVideoAdStartLoad -= OnRewardedVideoAdStartLoad;

        TradPlusManager.OnRewardedVideoDownloadStart -= OnRewardedVideoDownloadStart;

        TradPlusManager.OnRewardedVideoDownloadStart -= OnRewardedVideoDownloadStart;
        TradPlusManager.OnRewardedVideoDownloadUpdate -= OnRewardedVideoDownloadUpdate;
        TradPlusManager.OnRewardedVideoDownloadPause -= OnRewardedVideoDownloadPause;
        TradPlusManager.OnRewardedVideoDownloadFinish -= OnRewardedVideoDownloadFinish;
        TradPlusManager.OnRewardedVideoDownloadFail -= OnRewardedVideoDownloadFail;
        TradPlusManager.OnRewardedVideoInstalled -= OnRewardedVideoInstalled;

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
        TradPlusManager.OneOfferWallLayerStartLoad -= OneOfferWallLayerStartLoad;
        //TradPlusManager.OnOfferWallBiddingStart -= OnOfferWallBiddingStart;
        //TradPlusManager.OnOfferWallBiddingEnd -= OnOfferWallBiddingEnd;
        TradPlusManager.OnAwardCurrencySuccess -= OnAwardCurrencySuccess;
        TradPlusManager.OnSpendCurrencySuccess -= OnSpendCurrencySuccess;
        TradPlusManager.OnCurrencyBalanceSuccess -= OnCurrencyBalanceSuccess;
        TradPlusManager.OnAwardCurrencyFailed -= OnAwardCurrencyFailed;
        TradPlusManager.OnSpendCurrencyFailed -= OnSpendCurrencyFailed;
        TradPlusManager.OnCurrencyBalanceFailed -= OnCurrencyBalanceFailed;
        TradPlusManager.OnSetUserIdSuccess -= OnSetUserIdSuccess;
        TradPlusManager.OnSetUserIdFailed -= OnSetUserIdFailed;
        TradPlusManager.OnOfferWallAdShowFailed -= OnOfferWallAdShowFailed;
        TradPlusManager.OnOfferWallAdStartLoad -= OnOfferWallAdStartLoad;

        ////GDPR
        TradPlusManager.OnGDPRSuccessEvent -= OnGDPRSuccessEvent;
        TradPlusManager.OnGDPRFailedEvent -= OnGDPRFailedEvent;

        //地区查询
        TradPlusManager.OnCheckCurrentAreaSuccess -= OnCheckCurrentAreaSuccess;
        TradPlusManager.OnCheckCurrentAreaFailed -= OnCheckCurrentAreaFailed;
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
        Debug.Log("OnNativeAdLoadFailed: " + adUnitId + "error :" + error);
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

    private void OneNativeLayerStartLoad(string tpAdInfo)
    {
        Debug.Log("OneNativeLayerStartLoad: " + tpAdInfo);
    }

    private void OnNativeAdStartLoad(string tpAdInfo)
    {
        Debug.Log("OnNativeAdStartLoad: " + tpAdInfo);
    }

    private void OnNativeBiddingStart(string tpAdInfo)
    {
        Debug.Log("OnNativeBiddingStart: " + tpAdInfo);
    }

    private void OnNativeBiddingEnd(string tpAdInfo,string error)
    {
        Debug.Log("OnNativeBiddingEnd: " + tpAdInfo + "error:"+error);
    }

    private void OnNativeAdShowFailed(string tpAdInfo, string error)
    {
        Debug.Log("OnNativeAdShowFailed: " + tpAdInfo + "error:" + error);
    }

    private void OnNativeAdVideoPlayStart(string tpAdInfo)
    {
        Debug.Log("OnNativeAdVideoPlayStart: " + tpAdInfo);
    }

    private void OnNativeAdVideoPlayEnd(string tpAdInfo)
    {
        Debug.Log("OnNativeAdVideoPlayEnd: " + tpAdInfo);
    }

    private void OnNativeDownloadStart(string tpAdInfo,string networkinfo)
    {
        Debug.Log("OnNativeDownloadStart: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnNativeDownloadUpdate(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnNativeDownloadUpdate: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnNativeDownloadPause(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnNativeDownloadPause: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnNativeDownloadFinish(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnNativeDownloadFinish: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnNativeDownloadFail(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnNativeDownloadFail: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }


    private void OnNativeInstalled(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnNativeInstalled: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
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

    private void OneNativeBannerLayerStartLoad(string tpAdInfo)
    {
        Debug.Log("OneNativeBannerLayerStartLoad: " + tpAdInfo);
    }

    private void OnNativeBannerAdStartLoad(string tpAdInfo)
    {
        Debug.Log("OnNativeBannerAdStartLoad: " + tpAdInfo);
    }

    private void OnNativeBannerBiddingStart(string tpAdInfo)
    {
        Debug.Log("OnNativeBannerBiddingStart: " + tpAdInfo);
    }

    private void OnNativeBannerBiddingEnd(string tpAdInfo,string error)
    {
        Debug.Log("OnNativeBannerBiddingEnd: " + tpAdInfo + "error:"+ error);
    }

    private void OnNativeBannerAdShowFailed(string tpAdInfo, string error)
    {
        Debug.Log("OnNativeBannerAdShowFailed: " + tpAdInfo + "error:" + error);
    }

    private void AdFailed(string adUnitId, string action, string error)
    {
        var errorMsg = "Failed to " + action + " ad unit " + adUnitId;
        if (!string.IsNullOrEmpty(error))
            errorMsg += ": " + error;
        Debug.LogError(errorMsg);
        _demoGUI.UpdateStatusLabel("Error: " + errorMsg);
    }

    private void OnNativeBannerDownloadStart(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnNativeBannerDownloadStart: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnNativeBannerDownloadUpdate(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnNativeBannerDownloadUpdate: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnNativeBannerDownloadPause(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnNativeBannerDownloadPause: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnNativeBannerDownloadFinish(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnNativeBannerDownloadFinish: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnNativeBannerDownloadFail(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnNativeBannerDownloadFail: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }


    private void OnNativeBannerInstalled(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnNativeBannerInstalled: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
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

    private void OneLayerStartLoad(string tpAdInfo)
    {
        Debug.Log("Banner请求一次广告，每个开始加载的广告源==========oneLayerLoaded: " + tpAdInfo);
    }

    private void OnAdStartLoad(string tpAdInfo)
    {
        Debug.Log("Banner请求广告开始==========onLoadAdStart: " + tpAdInfo);
    }

    private void OnBiddingStart(string tpAdInfo)
    {
        Debug.Log("Banner Bidding开始==========onBiddingStart: " + tpAdInfo);
    }

    private void OnBiddingEnd(string tpAdInfo,string error)
    {
        Debug.Log("Banner Bidding结束==========onBiddingEnd: " + tpAdInfo + "error:"+error);
    }

    private void OnAdShowFailed(string tpAdInfo, string error)
    {
        Debug.Log("Banner 展示失败==========OnAdShowFailed: " + tpAdInfo + "error:" + error);
    }

    private void OnDownloadStart(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnDownloadStart: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnDownloadUpdate(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnDownloadUpdate: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnDownloadPause(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnDownloadPause: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnDownloadFinish(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnDownloadFinish: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnDownloadFail(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnDownloadFail: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }


    private void OnInstalled(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnInstalled: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
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

    private void OneInterstitialLayerStartLoad(string tpAdInfo)
    {
        Debug.Log("OneInterstitialLayerStartLoad: " + tpAdInfo);
    }

    private void OnInterstitialAdStartLoad(string tpAdInfo)
    {
        Debug.Log("OnInterstitialAdStartLoad: " + tpAdInfo);
    }

    private void OnInterstitialBiddingStart(string tpAdInfo)
    {
        Debug.Log("onInterstitialBiddingStart: " + tpAdInfo);
    }

    private void OnInterstitialBiddingEnd(string tpAdInfo,string error)
    {
        Debug.Log("onInterstitialBiddingEnd: " + tpAdInfo + "error" + error);
    }

    private void OnInterstitialAdVideoError(string tpAdInfo, string error)
    {
        Debug.Log("OnInterstitialAdVideoError: " + tpAdInfo + "error" + error);
    }

    private void OnInterstitialVideoPlayStart(string tpAdInfo)
    {
        Debug.Log("OnInterstitialVideoPlayStart: " + tpAdInfo);
    }

    private void OnInterstitialVideoPlayEnd(string tpAdInfo)
    {
        Debug.Log("OnInterstitialVideoPlayEnd: " + tpAdInfo);
    }

    private void OnInterstitialDownloadStart(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnInterstitialDownloadStart: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnInterstitialDownloadUpdate(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnInterstitialDownloadUpdate: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnInterstitialDownloadPause(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnInterstitialDownloadPause: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnInterstitialDownloadFinish(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnInterstitialDownloadFinish: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnInterstitialDownloadFail(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnInterstitialDownloadFail: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }


    private void OnInterstitialInstalled(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnInterstitialInstalled: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
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

    private void OnRewardedVideoAdVideoError(string adUnitId,string error)
    {
        Debug.Log("onRewardedVideoAdVideoError: " + adUnitId + "error" + error);
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

    private void OneRewardedVideoLayerStartLoad(string tpAdInfo)
    {
        Debug.Log("OneRewardedVideoLayerStartLoad: " + tpAdInfo);
    }

    private void OnRewardedVideoAdStartLoad(string tpAdInfo)
    {
        Debug.Log("OnRewardedVideoAdStartLoad: " + tpAdInfo);
    }

    private void OnRewardedVideoBiddingStart(string tpAdInfo)
    {
        Debug.Log("onRewardedVideoBiddingStart: " + tpAdInfo);
    }

    private void OnRewardedVideoBiddingEnd(string tpAdInfo,string error)
    {
        Debug.Log("onRewardedVideoBiddingEnd: " + tpAdInfo + "error:" + error);
    }

    private void OnRewardedVideoPlayStart(string tpAdInfo)
    {
        Debug.Log("OnRewardedVideoPlayStart: " + tpAdInfo);
    }

    private void OnRewardedVideoPlayEnd(string tpAdInfo)
    {
        Debug.Log("OnRewardedVideoPlayEnd: " + tpAdInfo);
    }

    private void OnRewardedVideoAgainImpression(string tpAdInfo)
    {
        Debug.Log("OnRewardedVideoAgainImpression: " + tpAdInfo);
    }

    private void OnRewardedVideoAgainVideoStart(string tpAdInfo)
    {
        Debug.Log("OnRewardedVideoAgainVideoStart: " + tpAdInfo);
    }

    private void OnRewardedVideoAgainVideoEnd(string tpAdInfo)
    {
        Debug.Log("OnRewardedVideoAgainVideoEnd: " + tpAdInfo);
    }

    private void OnRewardedVideoAgainVideoClicked(string tpAdInfo)
    {
        Debug.Log("OnRewardedVideoAgainVideoClicked: " + tpAdInfo);
    }

    private void OnRewardedVideoPlayAgainReward(string tpAdInfo)
    {
        Debug.Log("OnRewardedVideoPlayAgainReward: " + tpAdInfo);
    }

    private void OnRewardedVideoDownloadStart(string tpAdInfo, string networkInfo)
    {
        Debug.Log("OnRewardedVideoDownloadStart: " + tpAdInfo + ", networkInfo :" + networkInfo);
    }

    private void OnRewardedVideoDownloadUpdate(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnRewardedVideoDownloadUpdate: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnRewardedVideoDownloadPause(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnRewardedVideoDownloadPause: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnRewardedVideoDownloadFinish(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnRewardedVideoDownloadFinish: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }

    private void OnRewardedVideoDownloadFail(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnRewardedVideoDownloadFail: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
    }


    private void OnRewardedVideoInstalled(string tpAdInfo, string networkinfo)
    {
        Debug.Log("OnRewardedVideoInstalled: tpAdInfo :" + tpAdInfo + ", networkinfo :" + networkinfo);
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

    private void OneOfferWallLayerStartLoad(string tpAdInfo)
    {
        Debug.Log("OneOfferWallLayerStartLoad: " + tpAdInfo);
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

    private void OnCurrencyBalanceSuccess(string amout, string msg)
    {
        Debug.Log("OnCurrencyBalanceSuccess: amout :" + amout + ", msg :" + msg);
    }

    private void OnAwardCurrencySuccess(string amout, string msg)
    {
        Debug.Log("OnAwardCurrencySuccess: amout :" + amout + ", msg :" + msg);
    }

    private void OnSpendCurrencySuccess(string amout, string msg)
    {
        Debug.Log("OnSpendCurrencySuccess: amout :" + amout + ", msg :" + msg);
    }

    private void OnAwardCurrencyFailed(string msg)
    {
        Debug.Log("OnAwardCurrencyFailed:  msg :" + msg);
    }

    private void OnSpendCurrencyFailed(string msg)
    {
        Debug.Log("OnSpendCurrencyFailed: msg :" + msg);
    }

    private void OnCurrencyBalanceFailed(string msg)
    {
        Debug.Log("OnCurrencyBalanceFailed: msg :" + msg);
    }

    private void OnSetUserIdSuccess(string msg)
    {
        Debug.Log("OnSetUserIdSuccess: +  msg :" + msg);
    }

    private void OnSetUserIdFailed(string msg)
    {
        Debug.Log("OnSetUserIdFailed: +  msg :" + msg);
    }

    private void OnOfferWallAdShowFailed(string tpAdInfo, string error)
    {
        Debug.Log("OnOfferWallAdShowFailed: tpAdInfo:" + tpAdInfo + " error :" + error);
    }

    private void OnOfferWallAdStartLoad(string tpAdInfo)
    {
        Debug.Log("OnOfferWallAdStartLoad: " + tpAdInfo);
    }

    private void OnCheckCurrentAreaSuccess( bool isCn, bool isCA, bool isEU)
    {
        Debug.Log("OnCheckCurrentAreaSuccess: isCn = " + isCn + ", isCA = "+ isCA + ", isEU = " + isEU);
    }

    private void OnCheckCurrentAreaFailed(string error)
    {
        Debug.Log("OnCheckCurrentAreaFailed: error = " + error);
    }

}
