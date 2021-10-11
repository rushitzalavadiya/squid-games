// using GameAnalyticsSDK;
// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using UnityEngine;
//
// namespace TinySauceSDK
// {
// 	public class TinySauce : MonoBehaviour
// 	{
// 		public const string PlayerPrefAbTestGroup = "ABCohort";
//
// 		public const string PlayerPrefFirstInstallDateTime = "FirstInstallDateTime";
//
// 		public const string PlayerPrefLastRetentionDayTracked = "LastRetentionDayTracked";
//
// 		private const int MaxRetentionDayToRecord = 30;
//
// 		private bool _firstTimeUser;
//
// 		[Header("AB Testing - see documentation for details")]
// 		[Tooltip("One of the game test groups you set up as a Custom Dimension 01 in GameAnalytics.")]
// 		public string ForceABTestGroup;
//
// 		private static bool _hasBeenInitialized;
//
// 		private void Awake()
// 		{
// 			_hasBeenInitialized = true;
// 			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
// 			GameAnalytics.Initialize();
// 			CheckForFirstTimeUser();
// 			CheckForAndRegisterAbTestGroup();
// 			CheckAndTrackRetention();
// 		}
//
// 		private void CheckForFirstTimeUser()
// 		{
// 			if (!PlayerPrefs.HasKey("FirstInstallDateTime"))
// 			{
// 				UnityEngine.Debug.Log("TinySauce: Game opened for the first time. Setting first open date and time.");
// 				_firstTimeUser = true;
// 				string value = DateTime.Now.ToString(CultureInfo.InvariantCulture);
// 				PlayerPrefs.SetString("FirstInstallDateTime", value);
// 			}
// 		}
//
// 		private void CheckForAndRegisterAbTestGroup()
// 		{
// 			List<string> customDimensions = GameAnalytics.SettingsGA.CustomDimensions01;
// 			if (customDimensions == null || customDimensions.Count == 0)
// 			{
// 				UnityEngine.Debug.Log("TinySauce: No A/B are set up to run.");
// 				PlayerPrefs.DeleteKey("ABCohort");
// 				return;
// 			}
// 			if (_firstTimeUser)
// 			{
// 				string text = customDimensions[UnityEngine.Random.Range(0, customDimensions.Count)];
// 				PlayerPrefs.SetString("ABCohort", text);
// 				GameAnalytics.SetCustomDimension01(text);
// 				UnityEngine.Debug.Log("TinySauce: Setting user to test group: " + text + ".");
// 				return;
// 			}
// 			string @string = PlayerPrefs.GetString("ABCohort");
// 			if (customDimensions.Contains(@string))
// 			{
// 				GameAnalytics.SetCustomDimension01(@string);
// 				UnityEngine.Debug.Log("TinySauce: User has previously been set to test group: " + @string + ".");
// 			}
// 			else
// 			{
// 				PlayerPrefs.DeleteKey("ABCohort");
// 			}
// 		}
//
// 		private void CheckAndTrackRetention()
// 		{
// 			int @int = PlayerPrefs.GetInt("LastRetentionDayTracked", -1);
// 			DateTime d = DateTime.Parse(PlayerPrefs.GetString("FirstInstallDateTime"), CultureInfo.InvariantCulture);
// 			int days = (DateTime.Now - d).Days;
// 			if (days > @int && days <= 30)
// 			{
// 				UnityEngine.Debug.Log($"TinySauce: Sending retention event for tracking: D{days}.");
// 				GameAnalytics.NewDesignEvent($"D{days}");
// 				PlayerPrefs.SetInt("LastRetentionDayTracked", days);
// 			}
// 		}
//
// 		public static string GetPlayerTestGroup()
// 		{
// 			if (_hasBeenInitialized)
// 			{
// 				return PlayerPrefs.GetString("ABCohort", string.Empty);
// 			}
// 			UnityEngine.Debug.LogError("TinySauce: You have called GetPlayerTestGroup before the SDK is initialized. This happens in the Awake method. Please consider calling GetPlayerTestGroup in Start or change the script execution order.");
// 			return string.Empty;
// 		}
// 	}
// }
