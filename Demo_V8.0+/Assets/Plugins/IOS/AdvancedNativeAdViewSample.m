//
//  AdvancedNativeAdViewSample.m
//  iMsSDKSample
//
//  Created by ms-mac on 2017/2/24.
//  Copyright © 2017年 TradPlusAd All rights reserved.
//

#import "AdvancedNativeAdViewSample.h"
#import <TradPlusAds/TradPlusAds.h>

@interface AdvancedNativeAdViewSample()

@end

@implementation AdvancedNativeAdViewSample

- (void)layoutSubviews
{
//    _titleLabel.numberOfLines = 2;
//    [_titleLabel sizeToFit];
    _mainTextLabel.numberOfLines = 3;
    [_mainTextLabel sizeToFit];
    
    _ctaLabel.layer.cornerRadius = 10;
    _ctaLabel.layer.masksToBounds = YES;
    _ctaLabel.layer.borderWidth = 1.0;
    _ctaLabel.layer.borderColor = [[UIColor blueColor] CGColor];
}

#pragma mark - <MSNativeAdRendering>

- (UILabel *)nativeMainTextLabel
{
    return self.mainTextLabel;
}

- (UILabel *)nativeTitleTextLabel
{
    return self.titleLabel;
}

- (UILabel *)nativeCallToActionTextLabel
{
    return self.ctaLabel;
}

- (UIImageView *)nativeIconImageView
{
    return self.iconImageView;
}

- (UIImageView *)nativeMainImageView
{
    return self.mainImageView;
}

- (UIView *)nativeVideoView
{
    return self.mainImageView;
}

- (UIImageView *)nativePrivacyInformationIconImageView
{
    return self.privacyInformationIconImageView;
}

+ (UINib *)nibForAd
{
    NSBundle *resourceBundle = [NSBundle bundleForClass:self];
    return [UINib nibWithNibName:@"AdvancedNativeAdViewSample" bundle:resourceBundle];
}

@end
