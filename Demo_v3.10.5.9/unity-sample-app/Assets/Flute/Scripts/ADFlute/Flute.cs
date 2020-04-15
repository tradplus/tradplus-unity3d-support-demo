/// <summary>
/// This class is simply a proxy for calls to the platform-specific Flute APIs.
/// For the full documented API, <see cref="FluteUnityEditor"/>.
/// </summary>
public class Flute :
// Choose base class based on target platform...
#if UNITY_ANDROID
    FluteAndroid
#else
    FluteiOS
#endif
{
    private static string _sdkName;

    public static string SdkName {
        get { return _sdkName ?? (_sdkName = GetSdkName().Replace("+unity", "")); }
    }
}
