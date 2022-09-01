using System;
using System.Collections.Generic;
using UnityEngine;
using TradPlusInternal.ThirdParty.MiniJSON;

public class TradPlusAndroidRewardedVideo
{
    private readonly AndroidJavaObject _rewardedVideoPlugin;


	public TradPlusAndroidRewardedVideo(string adUnitId)
    {
		_rewardedVideoPlugin = new AndroidJavaObject("com.tradplus.ads.unity.RewardedVideoUnityPlugin", adUnitId);
    }

    // V740支持
    public void SetRewardedVideoCustomParams(Dictionary<string, object> map)
    {
        _rewardedVideoPlugin.Call("setCustomParams", Json.Serialize(map));
    }

    // 服务器奖励
    public void SetCustomParams(Dictionary<string, string> map)
    {
        _rewardedVideoPlugin.Call("setCustomParams", Json.Serialize(map));
    }

    //请求广告
    public void RequestRewardedVideo(bool autoReload = true)
    {
        _rewardedVideoPlugin.Call("request",autoReload);
    }

    //强制Reload
    public void ReloadRewardedVideoAd()
    {
        _rewardedVideoPlugin.Call("reloadRewardedVideoAd");
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

    // V740 支持
    public void RewardedVideoOnDestroy()
    {
        _rewardedVideoPlugin.Call("onDestroy");
    }

    // V760 支持
    public void RewardedVideoClearCache()
    {
        _rewardedVideoPlugin.Call("clearCacheAd");
    }
}
