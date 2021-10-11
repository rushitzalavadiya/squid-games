// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class MoPubAndroidRewardedVideo
// {
// 	private readonly AndroidJavaObject _plugin;
//
// 	private readonly Dictionary<MoPubBase.Reward, AndroidJavaObject> _rewardsDict = new Dictionary<MoPubBase.Reward, AndroidJavaObject>();
//
// 	public MoPubAndroidRewardedVideo(string adUnitId)
// 	{
// 		_plugin = new AndroidJavaObject("com.mopub.unity.MoPubRewardedVideoUnityPlugin", adUnitId);
// 	}
//
// 	public void RequestRewardedVideo(List<MoPubBase.LocalMediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
// 	{
// 		string text = MoPubBase.LocalMediationSetting.ToJson(mediationSettings);
// 		_plugin.Call("requestRewardedVideo", text, keywords, userDataKeywords, latitude, longitude, customerId);
// 	}
//
// 	public void ShowRewardedVideo(string customData)
// 	{
// 		_plugin.Call("showRewardedVideo", customData);
// 	}
//
// 	public bool HasRewardedVideo()
// 	{
// 		return _plugin.Call<bool>("hasRewardedVideo", Array.Empty<object>());
// 	}
//
// 	public List<MoPubBase.Reward> GetAvailableRewards()
// 	{
// 		_rewardsDict.Clear();
// 		using (AndroidJavaObject androidJavaObject = _plugin.Call<AndroidJavaObject>("getAvailableRewards", Array.Empty<object>()))
// 		{
// 			AndroidJavaObject[] array = AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(androidJavaObject.GetRawObject());
// 			if (array.Length <= 1)
// 			{
// 				return new List<MoPubBase.Reward>(_rewardsDict.Keys);
// 			}
// 			AndroidJavaObject[] array2 = array;
// 			foreach (AndroidJavaObject androidJavaObject2 in array2)
// 			{
// 				_rewardsDict.Add(new MoPubBase.Reward
// 				{
// 					Label = androidJavaObject2.Call<string>("getLabel", Array.Empty<object>()),
// 					Amount = androidJavaObject2.Call<int>("getAmount", Array.Empty<object>())
// 				}, androidJavaObject2);
// 			}
// 		}
// 		return new List<MoPubBase.Reward>(_rewardsDict.Keys);
// 	}
//
// 	public void SelectReward(MoPubBase.Reward selectedReward)
// 	{
// 		if (_rewardsDict.TryGetValue(selectedReward, out AndroidJavaObject value))
// 		{
// 			_plugin.Call("selectReward", value);
// 		}
// 		else
// 		{
// 			UnityEngine.Debug.LogWarning($"Selected reward {selectedReward} is not available.");
// 		}
// 	}
// }
