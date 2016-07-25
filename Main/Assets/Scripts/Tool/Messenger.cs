using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Messenger for invoke.
/// </summary>
public static class Messenger
{
	public static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
	public static List<string> permanentMessages = new List<string>();
	
	public static void AddListener(string eventType, Callback handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback) Delegate.Combine((Callback) eventTable[eventType], handler);
	}
	
	public static void AddListener<T>(string eventType, Callback<T> handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback<T>) Delegate.Combine((Callback<T>) eventTable[eventType], handler);
	}
	
	public static void AddListener<T, U>(string eventType, Callback<T, U> handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback<T, U>) Delegate.Combine((Callback<T, U>) eventTable[eventType], handler);
	}
	
	public static void AddListener<T, U, V>(string eventType, Callback<T, U, V> handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback<T, U, V>) Delegate.Combine((Callback<T, U, V>) eventTable[eventType], handler);
	}
	public static void AddReturnListener(string eventType, CallbackReturn handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (CallbackReturn)Delegate.Combine((CallbackReturn)eventTable[eventType], handler);
	}
	
	public static void AddReturnListener<T>(string eventType, CallbackReturn<T> handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (CallbackReturn<T>)Delegate.Combine((CallbackReturn<T>)eventTable[eventType], handler);
	}
	
	public static void AddReturnListener<T, U>(string eventType, CallbackReturn<T, U> handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (CallbackReturn<T, U>)Delegate.Combine((CallbackReturn<T, U>)eventTable[eventType], handler);
	}
	
	public static void AddReturnListener<T, U, V>(string eventType, CallbackReturn<T, U, V> handler)
	{
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (CallbackReturn<T, U, V>)Delegate.Combine((CallbackReturn<T, U, V>)eventTable[eventType], handler);
	}
	public static void Broadcast(string eventType)
	{
		Delegate delegate2;
		OnBroadcasting(eventType);
		if (eventTable.TryGetValue(eventType, out delegate2))
		{
			Callback callback = delegate2 as Callback;
			if (callback == null)
			{
				throw CreateBroadcastSignatureException(eventType);
			}
			callback();
		}
		else
			Debug.LogWarning("no handler for:" + eventType);
	}
	
	public static void Broadcast<T>(string eventType, T arg1)
	{
		Delegate delegate2;
		OnBroadcasting(eventType);
		if (eventTable.TryGetValue(eventType, out delegate2))
		{
			//Debug.Log("broadcast:"+ delegate2.GetType().ToString());
			
			Callback<T> callback = delegate2 as Callback<T>;
			if (callback == null)
			{
				throw CreateBroadcastSignatureException(eventType);
			}
			callback(arg1);
		}
		else
			Debug.LogWarning("no handler for:" + eventType);
	}
	
	public static void Broadcast<T, U>(string eventType, T arg1, U arg2)
	{
		Delegate delegate2;
		OnBroadcasting(eventType);
		if (eventTable.TryGetValue(eventType, out delegate2))
		{
			Callback<T, U> callback = delegate2 as Callback<T, U>;
			if (callback == null)
			{
				throw CreateBroadcastSignatureException(eventType);
			}
			callback(arg1, arg2);
		}
		else
			Debug.LogWarning("no handler for:" + eventType);
	}
	
	public static void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
	{
		Delegate delegate2;
		OnBroadcasting(eventType);
		if (eventTable.TryGetValue(eventType, out delegate2))
		{
			Callback<T, U, V> callback = delegate2 as Callback<T, U, V>;
			if (callback == null)
			{
				throw CreateBroadcastSignatureException(eventType);
			}
			callback(arg1, arg2, arg3);
		}
		else
			Debug.LogWarning("no handler for:" + eventType);
	}
	
	
	public static object RequireValue(string eventType)
	{
		Delegate delegate2;
		OnBroadcasting(eventType);
		if (eventTable.TryGetValue(eventType, out delegate2))
		{
			CallbackReturn callback = delegate2 as CallbackReturn;
			if (callback == null)
			{
				throw CreateBroadcastSignatureException(eventType);
			}
			return callback();
		}
		else
			Debug.LogWarning("no handler for:" + eventType);
		return null;
	}
	
	public static object RequireValue<T>(string eventType, T arg1)
	{
		Delegate delegate2;
		OnBroadcasting(eventType);
		if (eventTable.TryGetValue(eventType, out delegate2))
		{
			//Debug.Log("broadcast:"+ delegate2.GetType().ToString());
			
			CallbackReturn<T> callback = delegate2 as CallbackReturn<T>;
			if (callback == null)
			{
				throw CreateBroadcastSignatureException(eventType);
			}
			return callback(arg1);
		}
		else
			Debug.LogWarning("no handler for:" + eventType);
		return null;
	}
	
	public static object RequireValue<T, U>(string eventType, T arg1, U arg2)
	{
		Delegate delegate2;
		OnBroadcasting(eventType);
		if (eventTable.TryGetValue(eventType, out delegate2))
		{
			CallbackReturn<T, U> callback = delegate2 as CallbackReturn<T, U>;
			if (callback == null)
			{
				throw CreateBroadcastSignatureException(eventType);
			}
			return callback(arg1, arg2);
		}
		else
			Debug.LogWarning("no handler for:" + eventType);
		return null;
	}
	
	public static object RequireValue<T, U, V>(string eventType, T arg1, U arg2, V arg3)
	{
		Delegate delegate2;
		OnBroadcasting(eventType);
		if (eventTable.TryGetValue(eventType, out delegate2))
		{
			CallbackReturn<T, U, V> callback = delegate2 as CallbackReturn<T, U, V>;
			if (callback == null)
			{
				throw CreateBroadcastSignatureException(eventType);
			}
			return callback(arg1, arg2, arg3);
		}
		else
			Debug.LogWarning("no handler for:" + eventType);
		return null;
	}
	
	
	public static void Cleanup()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, Delegate> pair in eventTable)
		{
			bool flag = false;
			foreach (string str in permanentMessages)
			{
				if (pair.Key == str)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(pair.Key);
			}
		}
		foreach (string str2 in list)
		{
			eventTable.Remove(str2);
		}
	}
	
	public static BroadcastException CreateBroadcastSignatureException(string eventType)
	{
		return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
	}
	
	public static void MarkAsPermanent(string eventType)
	{
		permanentMessages.Add(eventType);
	}
	
	public static void OnBroadcasting(string eventType)
	{
	}
	
	public static void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
	{
		if (!eventTable.ContainsKey(eventType))
		{
			eventTable.Add(eventType, null);
		}
		Delegate delegate2 = eventTable[eventType];
		if ((delegate2 != null) && (delegate2.GetType() != listenerBeingAdded.GetType()))
		{
			throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, delegate2.GetType().Name, listenerBeingAdded.GetType().Name));
		}
	}
	
	public static void OnListenerRemoved(string eventType)
	{
		if (eventTable.ContainsKey(eventType) && (eventTable[eventType] == null))
		{
			eventTable.Remove(eventType);
		}
	}
	
	public static void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
	{
		if (eventTable.ContainsKey(eventType))
		{
			Delegate delegate2 = eventTable[eventType];
			if (delegate2 == null)
			{
				throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
			}
			if (delegate2.GetType() != listenerBeingRemoved.GetType())
			{
				throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, delegate2.GetType().Name, listenerBeingRemoved.GetType().Name));
			}
		}
	}
	
	public static void PrintEventTable()
	{
		Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");
		foreach (KeyValuePair<string, Delegate> pair in eventTable)
		{
			Debug.Log(string.Concat(new object[] { "\t\t\t", pair.Key, "\t\t", pair.Value }));
		}
		Debug.Log("\n");
	}
	
	public static void RemoveListener(string eventType, Callback handler)
	{
		OnListenerRemoving(eventType, handler);
		if (eventTable.ContainsKey(eventType))
		{
			eventTable[eventType] = (Callback) Delegate.Remove((Callback) eventTable[eventType], handler);
		}
		OnListenerRemoved(eventType);
	}
	
	public static void RemoveListener<T>(string eventType, Callback<T> handler)
	{
		OnListenerRemoving(eventType, handler);
		if (eventTable.ContainsKey(eventType))
		{
			eventTable[eventType] = (Callback<T>) Delegate.Remove((Callback<T>) eventTable[eventType], handler);
		}
		OnListenerRemoved(eventType);
	}
	
	public static void RemoveListener<T, U>(string eventType, Callback<T, U> handler)
	{
		OnListenerRemoving(eventType, handler);
		if (eventTable.ContainsKey(eventType))
		{
			eventTable[eventType] = (Callback<T, U>) Delegate.Remove((Callback<T, U>) eventTable[eventType], handler);
		}
		OnListenerRemoved(eventType);
	}
	
	public static void RemoveListener<T, U, V>(string eventType, Callback<T, U, V> handler)
	{
		OnListenerRemoving(eventType, handler);
		if (eventTable.ContainsKey(eventType))
		{
			eventTable[eventType] = (Callback<T, U, V>) Delegate.Remove((Callback<T, U, V>) eventTable[eventType], handler);
		}
		OnListenerRemoved(eventType);
	}
	
	public class BroadcastException : Exception
	{
		public BroadcastException(string msg) : base(msg)
		{
		}
	}
	
	public class ListenerException : Exception
	{
		public ListenerException(string msg) : base(msg)
		{
		}
	}
}

