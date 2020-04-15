using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UnityEngine;
using MP = FluteBinding;


/// <summary>
/// This class serves as a bridge to the Flute iOS SDK (via the Flute Unity iOS wrapper).
/// For full API documentation, See <see cref="FluteUnityEditor"/>.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class FluteiOS : FluteBase
{
    static FluteiOS()
    {
        InitManager();
    }


    private static readonly Dictionary<string, MP> PluginsDict = new Dictionary<string, MP>();


    #region SdkSetup

    /// See FluteUnityEditor.<see cref="FluteUnityEditor.InitializeSdk(string)"/>
    public static void InitializeSdk(string anyAdUnitId)
    {
        ValidateAdUnitForSdkInit(anyAdUnitId);
        InitializeSdk(new SdkConfiguration { AdUnitId = anyAdUnitId });
    }


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.InitializeSdk(FluteBase.SdkConfiguration)"/>
    public static void InitializeSdk(SdkConfiguration sdkConfiguration)
    {
        ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
        _fluteInitializeSdk(sdkConfiguration.AdUnitId);
    }


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.LoadBannerPluginsForAdUnits(string[])"/>
    public static void LoadBannerPluginsForAdUnits(string adUnitId)
    {
        LoadPluginsForAdUnits(adUnitId);
    }


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.LoadInterstitialPluginsForAdUnits(string[])"/>
    public static void LoadInterstitialPluginsForAdUnits(string adUnitId)
    {
        LoadPluginsForAdUnits(adUnitId);
    }


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.LoadRewardedVideoPluginsForAdUnits(string[])"/>
    public static void LoadRewardedVideoPluginsForAdUnits(string adUnitId)
    {
        LoadPluginsForAdUnits(adUnitId);
    }

    /// See FluteUnityEditor.<see cref="FluteUnityEditor.IsSdkInitialized"/>
    public static bool IsSdkInitialized {
        get { return true; }
    }
		
    /// See FluteUnityEditor.<see cref="FluteUnityEditor.GetSdkName"/>
    protected static string GetSdkName()
    {
        return "iOS SDK v" + _fluteGetSDKVersion();
    }


    private static void LoadPluginsForAdUnits(string adUnitId)
    {
        PluginsDict.Add(adUnitId, new MP(adUnitId));
        Debug.Log(" AdUnit loaded for plugins:\n" + adUnitId);
    }

    #endregion SdkSetup


    #region Banners


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.CreateBanner(string,FluteBase.AdPosition,FluteBase.BannerType)"/>
    public static void CreateBanner(string adUnitId, AdPosition position)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateBanner(position);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.ShowBanner(string,bool)"/>
    public static void ShowBanner(string adUnitId, bool shouldShow)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowBanner(shouldShow);
        else
            ReportAdUnitNotFound(adUnitId);
    }
		
    /// See FluteUnityEditor.<see cref="FluteUnityEditor.ForceRefresh(string)"/>
    public static void DestroyBanner(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyBanner();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    #endregion Banners


    #region Interstitials


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.RequestInterstitialAd(string,string,string)"/>
    public static void RequestInterstitialAd(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestInterstitialAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.ShowInterstitialAd(string)"/>
    public static void ShowInterstitialAd(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowInterstitialAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.IsInterstialReady(string)"/>
    public bool IsInterstialReady(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.IsInterstitialReady;
        ReportAdUnitNotFound(adUnitId);
        return false;
    }


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.DestroyInterstitialAd(string)"/>
    public void DestroyInterstitialAd(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyInterstitialAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    #endregion Interstitials


    #region RewardedVideos


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.RequestRewardedVideo(string,System.Collections.Generic.List{FluteBase.MediationSetting},string,string,double,double,string)"/>
	public static void RequestRewardedVideo(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
			plugin.RequestRewardedVideo();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.ShowRewardedVideo(string,string)"/>
    public static void ShowRewardedVideo(string adUnitId, string customData = null)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
			plugin.ShowRewardedVideo();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    /// See FluteUnityEditor.<see cref="FluteUnityEditor.HasRewardedVideo(string)"/>
    public static bool HasRewardedVideo(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.HasRewardedVideo();
        ReportAdUnitNotFound(adUnitId);
        return false;
    }

    #endregion RewardedVideos

    #region DllImports
#if ENABLE_IL2CPP && UNITY_ANDROID
    // IL2CPP on Android scrubs DllImports, so we need to provide stubs to unblock compilation
    private static void _fluteInitializeSdk(string adUnitId) {}
    private static bool _fluteIsSdkInitialized() { return false; }
    private static void _fluteSetAdvancedBiddingEnabled(bool advancedBiddingEnabled) {}
    private static bool _fluteIsAdvancedBiddingEnabled() { return false; }
    private static string _fluteGetSDKVersion() { return null; }
    private static void _fluteEnableLocationSupport(bool shouldUseLocation) {}
    private static int _fluteGetLogLevel() { return -1; }
    private static void _fluteSetLogLevel(int logLevel) {}
    private static void _fluteForceWKWebView(bool shouldForce) {}
    private static void _fluteReportApplicationOpen(string iTunesAppId) {}
    private static bool _fluteCanCollectPersonalInfo() { return false; }
#else
    [DllImport("__Internal")]
    private static extern void _fluteInitializeSdk(string adUnitId);

    [DllImport("__Internal")]
    private static extern string _fluteGetSDKVersion();

#endif
    #endregion DllImports
}
