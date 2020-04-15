using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
[SuppressMessage("ReSharper", "AccessToStaticMemberViaDerivedType")]
public class FluteEventListener : MonoBehaviour
{
    [SerializeField]
    private FluteDemoGUI _demoGUI;


    private void Awake()
    {
        if (_demoGUI == null)
            _demoGUI = GetComponent<FluteDemoGUI>();

        if (_demoGUI != null) return;
        Debug.LogError("Missing reference to FluteDemoGUI.  Please fix in the editor.");
        Destroy(this);
    }


    private void OnEnable()
    {
        FluteManager.OnSdkInitializedEvent += OnSdkInitializedEvent;

        FluteManager.OnAdLoadedEvent += OnAdLoadedEvent;
        FluteManager.OnAdFailedEvent += OnAdFailedEvent;
        FluteManager.OnAdClickedEvent += OnAdClickedEvent;
        FluteManager.OnAdCollapsedEvent += OnAdCollapsedEvent;

        FluteManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        FluteManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
        FluteManager.OnInterstitialShownEvent += OnInterstitialShownEvent;
        FluteManager.OnInterstitialClickedEvent += OnInterstitialClickedEvent;
        FluteManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;
        FluteManager.OnInterstitialExpiredEvent += OnInterstitialExpiredEvent;

        FluteManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
		FluteManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
		FluteManager.OnRewardedVideoShownEvent += OnRewardedVideoShownEvent;
		FluteManager.OnRewardedVideoClickedEvent += OnRewardedVideoClickedEvent;
		FluteManager.OnRewardedVideoDismissedEvent += OnRewardedVideoDismissedEvent;
		FluteManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;
        FluteManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;
    }


    private void OnDisable()
    {
        // Remove all event handlers
        FluteManager.OnSdkInitializedEvent -= OnSdkInitializedEvent;

        FluteManager.OnAdLoadedEvent -= OnAdLoadedEvent;
        FluteManager.OnAdFailedEvent -= OnAdFailedEvent;
        FluteManager.OnAdClickedEvent -= OnAdClickedEvent;
        FluteManager.OnAdCollapsedEvent -= OnAdCollapsedEvent;

        FluteManager.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
        FluteManager.OnInterstitialFailedEvent -= OnInterstitialFailedEvent;
        FluteManager.OnInterstitialShownEvent -= OnInterstitialShownEvent;
        FluteManager.OnInterstitialClickedEvent -= OnInterstitialClickedEvent;
        FluteManager.OnInterstitialDismissedEvent -= OnInterstitialDismissedEvent;
        FluteManager.OnInterstitialExpiredEvent -= OnInterstitialExpiredEvent;

        FluteManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
		FluteManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
		FluteManager.OnRewardedVideoShownEvent += OnRewardedVideoShownEvent;
		FluteManager.OnRewardedVideoClickedEvent += OnRewardedVideoClickedEvent;
		FluteManager.OnRewardedVideoDismissedEvent += OnRewardedVideoDismissedEvent;
        FluteManager.OnRewardedVideoReceivedRewardEvent -= OnRewardedVideoReceivedRewardEvent;
    }


    private void AdFailed(string adUnitId, string action, string error)
    {
        var errorMsg = "Failed to " + action + " ad unit " + adUnitId;
        if (!string.IsNullOrEmpty(error))
            errorMsg += ": " + error;
        Debug.LogError(errorMsg);
        _demoGUI.UpdateStatusLabel("Error: " + errorMsg);
    }


    private void OnSdkInitializedEvent(string adUnitId)
    {
        Debug.Log("OnSdkInitializedEvent: " + adUnitId);
        _demoGUI.SdkInitialized();
    }



    // Banner Events


    private void OnAdLoadedEvent(string adUnitId, float height)
    {
        Debug.Log("OnAdLoadedEvent: " + adUnitId + " height: " + height);
        _demoGUI.BannerLoaded(adUnitId, height);
    }


    private void OnAdFailedEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load banner", error);
    }


    private void OnAdClickedEvent(string adUnitId)
    {
        Debug.Log("OnAdClickedEvent: " + adUnitId);
    }


    private void OnAdExpandedEvent(string adUnitId)
    {
        Debug.Log("OnAdExpandedEvent: " + adUnitId);
    }


    private void OnAdCollapsedEvent(string adUnitId)
    {
        Debug.Log("OnAdCollapsedEvent: " + adUnitId);
    }


    // Interstitial Events


    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        Debug.Log("OnInterstitialLoadedEvent: " + adUnitId);
        _demoGUI.AdLoaded(adUnitId);
    }


    private void OnInterstitialFailedEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load interstitial", error);
    }


    private void OnInterstitialShownEvent(string adUnitId)
    {
        Debug.Log("OnInterstitialShownEvent: " + adUnitId);
    }


    private void OnInterstitialClickedEvent(string adUnitId)
    {
        Debug.Log("OnInterstitialClickedEvent: " + adUnitId);
    }


    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        Debug.Log("OnInterstitialDismissedEvent: " + adUnitId);
        _demoGUI.AdDismissed(adUnitId);
    }


    private void OnInterstitialExpiredEvent(string adUnitId)
    {
        Debug.Log("OnInterstitialExpiredEvent: " + adUnitId);
    }

 // Rewarded Video Events

	private void OnRewardedVideoLoadedEvent(string adUnitId)
	{
		Debug.Log("OnRewardedVideoLoadedEvent: " + adUnitId);
		_demoGUI.AdLoaded(adUnitId);
	}


	private void OnRewardedVideoFailedEvent(string adUnitId, string error)
	{
		AdFailed(adUnitId, "load RewardedVideo", error);
	}


	private void OnRewardedVideoShownEvent(string adUnitId)
	{
		Debug.Log("OnRewardedVideoShownEvent: " + adUnitId);
	}


	private void OnRewardedVideoClickedEvent(string adUnitId)
	{
		Debug.Log("OnRewardedVideoClickedEvent: " + adUnitId);
	}


	private void OnRewardedVideoDismissedEvent(string adUnitId)
	{
		Debug.Log("OnRewardedVideoDismissedEvent: " + adUnitId);
		_demoGUI.AdDismissed(adUnitId);
	}

	private void OnRewardedVideoReceivedRewardEvent(string adUnitId,string currencyName,int amount)
	{
		Debug.Log("OnRewardedVideoReceivedRewardEvent: " + adUnitId + "OnRewardedVideoReceivedRewardEvent:: " + currencyName + ":" + amount);
	}
	
}
