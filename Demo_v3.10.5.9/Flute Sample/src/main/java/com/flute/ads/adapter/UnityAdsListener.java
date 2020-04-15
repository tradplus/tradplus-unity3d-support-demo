package com.flute.ads.adapter;

import android.util.Log;

import com.flute.ads.mobileads.FluteErrorCode;
import com.unity3d.ads.IUnityAdsListener;
import com.unity3d.ads.UnityAds;
import com.unity3d.ads.misc.Utilities;

public class UnityAdsListener implements IUnityAdsListener {
    private String placementId, mCurrencyName, mAmount;
    private UnityInterstitialCallbackRouter unityInterstitialCallbackRouter;
    public boolean isRewarded() {
        return isRewarded;
    }

    public void setRewarded(boolean rewarded) {
        isRewarded = rewarded;
    }

    private boolean isRewarded;


    public UnityAdsListener(String id, String currencyName, String amount){
        placementId = id;
        mCurrencyName = currencyName;
        mAmount = amount;
        unityInterstitialCallbackRouter = UnityInterstitialCallbackRouter.getInstance();
    }

    public UnityAdsListener(String id, String currencyName, String amount,boolean isRewarded){
        placementId = id;
        mCurrencyName = currencyName;
        mAmount = amount;
        unityInterstitialCallbackRouter = UnityInterstitialCallbackRouter.getInstance();
        this.isRewarded = isRewarded;
    }

    @Override
    public void onUnityAdsReady(final String zoneId) {
        Utilities.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (placementId.equalsIgnoreCase(zoneId)) {
                    if (unityInterstitialCallbackRouter.getListener(zoneId) != null) {
                        unityInterstitialCallbackRouter.getListener(zoneId).onInterstitialLoaded();
                    }

                }
            }
        });

    }

    @Override
    public void onUnityAdsStart(String zoneId) {
        unityInterstitialCallbackRouter.getListener(zoneId).onInterstitialShown();
//        unityInterstitialCallbackRouter.getListener(zoneId).onInterstitialRewarded(mCurrencyName,Integer.parseInt(mAmount));

    }

    @Override
    public void onUnityAdsFinish(String zoneId, UnityAds.FinishState finishState) {
        if (!((UnityAdsListener)unityInterstitialCallbackRouter.getUnityAdsExtendedListeners(zoneId)).isRewarded) { //插屏
            if (finishState == UnityAds.FinishState.ERROR) {
                unityInterstitialCallbackRouter.getListener(placementId).onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
            }
        }else{//激励视频
            if(finishState == UnityAds.FinishState.COMPLETED) {
                if (mCurrencyName != null ) {
                    String amount = unityInterstitialCallbackRouter.getUnityPidCustom(zoneId).getAmount();
                    String currency = unityInterstitialCallbackRouter.getUnityPidCustom(zoneId).getCurrency();
                    unityInterstitialCallbackRouter.getListener(zoneId).onInterstitialRewarded(currency, Integer.parseInt(amount));
                }
            }else if(finishState == UnityAds.FinishState.ERROR){
                unityInterstitialCallbackRouter.getListener(placementId).onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
            }

        }

        unityInterstitialCallbackRouter.getListener(zoneId).onInterstitialDismissed();


    }

    @Override
    public void onUnityAdsError(UnityAds.UnityAdsError error, String message) {
        Log.d("Flute","UnityAds ad load failed，placement : " + placementId + ", error : " + error.name() + " , ErrorMessage : "+ message);
        unityInterstitialCallbackRouter.getListener(placementId).onInterstitialFailed(FluteErrorCode.UNSPECIFIED);
    }

}
