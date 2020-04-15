package com.flute.ads.adapter;

import android.content.Context;
import android.support.annotation.NonNull;
import android.util.Log;

import com.flute.ads.common.DataKeys;
import com.flute.ads.mobileads.CustomEventAdView;
import com.flute.ads.mobileads.Flute;
import com.flute.ads.mobileads.FluteErrorCode;
import com.google.android.gms.ads.AdListener;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.AdSize;
import com.google.android.gms.ads.AdView;
import com.flute.ads.common.util.Views;
import com.google.android.gms.ads.MobileAds;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

import static com.google.android.gms.ads.AdSize.BANNER;
import static com.google.android.gms.ads.AdSize.FULL_BANNER;
import static com.google.android.gms.ads.AdSize.LEADERBOARD;
import static com.google.android.gms.ads.AdSize.MEDIUM_RECTANGLE;

class GooglePlayServicesBanner extends CustomEventAdView {

    public static final String PLACEMENT_ID_KEY = "placementId";
    private CustomEventAdViewListener mBannerListener;
    private AdView mGoogleAdView;

    private long mAdLoadTimeStamp;
    private ArrayList<HashMap<String, String>> mAdEvents;
    private String mAdUnitId;
    private String mParams;

    @Override
    protected void loadAdView(
            final Context context,
            final CustomEventAdViewListener customEventBannerListener,
            final Map<String, Object> localExtras,
            final Map<String, String> serverExtras) {
        mBannerListener = customEventBannerListener;
        
        mAdUnitId = (String)localExtras.get(DataKeys.AD_UNIT_ID_KEY);
        mParams = serverExtras.toString();

        mAdLoadTimeStamp = System.currentTimeMillis();
        mAdEvents = new ArrayList<>();
        mAdEvents.add(Flute.getDebugEvent(mAdLoadTimeStamp, "loadAd", ""));

        MobileAds.initialize(context);
        final String placementId;
        if (serverExtrasAreValid(serverExtras)) {
            placementId = serverExtras.get(PLACEMENT_ID_KEY);
        } else {
            mBannerListener.onAdViewFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }

        mGoogleAdView = new AdView(context);
        mGoogleAdView.setAdListener(new AdViewListener());
        mGoogleAdView.setAdUnitId(placementId);

        int width;
        int height;
        if (localExtrasAreValid(localExtras)) {
            width = (Integer) localExtras.get(DataKeys.AD_WIDTH);
            height = (Integer) localExtras.get(DataKeys.AD_HEIGHT);
        } else {
            mBannerListener.onAdViewFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return ;
        }

        final AdSize adSize = calculateAdSize(width, height);
        if (adSize == null) {
            mBannerListener.onAdViewFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }

        mGoogleAdView.setAdSize(adSize);

        final AdRequest adRequest = new AdRequest.Builder().build();
//      final AdRequest adRequest = new AdRequest.Builder()
//            .addTestDevice("341B4A290DD37464E4C43C33FA0F9CF6")
//            .build();

        try {
            mGoogleAdView.loadAd(adRequest);
        } catch (NoClassDefFoundError e) {
            mBannerListener.onAdViewFailed(FluteErrorCode.NETWORK_NO_FILL);
        }

    }

    @Override
    protected void onInvalidate() {

        if (mGoogleAdView != null) {
            Views.removeFromParent(mGoogleAdView);
            mGoogleAdView.setAdListener(null);
            mGoogleAdView.destroy();
            mGoogleAdView = null;
        }

        Flute.sendDebugLog("banner", "admob", mAdUnitId, mParams, mAdLoadTimeStamp, mAdEvents.toString());
    }

    private boolean localExtrasAreValid(@NonNull final Map<String, Object> localExtras) {
        return localExtras.get(DataKeys.AD_WIDTH) instanceof Integer
                && localExtras.get(DataKeys.AD_HEIGHT) instanceof Integer;
    }

    private AdSize calculateAdSize(int width, int height) {
        if (width <= BANNER.getWidth() && height <= BANNER.getHeight()) {
            return BANNER;
        } else if (width <= MEDIUM_RECTANGLE.getWidth() && height <= MEDIUM_RECTANGLE.getHeight()) {
            return MEDIUM_RECTANGLE;
        } else if (width <= FULL_BANNER.getWidth() && height <= FULL_BANNER.getHeight()) {
            return FULL_BANNER;
        } else if (width <= LEADERBOARD.getWidth() && height <= LEADERBOARD.getHeight()) {
            return LEADERBOARD;
        } else {
            return null;
        }
    }

    private class AdViewListener extends AdListener {
        @Override
        public void onAdClosed() {
            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdClosed", ""));
        }

        @Override
        public void onAdFailedToLoad(int errorCode) {
            Log.d("Flute", "Google Play Services banner ad failed to load.");
            if (mBannerListener != null) {
                mBannerListener.onAdViewFailed(FluteErrorCode.NETWORK_NO_FILL);
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdFailedToLoad", "errorCode : " + errorCode));
        }

        @Override
        public void onAdLeftApplication() {
            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdLeftApplication", ""));
        }

        @Override
        public void onAdLoaded() {
            if (mGoogleAdView == null) {
                return;
            }

            Log.d("Flute", "Google Play Services banner ad loaded successfully. Showing ad...");
            if (mBannerListener != null) {
                mBannerListener.onAdViewLoaded(mGoogleAdView);
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdLoaded", ""));
        }

        @Override
        public void onAdOpened() {
            Log.d("Flute", "Google Play Services banner ad clicked.");
            if (mBannerListener != null) {
                mBannerListener.onAdViewClicked();
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdOpened", ""));
        }
    }
    
    private boolean serverExtrasAreValid(final Map<String, String> serverExtras) {
        final String placementId = serverExtras.get(PLACEMENT_ID_KEY);
        return (placementId != null && placementId.length() > 0);
    }    
}
