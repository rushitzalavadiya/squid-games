using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaperPlaneTools
{
	public class AlertAndroid
	{
		public enum ButtonType
		{
			POSITIVE = -1,
			NEGATIVE = -2,
			NEUTRAL = -3
		}

		private Dictionary<int, AlertButton> buttons = new Dictionary<int, AlertButton>();

		public Action OnDismiss;

		public string Title
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public bool Cancelable
		{
			get;
			set;
		}

		public AlertAndroid(string title = null, string message = null)
		{
			Title = title;
			Message = message;
			Cancelable = true;
		}

		public void SetPositiveButton(string title, Action handler)
		{
			SetButton(ButtonType.POSITIVE, title, handler);
		}

		public void SetNegativeButton(string title, Action handler)
		{
			SetButton(ButtonType.NEGATIVE, title, handler);
		}

		public void SetNeutralButton(string title, Action handler)
		{
			SetButton(ButtonType.NEUTRAL, title, handler);
		}

		public void SetButton(ButtonType whichButton, string title, Action handler)
		{
			buttons[(int)whichButton] = new AlertButton(title, handler);
		}

		public void Show(string gameObjectName)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.paperplanetools.Alert");
			androidJavaClass.CallStatic<int>("initBuilder", new object[1]
			{
				gameObjectName
			});
			if (Title != null)
			{
				androidJavaClass.CallStatic<int>("setTitle", new object[1]
				{
					Title
				});
			}
			if (Message != null)
			{
				androidJavaClass.CallStatic<int>("setMessage", new object[1]
				{
					Message
				});
			}
			foreach (KeyValuePair<int, AlertButton> button in buttons)
			{
				androidJavaClass.CallStatic<int>("setButton", new object[2]
				{
					button.Key,
					button.Value.Title
				});
			}
			androidJavaClass.CallStatic<int>("setCancelable", new object[1]
			{
				Cancelable
			});
			androidJavaClass.CallStatic<int>("show", Array.Empty<object>());
		}

		public void Dismiss()
		{
			new AndroidJavaClass("com.paperplanetools.Alert").CallStatic<int>("dismiss", Array.Empty<object>());
		}

		public void HandleButtonClick(int whichButton)
		{
			if (buttons.TryGetValue(whichButton, out AlertButton value) && value.Handler != null)
			{
				value.Handler();
			}
		}

		public void HandleCancel()
		{
		}

		public void HandleDismiss()
		{
			if (OnDismiss != null)
			{
				OnDismiss();
			}
		}
	}
}
