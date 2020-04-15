//
//  FluteBinding.m
//  Flute
//
//  Copyright (c) 2017 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>

#import "FluteManager.h"
#import "MsSDKUtils.h"
//#import "MoPub.h"

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

void _fluteInitializeSdk(const char* adUnitIdString, const char* advancedBiddersString,
                         const char* mediationSettingsJson, const char* networksToInitString)
{
    NSString* adUnitId = GetStringParam(adUnitIdString);
    [MsSDKUtils msSDKInit:^(NSError * _Nonnull error) {
        //
    }];
    
//    MPMoPubConfiguration * sdkConfig = [[MPMoPubConfiguration alloc] initWithAdUnitIdForAppInitialization: @"tXjjj4avSWU2jwaS68uReRDYhA_ayRkd"];
//    sdkConfig.globalMediationSettings = @[];
////    sdkConfig.loggingLevel = MPLogLevelInfo;
//    [[MoPub sharedInstance] initializeSdkWithConfiguration:sdkConfig completion:^{
//        NSLog(@"SDK initialization complete");
//        [FluteManager sendUnityEvent:@"EmitSdkInitializedEvent" withArgs:@[adUnitId]];
//    }];
}

//bool _fluteIsSdkInitialized()
//{
//}

const char* _fluteGetSDKVersion()
{
    return cStringCopy([MsSDKUtils getVersion]);
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Banners

void _fluteCreateBanner(int bannerPosition, const char* adUnitId)
{
    FluteAdPosition position = (FluteAdPosition)bannerPosition;

    [[FluteManager managerForAdunit:GetStringParam(adUnitId)] createBanner:position];
}

void _fluteShowBanner(const char* adUnitId, bool shouldShow)
{
    if (shouldShow)
        [[FluteManager managerForAdunit:GetStringParam(adUnitId)] showBanner];
    else
        [[FluteManager managerForAdunit:GetStringParam(adUnitId)] hideBanner:NO];
}

void _fluteDestroyBanner(const char* adUnitId)
{
    [[FluteManager managerForAdunit:GetStringParam(adUnitId)] destroyBanner];
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Interstitials

void _fluteRequestInterstitialAd(const char* adUnitId)
{
    [[FluteManager managerForAdunit:GetStringParam(adUnitId)] requestInterstitialAd];
}

bool _fluteIsInterstitialReady(const char* adUnitId)
{
    FluteManager* mgr = [FluteManager managerForAdunit:GetStringParam(adUnitId)];
    return mgr != nil && [mgr interstitialIsReady];
}

void _fluteShowInterstitialAd(const char* adUnitId)
{
    [[FluteManager managerForAdunit:GetStringParam(adUnitId)] showInterstitialAd];
}

void _fluteDestroyInterstitialAd(const char* adUnitId)
{
    [[FluteManager managerForAdunit:GetStringParam(adUnitId)] destroyInterstitialAd];
}

#pragma mark - RewardedVideo

void _fluteRequestRewardedVideo(const char* adUnitId)
{
    [[FluteManager managerForAdunit:GetStringParam(adUnitId)] requestRewardedVideo];
}

bool _fluteHasRewardedVideo(const char* adUnitId)
{
    FluteManager* mgr = [FluteManager managerForAdunit:GetStringParam(adUnitId)];
    return mgr != nil && [mgr hasRewardedVideo];
}

void _fluteShowRewardedVideo(const char* adUnitId)
{
    [[FluteManager managerForAdunit:GetStringParam(adUnitId)] showRewardedVideo];
}

void _fluteDestroyRewardedVideo(const char* adUnitId)
{
    [[FluteManager managerForAdunit:GetStringParam(adUnitId)] destroyRewardedVideo];
}

