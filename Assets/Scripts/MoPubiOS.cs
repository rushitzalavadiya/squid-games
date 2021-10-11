// using System;
// using System.Collections.Generic;
// using System.Runtime.InteropServices;
// using UnityEngine;
//
// public class MoPubiOS : MoPubBase
// {
// 	public static class PartnerApi
// 	{
// 		public static Uri CurrentConsentPrivacyPolicyUrl => MoPubBase.UrlFromString(_moPubCurrentConsentPrivacyPolicyUrl(MoPubBase.ConsentLanguageCode));
//
// 		public static Uri CurrentVendorListUrl => MoPubBase.UrlFromString(_moPubCurrentConsentVendorListUrl(MoPubBase.ConsentLanguageCode));
//
// 		public static string CurrentConsentIabVendorListFormat => _moPubCurrentConsentIabVendorListFormat();
//
// 		public static string CurrentConsentPrivacyPolicyVersion => _moPubCurrentConsentPrivacyPolicyVersion();
//
// 		public static string CurrentConsentVendorListVersion => _moPubCurrentConsentVendorListVersion();
//
// 		public static string PreviouslyConsentedIabVendorListFormat => _moPubPreviouslyConsentedIabVendorListFormat();
//
// 		public static string PreviouslyConsentedPrivacyPolicyVersion => _moPubPreviouslyConsentedPrivacyPolicyVersion();
//
// 		public static string PreviouslyConsentedVendorListVersion => _moPubPreviouslyConsentedVendorListVersion();
//
// 		public static void GrantConsent()
// 		{
// 			_moPubGrantConsent();
// 		}
//
// 		public static void RevokeConsent()
// 		{
// 			_moPubRevokeConsent();
// 		}
// 	}
//
// 	private static readonly Dictionary<string, MoPubBinding> PluginsDict;
//
// 	public static bool IsSdkInitialized => _moPubIsSdkInitialized();
//
// 	public static bool AllowLegitimateInterest
// 	{
// 		get
// 		{
// 			return _moPubAllowLegitimateInterest();
// 		}
// 		set
// 		{
// 			_moPubSetAllowLegitimateInterest(value);
// 		}
// 	}
//
// 	public static LogLevel SdkLogLevel
// 	{
// 		get
// 		{
// 			return (LogLevel)_moPubGetLogLevel();
// 		}
// 		set
// 		{
// 			MoPubBase.logLevel = value;
// 			_moPubSetLogLevel((int)value);
// 		}
// 	}
//
// 	public static bool CanCollectPersonalInfo => _moPubCanCollectPersonalInfo();
//
// 	public static Consent.Status CurrentConsentStatus => (Consent.Status)_moPubCurrentConsentStatus();
//
// 	public static bool ShouldShowConsentDialog => _moPubShouldShowConsentDialog();
//
// 	public static bool IsConsentDialogReady => _moPubIsConsentDialogReady();
//
// 	[Obsolete("Use the property name IsConsentDialogReady instead.")]
// 	public static bool IsConsentDialogLoaded => IsConsentDialogReady;
//
// 	public static bool? IsGdprApplicable
// 	{
// 		get
// 		{
// 			int num = _moPubIsGDPRApplicable();
// 			if (num != 0)
// 			{
// 				if (num <= 0)
// 				{
// 					return false;
// 				}
// 				return true;
// 			}
// 			return null;
// 		}
// 	}
//
// 	static MoPubiOS()
// 	{
// 		PluginsDict = new Dictionary<string, MoPubBinding>();
// 		MoPubBase.InitManager();
// 	}
//
// 	public static void InitializeSdk(string anyAdUnitId)
// 	{
// 		MoPubLog.Log("InitializeSdk", "SDK initialization started");
// 		MoPubBase.ValidateAdUnitForSdkInit(anyAdUnitId);
// 		SdkConfiguration sdkConfiguration = default(SdkConfiguration);
// 		sdkConfiguration.AdUnitId = anyAdUnitId;
// 		InitializeSdk(sdkConfiguration);
// 	}
//
// 	public static void InitializeSdk(SdkConfiguration sdkConfiguration)
// 	{
// 		MoPubBase.logLevel = sdkConfiguration.LogLevel;
// 		MoPubBase.ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
// 		_moPubInitializeSdk(sdkConfiguration.AdUnitId, sdkConfiguration.AdditionalNetworksString, sdkConfiguration.MediationSettingsJson, sdkConfiguration.AllowLegitimateInterest, (int)sdkConfiguration.LogLevel, sdkConfiguration.NetworkConfigurationsJson, sdkConfiguration.MoPubRequestOptionsJson);
// 	}
//
// 	public static void LoadBannerPluginsForAdUnits(string[] adUnitIds)
// 	{
// 		LoadPluginsForAdUnits(adUnitIds);
// 	}
//
// 	public static void LoadInterstitialPluginsForAdUnits(string[] adUnitIds)
// 	{
// 		LoadPluginsForAdUnits(adUnitIds);
// 	}
//
// 	public static void LoadRewardedVideoPluginsForAdUnits(string[] adUnitIds)
// 	{
// 		LoadPluginsForAdUnits(adUnitIds);
// 	}
//
// 	public static void EnableLocationSupport(bool shouldUseLocation)
// 	{
// 		_moPubEnableLocationSupport(shouldUseLocation: true);
// 	}
//
// 	public static void ReportApplicationOpen(string iTunesAppId = null)
// 	{
// 		_moPubReportApplicationOpen(iTunesAppId);
// 	}
//
// 	protected static string GetSdkName()
// 	{
// 		return "iOS SDK v" + _moPubGetSDKVersion();
// 	}
//
// 	private static void LoadPluginsForAdUnits(string[] adUnitIds)
// 	{
// 		foreach (string text in adUnitIds)
// 		{
// 			PluginsDict[text] = new MoPubBinding(text);
// 		}
// 		UnityEngine.Debug.Log(adUnitIds.Length + " AdUnits loaded for plugins:\n" + string.Join(", ", adUnitIds));
// 	}
//
// 	public static void ForceWKWebView(bool shouldForce)
// 	{
// 		_moPubForceWKWebView(shouldForce);
// 	}
//
// 	public static void CreateBanner(string adUnitId, AdPosition position, BannerType bannerType = BannerType.Size320x50)
// 	{
// 		MoPubLog.Log("CreateBanner", "Attempting to load ad");
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.CreateBanner(bannerType, position);
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public static void ShowBanner(string adUnitId, bool shouldShow)
// 	{
// 		MoPubLog.Log("ShowBanner", "Attempting to show ad");
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.ShowBanner(shouldShow);
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public static void RefreshBanner(string adUnitId, string keywords, string userDataKeywords = "")
// 	{
// 		MoPubLog.Log("RefreshBanner", "Attempting to show ad");
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.RefreshBanner(keywords, userDataKeywords);
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public void SetAutorefresh(string adUnitId, bool enabled)
// 	{
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.SetAutorefresh(enabled);
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public void ForceRefresh(string adUnitId)
// 	{
// 		MoPubLog.Log("ForceRefresh", "Attempting to show ad");
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.ForceRefresh();
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public static void DestroyBanner(string adUnitId)
// 	{
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.DestroyBanner();
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public static void RequestInterstitialAd(string adUnitId, string keywords = "", string userDataKeywords = "")
// 	{
// 		MoPubLog.Log("RequestInterstitialAd", "Attempting to load ad");
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.RequestInterstitialAd(keywords, userDataKeywords);
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public static void ShowInterstitialAd(string adUnitId)
// 	{
// 		MoPubLog.Log("ShowInterstitialAd", "Attempting to show ad");
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.ShowInterstitialAd();
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public bool IsInterstialReady(string adUnitId)
// 	{
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			return value.IsInterstitialReady;
// 		}
// 		MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		return false;
// 	}
//
// 	public void DestroyInterstitialAd(string adUnitId)
// 	{
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.DestroyInterstitialAd();
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public static void RequestRewardedVideo(string adUnitId, List<LocalMediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
// 	{
// 		MoPubLog.Log("RequestRewardedVideo", "Attempting to load ad");
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.RequestRewardedVideo(mediationSettings, keywords, userDataKeywords, latitude, longitude, customerId);
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public static void ShowRewardedVideo(string adUnitId, string customData = null)
// 	{
// 		MoPubLog.Log("ShowRewardedVideo", "Attempting to show ad");
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.ShowRewardedVideo(customData);
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public static bool HasRewardedVideo(string adUnitId)
// 	{
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			return value.HasRewardedVideo();
// 		}
// 		MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		return false;
// 	}
//
// 	public static List<Reward> GetAvailableRewards(string adUnitId)
// 	{
// 		if (!PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 			return null;
// 		}
// 		List<Reward> availableRewards = value.GetAvailableRewards();
// 		UnityEngine.Debug.Log($"GetAvailableRewards found {availableRewards.Count} rewards for ad unit {adUnitId}");
// 		return availableRewards;
// 	}
//
// 	public static void SelectReward(string adUnitId, Reward selectedReward)
// 	{
// 		if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding value))
// 		{
// 			value.SelectedReward = selectedReward;
// 		}
// 		else
// 		{
// 			MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		}
// 	}
//
// 	public static void LoadConsentDialog()
// 	{
// 		MoPubLog.Log("LoadConsentDialog", "Attempting to load consent dialog");
// 		_moPubLoadConsentDialog();
// 	}
//
// 	public static void ShowConsentDialog()
// 	{
// 		MoPubLog.Log("ShowConsentDialog", "Consent dialog attempting to show");
// 		_moPubShowConsentDialog();
// 	}
//
// 	public static void ForceGdprApplicable()
// 	{
// 		_moPubForceGDPRApplicable();
// 	}
//
// 	[DllImport("__Internal")]
// 	private static extern void _moPubInitializeSdk(string adUnitId, string additionalNetworksJson, string mediationSettingsJson, bool allowLegitimateInterest, int logLevel, string adapterConfigJson, string moPubRequestOptionsJson);
//
// 	[DllImport("__Internal")]
// 	private static extern bool _moPubIsSdkInitialized();
//
// 	[DllImport("__Internal")]
// 	private static extern string _moPubGetSDKVersion();
//
// 	[DllImport("__Internal")]
// 	private static extern void _moPubEnableLocationSupport(bool shouldUseLocation);
//
// 	[DllImport("__Internal")]
// 	private static extern void _moPubSetAllowLegitimateInterest(bool allowLegitimateInterest);
//
// 	[DllImport("__Internal")]
// 	private static extern bool _moPubAllowLegitimateInterest();
//
// 	[DllImport("__Internal")]
// 	private static extern int _moPubGetLogLevel();
//
// 	[DllImport("__Internal")]
// 	private static extern void _moPubSetLogLevel(int logLevel);
//
// 	[DllImport("__Internal")]
// 	private static extern void _moPubForceWKWebView(bool shouldForce);
//
// 	[DllImport("__Internal")]
// 	private static extern void _moPubReportApplicationOpen(string iTunesAppId);
//
// 	[DllImport("__Internal")]
// 	private static extern bool _moPubCanCollectPersonalInfo();
//
// 	[DllImport("__Internal")]
// 	private static extern int _moPubCurrentConsentStatus();
//
// 	[DllImport("__Internal")]
// 	private static extern int _moPubIsGDPRApplicable();
//
// 	[DllImport("__Internal")]
// 	private static extern int _moPubForceGDPRApplicable();
//
// 	[DllImport("__Internal")]
// 	private static extern bool _moPubShouldShowConsentDialog();
//
// 	[DllImport("__Internal")]
// 	private static extern bool _moPubIsConsentDialogReady();
//
// 	[DllImport("__Internal")]
// 	private static extern void _moPubLoadConsentDialog();
//
// 	[DllImport("__Internal")]
// 	private static extern void _moPubShowConsentDialog();
//
// 	[DllImport("__Internal")]
// 	private static extern string _moPubCurrentConsentPrivacyPolicyUrl(string isoLanguageCode = null);
//
// 	[DllImport("__Internal")]
// 	private static extern string _moPubCurrentConsentVendorListUrl(string isoLanguageCode = null);
//
// 	[DllImport("__Internal")]
// 	private static extern void _moPubGrantConsent();
//
// 	[DllImport("__Internal")]
// 	private static extern void _moPubRevokeConsent();
//
// 	[DllImport("__Internal")]
// 	private static extern string _moPubCurrentConsentIabVendorListFormat();
//
// 	[DllImport("__Internal")]
// 	private static extern string _moPubCurrentConsentPrivacyPolicyVersion();
//
// 	[DllImport("__Internal")]
// 	private static extern string _moPubCurrentConsentVendorListVersion();
//
// 	[DllImport("__Internal")]
// 	private static extern string _moPubPreviouslyConsentedIabVendorListFormat();
//
// 	[DllImport("__Internal")]
// 	private static extern string _moPubPreviouslyConsentedPrivacyPolicyVersion();
//
// 	[DllImport("__Internal")]
// 	private static extern string _moPubPreviouslyConsentedVendorListVersion();
// }
