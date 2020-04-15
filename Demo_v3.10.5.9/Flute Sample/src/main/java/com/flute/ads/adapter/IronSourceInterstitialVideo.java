package com.flute.ads.adapter;

import android.app.Activity;
import android.content.Context;
import android.os.Handler;
import android.text.TextUtils;

import com.ironsource.mediationsdk.IronSource;
import com.ironsource.mediationsdk.logger.IronSourceLogger;
import com.ironsource.mediationsdk.logger.LogListener;
import com.ironsource.mediationsdk.model.Placement;
import com.flute.ads.common.BaseLifecycleListener;
import com.flute.ads.common.DataKeys;
import com.flute.ads.common.LifecycleListener;
import com.flute.ads.mobileads.CustomEventInterstitial;
import com.flute.ads.mobileads.Flute;
import com.flute.ads.mobileads.FluteErrorCode;

import java.lang.ref.WeakReference;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class IronSourceInterstitialVideo extends CustomEventInterstitial {
    public static final String APP_ID_KEY = "appId";
    public static final String PLACEMENT_ID_KEY = "placementId";
    public static final String CURRENCY_NAME_KEY = "currencyName";
    public static final String AMOUNT_KEY = "amount";
    public static final String PASS_SCAN_KEY = "passScan";

    //    private CustomEventInterstitialListener mInterstitialListener;
    private WeakReference<Context> contextWeakReference;

    private String appKey, placementId, mCurrencyName, mAmount, mPassScan;

    private IronSourceInterstitialCallbackRouter ironSourceInterstitialCallbackRouter;

    @Override
    protected void loadInterstitial(final Context context,
                                    final CustomEventInterstitialListener customEventInterstitialListener,
                                    final Map<String, Object> localExtras,
                                    final Map<String, String> serverExtras) {
        contextWeakReference = new WeakReference(context);
//        mInterstitialListener = customEventInterstitialListener;
        ironSourceInterstitialCallbackRouter = IronSourceInterstitialCallbackRouter.getInstance();
//        IronSource.setAdaptersDebug(true);

        if (extrasAreValid(serverExtras)) {
            appKey = serverExtras.get(APP_ID_KEY);
            placementId = serverExtras.get(PLACEMENT_ID_KEY);
            mCurrencyName = serverExtras.get(CURRENCY_NAME_KEY);
            mAmount = serverExtras.get(AMOUNT_KEY);
            mPassScan = serverExtras.get(PASS_SCAN_KEY);
        } else {
            customEventInterstitialListener.onInterstitialFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }

        if (TextUtils.isEmpty(placementId)) {
            placementId = "0";
        }

        IronSourceAdsListener ironSourceAdsListener = new IronSourceAdsListener(placementId, mCurrencyName, mAmount);

        IronSource.setISDemandOnlyRewardedVideoListener(ironSourceAdsListener);
        ironSourceInterstitialCallbackRouter.addListener(placementId, customEventInterstitialListener);


        IronSourcePidReward ironSourcePidReward = new IronSourcePidReward(mCurrencyName, mAmount);
        ironSourceInterstitialCallbackRouter.addPidListener(placementId, ironSourcePidReward);

        IronSource.initISDemandOnly((Activity) context, appKey, IronSource.AD_UNIT.INTERSTITIAL, IronSource.AD_UNIT.REWARDED_VIDEO);

        Handler handler = new Handler();
        handler.postDelayed(new Runnable() {
            @Override
            public void run() {
                if (IronSource.isISDemandOnlyRewardedVideoAvailable(placementId)) {
                    customEventInterstitialListener.onInterstitialLoaded();
                } else {
                    IronSource.loadISDemandOnlyRewardedVideo(placementId);
                }

                try {
                    Context _context = contextWeakReference.get();
                    if (_context != null) {
                        IronSource.onResume((Activity) _context);
                    }
                } catch (Throwable th) {
                    th.printStackTrace();
                }
            }
        }, 5000);

    }

    @Override
    protected void showInterstitial() {
        String msg;
//        if (IronSource.isRewardedVideoAvailable()) {
//            if (placementId != null && placementId.length() > 0) {
        IronSource.showISDemandOnlyRewardedVideo(placementId);
//            } else {
//                IronSource.showRewardedVideo();
//            }
        msg = "isAdLoaded";
//        } else {
//            Log.d("TradPlus", "Tried to show a IronSource RewardedVideo ad before it finished loading. Please try again.");
//            msg = "noAdLoaded";
//        }

    }

    @Override
    protected void onInvalidate() {
        Context context = contextWeakReference.get();
        if (context != null) {
            IronSource.onPause((Activity) context);
        }

    }

    @Override
    protected LifecycleListener getLifecycleListener() {
        return new BaseLifecycleListener() {
            @Override
            public void onPause(final Activity activity) {
                IronSource.onPause(activity);
            }

            @Override
            public void onResume(final Activity activity) {
                IronSource.onResume(activity);
            }
        };
    }

    private boolean extrasAreValid(final Map<String, String> serverExtras) {
        return serverExtras.containsKey(APP_ID_KEY);
    }
}
