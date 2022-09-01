//
//  TradPlusManager.h
//  TradPlus
//
//  Copyright (c) 2017 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <TradPlusAds/TradPlusAds.h>

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


@interface TradPlusManager : NSObject <TradPlusADNativeDelegate,TradPlusADBannerDelegate,TradPlusADRewardedDelegate,TradPlusADInterstitialDelegate,TradPlusADNativeBannerDelegate,TradPlusADRewardedPlayAgainDelegate,TradPlusADOfferwallDelegate>
{
@private
    NSString* _adUnitId;
    BOOL _autorefresh;
}
@property (nonatomic) TradPlusAdPosition bannerPosition;
@property (nonatomic,assign) int bannerWidth;
@property (nonatomic,assign) int bannerHeight;

@property (nonatomic, strong) NSString *customMap;

//6.0+
@property (nonatomic,strong)TradPlusAdBanner *tpBanner;
@property (nonatomic,strong)TradPlusAdNative *tpNative;
@property (nonatomic,strong)TradPlusAdRewarded *tpRewarded;
@property (nonatomic,strong)TradPlusAdInterstitial *tpInterstitial;
@property (nonatomic,strong)TradPlusNativeBanner *nativeBanner;
@property (nonatomic,strong)TradPlusAdOfferwall *offerwall;
@property (nonatomic,strong)UIView *tpNativeAdView;
@property (nonatomic,assign)BOOL isAutoShow;
@property (nonatomic,assign)BOOL bannerWithRect;
@property (nonatomic,assign)CGRect bannerRect;
@property (nonatomic,copy)NSString *adSceneId;
@property (nonatomic,copy)NSString *nativeBannerRenderingClass;

+ (TradPlusManager*)sharedManager;

+ (TradPlusManager*)managerForAdunit:(NSString*)adUnitId;

+ (UIViewController*)unityViewController;

+ (void)sendUnityEvent:(NSString*)eventName withArgs:(NSArray*)args;

- (void)sendUnityEvent:(NSString*)eventName;

- (id)initWithAdUnit:(NSString*)adUnitId;

- (void)loadTPNative:(CGRect)rect;
- (void)autoShowTPNative:(CGRect)rect adSceneId:(NSString *)adSceneId;
- (void)showTPNative:(NSString *)adSceneId;
- (void)hideTPNative:(BOOL)needDestroy;
- (void)tpNativeEntryAdScenario:(NSString *)sceneId;

- (void)autoShowTPBanner:(TradPlusAdPosition)position rect:(CGRect)rect adSceneId:(NSString *)adSceneId;
- (void)autoShowTPBannerWithRect:(CGRect)rect adSceneId:(NSString *)adSceneId;
- (void)loadTPBanner:(TradPlusAdPosition)position rect:(CGRect)rect;
- (void)loadTPBannerWithRect:(CGRect)rect;
- (void)tpBannerEntryAdScenario:(NSString *)sceneId;
- (void)showTPBanner:(NSString *)adSceneId;
- (void)hideTPBanner;
- (void)destroyTPBanner;

- (void)loadTPRewarded:(BOOL)autoReload;
- (void)destroyTPRewarded;
- (void)tpRewardedEntryAdScenario:(NSString *)sceneId;
- (BOOL)tpRewardedIsReady;
- (void)showTPRewarded:(NSString *)adSceneId;
- (void)setTPRewardedServerSideWithUserID:(NSString *)userID customData:(NSString *)customData;

- (void)loadTPInterstitial:(BOOL)autoReload;
- (void)destroyTPInterstitial;
- (void)tpInterstitialEntryAdScenario:(NSString *)sceneId;
- (BOOL)tpInterstitialIsReady;
- (void)showTPInterstitial:(NSString *)adSceneId;

- (void)autoShowNativeBanner:(TradPlusAdPosition)position adSceneId:(NSString *)adSceneId className:(NSString *)className;
- (void)loadNativeBanner:(TradPlusAdPosition)position adSceneId:(NSString *)adSceneId className:(NSString *)className;
- (void)autoShowNativeBannerWithRect:(CGRect)rect adSceneId:(NSString *)adSceneId className:(NSString *)className;
- (void)loadNativeBannerWithRect:(CGRect)rect adSceneId:(NSString *)adSceneId className:(NSString *)className;
- (void)showNativeBanner:(NSString *)adSceneId;
- (void)nativeBannerEntryAdScenario:(NSString *)adSceneId;
- (void)hideNativeBanner:(BOOL)hide;
- (void)destroyNativeBanner;


- (void)loadTPOfferWall:(BOOL)autoReload;
- (void)showTPOfferWall:(NSString *)adSceneId;
- (void)tpOfferWallEntryAdScenario:(NSString *)adSceneId;
- (BOOL)tpOfferWallIsReady;
- (void)tpOfferWallGetCurrencyBalance;
- (void)tpOfferWallSpendCurrency:(int)amount;
- (void)tpOfferWallAwardCurrency:(int)amount;
- (void)tpOfferWallDestroy;
- (void)setOfferWallUserId:(NSString *)userId;
@end
