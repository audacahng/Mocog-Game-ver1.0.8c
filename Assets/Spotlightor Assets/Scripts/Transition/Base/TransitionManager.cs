using UnityEngine;
using System.Collections;

public enum TransitionState
{
	UNKNOWN,
	IN,
	TRANSITION_IN,
	OUT,
	TRANSITION_OUT
}

/// <summary>
/// 帮助你又快又好地建立Transition特效Component的Base Class
/// </summary>
public class TransitionManager : FLEventDispatcherMono, ITransition
{
    #region Constants
	public const string MSG_IN_START = "OnTransitionIn";
	public const string MSG_IN_COMPLETE = "OnTransitionInComplete";
	public const string MSG_OUT_START = "OnTransitionOut";
	public const string MSG_OUT_COMPLETE = "OnTransitionOutComplete";

	/// <summary>
	/// 是否在Out Complete的时候自动active=false？是否在In的时候自动active = true？
	/// </summary>
	public bool outWhenAwake = false;
	public bool autoActivate = false;
	/// <summary>
	/// 如果autoActivate，那么children也一起吗？
	/// </summary>
	public bool activateChildren = false;
	public float delayIn = 0;
	public float durationIn = 1.5f;
	public float delayOut = 0;
	public float durationOut = 1.5f;
	
    #endregion

    #region Fields
	protected TransitionState _state = TransitionState.UNKNOWN;
    #endregion

    #region Properties

	public bool inTransition {
		get { return _state == TransitionState.TRANSITION_IN || _state == TransitionState.TRANSITION_OUT; }
	}
	
	public bool IsOutOrUnkown {
		get { return _state == TransitionState.OUT || _state == TransitionState.UNKNOWN;}
	}

	public TransitionState state { 
		get { return _state; } 
		protected set { 
			_state = value;
		}
	}
	
	public float TotalTransitionInTime { get { return delayIn + durationIn; } }

	public float TotalTransitionOutTime { get { return delayOut + durationOut; } }
	
    #endregion

    #region Functions

	/// <summary>
	/// Override me please
	/// </summary>
	/// <param name="instant"></param>
	protected virtual void DoTransitionIn (bool instant)
	{
		TransitionInComplete ();
	}
	/// <summary>
	/// Override me please
	/// </summary>
	/// <param name="instant"></param>
	protected virtual void DoTransitionOut (bool instant)
	{
		TransitionOutComplete ();
	}

	/// <summary>
	/// CALL ME!!!!!
	/// </summary>
	protected void TransitionInComplete ()
	{
		state = TransitionState.IN;
		SendMessage (MSG_IN_COMPLETE, SendMessageOptions.DontRequireReceiver);
		DispatchEvent (new FLTransitionEvent (FLTransitionEvent.IN_COMPLETE));
	}

	/// <summary>
	/// CALL ME!!!!!
	/// </summary>
	protected void TransitionOutComplete ()
	{
		state = TransitionState.OUT;
        
		if (autoActivate) {
			if (activateChildren)
				gameObject.SetActiveRecursively (false);
			else
				gameObject.active = false;
		}
		
		SendMessage (MSG_OUT_COMPLETE, SendMessageOptions.DontRequireReceiver);
		DispatchEvent (new FLTransitionEvent (FLTransitionEvent.OUT_COMPLETE));
	}
	
	protected virtual void Awake ()
	{
		if (outWhenAwake)
			TransitionOut (true);
	}
	
	public virtual void OnEnable ()
	{
		if (_state == TransitionState.OUT && autoActivate) {
			if (activateChildren)
				gameObject.SetActiveRecursively (false);
			else
				gameObject.active = false;
		}
	}
    #endregion


    #region ITransition 成员

	public void TransitionIn ()
	{
		TransitionIn (false);
	}

	public void TransitionIn (bool instant)
	{
		switch (state) {
		case TransitionState.IN:
			{
				TransitionInComplete ();
				break;
			}
		case TransitionState.TRANSITION_IN:
			break;
		case TransitionState.UNKNOWN:
		case TransitionState.OUT:
		case TransitionState.TRANSITION_OUT:
			{
				state = TransitionState.TRANSITION_IN;

				if (autoActivate) {
					if (activateChildren)
						gameObject.SetActiveRecursively (true);
					else
						gameObject.SetActive(true);
				}
				//gameObject.SetActiveRecursively(true);
				SendMessage (MSG_IN_START, SendMessageOptions.DontRequireReceiver);
				DispatchEvent (new FLTransitionEvent (FLTransitionEvent.IN_START));

				DoTransitionIn (instant);
				break;
			}
		}
	}

	public void TransitionOut ()
	{
		TransitionOut (false);
	}

	public void TransitionOut (bool instant)
	{
		switch (state) {
		case TransitionState.OUT:
			{
				TransitionOutComplete ();
				break;
			}
		case TransitionState.TRANSITION_OUT:
			break;
		case TransitionState.UNKNOWN:
		case TransitionState.IN:
		case TransitionState.TRANSITION_IN:
			{
				state = TransitionState.TRANSITION_OUT;
				SendMessage (MSG_OUT_START, SendMessageOptions.DontRequireReceiver);
				DispatchEvent (new FLTransitionEvent (FLTransitionEvent.OUT_START));

				DoTransitionOut (instant);
				break;
			}
		}
	}

    #endregion
}
