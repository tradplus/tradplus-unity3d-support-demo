using System;
using System.Collections.Generic;
using System.Linq;
using TradPlusInternal.ThirdParty.MiniJSON;
using UnityEngine;

public class TradPlusManager : MonoBehaviour
{
    public static TradPlusManager Instance { get; private set; }


    public static event Action<string> OnSdkInitializedEvent;

    // Banner
    public static event Action<string, float> OnAdLoaded;
    public static event Action<string, string> OnAdLoadFailed;
    public static event Action<string> OnAdClicked;
    public static event Action<string> OnAdImpression;
    public static event Action<string> OnAdClosed;
    public static event Action<string, string> OnAdShowFailed;
    public static event Action<bool, string> OnAdAllLoaded;
    public static event Action<string, string> OneLayerLoadFailed;
    public static event Action<string> OneLayerLoaded;
    public static event Action<string> OnBiddingStart;
    public static event Action<string, string> OnBiddingEnd;

    // 8.1 新增 快手 头条 下载类监听回调
    public static event Action<string, string> OnDownloadStart;
    public static event Action<string, string> OnDownloadUpdate;
    public static event Action<string, string> OnDownloadPause;
    public static event Action<string, string> OnDownloadFinish;
    public static event Action<string, string> OnDownloadFail;
    public static event Action<string, string> OnInstalled;

    //每一个源开始加载时返回
    //原：public static event Action<string> OnLoadAdStart; 请使用 OneLayerStartLoad
    public static event Action<string> OneLayerStartLoad;
    //load开始时
    public static event Action<string> OnAdStartLoad;

    //native
    public static event Action<string, float> OnNativeAdLoaded;
    public static event Action<string, string> OnNativeAdLoadFailed;
    public static event Action<string> OnNativeAdClicked;
    public static event Action<string> OnNativeAdImpression;
    public static event Action<string> OnNativeAdClosed;
    public static event Action<string, string> OnNativeAdShowFailed;
    public static event Action<bool, string> OnNativeAdAllLoaded;
    public static event Action<string, string> OneNativeLayerLoadFailed;
    public static event Action<string> OneNativeLayerLoaded;
    public static event Action<string> OnNativeBiddingStart;
    public static event Action<string, string> OnNativeBiddingEnd;
    public static event Action<string> OnNativeAdVideoPlayStart; //Android 8.1 新增
    public static event Action<string> OnNativeAdVideoPlayEnd; //Android 8.1 新增

    // 8.1 新增 快手 头条 下载类监听回调
    public static event Action<string, string> OnNativeDownloadStart;
    public static event Action<string, string> OnNativeDownloadUpdate;
    public static event Action<string, string> OnNativeDownloadPause;
    public static event Action<string, string> OnNativeDownloadFinish;
    public static event Action<string, string> OnNativeDownloadFail;
    public static event Action<string, string> OnNativeInstalled;

    //每一个源开始加载时返回
    //原：public static event Action<string> OnNativeLoadAdStart; 请使用 OneNativeLayerStartLoad
    public static event Action<string> OneNativeLayerStartLoad;
    //load开始时 一次加载返回一次
    public static event Action<string> OnNativeAdStartLoad;

    //nativebanner
    public static event Action<string, float> OnNativeBannerAdLoaded;
    public static event Action<string, string> OnNativeBannerAdLoadFailed;
    public static event Action<string> OnNativeBannerAdClicked;
    public static event Action<string> OnNativeBannerAdImpression;
    public static event Action<string> OnNativeBannerAdClosed;
    public static event Action<string, string> OnNativeBannerAdShowFailed;
    public static event Action<bool, string> OnNativeBannerAdAllLoaded;
    public static event Action<string, string> OneNativeBannerLayerLoadFailed;
    public static event Action<string> OneNativeBannerLayerLoaded;
    public static event Action<string> OnNativeBannerBiddingStart;
    public static event Action<string, string> OnNativeBannerBiddingEnd;


    // 8.1 新增 快手 头条 下载类监听回调
    public static event Action<string, string> OnNativeBannerDownloadStart;
    public static event Action<string, string> OnNativeBannerDownloadUpdate;
    public static event Action<string, string> OnNativeBannerDownloadPause;
    public static event Action<string, string> OnNativeBannerDownloadFinish;
    public static event Action<string, string> OnNativeBannerDownloadFail;
    public static event Action<string, string> OnNativeBannerInstalled;

    //每一个源开始加载时返回
    //原：public static event Action<string> OnNativeBannerLoadAdStart; 请使用 OneNativeBannerLayerStartLoad
    public static event Action<string> OneNativeBannerLayerStartLoad;
    //load开始时 一次加载返回一次
    public static event Action<string> OnNativeBannerAdStartLoad;


    // Interstiatial
    // iOS only. Fired when an interstitial ad expires
    public static event Action<string> OnInterstitialExpiredEvent;

    public static event Action<string> OnInterstitialAdLoaded;
    public static event Action<string, string> OnInterstitialAdFailed;
    public static event Action<string> OnInterstitialAdImpression;
    public static event Action<string> OnInterstitialAdClosed;
    public static event Action<string> OnInterstitialAdClicked;
    public static event Action<string, string> OnInterstitialAdVideoError;
    public static event Action<bool, string> OnInterstitialAdAllLoaded;
    public static event Action<string, string> OneInterstitialLayerLoadFailed;
    public static event Action<string> OneInterstitialLayerLoaded;
    public static event Action<string> OnInterstitialBiddingStart;
    public static event Action<string, string> OnInterstitialBiddingEnd;
    public static event Action<string> OnInterstitialVideoPlayStart;//Android 8.1 新增
    public static event Action<string> OnInterstitialVideoPlayEnd;//Android 8.1 新增

    // 8.1 新增 快手 头条 下载类监听回调
    public static event Action<string, string> OnInterstitialDownloadStart;
    public static event Action<string, string> OnInterstitialDownloadUpdate;
    public static event Action<string, string> OnInterstitialDownloadPause;
    public static event Action<string, string> OnInterstitialDownloadFinish;
    public static event Action<string, string> OnInterstitialDownloadFail;
    public static event Action<string, string> OnInterstitialInstalled;

    //每一个源开始加载时返回
    //原：public static event Action<string> OnInterstitialLoadAdStart; 请使用 OneInterstitialLayerStartLoad
    public static event Action<string> OneInterstitialLayerStartLoad;
    //load开始时 一次加载返回一次
    public static event Action<string> OnInterstitialAdStartLoad;

    //RewardedVideo
    public static event Action<string> OnRewardedVideoAdLoaded;
    public static event Action<string, string> OnRewardedVideoAdFailed;
    public static event Action<string> OnRewardedVideoAdImpression;
    public static event Action<string> OnRewardedVideoAdClosed;
    public static event Action<string> OnRewardedVideoAdClicked;
    public static event Action<string> OnRewardedVideoAdReward;
    public static event Action<string, string> OnRewardedVideoAdVideoError;
    public static event Action<bool, string> OnRewardedVideoAdAllLoaded;
    public static event Action<string, string> OneRewardedVideoLayerLoadFailed;
    public static event Action<string> OneRewardedVideoLayerLoaded;
    public static event Action<string> OnRewardedVideoBiddingStart;
    public static event Action<string, string> OnRewardedVideoBiddingEnd;
    public static event Action<string> OnRewardedVideoPlayStart;// 8.1 新增 视频播放开始
    public static event Action<string> OnRewardedVideoPlayEnd;// 8.1 新增 视频播放关闭

    public static event Action<string> OnRewardedVideoAgainImpression; // 8.1 新增 再看一次 展示
    public static event Action<string> OnRewardedVideoAgainVideoStart;  // 8.1 新增 再看一次 视频播放开始
    public static event Action<string> OnRewardedVideoAgainVideoEnd;  // 8.1 新增 再看一次 视频播放结束
    public static event Action<string> OnRewardedVideoAgainVideoClicked;  // 8.1 新增 再看一次 点击

    // 8.1 新增 快手 头条 下载类监听回调
    public static event Action<string, string> OnRewardedVideoDownloadStart;
    public static event Action<string, string> OnRewardedVideoDownloadUpdate;
    public static event Action<string, string> OnRewardedVideoDownloadPause;
    public static event Action<string, string> OnRewardedVideoDownloadFinish;
    public static event Action<string, string> OnRewardedVideoDownloadFail;
    public static event Action<string, string> OnRewardedVideoInstalled;

    //此回调已废弃 请使用 OnRewardedVideoPlayAgainReward
    //public static event Action<string> OnAdPlayAgainReward;
    public static event Action<string> OnRewardedVideoPlayAgainReward;  // 再看一次 奖励

    //每一个源开始加载时返回
    //原：public static event Action<string> OnRewardedVideoLoadAdStart; 请使用 OneRewardedVideoLayerStartLoad
    public static event Action<string> OneRewardedVideoLayerStartLoad;
    //load开始时 一次加载返回一次
    public static event Action<string> OnRewardedVideoAdStartLoad;


    //OfferWall
    public static event Action<string> OnOfferWallAdLoaded;
    public static event Action<string, string> OnOfferWallAdFailed;
    public static event Action<string> OnOfferWallAdImpression;
    public static event Action<string> OnOfferWallAdClosed;
    public static event Action<string> OnOfferWallAdClicked;
    public static event Action<string> OnOfferWallAdReward;
    public static event Action<bool, string> OnOfferWallAdAllLoaded;
    public static event Action<string, string> OneOfferWallLayerLoadFailed;
    public static event Action<string> OneOfferWallLayerLoaded;
    
    //每一个源开始加载时返回
    //原：public static event Action<string> OnOfferWallLoadAdStart; 请使用 OneOfferWallLayerStartLoad
    public static event Action<string> OneOfferWallLayerStartLoad;
    //load开始时 一次加载返回一次
    public static event Action<string> OnOfferWallAdStartLoad;

    public static event Action<string,string> OnCurrencyBalanceSuccess; //8.1 查询总额成功
    public static event Action<string> OnCurrencyBalanceFailed;//8.1 查询总额失败
    public static event Action<string, string> OnSpendCurrencySuccess; //8.1 消耗积分成功
    public static event Action<string> OnSpendCurrencyFailed;//8.1 消耗积分失败
    public static event Action<string, string> OnAwardCurrencySuccess; //8.1 增加积分成功
    public static event Action<string> OnAwardCurrencyFailed;//8.1 增加积分
    public static event Action<string> OnSetUserIdSuccess;//设置userId成功
    public static event Action<string> OnSetUserIdFailed;//设置userId失败
    //积分墙展示失败
    public static event Action<string, string> OnOfferWallAdShowFailed;

    //GDPR
    public static event Action<string> OnGDPRSuccessEvent;
    public static event Action<string> OnGDPRFailedEvent;

    // Android V8.4.0.1 ；iOS V8.0.0.1 新增
    //check area params: isCN (是否中国) , isCA(是否在加州), isEU(是否在欧洲), error 如果为空表示成功返回
    public static event Action< bool, bool, bool> OnCheckCurrentAreaSuccess;
    //查询失败时返回
    public static event Action<string> OnCheckCurrentAreaFailed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }


    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }


    // Will return a non-null array of strings with at least 'min' non-null string values at the front.
    private string[] DecodeArgs(string argsJson, int min)
    {
        bool err = false;
        var args = Json.Deserialize(argsJson) as List<object>;
        if (args == null)
        {
            Debug.LogError("Invalid JSON data: " + argsJson);
            args = new List<object>();
            err = true;
        }
        if (args.Count < min)
        {
            if (!err)  // Don't double up the error messages for invalid JSON
                Debug.LogError("Missing one or more values: " + argsJson + " (expected " + min + ")");
            while (args.Count < min)
                args.Add("");
        }
        return args.Select(v => v == null ? "null" : v.ToString()).ToArray();
    }


    public void EmitSdkInitializedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        var evt = OnSdkInitializedEvent;
        if (evt != null) evt(adUnitId);
    }

    //GDPR Listeners
    public void EmitGDPRSuccessEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var msg = args[0];

        var evt = OnGDPRSuccessEvent;
        if (evt != null) evt(msg);
    }

    public void EmitFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var error = args[0];

        var evt = OnGDPRFailedEvent;
        if (evt != null) evt(error);
    }

    // Banner Listeners
    public void EmitOnAdLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var heightStr = args[1];

        var evt = OnAdLoaded;
        if (evt != null) evt(tpAdInfo, float.Parse(heightStr));
    }


    public void EmitOnAdLoadFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var error = args[1];

        var evt = OnAdLoadFailed;
        if (evt != null) evt(adUnitId, error);
    }


    public void EmitOnAdClickedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnAdClicked;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnAdClosedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnAdClosed;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnAdShowFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OnAdShowFailed;
        if (evt != null) evt(tpAdInfo, error);
    }

    public void EmitOnAdImpressionEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnAdImpression;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnAdAllLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var isSuccess = args[0];
        var adUnitId = args[1];

        var evt = OnAdAllLoaded;
        if (evt != null) evt(bool.Parse(isSuccess), adUnitId);
    }


    public void EmitOneLayerLoadFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OneLayerLoadFailed;
        if (evt != null) evt(tpAdInfo, error);
    }


    public void EmitOneLayerLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneLayerLoaded;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOneLayerStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneLayerStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnAdStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnAdStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnBiddingStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnBiddingStart;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnBiddingEndEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OnBiddingEnd;
        if (evt != null) evt(tpAdInfo, error);
    }


    public void EmitOnDownloadStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnDownloadStart;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnDownloadUpdateEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnDownloadUpdate;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnDownloadPauseEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnDownloadPause;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnDownloadFinishEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnDownloadFinish;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnDownloadFailEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnDownloadFail;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnInstalledEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnInstalled;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    // Interstitial Listeners
    public void EmitOnInterstitialAdLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnInterstitialAdLoaded;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnInterstitialAdFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var error = args[1];

        var evt = OnInterstitialAdFailed;
        if (evt != null) evt(adUnitId, error);
    }

    public void EmitOnInterstitialAdVideoErrorEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OnInterstitialAdVideoError;
        if (evt != null) evt(tpAdInfo, error);
    }

    public void EmitOnInterstitialAdClosedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnInterstitialAdClosed;
        if (evt != null) evt(tpAdInfo);
    }

    // Only iOS
    public void EmitInterstitialDidExpireEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];
        var evt = OnInterstitialExpiredEvent;
        if (evt != null) evt(adUnitId);
    }

    public void EmitOnInterstitialAdImpressionEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnInterstitialAdImpression;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnInterstitialAdClickedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnInterstitialAdClicked;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnInterstitialAdAllLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var isLoadedSuccess = args[0];
        var adUnitId = args[1];

        var evt = OnInterstitialAdAllLoaded;
        if (evt != null) evt(bool.Parse(isLoadedSuccess), adUnitId);
    }

    public void EmitOneInterstitialLayerLoadFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OneInterstitialLayerLoadFailed;
        if (evt != null) evt(tpAdInfo, error);
    }


    public void EmitOneInterstitialLayerLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneInterstitialLayerLoaded;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOneInterstitialLayerStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneInterstitialLayerStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnInterstitialAdStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnInterstitialAdStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnInterstitialBiddingStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnInterstitialBiddingStart;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnInterstitialBiddingEndEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OnInterstitialBiddingEnd;
        if (evt != null) evt(tpAdInfo, error);
    }


    public void EmitOnInterstitialVideoPlayStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnInterstitialVideoPlayStart;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnInterstitialVideoPlayEndEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnInterstitialVideoPlayEnd;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnInterstitialDownloadStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnInterstitialDownloadStart;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnInterstitialDownloadUpdateEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnInterstitialDownloadUpdate;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnInterstitialDownloadPauseEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnInterstitialDownloadPause;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnInterstitialDownloadFinishEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnInterstitialDownloadFinish;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnInterstitialDownloadFailEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnInterstitialDownloadFail;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnInterstitialInstalledEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnInterstitialInstalled;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    // Rewarded Video Listeners
    public void EmitOnRewardedVideoAdLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        var evt = OnRewardedVideoAdLoaded;
        if (evt != null) evt(adUnitId);
    }


    public void EmitOnRewardedVideoAdFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var error = args[1];

        var evt = OnRewardedVideoAdFailed;
        if (evt != null) evt(adUnitId, error);
    }


    public void EmitOnRewardedVideoAdClosedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoAdClosed;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnRewardedVideoAdImpressionEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoAdImpression;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnRewardedVideoAdClickedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoAdClicked;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnRewardedVideoAdRewardEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoAdReward;
        if (evt != null) evt(tpAdInfo);
    }

    //旧 PlayAgainReward 回调
    public void EmitOnAdPlayAgainRewardEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoPlayAgainReward;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnRewardedVideoAgainImpressionEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoAgainImpression;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnRewardedVideoAgainVideoStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoAgainVideoStart;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnRewardedVideoAgainVideoEndEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoAgainVideoEnd;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnRewardedVideoAgainVideoClickedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoAgainVideoClicked;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnRewardedVideoPlayAgainRewardEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoPlayAgainReward;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnRewardedVideoAdAllLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var isLoadedSuccess = args[0];
        var adUnitId = args[1];

        var evt = OnRewardedVideoAdAllLoaded;
        if (evt != null) evt(bool.Parse(isLoadedSuccess), adUnitId);
    }

    public void EmitOnRewardedVideoAdVideoErrorEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OnRewardedVideoAdVideoError;
        if (evt != null) evt(tpAdInfo, error);
    }

    public void EmitOneRewardedVideoLayerLoadFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OneRewardedVideoLayerLoadFailed;
        if (evt != null) evt(tpAdInfo, error);
    }


    public void EmitOneRewardedVideoLayerLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneRewardedVideoLayerLoaded;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOneRewardedVideoLayerStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneRewardedVideoLayerStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnRewardedVideoAdStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoAdStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnRewardedVideoBiddingStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoBiddingStart;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnRewardedVideoBiddingEndEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OnRewardedVideoBiddingEnd;
        if (evt != null) evt(tpAdInfo, error);
    }

    public void EmitOnRewardedVideoPlayStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoPlayStart;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnRewardedVideoPlayEndEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoPlayEnd;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnRewardedVideoDownloadStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnRewardedVideoDownloadStart;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnRewardedVideoDownloadUpdateEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnRewardedVideoDownloadUpdate;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnRewardedVideoDownloadPauseEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnRewardedVideoDownloadPause;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnRewardedVideoDownloadFinishEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnRewardedVideoDownloadFinish;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnRewardedVideoDownloadFailEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnRewardedVideoDownloadFail;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnRewardedVideoInstalledEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnRewardedVideoInstalled;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }



    // OfferWall Listeners
    public void EmitOnOfferWallAdLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnOfferWallAdLoaded;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnOfferWallAdFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var error = args[1];

        var evt = OnOfferWallAdFailed;
        if (evt != null) evt(adUnitId, error);
    }


    public void EmitOnOfferWallAdClosedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnOfferWallAdClosed;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnOfferWallAdImpressionEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnOfferWallAdImpression;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnOfferWallAdClickedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnOfferWallAdClicked;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnOfferWallAdRewardEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnOfferWallAdReward;
        if (evt != null) evt(tpAdInfo);
    }

      public void EmitOnOfferWallAdAllLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var isLoadedSuccess = args[0];
        var adUnitId = args[1];

        var evt = OnOfferWallAdAllLoaded;
        if (evt != null) evt(bool.Parse(isLoadedSuccess), adUnitId);
    }

    public void EmitOneOfferWallLayerLoadFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OneOfferWallLayerLoadFailed;
        if (evt != null) evt(tpAdInfo, error);
    }


    public void EmitOneOfferWallLayerLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneOfferWallLayerLoaded;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOneOfferWallLayerStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneOfferWallLayerStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnOfferWallAdStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnOfferWallAdStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnCurrencyBalanceSuccessEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var amount = args[0];
        var msg = args[1];

        var evt = OnCurrencyBalanceSuccess;
        if (evt != null) evt(amount, msg);
    }

    public void EmitOnCurrencyBalanceFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var msg = args[0];

        var evt = OnCurrencyBalanceFailed;
        if (evt != null) evt(msg);
    }

    public void EmitOnSpendCurrencySuccessEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var amount = args[0];
        var msg = args[1];

        var evt = OnSpendCurrencySuccess;
        if (evt != null) evt(amount, msg);
    }

    public void EmitOnSpendCurrencyFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var msg = args[0];

        var evt = OnSpendCurrencyFailed;
        if (evt != null) evt(msg);
    }


    public void EmitOnAwardCurrencySuccessEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var amount = args[0];
        var msg = args[1];

        var evt = OnAwardCurrencySuccess;
        if (evt != null) evt(amount, msg);
    }

    public void EmitOnAwardCurrencyFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var msg = args[0];

        var evt = OnAwardCurrencyFailed;
        if (evt != null) evt(msg);
    }

    public void EmitOnSetUserIdSuccessEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var msg = args[0];

        var evt = OnSetUserIdSuccess;
        if (evt != null) evt(msg);
    }

    public void EmitOnSetUserIdFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var msg = args[0];

        var evt = OnSetUserIdFailed;
        if (evt != null) evt(msg);
    }

    public void EmitOnOfferWallAdShowFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OnOfferWallAdShowFailed;
        if (evt != null) evt(tpAdInfo, error);
    }

    //public void EmitOnOfferWallBiddingStartEvent(string argsJson)
    //{
    //    var args = DecodeArgs(argsJson, min: 1);
    //    var tpAdInfo = args[0];

    //    var evt = OnOfferWallBiddingStart;
    //    if (evt != null) evt(tpAdInfo);
    //}


    //public void EmitOnOfferWallBiddingEndEvent(string argsJson)
    //{
    //    var args = DecodeArgs(argsJson, min: 1);
    //    var tpAdInfo = args[0];

    //    var evt = OnOfferWallBiddingEnd;
    //    if (evt != null) evt(tpAdInfo);
    //}

    // Native
    public void EmitOnNativeAdLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var heightStr = args[1];

        var evt = OnNativeAdLoaded;
        if (evt != null) evt(tpAdInfo, float.Parse(heightStr));
    }


    public void EmitOnNativeAdLoadFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var error = args[1];

        var evt = OnNativeAdLoadFailed;
        if (evt != null) evt(adUnitId, error);
    }


    public void EmitOnNativeAdClickedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeAdClicked;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeAdImpressionEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeAdImpression;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeAdClosedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeAdClosed;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeAdShowFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OnNativeAdShowFailed;
        if (evt != null) evt(tpAdInfo, error);
    }

    public void EmitOnNativeAdAllLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var isSuccess = args[0];
        var adUnitId = args[1];

        var evt = OnNativeAdAllLoaded;
        if (evt != null) evt(bool.Parse(isSuccess), adUnitId);
    }

    public void EmitOneNativeLayerLoadFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OneNativeLayerLoadFailed;
        if (evt != null) evt(tpAdInfo, error);
    }


    public void EmitOneNativeLayerLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneNativeLayerLoaded;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOneNativeLayerStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneNativeLayerStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeAdStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeAdStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeBiddingStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeBiddingStart;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnNativeBiddingEndEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OnNativeBiddingEnd;
        if (evt != null) evt(tpAdInfo, error);
    }

    public void EmitOnNativeAdVideoPlayStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeAdVideoPlayStart;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeAdVideoPlayEndEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeAdVideoPlayEnd;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeDownloadStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeDownloadStart;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnNativeDownloadUpdateEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeDownloadUpdate;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnNativeDownloadPauseEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeDownloadPause;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnNativeDownloadFinishEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeDownloadFinish;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnNativeDownloadFailEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeDownloadFail;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnNativeInstalledEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeInstalled;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    // Nativebanner
    public void EmitOnNativeBannerAdLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var heightStr = args[1];

        var evt = OnNativeBannerAdLoaded;
        if (evt != null) evt(tpAdInfo, float.Parse(heightStr));
    }


    public void EmitOnNativeBannerAdLoadFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var error = args[1];

        var evt = OnNativeBannerAdLoadFailed;
        if (evt != null) evt(adUnitId, error);
    }


    public void EmitOnNativeBannerAdClickedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeBannerAdClicked;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeBannerAdImpressionEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeBannerAdImpression;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeBannerAdClosedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeBannerAdClosed;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeBannerAdShowFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OnNativeBannerAdShowFailed;
        if (evt != null) evt(tpAdInfo, error);
    }

    public void EmitOnNativeBannerAdAllLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var isSuccess = args[0];
        var adUnitId = args[1];

        var evt = OnNativeBannerAdAllLoaded;
        if (evt != null) evt(bool.Parse(isSuccess), adUnitId);
    }

    public void EmitOneNativeBannerLayerLoadFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OneNativeBannerLayerLoadFailed;
        if (evt != null) evt(tpAdInfo, error);
    }


    public void EmitOneNativeBannerLayerLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneNativeBannerLayerLoaded;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOneNativeBannerLayerStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OneNativeBannerLayerStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeBannerAdStartLoadEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeBannerAdStartLoad;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnNativeBannerBiddingStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeBannerBiddingStart;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnNativeBannerBiddingEndEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var error = args[1];

        var evt = OnNativeBannerBiddingEnd;
        if (evt != null) evt(tpAdInfo, error);
    }

    public void EmitOnNativeBannerDownloadStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeBannerDownloadStart;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnNativeBannerDownloadUpdateEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeBannerDownloadUpdate;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnNativeBannerDownloadPauseEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeBannerDownloadPause;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnNativeBannerDownloadFinishEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeBannerDownloadFinish;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnNativeBannerDownloadFailEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeBannerDownloadFail;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnNativeBannerInstalledEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var tpAdInfo = args[0];
        var networkInfo = args[1];

        var evt = OnNativeBannerInstalled;
        if (evt != null) evt(tpAdInfo, networkInfo);
    }

    public void EmitOnCheckCurrentAreaSuccessEvent(string argsJson)
    {
         var args = DecodeArgs(argsJson, min: 3);
         var isCN = args[0];
         var isCA = args[1];
         var isEU = args[2];
         var evt = OnCheckCurrentAreaSuccess;
         if (evt != null) evt(bool.Parse(isCN), bool.Parse(isCA), bool.Parse(isEU));
    }

    public void EmitOnCheckCurrentAreaFailedEvent(string argsJson)
    {
        var evt = OnCheckCurrentAreaFailed;
        if (evt != null) evt("CheckCurrentAreaFailed");
    }

}
