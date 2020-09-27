using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UnityEngine;
using MP = TradPlusBinding;


/// <summary>
/// This class serves as a bridge to the TradPlus iOS SDK (via the TradPlus Unity iOS wrapper).
/// For full API documentation, See <see cref="TradPlusUnityEditor"/>.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class TradPlusiOS : TradPlusBase
{
    static TradPlusiOS()
    {
        InitManager();
    }


    private static readonly Dictionary<string, MP> PluginsDict = new Dictionary<string, MP>();


    #region SdkSetup

    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.InitializeSdk(string)"/>
    public static void InitializeSdk(string anyAdUnitId)
    {
        ValidateAdUnitForSdkInit(anyAdUnitId);
        InitializeSdk(new SdkConfiguration { AdUnitId = anyAdUnitId });
    }


    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.InitializeSdk(TradPlusBase.SdkConfiguration)"/>
    public static void InitializeSdk(SdkConfiguration sdkConfiguration)
    {
        ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
        _tradplusInitializeSdk(sdkConfiguration.AdUnitId);
    }

    ///GDPR
    public static void showUploadDataNotifyDialog(string url = "")
    {
        _tradplusShowUploadDataNotifyDialog(url);
    }


    public static void setGDPRUploadDataLevel(int level)
    {
        _tradplusSetGDPRUploadDataLevel(level);
    }


    public static int getGDPRUploadDataLevel()
    {
        return _tradplusGetGDPRUploadDataLevel();
    }


    public static bool isEUTraffic()
    {
        return _tradplusIsEUTraffic();
    }


    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.LoadBannerPluginsForAdUnits(string[])"/>
    public static void LoadBannerPluginsForAdUnits(string adUnitId)
    {
        LoadPluginsForAdUnits(adUnitId);
    }


    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.LoadInterstitialPluginsForAdUnits(string[])"/>
    public static void LoadInterstitialPluginsForAdUnits(string adUnitId)
    {
        LoadPluginsForAdUnits(adUnitId);
    }


    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.LoadRewardedVideoPluginsForAdUnits(string[])"/>
    public static void LoadRewardedVideoPluginsForAdUnits(string adUnitId)
    {
        LoadPluginsForAdUnits(adUnitId);
    }

    public static void LoadOfferWallPluginsForAdUnits(string adUnitId)
    {
        LoadPluginsForAdUnits(adUnitId);
    }

    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.IsSdkInitialized"/>
    public static bool IsSdkInitialized {
        get { return true; }
    }
		
    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.GetSdkName"/>
    protected static string GetSdkName()
    {
        return "iOS SDK v" + _tradplusGetSDKVersion();
    }


    private static void LoadPluginsForAdUnits(string adUnitId)
    {
        PluginsDict.Add(adUnitId, new MP(adUnitId));
        Debug.Log(" AdUnit loaded for plugins:\n" + adUnitId);
    }

    #endregion SdkSetup


    #region Banners


    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.CreateBanner(string,TradPlusBase.AdPosition,TradPlusBase.BannerType)"/>
    public static void CreateBanner(string adUnitId, AdPosition position)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateBanner(position);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.ShowBanner(string,bool)"/>
    public static void ShowBanner(string adUnitId, bool shouldShow)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowBanner(shouldShow);
        else
            ReportAdUnitNotFound(adUnitId);
    }
		
    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.ForceRefresh(string)"/>
    public static void DestroyBanner(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyBanner();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void ShowBannerConfirmUWSAd(string adUnitId)
    {
        // MP plugin;
        // if (PluginsDict.TryGetValue(adUnitId, out plugin))
        //     plugin.ShowBannerConfirmUWSAd();
        // else
        //     ReportAdUnitNotFound(adUnitId);
    }

    #endregion Banners


    #region Interstitials


    public static void RequestInterstitialAd(string adUnitId, bool autoReload = false, bool isPangleTemplateRender = false)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestInterstitialAd(autoReload,isPangleTemplateRender);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void loadForcedlyInterstitial(string adUnitId)
    {
        // MP plugin;
        // if (PluginsDict.TryGetValue(adUnitId, out plugin))
        //     plugin.loadForcedlyInterstitial();
        // else
        //     ReportAdUnitNotFound(adUnitId);
    }


    public static void ShowInterstitialAd(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowInterstitialAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void ShowInterstitialConfirmUWSAd(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowInterstitialConfirmUWSAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    public static bool IsInterstialReady(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.IsInterstitialReady;
        ReportAdUnitNotFound(adUnitId);
        return false;
    }


    public static void DestroyInterstitialAd(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyInterstitialAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    #endregion Interstitials


    #region RewardedVideos


	public static void RequestRewardedVideo(string adUnitId, bool autoReload = false, bool isPangleTemplateRender = false)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
			plugin.RequestRewardedVideo(autoReload,isPangleTemplateRender);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void loadForcedlyRewardedVideo(string adUnitId)
    {
        // MP plugin;
        // if (PluginsDict.TryGetValue(adUnitId, out plugin))
        //     plugin.loadForcedlyRewardedVideo();
        // else
        //     ReportAdUnitNotFound(adUnitId);
    }

    public static void ShowRewardedVideo(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowRewardedVideo();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void ShowRewardedVideoConfirmUWSAd(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowRewardedVideoConfirmUWSAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static bool HasRewardedVideo(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.HasRewardedVideo();
        ReportAdUnitNotFound(adUnitId);
        return false;
    }

    #endregion RewardedVideos



    #region Offerwall

	public static void RequestOfferWall(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestOfferWall();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void ShowOfferWall(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowOfferWall();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void ShowOfferWallConfirmUWSAd(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowOfferWallConfirmUWSAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    public static bool HasOfferWall(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.HasOfferWall();
        ReportAdUnitNotFound(adUnitId);
        return false;
    }

    #endregion Offerwall

    #region DllImports
#if ENABLE_IL2CPP && UNITY_ANDROID
    // IL2CPP on Android scrubs DllImports, so we need to provide stubs to unblock compilation
    private static void _tradplusInitializeSdk(string adUnitId) {}
    private static bool _tradplusIsSdkInitialized() { return false; }
    private static void _tradplusSetAdvancedBiddingEnabled(bool advancedBiddingEnabled) {}
    private static bool _tradplusIsAdvancedBiddingEnabled() { return false; }
    private static string _tradplusGetSDKVersion() { return null; }
    private static void _tradplusEnableLocationSupport(bool shouldUseLocation) {}
    private static int _tradplusGetLogLevel() { return -1; }
    private static void _tradplusSetLogLevel(int logLevel) {}
    private static void _tradplusForceWKWebView(bool shouldForce) {}
    private static void _tradplusReportApplicationOpen(string iTunesAppId) {}
    private static bool _tradplusCanCollectPersonalInfo() { return false; }
    private static void _tradplusShowUploadDataNotifyDialog(string url) {}
    private static bool _tradplusIsEUTraffic(){return false;}
    private static int _tradplusGetGDPRUploadDataLevel(){return 0;}
    private static void _tradplusSetGDPRUploadDataLevel(int level) {}

#else
    [DllImport("__Internal")]
    private static extern void _tradplusInitializeSdk(string adUnitId);

    [DllImport("__Internal")]
    private static extern string _tradplusGetSDKVersion();

    [DllImport("__Internal")]
    private static extern void _tradplusShowUploadDataNotifyDialog(string url);

    [DllImport("__Internal")]
    private static extern void _tradplusSetGDPRUploadDataLevel(int level);

    [DllImport("__Internal")]
    private static extern int _tradplusGetGDPRUploadDataLevel();

    [DllImport("__Internal")]
    private static extern bool _tradplusIsEUTraffic();

#endif
    #endregion DllImports
}
