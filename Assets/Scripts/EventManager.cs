using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public delegate void EventDelegate<T>(T e) where T : GameEvent;

	private delegate void EventDelegate(GameEvent e);

	public bool LimitQueueProcesing;

	public float QueueProcessTime;

	private Queue m_eventQueue = new Queue();

	private Dictionary<Type, EventDelegate> delegates = new Dictionary<Type, EventDelegate>();

	private Dictionary<Delegate, EventDelegate> delegateLookup = new Dictionary<Delegate, EventDelegate>();

	private Dictionary<Delegate, Delegate> onceLookups = new Dictionary<Delegate, Delegate>();

	private static EventManager s_Instance;

	public static EventManager Instance
	{
		get
		{
			if (s_Instance == null)
			{
				s_Instance = (UnityEngine.Object.FindObjectOfType(typeof(EventManager)) as EventManager);
				if (s_Instance == null)
				{
					UnityEngine.Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
				}
			}
			return s_Instance;
		}
	}

	private EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent
	{
		if (delegateLookup.ContainsKey(del))
		{
			return null;
		}
		EventDelegate eventDelegate = delegate(GameEvent e)
		{
			del((T)e);
		};
		delegateLookup[del] = eventDelegate;
		if (delegates.TryGetValue(typeof(T), out EventDelegate value))
		{
			value = (delegates[typeof(T)] = (EventDelegate)Delegate.Combine(value, eventDelegate));
		}
		else
		{
			delegates[typeof(T)] = eventDelegate;
		}
		return eventDelegate;
	}

	public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
	{
		AddDelegate(del);
	}

	public void AddListenerOnce<T>(EventDelegate<T> del) where T : GameEvent
	{
		EventDelegate eventDelegate = AddDelegate(del);
		if (eventDelegate != null)
		{
			onceLookups[eventDelegate] = del;
		}
	}

	public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
	{
		if (!delegateLookup.TryGetValue(del, out EventDelegate value))
		{
			return;
		}
		if (delegates.TryGetValue(typeof(T), out EventDelegate value2))
		{
			value2 = (EventDelegate)Delegate.Remove(value2, value);
			if (value2 == null)
			{
				delegates.Remove(typeof(T));
			}
			else
			{
				delegates[typeof(T)] = value2;
			}
		}
		delegateLookup.Remove(del);
	}

	public void RemoveAll()
	{
		delegates.Clear();
		delegateLookup.Clear();
		onceLookups.Clear();
	}

	public bool HasListener<T>(EventDelegate<T> del) where T : GameEvent
	{
		return delegateLookup.ContainsKey(del);
	}

	public void TriggerEvent(GameEvent e)
	{
		if (delegates.TryGetValue(e.GetType(), out EventDelegate value))
		{
			value(e);
			Delegate[] invocationList = delegates[e.GetType()].GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				EventDelegate eventDelegate = (EventDelegate)invocationList[i];
				if (onceLookups.ContainsKey(eventDelegate))
				{
					Dictionary<Type, EventDelegate> dictionary = delegates;
					Type type = e.GetType();
					dictionary[type] = (EventDelegate)Delegate.Remove(dictionary[type], eventDelegate);
					if (delegates[e.GetType()] == null)
					{
						delegates.Remove(e.GetType());
					}
					delegateLookup.Remove(onceLookups[eventDelegate]);
					onceLookups.Remove(eventDelegate);
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("Event: " + e.GetType() + " has no listeners");
		}
	}

	public bool QueueEvent(GameEvent evt)
	{
		if (!delegates.ContainsKey(evt.GetType()))
		{
			UnityEngine.Debug.Log("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetType());
			return false;
		}
		m_eventQueue.Enqueue(evt);
		return true;
	}

	private void Update()
	{
		float num = 0f;
		while (m_eventQueue.Count > 0 && (!LimitQueueProcesing || !(num > QueueProcessTime)))
		{
			GameEvent e = m_eventQueue.Dequeue() as GameEvent;
			TriggerEvent(e);
			if (LimitQueueProcesing)
			{
				num += Time.deltaTime;
			}
		}
	}

	public void OnApplicationQuit()
	{
		RemoveAll();
		m_eventQueue.Clear();
		s_Instance = null;
	}
}
