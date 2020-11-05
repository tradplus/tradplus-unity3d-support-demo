using UnityEngine;

public class TradPlusAndroidRewardedVideo
{
    private readonly AndroidJavaObject _rewardedVideoPlugin;


	public TradPlusAndroidRewardedVideo(string adUnitId)
    {
		_rewardedVideoPlugin = new AndroidJavaObject("com.tradplus.ads.unity.RewardedVideoUnityPlugin", adUnitId);
    }

    //请求广告
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
        _rewardedVideoPlugin.Call("show", adSceneId);
    }

    //进入广告位所在界面时调用
    public void ShowRewardedVideoConfirmUWSAd()
    {
        _rewardedVideoPlugin.Call("entryAdScenario");
    }

    //进入广告位所在界面时调用
    public void RewardedVideoEntryAdScenario()
    {
        _rewardedVideoPlugin.Call("entryAdScenario");
    }

    //进入广告位所在界面时调用（广告场景ID）
    public void RewardedVideoEntryAdScenario(string adSceneId)
    {
        _rewardedVideoPlugin.Call("entryAdScenario",adSceneId);
    }

    //check是否有可用广告
    public bool IsRewardedVideoReady 
    {
		get { return _rewardedVideoPlugin.Call<bool>("isReady"); }
    }

    //Check广告缓存数是否已达配置上限
    public bool IsRewardedVideoAllReady
    {
        get { return _rewardedVideoPlugin.Call<bool>("isAllReady"); }
    }

    //销毁广告
    public void DestroyRewardedVideo()
    {
		_rewardedVideoPlugin.Call("destroy");
    }
		
}
