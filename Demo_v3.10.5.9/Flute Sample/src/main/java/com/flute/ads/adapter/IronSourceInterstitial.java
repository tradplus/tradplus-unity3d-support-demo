package com.flute.ads.adapter;

import android.app.Activity;
import android.content.Context;
import android.os.Handler;
import android.support.annotation.NonNull;
import android.text.TextUtils;
import android.util.Log;

import com.ironsource.mediationsdk.IronSource;
import com.ironsource.mediationsdk.logger.IronSourceLogger;
import com.ironsource.mediationsdk.logger.LogListener;
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

public class IronSourceInterstitial extends CustomEventInterstitial {
    public static final String APP_ID_KEY = "appId";
    public static final String PLACEMENT_ID_KEY = "placementId";

    //    private CustomEventInterstitialListener mInterstitialListener;
    private WeakReference<Context> contextWeakReference;

    private long mAdLoadTimeStamp;
    private String mAdUnitId;
    private String mParams;
    private IronSourceInterstitialCallbackRouter ironSourceInterstitialCallbackRouter;

    private String appKey, placementId;

    @Override
    protected void loadInterstitial(final Context context,
                                    final CustomEventInterstitialListener customEventInterstitialListener,
                                    final Map<String, Object> localExtras,
                                    final Map<String, String> serverExtras) {
        contextWeakReference = new WeakReference(context);
//        mInterstitialListener = customEventInterstitialListener;
        ironSourceInterstitialCallbackRouter = IronSourceInterstitialCallbackRouter.getInstance();

        mAdUnitId = (String) localExtras.get(DataKeys.AD_UNIT_ID_KEY);
        mParams = serverExtras.toString();

        mAdLoadTimeStamp = System.currentTimeMillis();

        if (extrasAreValid(serverExtras)) {
            appKey = serverExtras.get(APP_ID_KEY);
            placementId = serverExtras.get(PLACEMENT_ID_KEY);
        } else {
            customEventInterstitialListener.onInterstitialFailed(FluteErrorCode.ADAPTER_CONFIGURATION_ERROR);
            return;
        }
        if (TextUtils.isEmpty(placementId)) {
            placementId = "0";
        }
        IronSourceAdsInterstitialListener ironSourceAdsInterstitialListener = new IronSourceAdsInterstitialListener(placementId);
        IronSource.setISDemandOnlyInterstitialListener(ironSourceAdsInterstitialListener);
//        IronSource.setLogListener(new LogListener() {
//            @Override
//            public void onLog(IronSourceLogger.IronSourceTag ironSourceTag, String s, int i) {
//                if (!TradPlus.getIsIronSourceInit()) {
//                    if (s.contains("init")) {
//                        TradPlus.setIsIronSourceInit(true);
//                        Handler mHandler = new Handler();
//                        mHandler.postDelayed(new Runnable() {
//                            @Override
//                            public void run() {
//                                IronSource.loadISDemandOnlyInterstitial(placementId);
//                            }
//                        }, 5000);
//                    }else if(s.contains("Init Fail")){
//                        ironSourceInterstitialCallbackRouter.getListener(placementId).onInterstitialFailed(TradPlusErrorCode.NETWORK_NO_CONFIG);
//                    }
//                }
//            }
//        });
        ironSourceInterstitialCallbackRouter.addListener(placementId, customEventInterstitialListener);

        IronSource.initISDemandOnly((Activity) context, appKey, IronSource.AD_UNIT.INTERSTITIAL, IronSource.AD_UNIT.REWARDED_VIDEO);

        Handler handler = new Handler();
        handler.postDelayed(new Runnable() {
            @Override
            public void run() {
                if (IronSource.isISDemandOnlyInterstitialReady(placementId)) {
                    customEventInterstitialListener.onInterstitialLoaded();
                } else {
                    IronSource.loadISDemandOnlyInterstitial(placementId);
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

        if (IronSource.isISDemandOnlyInterstitialReady(placementId)) {
            if (placementId != null && placementId.length() > 0) {
                IronSource.showISDemandOnlyInterstitial(placementId);
            }
            msg = "isAdLoaded";
        } else {
            Log.d("TradPlus", "Tried to show a IronSource interstitial ad before it finished loading. Please try again.");
            msg = "noAdLoaded";
            ironSourceInterstitialCallbackRouter.getListener(placementId).onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
        }

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
            public void onPause(@NonNull Activity activity) {
                IronSource.onPause(activity);
            }

            @Override
            public void onResume(@NonNull Activity activity) {
                IronSource.onResume(activity);
            }
        };
    }

    private boolean extrasAreValid(final Map<String, String> serverExtras) {
        return serverExtras.containsKey(APP_ID_KEY);
    }


}
