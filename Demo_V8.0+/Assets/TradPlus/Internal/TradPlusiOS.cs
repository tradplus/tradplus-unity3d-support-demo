using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UnityEngine;
using TradPlusInternal.ThirdParty.MiniJSON;
#if UNITY_IOS
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
    public static void initCustomMap(Dictionary<string, string> map)
    {
        _tradplusInitCustomMap(Json.Serialize(map));
    }

    public static void initPlacementCustomMap(String placementId, Dictionary<String, String> map)
    {
        _tradplusInitPlacementCustomMap(placementId, Json.Serialize(map));
    }

    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.InitializeSdk(string)"/>
    public static void InitializeSdk(string appId)
    {
        ValidateAdUnitForSdkInit(appId);
        InitializeSdk(new SdkConfiguration { AppId = appId });
    }


    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.InitializeSdk(TradPlusBase.SdkConfiguration)"/>
    public static void InitializeSdk(SdkConfiguration sdkConfiguration)
    {
        ValidateAdUnitForSdkInit(sdkConfiguration.AppId);
        _tradplusInitializeSdk(sdkConfiguration.AppId);
    }

    public static bool IsSdkInitialized()
    {
        return _tradplusIsSdkInitialized();
    }

    //是否开启 上传用户使用时长
    public static void SetAllowPostUseTime(bool isOpen)
    {
       _tradplusSetAllowPostUseTime(isOpen);
    }

    public static void SetCnServer(bool cnServer = false)
    {
        _tradplusSetCnServer(cnServer);
    }

    public static void SetAllowMessagePush(bool allowMessagePush = false)
    {
        _tradplusSetAllowMessagePush(allowMessagePush);
    }

    public static void SetOpenPersonalizedAd(bool isOpen)
    {
        _tradplusSetOpenPersonalizedAd(isOpen);
    }

    public static bool IsOpenPersonalizedAd()
    {
        return _tradplusIsOpenPersonalizedAd();
    }

    //GDPR
    public static void showUploadDataNotifyDialog(string url = "")
    {
        _tradplusShowUploadDataNotifyDialog(url);
    }


    public static void setGDPRDataCollection(int level)
    {
        _tradplusSetGDPRUploadDataLevel(level);
    }


    public static int getGDPRDataCollection()
    {
        return _tradplusGetGDPRUploadDataLevel();
    }


    public static bool isEUTraffic()
    {
        return _tradplusIsEUTraffic();
    }

    public static void checkCurrentArea()
    {
        _tradplusCheckCurrentArea();
    }

    //GDPRChild
    public static void setGDPRChild(bool isGDPRChild)
    {
        _tradplusSetGDPRChild(isGDPRChild);
    }

    //CCPA
    public static void setCCPADoNotSell(bool isCCPA)
    {
        _tradplusSetCCPADataCollection(isCCPA);
    }

    //COPPA
    public static void setCOPPAIsAgeRestrictedUser(bool isChild)
    {
        _tradplusSetCOPPAChild(isChild);
    }

    //AuthUID
    public static void setAuthUID(bool needAuthUID)
    {
        _tradplusSetAuthUID(needAuthUID);
    }

    //关闭开启每个5分钟自动过期检测
    public static void SetAutoExpiration(bool isOn)
    {
        _tradplusExpiredAdChecking(isOn);
    }

    //主动触发过期检测（如有失效广告会触发加载）
    public static void CheckAutoExpiration()
    {
        _tradplusExpiredAdCheck();
    }

    /// See TradPlusUnityEditor.<see cref="TradPlusUnityEditor.LoadBannerPluginsForAdUnits(string[])"/>
    public static void LoadBannerPluginsForAdUnits(string adUnitId)
    {
        LoadPluginsForAdUnits(adUnitId);
    }

    public static void LoadSplashPluginsForAdUnits(string adUnitId)
    {
        LoadPluginsForAdUnits(adUnitId);
    }

    public static void LoadNativePluginsForAdUnits(string adUnitId)
    {
        LoadPluginsForAdUnits(adUnitId);
    }

    public static void LoadNativeBannerPluginsForAdUnits(string adUnitId)
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

#region Splashs
    public static void LoadSplash(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.LoadSplash();
        else
            ReportAdUnitNotFound(adUnitId);
    }
    #endregion Splashs

    #region Banners

    public static void CreateBanner(string adUnitId, TradPlus.AdPosition position, string adSceneId = "", int width = 0, int height = 0)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateBanner(position, adSceneId, width, height);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //仅支持6.0+版本
    public static void AutoShowBannerWithRect(string adUnitId, int x, int y, int width, int height, string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.AutoShowBannerWithRect(x, y, width, height, adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //仅支持6.0+版本
    public static void LoadBannerWithRect(string adUnitId, int x, int y, int width, int height)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.LoadBannerWithRect(x,y,width,height);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //仅支持6.0+版本
    public static void LoadBanner(string adUnitId, TradPlus.AdPosition position, int width = 0, int height = 0)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.LoadBanner(position, width,height);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void ShowBanner(string adUnitId, bool shouldShow, string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowBanner(shouldShow, adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    public static void DestroyBanner(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyBanner();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void BannerEntryAdScenario(string adUnitId, string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.BannerEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion Banners

    #region Natives


    //请求广告 设置frame
    public static void ShowNative(string adUnitId, int x, int y, int width, int height, string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowNative(x, y, width, height, adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void loadNative(string adUnitId, int x, int y, int width, int height)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.loadNative(x, y, width, height);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void ShowNativeNotAuto(string adUnitId, string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowNativeNotAuto(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }


    //销毁广告，释放资源
    public static void HideNative(string adUnitId, bool needDestroy = false)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.HideNative(needDestroy);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void NativeEntryAdScenario(string adUnitId, string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.NativeEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion Natives

    #region Interstitials

    public static void InterstitialEntryAdScenario(string adUnitId, string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.InterstitialEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void RequestInterstitialAd(string adUnitId, bool autoReload = false, bool isPangleTemplateRender = false)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestInterstitialAd(autoReload,isPangleTemplateRender);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void ShowInterstitialAd(string adUnitId, string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowInterstitialAd(adSceneId);
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

    public static void RewardedVideoEntryAdScenario(string adUnitId, string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RewardedVideoEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void RequestRewardedVideo(string adUnitId, bool autoReload = false, bool isPangleTemplateRender = false)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestRewardedVideo(autoReload, isPangleTemplateRender);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void ShowRewardedVideo(string adUnitId, string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowRewardedVideo(adSceneId);
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

    //激励视频，清理指定广告位到缓存广告
    public static void RewardedVideoClearCache(string adUnitId)
    {
        _tradplusRewardedVideoClearCache(adUnitId);
    }

    // 服务器奖励
    public static void SetCustomParams(string adUnitId, Dictionary<string, string> map)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SetCustomParams(map);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion RewardedVideos


    #region NativeBanner
    //NativeBanner------

    //加载广告 自动加载并展示
    public static void CreateNativeBanner(string adUnitId, AdPosition position, string adSceneId = "", string className = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.CreateNativeBanner(position, adSceneId, className);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void LoadNativeBanner(string adUnitId, AdPosition position, string adSceneId = "", string className = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.LoadNativeBanner(position, adSceneId, className);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void AutoShowNativeBanner(string adUnitId, int x, int y, int width, int heigh, string adSceneId = "", string className = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.AutoShowNativeBanner(x,y, width, heigh, adSceneId, className);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void LoadNativeBannerWithRect(string adUnitId, int x, int y, int width, int heigh, string adSceneId = "", string className = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.LoadNativeBannerWithRect(x, y, width, heigh, adSceneId, className);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //使用 LoadNativeBanner 后 调用此方法展示
    public static void ShowNativeBanner(string adUnitId, string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowNativeBanner(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //显示已隐藏的native banner
    public static void DisplayNativeBanner(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.HideNativeBanner(false);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //隐藏native banner
    public static void HideNativeBanner(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.HideNativeBanner(true);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void DestroyNativeBanner(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.DestroyNativeBanner();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    public static void NativeBannerEntryAdScenario(string adUnitId,string adSceneId = "")
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.NativeBannerEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #region OfferWalls
    //请求广告
    public static void RequestOfferWall(string adUnitId, bool autoReload = false)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.RequestOfferWall(autoReload);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //展示广告
    public static void ShowOfferWall(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowOfferWall();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //展示广告（广告场景ID）
    public static void ShowOfferWall(string adUnitId, string adSceneId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.ShowOfferWall(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景
    public static void OfferWallEntryAdScenario(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.OfferWallEntryAdScenario();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //进入广告场景（广告场景ID）
    public static void OfferWallEntryAdScenario(string adUnitId, string adSceneId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.OfferWallEntryAdScenario(adSceneId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //设置积分墙userId
    public static void SetOfferWallUserId(string adUnitId, string userId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.setOfferWallUserId(userId);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //check是否有可用广告
    public static bool HasOfferWall(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            return plugin.HasOfferWall();
        ReportAdUnitNotFound(adUnitId);
        return false;
    }

    //查询总额
    public static void GetCurrencyBalance(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.GetCurrencyBalance();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //消耗积分
    public static void SpendCurrency(string adUnitId, int amount)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.SpendCurrency(amount);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //增加积分
    public static void AwardCurrency(string adUnitId, int amount)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.AwardCurrency(amount);
        else
            ReportAdUnitNotFound(adUnitId);
    }

    //释放资源
    public static void OfferWallOnDestroy(string adUnitId)
    {
        MP plugin;
        if (PluginsDict.TryGetValue(adUnitId, out plugin))
            plugin.OfferWallOnDestroy();
        else
            ReportAdUnitNotFound(adUnitId);
    }

    #endregion OfferWalls

    //-----------
    #endregion NativeBanner

    #region DllImports

    [DllImport("__Internal")]
    private static extern void _tradplusRewardedVideoClearCache(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _tradplusExpiredAdChecking(bool isOpen);

    [DllImport("__Internal")]
    private static extern bool _tradplusExpiredAdCheck();

    [DllImport("__Internal")]
    private static extern void _tradplusSetOpenPersonalizedAd(bool isOpen);

    [DllImport("__Internal")]
    private static extern bool _tradplusIsOpenPersonalizedAd();

    [DllImport("__Internal")]
    private static extern void _tradplusSetCnServer(bool cnServer);

    [DllImport("__Internal")]
    private static extern void _tradplusSetAllowMessagePush(bool messagePush);

    [DllImport("__Internal")]
    private static extern void _tradplusSetAllowPostUseTime(bool isOpen);

    [DllImport("__Internal")]
    private static extern void _tradplusInitializeSdk(string adUnitId);

    [DllImport("__Internal")]
    private static extern bool _tradplusIsSdkInitialized();

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

    [DllImport("__Internal")]
    private static extern void _tradplusSetGDPRChild(bool isGDPRChild);

    [DllImport("__Internal")]
    private static extern void _tradplusSetCCPADataCollection(bool isCCPA);

    [DllImport("__Internal")]
    private static extern void _tradplusSetCOPPAChild(bool isChild);

    [DllImport("__Internal")]
    private static extern void _tradplusSetAuthUID(bool needAuthUID);

    [DllImport("__Internal")]
    private static extern void _tradplusInitCustomMap(string customMap);

    [DllImport("__Internal")]
    private static extern void _tradplusInitPlacementCustomMap(string placementId, string customMap);

    [DllImport("__Internal")]
    private static extern void _tradplusCheckCurrentArea();

#endregion DllImports
}

#endif
