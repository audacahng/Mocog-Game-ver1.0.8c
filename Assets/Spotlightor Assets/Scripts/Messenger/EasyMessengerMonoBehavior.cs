using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EasyMessengerMonoBehavior : MonoBehaviour
{
	
	public struct MessengerHandlerInfo
	{
		public string eventType;
		public Callback handler;
	}
	
	private List<MessengerHandlerInfo> handlerInfos;
	
	private List<MessengerHandlerInfo> HandlerInfos {
		get {
			if (handlerInfos == null)
				handlerInfos = new List<MessengerHandlerInfo> ();
			return handlerInfos;
		}
	}
	
	protected void AddListener (Enum eventTypeEnum, Callback handler)
	{
		AddListener (eventTypeEnum.ToString (), handler);
	}
	
	protected void AddListener (string eventType, Callback handler)
	{
		Messenger.AddListener (eventType, handler);
		
		MessengerHandlerInfo info = new MessengerHandlerInfo ();//TODO: how about add same handler 2 times?
		info.eventType = eventType;
		info.handler = handler;
		HandlerInfos.Add (info);
	}
	
	protected void RemoveListener (Enum eventTypeEnum, Callback handler)
	{
		RemoveListener (eventTypeEnum.ToString (), handler);
	}
	
	protected void RemoveListener (string eventType, Callback handler)
	{
		Messenger.RemoveListener (eventType, handler);
		foreach (MessengerHandlerInfo info in HandlerInfos) {
			if (info.eventType == eventType && info.handler == handler) {
				HandlerInfos.Remove (info);
				return;//TODO: Maybe there are 2 listeners.
			}
		}
	}
	
	protected void AddSingleHandlerToMultiMessages (Callback handler, params Enum[] eventTypeEnums)
	{
		foreach (Enum eventTypeEnum in eventTypeEnums)
			AddListener (eventTypeEnum, handler);
	}
	
	protected void AddSingleHandlerToMultiMessages (Callback handler, params string[] eventTypes)
	{
		foreach (string eventType in eventTypes)
			AddListener (eventType, handler);
	}
	
	/// <summary>
	/// Listeners will be removed when OnDestroy
	/// </summary>
	protected virtual void OnDestroy ()
	{
		foreach (MessengerHandlerInfo handlerInfo in HandlerInfos) {
			Messenger.RemoveListener (handlerInfo.eventType, handlerInfo.handler);
		}
		handlerInfos.Clear ();
		handlerInfos = null;
	}
}
