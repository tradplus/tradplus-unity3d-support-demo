//
//  FluteManager.h
//  Flute
//
//  Copyright (c) 2017 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
//#import "FluteView.h"
//#import "FluteInterstitial.h"
#import "MsBannerView.h"
#import "MsInterstitialAdP.h"
#import "MsRewardedVideoAdP.h"

typedef enum
{
    FluteAdPositionTopLeft,
    FluteAdPositionTopCenter,
    FluteAdPositionTopRight,
    FluteAdPositionCentered,
    FluteAdPositionBottomLeft,
    FluteAdPositionBottomCenter,
    FluteAdPositionBottomRight
} FluteAdPosition;


@interface FluteManager : NSObject <MsBannerViewDelegate, MsInterstitialAdPDelegate, MsRewardedVideoAdPDelegate>
{
@private
    NSString* _adUnitId;
    BOOL _autorefresh;
}
@property (nonatomic, strong) MsBannerView *adView;
@property (nonatomic, strong) MsInterstitialAdP *interstitial;
@property (nonatomic, strong) MsRewardedVideoAdP *rewardedVideo;
@property (nonatomic) FluteAdPosition bannerPosition;


+ (FluteManager*)sharedManager;

+ (FluteManager*)managerForAdunit:(NSString*)adUnitId;

+ (UIViewController*)unityViewController;

+ (void)sendUnityEvent:(NSString*)eventName withArgs:(NSArray*)args;

- (void)sendUnityEvent:(NSString*)eventName;

- (id)initWithAdUnit:(NSString*)adUnitId;

- (void)createBanner:(FluteAdPosition)position;

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
