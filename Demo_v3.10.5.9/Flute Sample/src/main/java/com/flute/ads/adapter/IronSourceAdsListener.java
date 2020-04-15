package com.flute.ads.adapter;

import com.flute.ads.mobileads.FluteErrorCode;
import com.ironsource.mediationsdk.logger.IronSourceError;
import com.ironsource.mediationsdk.sdk.ISDemandOnlyRewardedVideoListener;

public class IronSourceAdsListener implements ISDemandOnlyRewardedVideoListener {
    private String placementId,currencyName,amount;
    private IronSourceInterstitialCallbackRouter ironSourceInterstitialCallbackRouter;

    public IronSourceAdsListener(String id,String currencyName,String amount){
        placementId = id;
        this.currencyName = currencyName;
        this.amount = amount;
        ironSourceInterstitialCallbackRouter = IronSourceInterstitialCallbackRouter.getInstance();
    }

    @Override
    public void onRewardedVideoAdLoadSuccess(String s) {
        ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialLoaded();
    }

    @Override
    public void onRewardedVideoAdLoadFailed(String s, IronSourceError ironSourceError) {
        ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
    }

    @Override
    public void onRewardedVideoAdOpened(String s) {
        ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialShown();
    }

    @Override
    public void onRewardedVideoAdClosed(String s) {
        ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialDismissed();
    }

    @Override
    public void onRewardedVideoAdShowFailed(String s, IronSourceError ironSourceError) {
        if ( ironSourceError.getErrorCode() == IronSourceError.ERROR_CODE_NO_ADS_TO_SHOW ) {
            ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
        } else {
            ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialFailed(FluteErrorCode.UNSPECIFIED);
        }
    }

    @Override
    public void onRewardedVideoAdClicked(String s) {
        ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialClicked();
    }

    @Override
    public void onRewardedVideoAdRewarded(String s) {
        String amount = ironSourceInterstitialCallbackRouter.getIronSourcePidReward(s).getAmount();
        String currency = ironSourceInterstitialCallbackRouter.getIronSourcePidReward(s).getCurrency();
        ironSourceInterstitialCallbackRouter.getListener(s)
                .onInterstitialRewarded(currency, Integer.parseInt(amount));
    }
}
