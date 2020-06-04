//
//  TradPlusBinding.m
//  TradPlus
//
//  Copyright (c) 2017 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>

#import "TradPlusManager.h"
#import <TradPlusAds/MsSDKUtils.h>

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

void _tradplusInitializeSdk(const char* adUnitIdString, const char* advancedBiddersString,
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
//        [TradPlusManager sendUnityEvent:@"EmitSdkInitializedEvent" withArgs:@[adUnitId]];
//    }];
}

//bool _tradplusIsSdkInitialized()
//{
//}

const char* _tradplusGetSDKVersion()
{
    return cStringCopy([MsSDKUtils getVersion]);
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Banners

void _tradplusCreateBanner(int bannerPosition, const char* adUnitId)
{
    TradPlusAdPosition position = (TradPlusAdPosition)bannerPosition;

    [[TradPlusManager managerForAdunit:GetStringParam(adUnitId)] createBanner:position];
}

void _tradplusShowBanner(const char* adUnitId, bool shouldShow)
{
    if (shouldShow)
        [[TradPlusManager managerForAdunit:GetStringParam(adUnitId)] showBanner];
    else
        [[TradPlusManager managerForAdunit:GetStringParam(adUnitId)] hideBanner:NO];
}

void _tradplusDestroyBanner(const char* adUnitId)
{
    [[TradPlusManager managerForAdunit:GetStringParam(adUnitId)] destroyBanner];
}

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Interstitials

void _tradplusRequestInterstitialAd(const char* adUnitId)
{
    [[TradPlusManager managerForAdunit:GetStringParam(adUnitId)] requestInterstitialAd];
}

bool _tradplusIsInterstitialReady(const char* adUnitId)
{
    TradPlusManager* mgr = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    return mgr != nil && [mgr interstitialIsReady];
}

void _tradplusShowInterstitialAd(const char* adUnitId)
{
    [[TradPlusManager managerForAdunit:GetStringParam(adUnitId)] showInterstitialAd];
}

void _tradplusDestroyInterstitialAd(const char* adUnitId)
{
    [[TradPlusManager managerForAdunit:GetStringParam(adUnitId)] destroyInterstitialAd];
}

#pragma mark - RewardedVideo

void _tradplusRequestRewardedVideo(const char* adUnitId)
{
    [[TradPlusManager managerForAdunit:GetStringParam(adUnitId)] requestRewardedVideo];
}

bool _tradplusHasRewardedVideo(const char* adUnitId)
{
    TradPlusManager* mgr = [TradPlusManager managerForAdunit:GetStringParam(adUnitId)];
    return mgr != nil && [mgr hasRewardedVideo];
}

void _tradplusShowRewardedVideo(const char* adUnitId)
{
    [[TradPlusManager managerForAdunit:GetStringParam(adUnitId)] showRewardedVideo];
}

void _tradplusDestroyRewardedVideo(const char* adUnitId)
{
    [[TradPlusManager managerForAdunit:GetStringParam(adUnitId)] destroyRewardedVideo];
}

