using UnityEngine;
using System.Collections;

public class FLEvent : FLEventBase{
	
	public const string SELECTED = "selected";
	public const string START = "start";
	public const string PAUSE = "pause";
	public const string RESUME = "resume";
	public const string STOP = "stop";
	public const string COMPLETE = "complete";
	public const string CHANGE = "change";
	
	public const string OPEN = "open";
	public const string CLOSE = "close";
	
	public const string ACTIVATE = "activate";
	public const string DEACTIVATE = "deactivate";


	public const string MAINGAMEUI_IN="MainGameUIIn";

	public const string TIMER_START="timerStart";
	public const string TIMER_PAUSE="timerPause";
	public const string TIMER_RESUME="timerResume";
	public const string TIMER_FINISH="timerFinish";
	public const string TIMER_RESET="timerReset";	
	
	
	public FLEvent(string type):base(type){}
}
