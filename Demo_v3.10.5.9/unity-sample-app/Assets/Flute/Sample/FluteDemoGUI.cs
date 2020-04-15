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

    private readonly Dictionary<string, List<Flute.Reward>> _adUnitToRewardsMapping =
        new Dictionary<string, List<Flute.Reward>>();

#if UNITY_IOS
    private readonly string _bannerAdUnits = "5CED82906F7F68A5E0711939BF642529";

    private readonly string _interstitialAdUnits = "B0607DB46A83EF57209D9E13FCB31EA5";

    private readonly string _rewardedVideoAdUnits = "A5F4A1CFB8C6AACA978F8426050C37F4";
    private readonly string _rewardedVideoAdUnits2 = "C6EEF03D12B70DD46BC2DE3FC1D7F341";

    private readonly string[] _rewardedRichMediaAdUnits = { };
#elif UNITY_ANDROID || UNITY_EDITOR	
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

    public void LoadAvailableRewards(string adUnitId, List<Flute.Reward> availableRewards)
    {
        // Remove any existing available rewards associated with this AdUnit from previous ad requests
        _adUnitToRewardsMapping.Remove(adUnitId);

        if (availableRewards != null) {
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
		
    private void Awake()
    {
        if (Screen.width < 960 && Screen.height < 960) {
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
        // NOTE: the Flute SDK needs to be initialized on Start() to ensure all other objects have been enabled first.
        var anyAdUnitId = _bannerAdUnits;
        Flute.InitializeSdk(new FluteBase.SdkConfiguration {
            AdUnitId = anyAdUnitId,
        });

        Flute.LoadBannerPluginsForAdUnits(_bannerAdUnits);
        Flute.LoadInterstitialPluginsForAdUnits(_interstitialAdUnits);
        Flute.LoadRewardedVideoPluginsForAdUnits(_rewardedVideoAdUnits);


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
        GUI.Label(new Rect(0, 10, Screen.width, 60), Flute.PluginName, _centeredStyle);
        _centeredStyle.fontSize = prevFontSize;
        GUI.Label(new Rect(0, 70, Screen.width, 60), "with " + Flute.SdkName, _centeredStyle);
    }


    private void CreateBannersSection()
    {
        const int titlePadding = 102;
        GUILayout.Space(titlePadding);
        GUILayout.Label("Banners");

                GUILayout.BeginHorizontal();

                GUI.enabled = !_adUnitToLoadedMapping[_bannerAdUnits];
                if (GUILayout.Button(CreateRequestButtonLabel(_bannerAdUnits))) {
                    Debug.Log("requesting banner with AdUnit: " + _bannerAdUnits);
                    UpdateStatusLabel("Requesting " + _bannerAdUnits);
                    Flute.CreateBanner(_bannerAdUnits, Flute.AdPosition.BottomCenter);
                }

                GUI.enabled = true;
                if (GUILayout.Button("Destroy")) {
                    ClearStatusLabel();
                    Flute.DestroyBanner(_bannerAdUnits);
                    _adUnitToLoadedMapping[_bannerAdUnits] = false;
                    _adUnitToShownMapping[_bannerAdUnits] = false;
                }

                GUI.enabled =true;
                if (GUILayout.Button("Show")) {
                    ClearStatusLabel();
                    Flute.ShowBanner(_bannerAdUnits, true);
                    _adUnitToShownMapping[_bannerAdUnits] = true;
                }

                GUI.enabled = true;
                if (GUILayout.Button("Hide")) {
                    ClearStatusLabel();
                    Flute.ShowBanner(_bannerAdUnits, false);
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
                if (GUILayout.Button(CreateRequestButtonLabel(_interstitialAdUnits))) {
                    Debug.Log("requesting interstitial with AdUnit: " + _interstitialAdUnits);
                    UpdateStatusLabel("Requesting " + _interstitialAdUnits);
                    Flute.RequestInterstitialAd(_interstitialAdUnits);
                }

                GUI.enabled = true;
                if (GUILayout.Button("Show")) {
                    ClearStatusLabel();
                    Flute.ShowInterstitialAd(_interstitialAdUnits);
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
                if (GUILayout.Button(CreateRequestButtonLabel(_rewardedVideoAdUnits))) {
                    Debug.Log("requesting rewarded video with AdUnit: " + _rewardedVideoAdUnits);
                    UpdateStatusLabel("Requesting " + _rewardedVideoAdUnits);
                    Flute.RequestRewardedVideo(_rewardedVideoAdUnits);
                }

                GUI.enabled = true;
                if (GUILayout.Button("Show")) {
                    ClearStatusLabel();
                    Flute.ShowRewardedVideo(_rewardedVideoAdUnits);
                }

                GUI.enabled = true;
                GUILayout.EndHorizontal();

    }
		

    private static void CreateCustomDataField(string fieldName, ref string customDataValue)
    {
        GUI.SetNextControlName(fieldName);
        customDataValue = GUILayout.TextField(customDataValue, GUILayout.MinWidth(200));
        if (Event.current.type != EventType.Repaint) return;
        if (GUI.GetNameOfFocusedControl() == fieldName && customDataValue == _customDataDefaultText) {
            // Clear default text when focused
            customDataValue = string.Empty;
        } else if (GUI.GetNameOfFocusedControl() != fieldName && string.IsNullOrEmpty(customDataValue)) {
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
