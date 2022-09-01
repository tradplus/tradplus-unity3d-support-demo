//
//  TradPlusBinding.m
//  TradPlus
//
//  Copyright (c) 2017 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "TradPlusManager.h"
#import <TradPlusAds/MSConsentManager.h>
#import <TradPlusAds/TradPlus.h>

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Helpers

// Converts C style string to NSString
#define GetStringParam(_x_) ((_x_) != NULL ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""])
#define GetNullableStringParam(_x_) ((_x_) != NULL ? [NSString stringWithUTF8String:_x_] : nil)

// Converts an NSString into a const char* ready to be sent to Unity
static char* cStringCopy(NSString* input)
{
    const char* string = [input UTF8String];
    return string ? strdup(string) : NULL;
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - SDK Setup
void _tradplusInitCustomMap(const char* customMap)
{
    NSData *data = [GetStringParam(customMap) dataUsingEncoding:NSUTF8StringEncoding];
    [TradPlus sharedInstance].dicCustomValue = [NSJSONSerialization JSONObjectWithData:data options:0 error:nil];
}

void _tradplusInitPlacementCustomMap(const char* placementId, const char* customMap)
{
    [TradPlusManager managerForAdunit:GetStringParam(placementId)].customMap = GetStringParam(customMap);
}

static bool is_sdk_initialized = false;
void _tradplusInitializeSdk(const char* adUnitIdString)
{
    if (!is_sdk_initialized)
    {
        NSString* appId = GetStringParam(adUnitIdString);
        [TradPlus initSDK:appId completionBlock:^(NSError * _Nonnull error) {
            is_sdk_initialized = true;
            [TradPlusManager sendUnityEvent:@"EmitSdkInitializedEvent" withArgs:@[appId]];
        }];
    }
}

bool _tradplusIsSdkInitialized()
{
    return is_sdk_initialized;
}

const char* _tradplusGetSDKVersion()
{
    return cStringCopy([TradPlus getVersion]);
}

void _tradplusLoadSplash(const char* adUnitId)
{
    
}

void _tradplusRewardedVideoClearCache(const char* adUnitId)
{
    [TradPlus clearCacheWithPlacementId:GetStringParam(adUnitId)];
}

#pragma mark - GDPR
void _tradplusShowUploadDataNotifyDialog(const char* url)
{
    [[MSConsentManager sharedManager] showConsentDialogFromViewController:[TradPlusManager unityViewController] didShow:nil didDismiss:nil];
}

void _tradplusSetGDPRUploadDataLevel(int level)
{
    [[MSConsentManager sharedManager] setCanCollectPersonalInfo:(level == 0)];
}

int _tradplusGetGDPRUploadDataLevel()
{
    return (int)[MSConsentManager sharedManager].currentStatus;
}

bool _tradplusIsEUTraffic()
{
    return [[MSConsentManager sharedManager] isGDPRApplicable] == MSBoolYes;
}

void _tradplusExpiredAdCheck()
{
    [TradPlus expiredAdCheck];
}

void _tradplusExpiredAdChecking(bool isOpen)
{
    [TradPlus sharedInstance].isExpiredAdChecking = isOpen;
}

void _tradplusSetGDPRChild(bool isGDPRChild)
{
    //TradPlus set
}

void _tradplusSetAllowPostUseTime(bool isOpen)
{
    [TradPlus setAppAllowUploadUseTime:isOpen];
}

void _tradplusSetCnServer(bool cnServer)
{
    [TradPlus setCnServer:cnServer];
}

void _tradplusSetAllowMessagePush(bool messagePush)
{
    [TradPlus setAllowMessagePush:messagePush];
}

void _tradplusSetOpenPersonalizedAd(bool isOpen)
{
    [TradPlus setOpenPersonalizedAd:isOpen];
}

bool _tradplusIsOpenPersonalizedAd()
{
    return [TradPlus sharedInstance].isOpenPersonalizedAd;
}


void _tradplusSetCCPADataCollection(bool isCCPA)
{
    [TradPlus setCCPADoNotSell:isCCPA];
}

void _tradplusSetCOPPAChild(bool isChild)
{
    [TradPlus setCOPPAIsAgeRestrictedUser:isChild];
}

void _tradplusSetAuthUID(bool needAuthUID)
{
}

#pragma mark - Native

void _tradplusShowNative60(const char* adUnitId, int x, int y, int width, int height,const char* adSceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    CGRect rect = CGRectMake(x, y, width, height);
    [manager autoShowTPNative:rect adSceneId:GetStringParam(adSceneId)];
}


void _tradplusLoadNative(const char* adUnitId,int x, int y, int width, int height)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    CGRect rect = CGRectMake(x, y, width, height);
    [manager loadTPNative:rect];
}

void _tradplusShowNativeNotAuto(const char* adUnitId, const char* adSceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager showTPNative:GetStringParam(adSceneId)];
}

void _tradplusHideNative60(const char* adUnitId, bool needDestroy)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager hideTPNative:needDestroy];
}

void _tradplusNativeEntryAdScenario60(const char* adUnitId, const char* adSceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager tpNativeEntryAdScenario:GetStringParam(adSceneId)];
}

#pragma mark - Banner

void _tradplusCreateBanner60(int bannerPosition, const char* adUnitId, int width, int height, const char* adSceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    CGRect rect = CGRectMake(0, 0, width, height);
    [manager autoShowTPBanner:bannerPosition rect:rect adSceneId:GetStringParam(adSceneId)];
}


void _tradplusAutoShowBannerWithRect(const char* adUnitId, int x, int y, int width, int height, const char* adSceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    CGRect rect = CGRectMake(x, y, width, height);
    [manager autoShowTPBannerWithRect:rect adSceneId:GetStringParam(adSceneId)];
}

void _tradplusLoadBanner(int bannerPosition, const char* adUnitId,int width, int height)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    CGRect rect = CGRectMake(0, 0, width, height);
    [manager loadTPBanner:bannerPosition rect:rect];
}

void _tradplusLoadBannerWithRect(const char* adUnitId, int x, int y, int width, int height)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    CGRect rect = CGRectMake(x, y, width, height);
    [manager loadTPBannerWithRect:rect];
}


void _tradplusShowBanner60(const char* adUnitId, bool shouldShow,const char* adSceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    if(shouldShow)
    {
        [manager showTPBanner:GetStringParam(adSceneId)];
    }
    else
    {
        [manager hideTPBanner];
    }
}

void _tradplusDestroyBanner60(const char* adUnitId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager destroyTPBanner];
}


void _tradplusBannerEntryAdScenario60(const char* adUnitId,const char* sceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager tpBannerEntryAdScenario:GetStringParam(sceneId)];
}

#pragma mark - RewardedVideo

void _tradplusRequestRewardedVideo60(const char* adUnitId, bool autoReload)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager loadTPRewarded:autoReload];
}

void _tradplusDestroyRewardedVideo60(const char* adUnitId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager destroyTPRewarded];
}

void _tradplusRewardedVideoEntryAdScenario60(const char* adUnitId,const char* sceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager tpRewardedEntryAdScenario:GetStringParam(sceneId)];
}

bool _tradplusHasRewardedVideo60(const char* adUnitId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    return [manager tpRewardedIsReady];
}


void _tradplusShowRewardedVideo60(const char* adUnitId, const char* adSceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager showTPRewarded:GetStringParam(adSceneId)];
}

void _tradplusRewardedVideoServerSide60(const char* adUnitId, const char* user_id,const char* custom_data)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager setTPRewardedServerSideWithUserID:GetStringParam(user_id) customData:GetStringParam(custom_data)];
}

#pragma mark - Interstitial

void _tradplusRequestInterstitialAd60(const char* adUnitId, bool autoReload)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager loadTPInterstitial:autoReload];
}

void _tradplusDestroyInterstitialAd60(const char* adUnitId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager destroyTPInterstitial];
}

void _tradplusShowInterstitialAd60(const char* adUnitId, const char* adSceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager showTPInterstitial:GetStringParam(adSceneId)];
}

bool _tradplusIsInterstitialReady60(const char* adUnitId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    return [manager tpInterstitialIsReady];
}

void _tradplusInterstitialEntryAdScenario60(const char* adUnitId, const char* adSceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    return [manager tpInterstitialEntryAdScenario:GetStringParam(adSceneId)];
}

#pragma mark - NativeBanner

void _tradplusCreateNativeBanner(const char* adUnitId, int position, const char* sceneId, const char* className)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager autoShowNativeBanner:position adSceneId:GetStringParam(sceneId) className:GetStringParam(className)];
}

void _tradplusLoadNativeBanner(const char* adUnitId, int position, const char* sceneId, const char* className)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager loadNativeBanner:position adSceneId:GetStringParam(sceneId) className:GetStringParam(className)];
}

void _tradplusAutoShowNativeBanner(const char* adUnitId, int x, int y, int width, int height, const char* sceneId, const char* className)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    CGRect rect = CGRectMake(x, y, width, height);
    [manager autoShowNativeBannerWithRect:rect adSceneId:GetStringParam(sceneId) className:GetStringParam(className)];
}

void _tradplusLoadNativeBannerWithRect(const char* adUnitId, int x, int y, int width, int height, const char* sceneId, const char* className)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    CGRect rect = CGRectMake(x, y, width, height);
    [manager loadNativeBannerWithRect:rect adSceneId:GetStringParam(sceneId) className:GetStringParam(className)];
}

void _tradplusShowNativeBanner(const char* adUnitId, const char* sceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager showNativeBanner:GetStringParam(sceneId)];
}

void _tradplusHideNativeBanner(const char* adUnitId, bool hide)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager hideNativeBanner:hide];
}

void _tradplusDestroyNativeBanner(const char* adUnitId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager destroyNativeBanner];
}

void _tradplusNativeBannerEntryAdScenario(const char* adUnitId,const char* sceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager nativeBannerEntryAdScenario:GetStringParam(sceneId)];
}

#pragma mark - OfferWall
void _tradplusRequestOfferWall(const char* adUnitId,bool autoReload)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager loadTPOfferWall:autoReload];
}

void _tradplusShowOfferWall(const char* adUnitId,const char* sceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager showTPOfferWall:GetStringParam(sceneId)];
}

void _tradplusOfferWallEntryAdScenario(const char* adUnitId,const char* sceneId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager tpOfferWallEntryAdScenario:GetStringParam(sceneId)];
}

bool _tradplusHasOfferWall(const char* adUnitId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    return [manager tpOfferWallIsReady];
}

void _tradplusGetCurrencyBalance(const char* adUnitId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager tpOfferWallGetCurrencyBalance];
}

void _tradplusSpendCurrency(const char* adUnitId,int amount)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager tpOfferWallSpendCurrency:amount];
}

void _tradplusAwardCurrency(const char* adUnitId,int amount)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager tpOfferWallAwardCurrency:amount];
}


void _tradplusOfferWallOnDestroy(const char* adUnitId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager tpOfferWallDestroy];
}

void _tradplusSetOfferWallUserId(const char* adUnitId, const char* userId)
{
    TradPlusManager *manager = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    [manager setOfferWallUserId:GetStringParam(userId)];
}


void _tradplusLoadSplash60(const char* adUnitId){}

void _tradplusCheckCurrentArea()
{
    [TradPlus checkCurrentArea:^(BOOL isUnknown,BOOL isCN,BOOL isCA,BOOL isEU) {
        if(!isUnknown)
        {
            [TradPlusManager sendUnityEvent:@"EmitOnCheckCurrentAreaSuccessEvent" withArgs:@[@(isCN),@(isCA),@(isEU)]];
        }
        else
        {
            [TradPlusManager sendUnityEvent:@"EmitOnCheckCurrentAreaFailedEvent" withArgs:@[@"isUnknown"]];
        }
    }];
}
