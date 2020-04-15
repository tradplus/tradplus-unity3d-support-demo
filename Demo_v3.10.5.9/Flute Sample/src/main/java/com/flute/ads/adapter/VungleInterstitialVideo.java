package com.flute.ads.adapter;

import android.content.Context;
import android.util.Log;

import com.flute.ads.common.DataKeys;
import com.flute.ads.common.LifecycleListener;
import com.flute.ads.mobileads.CustomEventInterstitial;
import com.flute.ads.mobileads.Flute;
import com.flute.ads.mobileads.FluteErrorCode;
import com.vungle.warren.AdConfig;
import com.vungle.warren.InitCallback;
import com.vungle.warren.LoadAdCallback;
import com.vungle.warren.PlayAdCallback;
import com.vungle.warren.Vungle;

import java.lang.ref.WeakReference;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class VungleInterstitialVideo extends CustomEventInterstitial {
    public static final String APP_ID_KEY = "appId";
    public static final String VIDEO_TYPE_KEY = "vedioType";
    public static final String CURRENCY_NAME_KEY = "currencyName";
    public static final String AMOUNT_KEY = "amount";
    public static final String PLACEMENT_ID = "placementId";

//    private CustomEventInterstitialListener mInterstitialListener;

    private WeakReference<Context> contextWeakReference;
    private long mAdLoadTimeStamp;
    private ArrayList<HashMap<String, String>> mAdEvents;
    private String mAdUnitId;
    private String mParams;
    private VungleInterstitialCallbackRouter vungleInterstitialCallbackRouter;
    private String appId, mVideoType, mCurrencyName, mAmount,placementId;

    @Override
    protected void loadInterstitial(final Context context,
            final CustomEventInterstitialListener customEventInterstitialListener,
            final Map<String, Object> localExtras,
            final Map<String, String> serverExtras) {
        contextWeakReference = new WeakReference(context);
//        mInterstitialListener = customEventInterstitialListener;
        vungleInterstitialCallbackRouter = VungleInterstitialCallbackRouter.getInstance();

        mAdLoadTimeStamp = System.currentTimeMillis();
        mAdEvents = new ArrayList<>();
        mAdEvents.add(Flute.getDebugEvent(mAdLoadTimeStamp, "loadAd", ""));

        if (extrasAreValid(serverExtras)) {
            appId = serverExtras.get(APP_ID_KEY);
            mVideoType = serverExtras.get(VIDEO_TYPE_KEY);
            mCurrencyName = serverExtras.get(CURRENCY_NAME_KEY);
            mAmount = serverExtras.get(AMOUNT_KEY);
            placementId = serverExtras.get(PLACEMENT_ID);

        } else {
            customEventInterstitialListener.onInterstitialFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }

        mAdUnitId = (String)localExtras.get(DataKeys.AD_UNIT_ID_KEY);
        mParams = serverExtras.toString();
        vungleInterstitialCallbackRouter.addListener(placementId,customEventInterstitialListener);
        if (!Flute.getIsVungleInit()) {
            Vungle.init(appId, context.getApplicationContext(), new InitCallback() {
                @Override
                public void onSuccess() {
                    Log.d("Flute","Vungle SDK initialized.");
                    Log.d("Flute","Vungle SDK Version - " + com.vungle.warren.BuildConfig.VERSION_NAME);
                    Flute.setIsVungleInit(true);
                    Vungle.loadAd(placementId, vungleLoadAdCallback);
                }

                @Override
                public void onError(Throwable throwable) {
                    if (throwable != null) {
                        Log.d("Flute","InitCallback - onError: " + throwable.getLocalizedMessage());
                        vungleInterstitialCallbackRouter.getListener(placementId).onInterstitialFailed(FluteErrorCode.NO_FILL);
                    } else {
                        Log.d("Flute","Throwable is null");
                    }
                }

                @Override
                public void onAutoCacheAdAvailable(String placementId) {

                }
            });
        }

        if (Flute.getIsVungleInit()) {
            Vungle.loadAd(placementId, vungleLoadAdCallback);
        }
    }

    private AdConfig getAdConfig() {
        AdConfig adConfig = new AdConfig();
        adConfig.setBackButtonImmediatelyEnabled(true);
        adConfig.setAutoRotate(false);
        adConfig.setMuted(false);
        adConfig.setOrdinal(1);

        return adConfig;
    }

    @Override
    protected LifecycleListener getLifecycleListener() {
        return null;
    }

    @Override
    protected void showInterstitial() {
        if (Vungle.isInitialized()) {
            if (Vungle.canPlayAd(placementId)) {
                final AdConfig adConfig = getAdConfig();
                // Play Vungle ad
                Vungle.playAd(placementId, adConfig, vunglePlayAdCallback);
            }else{
                vungleInterstitialCallbackRouter.getListener(placementId).onInterstitialFailed(FluteErrorCode.NO_FILL);
            }
        }

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "showInterstitial", ""));
    }

    @Override
    protected void onInvalidate() {

        Flute.sendDebugLog("interstitial", "vungle", mAdUnitId, mParams, mAdLoadTimeStamp, mAdEvents.toString());
    }

    private final LoadAdCallback vungleLoadAdCallback = new LoadAdCallback() {
        @Override
        public void onAdLoad(final String placementReferenceID) {
            vungleInterstitialCallbackRouter.getListener(placementReferenceID).onInterstitialLoaded();
        }

        @Override
        public void onError(final String placementReferenceID, Throwable throwable) {
            vungleInterstitialCallbackRouter.getListener(placementReferenceID).onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);//.....
        }
    };

    /**
     * Callback handler for playing a Vungle advertisement. This is given as a parameter to {@link Vungle#playAd(String, AdConfig, PlayAdCallback)}
     * and is triggered when the advertisement begins to play, when the advertisement ends, and when
     * any errors occur.
     */
    private final PlayAdCallback vunglePlayAdCallback = new PlayAdCallback() {
        @Override
        public void onAdStart(final String placementReferenceID) {
            // Called before playing an ad.
            vungleInterstitialCallbackRouter.getListener(placementReferenceID).onInterstitialShown();
        }

        @Override
        public void onAdEnd(final String placementReferenceID, final boolean completed, final boolean isCTAClicked) {
            if (completed) {
                try {
                    vungleInterstitialCallbackRouter.getListener(placementReferenceID).onInterstitialRewarded(mCurrencyName, Integer.parseInt(mAmount));
                } catch (Exception e) {
//                    e.printStackTrace();
                }
            }

            if (isCTAClicked) {
                vungleInterstitialCallbackRouter.getListener(placementReferenceID).onInterstitialClicked();
            }

            vungleInterstitialCallbackRouter.getListener(placementReferenceID).onInterstitialDismissed();
        }

        @Override
        public void onError(final String placementReferenceID, Throwable throwable) {
            // Called when VunglePub.playAd() was called but no ad is available to show to the user.

            vungleInterstitialCallbackRouter.getListener(placementReferenceID).onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
        }
    };

    private boolean extrasAreValid(final Map<String, String> serverExtras) {
        return serverExtras.containsKey(APP_ID_KEY);
    }
}
