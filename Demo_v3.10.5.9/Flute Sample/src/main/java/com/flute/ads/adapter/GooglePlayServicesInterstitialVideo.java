package com.flute.ads.adapter;

import android.content.Context;
import android.util.Log;

import com.flute.ads.common.LifecycleListener;
import com.flute.ads.mobileads.CustomEventInterstitial;
import com.flute.ads.mobileads.Flute;
import com.flute.ads.mobileads.FluteErrorCode;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.MobileAds;
import com.google.android.gms.ads.reward.RewardItem;
import com.google.android.gms.ads.reward.RewardedVideoAd;
import com.google.android.gms.ads.reward.RewardedVideoAdListener;

import java.lang.ref.WeakReference;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

/**
 * Created by sainase on 09/04/2019.
 */

public class GooglePlayServicesInterstitialVideo extends CustomEventInterstitial {

    public static final String AD_UNIT_ID_KEY = "placementId";
    public static final String CURRENCY_NAME_KEY = "currencyName";
    public static final String AMOUNT_KEY = "amount";

    private RewardedVideoAd mRewardedVideoAd;
    private CustomEventInterstitialListener mInterstitialListener;
    private WeakReference<Context> contextWeakReference;
    private long mAdLoadTimeStamp;
    private ArrayList<HashMap<String, String>> mAdEvents;
    private String mAdUnitId,mCurrencyName,mAmount,mParams;

    @Override
    protected void loadInterstitial(Context context,
                                    CustomEventInterstitialListener customEventInterstitialListener,
                                    Map<String, Object> localExtras,
                                    Map<String, String> serverExtras) {
        contextWeakReference = new WeakReference(context);
        mInterstitialListener = customEventInterstitialListener;
//        final String adUnitId;

        if (extrasAreValid(serverExtras)) {
            mAdUnitId = serverExtras.get(AD_UNIT_ID_KEY);
            mCurrencyName = serverExtras.get(CURRENCY_NAME_KEY);
            mAmount = serverExtras.get(AMOUNT_KEY);
        } else {
            mInterstitialListener.onInterstitialFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }


//        mAdUnitId = (String)localExtras.get(AD_UNIT_ID_KEY);
        mParams = serverExtras.toString();

        mAdLoadTimeStamp = System.currentTimeMillis();
        mAdEvents = new ArrayList<>();
        mAdEvents.add(Flute.getDebugEvent(mAdLoadTimeStamp, "loadAd", ""));

        MobileAds.initialize(context);
        mRewardedVideoAd = MobileAds.getRewardedVideoAdInstance(context);
        mRewardedVideoAd.setRewardedVideoAdListener(new InterstitialRewardAdListener());
        final AdRequest adRequest = new AdRequest.Builder().build();

        //TestDevice添加具体日志
//        final AdRequest adRequest = new AdRequest.Builder()
//                .addTestDevice("C949DECA5C1827A15A3C79C0A8F75424")
//                .build();

        try {
            mRewardedVideoAd.loadAd(mAdUnitId,adRequest);
        } catch (NoClassDefFoundError e) {
            mInterstitialListener.onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
        }
    }

    @Override
    protected void showInterstitial() {
        String msg;
        if (mRewardedVideoAd.isLoaded()) {
            mRewardedVideoAd.show();
            msg = "isAdLoaded";
        } else {
            Log.d("Flute", "Tried to show a Google Play Services rewarded video ad before it finished loading. Please try again.");
            msg = "noAdLoaded";
        }
        mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "showInterstitial", msg));
    }

    @Override
    protected void onInvalidate() {
        if (contextWeakReference.get() != null && mRewardedVideoAd != null) {
            mRewardedVideoAd.setRewardedVideoAdListener(null);
            mRewardedVideoAd = null;
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

    private class InterstitialRewardAdListener implements RewardedVideoAdListener {

        @Override
        public void onRewardedVideoAdLoaded() {
            if (mRewardedVideoAd == null) {
                return;
            }

            Log.d("Flute", "Google Play Services rewarded video ad loaded successfully.");
            if (contextWeakReference.get() != null && mInterstitialListener != null) {
                mInterstitialListener.onInterstitialLoaded();
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdLoaded", ""));

        }

        @Override
        public void onRewardedVideoAdOpened() {
            Log.d("Flute", "Showing Google Play Services rewarded video ad.");
            if (contextWeakReference.get() != null && mInterstitialListener != null) {
                mInterstitialListener.onInterstitialShown();
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdOpened", ""));
        }

        @Override
        public void onRewardedVideoStarted() {
            Log.d("Flute", "Google Play Services rewarded video ad started.");

            }

        @Override
        public void onRewardedVideoAdClosed() {
            Log.d("Flute", "Google Play Services rewarded video ad dismissed.");
            if (contextWeakReference.get() != null && mInterstitialListener != null) {
                mInterstitialListener.onInterstitialDismissed();
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdClosed", ""));
        }

        @Override
        public void onRewarded(RewardItem rewardItem) {
            Log.d("Flute", "Google Play Services rewarded video ad onRewardedVideoCompleted.");
            System.out.println("Google Admob Reward Type : " + rewardItem.getType() + " ， Amount ：" + rewardItem.getAmount());

            mInterstitialListener.onInterstitialRewarded(mCurrencyName, Integer.parseInt(mAmount));
//            mInterstitialListener.onInterstitialRewarded(mCurrencyName, rewardItem.getAmount());

        }

        @Override
        public void onRewardedVideoAdLeftApplication() {
            Log.d("Flute", "Google Play Services rewarded video ad clicked.");
            if (contextWeakReference.get() != null && mInterstitialListener != null) {
                mInterstitialListener.onInterstitialClicked();
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdLeftApplication", ""));

        }

        @Override
        public void onRewardedVideoAdFailedToLoad(int i) {
            Log.d("Flute", "Google Play Services rewarded video ad failed to load.");

            if (contextWeakReference.get() != null && mInterstitialListener != null) {
                mInterstitialListener.onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
            }

            mAdEvents.add(Flute.getDebugEvent(System.currentTimeMillis(), "onAdFailedToLoad", "errorCode : " + i));

        }

        @Override
        public void onRewardedVideoCompleted() {
            Log.d("Flute", "Google Play Services rewarded video ad completed.");
        }
    }
}
