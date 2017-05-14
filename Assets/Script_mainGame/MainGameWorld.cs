using UnityEngine;
using System;
using System.Collections;
//using System.Collections
using System.Collections.Generic;
using System.Linq;




public class MainGameWorld : FLEventDispatcherMono {

	enum TIMER_STATUS{
		TIMER_PAUSE,
		TIMER_RESUME,
	};


	#region DebugGUI Setting
	bool g_bDebugUI= false;
	#endregion

	public GameTimer GameTimer;
	TIMER_STATUS m_eTimerStatus = TIMER_STATUS.TIMER_PAUSE;

	#region UI
	/*public GameObject Panel_ShowScorePanel;
	public GameObject Panel_GameTutorial;
	public GameObject MainGameUI;
	public GameObject UI_BLOOD;*/

	public UISlider UI_LifeBar;
	public GameObject UI_BtnGameStart;
	public GameObject UI_BtnGameIntro;

	public Controller_Tutorial UIBehavior_TutorDialog;

	public GameObject UI_ResultPanel;


	public UILabel UI_Speed;
	public UILabel UI_Distance;
	public UILabel UI_LV; // <V2>


	//public GameObject UI_LevelTest;
	public UILabel UI_AccTest;
	Vector3 test_Dir = Vector3.zero;


	public GameObject UI_TunePanel;
	public UILabel UI_TunePanel_UploadStatus;

	public GameObject UI_PauseResumePanel;

	public GameObject UI_PauseButton;

	#endregion

	public PathGenerator  m_PathGenerator;
	public CarController m_Car;

	#region KAMA Effect
	public GameObject EffectBillBoard;
	#endregion


	#region Game_Point
	public float Point_Culture=0;
	public float Point_Science=0;
	public float Point_Health=0;
	#endregion

	private delegate void UpdateDelegate(); /// function prototype
	private UpdateDelegate updateDelegate;	


	private Vector3 m_Car_StartPosition;

	public System.DateTime  m_TimerRoundStart;
	public System.DateTime  m_TimerRoundEnd;

	public GameMode m_CurrentGameMode;
	

	public string ServerUrl = "http://127.0.0.1/UnityProject/NCUGameServer.php";

	protected void Awake () {

		//Application.targetFrameRate = 30;

		//iPhoneSettings.screenCanDarken = false;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		/*
		 * Panel Init
		 */
		UI_ResultPanel.SetActive(false);
		UI_PauseButton.SetActive(false);



		// close all first
		GameMode[] allGameMode;
		allGameMode = GameObject.Find("coreGameMode").GetComponents<GameMode>();
		foreach (GameMode mode in allGameMode) {
			mode.enabled = false;
		}

		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_NORMAL)
		{
			m_CurrentGameMode = GameObject.Find("coreGameMode").GetComponent<GameMode_Normal>();
		}
		else
		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_GREEREDLIGHT)
		{
			m_CurrentGameMode = GameObject.Find("coreGameMode").GetComponent<GameMode_RedGreenLight>();
		}
		else
		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_DECIDE_DIGITAL)
		{
			m_CurrentGameMode = GameObject.Find("coreGameMode").GetComponent<GameMode_NumberGuess>();
		}
		else  if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_MEMORY)
		{
			m_CurrentGameMode = GameObject.Find("coreGameMode").GetComponent<GameMode_Memory>();
		}


		m_CurrentGameMode.enabled = true;



	}

	// Use this for initialization
	void Start () {

		//EffectBillBoard.SetActive(true);
		float[] distances = new float[32];
		//distances[13]=15;
		//distances[12]=10f;
		//distances[11]=3.5f;
		//distances[10]=3.5f;
		//distances[10]=2.9f;
		distances[10]=2.7f;
		//distances[0]=1;
		Camera.main.layerCullDistances=distances;


//		if(  !xMain.Instance.GetPauseStatus()  )
//			xMain.Instance.PauseResumeGame();		

		UI_LifeBar.value = 1;

		m_Car_StartPosition = m_Car.gameObject.transform.position;


		// V2
		if (xMain.Instance.m_GameLevel_AotoMode == true) {


			int nGameLevelNow=0;
//			if( xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_NORMAL )
//				nGameLevelNow = xMain.Instance.mRoundData.level_normal;
//			else
//				nGameLevelNow = xMain.Instance.mRoundData.level_redgreen;
//
			nGameLevelNow = m_CurrentGameMode.GetCurrentLevel();


			UI_LV.text = "LV:" + GameSetting.Instance.m_LevelList[ nGameLevelNow ].Area + "-" + GameSetting.Instance.m_LevelList[ nGameLevelNow ].Layer;

			Debug.Log("V2-" + nGameLevelNow);
			//m_Car.UI_SetSpeedUpLimit(  GameSetting.Instance.m_LevelList[ nGameLevelNow ].speed   );
			m_Car.UI_SetSpeedUpLimit(  60.0f   ); // 2015.12.29 change according the requirement
			m_PathGenerator.AdjustWidth(   GameSetting.Instance.m_LevelList[ nGameLevelNow ].width  );


			int nRoadSeg_Current = GameSetting.Instance.m_LevelList[ nGameLevelNow ].curve;
			m_PathGenerator.AdjustProb_Straight(  GameSetting.Instance.Level_RoadSeg[ nRoadSeg_Current , 0]  );
			
			m_PathGenerator.AdjustProb_ShiftStraight(  GameSetting.Instance.Level_RoadSeg[ nRoadSeg_Current,1]  );
			m_PathGenerator.AdjustProb_15(   GameSetting.Instance.Level_RoadSeg[ nRoadSeg_Current,2] );
			m_PathGenerator.AdjustProb_30(  GameSetting.Instance.Level_RoadSeg[ nRoadSeg_Current,3] );
			m_PathGenerator.AdjustProb_45(  GameSetting.Instance.Level_RoadSeg[ nRoadSeg_Current,4] );
			m_PathGenerator.AdjustProb_60(  GameSetting.Instance.Level_RoadSeg[ nRoadSeg_Current,5] );



		}
		else
		{
			m_Car.UI_SetSpeedUpLimit(   GameSetting.Instance.Level_Speed[ GameSetting.Instance.LevelSpeedSet_Current  ]   );
			
			//Debug.Log("GameSetting.Instance.Level_Width[  GameSetting.Instance.LevelWidth_Current ] ="+GameSetting.Instance.Level_Width[  GameSetting.Instance.LevelWidth_Current ] );
			m_PathGenerator.AdjustWidth(   GameSetting.Instance.Level_Width[  GameSetting.Instance.LevelWidth_Current ]  );
			
			m_PathGenerator.AdjustProb_Straight(  GameSetting.Instance.Level_RoadSeg[ GameSetting.Instance.LevelRoadSeg_Current , 0]  );
			
			m_PathGenerator.AdjustProb_ShiftStraight(  GameSetting.Instance.Level_RoadSeg[ GameSetting.Instance.LevelRoadSeg_Current,1]  );
			m_PathGenerator.AdjustProb_15(   GameSetting.Instance.Level_RoadSeg[ GameSetting.Instance.LevelRoadSeg_Current,2] );
			m_PathGenerator.AdjustProb_30(  GameSetting.Instance.Level_RoadSeg[ GameSetting.Instance.LevelRoadSeg_Current,3] );
			m_PathGenerator.AdjustProb_45(  GameSetting.Instance.Level_RoadSeg[ GameSetting.Instance.LevelRoadSeg_Current,4] );
			m_PathGenerator.AdjustProb_60(  GameSetting.Instance.Level_RoadSeg[ GameSetting.Instance.LevelRoadSeg_Current,5] );
		}







		m_PathGenerator.GenerateRoad();
		//UI_SetSpeedUpLimit
		GameSetting.Instance.LogCurrentSetting();

	}


	void OnGameFinish( FLEventBase e )
	{

		m_Car.isMove = false;


		//UI_BtnGameStart.SetActive(false);
		m_PathGenerator.StopAutoGenerator();

//		xMain.Instance.PauseResumeGame();			
//		xMain.Instance.roundlog_InsertResult((int)Point_Health, (int)Point_Science, (int)Point_Culture, (int)Point_Civilization, (int)Point_Mine);
//
		UI_ResultPanel.SetActive(true);
		UI_PauseButton.SetActive(false);

		//Controller_ResultPanel

		bool bWin = false;
		if(UI_LifeBar.value > 0)
			bWin = true;

		if(bWin )
		{
			double LifeScore = Math.Round(  UI_LifeBar.value ,1,MidpointRounding.AwayFromZero);

			UI_ResultPanel.GetComponent<Controller_ResultPanel>().ShowResult(  bWin  , (int) (LifeScore*100 )*100 );


			// TODO:   should check the current level number first ( because you may play the previous mission now )
			//xMain.Instance.mRoundData.score_normal = (int) (UI_LifeBar.value*100 )*100 ;  // <RankingFix>-1
			xMain.Instance.nRoundCurrentScore_Normal =(int) (LifeScore*100 )*100 ;  // <RankingFix>-1
			xMain.Instance.mRoundData.score_normal += xMain.Instance.nRoundCurrentScore_Normal; // <RankingFix>-1


			if( xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_NORMAL )
			{
				xMain.Instance.mRoundData.level_normal += 1;
				xMain.Instance.RoundData_Update_Local(); // <RankingFix>-3
			}

		}
		else  // Lose
		{

			// V2
			if(xMain.Instance.m_GameLevel_AotoMode )
			{
				Debug.Log("Lose");
				if(xMain.Instance.mRoundData.level_normal >0)
				{

					xMain.Instance.mRoundData.level_normal-=1;

					Debug.Log("Lose-level minus 1:" + xMain.Instance.mRoundData.level_normal);

					xMain.Instance.RoundData_Update_Local(); // <RankingFix>-3
				}
			}

			UI_ResultPanel.GetComponent<Controller_ResultPanel>().ShowResult( bWin );

		

		}

		m_TimerRoundEnd = System.DateTime.Now;
		System.TimeSpan TimeDiff =m_TimerRoundEnd -m_TimerRoundStart;

		m_CurrentGameMode.GameFinish (bWin );
		//xMain.Instance.CurrentGameMode_GameFinish(bWin);


		xMain.Instance.WriteLogIntoFile("Time_End-"+bWin +","+m_TimerRoundEnd.ToString ("hh:mm:ss:fff")  );
		xMain.Instance.WriteLogIntoFile("TimeTotaSec,"+TimeDiff.TotalSeconds );


		//xMain.Instance.WriteLogIntoFile_DumpToFile(   );
		//WriteLogIntoFile_DumpToFile
	}


	public void BloodDamageUpdate(float fValue = 0.1f , string extraComment="")
	{
		UI_LifeBar.value -= fValue;



		if(UI_LifeBar.value >1) UI_LifeBar.value=1;

		if(UI_LifeBar.value <=0)
		{
			UI_LifeBar.value=0;
			DispatchEvent ( new FLEvent ( FLEvent.TIMER_FINISH));
		}



		//xMain.Instance.WriteLogIntoFile("BloodUpdate,"+UI_LifeBar.value + "," + m_PathGenerator.m_sCurrentSegmentName , true );
		xMain.Instance.WriteLogIntoFile("BloodUpdate,"+Math.Round(  UI_LifeBar.value,1,MidpointRounding.AwayFromZero)    + "," + m_PathGenerator.m_sCurrentSegmentName+","+extraComment , true );
	}


	public void GameNext()
	{
		GameReset();

		//xMain.Instance.c

		//if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_GREEREDLIGHT) // v0.9
		
	}



	public void GameReset()
	{
		Debug.Log("GameReset-Dump");
		xMain.Instance.WriteLogIntoFile_DumpToFile(   );

		UI_LifeBar.value =1;
		DispatchEvent ( new FLEvent ( FLEvent.TIMER_FINISH));
		DispatchEvent ( new FLEvent ( FLEvent.TIMER_RESET));


		UI_ResultPanel.SetActive(false);

		// Car 
		m_Car.gameObject.transform.position = m_Car_StartPosition;
		m_Car.gameObject.transform.eulerAngles = new Vector3(0,180,0);
		m_Car.isMove = false;


		//transform.eulerAngles
		//m_PathGenerator.GenerateRoad();
		UI_BtnGameStart.SetActive(true);
		UI_BtnGameIntro.SetActive(true);
		//UI_LevelTest.SetActive(true);



		if(UI_PauseResumePanel.activeSelf )
			PauseResumeGame();





		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_GREEREDLIGHT) // v0.9
		{
			GameObject.Find("coreGameMode").GetComponent<GameMode_RedGreenLight>().GameReset();
		}


		Time.timeScale =1;
		Application.LoadLevel("drivingScene");

		//Application.LoadLevel("drivingScene");

	}

	public void ShowTutorDialog()
	{


		List<string > m_String = new List<string>();


		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_NORMAL)
		{
			m_String.Add("左右傾斜手機來控制移動方向");
			m_String.Add("注意不要撞到路肩!");
			//
			UIBehavior_TutorDialog.gameObject.SetActive(true);
			UIBehavior_TutorDialog.ShowDialogContent(  m_String );
		}
		else
		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_GREEREDLIGHT)
		{
			m_String.Add("當變成黃色時，儘快按下左下角按鈕");
			m_String.Add("滿血時答對，可取得額外積分");
			//
			UIBehavior_TutorDialog.gameObject.SetActive(true);
			UIBehavior_TutorDialog.ShowDialogContent(  m_String );
		}
		else
		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_DECIDE_DIGITAL)
		{
			//m_String.Add("變成黑色時判斷奇數偶數");
			//m_String.Add("白色時判斷數字比5大或小");
			m_String.Add("當出現'O'或'△'時按下相同符號的按鈕");
			m_String.Add("但出現'X'時不能按任何按鈕");
			//
			UIBehavior_TutorDialog.gameObject.SetActive(true);
			UIBehavior_TutorDialog.ShowDialogContent(  m_String );
		}
		else
		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_MEMORY)
		{
			m_String.Add("請注意中央懸空的數字，\n數字為1時，代表玩家須判斷車頂方塊出現的內容(數字、圖形、顏色)\n是否與上一個內容(數字、圖形、顏色)相同，若相同就按下「決定」鍵");
			m_String.Add("在數字為2時，代表玩家必須判斷方塊內容是否與「上上」個內容\n(亦即往前回溯第二個)相同。依此類推。");
			//
			UIBehavior_TutorDialog.gameObject.SetActive(true);
			UIBehavior_TutorDialog.ShowDialogContent(  m_String );
		}



	}

	public void GameStart()
	{
		//m_PathGenerator.GenerateRoad();




		m_Car.Speed =0;
		m_Car.m_TotalDistance=0;
		m_Car.isMove = true;


		// Render the Road by setting 
		//UI_LevelTest.SetActive(false);
		UI_BtnGameStart.SetActive(false);
		UI_BtnGameIntro.SetActive(false);
		UI_PauseButton.SetActive(true);

		m_PathGenerator.StartAutoGenerator();


		//FLEvent.TIMER_FINISH

		DispatchEvent ( new FLEvent ( FLEvent.TIMER_START));
		updateDelegate = Behavior_UpdateMainGameLogic;
		
		
		//DispatchEvent ( new FLEvent ( FLEvent.PAUSE));

		GameTimer.GetComponent<GameTimer>().AddEventListener( FLEvent.STOP,OnGameFinish);

	//	GameTimer.GetComponent<GameTimer>().AddEventListener( FLEvent.TIMER_PAUSE,OnTimePause);
	//	GameTimer.GetComponent<GameTimer>().AddEventListener( FLEvent.TIMER_RESUME,OnTimerResume);


		//xMain.Instance.CurrentGameMode_GameStart();
		m_CurrentGameMode.GameStart ();


		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_NORMAL)
			m_Car.UI_SetSpeedUpLimit(60f);
		else
			m_Car.UI_SetSpeedUpLimit(40f); /// 2016.02.22 
		




		m_TimerRoundStart = System.DateTime.Now ;
		xMain.Instance.WriteLogIntoFile("GameStart,");
		xMain.Instance.WriteLogIntoFile("Time_Start,"+m_TimerRoundStart.ToString ("hh:mm:ss:fff")  );

	}

	public void GameTimer_Pause()
	{

		if( m_eTimerStatus == TIMER_STATUS.TIMER_RESUME)
		{
			m_eTimerStatus = TIMER_STATUS.TIMER_PAUSE;
			DispatchEvent ( new FLEvent ( FLEvent.TIMER_PAUSE));
			xMain.Instance.WriteLogIntoFile("GameTimer_Pause," , true);

			Debug.Log("GameTimer_Pause");

			//if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_GREEREDLIGHT) // v0.9
			{
				//GameObject.Find("coreGameMode").GetComponent<GameMode_RedGreenLight>().m_bTimePauseByGameTime = true;
				//GameObject.Find("coreGameMode").GetComponent<GameMode>().m_bTimePauseByGameTime = true;
				m_CurrentGameMode.m_bTimePauseByGameTime = true;
			}

		}
	}

	public void GameTimer_Resume()
	{
		if( m_eTimerStatus == TIMER_STATUS.TIMER_PAUSE)
		{
			m_eTimerStatus = TIMER_STATUS.TIMER_RESUME;
			DispatchEvent ( new FLEvent ( FLEvent.TIMER_RESUME));
			xMain.Instance.WriteLogIntoFile("GameTimer_Resume," , true);

			Debug.Log("GameTimer_Resume");

			//if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_GREEREDLIGHT) // v0.9
			{
				//GameObject.Find("coreGameMode").GetComponent<GameMode_RedGreenLight>().m_bTimePauseByGameTime = false;
				//GameObject.Find("coreGameMode").GetComponent<GameMode>().m_bTimePauseByGameTime = false;
				m_CurrentGameMode.m_bTimePauseByGameTime = false;
			}
		}
	}

	void Behavior_UpdateMainGameLogic()
	{
		UI_Speed.text =  m_Car.Speed + "km/h";
		UI_Distance.text = m_Car.m_TotalDistance + "m";
		//m_Car.Speed
		//m_Car.m_TotalDistance


		/*
#if UNITY_ANDROID
		test_Dir.x = Input.acceleration.x;
		test_Dir.y = -Input.acceleration.y;
		test_Dir.z = Input.acceleration.z;
		if( test_Dir.sqrMagnitude >1)
			test_Dir.Normalize();

		UI_AccTest.text = "AccX:"+test_Dir.x ;

#endif*/


	}


	void OnApplicationPause(bool pauseStatus)   // <AppExit>
	{
		if(pauseStatus)
		{
			xMain.Instance.WriteLogIntoFile(  "Quit",true);
			Debug.Log("OnApplicationPause-Dump");
			xMain.Instance.WriteLogIntoFile_DumpToFile(   );
			// Record something ?
			Application.Quit();
		}
	}

	public void ExitGame()
	{
		Debug.Log("ExitGame-Dump");
		xMain.Instance.WriteLogIntoFile_DumpToFile(   );


		//Application.Quit();
		Application.LoadLevel("scene_mainMenu");//  v0.8

	}




	public void UI_ShowHideTunePanel()
	{

		UI_TunePanel.SetActive(  ! UI_TunePanel.activeSelf );
	}

	public void PauseResumeGame()
	{

		UI_PauseResumePanel.SetActive(  !UI_PauseResumePanel.activeSelf );

		if(UI_PauseResumePanel.activeSelf) // Pause Now
		{
			// stop time clock
			Time.timeScale = 0;
		}
		else
			Time.timeScale = 1;

	}


	public void UploadGameSettingToServer()
	{



		UI_TunePanel_UploadStatus.text = "uploading...";

		//ServerUrl
		JSONObject  j = new JSONObject( JSONObject.Type.OBJECT);
		j.AddField("method", 0.5f);


		j.AddField("data", m_PathGenerator.GetRoadProbData()  +"MaxSpeed:"+m_Car.Speed_UpLimit );

	
		string encodingStr = j.Print();

		WWW www ;

		WWWForm form = new WWWForm();
		form.AddField("Save", encodingStr);

		www = new WWW(ServerUrl, form);
		StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		// check for errors
		if (www.error == null)
		{
			Debug.Log(" WWW Ok!: " + www.text);
			UI_TunePanel_UploadStatus.text = "Done";

		} else {
			Debug.Log("WWW Error: "+ www.error);
			UI_TunePanel_UploadStatus.text = "Fail:" + www.error;
		}    
	}


	// Update is called once per frame
	void Update () {
	
		if(updateDelegate != null)
		{
			updateDelegate(); // function pointer
		}     


		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.LoadLevel("scene_mainMenu");
	}
}
