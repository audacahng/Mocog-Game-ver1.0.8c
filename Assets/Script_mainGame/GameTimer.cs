using UnityEngine;
using System.Collections;


[RequireComponent (typeof (UILabel))]
public class GameTimer : FLEventDispatcherMono {


	/// <summary>
	/// Start this instance.
	/// 
	/// Receive Message - Pause
	/// Receive Message - Start
	/// </summary> 
	/// 

	float m_TimeTotal = 180f;
	//float m_TimeTotal = 30f;

	float m_bTimer;
	float m_bTimer_5SecCount=0f;
	UILabel UIClock;


	private delegate void UpdateDelegate(); /// function prototype
	private UpdateDelegate updateDelegate;	
	
	public void AddTime( float  fValue)
	{
		m_bTimer += fValue;
	}



	// Use this for initialization
	void Start () {


		GameObject core = GameObject.Find("core");
		if(core)
		{
			core.GetComponent<MainGameWorld>().AddEventListener(FLEvent.TIMER_START, OnTimerStart );
			core.GetComponent<MainGameWorld>().AddEventListener(FLEvent.TIMER_PAUSE, OnTimerPause );
			core.GetComponent<MainGameWorld>().AddEventListener(FLEvent.TIMER_FINISH, OnTimerFinish );
			core.GetComponent<MainGameWorld>().AddEventListener(FLEvent.TIMER_RESUME, OnTimerResume );
			core.GetComponent<MainGameWorld>().AddEventListener(FLEvent.TIMER_RESET, OnTimerReset);

			//public const string TIMER_RESET="timerReset";
		}
		else
		{
			Debug.LogError("Timer Listener Fail");
		}
		//TIMER_RESUME

		
		UIClock = gameObject.GetComponent< UILabel>();
		
		m_bTimer = 0 ;
		updateDelegate = null;
	}
	
	public float GetCurrentTime()
	{
		return m_bTimer;
	}
	
	
	private void OnTimerResume(FLEventBase e)
	{
	//	Debug.Log("OnTimerResume");
		//m_bTimer+=1.0f;
		
		//m_bTimer_5SecCount+=1.0f;
		
		updateDelegate = BehaviorDoUpdateTimer;
	}
	
	
	private void OnTimerStart (FLEventBase e)	
	{
		Debug.Log("OnTimerStart");
		
		updateDelegate = BehaviorDoUpdateTimer;

		Time.timeScale=1;
		Debug.Log("TimeScale:"+Time.timeScale);
		
		
	}
	
	void BehaviorDoUpdateTimer()
	{
		m_bTimer+=Time.deltaTime;
		m_bTimer_5SecCount+=Time.deltaTime;
		
		float m_bTimerInverse= m_TimeTotal - m_bTimer;
		UIClock.text = (int)(m_bTimerInverse/60) +":"+ ((int)( m_bTimerInverse )%60).ToString("D2") ;
		
 
		
		/*
		if( (int)(m_bTimer) == 0  && bItemSelectMode == false)
		{
			bItemSelectMode =true;
			DispatchEvent( new FLEvent ( FLEvent.PAUSE) );
			updateDelegate = null;
		}
		else
			if( (int)(m_bTimer) == 60 && bItemSelectMode == false)
		{
			bItemSelectMode =true;
			DispatchEvent( new FLEvent ( FLEvent.PAUSE) );
			updateDelegate = null;
		}
		else
			if( (int)(m_bTimer) == 120 && bItemSelectMode == false)
		{
			bItemSelectMode =true;
			DispatchEvent( new FLEvent ( FLEvent.PAUSE) );
			updateDelegate = null;
		}		
		else
			if( (int)(m_bTimer) == 180 && bItemSelectMode == false)
		{
			bItemSelectMode =true;
			DispatchEvent( new FLEvent ( FLEvent.STOP) ); // Game Finish , Counting Panel 
			Debug.Log( "FLEvent.STOP" );
			updateDelegate = null;
		}		
		*/

		if( (int)(m_bTimer) >= m_TimeTotal  )
		{
			DispatchEvent( new FLEvent ( FLEvent.STOP) ); // Game Finish , Counting Panel 
			Debug.Log( "FLEvent.STOP" );
			updateDelegate = null;
		}		
 
		
	}


	private void OnTimerReset (FLEventBase e)	
	{
		m_bTimer = 0;

		float m_bTimerInverse= 180f - m_bTimer;
		UIClock.text = (int)(m_bTimerInverse/60) +":"+ (int)( m_bTimerInverse )%60;
			
	}
	
	private void OnTimerPause (FLEventBase e)	
	{
//		Debug.Log("OnTimerPause");
		updateDelegate = null;
	}
	
	private void OnTimerFinish (FLEventBase e)	
	{
		Debug.Log("OnTimerFinish-Interrupt");
		
		DispatchEvent( new FLEvent ( FLEvent.STOP) ); // Game Finish , Counting Panel 
		Debug.Log( "FLEvent.STOP" );
		updateDelegate = null;

		
	}
	
	
	// Update is called once per frame
	void Update () {
		
		
		if(updateDelegate != null)
		{
			updateDelegate(); // function pointer
		}        	
	}}
