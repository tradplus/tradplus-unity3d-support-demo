package com.flute.ads.adapter;

import com.flute.ads.mobileads.CustomEventInterstitial;

import java.util.HashMap;
import java.util.Map;

public class IronSourceInterstitialCallbackRouter {

    private static IronSourceInterstitialCallbackRouter instance;
    private IronSourcePidReward mIronSourcePidReward;

    private final Map<String, CustomEventInterstitial.CustomEventInterstitialListener> listeners = new HashMap<>();
    private final Map<String, IronSourcePidReward> pidlisteners = new HashMap<>();

    public static IronSourceInterstitialCallbackRouter getInstance() {
        if(instance == null){
            instance = new IronSourceInterstitialCallbackRouter();
        }
        return instance;
    }

    public Map<String, CustomEventInterstitial.CustomEventInterstitialListener> getListeners() {
        return listeners;
    }

    public Map<String, IronSourcePidReward> getPidlisteners() {
        return pidlisteners;
    }

    public void addListener(String id, CustomEventInterstitial.CustomEventInterstitialListener listener){
        getListeners().put(id,listener);

    }

    public void addPidListener(String id, IronSourcePidReward mIronSourcePidReward) {
        getPidlisteners().put(id, mIronSourcePidReward);
    }

    public IronSourcePidReward getIronSourcePidReward(String id) {
        return getPidlisteners().get(id);
    }

    public CustomEventInterstitial.CustomEventInterstitialListener getListener(String id){
        return getListeners().get(id);
    }


}
