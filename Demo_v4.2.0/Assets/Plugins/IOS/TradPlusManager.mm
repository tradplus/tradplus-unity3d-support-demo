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

- (void)createBanner:(TradPlusAdPosition)position
{
    // kill the current adView if we have one
    if (_adView)
        [self hideBanner:YES];

    _bannerPosition = position;

    _adView = [[MsBannerView alloc] init];
    _adView.delegate = self;
    [_adView setAdUnitID:_adUnitId];

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

- (void)requestInterstitialAd
{
    if (_interstitial == nil)
    {
        _interstitial = [[MsInterstitialAdP alloc] init];
        [_interstitial setAdUnitID:_adUnitId];
        _interstitial.delegate = self;
    }

    [_interstitial loadAd];
}


- (BOOL)interstitialIsReady
{
    return _interstitial.isAdReady;
}


- (void)showInterstitialAd
{
    if (!_interstitial.isAdReady) {
        NSLog(@"TradPlusSDKLog->fm->interstitial ad is not yet loaded");
        return;
    }

    [_interstitial showAdFromRootViewController:[TradPlusManager unityViewController]];
}


- (void)destroyInterstitialAd
{
    _interstitial.delegate = nil;
    _interstitial = nil;
}

- (void)requestRewardedVideo
{
    if (_rewardedVideo == nil)
    {
        _rewardedVideo = [[MsRewardedVideoAdP alloc] init];
        [_rewardedVideo setAdUnitID:_adUnitId];
        _rewardedVideo.delegate = self;
    }
    [_rewardedVideo loadAd];
}

- (BOOL)hasRewardedVideo
{
    return _rewardedVideo.isAdReady;
}

- (void)showRewardedVideo
{
    if (!_rewardedVideo.isAdReady) {
        NSLog(@"TradPlusSDKLog->fm->rewarded ad is not yet loaded");
        return;
    }
    
    [_rewardedVideo showAdFromRootViewController:[TradPlusManager unityViewController]];
}

- (void)destroyRewardedVideo
{
    _rewardedVideo.delegate = nil;
    _rewardedVideo = nil;
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - TradPlusViewDelegate
- (UIViewController *)viewControllerForPresentingModalView
{
    return [TradPlusManager unityViewController];
}

- (void)MsBannerViewDidLoaded:(MsBannerView *)adView
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
    
    [[self class] sendUnityEvent:@"EmitAdLoadedEvent" withArgs:@[_adUnitId, @(_adView.frame.size.height)]];
}

- (void)MsBannerView:(MsBannerView *)adView didFailWithError:(NSError *)error
{
    _adView.hidden = YES;
    [self sendUnityEvent:@"EmitAdFailedEvent"];
}

- (void)MsBannerViewImpression:(MsBannerView *)adView
{
}

- (void)MsBannerViewDidClick:(MsBannerView *)adView
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
- (void)interstitialAdDidLoad:(MsInterstitialAdP *)interstitialAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s->%d", _adUnitId, __PRETTY_FUNCTION__, interstitialAd.readyAdCount);
    [self sendUnityEvent:@"EmitInterstitialLoadedEvent"];
}

- (void)interstitialAd:(MsInterstitialAdP *)interstitialAd didFailWithError:(NSError *)error;
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s adUnit: %@ error: %@", _adUnitId, __PRETTY_FUNCTION__, _adUnitId, error);
    [[self class] sendUnityEvent:@"EmitInterstitialFailedEvent" withArgs:@[_adUnitId, [NSString stringWithFormat:@"%ld", (long)(error?error.code:0)]]];
}


- (void)interstitialAdImpression:(MsInterstitialAdP *)interstitialAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __PRETTY_FUNCTION__);
    //    UnityPause(true);
    [self sendUnityEvent:@"EmitInterstitialShownEvent"];
}


- (void)interstitialAdDidClick:(MsInterstitialAdP *)interstitialAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __PRETTY_FUNCTION__);
    [self sendUnityEvent:@"EmitInterstitialClickedEvent"];
}


- (void)interstitialAdDismissed:(MsInterstitialAdP *)interstitialAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __PRETTY_FUNCTION__);
    //    UnityPause(false);
    [self sendUnityEvent:@"EmitInterstitialDismissedEvent"];
}


#pragma mark - MsRewardedVideoAdPDelegate implementation
- (void)rewardedVideoDidLoadAd:(MsRewardedVideoAdP *)rewardedVideoAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s->ready:%d", _adUnitId, __FUNCTION__, rewardedVideoAd.readyAdCount);
    [self sendUnityEvent:@"EmitRewardedVideoLoadedEvent"];
}

- (void)rewardedVideoAdImpression:(MsRewardedVideoAdP *)rewardedVideoAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __FUNCTION__);
    [self sendUnityEvent:@"EmitRewardedVideoShownEvent"];
}

- (void)rewardedVideoAdDidClick:(MsRewardedVideoAdP *)rewardedVideoAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __FUNCTION__);
    [self sendUnityEvent:@"EmitRewardedVideoClickedEvent"];
}

- (void)rewardedVideoAdDismissed:(MsRewardedVideoAdP *)rewardedVideoAd
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __PRETTY_FUNCTION__);
    [self sendUnityEvent:@"EmitRewardedVideoDismissedEvent"];
}

- (void)rewardedVideoAdShouldReward:(MsRewardedVideoAdP *)rewardedVideoAd reward:(MSRewardedVideoReward *)reward
{
    NSLog(@"TradPlusSDKLog(fm)->%@->%s", _adUnitId, __FUNCTION__);
    [[self class] sendUnityEvent:@"EmitRewardedVideoReceivedRewardEvent"
                        withArgs:@[_adUnitId, reward?reward.currencyType:@"unknown", [NSString stringWithFormat:@"%ld", (long)(reward?[reward.amount integerValue]:0)]]];
}

@end
