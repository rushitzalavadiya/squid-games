// using Facebook.Unity;
// using UnityEngine;
//
// public class FacebookScript : MonoBehaviour
// {
// 	private void Awake()
// 	{
// 		if (!FB.IsInitialized)
// 		{
// 			FB.Init(InitCallback, OnHideUnity);
// 		}
// 		else
// 		{
// 			FB.ActivateApp();
// 		}
// 	}
//
// 	private void InitCallback()
// 	{
// 		if (FB.IsInitialized)
// 		{
// 			FB.ActivateApp();
// 		}
// 		else
// 		{
// 			UnityEngine.Debug.Log("Failed to Initialize the Facebook SDK");
// 		}
// 	}
//
// 	private void OnHideUnity(bool isGameShown)
// 	{
// 		if (!isGameShown)
// 		{
// 			Time.timeScale = 0f;
// 		}
// 		else
// 		{
// 			Time.timeScale = 1f;
// 		}
// 	}
// }
