package com.flute.ads.adapter;

import com.flute.ads.mobileads.FluteErrorCode;
import com.ironsource.mediationsdk.logger.IronSourceError;
import com.ironsource.mediationsdk.sdk.ISDemandOnlyInterstitialListener;
import com.ironsource.mediationsdk.sdk.ISDemandOnlyRewardedVideoListener;

public class IronSourceAdsInterstitialListener implements ISDemandOnlyInterstitialListener {
    private String placementId;
    private IronSourceInterstitialCallbackRouter ironSourceInterstitialCallbackRouter;

    public IronSourceAdsInterstitialListener(String id){
        placementId = id;

        ironSourceInterstitialCallbackRouter = IronSourceInterstitialCallbackRouter.getInstance();
    }

    @Override
    public void onInterstitialAdReady(String s) {
        ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialLoaded();
    }

    @Override
    public void onInterstitialAdLoadFailed(String s, IronSourceError ironSourceError) {
        ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
    }

    @Override
    public void onInterstitialAdOpened(String s) {
        ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialShown();
    }

    @Override
    public void onInterstitialAdClosed(String s) {
        ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialDismissed();
    }

    @Override
    public void onInterstitialAdShowFailed(String s, IronSourceError ironSourceError) {
        if ( ironSourceError.getErrorCode() == IronSourceError.ERROR_CODE_NO_ADS_TO_SHOW ) {
            ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialFailed(FluteErrorCode.NETWORK_NO_FILL);
        } else {
            ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialFailed(FluteErrorCode.UNSPECIFIED);
        }
    }

    @Override
    public void onInterstitialAdClicked(String s) {
        ironSourceInterstitialCallbackRouter.getListener(s).onInterstitialClicked();
    }
}
