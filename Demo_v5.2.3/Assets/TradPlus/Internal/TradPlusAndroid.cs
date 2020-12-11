using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using MPBanner = TradPlusAndroidBanner;
using MPNative = TradPlusAndroidNative;
using MPInterstitial = TradPlusAndroidInterstitial;
using MPRewardedVideo = TradPlusAndroidRewardedVideo;
using MPOfferWall = TradPlusAndroidOfferWall;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class TradPlusAndroid : TradPlusBase
{
	static TradPlusAndroid()
    {
        InitManager();
    }

	private static readonly AndroidJavaClass PluginClass = new AndroidJavaClass("com.tradplus.ads.unity.TradplusUnityPlugin");

    private static readonly Dictionary<string, MPBanner> BannerPluginsDict = new Dictionary<string, MPBanner>();

    private static readonly Dictionary<string, MPNative> NativePluginsDict = new Dictionary<string, MPNative>();

    private static readonly Dictionary<string, MPInterstitial> InterstitialPluginsDict =
        new Dictionary<string, MPInterstitial>();

    private static readonly Dictionary<string, MPRewardedVideo> RewardedVideoPluginsDict =
        new Dictionary<string, MPRewardedVideo>();

    private static readonly Dictionary<string, MPOfferWall> OfferWallPluginsDict =
       new Dictionary<string, MPOfferWall>();
        
    #region SdkSetup

    //====初始化===
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

    //是否初始化成功
    public static bool IsSdkInitialized
    {
        get { return PluginClass.CallStatic<bool>("isSdkInitialized"); }
    }

    //=====GDPR====
    public static void setGDPRListener()
    {
        PluginClass.CallStatic("setGDPRListener");
    }

    public static void setFirstShow(bool firstShow = true)
    {
        PluginClass.CallStatic("setFirstShow", firstShow);
    }


    public static bool isFirstShow()
    {
        return PluginClass.CallStatic<bool>("isFirstShow"); ;

    }


    public static void showUploadDataNotifyDialog(String url = "")
    {
        PluginClass.CallStatic("showUploadDataNotifyDialog" , url);
    }


    public static void setGDPRUploadDataLevel(int level)
    {
        PluginClass.CallStatic("setGDPRUploadDataLevel", level);
    }


    public static int getGDPRUploadDataLevel()
    {
        return PluginClass.CallStatic<int>("getGDPRUploadDataLevel");
    }


    public static bool isEUTraffic()
    {
        return PluginClass.CallStatic<bool>("isEUTraffic"); ;
    }

    //GDPRChild
    public static void setGDPRChild(bool isGdprChild)
    {
        PluginClass.CallStatic("setGDPRChild", isGdprChild);
    }

    //CCPA
    public static void setCCPADataCollection(bool isCCPA)
    {
        PluginClass.CallStatic("setCCPADataCollection", isCCPA);
    }

    //COPPA
    public static void setCOPPAChild(bool isChild)
    {
        PluginClass.CallStatic("setCOPPAChild", isChild);
    }

    //California
    public static void setCalifornia(bool isCa)
    {
        PluginClass.CallStatic("setCalifornia", isCa);
    }

    //是否是California
    public static bool isCalifornia
    { get { return PluginClass.Call<bool>("isCalifornia"); } }


    //AuthUID
    public static void setAuthUID(bool needAuthUID) 
    {
        PluginClass.CallStatic("setAuthUID", needAuthUID);
    }

    //测试模式
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

    //中文log
    public static void SetCnsetIsCNLanguageLog(bool languageLog)
    {
        PluginClass.CallStatic("setIsCNLanguageLog", languageLog);
    }

    //初始化Banner广告位
    public static void LoadBannerPluginsForAdUnits(string bannerAdUnitId)
    {
        BannerPluginsDict.Add(bannerAdUnitId, new MPBanner(bannerAdUnitId));
        Debug.Log(
            " banner AdUnits loaded for plugins:\n"
            + bannerAdUnitId);
    }

    //初始化Native广告位
    public static void LoadNativePluginsForAdUnits(string nativeAdUnitId)
    {
        NativePluginsDict.Add(nativeAdUnitId, new MPNative(nativeAdUnitId));
        Debug.Log(
            " native AdUnits loaded for plugins:\n"
            + nativeAdUnitId);
    }

    //初始化插屏广告位
    public static void LoadInterstitialPluginsForAdUnits(string interstitialAdUnitId)
    {
        InterstitialPluginsDict.Add(interstitialAdUnitId, new MPInterstitial(interstitialAdUnitId));
        Debug.Log(
            " interstitial AdUnits loaded for plugins:\n"
            + interstitialAdUnitId);
    }

    //初始化激励视频广告位
	public static void LoadRewardedVideoPluginsForAdUnits(string rewardedAdUnitId)
	{
		RewardedVideoPluginsDict.Add(rewardedAdUnitId, new MPRewardedVideo(rewardedAdUnitId));
		Debug.Log(
			" rewarded video AdUnits loaded for plugins:\n"
			+ rewardedAdUnitId);
	}

    //初始化积分墙广告位
    public static void LoadOfferWallPluginsForAdUnits(string offerWallAdUnitId)
    {
        OfferWallPluginsDict.Add(offerWallAdUnitId, new MPOfferWall(offerWallAdUnitId));
        Debug.Log(
            " OfferWall AdUnits loaded for plugins:\n"
            + offerWallAdUnitId);
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

    //请求广告
    public static void CreateBanner(string adUnitId, AdPosition position)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateBanner(position);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //显示广告 or 隐藏广告
    public static void ShowBanner(string adUnitId, bool shouldShow)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowBanner(shouldShow);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //销毁广告，释放资源
    public static void DestroyBanner(string adUnitId)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyBanner();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景
    public static void ShowBannerConfirmUWSAd(string adUnitId)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowBannerConfirmUWSAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景
    public static void BannerEntryAdScenario(string adUnitId)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.BannerEntryAdScenario();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion Banners


    #region Natives
    //请求广告 设置位置（底部中间、顶部中间等）
    public static void CreateNative(string adUnitId, AdPosition position)
    {
        MPNative plugin;
        if (NativePluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateNative(position);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //请求广告 设置坐标参数
    //两种请求广告只能二选一
    public static void CreateNative(string adUnitId, int x, int y)
    {
        MPNative plugin;
        if (NativePluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateNative(x,y);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //销毁广告，释放资源
    public static void DestroyNative(string adUnitId)
    {
        MPNative plugin;
        if (NativePluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyNative();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //显示广告 or 隐藏广告
    public static void ShowNative(string adUnitId, bool shouldShow)
    {
        MPNative plugin;
        if (NativePluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowNative(shouldShow);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景
    public static void NativeEntryAdScenario(string adUnitId)
    {
        MPNative plugin;
        if (NativePluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.NativeEntryAdScenario();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion Natives


    #region Interstitials
    //请求广告
    public static void RequestInterstitialAd(string adUnitId, bool autoReload = false)
    {
        Debug.Log("RequestInterstitialAd 1\n");
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
        {
            plugin.RequestInterstitialAd(autoReload);
            Debug.Log("RequestInterstitialAd\n");
        }
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void loadForcedlyInterstitial(string adUnitId)
    {
        Debug.Log("loadForcedlyInterstitial 1\n");
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
        {
            plugin.loadForcedlyInterstitial();
            Debug.Log("loadForcedlyInterstitial\n");
        }
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //展示广告
    public static void ShowInterstitialAd(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowInterstitialAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //展示广告（广告场景ID）
    public static void ShowInterstitialAd(string adUnitId, string adSceneId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowInterstitialAd(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景
    public static void ShowInterstitialConfirmUWSAd(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowInterstitialConfirmUWSAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景
    public static void InterstitialEntryAdScenario(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.InterstitialEntryAdScenario();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景（广告场景ID）
    public static void InterstitialEntryAdScenario(string adUnitId, string adSceneId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.InterstitialEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //check是否有可用广告
    public static bool IsInterstialReady(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.IsInterstitialReady;
        ReportAdUnitNotFound(adUnitId);
        return false;
    }

    //Check广告缓存数是否已达配置上限
    public static bool IsInterstitialAllReady(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.IsInterstitialAllReady;
        ReportAdUnitNotFound(adUnitId);
        return false;
    }

    //销毁广告，释放资源
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
    //请求广告
	public static void RequestRewardedVideo(string adUnitId, bool autoReload = false)
	{
		MPRewardedVideo plugin;
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
			plugin.RequestRewardedVideo(autoReload);
		else
			ReportAdUnitNotFound(adUnitId);
	}

    public static void loadForcedlyRewardedVideo(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.loadForcedlyRewardedVideo();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //展示广告
    public static void ShowRewardedVideo(string adUnitId)
	{
		MPRewardedVideo plugin;
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
			plugin.ShowRewardedVideo();
		else
			ReportAdUnitNotFound(adUnitId);
	}

    //展示广告（广告场景ID）
    public static void ShowRewardedVideo(string adUnitId, string adSceneId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowRewardedVideo(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景
    public static void ShowRewardedVideoConfirmUWSAd(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowRewardedVideoConfirmUWSAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景
    public static void RewardedVideoEntryAdScenario(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RewardedVideoEntryAdScenario();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景（广告场景ID）
    public static void RewardedVideoEntryAdScenario(string adUnitId,string adSceneId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RewardedVideoEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //check是否有可用广告
    public static bool HasRewardedVideo(string adUnitId)
	{
		MPRewardedVideo plugin;
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
			return plugin.IsRewardedVideoReady;
		ReportAdUnitNotFound(adUnitId);
		return false;
	}

    //Check广告缓存数是否已达配置上限
    public static bool IsRewardedVideoAllReady(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.IsRewardedVideoAllReady;
        ReportAdUnitNotFound(adUnitId);
        return false;
    }

    //销毁广告，释放资源
    public void DestroyRewardedVideo(string adUnitId)
	{
		MPRewardedVideo plugin;
		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
			plugin.DestroyRewardedVideo();
		else
			ReportAdUnitNotFound(adUnitId);
	}
		
    #endregion RewardedVideos

    #region OfferWalls
    //请求广告
    public static void RequestOfferWall(string adUnitId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestOfferWall();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //展示广告
    public static void ShowOfferWall(string adUnitId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowOfferWall();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //展示广告（广告场景ID）
    public static void ShowOfferWall(string adUnitId,string adSceneId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowOfferWall(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景
    public static void ShowOfferWallConfirmUWSAd(string adUnitId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowOfferWallConfirmUWSAd();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景
    public static void OfferWallEntryAdScenario(string adUnitId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.OfferWallEntryAdScenario();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景（广告场景ID）
    public static void OfferWallEntryAdScenario(string adUnitId, string adSceneId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.OfferWallEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //check是否有可用广告
    public static bool HasOfferWall(string adUnitId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.IsOfferWallReady;
        ReportAdUnitNotFound(adUnitId);
        return false;
    }

    //Check广告缓存数是否已达配置上限
    public static bool IsOfferWallAllReady(string adUnitId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.IsOfferWallAllReady;
        ReportAdUnitNotFound(adUnitId);
        return false;
    }

    //销毁广告，释放资源
    public void DestroyOfferWall(string adUnitId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyOfferWall();
        else
            ReportAdUnitNotFound(adUnitId);
    }
    #endregion OfferWalls
    
}
