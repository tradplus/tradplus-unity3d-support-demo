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
        TradPlusManager.OnSdkInitializedEvent += OnSdkInitializedEvent;

        //Banner
        //加载成功，并展示
        TradPlusManager.OnAdLoadedEvent += OnAdLoadedEvent;
        //加载失败
        TradPlusManager.OnAdFailedEvent += OnAdFailedEvent;
        //广告被点击
        TradPlusManager.OnAdClickedEvent += OnAdClickedEvent;
        TradPlusManager.OnAdCollapsedEvent += OnAdCollapsedEvent;

        //插屏广告
        //广告加载成功
        TradPlusManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        //广告加载失败
        TradPlusManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
        //广告展示
        TradPlusManager.OnInterstitialShownEvent += OnInterstitialShownEvent;
        //广告被点击
        TradPlusManager.OnInterstitialClickedEvent += OnInterstitialClickedEvent;
        //广告被关闭
        //**********建议：广告关闭时重新加载广告
        TradPlusManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;
        //针对整个广告位加载的结果做一个状态的返回（多缓存）
        TradPlusManager.OnInterstitialAllLoadedEvent += OnInterstitialAllLoadedEvent;


        //激励视频广告
        //广告加载成功
        TradPlusManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
        //广告加载失败
        TradPlusManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
        //广告展示
        TradPlusManager.OnRewardedVideoShownEvent += OnRewardedVideoShownEvent;
        //广告被点击
        TradPlusManager.OnRewardedVideoClickedEvent += OnRewardedVideoClickedEvent;
        //广告被关闭
        //**********建议：广告关闭时重新加载广告
        TradPlusManager.OnRewardedVideoDismissedEvent += OnRewardedVideoDismissedEvent;
        //视频播放完毕，奖励
        TradPlusManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;

        //针对整个广告位加载的结果做一个状态的返回（多缓存）
        TradPlusManager.OnRewardedVideoAllLoadedEvent += OnRewardedVideoAllLoadedEvent;

        //积分墙广告
        //广告加载成功
        TradPlusManager.OnOfferWallLoadedEvent += OnOfferWallLoadedEvent;
        //广告加载失败
        TradPlusManager.OnOfferWallFailedEvent += OnOfferWallFailedEvent;
        //广告展示
        TradPlusManager.OnOfferWallShownEvent += OnOfferWallShownEvent;
        //广告被点击
        TradPlusManager.OnOfferWallClickedEvent += OnOfferWallClickedEvent;
        //广告被关闭
        //**********建议：广告关闭时重新加载广告
        TradPlusManager.OnOfferWallDismissedEvent += OnOfferWallDismissedEvent;
        //奖励
        TradPlusManager.OnOfferWallReceivedRewardEvent += OnOfferWallReceivedRewardEvent;



        //GDPR
        TradPlusManager.OnGDPRSuccessEvent += OnGDPRSuccessEvent;
        TradPlusManager.OnGDPRFailedEvent += OnGDPRFailedEvent;
    }


    private void OnDisable()
    {
        // Remove all event handlers
        TradPlusManager.OnSdkInitializedEvent -= OnSdkInitializedEvent;

        TradPlusManager.OnAdLoadedEvent -= OnAdLoadedEvent;
        TradPlusManager.OnAdFailedEvent -= OnAdFailedEvent;
        TradPlusManager.OnAdClickedEvent -= OnAdClickedEvent;
        TradPlusManager.OnAdCollapsedEvent -= OnAdCollapsedEvent;

        TradPlusManager.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
        TradPlusManager.OnInterstitialFailedEvent -= OnInterstitialFailedEvent;
        TradPlusManager.OnInterstitialShownEvent -= OnInterstitialShownEvent;
        TradPlusManager.OnInterstitialClickedEvent -= OnInterstitialClickedEvent;
        TradPlusManager.OnInterstitialDismissedEvent -= OnInterstitialDismissedEvent;
        TradPlusManager.OnInterstitialAllLoadedEvent -= OnInterstitialAllLoadedEvent;

        TradPlusManager.OnRewardedVideoLoadedEvent -= OnRewardedVideoLoadedEvent;
        TradPlusManager.OnRewardedVideoFailedEvent -= OnRewardedVideoFailedEvent;
        TradPlusManager.OnRewardedVideoShownEvent -= OnRewardedVideoShownEvent;
        TradPlusManager.OnRewardedVideoClickedEvent -= OnRewardedVideoClickedEvent;
        TradPlusManager.OnRewardedVideoDismissedEvent -= OnRewardedVideoDismissedEvent;
        TradPlusManager.OnRewardedVideoReceivedRewardEvent -= OnRewardedVideoReceivedRewardEvent;
        TradPlusManager.OnRewardedVideoAllLoadedEvent -= OnRewardedVideoAllLoadedEvent;

        TradPlusManager.OnOfferWallLoadedEvent -= OnOfferWallLoadedEvent;
        TradPlusManager.OnOfferWallFailedEvent -= OnOfferWallFailedEvent;
        TradPlusManager.OnOfferWallShownEvent -= OnOfferWallShownEvent;
        TradPlusManager.OnOfferWallClickedEvent -= OnOfferWallClickedEvent;
        TradPlusManager.OnOfferWallDismissedEvent -= OnOfferWallDismissedEvent;
        TradPlusManager.OnOfferWallReceivedRewardEvent -= OnOfferWallReceivedRewardEvent;

        ////GDPR
        TradPlusManager.OnGDPRSuccessEvent -= OnGDPRSuccessEvent;
        TradPlusManager.OnGDPRFailedEvent -= OnGDPRFailedEvent;
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

    //GDPR
    private void OnGDPRSuccessEvent(string appId)
    {
        Debug.Log("onGDPRSuccessEvent: " + appId);
        _demoGUI.GDPRSuccess(appId);
    }

    private void OnGDPRFailedEvent(string appId)
    {
        Debug.Log("onGDPRFailedEvent: " + appId);
        _demoGUI.GDPRFailed(appId);
    }

    // Banner Events

    private void OnAdLoadedEvent(string adUnitId, float height,string channalName)
    {
        Debug.Log("OnAdLoadedEvent: " + adUnitId + " height: " + height + "channelName : " + channalName);
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


    private void OnInterstitialShownEvent(string adUnitId,string channelName)
    {
        Debug.Log("OnInterstitialShownEvent: " + adUnitId + "channel name: " + channelName);
    }


    private void OnInterstitialClickedEvent(string adUnitId)
    {
        Debug.Log("OnInterstitialClickedEvent: " + adUnitId);
    }


    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        Debug.Log("OnInterstitialDismissedEvent: " + adUnitId);
        _demoGUI.AdDismissed(adUnitId);
        //_demoGUI.InterstitialDismissed(adUnitId);
    }

    private void OnInterstitialAllLoadedEvent(bool isLoadedSucces, string adUnitId)
    {
        Debug.Log("OnInterstitialAllLoadedEvent: " + adUnitId + "OnInterstitialAllLoadedEvent:: " + " isLoadedSucces:" + isLoadedSucces);
        _demoGUI.OnInterstitialAllLoaded(isLoadedSucces,adUnitId);
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


	private void OnRewardedVideoShownEvent(string adUnitId,string channalName)
	{
		Debug.Log("OnRewardedVideoShownEvent: " + adUnitId + "channalName: " + channalName);
	}


	private void OnRewardedVideoClickedEvent(string adUnitId)
	{
		Debug.Log("OnRewardedVideoClickedEvent: " + adUnitId);
	}


	private void OnRewardedVideoDismissedEvent(string adUnitId)
	{
		Debug.Log("OnRewardedVideoDismissedEvent: " + adUnitId);
		_demoGUI.AdDismissed(adUnitId);
        //_demoGUI.RewardedVideoDismissed(adUnitId);

	}

	private void OnRewardedVideoReceivedRewardEvent(string adUnitId,string currencyName,int amount)
	{
		Debug.Log("OnRewardedVideoReceivedRewardEvent: " + adUnitId + "OnRewardedVideoReceivedRewardEvent:: " + currencyName + ":" + amount);
	}


    private void OnRewardedVideoAllLoadedEvent(bool isLoadedSucces,string adUnitId)
    {
        Debug.Log("OnRewardedVideoAllLoadedEvent: " + adUnitId + "OnRewardedVideoAllLoadedEvent:: " +" isLoadedSucces:" + isLoadedSucces);
        _demoGUI.OnRewardedVideoAllLoaded(isLoadedSucces, adUnitId);
    }


    //OfferWall Events

    private void OnOfferWallLoadedEvent(string adUnitId)
    {
        Debug.Log("OnOfferWallLoadedEvent: " + adUnitId);
        _demoGUI.AdLoaded(adUnitId);
    }


    private void OnOfferWallFailedEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load OfferWall", error);
    }


    private void OnOfferWallShownEvent(string adUnitId, string channalName)
    {
        Debug.Log("OnOfferWallShownEvent: " + adUnitId + "channalName: " + channalName);
    }


    private void OnOfferWallClickedEvent(string adUnitId)
    {
        Debug.Log("OnOfferWallClickedEvent: " + adUnitId);
    }


    private void OnOfferWallDismissedEvent(string adUnitId)
    {
        Debug.Log("OnOfferWallDismissedEvent: " + adUnitId);
        //_demoGUI.AdOfferWallDismissed(adUnitId);
        //_demoGUI.OfferWallDismissed(adUnitId);

    }

    private void OnOfferWallReceivedRewardEvent(string adUnitId, string currencyName, int amount)
    {
        Debug.Log("OnOfferWallReceivedRewardEvent: " + adUnitId + "OnOfferWallReceivedRewardEvent:: " + currencyName + ":" + amount);
    }
}
