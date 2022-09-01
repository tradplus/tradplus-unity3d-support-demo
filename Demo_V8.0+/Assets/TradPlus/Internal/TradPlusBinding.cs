using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
#if UNITY_IOS
[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
public class TradPlusBinding
{
    public TradPlus.Reward SelectedReward;

    private readonly string _adUnitId;
    public TradPlusBinding(string adUnitId)
    {
        _adUnitId = adUnitId;
        SelectedReward = new TradPlus.Reward { Label = string.Empty };
    }

    //splash
    public void LoadSplash()
    {
        _tradplusLoadSplash60(_adUnitId);
    }

    //Banner
    public void CreateBanner(TradPlus.AdPosition position, string adSceneId, int width, int height)
    {
        _tradplusCreateBanner60((int)position, _adUnitId, width, height, adSceneId);
    }

    public void LoadBanner(TradPlus.AdPosition position, int width, int height)
    {
        _tradplusLoadBanner((int)position, _adUnitId, width,height);
    }

    public void AutoShowBannerWithRect(int x, int y, int width, int height, string adSceneId)
    {
        _tradplusAutoShowBannerWithRect(_adUnitId, x, y, width, height, adSceneId);
    }

    public void LoadBannerWithRect(int x, int y, int width, int height)
    {
        _tradplusLoadBannerWithRect(_adUnitId, x, y, width, height);
    }
    
    public void DestroyBanner()
    {
        _tradplusDestroyBanner60(_adUnitId);
    }

    public void ShowBanner(bool shouldShow, string adSceneId)
    {
        _tradplusShowBanner60(_adUnitId, shouldShow, adSceneId);
    }

    public void BannerEntryAdScenario(string sceneId)
    {
        _tradplusBannerEntryAdScenario60(_adUnitId, sceneId);
    }

    //Native
    public void loadNative(int x, int y, int width, int height)
    {
        _tradplusLoadNative(_adUnitId, x, y, width, height);
    }

    public void ShowNativeNotAuto(string adSceneId)
    {
        _tradplusShowNativeNotAuto(_adUnitId,adSceneId);
    }

    public void NativeEntryAdScenario(string sceneId)
    {
        _tradplusNativeEntryAdScenario60(_adUnitId, sceneId);
    }


    public void ShowNative(int x, int y, int width, int height, string adSceneId)
    {
        _tradplusShowNative60(_adUnitId, x, y, width, height, adSceneId);
    }

    //显示广告 or 隐藏广告
    public void HideNative(bool needDestroy)
    {
        _tradplusHideNative60(_adUnitId, needDestroy);
    }

    //Interstital

    public void InterstitialEntryAdScenario(string adSceneId)
    {
        _tradplusInterstitialEntryAdScenario60(_adUnitId, adSceneId);
    }

    public void RequestInterstitialAd(bool autoReload, bool isPangleTemplateRender)
    {
        _tradplusRequestInterstitialAd60(_adUnitId, autoReload);
    }

    public bool IsInterstitialReady {
        get {
            return _tradplusIsInterstitialReady60(_adUnitId);
        }
    }

    public void ShowInterstitialAd(string adSceneId)
    {
        _tradplusShowInterstitialAd60(_adUnitId, adSceneId);
    }

    public void DestroyInterstitialAd()
    {
        _tradplusDestroyInterstitialAd60(_adUnitId);
    }

    //RewardedVideo
    public void SetCustomParams(Dictionary<string, string> map)
    {
        string user_id = map["user_id"];
        string custom_data = map["custom_data"];
        _tradplusRewardedVideoServerSide60(_adUnitId, user_id, custom_data);
    }

    public void RewardedVideoEntryAdScenario(string adSceneId)
    {
        _tradplusRewardedVideoEntryAdScenario60(_adUnitId, adSceneId);
    }

    public void RequestRewardedVideo(bool autoReload, bool isPangleTemplateRender)
    {
        _tradplusRequestRewardedVideo60(_adUnitId, autoReload);
    }

    // Queries if a rewarded video ad has been loaded for the given ad unit id.
    public bool HasRewardedVideo()
    {
        return _tradplusHasRewardedVideo60(_adUnitId);
    }

    // If a rewarded video ad is loaded this will take over the screen and show the ad
    public void ShowRewardedVideo(string adSceneId)
    {
        _tradplusShowRewardedVideo60(_adUnitId, adSceneId);
    }

    public void DestroyRewardedVideo()
    {
        _tradplusDestroyRewardedVideo60(_adUnitId);
    }


    //NativeBanner
    public void CreateNativeBanner(TradPlus.AdPosition position, string adSceneId, string className)
    {
        _tradplusCreateNativeBanner(_adUnitId, (int)position, adSceneId, className);
    }

    public void LoadNativeBanner(TradPlus.AdPosition position, string adSceneId, string className)
    {
        _tradplusLoadNativeBanner(_adUnitId, (int)position, adSceneId, className);
    }

    public void AutoShowNativeBanner(int x, int y, int width, int height, string adSceneId, string className)
    {
        _tradplusAutoShowNativeBanner(_adUnitId,x,y,width,height,adSceneId, className);
    }

    public void LoadNativeBannerWithRect(int x, int y, int width, int height, string adSceneId, string className)
    {
        _tradplusLoadNativeBannerWithRect(_adUnitId, x, y, width, height, adSceneId, className);
    }

    public void ShowNativeBanner(string adSceneId)
    {
        _tradplusShowNativeBanner(_adUnitId,adSceneId);
    }

    public void HideNativeBanner(bool hide)
    {
        _tradplusHideNativeBanner(_adUnitId, hide);
    }

    public void DestroyNativeBanner()
    {
        _tradplusDestroyNativeBanner(_adUnitId);
    }

    public void NativeBannerEntryAdScenario(string adSceneId)
    {
        _tradplusNativeBannerEntryAdScenario(_adUnitId,adSceneId);
    }

    //OfferWall
    //请求广告
    public void RequestOfferWall(bool autoReload)
    {
        _tradplusRequestOfferWall(_adUnitId, autoReload);
    }

    //展示广告
    public void ShowOfferWall(string adSceneId = "")
    {
        _tradplusShowOfferWall(_adUnitId,adSceneId);
    }

    //进入广告场景
    public void OfferWallEntryAdScenario(string adSceneId = "")
    {
        _tradplusOfferWallEntryAdScenario(_adUnitId,adSceneId);
    }

    //设置积分墙userId
    public void setOfferWallUserId(string userId)
    {
        _tradplusSetOfferWallUserId(_adUnitId, userId);
    }

    //check是否有可用广告
    public bool HasOfferWall()
    {
        return _tradplusHasOfferWall(_adUnitId);
    }

    //查询总额
    public void GetCurrencyBalance()
    {
        _tradplusGetCurrencyBalance(_adUnitId);
    }

    //消耗积分
    public void SpendCurrency( int amount)
    {
        _tradplusSpendCurrency(_adUnitId, amount);
    }

    //增加积分
    public void AwardCurrency(int amount)
    {
        _tradplusAwardCurrency(_adUnitId, amount);
    }

    //释放资源
    public void OfferWallOnDestroy()
    {
        _tradplusOfferWallOnDestroy(_adUnitId);
    }


    #region DllImports

    [DllImport("__Internal")]
    private static extern void _tradplusSetOfferWallUserId(string adUnitId, string userId);

    [DllImport("__Internal")]
    private static extern void _tradplusRequestOfferWall(string adUnitId,bool autoReload);

    [DllImport("__Internal")]
    private static extern void _tradplusShowOfferWall(string adUnitId, string adSceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusOfferWallEntryAdScenario(string adUnitId, string adSceneId);

    [DllImport("__Internal")]
    private static extern bool _tradplusHasOfferWall(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _tradplusGetCurrencyBalance(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _tradplusSpendCurrency(string adUnitId, int amount);

    [DllImport("__Internal")]
    private static extern void _tradplusAwardCurrency(string adUnitId, int amount);

    [DllImport("__Internal")]
    private static extern void _tradplusOfferWallOnDestroy(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _tradplusNativeBannerEntryAdScenario(string adUnitId, string adSceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusCreateNativeBanner(string adUnitId, int position, string sceneId, string className);

    [DllImport("__Internal")]
    private static extern void _tradplusShowNativeBanner(string adUnitId, string adSceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusLoadNativeBanner(string adUnitId, int position, string sceneId,string className);

    [DllImport("__Internal")]
    private static extern void _tradplusAutoShowNativeBanner(string adUnitId, int x, int y, int width, int height, string adSceneId, string className);

    [DllImport("__Internal")]
    private static extern void _tradplusLoadNativeBannerWithRect(string adUnitId, int x, int y, int width, int height, string adSceneId, string className);
    
    [DllImport("__Internal")]
    private static extern void _tradplusNativeEntryAdScenario60(string adUnitId, string adSceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusAutoShowBannerWithRect(string adUnitId, int x, int y, int width, int height,string adSceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusLoadBannerWithRect(string adUnitId, int x, int y, int width, int height);

    [DllImport("__Internal")]
    private static extern void _tradplusLoadBanner(int position, string adUnitId, int width, int height);

    [DllImport("__Internal")]
    private static extern void _tradplusShowNativeNotAuto(string adUnitId,string adSceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusLoadNative(string adUnitId, int x, int y, int width, int height);

    [DllImport("__Internal")]
    private static extern void _tradplusDestroyRewardedVideo60(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _tradplusShowRewardedVideo60(string adUnitId, string adSceneId);

    [DllImport("__Internal")]
    private static extern bool _tradplusHasRewardedVideo60(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _tradplusRequestRewardedVideo60(string adUnitId, bool autoReload);

    [DllImport("__Internal")]
    private static extern void _tradplusRewardedVideoEntryAdScenario60(string adUnitId, string adSceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusRewardedVideoServerSide60(string adUnitI, string user_id, string custom_data);

    [DllImport("__Internal")]
    private static extern void _tradplusDestroyInterstitialAd60(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _tradplusShowInterstitialAd60(string adUnitId, string adSceneId);

    [DllImport("__Internal")]
    private static extern bool _tradplusIsInterstitialReady60(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _tradplusRequestInterstitialAd60(string adUnitId, bool autoReload);

    [DllImport("__Internal")]
    private static extern void _tradplusInterstitialEntryAdScenario60(string adUnitId, string adSceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusLoadSplash60(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _tradplusShowNative60(string adUnitId, int x, int y, int width, int height, string adSceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusHideNative60(string adUnitId, bool needDestroy);

    [DllImport("__Internal")]
    private static extern void _tradplusBannerEntryAdScenario60(string adUnitId, string sceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusCreateBanner60(int position, string adUnitId, int width, int height, string sceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusDestroyBanner60(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _tradplusShowBanner60(string adUnitId, bool shouldShow, string sceneId);

    [DllImport("__Internal")]
    private static extern void _tradplusDestroyNativeBanner(string adUnitId);

    [DllImport("__Internal")]
    private static extern void _tradplusHideNativeBanner(string adUnitId, bool hide);

#endregion
}

#endif
