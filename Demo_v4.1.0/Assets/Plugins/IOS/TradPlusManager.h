//
//  TradPlusManager.h
//  TradPlus
//
//  Copyright (c) 2017 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <TradPlusAds/MsBannerView.h>
#import <TradPlusAds/MsInterstitialAdP.h>
#import <TradPlusAds/MsRewardedVideoAdP.h>

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


@interface TradPlusManager : NSObject <MsBannerViewDelegate, MsInterstitialAdPDelegate, MsRewardedVideoAdPDelegate>
{
@private
    NSString* _adUnitId;
    BOOL _autorefresh;
}
@property (nonatomic, strong) MsBannerView *adView;
@property (nonatomic, strong) MsInterstitialAdP *interstitial;
@property (nonatomic, strong) MsRewardedVideoAdP *rewardedVideo;
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

- (void)requestInterstitialAd;

- (BOOL)interstitialIsReady;

- (void)showInterstitialAd;

- (void)destroyInterstitialAd;

- (void)requestRewardedVideo;

- (BOOL)hasRewardedVideo;

- (void)showRewardedVideo;

- (void)destroyRewardedVideo;

@end
