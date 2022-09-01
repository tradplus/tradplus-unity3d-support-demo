using System;
using UnityEngine;
using System.Collections.Generic;
using TradPlusInternal.ThirdParty.MiniJSON;


public class TradPlusAndroidNativeBanner
{
    private readonly AndroidJavaObject _nativebannerPlugin;


    public TradPlusAndroidNativeBanner(string adUnitId)
    {
        _nativebannerPlugin = new AndroidJavaObject("com.tradplus.ads.unity.NativeBannerUnityPlugin", adUnitId);
    }

    // 设置AdSize，加载广告前调用
    // 不设置默认320 * 50 ，单位dp
    public void SetNativeBannerSize(int width, int height)
    {
        _nativebannerPlugin.Call("setNativeBannerSize", width, height);
    }

    public void SetNativeBannerCustomParams(Dictionary<string, object> map)
    {
        _nativebannerPlugin.Call("setCustomParams", Json.Serialize(map));
    }

    //加载广告
    //参数1:设置位置（底部中间、顶部中间）
    //参数2:(可选)adSceneId 广告场景ID
    //参数3:自定义布局文件name
    public void CreateNativeBanner(TradPlus.AdPosition position, string adSceneId = "", string LayoutIdByName = "")
    {
        _nativebannerPlugin.Call("createNativeBanner", (int)position, adSceneId, LayoutIdByName);
    }

    //8.0.30 废弃 HideNativeBanner(bool shouldShow)方法
    //8.0.30支持 隐藏广告
    public void HideNativeBanner()
    {
        _nativebannerPlugin.Call("hideNativeBanner");
    }

    //8.0.30支持 展示广告（隐藏后用于再次展示）
    public void DisplayNativeBanner()
    {
        _nativebannerPlugin.Call("displayNativeBanner");
    }

    //销毁广告
    public void DestroyNativeBanner()
    {
        _nativebannerPlugin.Call("destroyNativeBanner");
    }

    //进入广告场景
    public void NativeBannerEntryAdScenario(string adSceneId = "")
    {
        _nativebannerPlugin.Call("entryAdScenario", adSceneId);
    }
}
