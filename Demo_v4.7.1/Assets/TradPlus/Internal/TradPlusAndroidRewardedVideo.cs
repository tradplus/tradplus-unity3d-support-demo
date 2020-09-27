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

    public void loadForcedlyRewardedVideo()
    {
        _rewardedVideoPlugin.Call("loadForcedly");
    }

    //展示广告
    public void ShowRewardedVideo()
    {
		_rewardedVideoPlugin.Call("show");
    }

    //展示广告（广告场景ID）
    public void ShowRewardedVideo(string adSceneId)
    {
        _rewardedVideoPlugin.Call("show");
    }

    //进入广告位所在界面时调用
    public void ShowRewardedVideoConfirmUWSAd()
    {
        _rewardedVideoPlugin.Call("confirmUWSAd");
    }

    //进入广告位所在界面时调用（广告场景ID）
    public void ShowRewardedVideoConfirmUWSAd(string adSceneId)
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
