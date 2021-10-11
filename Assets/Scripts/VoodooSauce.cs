
 // using Voodoo.Sauce.Internal;
 // using Voodoo.Sauce.Internal.Ads;
 using Voodoo.Sauce.Internal.Analytics;
 // using Voodoo.Sauce.Internal.AppRater;
 // using Voodoo.Sauce.Internal.CrossPromo;
 // using Voodoo.Sauce.Internal.CrossPromo.Models;
 // using Voodoo.Sauce.Internal.GDPR;
 // using Voodoo.Sauce.Internal.IAP;

 public static class VoodooSauce
 {
 	// public delegate void AppResumedEventHandler();
	 //
 	// public const string VERSION = "3.6.0";
	 //
 	// public static event AppResumedEventHandler SubscriptionsRefreshed;
	 //
 	// internal static void InvokeAppResumed()
 	// {
 	// 	if (VoodooSauce.SubscriptionsRefreshed != null)
 	// 	{
 	// 		VoodooSauce.SubscriptionsRefreshed();
 	// 	}
 	// }

 	public static string GetPlayerCohort()
 	{
 		return VoodooAnalytics.GetPlayerCohort();
 	}

 	// public static void OnGameStarted()
 	// {
 	// 	VoodooAnalytics.OnGameStarted();
 	// }
	 //
 	// public static void OnGameFinished(float score)
 	// {
 	// 	OnGameFinished(levelComplete: true, score, null);
 	// }
	 //
 	// public static void OnGameFinished(bool levelComplete, float score)
 	// {
 	// 	OnGameFinished(levelComplete, score, null);
 	// }
	 //
 	// public static void OnGameFinished(bool levelComplete, float score, Dictionary<string, object> eventProperties)
 	// {
 	// 	VoodooAnalytics.OnGameFinished(levelComplete, score, eventProperties);
 	// 	VoodooAds.OnGamePlayed();
 	// }
	 //
 	// public static void TrackCustomEvent(string eventName, Dictionary<string, object> eventProperties)
 	// {
 	// 	VoodooAnalytics.TrackCustomEvent(eventName, eventProperties);
 	// }
	 //
 	// public static void TrackCustomEvent(string eventName)
 	// {
 	// 	TrackCustomEvent(eventName, null);
 	// }
	 //
 	// public static void TrackTenjinCustomEvent(string eventName)
 	// {
 	// 	TenjinWrapper.TrackTenjinCustomEvent(eventName);
 	// }
	 //
 	// public static void RegisterPurchaseDelegate(IPurchaseDelegate purchaseDelegate)
 	// {
 	// 	VoodooIAP.SetPurchaseDelegate(purchaseDelegate);
 	// }
	 //
 	// public static void Purchase(string productId)
 	// {
 	// 	VoodooIAP.BuyProduct(productId);
 	// }
	 //
 	// public static void RestorePurchases()
 	// {
 	// 	VoodooIAP.RestorePurchases();
 	// }
	 //
 	// public static SubscriptionInfoContainer GetSubscriptionInfo(string productId)
 	// {
 	// 	return VoodooIAP.GetSubscriptionInfo(productId);
 	// }
	 //
 	// public static bool IsSubscribed(string productId)
 	// {
 	// 	return VoodooIAP.IsSubscribed(productId);
 	// }
	 //
 	// public static void ShowBanner(Action<float> onBannerDisplayed = null)
 	// {
 	// 	VoodooAds.ShowBanner(onBannerDisplayed);
 	// }
	 //
 	// public static void HideBanner()
 	// {
 	// 	VoodooAds.HideBanner();
 	// }
	 //
 	// public static void ShowInterstitial(Action onComplete = null, bool ignoreConditions = false)
 	// {
 	// 	VoodooAds.ShowInterstitial(onComplete, ignoreConditions);
 	// }
	 //
 	// public static bool IsRewardedVideoAvailable()
 	// {
 	// 	return VoodooAds.IsRewardedVideoAvailable();
 	// }
	 //
 	// public static bool IsInterstitialAvailable()
 	// {
 	// 	return VoodooAds.IsInterstitialAvailable();
 	// }
	 //
 	// public static void ShowRewardedVideo(Action<bool> onComplete)
 	// {
 	// 	VoodooAds.ShowRewardedVideo(onComplete);
 	// }
	 //
 	// public static void SetInterstitialAdsDisplayConditions(int delayInSecondsBeforeFirstInterstitialAd, int delayInSecondsBetweenInterstitialAds, int maxGamesBetweenInterstitialAds)
 	// {
 	// 	VoodooAds.SetInterstitialAdsDisplayConditions(delayInSecondsBeforeFirstInterstitialAd, delayInSecondsBetweenInterstitialAds, maxGamesBetweenInterstitialAds);
 	// }
	 //
 	// public static void SetInterstitialAtLaunchConditions(bool interstitialAtLaunch, int interstitialAtLaunchDelay, int interstitialAtLaunchTimeout)
 	// {
 	// 	VoodooAds.SetInterstitialAtLaunchConditions(interstitialAtLaunch, interstitialAtLaunchDelay, interstitialAtLaunchTimeout);
 	// }
	 //
 	// public static void SetAdUnit(AdUnitType adUnitType, string adUnit)
 	// {
 	// 	VoodooAds.SetAdUnit(adUnitType, adUnit);
 	// }
	 //
 	// public static void EnablePremium()
 	// {
 	// 	VoodooPremium.EnablePremium();
 	// }
	 //
 	// public static bool IsPremium()
 	// {
 	// 	return VoodooPremium.IsPremium();
 	// }
	 //
 	// public static void ShowCrossPromo(Action<AssetModel> onSuccess = null, Action onFailure = null)
 	// {
 	// 	VoodooCrossPromo.Show(onSuccess, onFailure);
 	// }
	 //
 	// public static void HideCrossPromo()
 	// {
 	// 	VoodooCrossPromo.Hide();
 	// }
	 //
 	// public static string GetLocalizedProductPrice(string productId)
 	// {
 	// 	return VoodooIAP.GetLocalizedProductPrice(productId);
 	// }
	 //
 	// public static void TryToShowAppRater()
 	// {
 	// 	AppRater.TryToShow();
 	// }
	 //
 	// public static void ShowGDPRSettings(Action onSettingsClosed = null)
 	// {
 	// 	VoodooGDPR.ShowOptInSettings(onSettingsClosed);
 	// }
	 //
 	// public static void RequestGdprApplicability(Action<bool> callback)
 	// {
 	// 	VoodooGDPR.RequestGdprApplicability(callback);
 	// }
	 //
 	// public static void ShowGDPRBanner()
 	// {
 	// 	VoodooGDPR.ShowGDPRBanner(force: false);
 	// }
	 //
 	// public static string[] GetAbTests()
 	// {
 	// 	return VoodooAnalytics.GetAbTests();
 	// }
	 //
 	// public static void SetPlayerCohort(string cohortName)
 	// {
 	// 	VoodooAnalytics.SetPlayerCohort(cohortName);
 	// }
	 //
 	// public static void ShowCohortDebugMenu()
 	// {
 	// 	VoodooSauceBehaviour.ShowCohortDebugMenu();
 	// }
}
