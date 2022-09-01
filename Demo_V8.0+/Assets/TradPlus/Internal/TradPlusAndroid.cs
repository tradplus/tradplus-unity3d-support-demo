using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using TradPlusInternal.ThirdParty.MiniJSON;
using MPBanner = TradPlusAndroidBanner;
using MPNative = TradPlusAndroidNative;
using MPNativeBanner = TradPlusAndroidNativeBanner;
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

    private static readonly Dictionary<string, MPNativeBanner> NativeBannerPluginsDict = new Dictionary<string, MPNativeBanner>();

    private static readonly Dictionary<string, MPInterstitial> InterstitialPluginsDict =
        new Dictionary<string, MPInterstitial>();

    private static readonly Dictionary<string, MPRewardedVideo> RewardedVideoPluginsDict =
        new Dictionary<string, MPRewardedVideo>();

    private static readonly Dictionary<string, MPOfferWall> OfferWallPluginsDict =
       new Dictionary<string, MPOfferWall>();

    public static string NeedDownloadImg = "need_down_load_img";


    #region SdkSetup

    //====初始化===
    public static void InitializeSdk(string anyAppID)
    {
        ValidateAdUnitForSdkInit(anyAppID);
        InitializeSdk(new SdkConfiguration { AppId = anyAppID });
    }

    public static void InitializeSdk(SdkConfiguration sdkConfiguration)
    {
        ValidateAdUnitForSdkInit(sdkConfiguration.AppId);
        PluginClass.CallStatic(
            "initializeSdk", sdkConfiguration.AppId);
    }

    //是否初始化成功
    public static bool IsSdkInitialized
    {
        get { return PluginClass.CallStatic<bool>("isSdkInitialized"); }
    }

    public static void SetAllowPostUseTime(bool allPostUseTime)
    {
        PluginClass.CallStatic("setAllowPostUseTime", allPostUseTime);
    }

    public static void SetAllowMessagePush(bool allowMessagePush)
    {
        PluginClass.CallStatic("setAllowMessagePush", allowMessagePush);
    }

    // load 后调用；关闭每个5分钟自动过期检测
    public static void SetAutoExpiration(bool isOn)
    {
        PluginClass.CallStatic("setAutoExpiration", isOn);
    }

    // load 后调用；主动触发过期检测（如有失效广告会触发加载）
    public static void CheckAutoExpiration()
    {
        PluginClass.CallStatic("checkAutoExpiration");
    }

    // 默认false，访问海外服务器；设置true，访问国内服务器
    public static void SetCnServer(bool cnServer)
    {
        PluginClass.CallStatic("setCnServer", cnServer);
    }

    /**
     * V7.0.60.1 support
     * @param openPersonalizedAd 个性化广告开关 : 默认开启true，false关闭
     */
    public static void SetOpenPersonalizedAd(bool openPersonalizedAd = true)
    {
        PluginClass.CallStatic("setOpenPersonalizedAd", openPersonalizedAd);
    }

    /**
     * V7.0.60.1 support
     * 判断个性化广告开关是否开启
     */
    public static bool IsOpenPersonalizedAd()
    {
        return PluginClass.CallStatic<bool>("isOpenPersonalizedAd");
    }


    /**
     * V7.3.0.1 support
     * @param privacyUserAgree 隐私权限信息 : 默认开启，false关闭
     */
    public static void SetPrivacyUserAgree(bool privacyUserAgree = true)
    {
        PluginClass.CallStatic("setPrivacyUserAgree", privacyUserAgree);
    }

    /**
     * V7.3.0.1 support
     * 判断是否获取隐私权限信息
     */
    public static bool IsPrivacyUserAgree()
    {
        return PluginClass.CallStatic<bool>("isPrivacyUserAgree");
    }

    // 穿山甲（国内）设置下载确认弹窗（请求广告前调用）
    public static void SetToutiaoIsConfirmDownload(bool isConfirm)
    {
        PluginClass.CallStatic("setToutiaoIsConfirmDownload", isConfirm);
    }

    // 腾讯优量汇（国内）下载确认弹窗（广告展示出来后，根据用户点击是否下载调用，直接下载true；拒绝或取消false）
    public void IsGDTConfirmDownload(bool isConfirm)
    {
        PluginClass.Call("isGDTConfirmDownload", isConfirm);
    }

    //=====GDPR====
    public static void setGDPRListener()
    {
        PluginClass.CallStatic("setGDPRListener");
    }

    // Android V8.4.0.1 ;iOS V 8.0.0 开始支持
    public static void checkCurrentArea()
    {
        PluginClass.CallStatic("checkCurrentArea");
    }

    public static void setFirstShow(bool firstShow = true)
    {
        PluginClass.CallStatic("setFirstShow", firstShow);
    }


    public static bool isFirstShow()
    {
        return PluginClass.CallStatic<bool>("isFirstShow");

    }

    // 8.0.30 开始支持设置微信小程序APPID Android only
    public static void SetWxAppId(string wxAppId)
    {
        PluginClass.CallStatic("setWxAppId", wxAppId);
    }


    public static void showUploadDataNotifyDialog(String url = "")
    {
        PluginClass.CallStatic("showUploadDataNotifyDialog", url);
    }


    public static void setGDPRDataCollection(int level)
    {
        PluginClass.CallStatic("setGDPRDataCollection", level);
    }


    public static int getGDPRDataCollection()
    {
        return PluginClass.CallStatic<int>("getGDPRDataCollection");
    }


    public static bool isEUTraffic()
    {
        return PluginClass.CallStatic<bool>("isEUTraffic");
    }

    //GDPRChild
    public static void setGDPRChild(bool isGdprChild)
    {
        PluginClass.CallStatic("setGDPRChild", isGdprChild);
    }

    //CCPA
    public static void setCCPADoNotSell(bool isCCPA)
    {
        PluginClass.CallStatic("setCCPADoNotSell", isCCPA);
    }

    //COPPA
    public static void setCOPPAIsAgeRestrictedUser(bool isChild)
    {
        PluginClass.CallStatic("setCOPPAIsAgeRestrictedUser", isChild);
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

    //V7801 支持测试模式并设置测试设备
    public static void SetNeedTestDevice(bool needTestDevice,string testmodeId)
    {
        PluginClass.CallStatic("setNeedTestDevice", needTestDevice, testmodeId);
    }

    public static void SetFacebookTestDevice(string testDevice)
    {
        PluginClass.CallStatic("setFacebookTestDevice", testDevice);
    }


    public static void SetAdmobTestDevice(string testDevice)
    {
        PluginClass.CallStatic("setAdmobTestDevice", testDevice);
    }

    //流量分组
    public static void initCustomMap(Dictionary<String, String> map)
    {
        PluginClass.CallStatic("initCustomMap", Json.Serialize(map));
    }

    //流量分组
    public static void initPlacementCustomMap(String placementId, Dictionary<String, String> map)
    {
        PluginClass.CallStatic("initPlacementCustomMap", placementId, Json.Serialize(map));

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

    //初始化NativeBanner广告位
    public static void LoadNativeBannerPluginsForAdUnits(string nativebannerAdUnitId)
    {
        NativeBannerPluginsDict.Add(nativebannerAdUnitId, new MPNativeBanner(nativebannerAdUnitId));
        Debug.Log(
            " nativebanner AdUnits loaded for plugins:\n"
            + nativebannerAdUnitId);
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

    //请求广告并传入广告场景ID
    public static void CreateBanner(string adUnitId, AdPosition position, string adSceneId)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateBanner(position, adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void SetBannerCustomParams(string adUnitId, Dictionary<string, object> map)
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetBannerCustomParams(map);
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
    public static void BannerEntryAdScenario(string adUnitId, string adSceneId = "")
    {
        MPBanner plugin;
        if (BannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.BannerEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion Banners


    #region Natives
    //设置广告宽高，请求广告前调用
    public static void SetAdSize(string adUnitId, int width, int height)
    {
        MPNative plugin;
        if (NativePluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetAdSize(width, height);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void SetNativeCustomParams(string adUnitId, Dictionary<string, object> map)
    {
        MPNative plugin;
        if (NativePluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetNativeCustomParams(map);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //请求广告 设置位置（底部中间、顶部中间等）
    public static void CreateNative(string adUnitId, AdPosition position, string adSceneId = "")
    {
        MPNative plugin;
        if (NativePluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateNative(position, adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //请求广告 设置坐标参数
    //两种请求广告只能二选一
    public static void CreateNative(string adUnitId, int x, int y, string adSceneId = "")
    {
        MPNative plugin;
        if (NativePluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateNative(x, y, adSceneId);
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
    public static void NativeEntryAdScenario(string adUnitId, string adSceneId = "")
    {
        MPNative plugin;
        if (NativePluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.NativeEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion Natives


    #region NativeBanners

    //设置广告宽高，请求广告前调用
    public static void SetNativeBannerSize(string adUnitId, int width, int height)
    {
        MPNativeBanner plugin;
        if (NativeBannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetNativeBannerSize(width, height);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void SetNativeBannerCustomParams(string adUnitId, Dictionary<string, object> map)
    {
        MPNativeBanner plugin;
        if (NativeBannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetNativeBannerCustomParams(map);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //加载广告
    //参数1:设置位置（底部中间、顶部中间）
    //参数2:(可选)adSceneId 广告场景ID
    //参数3:自定义布局文件name
    public static void CreateNativeBanner(string adUnitId, AdPosition position, string adSceneId = "", string LayoutIdByName = "")
    {
        MPNativeBanner plugin;
        if (NativeBannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateNativeBanner(position, adSceneId, LayoutIdByName);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //8.0.30 原HideNativeBanner(string adUnitId, bool shouldShow)废弃
    public static void HideNativeBanner(string adUnitId)
    {
        MPNativeBanner plugin;
        if (NativeBannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.HideNativeBanner();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //8.0.30 展示广告（用于隐藏后再次展示）
    public static void DisplayNativeBanner(string adUnitId)
    {
        MPNativeBanner plugin;
        if (NativeBannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DisplayNativeBanner();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //销毁广告，释放资源
    public static void DestroyNativeBanner(string adUnitId)
    {
        MPNativeBanner plugin;
        if (NativeBannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyNativeBanner();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景
    public static void NativeBannerEntryAdScenario(string adUnitId, string adSceneId = "")
    {
        MPNativeBanner plugin;
        if (NativeBannerPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.NativeBannerEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion NativeBanners

    #region Interstitials
    //请求广告
    public static void RequestInterstitialAd(string adUnitId, bool autoReload = false)
    {
        Debug.Log("RequestInterstitialAd ");
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
        {
            plugin.RequestInterstitialAd(autoReload);
            Debug.Log("RequestInterstitialAd");
        }
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void SetInterstitialCustomParams(string adUnitId, Dictionary<string, object> map)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetInterstitialCustomParams(map);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //强制请求
    public static void ReloadInterstitialAd(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
        {
            plugin.ReloadInterstitialAd();
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

    // V740 支持
    public static void InterstitialOnDestroy(string adUnitId)
    {
        MPInterstitial plugin;
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.InterstitialOnDestroy();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion Interstitials


    #region RewardedVideos

    // 服务器奖励
    public static void SetCustomParams(string adUnitId, Dictionary<String, String> map)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetCustomParams(map);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    //请求广告
    public static void RequestRewardedVideo(string adUnitId, bool autoReload = false)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestRewardedVideo(autoReload);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void SetRewardedVideoCustomParams(string adUnitId, Dictionary<string, object> map)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetRewardedVideoCustomParams(map);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void ReloadRewardedVideoAd(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ReloadRewardedVideoAd();
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
    public static void RewardedVideoEntryAdScenario(string adUnitId, string adSceneId)
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

    // V740 支持
    public static void RewardedVideoOnDestroy(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RewardedVideoOnDestroy();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    // V760 支持
    public static void RewardedVideoClearCache(string adUnitId)
    {
        MPRewardedVideo plugin;
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RewardedVideoClearCache();
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

    //8.2.0.1开始支持
    public static void SetOfferWallCustomParams(string adUnitId, Dictionary<string, object> map)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetOfferWallCustomParams(map);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //8.2.0.1开始支持
    public static void SetOfferWallUserId(string adUnitId, string userId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetOfferWallUserId(userId);
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
    public static void ShowOfferWall(string adUnitId, string adSceneId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowOfferWall(adSceneId);
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

    //查询总额
    public static void GetCurrencyBalance(string adUnitId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.GetCurrencyBalance();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //消耗积分
    public static void SpendCurrency(string adUnitId, int count)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SpendCurrency(count);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //增加积分
    public static void AwardCurrency(string adUnitId, int amount)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.AwardCurrency(amount);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //释放资源
    public static void OfferWallOnDestroy(string adUnitId)
    {
        MPOfferWall plugin;
        if (OfferWallPluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.OfferWallOnDestroy();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion OfferWalls


}
