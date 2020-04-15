package com.flute.ads.adapter;

import android.content.Context;
import android.util.Log;

import com.facebook.ads.Ad;
import com.facebook.ads.AdError;
import com.facebook.ads.AdSettings;
import com.facebook.ads.AudienceNetworkAds;
import com.facebook.ads.RewardedVideoAd;
import com.facebook.ads.RewardedVideoAdListener;
import com.flute.ads.common.DataKeys;
import com.flute.ads.common.LifecycleListener;
import com.flute.ads.mobileads.CustomEventInterstitial;
import com.flute.ads.mobileads.Flute;
import com.flute.ads.mobileads.FluteErrorCode;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class FacebookInterstitialVideo extends CustomEventInterstitial implements RewardedVideoAdListener {
    public static final String PLACEMENT_ID_KEY = "placementId";
    public static final String CURRENCY_NAME_KEY = "currencyName";
    public static final String AMOUNT_KEY = "amount";

    private RewardedVideoAd mRewardedVideoAd;
    private CustomEventInterstitialListener mInterstitialListener;

    private long mAdLoadTimeStamp;
    private ArrayList<HashMap<String, String>> mAdEvents;
    private String mAdUnitId;
    private String mParams;

    private String mPlacementId, mCurrencyName, mAmount;

    @Override
    protected void loadInterstitial(final Context context,
            final CustomEventInterstitialListener customEventInterstitialListener,
            final Map<String, Object> localExtras,
            final Map<String, String> serverExtras) {
        mInterstitialListener = customEventInterstitialListener;

        AudienceNetworkAds.initialize(context);
        AudienceNetworkAds.isInAdsProcess(context);
        if (extrasAreValid(serverExtras)) {
            mPlacementId = serverExtras.get(PLACEMENT_ID_KEY);
            mCurrencyName = serverExtras.get(CURRENCY_NAME_KEY);
            mAmount = serverExtras.get(AMOUNT_KEY);
        } else {
            mInterstitialListener.onInterstitialFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }

        mAdUnitId = (String)localExtras.get(DataKeys.AD_UNIT_ID_KEY);
        mParams = serverExtras.toString();

        mAdLoadTimeStamp = System.currentTimeMillis();
        mAdEvents = new ArrayList<>();
        mAdEvents.add(Flute.getDebugEvent(mAdLoadTimeStamp, "loadAd", ""));

//        AdSettings.addTestDevice("2f0e7fe36af2496de60b7576a2201027");
        AdSettings.addTestDevice("54a72477-c51a-4f9a-b3c1-456babf4349e");

        mRewardedVideoAd = new RewardedVideoAd(context, mPlacementId);
        mRewardedVideoAd.setAdListener(this);
        mRewardedVideoAd.loadAd();
    }

    @Override
    protected void showInterstitial() {
        String msg;

        if (mRewardedVideoAd != null && mRewardedVideoAd.isAdLoaded()) {
            mRewardedVideoAd.show();
            msg = "isAdLoaded";
        } else {
            Log.d("Flute", "Tried to show a Facebook rewarded video ad before it finished loading. Please try again.");
            if (mInterstitialListener != null) {
                onError(mRewardedVideoAd, AdError.INTERNAL_ERROR);
            } else {
                Log.d("Flute", "rewarded video listener not instantiated. Please load interstitial again.");
            }
            msg = "noAdLoaded";
        }

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "showInterstitial", msg));
    }

    @Override
    protected void onInvalidate() {

//      AdSettings.clearTestDevices();

        if (mRewardedVideoAd != null) {
            mRewardedVideoAd.destroy();
            mRewardedVideoAd = null;
        }

        Flute.sendDebugLog("interstitial", "facebook", mAdUnitId, mParams, mAdLoadTimeStamp, mAdEvents.toString());
    }

    @Override
    protected LifecycleListener getLifecycleListener() {
        return null;
    }

    @Override
    public void onAdClicked(Ad ad) {

        Log.d("Flute", "Facebook rewarded video ad clicked.");

        mInterstitialListener.onInterstitialClicked();
        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdClicked", ""));
    }

    @Override
    public void onAdLoaded(Ad ad) {
        if (mRewardedVideoAd == null) {
            return;
        }

        Log.d("Flute", "Facebook rewarded video ad loaded successfully.");
        mInterstitialListener.onInterstitialLoaded();

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdLoaded", ""));
    }

    @Override
    public void onError(Ad ad, AdError error) {
            Log.d("Flute","Facebook RewardedVideo ad load failed，error : " + error + ", ErrorCode : " + error.getErrorCode() + ", ErrorMessage : " + error.getErrorMessage());

        if (error.getErrorCode() == AdError.NO_FILL.getErrorCode()) {
            FluteErrorCode fluteErrorCode = FluteErrorCode.NETWORK_NO_FILL;
            fluteErrorCode.setCode(error.getErrorCode()+"");
            fluteErrorCode.setErrorMessage(error.getErrorMessage());
            mInterstitialListener.onInterstitialFailed(fluteErrorCode);
        } else if (error.getErrorCode() == AdError.INTERNAL_ERROR.getErrorCode()) {
            mInterstitialListener.onInterstitialFailed(FluteErrorCode.NETWORK_INVALID_STATE);
        } else {
            mInterstitialListener.onInterstitialFailed(FluteErrorCode.UNSPECIFIED);
        }

        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onError", error.getErrorCode() + " : " + error.getErrorMessage()));

    }

    @Override
    public void onLoggingImpression(Ad ad) {
        Log.d("Flute", "Facebook rewarded video ad onLoggingImpression.");
        mInterstitialListener.onInterstitialShown();
        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onLoggingImpression", ""));
    }

    @Override
    public void onRewardedVideoClosed() {
        Log.d("Flute", "Facebook rewarded video ad onRewardedVideoClosed.");

        mInterstitialListener.onInterstitialDismissed();
        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onRewardedVideoClosed", ""));
    }

    @Override
    public void onRewardedVideoCompleted() {
        Log.d("Flute", "Facebook rewarded video ad onRewardedVideoCompleted.");

        mInterstitialListener.onInterstitialRewarded(mCurrencyName, Integer.parseInt(mAmount));
        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onRewardedVideoCompleted", ""));
    }

    private boolean extrasAreValid(final Map<String, String> serverExtras) {
        final String placementId = serverExtras.get(PLACEMENT_ID_KEY);
        return (placementId != null && placementId.length() > 0);
    }
}
