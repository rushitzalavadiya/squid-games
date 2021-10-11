// using GameAnalyticsSDK;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Voodoo.Sauce.Internal.Analytics
{
	internal static class VoodooAnalytics
	{
		private const string TAG = "VoodooAnalytics";

		private const string CohortDefault = "Control";

		private const int ControlCohortCount = 1;

		private const string PrefsCohort = "VoodooSauce_Cohort";

		private const string PrefsLaunchCount = "VoodooSauce_AppLaunchCount";

		private const string PrefsGameCount = "VoodooSauce_GameCount";

		private const string PrefsHighScore = "VoodooSauce_HighScore";

		private static readonly int[] GameCountsToTrackOnTenjin = new int[4]
		{
			2,
			10,
			100,
			1000
		};

		private const string CrossPromoAdShownEventName = "Cross Promo Ad Shown";

		private const string CrossPromoAdClickEventName = "Cross Promo Ad Click";

		private const string CrossPromoAdErrorEventName = "Cross Promo Ad Error";

		internal static Action<int, bool> OnGamePlayed;

		private static bool _forcedMixpanelInit;

		private static string[] _runningAbTests;

		internal static bool HasGameStarted
		{
			get;
			private set;
		}
		//
		// internal static void InitializeAbTest(string[] runningAbTests, float usersPercentPerCohort, DebugForcedCohort debugForcedCohort)
		// {
		// 	PlayerPrefs.SetInt("VoodooSauce_AppLaunchCount", PlayerPrefs.GetInt("VoodooSauce_AppLaunchCount", 0) + 1);
		// 	_runningAbTests = new string[runningAbTests.Length + 1];
		// 	for (int i = 0; i < 1; i++)
		// 	{
		// 		_runningAbTests[i] = "Control " + (i + 1);
		// 	}
		// 	for (int j = 0; j < runningAbTests.Length; j++)
		// 	{
		// 		_runningAbTests[j + 1] = runningAbTests[j];
		// 	}
		// 	if (debugForcedCohort.IsDebugCohort())
		// 	{
		// 		VoodooLog.Log("VoodooAnalytics", "Re-initializing cohort because debug is enabled.");
		// 		_forcedMixpanelInit = !IsFirstTimeAppLaunched();
		// 		if (debugForcedCohort.HasForcedNoCohort())
		// 		{
		// 			SetPlayerCohort(null);
		// 		}
		// 		else
		// 		{
		// 			SetPlayerCohort(debugForcedCohort.GetCohort());
		// 		}
		// 	}
		// 	else if (IsFirstTimeAppLaunched())
		// 	{
		// 		string text = VoodooGDPRUtils.GetLocale().ToLower();
		// 		if (Application.platform == RuntimePlatform.IPhonePlayer || (Application.platform == RuntimePlatform.OSXEditor && text == "us"))
		// 		{
		// 			VoodooLog.Log("VoodooAnalytics", "Initializing cohort because first time app launched.");
		// 			SetPlayerCohort(GenerateNewRandomCohort(_runningAbTests, usersPercentPerCohort));
		// 		}
		// 		else
		// 		{
		// 			VoodooLog.Log("VoodooAnalytics", $"Not initializing AB Test with {Application.platform} - {text}");
		// 		}
		// 	}
		// 	VoodooLog.Log("VoodooAnalytics", $"AB Test Status: AB Test: {GetPlayerCohort()} - Event tracked: {PlayerIsInACohort()} - People tracked: false");
		// }
		//
		// internal static void InitializeAnalytics(bool consent, int subVersion)
		// {
		// 	VoodooTrackers.SetConsent(consent);
		// 	VoodooTrackers.TrackAppOpen();
		// 	TenjinWrapper.InitWithConsent(consent, subVersion);
		// 	MoPubManager.OnAdClickedEvent += delegate
		// 	{
		// 		TenjinWrapper.TrackTenjinCustomEvent("banner_click");
		// 	};
		// 	MoPubManager.OnInterstitialClickedEvent += delegate
		// 	{
		// 		TenjinWrapper.TrackTenjinCustomEvent("fs_click");
		// 	};
		// 	MoPubManager.OnRewardedVideoClickedEvent += delegate
		// 	{
		// 		TenjinWrapper.TrackTenjinCustomEvent("rv_click");
		// 	};
		// 	if (IsFirstTimeAppLaunched() || _forcedMixpanelInit)
		// 	{
		// 		MixpanelWrapper.RegisterMixpanelSuperProperties();
		// 	}
		// 	MixpanelWrapper.SetConsent(consent);
		// 	if (IsFirstTimeAppLaunched() || _forcedMixpanelInit)
		// 	{
		// 		MixpanelWrapper.OnFirstAppLaunched();
		// 	}
		// 	MixpanelWrapper.OnAppLaunched();
		// }
		//
		// internal static void SetConsent(bool consent)
		// {
		// 	TenjinWrapper.SetConsent(consent);
		// 	MixpanelWrapper.SetConsent(consent);
		// }
		//
		// internal static bool IsFirstTimeAppLaunched()
		// {
		// 	return PlayerPrefs.GetInt("VoodooSauce_AppLaunchCount", 0) == 1;
		// }

		public static void SetPlayerCohort(string cohort)
		{
			if (cohort == null)
			{
				if (PlayerPrefs.HasKey("VoodooSauce_Cohort"))
				{
					PlayerPrefs.DeleteKey("VoodooSauce_Cohort");
				}
			}
			else
			{
				PlayerPrefs.SetString("VoodooSauce_Cohort", cohort);
			}
		}

		private static string GenerateNewRandomCohort(string[] runningAbTests, float usersPercentPerCohort)
		{
			float value = UnityEngine.Random.value;
			for (int i = 0; i < runningAbTests.Length; i++)
			{
				if (value < (float)(i + 1) * usersPercentPerCohort)
				{
					return runningAbTests[i];
				}
			}
			return null;
		}

		public static bool PlayerIsInACohort()
		{
			return PlayerPrefs.HasKey("VoodooSauce_Cohort");
		}

		internal static string GetPlayerCohort()
		{
			if (!PlayerIsInACohort())
			{
				return null;
			}
			return PlayerPrefs.GetString("VoodooSauce_Cohort");
		}

		// internal static void OnGameStarted()
		// {
		// 	HasGameStarted = true;
		// 	PlayerPrefs.SetInt("VoodooSauce_GameCount", PlayerPrefs.GetInt("VoodooSauce_GameCount", 0) + 1);
		// 	MixpanelWrapper.OnGameStarted();
		// 	GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");
		// }
		//
		// internal static void OnGameFinished(bool levelComplete, float score, Dictionary<string, object> eventProperties)
		// {
		// 	int @int = PlayerPrefs.GetInt("VoodooSauce_GameCount");
		// 	float @float = PlayerPrefs.GetFloat("VoodooSauce_HighScore", 0f);
		// 	if (score > @float)
		// 	{
		// 		PlayerPrefs.SetFloat("VoodooSauce_HighScore", score);
		// 	}
		// 	MixpanelWrapper.OnGameFinished(levelComplete, score, @int, eventProperties);
		// 	VoodooLog.Log("VoodooAnalytics", "Sending progression event \"Complete\" to Game Analytics.");
		// 	GameAnalytics.NewProgressionEvent(levelComplete ? GAProgressionStatus.Complete : GAProgressionStatus.Fail, "game", (int)score);
		// 	if (Array.IndexOf(GameCountsToTrackOnTenjin, @int) > -1)
		// 	{
		// 		VoodooLog.Log("VoodooAnalytics", "Sending " + @int + "_games_played event to Tenjin.");
		// 		TenjinWrapper.TrackTenjinCustomEvent(@int + "_games_played");
		// 	}
		// 	HasGameStarted = false;
		// 	if (OnGamePlayed != null)
		// 	{
		// 		OnGamePlayed(@int, score > @float);
		// 	}
		// }
		//
		// internal static void OnFsShown()
		// {
		// 	TenjinWrapper.TrackTenjinCustomEvent("fs_shown");
		// 	MixpanelWrapper.TrackFsShown();
		// }
		//
		// internal static void OnRvShown()
		// {
		// 	TenjinWrapper.TrackTenjinCustomEvent("rv_shown");
		// 	MixpanelWrapper.TrackRvShown();
		// }
		//
		// internal static void TrackCustomEvent(string eventName, Dictionary<string, object> eventProperties)
		// {
		// 	MixpanelWrapper.TrackCustomEvent(eventName, eventProperties);
		// 	VoodooLog.Log("VoodooAnalytics", "Sending custom event to GameAnalytics : " + eventName);
		// 	GameAnalytics.NewDesignEvent(eventName);
		// }
		//
		// internal static void TrackPurchase(Product product, float productPriceInUSD)
		// {
		// 	VoodooLog.Log("VoodooAnalytics", "Tracking purchase of " + product.definition.id + " for " + productPriceInUSD + "$ in GameAnalytics");
		// 	GameAnalytics.NewBusinessEvent(product.metadata.isoCurrencyCode, Mathf.RoundToInt((float)product.metadata.localizedPrice * 100f), product.definition.id, product.definition.id, "");
		// 	MixpanelWrapper.TrackPurchase(product, productPriceInUSD);
		// 	VoodooLog.Log("VoodooAnalytics", "Tracking purchase on Tenjin too");
		// 	TenjinWrapper.TrackTenjinPurchase(product);
		// }
		//
		// internal static void OnCrossPromoShown(AssetModel asset)
		// {
		// 	MixpanelWrapper.TrackCrossPromoEvent("Cross Promo Ad Shown", asset);
		// }
		//
		// internal static void OnCrossPromoClick(AssetModel asset)
		// {
		// 	MixpanelWrapper.TrackCrossPromoEvent("Cross Promo Ad Click", asset);
		// }
		//
		// internal static void OnCrossPromoError(string errMessage)
		// {
		// 	MixpanelWrapper.TrackCrossPromoErrorEvent("Cross Promo Ad Error", errMessage);
		// }
		//
		// internal static string[] GetAbTests()
		// {
		// 	return _runningAbTests;
		// }
	}
}
