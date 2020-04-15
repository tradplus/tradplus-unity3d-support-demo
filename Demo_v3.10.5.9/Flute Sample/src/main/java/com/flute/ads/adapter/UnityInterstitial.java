package com.flute.ads.adapter;

import android.app.Activity;
import android.content.Context;

import com.flute.ads.common.DataKeys;
import com.flute.ads.common.LifecycleListener;
import com.flute.ads.mobileads.CustomEventInterstitial;
import com.flute.ads.mobileads.Flute;
import com.flute.ads.mobileads.FluteErrorCode;
import com.unity3d.ads.UnityAds;
import com.unity3d.ads.properties.SdkProperties;

import java.lang.ref.WeakReference;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class UnityInterstitial extends CustomEventInterstitial {
    public static final String APP_ID_KEY = "appId";
    public static final String PLACEMENT_ID_KEY = "placementId";
    public static final String CURRENCY_NAME_KEY = "currencyName";
    public static final String AMOUNT_KEY = "amount";

//    private CustomEventInterstitialListener mInterstitialListener;

    private WeakReference<Context> contextWeakReference;
    private long mAdLoadTimeStamp;
    private ArrayList<HashMap<String, String>> mAdEvents;
    private String mAdUnitId;
    private String mParams;

    private String appId, placementId, mCurrencyName, mAmount;
    private UnityInterstitialCallbackRouter unityInterstitialCallbackRouter;

    @Override
    protected void loadInterstitial(final Context context,
                                    final CustomEventInterstitialListener customEventInterstitialListener,
                                    final Map<String, Object> localExtras,
                                    final Map<String, String> serverExtras) {
        contextWeakReference = new WeakReference(context);
//        mInterstitialListener = customEventInterstitialListener;

        unityInterstitialCallbackRouter = UnityInterstitialCallbackRouter.getInstance();

        mAdLoadTimeStamp = System.currentTimeMillis();
        mAdEvents = new ArrayList<>();
        mAdEvents.add(Flute.getDebugEvent(mAdLoadTimeStamp, "loadAd", ""));

        if (extrasAreValid(serverExtras)) {
            appId = serverExtras.get(APP_ID_KEY);
            placementId = serverExtras.get(PLACEMENT_ID_KEY);
            mCurrencyName = serverExtras.get(CURRENCY_NAME_KEY);
            mAmount = serverExtras.get(AMOUNT_KEY);
        } else {
            customEventInterstitialListener.onInterstitialFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }

        unityInterstitialCallbackRouter.addListener(placementId, customEventInterstitialListener);

        mAdUnitId = (String) localExtras.get(DataKeys.AD_UNIT_ID_KEY);
        mParams = serverExtras.toString();
        UnityAdsListener unityAdsListener = new UnityAdsListener(placementId,null,mAmount);
        if (!SdkProperties.isInitialized()) {
            UnityAds.initialize((Activity) context, appId, unityAdsListener, false);
            unityInterstitialCallbackRouter.addUnityAdsExtendedListeners(placementId,unityAdsListener);
        } else {
            UnityAds.setListener(unityAdsListener);
            unityInterstitialCallbackRouter.addUnityAdsExtendedListeners(placementId,unityAdsListener);
        }

        if (UnityAds.isReady(placementId)) {
            unityInterstitialCallbackRouter.getListener(placementId).onInterstitialLoaded();
        }
    }

    @Override
    protected void showInterstitial() {
        Activity context = (Activity) contextWeakReference.get();
        if (context != null) {
            UnityAds.show(context, placementId);
        }

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "showInterstitial", ""));
    }

    @Override
    protected void onInvalidate() {
        Flute.sendDebugLog("interstitial", "unity", mAdUnitId, mParams, mAdLoadTimeStamp, mAdEvents.toString());
    }

    @Override
    protected LifecycleListener getLifecycleListener() {
        return null;
    }

    private boolean extrasAreValid(final Map<String, String> serverExtras) {
        return serverExtras.containsKey(APP_ID_KEY) && serverExtras.containsKey(PLACEMENT_ID_KEY);
    }
}
