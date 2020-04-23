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
    private readonly string _appId = "6640E7E3BDAC951B8F28D4C8C50E50B5";
    private readonly string _bannerAdUnits = "A24091715B4FCD50C0F2039A5AF7C4BB";
    private readonly string _interstitialAdUnits = "E609A0A67AF53299F2176C3A7783C46D";
    private readonly string _rewardedVideoAdUnits = "39DAC7EAC046676C5404004A311D1DB1";
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

    //插屏广告，广告关闭的时候进行重新加载
    public void InterstitialDismissed(string adUnit)
    {
        TradPlus.RequestInterstitialAd(_interstitialAdUnits);
    }

    //激励视频广告，广告关闭的时候进行重新加载
    public void RewardedVideoDismissed(string adUnit)
    {
        TradPlus.RequestRewardedVideo(_rewardedVideoAdUnits);
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
        AddAdUnitsToStateMaps(_interstitialAdUnits);
        AddAdUnitsToStateMaps(_rewardedVideoAdUnits);
    }


    private void Start()
    {
       
        var anyAdUnitId = _appId;
        //初始化TradPlus SDK
        TradPlus.InitializeSdk(new TradPlusBase.SdkConfiguration
        {
            AdUnitId = anyAdUnitId,
        });

        //添加测试设备
        TradPlusAndroid.SetNeedTestDevice(true);
        //facebook测试设备ID添加
        //注意：正式上线前注释
        TradPlusAndroid.SetFacebookTestDevice("c6fbde6c-c968-4c52-ad9b-bdbf32e8fc81");

        //初始化广告位，只需要在初始化SDK之后调用一次即可
        TradPlus.LoadBannerPluginsForAdUnits(_bannerAdUnits);
        TradPlus.LoadInterstitialPluginsForAdUnits(_interstitialAdUnits);
        TradPlus.LoadRewardedVideoPluginsForAdUnits(_rewardedVideoAdUnits);


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
        CreateInterstitialsSection();
        CreateRewardedVideosSection();
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
        }

        GUI.enabled = true;
        if (GUILayout.Button("Destroy"))
        {
            ClearStatusLabel();
            TradPlus.DestroyBanner(_bannerAdUnits);
            _adUnitToLoadedMapping[_bannerAdUnits] = false;
            _adUnitToShownMapping[_bannerAdUnits] = false;
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            TradPlus.ShowBanner(_bannerAdUnits, true);
            _adUnitToShownMapping[_bannerAdUnits] = true;
        }

        GUI.enabled = true;
        if (GUILayout.Button("Hide"))
        {
            ClearStatusLabel();
            TradPlus.ShowBanner(_bannerAdUnits, false);
            _adUnitToShownMapping[_bannerAdUnits] = false;
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
            //请求插屏广告
            TradPlus.RequestInterstitialAd(_interstitialAdUnits);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //判断是否有插屏广告加载成功
            if(TradPlus.IsInterstialReady(_interstitialAdUnits))
            {
                //展示插屏广告
                TradPlus.ShowInterstitialAd(_interstitialAdUnits);
            }
            else
            {
                //如果没有广告加载成功，重新加载
                TradPlus.RequestInterstitialAd(_interstitialAdUnits);
            }

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
            //请求激励视频
            TradPlus.RequestRewardedVideo(_rewardedVideoAdUnits);
        }

        GUI.enabled = true;
        if (GUILayout.Button("Show"))
        {
            ClearStatusLabel();
            //判断是否有激励视频加载成功
            if(TradPlus.HasRewardedVideo(_rewardedVideoAdUnits))
            {
                //展示广告 
                TradPlus.ShowRewardedVideo(_rewardedVideoAdUnits);
            }
            else 
            {
                //如果没有广告加载成功，重新加载
                TradPlus.RequestRewardedVideo(_rewardedVideoAdUnits);
            }

        }

        GUI.enabled = true;
        GUILayout.EndHorizontal();

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
