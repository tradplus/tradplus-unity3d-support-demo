using System;
using System.Collections.Generic;
using System.Linq;
using TradPlusInternal.ThirdParty.MiniJSON;
using UnityEngine;

public class TradPlusManager : MonoBehaviour
{
    public static TradPlusManager Instance { get; private set; }

    // Fired when the SDK has finished initializing
    public static event Action<string> OnSdkInitializedEvent;

    // Fired when an ad loads in the banner. Includes the ad height.
    public static event Action<string, float, string> OnAdLoadedEvent;

    // Fired when an ad fails to load for the banner
    public static event Action<string, string> OnAdFailedEvent;

    // Android only. Fired when a banner ad is clicked
    public static event Action<string> OnAdClickedEvent;

    // Android only. Fired when a banner ad collapses back to its initial size
    public static event Action<string> OnAdCollapsedEvent;

    // Fired when an interstitial ad is loaded and ready to be shown
    public static event Action<string> OnInterstitialLoadedEvent;

    // Fired when an interstitial ad fails to load
    public static event Action<string, string> OnInterstitialFailedEvent;

    // Fired when an interstitial ad is dismissed
    public static event Action<string> OnInterstitialDismissedEvent;

    // iOS only. Fired when an interstitial ad expires
    public static event Action<string> OnInterstitialExpiredEvent;

    // Android only. Fired when an interstitial ad is displayed
    public static event Action<string, string> OnInterstitialShownEvent;

    // Android only. Fired when an interstitial ad is clicked
    public static event Action<string> OnInterstitialClickedEvent;

    public static event Action<bool, string> OnInterstitialAllLoadedEvent;

    public static event Action<string, string, int> OnRewardedVideoReceivedRewardEvent;

    // Rewarded Videos

    public static event Action<string> OnRewardedVideoLoadedEvent;
    public static event Action<string, string> OnRewardedVideoFailedEvent;
    public static event Action<string> OnRewardedVideoDismissedEvent;
    public static event Action<string, string> OnRewardedVideoShownEvent;
    public static event Action<string> OnRewardedVideoClickedEvent;
    public static event Action<bool, string> OnRewardedVideoAllLoadedEvent;

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


    // Banner Listeners


    public void EmitAdLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 3);
        var adUnitId = args[0];
        var heightStr = args[1];
        var channelName = args[2];

        var evt = OnAdLoadedEvent;
        if (evt != null) evt(adUnitId, float.Parse(heightStr), channelName);
    }


    public void EmitAdFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var error = args[1];

        var evt = OnAdFailedEvent;
        if (evt != null) evt(adUnitId, error);
    }


    public void EmitAdClickedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        var evt = OnAdClickedEvent;
        if (evt != null) evt(adUnitId);
    }

    public void EmitAdCollapsedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        var evt = OnAdCollapsedEvent;
        if (evt != null) evt(adUnitId);
    }


    // Interstitial Listeners


    public void EmitInterstitialLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        var evt = OnInterstitialLoadedEvent;
        if (evt != null) evt(adUnitId);
    }


    public void EmitInterstitialFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var error = args[1];

        var evt = OnInterstitialFailedEvent;
        if (evt != null) evt(adUnitId, error);
    }


    public void EmitInterstitialDismissedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        var evt = OnInterstitialDismissedEvent;
        if (evt != null) evt(adUnitId);
    }


    public void EmitInterstitialDidExpireEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        var evt = OnInterstitialExpiredEvent;
        if (evt != null) evt(adUnitId);
    }


    public void EmitInterstitialShownEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var channelName = args[1];

        var evt = OnInterstitialShownEvent;
        if (evt != null) evt(adUnitId, channelName);
    }


    public void EmitInterstitialClickedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        var evt = OnInterstitialClickedEvent;
        if (evt != null) evt(adUnitId);
    }

    public void EmitInterstitialAllLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var isLoadedSuccess = args[0];
        var adUnitId = args[1];

        var evt = OnInterstitialAllLoadedEvent;
        if (evt != null) evt(bool.Parse(isLoadedSuccess), adUnitId);
    }


    // Rewarded Video Listeners

    public void EmitRewardedVideoLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        var evt = OnRewardedVideoLoadedEvent;
        if (evt != null) evt(adUnitId);
    }


    public void EmitRewardedVideoFailedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var error = args[1];

        var evt = OnRewardedVideoFailedEvent;
        if (evt != null) evt(adUnitId, error);
    }


    public void EmitRewardedVideoDismissedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        var evt = OnRewardedVideoDismissedEvent;
        if (evt != null) evt(adUnitId);
    }


    public void EmitRewardedVideoShownEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var adUnitId = args[0];
        var channelName = args[1];

        var evt = OnRewardedVideoShownEvent;
        if (evt != null) evt(adUnitId, channelName);
    }


    public void EmitRewardedVideoClickedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 1);
        var adUnitId = args[0];

        var evt = OnRewardedVideoClickedEvent;
        if (evt != null) evt(adUnitId);
    }

    public void EmitRewardedVideoReceivedRewardEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 3);
        var adUnitId = args[0];
        var currencyName = args[1];
        var amount = args[2];

        var evt = OnRewardedVideoReceivedRewardEvent;
        if (evt != null) evt(adUnitId, currencyName, int.Parse(amount));
    }

    public void EmitRewardedVideoAllLoadedEvent(string argsJson)
    {
        var args = DecodeArgs(argsJson, min: 2);
        var isLoadedSuccess = args[0];
        var adUnitId = args[1];

        var evt = OnRewardedVideoAllLoadedEvent;
        if (evt != null) evt(bool.Parse(isLoadedSuccess), adUnitId);
    }



}
