using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
#if Flute_native_beta

// This feature is still in Beta! If you're interested in our Beta Program, please contact support@Flute.com
using NativeAdData = AbstractNativeAd.Data;

#endif

[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
public class FluteDemoGUI : MonoBehaviour
{
    // State maps to enable/disable GUI ad state buttons
    private readonly Dictionary<string, bool> _adUnitToLoadedMapping = new Dictionary<string, bool>();

    private readonly Dictionary<string, bool> _adUnitToShownMapping = new Dictionary<string, bool>();

    private readonly Dictionary<string, List<TradPlus.Reward>> _adUnitToRewardsMapping =
        new Dictionary<string, List<TradPlus.Reward>>();

#if UNITY_IOS
    //iOS -测试专用广告位
    //private readonly string _appId = "6EAB93D825A2FA6296D96F215025567F";
    //private readonly string _bannerAdUnits = "C89052CE461113F9B3EAC920577B5018";
    //private readonly string _nativeAdUnits = "BF52F20494C1E9B7BCA02C79A8F77FE4";
    //private readonly string _nativebannerAdUnits = "C8C1CADB31129C56044BB3011E9F65B1";
    //private readonly string _interstitialAdUnits = "9DCB39C5C1C17E38B6B30DD702F7E823";
    //private readonly string _rewardedVideoAdUnits = "5ED958AAFE5F15831CE5B8D9CA40D778";
    //private readonly string _offerWallAdUnits = "D109A3D5BF61B540F1BA63C4DE70B31C";
    //iOS - 自测专用广告位
    private readonly string _appId = "75AA158112F1EFA29169E26AC63AFF94";
    private readonly string _bannerAdUnits = "6008C47DF1201CC875F2044E88FCD287";
    private readonly string _nativeAdUnits = "E8D198EBD7FDC4F8A725066C82D707E1";
    private readonly string _nativebannerAdUnits = "0AA7414819EE56542DBA126FE5A19C7E";
    private readonly string _interstitialAdUnits = "063265866B93A4C6F93D1DDF7BF7329B";
    private readonly string _rewardedVideoAdUnits = "160AFCDF01DDA48CCE0DBDBE69C8C669";
    private readonly string _offerWallAdUnits = "470166B2D4DEA9A7DCD3F42C5CE658B0";

#elif UNITY_ANDROID || UNITY_EDITOR
    //测试广告位，上线前要替换成您申请的广告位
    private readonly string _appId = "44273068BFF4D8A8AFF3D5B11CBA3ADE";
    private readonly string _bannerAdUnits = "A24091715B4FCD50C0F2039A5AF7C4BB";
    private readonly string _nativeAdUnits = "DDBF26FBDA47FBE2765F1A089F1356BF";
    private readonly string _nativebannerAdUnits = "9F4D76E204326B16BD42FA877AFE8E7D";
    private readonly string _interstitialAdUnits = "E609A0A67AF53299F2176C3A7783C46D";
    private readonly string _rewardedVideoAdUnits = "39DAC7EAC046676C5404004A311D1DB1";
    private readonly string _offerWallAdUnits = "4F7F1B9288B2FD513C8549A4A9F5D60F";


#endif

    [SerializeField]
    private GUISkin _skin;

    // Label style for no ad unit messages
    private GUIStyle _smallerFont;

    // Buffer space between sections
    private int _sectionMarginSize;

    // Label style for plugin and SDK version banner
    private GUIStyle _centeredStyle;

    // Default text for custom data fields
    private static string _customDataDefaultText = "Optional custom data";

    // String to fill with custom data for Rewarded Videos
    private string _rvCustomData = _customDataDefaultText;

    // Status string for tracking current state
    private string _status = string.Empty;

    private int position;

    private int postUseTime;

    private int cnServer;

    private int pushMessage;

    private int openPersonalizedAd;

    public Vector2 scrollPosition = Vector2.zero;

    private static bool IsAdUnitArrayNullOrEmpty(ICollection<string> adUnitArray)
    {
        return (adUnitArray == null || adUnitArray.Count == 0);
    }


    private void AddAdUnitsToStateMaps(string adUnit)
    {
        _adUnitToLoadedMapping.Add(adUnit, false);
        _adUnitToShownMapping.Add(adUnit, false);
    }


    public void SdkInitialized()
    {

    }


    public void UpdateStatusLabel(string message)
    {
        _status = message;
    }


    public void ClearStatusLabel()
    {
        UpdateStatusLabel(string.Empty);
    }

    public void LoadAvailableRewards(string adUnitId, List<TradPlus.Reward> availableRewards)
    {
        // Remove any existing available rewards associated with this AdUnit from previous ad requests
        _adUnitToRewardsMapping.Remove(adUnitId);

        if (availableRewards != null)
        {
            _adUnitToRewardsMapping[adUnitId] = availableRewards;
        }
    }

    public void NativeLoaded(string adUnitId, float height)
    {
        _adUnitToLoadedMapping[adUnitId] = true;
    }

    // Banner
    public void BannerLoaded(string adUnitId, float height)
    {
        AdLoaded(adUnitId);
        _adUnitToShownMapping[adUnitId] = true;
    }

    public void AdLoaded(string adUnit)
    {
        _adUnitToLoadedMapping[adUnit] = true;
        UpdateStatusLabel("Loaded " + adUnit);
    }


    public void AdDismissed(string adUnit)
    {
        _adUnitToLoadedMapping[adUnit] = false;
        ClearStatusLabel();
    }

    private void Awake()
    {

        if (Screen.width < 960 && Screen.height < 960)
        {
            _skin.button.fixedHeight = 50;
        }

        _smallerFont = new GUIStyle(_skin.label) { fontSize = _skin.button.fontSize };
        _centeredStyle = new GUIStyle(_skin.label) { alignment = TextAnchor.UpperCenter };

        // Buffer space between sections
        _sectionMarginSize = _skin.label.fontSize;

        position = PlayerPrefs.GetInt("AdPosition", 5);

        cnServer = PlayerPrefs.GetInt("CnServerSetting", 1);

        pushMessage = PlayerPrefs.GetInt("PushMessageSetting", 1);

        openPersonalizedAd = PlayerPrefs.GetInt("OpenPersonalizedAd", 1);

        AddAdUnitsToStateMaps(_bannerAdUnits);
        AddAdUnitsToStateMaps(_nativeAdUnits);
        AddAdUnitsToStateMaps(_nativebannerAdUnits);
        AddAdUnitsToStateMaps(_interstitialAdUnits);
        AddAdUnitsToStateMaps(_rewardedVideoAdUnits);
        AddAdUnitsToStateMaps(_offerWallAdUnits);
        AddAdUnitsToStateMaps("GDPR");
        AddAdUnitsToStateMaps("CCPA");
        AddAdUnitsToStateMaps("COPPAChild");
    }


    private void Start()
    {

#if UNITY_ANDROID
        //在初始化SDK之前调用
        TradPlus.setGDPRListener();
#elif UNITY_IOS
        if(cnServer == 2)
        {
            TradPlus.SetCnServer(true);
        }
        if(pushMessage == 2)
        {
            TradPlus.SetAllowMessagePush(true);
        }
#endif
        //初始化TradPlus SDK
        TradPlus.InitializeSdk(new TradPlusBase.SdkConfiguration
        {
            AppId = _appId,
        });
#if UNITY_ANDROID
        //设置微信小程序APPID
        TradPlus.SetWxAppId("123456");
        //添加测试设备,正式上线前注释
        TradPlusAndroid.SetNeedTestDevice(true);
#endif
        /*
         *    AdUnitID，TradPlus后台设置 对应广告类型的广告位（非三方广告网络的placementId）
         *    这里添加的是供测试使用的广告位，正式上线前必须替换成您申请的广告位
         *
         *    注意广告位不能填错，否则无法拿到广告
         *    仅在初始化广告位时调用一次
         */

        TradPlus.LoadBannerPluginsForAdUnits(_bannerAdUnits);
        TradPlus.LoadNativePluginsForAdUnits(_nativeAdUnits);
        TradPlus.LoadNativeBannerPluginsForAdUnits(_nativebannerAdUnits);
        TradPlus.LoadInterstitialPluginsForAdUnits(_interstitialAdUnits);
        TradPlus.LoadRewardedVideoPluginsForAdUnits(_rewardedVideoAdUnits);
        TradPlus.LoadOfferWallPluginsForAdUnits(_offerWallAdUnits);


#if !(UNITY_ANDROID || UNITY_IOS)
        Debug.LogError("Please switch to either Android or iOS platforms to run sample app!");
#endif

#if UNITY_EDITOR
        Debug.LogWarning("No SDK was loaded since this is not on a mobile device! Real ads will not load.");
#endif

        var nativeAdsGO = GameObject.Find("FluteNativeAds");
        if (nativeAdsGO != null)
            nativeAdsGO.SetActive(false);
    }

    private void OnGUI()
    {
        GUI.skin = _skin;

#if UNITY_2017_3_OR_NEWER
        // Screen.safeArea was added in Unity 2017.2.0p1
        var guiArea = Screen.safeArea;
#else
        var guiArea = new Rect(0, 0, Screen.width, Screen.height);
#endif
        guiArea.x += 20;
        guiArea.width -= 40;
        GUILayout.BeginArea(guiArea);

        CreateTitleSection();
        //CreatePosition();
        CreateOtherSection();
        CreateSetOpenPersonalizedAd();
        CreateBannersSection();
#if UNITY_IOS
//        CreateRectBannersSection();
#endif
        CreateNativesSection();
        CreateInterstitialsSection();
        CreateRewardedVideosSection();
#if UNITY_IOS
//        CreateSetCnServer();
//        CreateRectNativeBanner();
//        CreateUseTime();
#endif
        CreateNativeBannerSection();
        CreateOfferWallSection();
        CreateGDPR();
        CreateCCPA();
        CreateCOPPAChild();
        CreateStatusSection();

         GUILayout.EndArea();
    }

    private void CreateSetOpenPersonalizedAd()
    {
        if (openPersonalizedAd == 1)
        {
            GUILayout.Label("个性化推荐 开启");
        }
        else
        {
            GUILayout.Label("个性化推荐 关闭");
        }
        if (GUILayout.Button("个性化推荐"))
        {
            bool isOpen = true;
            if (openPersonalizedAd == 0)
            {
                openPersonalizedAd = 1;
            }
            else
            {
                openPersonalizedAd = 0;
                isOpen = false;
            }
            PlayerPrefs.SetInt("OpenPersonalizedAd", openPersonalizedAd);
            TradPlus.SetOpenPersonalizedAd(isOpen);
        }
    }
#if UNITY_IOS
    //暂时 iOS 支持
    private void CreateSetCnServer()
    {
        GUILayout.BeginHorizontal();
        if (cnServer == 2)
        {
            GUILayout.Label("cnServer: YES，设置后需重启");
        }
        else
        {
            GUILayout.Label("cnServer: NO，设置后需重启");
        }
        if (pushMessage == 2)
        {
            GUILayout.Label("埋点上报: 关闭，设置后需重启");
        }
        else
        {
            GUILayout.Label("埋点上报: 开启，设置后需重启");
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("cnServer"))
        {
            if (cnServer == 2)
            {
                cnServer = 1;
            }
            else
            {
                cnServer = 2;
            }
            PlayerPrefs.SetInt("CnServerSetting", cnServer);
        }
        if (GUILayout.Button("埋点上报"))
        {
            if (pushMessage == 2)
            {
                pushMessage = 1;
            }
            else
            {
                pushMessage = 2;
            }
            PlayerPrefs.SetInt("PushMessageSetting", pushMessage);
        }
        GUILayout.EndHorizontal();
    }

    private void CreateUseTime()
    {
        if (postUseTime != 2)
        {
            GUILayout.Label("postUseTime: ON");
        }
        else
        {
            GUILayout.Label("postUseTime: OFF");
        }
        if (GUILayout.Button("postUseTime"))
        {
            bool isOpen = true;
            if (postUseTime == 2)
            {
                postUseTime = 1;
            }
            else
            {
                postUseTime = 2;
                isOpen = false;
            }
            PlayerPrefs.SetInt("PostUseTimeSetting", postUseTime);
            TradPlus.SetAllowPostUseTime(isOpen);
        }
    }
#endif
    private void CreateTitleSection()
    {
        // App title including Plugin and SDK versions
        var prevFontSize = _centeredStyle.fontSize;
        _centeredStyle.fontSize = 48;
        GUI.Label(new Rect(0, 10, Screen.width, 60), TradPlus.PluginName, _centeredStyle);
        _centeredStyle.fontSize = prevFontSize;
        GUI.Label(new Rect(0, 70, Screen.width, 60), "with " + TradPlus.SdkName, _centeredStyle);
        const int titlePadding = 120;
        GUILayout.Space(titlePadding);
    }

    private void CreatePosition()
    {
        var textStr = "";
        switch (position)
        {
            case 0://TradPlusBase.AdPosition.TopLeft:
                textStr = "TopLeft";
                break;
            case 1:// TradPlusBase.AdPosition.TopCenter:
                textStr = "TopCenter";
                break;
            case 2:// TradPlusBase.AdPosition.TopRight:
                textStr = "TopRight";
                break;
            case 3:// TradPlusBase.AdPosition.Centered:
                textStr = "Centered";
                break;
            case 4:// TradPlusBase.AdPosition.BottomLeft:
                textStr = "BottomLeft";
                break;
            case 5:// TradPlusBase.AdPosition.BottomCenter:
                textStr = "BottomCenter";
                break;
            case 6:// TradPlusBase.AdPosition.BottomRight:
                textStr = "BottomRight";
                break;
        }
        GUILayout.Label("Position: --- " + textStr);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("TopLeft"))
        {
            position = 0;
            PlayerPrefs.SetInt("AdPosition", 0);
        }
        if (GUILayout.Button("TopCenter"))
        {
            position = 1;
            PlayerPrefs.SetInt("AdPosition", 1);
        }
        if (GUILayout.Button("TopRight"))
        {
            position = 2;
            PlayerPrefs.SetInt("AdPosition", 2);
        }
        if (GUILayout.Button("Centered"))
        {
            position = 3;
            PlayerPrefs.SetInt("AdPosition", 3);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("BottomLeft"))
        {
            position = 4;
            PlayerPrefs.SetInt("AdPosition", 4);
        }
        if (GUILayout.Button("BottomCenter"))
        {
            position = 5;
            PlayerPrefs.SetInt("AdPosition", 5);
        }
        if (GUILayout.Button("BottomRight"))
        {
            position = 6;
            PlayerPrefs.SetInt("AdPosition", 6);
        }
        GUILayout.EndHorizontal();
    }

    private void CreateOtherSection()
    {
        GUILayout.Space(_sectionMarginSize);
        GUILayout.Label("TradPlus ");

        GUILayout.BeginHorizontal();

        GUI.enabled = true;
        if (GUILayout.Button("查询地区信息"))
        {
            ClearStatusLabel();
            TradPlus.checkCurrentArea();
            UpdateStatusLabel("checkCurrentArea");
        }

        GUI.enabled = true;
        if (GUILayout.Button("AllowPostUseTime"))
        {
            ClearStatusLabel();
            TradPlus.SetAllowPostUseTime(false);
            UpdateStatusLabel("AllowPostUseTime ： " + false);
        }

        GUI.enabled = true;
        if (GUILayout.Button("setAllowMessagePush"))
        {
            ClearStatusLabel();
            TradPlus.SetAllowMessagePush(false);
            UpdateStatusLabel("AllowMessagePush ： " + false);
        }

        GUILayout.EndHorizontal();

    }

    private void CreateBannersSection()
    {
        GUILayout.Label("Banner");

        GUILayout.BeginHorizontal();
#if UNITY_ANDROID
        GUI.enabled = !_adUnitToLoadedMapping[_bannerAdUnits];

        if (GUILayout.Button(CreateRequestButtonLabel(_bannerAdUnits)))
        {
            Debug.Log("requesting banner with AdUnit: " + _bannerAdUnits);
            UpdateStatusLabel("Requesting " + _bannerAdUnits);
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("width", 500);
            //dic.Add("height", 250);
            //TradPlus.SetBannerCustomParams(_bannerAdUnits, dic);
            //请求Banner广告，并设置在底部弹出
            TradPlus.CreateBanner(_bannerAdUnits, TradPlus.AdPosition.BottomCenter);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Destroy"))
        {
            ClearStatusLabel();
            //销毁广告
            TradPlus.DestroyBanner(_bannerAdUnits);
            _adUnitToLoadedMapping[_bannerAdUnits] = false;
            _adUnitToShownMapping[_bannerAdUnits] = false;
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //展示广告
            TradPlus.ShowBanner(_bannerAdUnits, true);
            _adUnitToShownMapping[_bannerAdUnits] = true;
        }

        GUI.enabled = true;
        if (GUILayout.Button("Hide"))
        {
            ClearStatusLabel();
            //隐藏广告
            TradPlus.ShowBanner(_bannerAdUnits, false);
            _adUnitToShownMapping[_bannerAdUnits] = false;
        }

        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUI.enabled = true;

        if (GUILayout.Button("EntryAdScenario"))
        {
            //进入广告场景页面调用
            TradPlus.BannerEntryAdScenario(_bannerAdUnits, "11111");
        }

#elif UNITY_IOS
        GUI.enabled = !_adUnitToLoadedMapping[_bannerAdUnits];
        if (GUILayout.Button("autoShow"))
        {
            UpdateStatusLabel("Requesting " + _bannerAdUnits);
            //请求Banner广告，并设置在底部弹出
            TradPlus.CreateBanner(_bannerAdUnits, (TradPlus.AdPosition)position);
            //进入广告场景页面调用
            TradPlus.BannerEntryAdScenario(_bannerAdUnits, null);
        }

        //load & show 分开
        if (GUILayout.Button("load"))
        {
            TradPlus.LoadBanner(_bannerAdUnits, (TradPlus.AdPosition)position);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //展示广告
            TradPlus.ShowBanner(_bannerAdUnits, true);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Hide"))
        {
            ClearStatusLabel();
            //隐藏广告
            TradPlus.ShowBanner(_bannerAdUnits, false);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Destroy"))
        {
            ClearStatusLabel();
            //销毁广告
            TradPlus.DestroyBanner(_bannerAdUnits);
        }
#endif
        GUI.enabled = true;

        GUILayout.EndHorizontal();
    }

#if UNITY_IOS
    //指定位置
    private void CreateRectBannersSection()
    {
        GUILayout.Label("Banner Rect");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("autoShow"))
        {
            //请求Banner广告，并设置在底部弹出
            TradPlus.AutoShowBannerWithRect(_bannerAdUnits, 0,200,320,120);
            TradPlus.BannerEntryAdScenario(_bannerAdUnits,"123");
        }

        //load & show 分开
        if (GUILayout.Button("load"))
        {
            TradPlus.LoadBannerWithRect(_bannerAdUnits, 0, 200, 320, 120);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //展示广告
            TradPlus.ShowBanner(_bannerAdUnits, true);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Hide"))
        {
            ClearStatusLabel();
            //隐藏广告
            TradPlus.ShowBanner(_bannerAdUnits, false);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Destroy"))
        {
            ClearStatusLabel();
            //销毁广告
            TradPlus.DestroyBanner(_bannerAdUnits);
        }
        GUILayout.EndHorizontal();
    }
#endif
    private void CreateNativesSection()
    {

        GUILayout.Label("Native");

        GUILayout.BeginHorizontal();
#if UNITY_ANDROID

        if (GUILayout.Button(CreateRequestButtonLabel(_nativeAdUnits)))
        {
            Debug.Log("requesting native with AdUnit: " + _nativeAdUnits);
            UpdateStatusLabel("Requesting " + _nativeAdUnits);
            TradPlus.SetAdSize(_nativeAdUnits, 320, 340);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(TradPlus.NeedDownloadImg, "true");
            dic.Add("ad_click_fullscreen", "1");
            dic.Add("adx_provicy_icon", "1");
            TradPlus.SetNativeCustomParams(_nativeAdUnits, dic);
            //请求Native广告，并设置在底部弹出
            TradPlus.CreateNative(_nativeAdUnits, TradPlusBase.AdPosition.BottomCenter);
            //请求Native广告，设置 X Y
            //TradPlus.CreateNative(_nativeAdUnits,100,0);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //展示广告
            TradPlus.ShowNative(_nativeAdUnits, true);
            _adUnitToShownMapping[_nativeAdUnits] = true;
        }

        GUI.enabled = true;
        if (GUILayout.Button("Hide"))
        {
            ClearStatusLabel();
            //隐藏广告
            TradPlus.ShowNative(_nativeAdUnits, false);
            _adUnitToShownMapping[_nativeAdUnits] = false;
        }

        GUI.enabled = true;
        if (GUILayout.Button("Destroy"))
        {
            ClearStatusLabel();
            //销毁广告
            TradPlus.DestroyNative(_nativeAdUnits);
            _adUnitToLoadedMapping[_nativeAdUnits] = false;
            _adUnitToShownMapping[_nativeAdUnits] = false;
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.enabled = true;

        if (GUILayout.Button("EntryAdScenario"))
        {
            ClearStatusLabel();

            TradPlus.NativeEntryAdScenario(_nativeAdUnits, "22222");
            _adUnitToShownMapping[_nativeAdUnits] = false;
        }

#elif UNITY_IOS
        if (GUILayout.Button("autoShow"))
        {
            TradPlus.ShowNative(_nativeAdUnits, 0, 200, 320, 400);
            TradPlus.NativeEntryAdScenario(_bannerAdUnits);
        }
        //show & load 分开
        if (GUILayout.Button("load"))
        {
            TradPlus.loadNative(_nativeAdUnits, 0, 200, 320, 400);
        }
        if (GUILayout.Button("show"))
        {
            TradPlus.ShowNativeNotAuto(_nativeAdUnits);
        }
        if (GUILayout.Button("Hide"))
        {
            TradPlus.HideNative(_nativeAdUnits, false);
        }
#endif
        GUI.enabled = true;
        GUILayout.EndHorizontal();
    }


    private void CreateNativeBannerSection()
    {

        GUILayout.Label("NativeBanner");

        GUILayout.BeginHorizontal();

#if UNITY_ANDROID
        if (GUILayout.Button(CreateRequestButtonLabel(_nativebannerAdUnits)))
#elif UNITY_IOS
        if (GUILayout.Button("autoShow"))
#endif
        {
            Debug.Log("requesting nativebanner with AdUnit: " + _nativebannerAdUnits);
            UpdateStatusLabel("Requesting " + _nativebannerAdUnits);
            //宽 -1 为宽全屏显示
            //TradPlus.SetNativeBannerSize(_nativebannerAdUnits, -1, 50);
            //默认不传 尺寸为 320 * 50
#if UNITY_ANDROID
            TradPlus.SetNativeBannerSize(_nativebannerAdUnits, 320, 50);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(TradPlus.NeedDownloadImg, "true");
            dic.Add("ad_click_fullscreen", "1");
            dic.Add("adx_provicy_icon", "1");
            TradPlus.SetNativeCustomParams(_nativebannerAdUnits, dic);
#endif
            TradPlus.CreateNativeBanner(_nativebannerAdUnits, TradPlusBase.AdPosition.TopCenter);
            //设置自定义布局 native_banner_ad_unit为您的布局文件
            //TradPlus.CreateNativeBanner(_nativebannerAdUnits, TradPlusBase.AdPosition.BottomCenter, "", "native_banner_ad_unit");

        }
#if UNITY_IOS
        if (GUILayout.Button("Load"))
        {
            //展示广告
            TradPlus.LoadNativeBanner(_nativebannerAdUnits, (TradPlus.AdPosition)position);
        }
#endif
        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //展示广告
#if UNITY_ANDROID
            TradPlus.DisplayNativeBanner(_nativebannerAdUnits);
#elif UNITY_IOS
            TradPlus.ShowNativeBanner(_nativebannerAdUnits);
#endif
            _adUnitToShownMapping[_nativebannerAdUnits] = true;
        }

        GUI.enabled = true;
        if (GUILayout.Button("Hide"))
        {
            ClearStatusLabel();
            //隐藏广告
            TradPlus.HideNativeBanner(_nativebannerAdUnits);
            _adUnitToShownMapping[_nativebannerAdUnits] = false;
        }
        if (GUILayout.Button("Display"))
        {
            ClearStatusLabel();
            //显示广告
            TradPlus.DisplayNativeBanner(_nativebannerAdUnits);
            _adUnitToShownMapping[_nativebannerAdUnits] = true;
        }
        GUI.enabled = true;
        if (GUILayout.Button("Destroy"))
        {
            ClearStatusLabel();
            //销毁广告
            TradPlus.DestroyNativeBanner(_nativebannerAdUnits);
            _adUnitToLoadedMapping[_nativebannerAdUnits] = false;
            _adUnitToShownMapping[_nativebannerAdUnits] = false;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("EntryAdScenario"))
        {
            ClearStatusLabel();
            //进入广告场景
            TradPlus.NativeBannerEntryAdScenario(_nativebannerAdUnits, "33333");
            _adUnitToShownMapping[_nativebannerAdUnits] = false;
        }
        GUILayout.EndHorizontal();
    }

#if UNITY_IOS
    private void CreateRectNativeBanner()
    {

        GUILayout.Label("NativeBannerRect");

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("autoShow"))
        {
            TradPlus.AutoShowNativeBanner(_nativebannerAdUnits, 0, 200, 320, 100);
        }

        if (GUILayout.Button("Load"))
        {
            //展示广告
            TradPlus.LoadNativeBannerWithRect(_nativebannerAdUnits, 0, 200,320,100);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //展示广告

            TradPlus.ShowNativeBanner(_nativebannerAdUnits);
            _adUnitToShownMapping[_nativebannerAdUnits] = true;
        }

        GUI.enabled = true;
        if (GUILayout.Button("Hide"))
        {
            ClearStatusLabel();
            //隐藏广告
            TradPlus.HideNativeBanner(_nativebannerAdUnits);
            _adUnitToShownMapping[_nativebannerAdUnits] = false;
        }

        GUI.enabled = true;
        if (GUILayout.Button("Destroy"))
        {
            ClearStatusLabel();
            //销毁广告
            TradPlus.DestroyNativeBanner(_nativebannerAdUnits);
            _adUnitToLoadedMapping[_nativebannerAdUnits] = false;
            _adUnitToShownMapping[_nativebannerAdUnits] = false;
        }

        GUI.enabled = true;

        GUILayout.EndHorizontal();
    }
#endif
    private void CreateInterstitialsSection()
    {
        GUILayout.Label("Interstitials");


        GUILayout.BeginHorizontal();

        GUI.enabled = true;
#if UNITY_ANDROID
        if (GUILayout.Button(CreateRequestButtonLabel(_interstitialAdUnits)))
#elif UNITY_IOS
        if (GUILayout.Button("Load"))
#endif
        {
            UpdateStatusLabel("Requesting " + _interstitialAdUnits);

            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("user_age", "10");
            map.Add("user_gender", "male");//男性
            map.Add("user_level", "10");//游戏等级10
            TradPlus.initPlacementCustomMap(_interstitialAdUnits, map);
            TradPlus.RequestInterstitialAd(_interstitialAdUnits, false);

        }

        GUI.enabled = true;
        if (GUILayout.Button("IsReady"))
        {
            ClearStatusLabel();
            //Check是否有可用广告
            bool isReady = TradPlus.IsInterstialReady(_interstitialAdUnits);
            UpdateStatusLabel("是否有可用广告 ： " + isReady);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();

            //进入场景时调用 
            TradPlus.InterstitialEntryAdScenario(_interstitialAdUnits);
            //设置自动reload为true,无可用广告会自动加载；有广告时直接展示
            TradPlus.ShowInterstitialAd(_interstitialAdUnits);
        }

        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.enabled = true;
#if UNITY_ANDROID
        if (GUILayout.Button("Reload"))
        {
            ClearStatusLabel();
            //强制请求
            TradPlus.ReloadInterstitialAd(_interstitialAdUnits);
            _adUnitToShownMapping[_interstitialAdUnits] = false;
        }
#endif
        if (GUILayout.Button("EntryAdScenario"))
        {
            ClearStatusLabel();
            //进入广告场景
            TradPlus.InterstitialEntryAdScenario(_interstitialAdUnits, "44444");
            _adUnitToShownMapping[_nativebannerAdUnits] = false;
        }
        GUILayout.EndHorizontal();
    }


    private void CreateRewardedVideosSection()
    {
        GUILayout.Label("Rewarded Videos");

        GUILayout.BeginHorizontal();

        GUI.enabled = true;
#if UNITY_ANDROID
        if (GUILayout.Button(CreateRequestButtonLabel(_rewardedVideoAdUnits)))
#elif UNITY_IOS
        if (GUILayout.Button("Load"))
#endif
        {
            UpdateStatusLabel("Requesting " + _rewardedVideoAdUnits);

            // 流量分组（可选）
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("user_age", "20");
            map.Add("user_gender", "male");//男性
            map.Add("user_level", "20");//游戏等级20
            TradPlus.initPlacementCustomMap(_rewardedVideoAdUnits, map);

            // 设置服务器奖励 （可选）
            Dictionary<string, string> rewardedmap = new Dictionary<string, string>();
            rewardedmap.Add("user_id", "123");
            rewardedmap.Add("custom_data", "25");
            TradPlus.SetCustomParams(_rewardedVideoAdUnits, rewardedmap);

            TradPlus.RequestRewardedVideo(_rewardedVideoAdUnits, true);

        }

        GUI.enabled = true;
        if (GUILayout.Button("IsReady"))
        {
            ClearStatusLabel();
            //Check是否有可用广告
            bool isReady = TradPlus.HasRewardedVideo(_rewardedVideoAdUnits);
            UpdateStatusLabel("是否有可用广告 ： " + isReady);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //进入场景时调用
            TradPlus.RewardedVideoEntryAdScenario(_rewardedVideoAdUnits);
            //设置自动reload为true,无可用广告会自动加载；有广告时直接展示
            TradPlus.ShowRewardedVideo(_rewardedVideoAdUnits);

        }
#if UNITY_IOS
        if (GUILayout.Button("clearCache"))
        {
            TradPlus.RewardedVideoClearCache(_rewardedVideoAdUnits);
        }
#endif

        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.enabled = true;
#if UNITY_ANDROID
        if (GUILayout.Button("Clear"))
        {
            ClearStatusLabel();
            TradPlus.RewardedVideoClearCache(_rewardedVideoAdUnits);
            _adUnitToShownMapping[_rewardedVideoAdUnits] = false;
        }
#endif
        if (GUILayout.Button("广告场景"))
        {
            ClearStatusLabel();
            //进入广告场景
            TradPlus.RewardedVideoEntryAdScenario(_rewardedVideoAdUnits, "55555");
            _adUnitToShownMapping[_rewardedVideoAdUnits] = false;
        }
        GUILayout.EndHorizontal();
    }

    private void CreateOfferWallSection()
    {
        GUILayout.Label("OfferWall");

        GUILayout.BeginHorizontal();

        GUI.enabled = true;
#if UNITY_ANDROID
        if (GUILayout.Button(CreateRequestButtonLabel(_offerWallAdUnits)))
#elif UNITY_IOS
        if (GUILayout.Button("Load"))
#endif
        {
            UpdateStatusLabel("Requesting " + _offerWallAdUnits);
            TradPlus.RequestOfferWall(_offerWallAdUnits);

        }

        GUI.enabled = true;
        if (GUILayout.Button("IsReady"))
        {
            ClearStatusLabel();
            //Check是否有可用广告
            bool isReady = TradPlus.HasOfferWall(_offerWallAdUnits);
            Debug.Log("积分墙 是否有可用广告 : " + isReady);
            UpdateStatusLabel("是否有可用广告 ： " + isReady);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //进入场景时调用
            TradPlus.OfferWallEntryAdScenario(_offerWallAdUnits);
            //设置自动reload为true,无可用广告会自动加载；有广告时直接展示
            TradPlus.ShowOfferWall(_offerWallAdUnits);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.enabled = true;
        if (GUILayout.Button("查询总额"))
        {
            ClearStatusLabel();
            TradPlus.GetCurrencyBalance(_offerWallAdUnits);
        }

        GUI.enabled = true;
        if (GUILayout.Button("消耗积分"))
        {
            ClearStatusLabel();
            TradPlus.SpendCurrency(_offerWallAdUnits, 10);
        }

        GUI.enabled = true;
        if (GUILayout.Button("增加积分"))
        {
            ClearStatusLabel();
            TradPlus.AwardCurrency(_offerWallAdUnits, 20);
        }
        GUI.enabled = true;
        if (GUILayout.Button("设置用户名"))
        {
            ClearStatusLabel();
            TradPlus.SetOfferWallUserId(_offerWallAdUnits, "tp_unity_user_id");
        }
        GUILayout.EndHorizontal();
    }

    private void CreateGDPR()
    {
        GUILayout.Space(_sectionMarginSize);
        GUILayout.Label("GDPR");

        GUILayout.BeginHorizontal();

        GUI.enabled = true;
        if (GUILayout.Button(CreateRequestButtonLabel("isEUTraffic")))
        {
            ClearStatusLabel();
            bool isEUTraffic = TradPlus.isEUTraffic();
            UpdateStatusLabel("是否是欧盟用户 ： " + isEUTraffic);
        }

        GUI.enabled = true;
        if (GUILayout.Button("选择结果"))
        {
            ClearStatusLabel();
            int level = TradPlus.getGDPRDataCollection();
            if (level == 0)
            {
                UpdateStatusLabel("用户同意");
            }
            else if (level == 1)
            {
                UpdateStatusLabel("用户不同意");
            }
        }

        GUI.enabled = true;
        if (GUILayout.Button("Yes GDPRChild"))
        {
            ClearStatusLabel();
            TradPlus.setGDPRChild(true);
            UpdateStatusLabel("同意GDPRChild");

        }

        GUI.enabled = true;
        if (GUILayout.Button("NO GDPRChild"))
        {
            ClearStatusLabel();
            TradPlus.setGDPRChild(false);
            UpdateStatusLabel("不同意 GDPRChild");


        }

        GUI.enabled = true;
        GUILayout.EndHorizontal();

    }

    private void CreateCCPA()
    {
        GUILayout.Space(_sectionMarginSize);
        GUILayout.Label("权限隐私");

        GUILayout.BeginHorizontal();
#if UNITY_ANDROID
        GUI.enabled = true;
        if (GUILayout.Button(CreateRequestButtonLabel("关闭权限隐私")))
        {
            ClearStatusLabel();
            TradPlus.SetPrivacyUserAgree(false);
            UpdateStatusLabel("关闭权限隐私");
        }

        GUI.enabled = true;
        if (GUILayout.Button(CreateRequestButtonLabel("开启权限隐私")))
        {
            ClearStatusLabel();
            TradPlus.SetPrivacyUserAgree(true);
            UpdateStatusLabel("开启权限隐私");
        }

        GUI.enabled = true;
        if (GUILayout.Button(CreateRequestButtonLabel("权限隐私状态")))
        {
            ClearStatusLabel();
            bool IsPrivacyUserAgree = TradPlus.IsPrivacyUserAgree();

        }
#endif
        GUI.enabled = true;
        GUILayout.EndHorizontal();

    }

    public void CreateCOPPAChild()
    {
        GUILayout.Space(_sectionMarginSize);
        GUILayout.Label("COPPAChild");

        GUILayout.BeginHorizontal();

        GUI.enabled = true;
        if (GUILayout.Button(CreateRequestButtonLabel("Yes COPPAChild")))
        {
            ClearStatusLabel();
            TradPlus.setCOPPAIsAgeRestrictedUser(true);
            UpdateStatusLabel("同意COPPAChild");
        }

        GUI.enabled = true;
        if (GUILayout.Button("NO COPPAChild"))
        {
            ClearStatusLabel();
            TradPlus.setCOPPAIsAgeRestrictedUser(false);
            UpdateStatusLabel("不同意COPPAChild");

        }

        //GUI.enabled = true;
        //if (GUILayout.Button(CreateRequestButtonLabel("Yes CCPPA")))
        //{
        //    ClearStatusLabel();
        //    TradPlus.setCCPADoNotSell(true);
        //    UpdateStatusLabel("同意CCPA");


        //}

        //GUI.enabled = true;
        //if (GUILayout.Button("NO CCPA "))
        //{
        //    ClearStatusLabel();
        //    TradPlus.setCCPADoNotSell(false);
        //    UpdateStatusLabel("不同意CCPA");

        //}

        GUI.enabled = true;
        GUILayout.EndHorizontal();
    }

    public void GDPRSuccess(string appId)
    {
        bool isEUTraffic = TradPlus.isEUTraffic();
        ClearStatusLabel();
        //判断是否是欧盟用户
        if (isEUTraffic)
        {
#if UNITY_ANDROID
            bool isFirstShow = TradPlus.isFirstShow();
#elif UNITY_IOS
                    bool isFirstShow = (TradPlus.getGDPRDataCollection() == 0);
#endif
            //isFirstShow 默认false，没有弹过授权页；true，表示用户选择过
            if (isFirstShow)
            {
                UpdateStatusLabel("该用户是欧盟用户！用户已选择");

            }
            else
            {
                UpdateStatusLabel("该用户是欧盟用户！用户没有选择过");
                //是欧盟用户，弹出授权页，让客户选择
                TradPlus.showUploadDataNotifyDialog();
            }
        }
        else
        {
            UpdateStatusLabel("该用户是不欧盟用户！");

        }

    }

    public void GDPRFailed(string appId)
    {
        ClearStatusLabel();
        UpdateStatusLabel("未知国家");
#if UNITY_ANDROID
        bool isFirstShow = TradPlus.isFirstShow();
#elif UNITY_IOS
                bool isFirstShow = (TradPlus.getGDPRDataCollection() == 0);
#endif
        if (isFirstShow)
        {
            //客户已经作出过选择
            UpdateStatusLabel("未知国家用户！用户已选择");

        }
        else
        {
            UpdateStatusLabel("未知国家用户！用户没选择过");
            //弹出授权页，让客户选择
            TradPlus.showUploadDataNotifyDialog();
        }
    }


    private static void CreateCustomDataField(string fieldName, ref string customDataValue)
    {
        GUI.SetNextControlName(fieldName);
        customDataValue = GUILayout.TextField(customDataValue, GUILayout.MinWidth(200));
        if (Event.current.type != EventType.Repaint) return;
        if (GUI.GetNameOfFocusedControl() == fieldName && customDataValue == _customDataDefaultText)
        {
            // Clear default text when focused
            customDataValue = string.Empty;
        }
        else if (GUI.GetNameOfFocusedControl() != fieldName && string.IsNullOrEmpty(customDataValue))
        {
            // Restore default text when unfocused and empty
            customDataValue = _customDataDefaultText;
        }
    }


    private void CreateStatusSection()
    {
        GUILayout.Space(40);
        GUILayout.Label(_status, _smallerFont);
    }


    private static string GetCustomData(string customDataFieldValue)
    {
        return customDataFieldValue != _customDataDefaultText ? customDataFieldValue : null;
    }


    private static string CreateRequestButtonLabel(string adUnit)
    {
        return adUnit.Length > 10 ? "Request " + adUnit.Substring(0, 10) + "..." : adUnit;
    }

}
