//
//  TradPlusManager.m
//  TradPlus
//
//  Copyright (c) 2017 __MyCompanyName__. All rights reserved.
//

//iOS TradPlusSDK v7.8.0+

#import "TradPlusManager.h"

#ifdef __cplusplus
extern "C" {
#endif
    UIViewController* UnityGetGLViewController(void);
    UIWindow* UnityGetMainWindow(void);
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
    return UnityGetGLViewController() ?: UnityGetMainWindow().rootViewController ?: [self getRootViewController];
}

+ (UIViewController*)getRootViewController
{
    if (@available(iOS 13.0, *))
    {
        UIWindow *window = [UIApplication sharedApplication].windows.firstObject;
        return window.rootViewController;
    }
    else
    {
        return [[[UIApplication sharedApplication] keyWindow] rootViewController];
    }
}


+ (void)sendUnityEvent:(NSString*)eventName withArgs:(NSArray*)args
{
    MSLogTrace(@"ttt->%@ args:%@", eventName,args);
    NSData* data = [NSJSONSerialization dataWithJSONObject:args options:0 error:nil];
    UnitySendMessage("TradPlusManager", eventName.UTF8String, [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding].UTF8String);
}


- (void)sendUnityEvent:(NSString*)eventName
{
    [[self class] sendUnityEvent:eventName withArgs:@[_adUnitId]];
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

#pragma mark - TradPlusViewDelegate
- (UIViewController *)viewControllerForPresentingModalView
{
    return [TradPlusManager unityViewController];
}

#pragma mark - Native

- (TradPlusAdNative *)tpNative
{
    if(_tpNative == nil)
    {
        _tpNative = [[TradPlusAdNative alloc] init];
        [_tpNative setAdUnitID:_adUnitId];
        _tpNative.delegate = self;
        if(self.customMap != nil)
        {
            NSData *data = [self.customMap dataUsingEncoding:NSUTF8StringEncoding];
            NSError *error;
            NSDictionary *customDic = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
            if(error == nil && customDic != nil)
            {
                _tpNative.dicCustomValue = customDic;
            }
        }
    }
    return _tpNative;
}

- (void)setupNativeAdViewWithRect:(CGRect)rect
{
    if(self.tpNativeAdView == nil)
    {
        self.tpNativeAdView = [[UIView alloc] initWithFrame:rect];
        self.tpNativeAdView.backgroundColor = [UIColor whiteColor];
    }
    else
    {
        self.tpNativeAdView.frame = rect;
    }
}

- (void)loadTPNative:(CGRect)rect
{
    self.isAutoShow = NO;
    [self setupNativeAdViewWithRect:rect];
    [self.tpNative setTemplateRenderSize:rect.size];
    [self.tpNative loadAd];
}

- (void)autoShowTPNative:(CGRect)rect adSceneId:(NSString *)adSceneId
{
    self.isAutoShow = YES;
    self.adSceneId = adSceneId;
    [self setupNativeAdViewWithRect:rect];
    [self.tpNative setTemplateRenderSize:rect.size];
    [self.tpNative loadAd];
}

- (void)tpNativeEntryAdScenario:(NSString *)sceneId
{
    [self.tpNative entryAdScenario:sceneId];
}

- (void)showTPNative:(NSString *)adSceneId
{
    [[TradPlusManager unityViewController].view addSubview:self.tpNativeAdView];
    [self.tpNative showADWithRenderingViewClass:[self getRenderingViewClass]
                                        subview:self.tpNativeAdView
                                        sceneId:adSceneId];
    self.tpNativeAdView.hidden = NO;
}

//设置指定的渲染模版
- (Class)getRenderingViewClass
{
    Class renderingClass = NSClassFromString(@"AdvancedNativeAdViewSample");
    if(renderingClass == nil)
    {
        NSLog(@"%s No Find Rendering Class",__PRETTY_FUNCTION__);
    }
    return renderingClass;
}

- (void)hideTPNative:(BOOL)needDestroy
{
    self.tpNativeAdView.hidden = YES;
}

#pragma mark - TradPlusADNativeDelegate

- (void)tpNativeAdLoaded:(NSDictionary *)adInfo
{
    if(self.isAutoShow)
    {
        [self showTPNative:self.adSceneId];
    }
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeAdLoadedEvent"
            withInfo:@"0"];
}
- (void)tpNativeAdLoadFailWithError:(NSError *)error
{
    [self sendAdInfo:nil
           eventName:@"EmitOnNativeAdLoadFailedEvent"
            withInfo:[self getMsgWithError:error]];
}
- (void)tpNativeAdImpression:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeAdImpressionEvent"
            withInfo:nil];
}
- (void)tpNativeAdShow:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeAdShowFailedEvent"
            withInfo:[self getMsgWithError:error]];
}
- (void)tpNativeAdClicked:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeAdClickedEvent"
            withInfo:nil];
}
- (void)tpNativeAdClose:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeAdClosedEvent"
            withInfo:nil];
}
- (void)tpNativeAdBidStart:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeBiddingStartEvent"
            withInfo:nil];
}

- (void)tpNativeAdBidEnd:(NSDictionary *)adInfo error:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeBiddingEndEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpNativeAdStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeAdStartLoadEvent"
            withInfo:nil];
}

- (void)tpNativeAdOneLayerStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneNativeLayerStartLoadEvent"
            withInfo:nil];
}
- (void)tpNativeAdOneLayerLoaded:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneNativeLayerLoadedEvent"
            withInfo:nil];
}
- (void)tpNativeAdOneLayerLoad:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneNativeLayerLoadFailedEvent"
            withInfo:[self getMsgWithError:error]];
}
- (void)tpNativeAdAllLoaded:(BOOL)success
{
    [self sendAdInfo:nil
           eventName:@"EmitOnNativeAdAllLoadedEvent"
            withInfo:@[@(success), _adUnitId]];
}

///开始播放 v7.8.0+
- (void)tpNativeAdVideoPlayStart:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeAdVideoPlayStartEvent"
            withInfo:nil];
}

///播放结束 v7.8.0+
- (void)tpNativeAdVideoPlayEnd:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeAdVideoPlayEndEvent"
            withInfo:nil];
}

#pragma mark -Banner

- (TradPlusAdBanner *)tpBanner
{
    if(_tpBanner == nil)
    {
        _tpBanner = [[TradPlusAdBanner alloc] init];
        [_tpBanner setAdUnitID:_adUnitId];
        _tpBanner.delegate = self;
        if(self.customMap != nil)
        {
            NSData *data = [self.customMap dataUsingEncoding:NSUTF8StringEncoding];
            NSError *error;
            NSDictionary *customDic = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
            if(error == nil && customDic != nil)
            {
                _tpBanner.dicCustomValue = customDic;
            }
        }
    }
    return _tpBanner;
}

- (void)autoShowTPBanner:(TradPlusAdPosition)position rect:(CGRect)rect adSceneId:(NSString *)adSceneId;
{
    self.bannerWithRect = NO;
    self.bannerPosition = position;
    self.tpBanner.autoShow = NO;
    self.isAutoShow = YES;
    self.adSceneId = adSceneId;
    self.bannerRect = rect;
    [self.tpBanner loadAdWithSceneId:adSceneId];
}

- (void)autoShowTPBannerWithRect:(CGRect)rect adSceneId:(NSString *)adSceneId
{
    self.bannerWithRect = YES;
    self.tpBanner.frame = rect;
    self.tpBanner.autoShow = NO;
    self.isAutoShow = YES;
    self.adSceneId = adSceneId;
    [self.tpBanner loadAdWithSceneId:adSceneId];
}

- (void)loadTPBanner:(TradPlusAdPosition)position rect:(CGRect)rect
{
    self.bannerWithRect = NO;
    self.bannerPosition = position;
    self.tpBanner.autoShow = NO;
    self.isAutoShow = NO;
    self.bannerRect = rect;
    [self.tpBanner loadAdWithSceneId:nil];
}

- (void)loadTPBannerWithRect:(CGRect)rect
{
    self.bannerWithRect = YES;
    self.tpBanner.frame = rect;
    self.tpBanner.autoShow = NO;
    self.isAutoShow = NO;
    [self.tpBanner loadAdWithSceneId:nil];
}

- (void)tpBannerEntryAdScenario:(NSString *)sceneId
{
    [self.tpBanner entryAdScenario:sceneId];
}

- (void)showTPBanner:(NSString *)adSceneId
{
    [[TradPlusManager unityViewController].view addSubview:self.tpBanner];
    [self.tpBanner showWithSceneId:adSceneId];
    self.tpBanner.hidden = NO;
    if(!self.bannerWithRect)
    {
        if(self.bannerRect.size.width > 0 && self.bannerRect.size.height > 0)
        {
            CGRect rect = self.tpBanner.frame;
            rect.size = self.bannerRect.size;
            self.tpBanner.frame = rect;
        }
        [self adjustView:self.tpBanner];
    }
}
- (void)hideTPBanner
{
    self.tpBanner.hidden = YES;
}
- (void)destroyTPBanner
{
    [self.tpBanner removeFromSuperview];
    self.tpBanner.delegate = nil;
    self.tpBanner = nil;
}

#pragma mark - TradPlusADBannerDelegate

- (void)tpBannerAdLoaded:(NSDictionary *)adInfo
{
    if(self.isAutoShow)
    {
        [self showTPBanner:self.adSceneId];
    }
    [self sendAdInfo:adInfo
           eventName:@"EmitOnAdLoadedEvent"
            withInfo:@"0"];
}

- (void)tpBannerAdLoadFailWithError:(NSError *)error
{
    [self sendAdInfo:nil
           eventName:@"EmitOnAdLoadFailedEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpBannerAdImpression:(NSDictionary *)adInfo
{
    if(!self.bannerWithRect)
    {
        self.tpBanner.hidden = NO;
        UIView *subview = self.tpBanner.subviews.firstObject;
        if(subview != nil)
        {
            self.tpBanner.frame = subview.bounds;
        }
        [self adjustView:self.tpBanner];
    }
    [self sendAdInfo:adInfo
           eventName:@"EmitOnAdImpressionEvent"
            withInfo:nil];
}

- (void)tpBannerAdShow:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnAdShowFailedEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpBannerAdClicked:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnAdClickedEvent"
            withInfo:nil];
}


- (void)tpBannerAdBidStart:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnBiddingStartEvent"
            withInfo:nil];
}

- (void)tpBannerAdBidEnd:(NSDictionary *)adInfo error:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnBiddingEndEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpBannerAdStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnAdStartLoadEvent"
            withInfo:nil];
}

- (void)tpBannerAdOneLayerStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneLayerStartLoadEvent"
            withInfo:nil];
}

- (void)tpBannerAdOneLayerLoaded:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneLayerLoadedEvent"
            withInfo:nil];
}

- (void)tpBannerAdOneLayerLoad:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneLayerLoadFailedEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpBannerAdAllLoaded:(BOOL)success
{
    [self sendAdInfo:nil
           eventName:@"EmitOnAdAllLoadedEvent"
            withInfo:@[@(success), _adUnitId]];
}

- (void)tpBannerAdClose:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnAdClosedEvent"
            withInfo:nil];
}

#pragma mark - Rewarded

- (void)loadTPRewarded:(BOOL)autoReload
{
    if(self.tpRewarded == nil)
    {
        self.tpRewarded = [[TradPlusAdRewarded alloc] init];
        self.tpRewarded.delegate = self;
        self.tpRewarded.playAgainDelegate = self;
        if(self.customMap != nil)
        {
            NSData *data = [self.customMap dataUsingEncoding:NSUTF8StringEncoding];
            NSError *error;
            NSDictionary *customDic = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
            if(error == nil && customDic != nil)
            {
                self.tpRewarded.dicCustomValue = customDic;
            }
        }
        [self.tpRewarded setAdUnitID:_adUnitId isAutoLoad:autoReload];
        if(!autoReload)
        {
            [self.tpRewarded loadAd];
        }
    }
    else
    {
        [self.tpRewarded loadAd];
    }
}

- (void)destroyTPRewarded
{
    if(self.tpRewarded)
    {
        self.tpRewarded.delegate = nil;
        self.tpRewarded.playAgainDelegate = nil;
        self.tpRewarded = nil;
    }
}

- (void)tpRewardedEntryAdScenario:(NSString *)sceneId
{
    if(self.tpRewarded)
    {
        [self.tpRewarded entryAdScenario:sceneId];
    }
}

- (BOOL)tpRewardedIsReady
{
    if(self.tpRewarded)
    {
        return [self.tpRewarded isAdReady];
    }
    else
    {
        return NO;
    }
}
- (void)showTPRewarded:(NSString *)adSceneId
{
    if(self.tpRewarded)
    {
        [self.tpRewarded showAdFromRootViewController:[TradPlusManager unityViewController] sceneId:adSceneId];
    }
}
- (void)setTPRewardedServerSideWithUserID:(NSString *)userID customData:(NSString *)customData
{
    if(self.tpRewarded)
    {
        [self.tpRewarded setServerSideVerificationOptionsWithUserID:userID customData:customData];
    }
}

#pragma mark - TradPlusADRewardedDelegate

- (void)tpRewardedAdLoaded:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoAdLoadedEvent"
            withInfo:nil];
}

- (void)tpRewardedAdLoadFailWithError:(NSError *)error
{
    [self sendAdInfo:nil
           eventName:@"EmitOnRewardedVideoAdFailedEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpRewardedAdImpression:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoAdImpressionEvent"
            withInfo:nil];
}

- (void)tpRewardedAdShow:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoAdVideoErrorEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpRewardedAdClicked:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoAdClickedEvent"
            withInfo:nil];
}

- (void)tpRewardedAdDismissed:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoAdClosedEvent"
            withInfo:nil];
}

- (void)tpRewardedAdReward:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoAdRewardEvent"
            withInfo:nil];
}

- (void)tpRewardedAdBidStart:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoBiddingStartEvent"
            withInfo:nil];
}

- (void)tpRewardedAdBidEnd:(NSDictionary *)adInfo error:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoBiddingEndEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpRewardedAdStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoAdStartLoadEvent"
            withInfo:nil];
}

- (void)tpRewardedAdOneLayerStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneRewardedVideoLayerStartLoadEvent"
            withInfo:nil];
}
- (void)tpRewardedAdOneLayerLoaded:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneRewardedVideoLayerLoadedEvent"
            withInfo:nil];
}
- (void)tpRewardedAdOneLayerLoad:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneRewardedVideoLayerLoadFailedEvent"
            withInfo:[self getMsgWithError:error]];
}
- (void)tpRewardedAdAllLoaded:(BOOL)success
{
    [self sendAdInfo:nil
           eventName:@"EmitOnRewardedVideoAdAllLoadedEvent"
            withInfo:@[@(success), _adUnitId]];
}

- (void)tpRewardedAdPlayStart:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoPlayStartEvent"
            withInfo:nil];
}
- (void)tpRewardedAdPlayEnd:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoPlayEndEvent"
            withInfo:nil];
}

#pragma mark - TradPlusADRewardedPlayAgainDelegate


///AD展现
- (void)tpRewardedAdPlayAgainImpression:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoAgainImpressionEvent"
            withInfo:nil];
}

///AD被点击
- (void)tpRewardedAdPlayAgainClicked:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoAgainVideoClickedEvent"
            withInfo:nil];
}

///完成奖励
- (void)tpRewardedAdPlayAgainReward:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoPlayAgainRewardEvent"
            withInfo:nil];
}

///开始播放
- (void)tpRewardedAdPlayAgainPlayStart:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoAgainVideoStartEvent"
            withInfo:nil];
}

///播放结束
- (void)tpRewardedAdPlayAgainPlayEnd:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnRewardedVideoAgainVideoEndEvent"
            withInfo:nil];
}

///AD展现失败
- (void)tpRewardedAdPlayAgainShow:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    
}

#pragma mark - Interstitial
- (void)loadTPInterstitial:(BOOL)autoReload
{
    if(self.tpInterstitial == nil)
    {
        self.tpInterstitial = [[TradPlusAdInterstitial alloc] init];
        self.tpInterstitial.delegate = self;
        if(self.customMap != nil)
        {
            NSData *data = [self.customMap dataUsingEncoding:NSUTF8StringEncoding];
            NSError *error;
            NSDictionary *customDic = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
            if(error == nil && customDic != nil)
            {
                self.tpInterstitial.dicCustomValue = customDic;
            }
        }
        [self.tpInterstitial setAdUnitID:_adUnitId isAutoLoad:autoReload];
        if(!autoReload)
        {
            [self.tpInterstitial loadAd];
        }
    }
    else
    {
        [self.tpInterstitial loadAd];
    }
}

- (void)destroyTPInterstitial
{
    if(self.tpInterstitial)
    {
        self.tpInterstitial.delegate = nil;
        self.tpInterstitial = nil;
    }
}
- (void)tpInterstitialEntryAdScenario:(NSString *)sceneId
{
    if(self.tpInterstitial)
    {
        [self.tpInterstitial entryAdScenario:sceneId];
    }
}
- (BOOL)tpInterstitialIsReady
{
    if(self.tpInterstitial)
    {
        return self.tpInterstitial.isAdReady;
    }
    else
    {
        return NO;
    }
}

- (void)showTPInterstitial:(NSString *)adSceneId
{
    if(self.tpInterstitial)
    {
        [self.tpInterstitial showAdFromRootViewController:[TradPlusManager unityViewController] sceneId:adSceneId];
    }
}

#pragma mark - TradPlusADInterstitialDelegate

- (void)tpInterstitialAdLoaded:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnInterstitialAdLoadedEvent"
            withInfo:nil];
}

- (void)tpInterstitialAdLoadFailWithError:(NSError *)error
{
    [self sendAdInfo:nil
           eventName:@"EmitOnInterstitialAdFailedEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpInterstitialAdImpression:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnInterstitialAdImpressionEvent"
            withInfo:nil];
}

- (void)tpInterstitialAdShow:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnInterstitialAdVideoErrorEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpInterstitialAdClicked:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnInterstitialAdClickedEvent"
            withInfo:nil];
}

- (void)tpInterstitialAdDismissed:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnInterstitialAdClosedEvent"
            withInfo:nil];
}

- (void)tpInterstitialAdBidStart:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnInterstitialBiddingStartEvent"
            withInfo:nil];
}

- (void)tpInterstitialAdBidEnd:(NSDictionary *)adInfo error:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnInterstitialBiddingEndEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpInterstitialAdStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnInterstitialAdStartLoadEvent"
            withInfo:nil];
}

- (void)tpInterstitialAdOneLayerStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneInterstitialLayerStartLoadEvent"
            withInfo:nil];
}

- (void)tpInterstitialAdOneLayerLoaded:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneInterstitialLayerLoadedEvent"
            withInfo:nil];
}
- (void)tpInterstitialAdOneLayerLoad:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneInterstitialLayerLoadFailedEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpInterstitialAdAllLoaded:(BOOL)success
{
    [self sendAdInfo:nil
           eventName:@"EmitOnInterstitialAdAllLoadedEvent"
            withInfo:@[@(success), _adUnitId]];
}

///开始播放
- (void)tpInterstitialAdPlayStart:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnInterstitialVideoPlayStartEvent"
            withInfo:nil];
}

///播放结束
- (void)tpInterstitialAdPlayEnd:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnInterstitialVideoPlayEndEvent"
            withInfo:nil];
}

#pragma mark - NativeBanner

- (TradPlusNativeBanner *)nativeBanner
{
    if(_nativeBanner == nil)
    {
        _nativeBanner = [[TradPlusNativeBanner alloc] init];
//        CGRect rect = _nativeBanner.bounds;
//        rect.origin = CGPointMake(CGRectGetWidth([UIScreen mainScreen].bounds), 0);
//        _nativeBanner.frame = rect;
        [_nativeBanner setAdUnitID:_adUnitId];
        _nativeBanner.delegate = self;
        if(self.customMap != nil)
        {
            NSData *data = [self.customMap dataUsingEncoding:NSUTF8StringEncoding];
            NSError *error;
            NSDictionary *customDic = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
            if(error == nil && customDic != nil)
            {
                _nativeBanner.dicCustomValue = customDic;
            }
        }
    }
    return _nativeBanner;
}

- (void)autoShowNativeBanner:(TradPlusAdPosition)position adSceneId:(NSString *)adSceneId className:(NSString *)className
{
    self.bannerWithRect = NO;
    self.bannerPosition = position;
    self.nativeBanner.autoShow = NO;
    self.isAutoShow = YES;
    self.nativeBannerRenderingClass = className;
    self.adSceneId = adSceneId;
    [self.nativeBanner loadAdWithSceneId:adSceneId];
}

- (void)loadNativeBanner:(TradPlusAdPosition)position adSceneId:(NSString *)adSceneId className:(NSString *)className
{
    self.bannerWithRect = NO;
    self.bannerPosition = position;
    self.nativeBanner.autoShow = NO;
    self.isAutoShow = NO;
    self.nativeBannerRenderingClass = className;
    [self.nativeBanner loadAdWithSceneId:adSceneId];
}

- (void)autoShowNativeBannerWithRect:(CGRect)rect adSceneId:(NSString *)adSceneId className:(NSString *)className
{
    self.bannerWithRect = YES;
    self.nativeBanner.frame = rect;
    self.nativeBanner.autoShow = NO;
    self.isAutoShow = YES;
    self.nativeBannerRenderingClass = className;
    self.adSceneId = adSceneId;
    [self.nativeBanner loadAdWithSceneId:adSceneId];
}

- (void)loadNativeBannerWithRect:(CGRect)rect adSceneId:(NSString *)adSceneId className:(NSString *)className
{
    self.bannerWithRect = YES;
    self.nativeBanner.frame = rect;
    self.nativeBanner.autoShow = NO;
    self.isAutoShow = NO;
    self.nativeBannerRenderingClass = className;
    [self.nativeBanner loadAdWithSceneId:adSceneId];
}

- (void)showNativeBanner:(NSString *)adSceneId
{
    if(self.nativeBannerRenderingClass != nil && self.nativeBannerRenderingClass.length > 0)
    {
        Class renderingClass = NSClassFromString(self.nativeBannerRenderingClass);
        [self.nativeBanner showWithRenderingViewClass:renderingClass sceneId:adSceneId];
    }
    else
    {
        [self.nativeBanner showWithSceneId:adSceneId];
    }
    self.nativeBanner.hidden = NO;
    [[TradPlusManager unityViewController].view addSubview:self.nativeBanner];
    if(!self.bannerWithRect)
    {
        [self adjustView:self.nativeBanner];
    }
}

- (void)nativeBannerEntryAdScenario:(NSString *)adSceneId
{
    [self.nativeBanner entryAdScenario:adSceneId];
}

- (void)hideNativeBanner:(BOOL)hide
{
    self.nativeBanner.hidden = hide;
}

- (void)destroyNativeBanner
{
    if(self.nativeBanner != nil)
    {
        [self.nativeBanner removeFromSuperview];
        self.nativeBanner.delegate = nil;
        self.nativeBanner = nil;
    }
}

#pragma mark - TradPlusADNativeBannerDelegate

- (void)tpNativeBannerAdDidLoaded:(NSDictionary *)adInfo
{
    if(self.isAutoShow)
    {
        [self showNativeBanner:self.adSceneId];
    }
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeBannerAdLoadedEvent"
            withInfo:@"0"];
}

- (void)tpNativeBannerAdLoadFailWithError:(NSError *)error
{
    [self sendAdInfo:nil
           eventName:@"EmitOnNativeBannerAdLoadFailedEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpNativeBannerAdImpression:(NSDictionary *)adInfo
{
    if(!self.bannerWithRect)
    {
        UIView *subview = self.nativeBanner.subviews.firstObject;
        if(subview != nil)
        {
            self.nativeBanner.frame = subview.bounds;
        }
        [self adjustView:self.nativeBanner];
    }
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeBannerAdImpressionEvent"
            withInfo:nil];
}

- (void)tpNativeBannerAdShow:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeBannerAdShowFailedEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpNativeBannerAdClicked:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeBannerAdClickedEvent"
            withInfo:nil];
}

- (void)tpNativeBannerAdBidStart:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeBannerBiddingStartEvent"
            withInfo:nil];
}

- (void)tpNativeBannerAdBidEnd:(NSDictionary *)adInfo error:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnNativeBannerBiddingEndEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpNativeBannerAdStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOnInterstitialAdStartLoadEvent"
            withInfo:nil];
}

- (void)tpNativeBannerAdOneLayerStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneInterstitialLayerStartLoadEvent"
            withInfo:nil];
}
- (void)tpNativeBannerAdOneLayerLoaded:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneNativeBannerLayerLoadedEvent"
            withInfo:nil];
}

- (void)tpNativeBannerAdOneLayerLoad:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
           eventName:@"EmitOneNativeBannerLayerLoadFailedEvent"
            withInfo:[self getMsgWithError:error]];
}

- (void)tpNativeBannerAdAllLoaded:(BOOL)success
{
    [self sendAdInfo:nil
           eventName:@"EmitOnNativeBannerAdAllLoadedEvent"
            withInfo:@[@(success), _adUnitId]];
}

#pragma mark -

- (void)adjustView:(UIView *)view
{
    CGFloat top = 0;
    CGFloat bottom = 0;
    if (@available(iOS 11.0, *))
    {
        UIEdgeInsets safeAreaInsets = [TradPlusManager unityViewController].view.safeAreaInsets;
        top = safeAreaInsets.top;
        bottom = safeAreaInsets.bottom;
    }
    switch (self.bannerPosition)
    {
        case TradPlusAdPositionTopLeft:
        {
            CGRect rect = view.bounds;
            rect.origin.y = top;
            view.frame = rect;
            break;
        }
        case TradPlusAdPositionTopCenter:
        {
            CGPoint center = CGPointZero;
            center.x = CGRectGetWidth(view.superview.bounds)/2;
            center.y = CGRectGetHeight(view.bounds)/2 + top;
            view.center = center;
            break;
        }
        case TradPlusAdPositionTopRight:
        {
            CGRect rect = view.bounds;
            rect.origin.x = CGRectGetWidth(view.superview.bounds) - CGRectGetWidth(view.bounds);
            rect.origin.y = top;
            view.frame = rect;
            break;
        }
        case TradPlusAdPositionCentered:
        {
            CGPoint center = CGPointZero;
            center.x = CGRectGetWidth(view.superview.bounds)/2;
            center.y = CGRectGetHeight(view.superview.bounds)/2;
            view.center = center;
            break;
        }
        case TradPlusAdPositionBottomLeft:
        {
            CGRect rect = view.bounds;
            rect.origin.y = CGRectGetHeight(view.superview.bounds) - CGRectGetHeight(view.bounds) - bottom;
            view.frame = rect;
            break;
        }
        case TradPlusAdPositionBottomCenter:
        {
            CGPoint center = CGPointZero;
            center.x = CGRectGetWidth(view.superview.bounds)/2;
            center.y = CGRectGetHeight(view.superview.bounds) - CGRectGetHeight(view.bounds)/2 - bottom;
            view.center = center;
            break;
        }
        case TradPlusAdPositionBottomRight:
        {
            CGRect rect = view.bounds;
            rect.origin.x = CGRectGetWidth(view.superview.bounds) - CGRectGetWidth(view.bounds);
            rect.origin.y = CGRectGetHeight(view.superview.bounds) - CGRectGetHeight(view.bounds) - bottom;
            view.frame = rect;
            break;
        }
    }
}

- (NSString *)getMsgWithError:(NSError *)error
{
    if(error != nil)
    {
        return [NSString stringWithFormat:@"errCode: %ld, errMsg: %@", (long)error.code, error.localizedDescription];
    }
    else
    {
        return @"";
    }
}

- (void)sendAdInfo:(NSDictionary *)adInfo eventName:(NSString *)eventName withInfo:(id)info
{
    NSMutableArray *array = [[NSMutableArray alloc] init];
    if(info != nil && [info isKindOfClass:[NSArray class]])
    {
        [array addObjectsFromArray:info];
    }
    else
    {
        if(adInfo != nil)
        {
            NSData *data = [NSJSONSerialization dataWithJSONObject:adInfo options:0 error:nil];
            NSString *adInfoStr = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
            [array addObject:adInfoStr];
        }
        else
        {
            [array addObject:_adUnitId];
        }
        if (info != nil)
        {
            [array addObject:info];
        }
    }
    [TradPlusManager sendUnityEvent:eventName withArgs:array];
}

#pragma mark -OfferWall

- (void)loadTPOfferWall:(BOOL)autoReload
{
    if(self.offerwall == nil)
    {
        self.offerwall = [[TradPlusAdOfferwall alloc] init];
        self.offerwall.delegate = self;
        if(self.customMap != nil)
        {
            NSData *data = [self.customMap dataUsingEncoding:NSUTF8StringEncoding];
            NSError *error;
            NSDictionary *customDic = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
            if(error == nil && customDic != nil)
            {
                self.offerwall.dicCustomValue = customDic;
            }
        }
        [self.offerwall setAdUnitID:_adUnitId isAutoLoad:autoReload];
        if(!autoReload)
        {
            [self.offerwall loadAd];
        }
    }
    else
    {
        [self.offerwall loadAd];
    }
}

- (void)showTPOfferWall:(NSString *)adSceneId
{
    if(self.offerwall)
    {
        [self.offerwall showAdFromRootViewController:[TradPlusManager unityViewController] sceneId:adSceneId];
    }
}

- (void)tpOfferWallEntryAdScenario:(NSString *)adSceneId
{
    if(self.offerwall)
    {
        [self.offerwall entryAdScenario:adSceneId];
    }
}

- (BOOL)tpOfferWallIsReady
{
    if(self.offerwall)
    {
        return self.offerwall.isAdReady;
    }
    return NO;
}

- (void)tpOfferWallGetCurrencyBalance
{
    if(self.offerwall)
    {
        [self.offerwall getCurrencyBalance];
    }
}

- (void)tpOfferWallSpendCurrency:(int)amount
{
    if(self.offerwall)
    {
        [self.offerwall spendCurrency:amount];
    }
}

- (void)tpOfferWallAwardCurrency:(int)amount
{
    if(self.offerwall)
    {
        [self.offerwall awardCurrency:amount];
    }
}

- (void)setOfferWallUserId:(NSString *)userId;
{
    if(self.offerwall)
    {
        [self.offerwall setUserId:userId];
    }
}

- (void)tpOfferWallDestroy
{
    if(self.offerwall)
    {
        self.offerwall.delegate = nil;
        self.offerwall = nil;
    }
}

///AD加载完成 首个广告源加载成功时回调 一次加载流程只会回调一次
- (void)tpOfferwallAdLoaded:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
               eventName:@"EmitOnOfferWallAdLoadedEvent"
                withInfo:nil];
}

///AD加载失败
///tpOfferwallAdOneLayerLoaded:didFailWithError：返回三方源的错误信息
- (void)tpOfferwallAdLoadFailWithError:(NSError *)error
{
    [self sendAdInfo:nil
               eventName:@"EmitOnOfferWallAdFailedEvent"
                withInfo:[self getMsgWithError:error]];
}

///AD展现
- (void)tpOfferwallAdImpression:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
               eventName:@"EmitOnOfferWallAdImpressionEvent"
                withInfo:nil];
}

///AD展现失败
- (void)tpOfferwallAdShow:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
               eventName:@"EmitOnOfferWallAdShowFailedEvent"
                withInfo:[self getMsgWithError:error]];
}

///AD被点击
- (void)tpOfferwallAdClicked:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
               eventName:@"EmitOnOfferWallAdClickedEvent"
                withInfo:nil];
}

///AD关闭
- (void)tpOfferwallAdDismissed:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
               eventName:@"EmitOnOfferWallAdClosedEvent"
                withInfo:nil];
}

///开始加载流程
- (void)tpOfferwallAdStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
               eventName:@"EmitOnOfferWallAdStartLoadEvent"
                withInfo:nil];
}

///当每个广告源开始加载时会都会回调一次。
- (void)tpOfferwallAdOneLayerStartLoad:(NSDictionary *)adInfo
{
    [self sendAdInfo:adInfo
               eventName:@"EmitOneOfferWallLayerStartLoadEvent"
                withInfo:nil];
}

///当每个广告源加载成功后会都会回调一次。
- (void)tpOfferwallAdOneLayerLoaded:(NSDictionary *)adInfo;
{
    [self sendAdInfo:adInfo
               eventName:@"EmitOneOfferWallLayerLoadedEvent"
                withInfo:nil];
}

///当每个广告源加载失败后会都会回调一次，返回三方源的错误信息
- (void)tpOfferwallAdOneLayerLoad:(NSDictionary *)adInfo didFailWithError:(NSError *)error
{
    [self sendAdInfo:adInfo
               eventName:@"EmitOneOfferWallLayerLoadFailedEvent"
                withInfo:[self getMsgWithError:error]];
}

///加载流程全部结束
- (void)tpOfferwallAdAllLoaded:(BOOL)success
{
    [self sendAdInfo:nil
           eventName:@"EmitOnOfferWallAdAllLoadedEvent"
            withInfo:@[@(success), _adUnitId]];
}

///userID 设置结束 error = nil 成功
- (void)tpOfferwallSetUserIdFinish:(NSError *)error
{
    if(error == nil)
    {
        [self sendAdInfo:nil
               eventName:@"EmitOnSetUserIdSuccessEvent"
                withInfo:@[_adUnitId]];
    }
    else
    {
        [self sendAdInfo:nil
               eventName:@"EmitOnSetUserIdFailedEvent"
                withInfo:@[[self getMsgWithError:error]]];
    }
}

///用户当前积分墙积分数量
- (void)tpOfferwallGetCurrencyBalance:(NSDictionary *)response error:(NSError *)error
{
    if(error == nil)
    {
        NSInteger amount = 0;
        if(response[@"amount"])
        {
            amount = [response[@"amount"] integerValue];
        }
        NSMutableDictionary *dic = [[NSMutableDictionary alloc] initWithDictionary:response];
        [dic removeObjectForKey:@"amount"];
        NSData *data = [NSJSONSerialization dataWithJSONObject:dic options:0 error:nil];
        NSString *msg = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
        if(msg == nil)
        {
            msg = @"";
        }
        [self sendAdInfo:nil
               eventName:@"EmitOnCurrencyBalanceSuccessEvent"
                withInfo:@[@(amount), msg]];
    }
    else
    {
        NSData *data = [NSJSONSerialization dataWithJSONObject:response options:0 error:nil];
        NSString *msg = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
        if(msg == nil)
        {
            msg = @"";
        }
        [self sendAdInfo:nil
               eventName:@"EmitOnCurrencyBalanceFailedEvent"
                withInfo:@[msg]];
    }
}

//扣除用户积分墙积分回调
- (void)tpOfferwallSpendCurrency:(NSDictionary *)response error:(NSError *)error
{
    if(error == nil)
    {
        NSInteger amount = 0;
        if(response[@"amount"])
        {
            amount = [response[@"amount"] integerValue];
        }
        NSMutableDictionary *dic = [[NSMutableDictionary alloc] initWithDictionary:response];
        [dic removeObjectForKey:@"amount"];
        NSData *data = [NSJSONSerialization dataWithJSONObject:dic options:0 error:nil];
        NSString *msg = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
        if(msg == nil)
        {
            msg = @"";
        }
        [self sendAdInfo:nil
               eventName:@"EmitOnSpendCurrencySuccessEvent"
                withInfo:@[@(amount), msg]];
    }
    else
    {
        NSData *data = [NSJSONSerialization dataWithJSONObject:response options:0 error:nil];
        NSString *msg = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
        if(msg == nil)
        {
            msg = @"";
        }
        [self sendAdInfo:nil
               eventName:@"EmitOnSpendCurrencyFailedEvent"
                withInfo:@[msg]];
    }
}

//添加用户积分墙积分回调
- (void)tpOfferwallAwardCurrency:(NSDictionary *)response error:(NSError *)error
{
    if(error == nil)
    {
        NSInteger amount = 0;
        if(response[@"amount"])
        {
            amount = [response[@"amount"] integerValue];
        }
        NSMutableDictionary *dic = [[NSMutableDictionary alloc] initWithDictionary:response];
        [dic removeObjectForKey:@"amount"];
        NSData *data = [NSJSONSerialization dataWithJSONObject:dic options:0 error:nil];
        NSString *msg = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
        if(msg == nil)
        {
            msg = @"";
        }
        [self sendAdInfo:nil
               eventName:@"EmitOnAwardCurrencySuccessEvent"
                withInfo:@[@(amount), msg]];
    }
    else
    {
        NSData *data = [NSJSONSerialization dataWithJSONObject:response options:0 error:nil];
        NSString *msg = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
        if(msg == nil)
        {
            msg = @"";
        }
        [self sendAdInfo:nil
               eventName:@"EmitOnAwardCurrencyFailedEvent"
                withInfo:@[msg]];
    }
}
@end
