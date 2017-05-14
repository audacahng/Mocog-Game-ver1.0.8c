using UnityEngine;
using System.Collections;
using AnimationOrTween;
using System.Collections.Generic;

public class GameMode_RedGreenLight : GameMode {

	public GameObject ReactiveButton;
	public UISprite CarBillBoard;
	public UILabel CarBillBoard_Label;

	public UISlider TimerCountDownBar;
	public UILabel UI_ExtraCreditLabel;

	public GameObject ParticleToAnime;

	public bool m_bAnsBtnLock = false; // when press the Answer Btn in GreenLight , due to the spare time of animation Show , must keep the lock until the animation finish.

	//UpdateLightDisplay
	public bool bTestModeStart=false;

	string m_LogTag = "[Mode_RG]";


	int m_Score_ExtraCredit=0;

	float m_fCurrentIdleWaitTime = 2;

	List<int> RandomIndex = new List<int> (); // for random order  

	public int  GetScore_ExtraCredit()
	{
		return m_Score_ExtraCredit;
	}

	void UpdateExtraCredit( int nScoreToAdd=0)
	{
		m_Score_ExtraCredit += nScoreToAdd;


		UI_ExtraCreditLabel.gameObject.transform.parent.gameObject.SetActive(true);

		UI_ExtraCreditLabel.text = "額外加分:" + m_Score_ExtraCredit;
		xMain.Instance.WriteLogIntoFile( m_LogTag + "ScoreAdd:" + nScoreToAdd , true);
	}

	private enum LIGHT_STATUS{
		YELLOW,
		RED,
		GREEN
	};


	private delegate void UpdateDelegate(); /// function prototype
	private UpdateDelegate updateDelegate;
	private float elapseTime=0;


	bool m_bGameStart= false;
	//const float RoundTimeTotal = 183;

//	const int cTimesTotal_Green=33;
//	const int cTimesTotal_Red=11;
	const int cTimesTotal_Green=75;
	const int cTimesTotal_Red=25;


	const int cTimesTotal = cTimesTotal_Green + cTimesTotal_Red;
	//const int cTimesTotal_Green=3;
	//const int cTimesTotal_Red=3;

	int mTimesTotal_Green = cTimesTotal_Green;
	int mTimesTotal_Red = cTimesTotal_Red;
	int mTimesTotal = cTimesTotal;
	LIGHT_STATUS []Arr_AnserSet_Rand; //Anser Set


	int mSecondToKeep=1;

	LIGHT_STATUS m_CurrentLight= LIGHT_STATUS.YELLOW;


	//public bool m_bTimePauseByGameTime = false;  // v0.9


	void Behaviour_Idle()
	{
		if(elapseTime==0)
		{
			TimerCountDownBar.value=0;
			m_fCurrentIdleWaitTime = Random.Range( 1.5f , 2.5f );
		}

		if(m_bTimePauseByGameTime == false) // v0.9
		elapseTime +=Time.deltaTime;


		if(  elapseTime  > m_fCurrentIdleWaitTime )
		//if(  elapseTime  > 2 )
		{
			elapseTime=0;
			m_fCurrentIdleWaitTime = 2f;

			Animation Anm = CarBillBoard.gameObject.GetComponent<Animation>();
			Direction dir = Direction.Forward;
			ActiveAnimation anim0;
			anim0 = ActiveAnimation.Play(Anm, "Animation_Blink", dir, EnableCondition.EnableThenPlay, DisableCondition.DoNotDisable);
			//ActiveAnimation.Play(  Anm, AnimationOrTween.Direction.Forward);
			anim0.eventReceiver = gameObject;
			//anim0.callWhenFinished = "AnimationLoop";

			updateDelegate = Behaviour_Changing;
		}

	}

//	void AnimationLoop()
//	{
//		Animation Anm = CarBillBoard.gameObject.GetComponent<Animation>();
//		Direction dir = Direction.Forward;
//		ActiveAnimation anim0;
//		anim0 = ActiveAnimation.Play(Anm, "Animation_Blink", dir, EnableCondition.EnableThenPlay, DisableCondition.DoNotDisable);
//		//ActiveAnimation.Play(  Anm, AnimationOrTween.Direction.Forward);
//		anim0.eventReceiver = gameObject;
//		//anim0.callWhenFinished = "AnimationLoop";
//
//	}
//
	
	void Behaviour_Changing()
	{
		// 0.5 sec , 
		if(m_bTimePauseByGameTime == false) // v0.9
		elapseTime +=Time.deltaTime;

		if(  elapseTime  > 0.5f )
		{
			elapseTime=0;



			ColorDecide();


			//TimerCountDownBar.gameObject.SetActive(true);
			TimerCountDownBar.gameObject.SetActive(false);// v1.1
			updateDelegate=Behaviour_Changed;
		}

	}

	private void ColorDecide()
	{
		//mTimesTotal_Green;
		//mTimesTotal_Red;

		//Arr_AnserSet_Rand

		if(  mTimesTotal !=0 )
		{
			int nCurrentIndex = cTimesTotal -  mTimesTotal;

			if (Arr_AnserSet_Rand[  nCurrentIndex]  == LIGHT_STATUS.RED)
			{
				UpdateLightDisplay(LIGHT_STATUS.RED);
			}
			else
				UpdateLightDisplay(LIGHT_STATUS.GREEN);



			mTimesTotal-=1;

			return;
		}


		// Random Get 
//		if( Random.Range(1,10) >4)
//		//if( Random.Range(1,mTimesTotal_Green + mTimesTotal_Red  ) >mTimesTotal_Red  )
//		{
//
//			if(mTimesTotal_Red>0)
//			{
//				UpdateLightDisplay(LIGHT_STATUS.RED);
//				mTimesTotal_Red-=1;
//			}
//			else
//			if(mTimesTotal_Green>0)
//			{
//				UpdateLightDisplay(LIGHT_STATUS.GREEN);
//				mTimesTotal_Green-=1;
//			}
//
//		}
//		else
//		{
//			if(mTimesTotal_Green>0)
//			{
//				UpdateLightDisplay(LIGHT_STATUS.GREEN);
//				mTimesTotal_Green-=1;
//			}
//			else
//			if(mTimesTotal_Red>0)
//			{
//				UpdateLightDisplay(LIGHT_STATUS.RED);
//				mTimesTotal_Red-=1;
//			}
//		}
//


	}

	void Behaviour_Done()
	{

		elapseTime=0;
		TimerCountDownBar.value =0;
		TimerCountDownBar.gameObject.SetActive(false);
		
		UpdateLightDisplay(LIGHT_STATUS.YELLOW);
		
		updateDelegate=Behaviour_Idle;



		//xMain.Instance.WriteLogIntoFile("RGLight_Hide,"+CarBillBoard.color   + "," + xMain.Instance.GetGameCore().m_PathGenerator.m_sCurrentSegmentName , true );
		
		//if(mTimesTotal_Red ==0 && mTimesTotal_Green==0)
		if(mTimesTotal ==0)
		{	
			//GameFinish();
			updateDelegate=null;
			CarBillBoard.gameObject.SetActive(true);

			//CarBillBoard.color = Color.yellow;
			UpdateLightDisplay(LIGHT_STATUS.YELLOW);

			TimerCountDownBar.gameObject.SetActive(false);

			return;
		}
	}

	public void GameReset()
	{
		CarBillBoard.gameObject.SetActive(false);

	}
	//m_bAnsBtnLock
	void Behaviour_Changed()
	{
		// 1 sec , count down
		elapseTime +=Time.deltaTime;
		TimerCountDownBar.value = 1 - elapseTime;

		if(  elapseTime  > 1f )
		{
			if(m_CurrentLight == LIGHT_STATUS.GREEN)
			{
				xMain.Instance.GetGameCore().BloodDamageUpdate( 0.1f,"OverTime");

				//xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",BtnPress,"+"Green" , true );
			}
			else
			{

				if(m_bAnsBtnLock) return;
				
				GameObject particieObj = GameObject.Instantiate( ParticleToAnime ) as GameObject;
				
				particieObj.layer = LayerMask.NameToLayer( "UI");
				particieObj.transform.parent = TimerCountDownBar.gameObject.transform.parent;
				particieObj.transform.localScale = Vector3.one;
				particieObj.transform.position = TimerCountDownBar.transform.position;
				particieObj.AddComponent<Behaviour_AutoDestroy>();
				//particieObj.SetActive(false);
				
				//Hashtable MoveSetting = iTween.Hash( "time", 0.4f ,"islocal",false, "position" ,GameObject.Find("UI_LifeBar").transform.position ,"easeInSine","linear","oncomplete", "OnMoveToBarComplete_Red", "oncompletetarget", gameObject  );
				Hashtable MoveSetting = iTween.Hash( "time", 0.0f ,"islocal",false, "position" ,GameObject.Find("UI_LifeBar").transform.position ,"easeInSine","linear","oncomplete", "OnMoveToBarComplete_Red", "oncompletetarget", gameObject  );
				iTween.MoveTo(  particieObj , MoveSetting );
				
				elapseTime=0;
				updateDelegate=null;
				
				m_bAnsBtnLock = true;


//				if(xMain.Instance.GetGameCore().UI_LifeBar.value>=1 )
//				{
//					UpdateExtraCredit(  (int) (  50 ) );
//				}
//				xMain.Instance.GetGameCore().BloodDamageUpdate( -0.1f);




			}

			updateDelegate=Behaviour_Done;

		}

		
	}



	void UpdateLightDisplay( LIGHT_STATUS status)
	{
		m_CurrentLight = status;


		if(m_CurrentLight == LIGHT_STATUS.YELLOW)
		{
			CarBillBoard.color = Color.yellow;
			CarBillBoard_Label.text = "";
			return;
		}
		else
		if(m_CurrentLight == LIGHT_STATUS.GREEN)
		{
			CarBillBoard.color = Color.green;
			CarBillBoard_Label.text = "O";
		}
		else
		if(m_CurrentLight == LIGHT_STATUS.RED)
		{
			CarBillBoard.color = Color.red;
			CarBillBoard_Label.text = "X";
		}

		string sColorStr;
		if(m_CurrentLight == LIGHT_STATUS.RED )
			sColorStr="R";
		else
			sColorStr="G";
			

		xMain.Instance.WriteLogIntoFile(m_LogTag+"["+ (  cTimesTotal - mTimesTotal ) +  "]:"+sColorStr   + "," + xMain.Instance.GetGameCore().m_PathGenerator.m_sCurrentSegmentName , true );
	}



	public void GenerateRandomOrderSet()
	{
		//mTimesTotal
		LIGHT_STATUS []Arr_AnserSet_Ori = new LIGHT_STATUS[ cTimesTotal];
		Arr_AnserSet_Rand = new LIGHT_STATUS[ cTimesTotal];


		for(int i = 0 ; i < cTimesTotal ; i++)
		{
			if( i < cTimesTotal_Green )
				Arr_AnserSet_Ori[i] = LIGHT_STATUS.GREEN;
			else
			//if( i < cTimesTotal_Green + cTimesTotal_Red )
			{
				Arr_AnserSet_Ori[i] = LIGHT_STATUS.RED;
			}

			RandomIndex.Add(i);
		}

		string Record="";
		for(int i = 0 ; i < cTimesTotal ; i++)
		{
			int nRan = Random.Range (0, RandomIndex.Count);
			Arr_AnserSet_Rand[  i ] =Arr_AnserSet_Ori[ RandomIndex[ nRan ] ];
			RandomIndex.Remove ( RandomIndex[ nRan ] );


			if(Arr_AnserSet_Rand[i] == LIGHT_STATUS.RED)
				Record+="R,";
			else
				Record+="G,";
			//Debug.Log("Arr_AnserSet_Rand["+ i + "]="+Arr_AnserSet_Rand[i]);
		}

		xMain.Instance.WriteLogIntoFile( Record);


	}

	override public void GameStart()
	{
		ReactiveButton.SetActive(true);
		CarBillBoard.gameObject.SetActive(true);
		TimerCountDownBar.gameObject.SetActive(false);

		mTimesTotal = cTimesTotal;
		GenerateRandomOrderSet();



		m_bGameStart = true;

		CarBillBoard.color = Color.yellow;
		//CarBillBoard.color

		m_Score_ExtraCredit=0;
		UpdateExtraCredit();

		updateDelegate = Behaviour_Idle;
	}


	override public void InitSetting ()
	{

	}


	override public void GameFinish(bool bWinLose)
	{
		//ReactiveButton.SetActive(false);  // v0.9

		//CarBillBoard.gameObject.SetActive(false);  // after finish , keep it in yellow
		CarBillBoard.gameObject.SetActive(true);
		CarBillBoard.color = Color.yellow;
		TimerCountDownBar.gameObject.SetActive(false);

		xMain.Instance.WriteLogIntoFile(m_LogTag+",GameFinish-Extrascore,"+ m_Score_ExtraCredit  , true );
		//xMain.Instance.WriteLogIntoFile_DumpToFile();

		if(bWinLose== true  )
		{

			if( xMain.Instance.mRoundData.level_redgreen < ( GameSetting.Instance.m_LevelList.Count -1 )  )
				xMain.Instance.mRoundData.level_redgreen+=1;

			//xMain.Instance.mRoundData.score_redgreen = xMain.Instance.mRoundData.score_normal + m_Score_ExtraCredit; // <RankingFix>-2
			//xMain.Instance.mRoundData.score_normal=0;

			// <RankingFix>-2
			xMain.Instance.nRoundCurrentScore_RedGreen = xMain.Instance.nRoundCurrentScore_Normal + m_Score_ExtraCredit;
			// accumulate 
			xMain.Instance.mRoundData.score_redgreen  += xMain.Instance.nRoundCurrentScore_RedGreen;

			//xMain.Instance.mRoundData.score_redgreen 


			xMain.Instance.RoundData_Update_Local();
		}
		else
		{
			// V2
			if(xMain.Instance.m_GameLevel_AotoMode )
				if(xMain.Instance.mRoundData.level_redgreen >0)
				{
					xMain.Instance.mRoundData.level_redgreen-=1;
					xMain.Instance.RoundData_Update_Local();
				}
		}


		updateDelegate=null;
	}


	public void GamePause()
	{

		m_bGameStart = false;
	}


	public void AnswerButtonPress()
	{
		xMain.Instance.GetGameCore();



		if( updateDelegate == Behaviour_Changing)
		{
			//


			xMain.Instance.GetGameCore().BloodDamageUpdate( 0.1f,"TooFast");

			//m_Score_ExtraCredit
			xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",BtnPress,"+"Yellow"  , true );
			Debug.Log("BtnPress,Yellow");
			updateDelegate=Behaviour_Done;
		}
		else
		// Current Status Check
		if( m_CurrentLight == LIGHT_STATUS.GREEN)
		{
			if(m_bAnsBtnLock) return;

			GameObject particieObj = GameObject.Instantiate( ParticleToAnime ) as GameObject;

			particieObj.layer = LayerMask.NameToLayer( "UI");
			particieObj.transform.parent = TimerCountDownBar.gameObject.transform.parent;
			particieObj.transform.localScale = Vector3.one;
			particieObj.transform.position = TimerCountDownBar.transform.position;
			particieObj.AddComponent<Behaviour_AutoDestroy>();
			//particieObj.SetActive(false);

			//Hashtable MoveSetting = iTween.Hash( "time", 0.4f ,"islocal",false, "position" ,GameObject.Find("UI_LifeBar").transform.position ,"easeInSine","linear","oncomplete", "OnMoveToBarComplete_Green", "oncompletetarget", gameObject  );
			Hashtable MoveSetting = iTween.Hash( "time", 0.0f ,"islocal",false, "position" ,GameObject.Find("UI_LifeBar").transform.position ,"easeInSine","linear","oncomplete", "OnMoveToBarComplete_Green", "oncompletetarget", gameObject  );
			iTween.MoveTo(  particieObj , MoveSetting );

			elapseTime=0;
			updateDelegate=null;

			m_bAnsBtnLock = true;
			//Debug.Log("BtnPress,Green");
			xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",BtnPress,"+"Green" , true );
		}
		else
		if( m_CurrentLight == LIGHT_STATUS.RED)
		{

			xMain.Instance.GetGameCore().BloodDamageUpdate( 0.1f,"WrongPress");
			updateDelegate=Behaviour_Done;
			Debug.Log("BtnPress,Red");
			xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",BtnPress,"+"Red" , true );
		}


	}

	// Add Score
	void OnMoveToBarComplete_Green()
	{
		if(xMain.Instance.GetGameCore().UI_LifeBar.value>=1 )
		{
			UpdateExtraCredit(  (int) (TimerCountDownBar.value*100 ) );
		}
		xMain.Instance.GetGameCore().BloodDamageUpdate( -0.1f,"O_Green");

		updateDelegate=Behaviour_Done;
		m_bAnsBtnLock = false;
	}

	void OnMoveToBarComplete_Red()
	{
		if(xMain.Instance.GetGameCore().UI_LifeBar.value>=1 )
		{
			UpdateExtraCredit(  (int) (  50 ) );
		}
		xMain.Instance.GetGameCore().BloodDamageUpdate( -0.1f,"O_DidnPressInRed");

		updateDelegate=Behaviour_Done;
		m_bAnsBtnLock = false;

	}


	// Use this for initialization
	void Start () {
	

		TimerCountDownBar.value=0;

		ReactiveButton.SetActive(true); // v0.9

		//GameStart();
		//GenerateRandomOrderSet();


	}

	override public int  GetCurrentLevel ()
	{
		return xMain.Instance.mRoundData.level_redgreen;
	}


	
	
	// Update is called once per frame
	void Update () {
		//UpdateCube();

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
