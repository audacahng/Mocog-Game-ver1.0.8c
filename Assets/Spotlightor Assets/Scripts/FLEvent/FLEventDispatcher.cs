using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FLEventDispatcher : IFLEventDispatcher
{
	private Dictionary<string, List<FLEventBase.FLEventHandler>> _handlers;

	/// <summary>
	/// 用来处理在DisptachEvent的过程中，添加或者删除Listener
	/// </summary>
	private string _dispatchingEvent = null;
	/// <summary>
	/// 需要在DisptachEvent结束后删除的Hanlder
	/// </summary>
	private List<FLEventBase.FLEventHandler> _delayedHandlersToRemove;
	/// <summary>
	/// 需要在DisptachEvent结束后添加的Handler
	/// </summary>
	private List<FLEventBase.FLEventHandler> _delayedHandlersToAdd;

	public void DispatchEvent (FLEventBase e)
	{
		if (e.target == null)
			e.target = this;
		if (_handlers != null && _handlers.ContainsKey (e.Type)) {
			List<FLEventBase.FLEventHandler> eventHandlers = _handlers [e.Type];
			int i = 0;
			FLEventBase.FLEventHandler lastHandler = null;
			FLEventBase.FLEventHandler currentHandler = null;
			while (i < eventHandlers.Count) {
				currentHandler = eventHandlers [i];
				if (currentHandler != lastHandler)
					currentHandler (e);
				else
					i++;
				lastHandler = currentHandler;
			}
		}
	}

	public void AddEventListener (string eventType, FLEventBase.FLEventHandler handler)
	{
		if (_dispatchingEvent == eventType) {
			if (_delayedHandlersToAdd == null)
				_delayedHandlersToAdd = new List<FLEventBase.FLEventHandler> ();
			_delayedHandlersToAdd.Add (handler);
		} else {
			if (_handlers == null)
				_handlers = new Dictionary<string, List<FLEventBase.FLEventHandler>> ();

			List<FLEventBase.FLEventHandler> eventHandlers;
			if (!_handlers.ContainsKey (eventType))
				_handlers [eventType] = eventHandlers = new List<FLEventBase.FLEventHandler> ();
			else
				eventHandlers = _handlers [eventType];

			if (eventHandlers.IndexOf (handler) == -1)
				eventHandlers.Add (handler);
		}
	}

	public void RemoveEventListener (string eventType, FLEventBase.FLEventHandler handler)
	{
		if (_dispatchingEvent == eventType) {
			if (_delayedHandlersToRemove == null)
				_delayedHandlersToRemove = new List<FLEventBase.FLEventHandler> ();
			_delayedHandlersToRemove.Add (handler);
		} else {
			if (_handlers != null && _handlers.ContainsKey (eventType)) {
				List<FLEventBase.FLEventHandler> eventHandlers = _handlers [eventType];
				eventHandlers.Remove (handler);
			}
		}
	}

	private void AddRemoveDelayedHandlers (string eventType)
	{
		if (_delayedHandlersToRemove != null) {
			foreach (FLEventBase.FLEventHandler handler in _delayedHandlersToRemove)
				RemoveEventListener (eventType, handler);
		}
		if (_delayedHandlersToAdd != null) {
			foreach (FLEventBase.FLEventHandler handler in _delayedHandlersToAdd)
				AddEventListener (eventType, handler);
		}
		_delayedHandlersToRemove = null;
		_delayedHandlersToAdd = null;
	}
}
