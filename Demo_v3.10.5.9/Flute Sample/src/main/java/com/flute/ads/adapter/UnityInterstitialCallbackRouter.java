package com.flute.ads.adapter;

import com.flute.ads.mobileads.CustomEventInterstitial;
import com.unity3d.ads.IUnityAdsListener;

import java.util.HashMap;
import java.util.Map;

public class UnityInterstitialCallbackRouter {

    private static UnityInterstitialCallbackRouter instance;
    private UnityPidReward mUnityPidReward;

    private final Map<String, IUnityAdsListener> unityAdsExtendedListeners = new HashMap<>();
    private final Map<String, CustomEventInterstitial.CustomEventInterstitialListener> listeners = new HashMap<>();
    private final Map<String, UnityPidReward> pidlisteners = new HashMap<>();//currency、amount

    public static UnityInterstitialCallbackRouter getInstance() {
        if(instance == null){
            instance = new UnityInterstitialCallbackRouter();
        }
        return instance;
    }

    public Map<String, CustomEventInterstitial.CustomEventInterstitialListener> getListeners() {
        return listeners;
    }

    public Map<String, UnityPidReward> getPidlisteners() {
        return pidlisteners;
    }

    public void addListener(String id, CustomEventInterstitial.CustomEventInterstitialListener listener){
        getListeners().put(id,listener);

    }
    public void addPidListener(String id, UnityPidReward unityPidReward) {
        getPidlisteners().put(id, unityPidReward);
    }

    public CustomEventInterstitial.CustomEventInterstitialListener getListener(String id){
        return getListeners().get(id);
    }

    public UnityPidReward getUnityPidCustom(String id) {
        return getPidlisteners().get(id);
    }

    public Map<String, IUnityAdsListener> getUnityAdsExtendedListeners() {
        return unityAdsExtendedListeners;
    }

    public void addUnityAdsExtendedListeners(String id, IUnityAdsListener listener){
        getUnityAdsExtendedListeners().put(id,listener);

    }

    public IUnityAdsListener getUnityAdsExtendedListeners(String id){
        return getUnityAdsExtendedListeners().get(id);
    }
}
