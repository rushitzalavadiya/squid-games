// using System;
// using System.Collections.Generic;
// using System.IO;
// using UnityEngine;
// using UnityEngine.Purchasing;
// //using Voodoo.Sauce.Internal;
// //using Voodoo.Sauce.Internal.IAP.Voodoo.Sauce.Internal.Misc;
//
// [Serializable]
// public class SubscriptionInfoContainer : ISerializationCallbackReceiver
// {
// 	private const string TAG = "SubscriptionInfoContainer";
//
// 	[SerializeField]
// 	private long _lPurchaseDate;
//
// 	[SerializeField]
// 	private long _lIntroductoryPricePeriod;
//
// 	[SerializeField]
// 	private long _lExpireDate;
//
// 	[SerializeField]
// 	private long _lCancelDate;
//
// 	[SerializeField]
// 	private long _lFreeTrialPeriod;
//
// 	[SerializeField]
// 	private long _lSubscriptionPeriod;
//
// 	public string productId;
//
// 	[NonSerialized]
// 	public DateTime purchaseDate;
//
// 	public Result isSubscribed;
//
// 	public Result isExpired;
//
// 	public Result isCancelled;
//
// 	public Result isFreeTrial;
//
// 	public Result isAutoRenewing;
//
// 	public Result isIntroductoryPricePeriod;
//
// 	[NonSerialized]
// 	public TimeSpan introductoryPricePeriod;
//
// 	public string introductoryPrice;
//
// 	public long introductoryPricePeriodCycles;
//
// 	[NonSerialized]
// 	public DateTime expireDate;
//
// 	[NonSerialized]
// 	public DateTime cancelDate;
//
// 	[NonSerialized]
// 	public TimeSpan freeTrialPeriod;
//
// 	[NonSerialized]
// 	public TimeSpan subscriptionPeriod;
//
// 	public string freeTrialPeriodString;
//
// 	public string skuDetails;
//
// 	public string subscriptionInfoJson;
//
// 	public SubscriptionInfoContainer()
// 	{
// 	}
//
// 	public SubscriptionInfoContainer(SubscriptionInfo data)
// 	{
// 		productId = data.getProductId();
// 		purchaseDate = data.getPurchaseDate().ToUniversalTime();
// 		isSubscribed = data.isSubscribed();
// 		isExpired = data.isExpired();
// 		isCancelled = data.isCancelled();
// 		isFreeTrial = data.isFreeTrial();
// 		isAutoRenewing = data.isAutoRenewing();
// 		isIntroductoryPricePeriod = data.isIntroductoryPricePeriod();
// 		introductoryPricePeriod = data.getIntroductoryPricePeriod();
// 		introductoryPrice = data.getIntroductoryPrice();
// 		introductoryPricePeriodCycles = data.getIntroductoryPricePeriodCycles();
// 		expireDate = data.getExpireDate().ToUniversalTime();
// 		cancelDate = data.getCancelDate().ToUniversalTime();
// 		freeTrialPeriod = data.getFreeTrialPeriod();
// 		subscriptionPeriod = data.getSubscriptionPeriod();
// 		freeTrialPeriodString = data.getFreeTrialPeriodString();
// 		skuDetails = data.getSkuDetails();
// 		subscriptionInfoJson = data.getSubscriptionInfoJsonString();
// 	}
//
// 	public void OnBeforeSerialize()
// 	{
// 		_lPurchaseDate = purchaseDate.Ticks;
// 		_lIntroductoryPricePeriod = introductoryPricePeriod.Ticks;
// 		_lExpireDate = expireDate.Ticks;
// 		_lCancelDate = cancelDate.Ticks;
// 		_lFreeTrialPeriod = freeTrialPeriod.Ticks;
// 		_lSubscriptionPeriod = subscriptionPeriod.Ticks;
// 	}
//
// 	public void OnAfterDeserialize()
// 	{
// 		purchaseDate = new DateTime(_lPurchaseDate);
// 		introductoryPricePeriod = new TimeSpan(_lIntroductoryPricePeriod);
// 		expireDate = new DateTime(_lExpireDate);
// 		cancelDate = new DateTime(_lCancelDate);
// 		freeTrialPeriod = new TimeSpan(_lFreeTrialPeriod);
// 		subscriptionPeriod = new TimeSpan(_lSubscriptionPeriod);
// 	}
//
// 	// public static void ClearSavedSubscriptionInfo()
// 	// {
// 	// 	if (Directory.Exists(VoodooConfig.SUBSCRIPTION_FILE_PATH))
// 	// 	{
// 	// 		Directory.Delete(VoodooConfig.SUBSCRIPTION_FILE_PATH, recursive: true);
// 	// 	}
// 	// }
//
// 	// public static void SaveSubscriptionInfo(SubscriptionInfoContainer data)
// 	// {
// 	// 	string toEncrypt = JsonUtility.ToJson(data);
// 	// 	toEncrypt = Encryptor.EncryptData(toEncrypt);
// 	// 	try
// 	// 	{
// 	// 		Directory.CreateDirectory(VoodooConfig.SUBSCRIPTION_FILE_PATH);
// 	// 		using (StreamWriter streamWriter = File.CreateText(VoodooConfig.SUBSCRIPTION_FILE_PATH + data.productId + ".JSON"))
// 	// 		{
// 	// 			streamWriter.Write(toEncrypt);
// 	// 			streamWriter.Close();
// 	// 		}
// 	// 	}
// 	// 	catch (Exception ex)
// 	// 	{
// 	// 		VoodooLog.LogE("SubscriptionInfoContainer", "Exception in saving subscription information");
// 	// 		VoodooLog.LogE("SubscriptionInfoContainer", ex.Message);
// 	// 	}
// 	// }
//
// 	// public static Dictionary<string, SubscriptionInfoContainer> LoadSubscriptionInfo()
// 	// {
// 	// 	Dictionary<string, SubscriptionInfoContainer> dictionary = new Dictionary<string, SubscriptionInfoContainer>();
// 	// 	if (Directory.Exists(VoodooConfig.SUBSCRIPTION_FILE_PATH))
// 	// 	{
// 	// 		FileInfo[] files = new DirectoryInfo(VoodooConfig.SUBSCRIPTION_FILE_PATH).GetFiles("*.JSON");
// 	// 		foreach (FileInfo fileInfo in files)
// 	// 		{
// 	// 			using (StreamReader streamReader = new StreamReader(VoodooConfig.SUBSCRIPTION_FILE_PATH + fileInfo.Name))
// 	// 			{
// 	// 				string toDecrypt = streamReader.ReadToEnd();
// 	// 				toDecrypt = Encryptor.DecryptData(toDecrypt);
// 	// 				dictionary[Path.GetFileNameWithoutExtension(fileInfo.Name)] = JsonUtility.FromJson<SubscriptionInfoContainer>(toDecrypt);
// 	// 			}
// 	// 		}
// 	// 	}
// 	// 	return dictionary;
// 	// }
// }
