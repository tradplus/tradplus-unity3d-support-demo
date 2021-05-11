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
    private readonly string _bannerAdUnits = "5CED82906F7F68A5E0711939BF642529";

    private readonly string _interstitialAdUnits = "B0607DB46A83EF57209D9E13FCB31EA5";

    private readonly string _rewardedVideoAdUnits = "A5F4A1CFB8C6AACA978F8426050C37F4";
    private readonly string _rewardedVideoAdUnits2 = "C6EEF03D12B70DD46BC2DE3FC1D7F341";

    private readonly string[] _rewardedRichMediaAdUnits = { };
#elif UNITY_ANDROID || UNITY_EDITOR
    //测试广告位，上线前要替换成您申请的广告位
    private readonly string _appId = "44273068BFF4D8A8AFF3D5B11CBA3ADE";
    private readonly string _bannerAdUnits = "A24091715B4FCD50C0F2039A5AF7C4BB";
    private readonly string _nativeAdUnits = "DDBF26FBDA47FBE2765F1A089F1356BF";
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

    public void NativeLoaded(string adUnitId, float height) {
        _adUnitToLoadedMapping[adUnitId] = true;
    }

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

        AddAdUnitsToStateMaps(_bannerAdUnits);
        AddAdUnitsToStateMaps(_nativeAdUnits);
        AddAdUnitsToStateMaps(_interstitialAdUnits);
        AddAdUnitsToStateMaps(_rewardedVideoAdUnits);
        AddAdUnitsToStateMaps(_offerWallAdUnits);
        AddAdUnitsToStateMaps("GDPR");
        AddAdUnitsToStateMaps("CCPA");
        AddAdUnitsToStateMaps("COPPAChild");

    }


    private void Start()
    {
        //在初始化SDK之前调用
        TradPlus.setGDPRListener();


        var anyAdUnitId = _appId;
        //初始化TradPlus SDK
        TradPlus.InitializeSdk(new TradPlusBase.SdkConfiguration
        {
            AdUnitId = anyAdUnitId,
        });

        //添加测试设备,正式上线前注释
        TradPlusAndroid.SetNeedTestDevice(true);
      

        /*
       *          AdUnitID，TradPlus后台设置 对应广告类型的广告位（非三方广告网络的placementId）
       *          这里添加的是供测试使用的广告位，正式上线前必须替换成您申请的广告位
       *
       *          注意广告位不能填错，否则无法拿到广告
       *          仅在初始化广告位时调用一次
       */
        TradPlus.LoadBannerPluginsForAdUnits(_bannerAdUnits);
        TradPlus.LoadNativePluginsForAdUnits(_nativeAdUnits);
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
        CreateBannersSection();
        CreateNativesSection();
        CreateInterstitialsSection();
        CreateRewardedVideosSection();
        CreateOfferWallSection();
        CreateGDPR();
        CreateCCPA();
        CreateCOPPAChild();
        CreateStatusSection();

        GUILayout.EndArea();
    }


    private void CreateTitleSection()
    {
        // App title including Plugin and SDK versions
        var prevFontSize = _centeredStyle.fontSize;
        _centeredStyle.fontSize = 48;
        GUI.Label(new Rect(0, 10, Screen.width, 60), TradPlus.PluginName, _centeredStyle);
        _centeredStyle.fontSize = prevFontSize;
        GUI.Label(new Rect(0, 70, Screen.width, 60), "with " + TradPlus.SdkName, _centeredStyle);
    }

  
    private void CreateBannersSection()
    {
        const int titlePadding = 102;
        GUILayout.Space(titlePadding);
        GUILayout.Label("Banners");

        GUILayout.BeginHorizontal();

        GUI.enabled = !_adUnitToLoadedMapping[_bannerAdUnits];
        if (GUILayout.Button(CreateRequestButtonLabel(_bannerAdUnits)))
        {
            Debug.Log("requesting banner with AdUnit: " + _bannerAdUnits);
            UpdateStatusLabel("Requesting " + _bannerAdUnits);
            //请求Banner广告，并设置在底部弹出
            TradPlus.CreateBanner(_bannerAdUnits, TradPlus.AdPosition.BottomCenter);
            //进入广告场景页面调用
            TradPlus.BannerEntryAdScenario(_bannerAdUnits);
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

        GUI.enabled = true;

        GUILayout.EndHorizontal();
    }


    private void CreateNativesSection()
    {

        GUILayout.Label("Natives");

        GUILayout.BeginHorizontal();

        if (GUILayout.Button(CreateRequestButtonLabel(_nativeAdUnits)))
        {
            Debug.Log("requesting native with AdUnit: " + _nativeAdUnits);
            UpdateStatusLabel("Requesting " + _nativeAdUnits);
            //请求Native广告，并设置在底部弹出
            TradPlus.CreateNative(_nativeAdUnits,TradPlusBase.AdPosition.BottomCenter);
            //请求Native广告，设置 X Y
             //TradPlus.CreateNative(_nativeAdUnits,100,0);
            //进入广告场景页面调用
            TradPlus.NativeEntryAdScenario(_nativeAdUnits);
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

        GUI.enabled = true;

        GUILayout.EndHorizontal();
    }

   

    private void CreateInterstitialsSection()
    {
        GUILayout.Space(_sectionMarginSize);
        GUILayout.Label("Interstitials");


        GUILayout.BeginHorizontal();

        GUI.enabled = true;
        if (GUILayout.Button(CreateRequestButtonLabel(_interstitialAdUnits)))
        {
            Debug.Log("requesting interstitial with AdUnit: " + _interstitialAdUnits);
            UpdateStatusLabel("Requesting " + _interstitialAdUnits);

            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("user_age", "10");
            map.Add("user_gender", "male");//男性
            map.Add("user_level", "10");//游戏等级10
            TradPlus.initPlacementCustomMap(_interstitialAdUnits, map);
            TradPlus.RequestInterstitialAd(_interstitialAdUnits,false);

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
        if (GUILayout.Button("IsAllReady"))
        {
            ClearStatusLabel();
            //Check广告缓存数是否已达配置上限
            bool isReady = TradPlus.IsInterstitialAllReady(_interstitialAdUnits);
            UpdateStatusLabel("广告缓存数是否已达配置上限 ： " + isReady);
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


    }


    private void CreateRewardedVideosSection()
    {
        GUILayout.Space(_sectionMarginSize);
        GUILayout.Label("Rewarded Videos");

        GUILayout.BeginHorizontal();

        GUI.enabled = true;
        if (GUILayout.Button(CreateRequestButtonLabel(_rewardedVideoAdUnits)))
        {
            Debug.Log("requesting rewarded video with AdUnit: " + _rewardedVideoAdUnits);
            UpdateStatusLabel("Requesting " + _rewardedVideoAdUnits);
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("user_age", "20");
            map.Add("user_gender", "male");//男性
            map.Add("user_level", "20");//游戏等级20
            TradPlus.initPlacementCustomMap(_rewardedVideoAdUnits, map);
            TradPlus.RequestRewardedVideo(_rewardedVideoAdUnits, false);

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
        if (GUILayout.Button("IsAllReady"))
        {
            ClearStatusLabel();
        
            bool isReady = TradPlus.IsRewardedVideoAllReady(_rewardedVideoAdUnits);
            UpdateStatusLabel("Check广告缓存数是否已达配置上限 ： " + isReady);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //进入场景时调用
            TradPlus.RewardedVideoEntryAdScenario(_rewardedVideoAdUnits);
            //设置自动reload为true,无可用广告会自动加载；有广告时直接展示
            TradPlus.ShowRewardedVideo(_rewardedVideoAdUnits);

            //不设置自动reload为true
            //if(TradPlus.HasRewardedVideo(_rewardedVideoAdUnits))
            //{
            //    TradPlus.ShowRewardedVideo(_rewardedVideoAdUnits);
            //}else
            //{
            //    TradPlus.RequestRewardedVideo(_rewardedVideoAdUnits);
            //}

        }

        GUI.enabled = true;
        GUILayout.EndHorizontal();

    }


    private void CreateOfferWallSection()
    {
        GUILayout.Space(_sectionMarginSize);
        GUILayout.Label("OfferWall Videos");

        GUILayout.BeginHorizontal();

        GUI.enabled = true;
        if (GUILayout.Button(CreateRequestButtonLabel(_offerWallAdUnits)))
        {
            Debug.Log("requesting OfferWall video with AdUnit: " + _offerWallAdUnits);
            UpdateStatusLabel("Requesting " + _offerWallAdUnits);
            //请求积分墙
            TradPlus.RequestOfferWall(_offerWallAdUnits);

        }

        GUI.enabled = true;
        if (GUILayout.Button("IsReady"))
        {
            ClearStatusLabel();
            //Check是否有可用广告
            bool isReady = TradPlus.HasOfferWall(_offerWallAdUnits);
            UpdateStatusLabel("是否有可用广告 ： " + isReady);
        }

        GUI.enabled = true;
        if (GUILayout.Button("IsAllReady"))
        {
            ClearStatusLabel();

            bool isReady = TradPlus.IsOfferWallAllReady(_offerWallAdUnits);
            UpdateStatusLabel("Check广告缓存数是否已达配置上限 ： " + isReady);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //进入场景时调用
            TradPlus.OfferWallEntryAdScenario(_offerWallAdUnits);

            //判断是否有积分墙加载成功
            if (TradPlus.HasOfferWall(_offerWallAdUnits))
            {
                Debug.Log("HasOfferWall: " + _offerWallAdUnits);
                //展示广告 
                TradPlus.ShowOfferWall(_offerWallAdUnits);
            }

        }

        GUI.enabled = true;
        GUILayout.EndHorizontal();
    }

 
    private void CreateGDPR()
    {
        GUILayout.Space(_sectionMarginSize);
        GUILayout.Label("TradPlus GDPR");

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
        GUILayout.Label("CCPPA");

        GUILayout.BeginHorizontal();

        GUI.enabled = true;
        if (GUILayout.Button(CreateRequestButtonLabel("Yes CCPPA")))
        {
            ClearStatusLabel();
            TradPlus.setCCPADoNotSell(true);
            UpdateStatusLabel("同意CCPA");


        }

        GUI.enabled = true;
        if (GUILayout.Button("NO CCPA "))
        {
            ClearStatusLabel();
            TradPlus.setCCPADoNotSell(false);
            UpdateStatusLabel("不同意CCPA");

        }

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
            
            bool isFirstShow = TradPlus.isFirstShow();
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
        bool isFirstShow = TradPlus.isFirstShow();
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
