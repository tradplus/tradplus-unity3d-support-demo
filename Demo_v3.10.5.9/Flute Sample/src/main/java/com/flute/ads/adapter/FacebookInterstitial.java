package com.flute.ads.adapter;

import android.content.Context;
import android.util.Log;

import com.facebook.ads.Ad;
import com.facebook.ads.AdError;
import com.facebook.ads.AdSettings;
import com.facebook.ads.AudienceNetworkAds;
import com.facebook.ads.InterstitialAd;
import com.facebook.ads.InterstitialAdListener;
import com.flute.ads.common.DataKeys;
import com.flute.ads.common.LifecycleListener;
import com.flute.ads.mobileads.CustomEventInterstitial;
import com.flute.ads.mobileads.Flute;
import com.flute.ads.mobileads.FluteErrorCode;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class FacebookInterstitial extends CustomEventInterstitial implements InterstitialAdListener {
    public static final String PLACEMENT_ID_KEY = "placementId";

    private InterstitialAd mFacebookInterstitial;
    private CustomEventInterstitialListener mInterstitialListener;

    private long mAdLoadTimeStamp;
    private ArrayList<HashMap<String, String>> mAdEvents;
    private String mAdUnitId;
    private String mParams;

    @Override
    protected void loadInterstitial(final Context context,
            final CustomEventInterstitialListener customEventInterstitialListener,
            final Map<String, Object> localExtras,
            final Map<String, String> serverExtras) {
        mInterstitialListener = customEventInterstitialListener;

        AudienceNetworkAds.initialize(context);
        AudienceNetworkAds.isInAdsProcess(context);

//        AdSettings.setDebugBuild(true);

        final String placementId;
        if (extrasAreValid(serverExtras)) {
            placementId = serverExtras.get(PLACEMENT_ID_KEY);
        } else {
            mInterstitialListener.onInterstitialFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }


        mAdUnitId = (String)localExtras.get(DataKeys.AD_UNIT_ID_KEY);
        mParams = serverExtras.toString();

        mAdLoadTimeStamp = System.currentTimeMillis();
        mAdEvents = new ArrayList<>();
        mAdEvents.add(Flute.getDebugEvent(mAdLoadTimeStamp, "loadAd", ""));

        AdSettings.addTestDevice("54a72477-c51a-4f9a-b3c1-456babf4349e");
//        AdSettings.addTestDevice("79a701ec-48dc-495b-a7a4-5144f8133d83");


        mFacebookInterstitial = new InterstitialAd(context, placementId);
        mFacebookInterstitial.setAdListener(this);
        mFacebookInterstitial.loadAd();
    }

    @Override
    protected void showInterstitial() {
        String msg;

        if (mFacebookInterstitial != null && mFacebookInterstitial.isAdLoaded()) {
            mFacebookInterstitial.show();
            msg = "isAdLoaded";
        } else {
            Log.d("Flute", "Tried to show a Facebook interstitial ad before it finished loading. Please try again.");
            if (mInterstitialListener != null) {
                onError(mFacebookInterstitial, AdError.INTERNAL_ERROR);
            } else {
                Log.d("Flute", "Interstitial listener not instantiated. Please load interstitial again.");
            }
            msg = "noAdLoaded";
        }

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "showInterstitial", msg));
    }

    @Override
    protected void onInvalidate() {

//      AdSettings.clearTestDevices();

        if (mFacebookInterstitial != null) {
            mFacebookInterstitial.destroy();
            mFacebookInterstitial = null;
        }

        Flute.sendDebugLog("interstitial", "facebook", mAdUnitId, mParams, mAdLoadTimeStamp, mAdEvents.toString());
    }

    @Override
    protected LifecycleListener getLifecycleListener() {
        return null;
    }

    @Override
    public void onAdLoaded(final Ad ad) {
        if (mFacebookInterstitial == null) {
            return;
        }

        Log.d("Flute", "Facebook interstitial ad loaded successfully.");
        mInterstitialListener.onInterstitialLoaded();

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdLoaded", ""));
    }

    @Override
    public void onError(final Ad ad, final AdError error) {
        Log.d("Flute","Facebook Interstitial ad load failed，error : " + error + ", ErrorCode : " + error.getErrorCode() + ", ErrorMessage : " + error.getErrorMessage());

        if (error.getErrorCode() == AdError.NO_FILL.getErrorCode()) {
            mInterstitialListener.onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
        } else if (error.getErrorCode() == AdError.INTERNAL_ERROR.getErrorCode()) {
            mInterstitialListener.onInterstitialFailed(FluteErrorCode.NETWORK_INVALID_STATE);
        } else {
            mInterstitialListener.onInterstitialFailed(FluteErrorCode.UNSPECIFIED);
        }

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onError", error.getErrorCode() + " : " + error.getErrorMessage()));

    }

    @Override
    public void onInterstitialDisplayed(final Ad ad) {
        Log.d("Flute", "Showing Facebook interstitial ad.");
        mInterstitialListener.onInterstitialShown();
        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onInterstitialDisplayed", ""));
    }

    @Override
    public void onAdClicked(final Ad ad) {
        Log.d("Flute", "Facebook interstitial ad clicked.");
        mInterstitialListener.onInterstitialClicked();

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdClicked", ""));
    }

    @Override
    public void onInterstitialDismissed(final Ad ad) {
        Log.d("Flute", "Facebook interstitial ad dismissed.");
        mInterstitialListener.onInterstitialDismissed();

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onInterstitialDismissed", ""));
    }

    @Override
    public void onLoggingImpression(Ad ad) {
        Log.d("Flute", "Facebook interstitial ad onLoggingImpression.");
        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onLoggingImpression", ""));
    }

    private boolean extrasAreValid(final Map<String, String> serverExtras) {
        final String placementId = serverExtras.get(PLACEMENT_ID_KEY);
        return (placementId != null && placementId.length() > 0);
    }
}
