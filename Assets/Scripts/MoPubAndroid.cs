// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class MoPubAndroid : MoPubBase
// {
// 	public enum LocationAwareness
// 	{
// 		TRUNCATED,
// 		DISABLED,
// 		NORMAL
// 	}
//
// 	public static class PartnerApi
// 	{
// 		public static Uri CurrentConsentPrivacyPolicyUrl => MoPubBase.UrlFromString(PluginClass.CallStatic<string>("getCurrentPrivacyPolicyLink", new object[1]
// 		{
// 			MoPubBase.ConsentLanguageCode
// 		}));
//
// 		public static Uri CurrentVendorListUrl => MoPubBase.UrlFromString(PluginClass.CallStatic<string>("getCurrentVendorListLink", new object[1]
// 		{
// 			MoPubBase.ConsentLanguageCode
// 		}));
//
// 		public static string CurrentConsentIabVendorListFormat => PluginClass.CallStatic<string>("getCurrentVendorListIabFormat", Array.Empty<object>());
//
// 		public static string CurrentConsentPrivacyPolicyVersion => PluginClass.CallStatic<string>("getCurrentPrivacyPolicyVersion", Array.Empty<object>());
//
// 		public static string CurrentConsentVendorListVersion => PluginClass.CallStatic<string>("getCurrentVendorListVersion", Array.Empty<object>());
//
// 		public static string PreviouslyConsentedIabVendorListFormat => PluginClass.CallStatic<string>("getConsentedVendorListIabFormat", Array.Empty<object>());
//
// 		public static string PreviouslyConsentedPrivacyPolicyVersion => PluginClass.CallStatic<string>("getConsentedPrivacyPolicyVersion", Array.Empty<object>());
//
// 		public static string PreviouslyConsentedVendorListVersion => PluginClass.CallStatic<string>("getConsentedVendorListVersion", Array.Empty<object>());
//
// 		public static void GrantConsent()
// 		{
// 			PluginClass.CallStatic("grantConsent");
// 		}
//
// 		public static void RevokeConsent()
// 		{
// 			PluginClass.CallStatic("revokeConsent");
// 		}
// 	}
//
// 	private static readonly AndroidJavaClass PluginClass;
//
// 	private static readonly Dictionary<string, MoPubAndroidBanner> BannerPluginsDict;
//
// 	private static readonly Dictionary<string, MoPubAndroidInterstitial> InterstitialPluginsDict;
//
// 	private static readonly Dictionary<string, MoPubAndroidRewardedVideo> RewardedVideoPluginsDict;
//
// 	public static bool IsSdkInitialized => PluginClass.CallStatic<bool>("isSdkInitialized", Array.Empty<object>());
//
// 	public static bool AllowLegitimateInterest
// 	{
// 		get
// 		{
// 			return PluginClass.CallStatic<bool>("shouldAllowLegitimateInterest", Array.Empty<object>());
// 		}
// 		set
// 		{
// 			PluginClass.CallStatic("setAllowLegitimateInterest", value);
// 		}
// 	}
//
// 	public static LogLevel SdkLogLevel
// 	{
// 		get
// 		{
// 			return (LogLevel)PluginClass.CallStatic<int>("getLogLevel", Array.Empty<object>());
// 		}
// 		set
// 		{
// 			PluginClass.CallStatic<int>("setLogLevel", new object[1]
// 			{
// 				(int)value
// 			});
// 			MoPubBase.logLevel = value;
// 		}
// 	}
//
// 	public static bool CanCollectPersonalInfo => PluginClass.CallStatic<bool>("canCollectPersonalInfo", Array.Empty<object>());
//
// 	public static Consent.Status CurrentConsentStatus => Consent.FromString(PluginClass.CallStatic<string>("getPersonalInfoConsentState", Array.Empty<object>()));
//
// 	public static bool ShouldShowConsentDialog => PluginClass.CallStatic<bool>("shouldShowConsentDialog", Array.Empty<object>());
//
// 	public static bool IsConsentDialogReady => PluginClass.CallStatic<bool>("isConsentDialogReady", Array.Empty<object>());
//
// 	[Obsolete("Use the property name IsConsentDialogReady instead.")]
// 	public static bool IsConsentDialogLoaded => IsConsentDialogReady;
//
// 	public static bool? IsGdprApplicable
// 	{
// 		get
// 		{
// 			int num = PluginClass.CallStatic<int>("gdprApplies", Array.Empty<object>());
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
// 	static MoPubAndroid()
// 	{
// 		PluginClass = new AndroidJavaClass("com.mopub.unity.MoPubUnityPlugin");
// 		BannerPluginsDict = new Dictionary<string, MoPubAndroidBanner>();
// 		InterstitialPluginsDict = new Dictionary<string, MoPubAndroidInterstitial>();
// 		RewardedVideoPluginsDict = new Dictionary<string, MoPubAndroidRewardedVideo>();
// 		MoPubBase.InitManager();
// 	}
//
// 	public static void InitializeSdk(string anyAdUnitId)
// 	{
// 		MoPubBase.ValidateAdUnitForSdkInit(anyAdUnitId);
// 		SdkConfiguration sdkConfiguration = default(SdkConfiguration);
// 		sdkConfiguration.AdUnitId = anyAdUnitId;
// 		InitializeSdk(sdkConfiguration);
// 	}
//
// 	public static void InitializeSdk(SdkConfiguration sdkConfiguration)
// 	{
// 		MoPubBase.logLevel = sdkConfiguration.LogLevel;
// 		MoPubLog.Log("InitializeSdk", "SDK initialization started");
// 		MoPubBase.ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
// 		PluginClass.CallStatic("initializeSdk", sdkConfiguration.AdUnitId, sdkConfiguration.AdditionalNetworksString, sdkConfiguration.MediationSettingsJson, sdkConfiguration.AllowLegitimateInterest, (int)sdkConfiguration.LogLevel, sdkConfiguration.NetworkConfigurationsJson, sdkConfiguration.MoPubRequestOptionsJson);
// 	}
//
// 	public static void LoadBannerPluginsForAdUnits(string[] bannerAdUnitIds)
// 	{
// 		foreach (string text in bannerAdUnitIds)
// 		{
// 			BannerPluginsDict[text] = new MoPubAndroidBanner(text);
// 		}
// 		UnityEngine.Debug.Log(bannerAdUnitIds.Length + " banner AdUnits loaded for plugins:\n" + string.Join(", ", bannerAdUnitIds));
// 	}
//
// 	public static void LoadInterstitialPluginsForAdUnits(string[] interstitialAdUnitIds)
// 	{
// 		foreach (string text in interstitialAdUnitIds)
// 		{
// 			InterstitialPluginsDict[text] = new MoPubAndroidInterstitial(text);
// 		}
// 		UnityEngine.Debug.Log(interstitialAdUnitIds.Length + " interstitial AdUnits loaded for plugins:\n" + string.Join(", ", interstitialAdUnitIds));
// 	}
//
// 	public static void LoadRewardedVideoPluginsForAdUnits(string[] rewardedVideoAdUnitIds)
// 	{
// 		foreach (string text in rewardedVideoAdUnitIds)
// 		{
// 			RewardedVideoPluginsDict[text] = new MoPubAndroidRewardedVideo(text);
// 		}
// 		UnityEngine.Debug.Log(rewardedVideoAdUnitIds.Length + " rewarded video AdUnits loaded for plugins:\n" + string.Join(", ", rewardedVideoAdUnitIds));
// 	}
//
// 	public static void EnableLocationSupport(bool shouldUseLocation)
// 	{
// 		PluginClass.CallStatic("setLocationAwareness", LocationAwareness.NORMAL.ToString());
// 	}
//
// 	public static void ReportApplicationOpen(string iTunesAppId = null)
// 	{
// 		PluginClass.CallStatic("reportApplicationOpen");
// 	}
//
// 	protected static string GetSdkName()
// 	{
// 		return "Android SDK v" + PluginClass.CallStatic<string>("getSDKVersion", Array.Empty<object>());
// 	}
//
// 	public static void AddFacebookTestDeviceId(string hashedDeviceId)
// 	{
// 		PluginClass.CallStatic("addFacebookTestDeviceId", hashedDeviceId);
// 	}
//
// 	public static void CreateBanner(string adUnitId, AdPosition position)
// 	{
// 		MoPubLog.Log("CreateBanner", "Attempting to load ad");
// 		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
// 		{
// 			value.CreateBanner(position);
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
// 		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
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
// 		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
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
// 		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
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
// 		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
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
// 		if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner value))
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
// 		if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial value))
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
// 		if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial value))
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
// 		if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial value))
// 		{
// 			return value.IsInterstitialReady;
// 		}
// 		MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		return false;
// 	}
//
// 	public void DestroyInterstitialAd(string adUnitId)
// 	{
// 		if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial value))
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
// 		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo value))
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
// 		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo value))
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
// 		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo value))
// 		{
// 			return value.HasRewardedVideo();
// 		}
// 		MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		return false;
// 	}
//
// 	public static List<Reward> GetAvailableRewards(string adUnitId)
// 	{
// 		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo value))
// 		{
// 			return value.GetAvailableRewards();
// 		}
// 		MoPubBase.ReportAdUnitNotFound(adUnitId);
// 		return null;
// 	}
//
// 	public static void SelectReward(string adUnitId, Reward selectedReward)
// 	{
// 		if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo value))
// 		{
// 			value.SelectReward(selectedReward);
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
// 		PluginClass.CallStatic("loadConsentDialog");
// 	}
//
// 	public static void ShowConsentDialog()
// 	{
// 		MoPubLog.Log("ShowConsentDialog", "Consent dialog attempting to show");
// 		PluginClass.CallStatic("showConsentDialog");
// 	}
//
// 	public static void ForceGdprApplicable()
// 	{
// 		PluginClass.CallStatic("forceGdprApplies");
// 	}
// }
