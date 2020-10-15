//
//  TradPlusManager.m
//  TradPlus
//
//  Copyright (c) 2017 __MyCompanyName__. All rights reserved.
//

#import "TradPlusManager.h"

#ifdef __cplusplus
extern "C" {
#endif
    // life cycle management
    void UnityPause(int pause);
    void UnitySendMessage(const char* obj, const char* method, const char* msg);
#ifdef __cplusplus
}
#endif


@implementation TradPlusManager

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

// Manager to be used for methods that do not require a specific adunit to operate on.
+ (TradPlusManager*)sharedManager
{
    static TradPlusManager* sharedManager = nil;

    if (!sharedManager)
        sharedManager = [[TradPlusManager alloc] init];

    return sharedManager;
}

// Manager to be used for adunit specific methods
+ (TradPlusManager*)managerForAdunit:(NSString*)adUnitId
{
    static NSMutableDictionary* managerDict = nil;

    if (!managerDict)
        managerDict = [[NSMutableDictionary alloc] init];

    TradPlusManager* manager = [managerDict valueForKey:adUnitId];
    if (!manager) {
        manager = [[TradPlusManager alloc] initWithAdUnit:adUnitId];
        managerDict[adUnitId] = manager;
    }

    return manager;
}


+ (UIViewController*)unityViewController
{
    return [[[UIApplication sharedApplication] keyWindow] rootViewController];
}


+ (void)sendUnityEvent:(NSString*)eventName withArgs:(NSArray*)args
{
    NSData* data = [NSJSONSerialization dataWithJSONObject:args options:0 error:nil];
    UnitySendMessage("TradPlusManager", eventName.UTF8String, [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding].UTF8String);
}


- (void)sendUnityEvent:(NSString*)eventName
{
    [[self class] sendUnityEvent:eventName withArgs:@[_adUnitId]];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Private

- (void)adjustAdViewFrameToShowAdView
{
    CGRect origFrame = _adView.frame;
    
    CGFloat screenHeight = [UIScreen mainScreen].bounds.size.height;
    CGFloat screenWidth = [UIScreen mainScreen].bounds.size.width;
    
    switch(_bannerPosition) {
        case TradPlusAdPositionTopLeft:
            origFrame.origin.x = 0;
            origFrame.origin.y = 0;
            _adView.autoresizingMask = (UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleBottomMargin);
            break;
        case TradPlusAdPositionTopCenter:
            origFrame.origin.x = (screenWidth / 2) - (origFrame.size.width / 2);
            _adView.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleBottomMargin);
            break;
        case TradPlusAdPositionTopRight:
            origFrame.origin.x = screenWidth - origFrame.size.width;
            origFrame.origin.y = 0;
            _adView.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleBottomMargin);
            break;
        case TradPlusAdPositionCentered:
            origFrame.origin.x = (screenWidth / 2) - (origFrame.size.width / 2);
            origFrame.origin.y = (screenHeight / 2) - (origFrame.size.height / 2);
            _adView.autoresizingMask = (UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleTopMargin | UIViewAutoresizingFlexibleBottomMargin);
            break;
        case TradPlusAdPositionBottomLeft:
            origFrame.origin.x = 0;
            origFrame.origin.y = screenHeight - origFrame.size.height;
            _adView.autoresizingMask = (UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin);
            break;
        case TradPlusAdPositionBottomCenter:
            origFrame.origin.x = (screenWidth / 2) - (origFrame.size.width / 2);
            origFrame.origin.y = screenHeight - origFrame.size.height;
            _adView.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin);
            break;
        case TradPlusAdPositionBottomRight:
            origFrame.origin.x = screenWidth - _adView.frame.size.width;
            origFrame.origin.y = screenHeight - origFrame.size.height;
            _adView.autoresizingMask = (UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleTopMargin);
            break;
    }
    
    _adView.frame = origFrame;
    NSLog(@"TradPlusSDKLog->fm->setting adView frame: %@", NSStringFromCGRect(origFrame));
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (id)initWithAdUnit:(NSString*)adUnitId
{
    self = [super init];
    if (self) self->_adUnitId = adUnitId;
    return self;
}

+ (BOOL)getIsIpad
{
    NSString *deviceType = [UIDevice currentDevice].model;
    
    if([deviceType isEqualToString:@"iPhone"]) {
        //iPhone
        return NO;
    }
    else if([deviceType isEqualToString:@"iPod touch"]) {
        //iPod Touch
        return NO;
    }
    else if([deviceType isEqualToString:@"iPad"]) {
        //iPad
        return YES;
    }
    return NO;
}

- (void)createBanner:(TradPlusAdPosition)position
{
    // kill the current adView if we have one
    if (_adView)
        [self hideBanner:YES];

    _bannerPosition = position;

    _adView = [[MsBannerView alloc] init];
    _adView.delegate = self;
    [_adView setAdUnitID:_adUnitId];
    
    if ([TradPlusManager getIsIpad])
        _adView.frame = CGRectMake(0, 0, 728, 90);
    else
        _adView.frame = CGRectMake(0, 0, 320, 50);

    _autorefresh = YES;
    [[TradPlusManager unityViewController].view addSubview:_adView];
    [_adView loadAd];
}


- (void)destroyBanner
{
    [_adView removeFromSuperview];
    //_adView.delegate = nil;
    self.adView = nil;
}


- (void)showBanner
{
    if (!_adView)
        return;
    
    _adView.hidden = NO;
}


- (void)hideBanner:(BOOL)shouldDestroy
{
    if (!_adView)
        return;

    _adView.hidden = YES;
    //[_adView stopAutomaticallyRefreshingContents];

    if (shouldDestroy)
        [self destroyBanner];
}

- (void)requestInterstitialAd:(BOOL)autoReload
{
    if (_interstitial == nil)
    {
        _interstitial = [[MsInterstitialAd alloc] init];
        [_interstitial setAdUnitID:_adUnitId isAutoLoad:autoReload];
        _interstitial.delegate = self;
    }

    [_interstitial loadAd];
}


- (BOOL)interstitialIsReady
{
    return _interstitial.isAdReady;
}

- (BOOL)interstitialEnterScene
{
    return _interstitial.isNetWorkAdReady;
}

- (void)showInterstitialAd
{
//    if (!_interstitial.isAdReady) {
//        NSLog(@"TradPlusSDKLog->fm->interstitial ad is not yet loaded");
//        return;
//    }

    [_interstitial showAdFromRootViewController:[TradPlusManager unityViewController]];
}


- (void)destroyInterstitialAd
{
    _interstitial.delegate = nil;
    _interstitial = nil;
}

- (void)requestRewardedVideo:(BOOL)autoReload
{
    if (_rewardedVideo == nil)
    {
        _rewardedVideo = [[MsRewardedVideoAd alloc] init];
        [_rewardedVideo setAdUnitID:_adUnitId isAutoLoad:autoReload];
        _rewardedVideo.delegate = self;
    }
    [_rewardedVideo loadAd];
}

- (BOOL)hasRewardedVideo
{
    return _rewardedVideo.isAdReady;
}

- (BOOL)rewardedVideoEnterScene
{
    return _rewardedVideo.isNetWorkAdReady;
}

- (void)showRewardedVideo
{
//    if (!_rewardedVideo.isAdReady) {
//        NSLog(@"TradPlusSDKLog->fm->rewarded ad is not yet loaded");
//        return;
//    }
    
    [_rewardedVideo showAdFromRootViewController:[TradPlusManager unityViewController]];
}

- (void)destroyRewardedVideo
{
    _rewardedVideo.delegate = nil;
    _rewardedVideo = nil;
}

- (void)requestOfferwall
{
    if (_offerwall == nil)
    {
        _offerwall = [[MsOfferwallAd alloc] init];
        [_offerwall setAdUnitID:_adUnitId];
        _offerwall.delegate = self;
    }
    [_offerwall loadAd];
}

- (BOOL)hasOfferwall
{
    return _rewardedVideo.isAdReady;
}

- (BOOL)offerwallEnterScene
{
    return _offerwall.isNetWorkAdReady;
}

- (void)showOfferwall
{
//    if (!_offerwall.isAdReady) {
//        NSLog(@"TradPlusSDKLog->fm->offerwall ad is not yet loaded");
//        return;
//    }
    
    [_offerwall showAdFromRootViewController:[TradPlusManager unityViewController]];
}

- (void)destroyOfferwall
{
    _offerwall.delegate = nil;
    _offerwall = nil;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - TradPlusViewDelegate
- (UIViewController *)viewControllerForPresentingModalView
{
    return [TradPlusManager unityViewController];
}

- (void)MsBannerViewLoaded:(MsBannerView *)adView
{
    // resize the banner
//        CGRect newFrame = _adView.frame;
//        newFrame.size = _adView.adContentViewSize;
//        _adView.frame = newFrame;
    //
    //CGRect frame = CGRectMake(0, 200, 320, 50); ///w
    CGRect frame = adView.frame;
    frame.size = CGSizeMake(320, 50);
    _adView.frame = frame;
    [self adjustAdViewFrameToShowAdView];
    _adView.hidden = NO;
    
    [[self class] sendUnityEvent:@"EmitAdLoadedEvent" withArgs:@[_adUnitId, @(_adView.frame.size.height), @"channel"]];
}

- (void)MsBannerView:(MsBannerView *)adView didFailWithError:(NSError *)error
{
    _adView.hidden = YES;
    [self sendUnityEvent:@"EmitAdFailedEvent"];
}

- (void)MsBannerViewImpression:(MsBannerView *)adView
{
}

- (void)MsBannerViewClicked:(MsBannerView *)adView
{
    NSLog(@"TradPlusSDKLog->fm->willPresentModalViewForAd");
    [self sendUnityEvent:@"EmitAdExpandedEvent"];
}

- (void)adViewShouldClose:(MsBannerView*)view
{
    NSLog(@"TradPlusSDKLog->fm->adViewShouldClose");
    [self hideBanner:YES];
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - TradPlusInterstitialDelegate
- (void)interstitialAdAllLoaded:(MsInterstitialAd *)interstitialAd readyCount:(int)readyCount
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s->ready:%d", _adUnitId, __FUNCTION__, readyCount);
    [[self class] sendUnityEvent:@"EmitInterstitialAllLoadedEvent" withArgs:@[@(readyCount > 0), _adUnitId]];
}

- (void)interstitialAdLoaded:(MsInterstitialAd *)interstitialAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s->%d", _adUnitId, __PRETTY_FUNCTION__, interstitialAd.readyAdCount);
    [self sendUnityEvent:@"EmitInterstitialLoadedEvent"];
}

- (void)interstitialAd:(MsInterstitialAd *)interstitialAd didFailWithError:(NSError *)error;
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s adUnit: %@ error: %@", _adUnitId, __PRETTY_FUNCTION__, _adUnitId, error);
    [[self class] sendUnityEvent:@"EmitInterstitialFailedEvent" withArgs:@[_adUnitId, [NSString stringWithFormat:@"%ld", (long)(error?error.code:0)]]];
}


- (void)interstitialAdShown:(MsInterstitialAd *)interstitialAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __PRETTY_FUNCTION__);
    //    UnityPause(true);
//    [self sendUnityEvent:@"EmitInterstitialShownEvent"];
    [[self class] sendUnityEvent:@"EmitInterstitialShownEvent" withArgs:@[_adUnitId, interstitialAd.channelName]];
}


- (void)interstitialAdClicked:(MsInterstitialAd *)interstitialAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __PRETTY_FUNCTION__);
    [self sendUnityEvent:@"EmitInterstitialClickedEvent"];
}


- (void)interstitialAdDismissed:(MsInterstitialAd *)interstitialAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __PRETTY_FUNCTION__);
    //    UnityPause(false);
    [self sendUnityEvent:@"EmitInterstitialDismissedEvent"];
}


#pragma mark - MsRewardedVideoAdDelegate implementation
- (void)rewardedVideoAdAllLoaded:(MsRewardedVideoAd *)rewardedVideoAd readyCount:(int)readyCount
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s->ready:%d", _adUnitId, __FUNCTION__, readyCount);
    [[self class] sendUnityEvent:@"EmitRewardedVideoAllLoadedEvent" withArgs:@[@(readyCount > 0), _adUnitId]];
}

- (void)rewardedVideoAdLoaded:(MsRewardedVideoAd *)rewardedVideoAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s->ready:%d", _adUnitId, __FUNCTION__, rewardedVideoAd.readyAdCount);
    [self sendUnityEvent:@"EmitRewardedVideoLoadedEvent"];
}

- (void)rewardedVideoAd:(MsRewardedVideoAd *)rewardedVideoAd didFailWithError:(NSError *)error
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s adUnit: %@ error: %@", _adUnitId, __PRETTY_FUNCTION__, _adUnitId, error);
    [[self class] sendUnityEvent:@"EmitRewardedVideoFailedEvent" withArgs:@[_adUnitId, @"unknown error"]];
}

- (void)rewardedVideoAdShown:(MsRewardedVideoAd *)rewardedVideoAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __FUNCTION__);
    [[self class] sendUnityEvent:@"EmitRewardedVideoShownEvent" withArgs:@[_adUnitId, rewardedVideoAd.channelName]];
}

- (void)rewardedVideoAdClicked:(MsRewardedVideoAd *)rewardedVideoAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __FUNCTION__);
    [self sendUnityEvent:@"EmitRewardedVideoClickedEvent"];
}

- (void)rewardedVideoAdDismissed:(MsRewardedVideoAd *)rewardedVideoAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __PRETTY_FUNCTION__);
    [self sendUnityEvent:@"EmitRewardedVideoDismissedEvent"];
}

- (void)rewardedVideoAdShouldReward:(MsRewardedVideoAd *)rewardedVideoAd reward:(MSRewardedVideoReward *)reward
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __FUNCTION__);
    [[self class] sendUnityEvent:@"EmitRewardedVideoReceivedRewardEvent"
                        withArgs:@[_adUnitId, reward?reward.currencyType:@"unknown", [NSString stringWithFormat:@"%ld", (long)(reward?[reward.amount integerValue]:0)]]];
}

#pragma mark -MsOfferwallAdDelegate
- (void)offerwallAdAllLoaded:(MsOfferwallAd *)offerwallAd readyCount:(int)readyCount
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s->ready:%d", _adUnitId, __FUNCTION__, readyCount);
    if (readyCount > 0)
    {
        [self sendUnityEvent:@"EmitOfferWallLoadedEvent"];
    }
    else {
        [[self class] sendUnityEvent:@"EmitOfferWallFailedEvent" withArgs:@[_adUnitId, @"unknown error"]];
    }
}

- (void)offerwallAdLoaded:(MsOfferwallAd *)offerwallAd
{
    
}

- (void)offerwallAd:(MsOfferwallAd *)offerwallAd didFailWithError:(NSError *)error
{
    
}

- (void)offerwallAdShown:(MsOfferwallAd *)offerwallAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __FUNCTION__);
    [[self class] sendUnityEvent:@"EmitOfferWallShownEvent" withArgs:@[_adUnitId, offerwallAd.channelName]];
}

- (void)offerwallAdClicked:(MsOfferwallAd *)offerwallAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __FUNCTION__);
    [self sendUnityEvent:@"EmitOfferWallClickedEvent"];
}

- (void)offerwallAdDismissed:(MsOfferwallAd *)offerwallAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __PRETTY_FUNCTION__);
    [self sendUnityEvent:@"EmitOfferWallDismissedEvent"];
}

- (void)offerwallAdShouldReward:(MsOfferwallAd *)offerwallAd reward:(MSRewardedVideoReward *)reward
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __FUNCTION__);
    [[self class] sendUnityEvent:@"EmitOfferWallReceivedRewardEvent"
                        withArgs:@[_adUnitId, reward?reward.currencyType:@"unknown", [NSString stringWithFormat:@"%ld", (long)(reward?[reward.amount integerValue]:0)]]];
}

@end
