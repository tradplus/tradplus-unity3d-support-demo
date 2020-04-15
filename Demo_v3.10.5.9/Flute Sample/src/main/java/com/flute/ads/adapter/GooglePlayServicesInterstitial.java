package com.flute.ads.adapter;

import android.content.Context;
import android.util.Log;

import com.flute.ads.common.DataKeys;
import com.flute.ads.common.LifecycleListener;
import com.flute.ads.mobileads.CustomEventInterstitial;
import com.flute.ads.mobileads.Flute;
import com.flute.ads.mobileads.FluteErrorCode;
import com.google.android.gms.ads.AdListener;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.InterstitialAd;
import com.google.android.gms.ads.MobileAds;

import java.lang.ref.WeakReference;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class GooglePlayServicesInterstitial extends CustomEventInterstitial {

    public static final String AD_UNIT_ID_KEY = "placementId";

    private InterstitialAd mGoogleInterstitialAd;

    private CustomEventInterstitialListener mInterstitialListener;
    private WeakReference<Context> contextWeakReference;

    private long mAdLoadTimeStamp;
    private ArrayList<HashMap<String, String>> mAdEvents;
    private String mAdUnitId;
    private String mParams;

    @Override
    protected void loadInterstitial(
            final Context context,
            final CustomEventInterstitialListener customEventInterstitialListener,
            final Map<String, Object> localExtras,
            final Map<String, String> serverExtras) {
        contextWeakReference = new WeakReference(context);
        mInterstitialListener = customEventInterstitialListener;
        final String adUnitId;

        MobileAds.initialize(context);
        if (extrasAreValid(serverExtras)) {
            adUnitId = serverExtras.get(AD_UNIT_ID_KEY);
        } else {
            mInterstitialListener.onInterstitialFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }

        mAdUnitId = (String)localExtras.get(DataKeys.AD_UNIT_ID_KEY);
        mParams = serverExtras.toString();

        mAdLoadTimeStamp = System.currentTimeMillis();
        mAdEvents = new ArrayList<>();
        mAdEvents.add(Flute.getDebugEvent(mAdLoadTimeStamp, "loadAd", ""));

        mGoogleInterstitialAd = new InterstitialAd(context);
        mGoogleInterstitialAd.setAdListener(new InterstitialAdListener());
        mGoogleInterstitialAd.setAdUnitId(adUnitId);

//        final AdRequest adRequest = new AdRequest.Builder().build();
      final AdRequest adRequest = new AdRequest.Builder()
                .addTestDevice("6B4BBE4293D1782CAF89EB361E013970")
                .build();

        try {
            mGoogleInterstitialAd.loadAd(adRequest);
        } catch (NoClassDefFoundError e) {
            mInterstitialListener.onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
        }
    }

    @Override
    protected void showInterstitial() {
        String msg;

        if (mGoogleInterstitialAd.isLoaded()) {
            mGoogleInterstitialAd.show();
            msg = "isAdLoaded";
        } else {
            Log.d("Flute", "Tried to show a Google Play Services interstitial ad before it finished loading. Please try again.");
            msg = "noAdLoaded";
        }

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "showInterstitial", msg));
    }

    @Override
    protected void onInvalidate() {
        if (contextWeakReference.get() != null && mGoogleInterstitialAd != null) {
            mGoogleInterstitialAd.setAdListener(null);
            mGoogleInterstitialAd = null;
        }

        Flute.sendDebugLog("interstitial", "admob", mAdUnitId, mParams, mAdLoadTimeStamp, mAdEvents.toString());
    }

    @Override
    protected LifecycleListener getLifecycleListener() {
        return null;
    }

    private boolean extrasAreValid(Map<String, String> serverExtras) {
        return serverExtras.containsKey(AD_UNIT_ID_KEY);
    }

    private class InterstitialAdListener extends AdListener {
        @Override
        public void onAdClosed() {
            Log.d("Flute", "Google Play Services interstitial ad dismissed.");
            if (contextWeakReference.get() != null && mInterstitialListener != null) {
                mInterstitialListener.onInterstitialDismissed();
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdClosed", ""));
        }

        @Override
        public void onAdFailedToLoad(int errorCode) {
            Log.d("Flute", "Google Play Services interstitial ad failed to load.");
            if (contextWeakReference.get() != null && mInterstitialListener != null) {
                mInterstitialListener.onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdFailedToLoad", "errorCode : " + errorCode));
        }

        @Override
        public void onAdLeftApplication() {
            Log.d("Flute", "Google Play Services interstitial ad clicked.");
            if (contextWeakReference.get() != null && mInterstitialListener != null) {
                mInterstitialListener.onInterstitialClicked();
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdLeftApplication", ""));
        }

        @Override
        public void onAdLoaded() {
            if (mGoogleInterstitialAd == null) {
                return;
            }

            Log.d("Flute", "Google Play Services interstitial ad loaded successfully.");
            if (contextWeakReference.get() != null && mInterstitialListener != null) {
                mInterstitialListener.onInterstitialLoaded();
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdLoaded", ""));
        }

        @Override
        public void onAdOpened() {
            Log.d("Flute", "Showing Google Play Services interstitial ad.");
            if (contextWeakReference.get() != null && mInterstitialListener != null) {
                mInterstitialListener.onInterstitialShown();
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdOpened", ""));
        }
    }
}
