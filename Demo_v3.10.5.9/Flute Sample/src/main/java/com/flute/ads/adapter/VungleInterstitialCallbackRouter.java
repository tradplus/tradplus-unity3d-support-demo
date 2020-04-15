package com.flute.ads.adapter;

import com.flute.ads.mobileads.CustomEventInterstitial;

import java.util.HashMap;
import java.util.Map;

public class VungleInterstitialCallbackRouter {

    private static VungleInterstitialCallbackRouter instance;

    private final Map<String, CustomEventInterstitial.CustomEventInterstitialListener> listeners = new HashMap<>();

    public static VungleInterstitialCallbackRouter getInstance() {
        if(instance == null){
            instance = new VungleInterstitialCallbackRouter();
        }
        return instance;
    }

    public Map<String, CustomEventInterstitial.CustomEventInterstitialListener> getListeners() {
        return listeners;
    }

    public void addListener(String id, CustomEventInterstitial.CustomEventInterstitialListener listener){
        getListeners().put(id,listener);

    }

    public CustomEventInterstitial.CustomEventInterstitialListener getListener(String id){
        return getListeners().get(id);
    }


}
