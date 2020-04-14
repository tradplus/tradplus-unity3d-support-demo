using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using MPBanner = TradPlusAndroidBanner;
using MPInterstitial = TradPlusAndroidInterstitial;
using MPRewardedVideo = TradPlusAndroidRewardedVideo;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class TradPlusAndroid : TradPlusBase
{
    static TradPlusAndroid()
    {
        InitManager();
    }

    private static readonly AndroidJavaClass PluginClass = new AndroidJavaClass("com.tradplus.ads.unity.TradplusUnityPlugin");


    private static readonly Dictionary<string, MPBanner> BannerPluginsDict = new Dictionary<string, MPBanner>();

    private static readonly Dictionary<string, MPInterstitial> InterstitialPluginsDict =
        new Dictionary<string, MPInterstitial>();

    private static readonly Dictionary<string, MPRewardedVideo> RewardedVideoPluginsDict =
        new Dictionary<string, MPRewardedVideo>();


    #region SdkSetup

    public static void InitializeSdk(string anyAdUnitId)
    {
        ValidateAdUnitForSdkInit(anyAdUnitId);
        InitializeSdk(new SdkConfiguration { AdUnitId = anyAdUnitId });
    }


    public static void InitializeSdk(SdkConfiguration sdkConfiguration)
    {
        ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
        PluginClass.CallStatic(
            "initializeSdk", sdkConfiguration.AdUnitId);
    }

    public static void SetNeedTestDevice(bool needTestDevice)
    {
        PluginClass.CallStatic("setNeedTestDevice", needTestDevice);

    }


    public static void SetFacebookTestDevice(string testDevice)
    {
        PluginClass.CallStatic("setFacebookTestDevice", testDevice);

    }


    public static void SetAdmobTestDevice(string testDevice)
    {
        PluginClass.CallStatic("setAdmobTestDevice", testDevice);

    }

    public static void SetCnsetIsCNLanguageLog(bool languageLog)
    {
        PluginClass.CallStatic("setIsCNLanguageLog", languageLog);
    }

    public static void LoadBannerPluginsForAdUnits(string bannerAdUnitId)
    {
        BannerPluginsDict.Add(bannerAdUnitId, new MPBanner(bannerAdUnitId));

    }


    public static void LoadInterstitialPluginsForAdUnits(string interstitialAdUnitId)
    {
        InterstitialPluginsDict.Add(interstitialAdUnitId, new MPInterstitial(interstitialAdUnitId));

        Debug.Log(
            " interstitial AdUnits loaded for plugins:\n"
            + interstitialAdUnitId);
    }


    public static void LoadRewardedVideoPluginsForAdUnits(string rewardedAdUnitId)
    {
        RewardedVideoPluginsDict.Add(rewardedAdUnitId, new MPRewardedVideo(rewardedAdUnitId));

        Debug.Log(
            " rewarded video AdUnits loaded for plugins:\n"
            + rewardedAdUnitId);
    }

    public static bool IsSdkInitialized
    {
        get { return PluginClass.CallStatic<bool>("isSdkInitialized"); }
    }


    protected static string GetSdkName()
    {
        return "Android SDK v " + PluginClass.CallStatic<string>("getSDKVersion");
    }

    #endregion SdkSetup


    #region AndroidOnly

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum LocationAwareness
    {
        TRUNCATED,
        DISABLED,
        NORMAL
    }


    /// <summary>
    /// Registers the given device as a Facebook Ads test device.
    /// </summary>
    /// <param name="hashedDeviceId">String with the hashed ID of the device.</param>
    /// <remarks>See https://developers.facebook.com/docs/reference/android/current/class/AdSettings/ for details
    /// </remarks>
    public static void AddFacebookTestDeviceId(string hashedDeviceId)
    {
        PluginClass.CallStatic("addFacebookTestDeviceId", hashedDeviceId);
    }

    #endregion AndroidOnly


    #region Banners


    public static void CreateBanner(string adUnitId, AdPosition position)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateBanner(position);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    public static void ShowBanner(string adUnitId, bool shouldShow)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowBanner(shouldShow);
        else
            ReportAdUnitNotFound(adUnitId);
    }



    public static void RefreshBanner(string adUnitId, string keywords, string userDataKeywords = "")
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RefreshBanner(keywords, userDataKeywords);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    public void SetAutorefresh(string adUnitId, bool enabled)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetAutorefresh(enabled);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    public void ForceRefresh(string adUnitId)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ForceRefresh();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    public static void DestroyBanner(string adUnitId)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyBanner();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    #endregion Banners


    #region Interstitials


    public static void RequestInterstitialAd(string adUnitId, string keywords = "", string userDataKeywords = "")
    {
        Debug.Log("RequestInterstitialAd 1\n");
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
        {
            plugin.RequestInterstitialAd(keywords, userDataKeywords);
            Debug.Log("RequestInterstitialAd\n");
        }
        else
            ReportAdUnitNotFound(adUnitId);
    }


    public static void ShowInterstitialAd(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowInterstitialAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    public static bool IsInterstialReady(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.IsInterstitialReady;
        ReportAdUnitNotFound(adUnitId);
        return false;
    }


    public void DestroyInterstitialAd(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyInterstitialAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }



    #endregion Interstitials


    #region RewardedVideos


    public static void RequestRewardedVideo(string adUnitId, string keywords = "", string userDataKeywords = "")
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestRewardedVideo(keywords, userDataKeywords);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    public static void ShowRewardedVideo(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowRewardedVideo();
        else
            ReportAdUnitNotFound(adUnitId);
    }


    public static bool HasRewardedVideo(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.IsRewardedVideoReady;
        ReportAdUnitNotFound(adUnitId);
        return false;
    }


    public void DestroyRewardedVideo(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyRewardedVideo();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion RewardedVideos

}
