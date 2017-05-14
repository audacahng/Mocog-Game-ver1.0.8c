using UnityEngine;
using System.Collections;

public class GameMode_Normal : GameMode {

	#region Game Mode related Define
	
	#endregion
	
	#region TestCase 
	public bool bTestModeStart=false;
	#endregion
	
	
	#region UI
	/// UI 
	#endregion
	
	
	#region AI Behavior
	// Behavior 
	private delegate void UpdateDelegate(); /// function prototype
	private UpdateDelegate updateDelegate;
	private float elapseTime=0;
	#endregion
	
	
	
	override public  void InitSetting (){
		
	}
	
	override public  int  GetCurrentLevel(){
		return 1;
	}
	
	override public  void GameStart(){
		
	}
	
	override public  void GameFinish ( bool bWinLose ){
		
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		
		if(bTestModeStart == true)
		{
			bTestModeStart=false;
			GameStart();
		}
		
		if(updateDelegate != null)
		{
			updateDelegate(); // function pointer
		}		
		
	}
}
