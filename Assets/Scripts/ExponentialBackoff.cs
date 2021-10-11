using System;
using System.Collections;
using UnityEngine;
// using Voodoo.Sauce.Internal;

public class ExponentialBackoff : MonoBehaviour
{
	public class ExpBackoff
	{
		public float[] Delays;

		public Action Callback;

		public string Name;

		private Coroutine _coroutine;

		private int _currentDelayIndex;

		public ExpBackoff()
		{
			_currentDelayIndex = 0;
		}

		public void Start()
		{
			if (_coroutine == null)
			{
				float num = Delays[_currentDelayIndex];
				if (_currentDelayIndex < Delays.Length - 1)
				{
					_currentDelayIndex++;
				}
				if (num > 0f)
				{
					_coroutine = _instance.StartCoroutine(_instance.BackoffCoroutine(num, delegate
					{
						Stop();
						Callback();
					}, Name));
				}
				else
				{
					Callback();
				}
			}
		}

		public void Reset()
		{
			_currentDelayIndex = 0;
			Stop();
		}

		public void Stop()
		{
			if (_coroutine != null)
			{
				_instance.StopCoroutine(_coroutine);
				_coroutine = null;
			}
		}
	}

	private const string TAG = "ExponentialBackoff";

	private static ExponentialBackoff _instance;

	private void Awake()
	{
		_instance = this;
	}

	public static ExpBackoff CreateExpBackoff(Action callback, string name)
	{
		ExpBackoff expBackoff = new ExpBackoff();
		expBackoff.Delays = new float[9]
		{
			0.1f,
			1f,
			2f,
			4f,
			8f,
			15f,
			30f,
			60f,
			90f
		};
		expBackoff.Callback = callback;
		expBackoff.Name = name;
		return expBackoff;
	}

	private IEnumerator BackoffCoroutine(float delay, Action callback, string name)
	{
		//VoodooLog.Log("ExponentialBackoff", $"{name} will wait: {delay} seconds before callback");
		yield return new WaitForSecondsRealtime(delay);
		//VoodooLog.Log("ExponentialBackoff", $"{name} calling callback...");
		callback();
	}
}
