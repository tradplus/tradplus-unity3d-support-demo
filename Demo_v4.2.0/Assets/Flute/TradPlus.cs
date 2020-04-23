using System;
/// <summary>
/// This class is simply a proxy for calls to the platform-specific TradPlus APIs.
/// For the full documented API, <see cref="TradPlusUnityEditor"/>.
/// </summary>
public class TradPlus :
// Choose base class based on target platform...
#if UNITY_ANDROID
    TradPlusAndroid
#else
    TradPlusiOS
#endif
{
    private static string _sdkName;

    public static string SdkName
    {
        get { return _sdkName ?? (_sdkName = GetSdkName().Replace("+unity", "")); }
    }

    internal static void HasRewardedVide(string rewardedVideoAdUnits)
    {
        throw new NotImplementedException();
    }
}
