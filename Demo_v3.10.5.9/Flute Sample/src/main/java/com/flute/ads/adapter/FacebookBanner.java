package com.flute.ads.adapter;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.util.Log;

import com.facebook.ads.Ad;
import com.facebook.ads.AdError;
import com.facebook.ads.AdListener;
import com.facebook.ads.AdSettings;
import com.facebook.ads.AdSize;
import com.facebook.ads.AdView;
import com.facebook.ads.AudienceNetworkAds;
import com.flute.ads.common.DataKeys;
import com.flute.ads.common.util.Views;
import com.flute.ads.mobileads.CustomEventAdView;
import com.flute.ads.mobileads.Flute;
import com.flute.ads.mobileads.FluteErrorCode;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class FacebookBanner extends CustomEventAdView implements AdListener {
    public static final String PLACEMENT_ID_KEY = "placementId";

    private AdView mFacebookBanner;
    private CustomEventAdViewListener mBannerListener;

    private long mAdLoadTimeStamp;
    private ArrayList<HashMap<String, String>> mAdEvents;
    private String mAdUnitId;
    private String mParams;

    @Override
    protected void loadAdView(final Context context,
                              final CustomEventAdViewListener customEventBannerListener,
                              final Map<String, Object> localExtras,
                              final Map<String, String> serverExtras) {
        mBannerListener = customEventBannerListener;

        AudienceNetworkAds.initialize(context);
        AudienceNetworkAds.isInAdsProcess(context);

        mAdUnitId = (String)localExtras.get(DataKeys.AD_UNIT_ID_KEY);
        mParams = serverExtras.toString();

        mAdLoadTimeStamp = System.currentTimeMillis();
        mAdEvents = new ArrayList<>();
        mAdEvents.add(Flute.getDebugEvent(mAdLoadTimeStamp, "loadAd", ""));


        final String placementId;
        if (serverExtrasAreValid(serverExtras)) {
            placementId = serverExtras.get(PLACEMENT_ID_KEY);
        } else {
            mBannerListener.onAdViewFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }

        int width;
        int height;
        if (localExtrasAreValid(localExtras)) {
            width = (Integer) localExtras.get(DataKeys.AD_WIDTH);
            height = (Integer) localExtras.get(DataKeys.AD_HEIGHT);
        } else {
            mBannerListener.onAdViewFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return  ;
        }

        AdSize adSize = calculateAdSize(width, height);
        if (adSize == null) {
            mBannerListener.onAdViewFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }

        AdSettings.addTestDevice("54a72477-c51a-4f9a-b3c1-456babf4349e");


        mFacebookBanner = new AdView(context, placementId, adSize);
        mFacebookBanner.setAdListener(this);
        mFacebookBanner.disableAutoRefresh();
        mFacebookBanner.loadAd();

    }

    @Override
    protected void onInvalidate() {

//      AdSettings.clearTestDevices();

        if (mFacebookBanner != null) {
            Views.removeFromParent(mFacebookBanner);
            mFacebookBanner.destroy();
            mFacebookBanner = null;
        }

        Flute.sendDebugLog("banner", "facebook", mAdUnitId, mParams, mAdLoadTimeStamp, mAdEvents.toString());
    }

    @Override
    public void onAdLoaded(Ad ad) {
        if (mFacebookBanner == null) {
            return;
        }

        Log.d("Flute", "Facebook banner ad loaded successfully. Showing ad...");
        mBannerListener.onAdViewLoaded(mFacebookBanner);

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdLoaded", ""));
    }

    @Override
    public void onError(final Ad ad, final AdError error) {

        Log.d("Flute","Facebook banner ad load failed，error : " + error + ", ErrorCode : " + error.getErrorCode() + ", ErrorMessage : " + error.getErrorMessage());
        if (error.getErrorCode() == AdError.NO_FILL.getErrorCode()) {
            mBannerListener.onAdViewFailed(FluteErrorCode.NETWORK_NO_FILL);
        } else if (error.getErrorCode() == AdError.INTERNAL_ERROR.getErrorCode()) {
            mBannerListener.onAdViewFailed(FluteErrorCode.NETWORK_INVALID_STATE);
        } else {
            mBannerListener.onAdViewFailed(FluteErrorCode.UNSPECIFIED);
        }

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onError", error.getErrorCode() + " : " + error.getErrorMessage()));
    }

    @Override
    public void onAdClicked(Ad ad) {
        Log.d("Flute", "Facebook banner ad clicked.");
        mBannerListener.onAdViewClicked();

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdClicked", ""));
    }

    @Override
    public void onLoggingImpression(Ad ad) {
        Log.d("Flute", "Facebook banner ad onLoggingImpression.");
        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onLoggingImpression", ""));
    }

    private boolean serverExtrasAreValid(final Map<String, String> serverExtras) {
        final String placementId = serverExtras.get(PLACEMENT_ID_KEY);
        return (placementId != null && placementId.length() > 0);
    }

    private boolean localExtrasAreValid(@NonNull final Map<String, Object> localExtras) {
        return localExtras.get(DataKeys.AD_WIDTH) instanceof Integer
                && localExtras.get(DataKeys.AD_HEIGHT) instanceof Integer;
    }

    @Nullable
    private AdSize calculateAdSize(int width, int height) {
        if (height <= AdSize.BANNER_320_50.getHeight()) {
            return AdSize.BANNER_320_50;
        } else if (height <= AdSize.BANNER_HEIGHT_90.getHeight()) {
            return AdSize.BANNER_HEIGHT_90;
        } else if (height <= AdSize.RECTANGLE_HEIGHT_250.getHeight()) {
            return AdSize.RECTANGLE_HEIGHT_250;
        } else {
            return null;
        }
    }
}
