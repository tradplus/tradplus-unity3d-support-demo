using UnityEngine;

public class TradPlusAndroidRewardedVideo
{
    private readonly AndroidJavaObject _rewardedVideoPlugin;


	public TradPlusAndroidRewardedVideo(string adUnitId)
    {
		_rewardedVideoPlugin = new AndroidJavaObject("com.tradplus.ads.unity.RewardedVideoUnityPlugin", adUnitId);
    }

    public void RequestRewardedVideo(bool autoReload = false)
    {
        _rewardedVideoPlugin.Call("request",autoReload);
    }

	public void ShowRewardedVideo()
    {
		_rewardedVideoPlugin.Call("show");
    }

    public void ShowRewardedVideoConfirmUWSAd()
    {
        _rewardedVideoPlugin.Call("confirmUWSAd");
    }

    public bool IsRewardedVideoReady {
		get { return _rewardedVideoPlugin.Call<bool>("isReady"); }
    }


	public void DestroyRewardedVideo()
    {
		_rewardedVideoPlugin.Call("destroy");
    }
		
}
