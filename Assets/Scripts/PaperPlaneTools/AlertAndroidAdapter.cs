using System;
using UnityEngine;

namespace PaperPlaneTools
{
	public class AlertAndroidAdapter : IAlertPlatformAdapter
	{
		private AlertAndroid alertAndroid;

		private Action onDismiss;

		private GameObject gameObject;

		public AlertAndroidAdapter()
		{
			gameObject = (UnityEngine.Object.Instantiate(Resources.Load("PaperPlaneTools/Alert/AlertCallbackHandler")) as GameObject);
		}

		void IAlertPlatformAdapter.SetOnDismiss(Action action)
		{
			onDismiss = action;
		}

		void IAlertPlatformAdapter.Show(Alert alert)
		{
			if (alert.OnDismiss != null)
			{
				onDismiss = (Action)Delegate.Combine(onDismiss, alert.OnDismiss);
			}
			AlertAndroidOptions alertAndroidOptions = null;
			foreach (object option in alert.Options)
			{
				if (option is AlertAndroidOptions)
				{
					alertAndroidOptions = (AlertAndroidOptions)option;
					break;
				}
			}
			if (alertAndroidOptions == null)
			{
				alertAndroidOptions = new AlertAndroidOptions();
			}
			alertAndroid = new AlertAndroid();
			alertAndroid.Title = alert.Title;
			alertAndroid.Message = alert.Message;
			if (alert.PositiveButton != null)
			{
				alertAndroid.SetPositiveButton(alert.PositiveButton.Title, alert.PositiveButton.Handler);
			}
			if (alert.NegativeButton != null)
			{
				alertAndroid.SetNegativeButton(alert.NegativeButton.Title, alert.NegativeButton.Handler);
			}
			if (alert.NeutralButton != null)
			{
				alertAndroid.SetNeutralButton(alert.NeutralButton.Title, alert.NeutralButton.Handler);
			}
			alertAndroid.OnDismiss = onDismissCallback;
			alertAndroid.Cancelable = alertAndroidOptions.Cancelable;
			alertAndroid.Show(gameObject.transform.name);
		}

		void IAlertPlatformAdapter.Dismiss()
		{
			if (alertAndroid != null)
			{
				alertAndroid.Dismiss();
			}
		}

		void IAlertPlatformAdapter.HandleEvent(string eventName, string value)
		{
			if (alertAndroid != null)
			{
				if (eventName == "AlertAndroid_OnButtonClick")
				{
					alertAndroid.HandleButtonClick(int.Parse(value));
				}
				if (eventName == "AlertAndroid_OnCancel")
				{
					alertAndroid.HandleCancel();
				}
				if (eventName == "AlertAndroid_OnDismiss")
				{
					alertAndroid.HandleDismiss();
				}
			}
		}

		private void onDismissCallback()
		{
			if (onDismiss != null)
			{
				onDismiss();
			}
			UnityEngine.Object.Destroy(gameObject);
		}
	}
}
