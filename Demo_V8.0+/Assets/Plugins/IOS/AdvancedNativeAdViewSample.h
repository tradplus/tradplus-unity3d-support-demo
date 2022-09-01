//
//  AdvancedNativeAdViewSample.h
//  iMsSDKSample
//
//  Created by ms-mac on 2017/2/24.
//  Copyright © 2017年 TradPlusAd All rights reserved.
//

#import <UIKit/UIKit.h>

@interface AdvancedNativeAdViewSample : UIView

@property (weak, nonatomic) IBOutlet UIImageView *iconImageView;
@property (weak, nonatomic) IBOutlet UIImageView *mainImageView;
@property (weak, nonatomic) IBOutlet UIImageView *privacyInformationIconImageView;
@property (weak, nonatomic) IBOutlet UILabel *titleLabel;
@property (weak, nonatomic) IBOutlet UILabel *ctaLabel;
@property (weak, nonatomic) IBOutlet UILabel *mainTextLabel;


+ (UINib *)nibForAd;

@end
