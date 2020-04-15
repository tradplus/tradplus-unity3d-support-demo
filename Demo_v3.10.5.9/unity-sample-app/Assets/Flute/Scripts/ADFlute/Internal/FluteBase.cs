using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluteInternal.ThirdParty.MiniJSON;
using UnityEngine;

/// <summary>
/// This class provides common classes and utitilies needed across platforms
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class FluteBase
{
    public enum AdPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        Centered,
        BottomLeft,
        BottomCenter,
        BottomRight
    }
		
    /// <summary>
    /// Data object holding any SDK initialization parameters.
    /// </summary>
    public struct SdkConfiguration
    {
        /// <summary>
        /// Any ad unit that your app uses.
        /// </summary>
        public string AdUnitId;
    }
		

    public struct Reward
    {
        public string Label;
        public int Amount;


        public override string ToString()
        {
            return string.Format("\"{0} {1}\"", Amount, Label);
        }


        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Label) && Amount > 0;
        }
    }

    public static readonly string fluteSDKVersion = "3.10.5.2";
    private static string _pluginName;

    public static string PluginName {
        get { return _pluginName ?? (_pluginName = "Flute Unity Plugin v" + fluteSDKVersion); }
    }


    protected static void ValidateAdUnitForSdkInit(string adUnitId)
    {
        if (string.IsNullOrEmpty(adUnitId))
            Debug.LogError("A valid ad unit ID is needed to initialize the Flute SDK.");
    }


    protected static void ReportAdUnitNotFound(string adUnitId)
    {
        Debug.LogWarning(string.Format("AdUnit {0} not found: no plugin was initialized", adUnitId));
    }


    protected static Uri UrlFromString(string url)
    {
        if (String.IsNullOrEmpty(url)) return null;
        try {
            return new Uri(url);
        } catch {
            Debug.LogError("Invalid URL: " + url);
            return null;
        }
    }


    // Allocate the FluteManager singleton, which receives all callback events from the native SDKs.
    protected static void InitManager()
    {
        var type = typeof(FluteManager);
        var mgr = new GameObject("FluteManager", type).GetComponent<FluteManager>(); // Its Awake() method sets Instance.
        if (FluteManager.Instance != mgr)
            Debug.LogWarning(
                "It looks like you have the " + type.Name
                + " on a GameObject in your scene. Please remove the script from your scene.");
    }


    protected FluteBase() { }
}
