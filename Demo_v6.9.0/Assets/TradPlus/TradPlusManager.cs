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
    public static event Action<bool, string> OnAdAllLoaded;
    public static event Action<string, string> OneLayerLoadFailed;
    public static event Action<string> OneLayerLoaded;
    public static event Action<string> OnLoadAdStart;
    public static event Action<string> OnBiddingStart;
    public static event Action<string> OnBiddingEnd;

    //native
    public static event Action<string, float> OnNativeAdLoaded;
    public static event Action<string, string> OnNativeAdLoadFailed;
    public static event Action<string> OnNativeAdClicked;
    public static event Action<string> OnNativeAdImpression;
    public static event Action<string> OnNativeAdClosed;
    public static event Action<bool, string> OnNativeAdAllLoaded;
    public static event Action<string, string> OneNativeLayerLoadFailed;
    public static event Action<string> OneNativeLayerLoaded;
    public static event Action<string> OnNativeLoadAdStart;
    public static event Action<string> OnNativeBiddingStart;
    public static event Action<string> OnNativeBiddingEnd;

    //nativebanner
    public static event Action<string, float> OnNativeBannerAdLoaded;
    public static event Action<string, string> OnNativeBannerAdLoadFailed;
    public static event Action<string> OnNativeBannerAdClicked;
    public static event Action<string> OnNativeBannerAdImpression;
    public static event Action<string> OnNativeBannerAdClosed;
    public static event Action<bool, string> OnNativeBannerAdAllLoaded;
    public static event Action<string, string> OneNativeBannerLayerLoadFailed;
    public static event Action<string> OneNativeBannerLayerLoaded;
    public static event Action<string> OnNativeBannerLoadAdStart;
    public static event Action<string> OnNativeBannerBiddingStart;
    public static event Action<string> OnNativeBannerBiddingEnd;

    // Interstiatial
    // iOS only. Fired when an interstitial ad expires
    public static event Action<string> OnInterstitialExpiredEvent;

    public static event Action<string> OnInterstitialAdLoaded;
    public static event Action<string, string> OnInterstitialAdFailed;
    public static event Action<string> OnInterstitialAdImpression;
    public static event Action<string> OnInterstitialAdClosed;
    public static event Action<string> OnInterstitialAdClicked;
    public static event Action<bool, string> OnInterstitialAdAllLoaded;
    public static event Action<string, string> OneInterstitialLayerLoadFailed;
    public static event Action<string> OneInterstitialLayerLoaded;
    public static event Action<string> OnInterstitialLoadAdStart;
    public static event Action<string> OnInterstitialBiddingStart;
    public static event Action<string> OnInterstitialBiddingEnd;

    //RewardedVideo
    public static event Action<string> OnRewardedVideoAdLoaded;
    public static event Action<string, string> OnRewardedVideoAdFailed;
    public static event Action<string> OnRewardedVideoAdImpression;
    public static event Action<string> OnRewardedVideoAdClosed;
    public static event Action<string> OnRewardedVideoAdClicked;
    public static event Action<string> OnRewardedVideoAdReward;
    public static event Action<string> OnRewardedVideoAdVideoError;
    public static event Action<bool, string> OnRewardedVideoAdAllLoaded;
    public static event Action<string, string> OneRewardedVideoLayerLoadFailed;
    public static event Action<string> OneRewardedVideoLayerLoaded;
    public static event Action<string> OnRewardedVideoLoadAdStart;
    public static event Action<string> OnRewardedVideoBiddingStart;
    public static event Action<string> OnRewardedVideoBiddingEnd;


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
    public static event Action<string> OnOfferWallLoadAdStart;
    public static event Action<string> OnOfferWallBiddingStart;
    public static event Action<string> OnOfferWallBiddingEnd;


    //GDPR
    public static event Action<string> OnGDPRSuccessEvent;
    public static event Action<string> OnGDPRFailedEvent;


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

    public void EmitOnLoadAdStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnLoadAdStart;
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
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnBiddingEnd;
        if (evt != null) evt(tpAdInfo);
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

    public void EmitOnInterstitialLoadAdStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnInterstitialLoadAdStart;
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
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnInterstitialBiddingEnd;
        if (evt != null) evt(tpAdInfo);
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

        var evt = OnRewardedVideoAdVideoError;
        if (evt != null) evt(tpAdInfo);
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

    public void EmitOnRewardedVideoLoadAdStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoLoadAdStart;
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
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnRewardedVideoBiddingEnd;
        if (evt != null) evt(tpAdInfo);
    }



    // OfferWall Listeners
    public void EmitOnOfferWallAdLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnOfferWallAdLoaded;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitnOfferWallAdFailedEvent(string argsJson)
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

    public void EmitOnOfferWallLoadAdStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnOfferWallLoadAdStart;
        if (evt != null) evt(tpAdInfo);
    }

    public void EmitOnOfferWallBiddingStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnOfferWallBiddingStart;
        if (evt != null) evt(tpAdInfo);
    }


    public void EmitOnOfferWallBiddingEndEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnOfferWallBiddingEnd;
        if (evt != null) evt(tpAdInfo);
    }

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

    public void EmitOnNativeLoadAdStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeLoadAdStart;
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
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeBiddingEnd;
        if (evt != null) evt(tpAdInfo);
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

    public void EmitOnNativeBannerLoadAdStartEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeBannerLoadAdStart;
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
        var args = DecodeArgs(argsJson, min: 1);
        var tpAdInfo = args[0];

        var evt = OnNativeBannerBiddingEnd;
        if (evt != null) evt(tpAdInfo);
    }
}
