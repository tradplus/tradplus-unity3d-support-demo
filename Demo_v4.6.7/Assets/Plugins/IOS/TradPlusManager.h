//
//  TradPlusManager.h
//  TradPlus
//
//  Copyright (c) 2017 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <TradPlusAds/MsBannerView.h>
#import <TradPlusAds/MsInterstitialAd.h>
#import <TradPlusAds/MsRewardedVideoAd.h>
#import <TradPlusAds/MsOfferwallAd.h>

typedef enum
{
    TradPlusAdPositionTopLeft,
    TradPlusAdPositionTopCenter,
    TradPlusAdPositionTopRight,
    TradPlusAdPositionCentered,
    TradPlusAdPositionBottomLeft,
    TradPlusAdPositionBottomCenter,
    TradPlusAdPositionBottomRight
} TradPlusAdPosition;


@interface TradPlusManager : NSObject <MsBannerViewDelegate, MsInterstitialAdDelegate, MsRewardedVideoAdDelegate, MsOfferwallAdDelegate>
{
@private
    NSString* _adUnitId;
    BOOL _autorefresh;
}
@property (nonatomic, strong) MsBannerView *adView;
@property (nonatomic, strong) MsInterstitialAd *interstitial;
@property (nonatomic, strong) MsRewardedVideoAd *rewardedVideo;
@property (nonatomic, strong) MsOfferwallAd *offerwall;
@property (nonatomic) TradPlusAdPosition bannerPosition;


+ (TradPlusManager*)sharedManager;

+ (TradPlusManager*)managerForAdunit:(NSString*)adUnitId;

+ (UIViewController*)unityViewController;

+ (void)sendUnityEvent:(NSString*)eventName withArgs:(NSArray*)args;

- (void)sendUnityEvent:(NSString*)eventName;

- (id)initWithAdUnit:(NSString*)adUnitId;

- (void)createBanner:(TradPlusAdPosition)position;

- (void)destroyBanner;

- (void)showBanner;

- (void)hideBanner:(BOOL)shouldDestroy;

- (void)requestInterstitialAd:(BOOL)autoReload;

- (BOOL)interstitialIsReady;
- (BOOL)interstitialEnterScene;

- (void)showInterstitialAd;

- (void)destroyInterstitialAd;

- (void)requestRewardedVideo:(BOOL)autoReload;

- (BOOL)hasRewardedVideo;
- (BOOL)rewardedVideoEnterScene;

- (void)showRewardedVideo;

- (void)destroyRewardedVideo;

- (void)requestOfferwall;

- (BOOL)hasOfferwall;
- (BOOL)offerwallEnterScene;

- (void)showOfferwall;

- (void)destroyOfferwall;

@end
