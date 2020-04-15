using UnityEngine;

public class FluteAndroidRewardedVideo
{
    private readonly AndroidJavaObject _rewardedVideoPlugin;


	public FluteAndroidRewardedVideo(string adUnitId)
    {
		_rewardedVideoPlugin = new AndroidJavaObject("com.flute.ads.unity.RewardedVideoUnityPlugin", adUnitId);
    }


	public void RequestRewardedVideo(string keywords = "", string userDataKeywords = "")
    {
		_rewardedVideoPlugin.Call("request", keywords, userDataKeywords);
    }


	public void ShowRewardedVideo()
    {
		_rewardedVideoPlugin.Call("show");
    }


	public bool IsRewardedVideoReady {
		get { return _rewardedVideoPlugin.Call<bool>("isReady"); }
    }


	public void DestroyRewardedVideo()
    {
		_rewardedVideoPlugin.Call("destroy");
    }
		
}
