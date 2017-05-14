using UnityEngine;
using System.Collections;


public class GameMode_NumberGuess : GameMode {


	/*enum QTYPE{
		A,  // odd and event guess
		B,  // bigger or smaller
		COUNT
	}


	enum MODE_NUMTYPE{
		MODE_NUM_BLACK,
		MODE_NUM_WHITE,
		MODE_NUM_MIX,
		MODE_COUNT
	};*/


	enum SIGNAL{
		Circle,
		Triangle,
		Cross,
		Blank
	}


	//string m_LogTag = "[Mode_GN]";
	string m_LogTag = "[Mode_SS]";

	//MODE_NUMTYPE m_Mode_Current;

	//QTYPE	m_Type_Current;

	public bool bTestModeStart=false;

	// set cross signal can/can't show, true = can't show, false = can show
	private bool cross_random = false;
	// true = show zcross, false = hide cross
	private bool cross_show = false;

	private float crossDelayTime = 0.4f; // Cross show up time
	private float deltaDelay = 0.02f;

	/// <summary>
	/// UI Part
	/// </summary>
	public GameObject ReactiveButton_Left;
	public GameObject ReactiveButton_Right;

	public UILabel Label_LeftBtn;
	public UILabel Label_RightBtn;

	public UISprite CarBillBoard;
	public UILabel CarBillBoard_Label; // number show 


	public UILabel CarBillBoard_Label_Debug; // number show 
	public UILabel CarBillBoard_Label_Debug_2; // number show 






	/// <summary>
	/// Update the Color 
	/// </summary>
	private delegate void UpdateDelegate(); /// function prototype
	private UpdateDelegate updateDelegate;
	private float elapseTime=0;


	/// <summary>
	/// Update the Digital
	/// </summary>
	private delegate void UpdateDelegate_num(); /// function prototype
	private UpdateDelegate updateDelegate_num;
	private float elapseTime_num=0;



	bool m_bGameStart= false;
	//const float RoundTimeTotal = 183;


	const int cTimeForEach = 2;   
	const int cTime_ModeBlack=44;
	const int cTime_ModeWhite=44;
	const int cTime_ModeMix=90;
	const int cTimesTotal = cTime_ModeBlack + cTime_ModeWhite +cTime_ModeMix ;
	const int m_QuestionTotal =  cTimesTotal/2;


	//ArrayList m_QuestionBankList = new ArrayList(); 



	//  {A B A B B B A A }, {A B B A A B B A}, {A B B A B B A A}
	//QTYPE[,]Arr_3_8Set = new QTYPE[3,8]{  {QTYPE.A,QTYPE.B,QTYPE.A,QTYPE.B,QTYPE.B,QTYPE.B,QTYPE.A,QTYPE.A}, {QTYPE.A,QTYPE.B,QTYPE.B,QTYPE.A,QTYPE.A,QTYPE.B,QTYPE.B,QTYPE.A}, {QTYPE.A,QTYPE.B,QTYPE.B,QTYPE.A,QTYPE.B,QTYPE.B,QTYPE.A,QTYPE.A}};

	// {A B A B B}, {A B B A A}, {A B B A B}, {B B B A A}, {B B A A A}, {B B A A B}, {B A A B B}
	//QTYPE [,]Arr_7_5Set = new QTYPE[7,5]{ {QTYPE.A,QTYPE.B,QTYPE.A,QTYPE.B,QTYPE.B}, {QTYPE.A,QTYPE.B,QTYPE.B,QTYPE.A,QTYPE.A}, {QTYPE.A,QTYPE.B,QTYPE.B,QTYPE.A,QTYPE.B}, {QTYPE.B,QTYPE.B,QTYPE.B,QTYPE.A,QTYPE.A}, {QTYPE.B,QTYPE.B,QTYPE.A,QTYPE.A,QTYPE.A}, {QTYPE.B,QTYPE.B,QTYPE.A,QTYPE.A,QTYPE.B}, {QTYPE.B,QTYPE.A,QTYPE.A,QTYPE.B,QTYPE.B}};


	int m_QuestionCurrentCount=0;

	//int m_nDigitalCurrent = -1;
	SIGNAL m_nSignalCurrent = SIGNAL.Blank;

	bool m_bAnswerPress = false;


	float m_fCurrentIdleWaitTime = 2;
	//public bool m_bTimePauseByGameTime = false;  // v0.9


	override public  void InitSetting (){

		/// Init the UI
		ReactiveButton_Left.SetActive(true); //
		ReactiveButton_Right.SetActive(true); //
		CarBillBoard.gameObject.SetActive (true);

		Label_LeftBtn.text = "O";
		Label_RightBtn.text = "△";

		//m_Type_Current = QTYPE.A;
	}



	/*void Init_QuestionOrder_Debug()
	{
		m_QuestionBankList.Clear ();

		int nTotal = 22 + 22 + 5 +1 ;

		for(int i = 0 ; i < nTotal ; i++){

			m_QuestionBankList.Add(  (QTYPE)(i  % (int)QTYPE.COUNT) );


		}


		// check 
		string log_questionBank="";
		Debug.Log ("Total Count:"+ m_QuestionBankList.Count);
		foreach (QTYPE t in m_QuestionBankList)
			log_questionBank+= ( t == QTYPE.A? "A,":"B,"  );
		
		Debug.Log ("QuestionBank:\n"+log_questionBank);
		xMain.Instance.WriteLogIntoFile ("Questions," + log_questionBank);


	}*/


	/*void Init_QuestionOrder()
	{
		m_QuestionBankList.Clear ();

		// first 22 and second 22
		if (Random.Range (0, 10) > 5) {

			for(int i = 0 ; i < 22 ; i++)
				m_QuestionBankList.Add( QTYPE.A  );

			for(int i = 0 ; i < 22 ; i++)
				m_QuestionBankList.Add( QTYPE.B  );

		}
		else{
			for(int i = 0 ; i < 22 ; i++)
				m_QuestionBankList.Add( QTYPE.B  );

			for(int i = 0 ; i < 22 ; i++)
				m_QuestionBankList.Add( QTYPE.A  );
		}



		// get 5 times from 3_8 array
		for (int nCount_1 = 0; nCount_1 <5; nCount_1++) {
			int nRand_8 = 0;
			// 8-1
			nRand_8 = Random.Range (0, 3); // 0~2
			for (int i = 0 ; i<8 ; i++)
				m_QuestionBankList.Add( Arr_3_8Set[ nRand_8 ,i] );
		}

		// get 1 time from 7_5 array
		int nRand_5 = Random.Range (0, 7); // 0~6
		for (int i = 0 ; i<5 ; i++)
			m_QuestionBankList.Add( Arr_7_5Set[ nRand_5 ,i] );

		// check 
		string log_questionBank="";
		Debug.Log ("Total Count:"+ m_QuestionBankList.Count);
		foreach (QTYPE t in m_QuestionBankList)
			log_questionBank+= ( t == QTYPE.A? "A,":"B,"  );

		Debug.Log ("QuestionBank:\n"+log_questionBank);
		xMain.Instance.WriteLogIntoFile ("Questions," + log_questionBank);


	}*/



	void Start()
	{
		//Init_QuestionOrder ();
		//Init_QuestionOrder_Debug();
		//InitSetting ();
	}



	/*int GetRandomDigitExcept5(){
		int nCurrent = Random.Range(1,10); // 1~9

		while(nCurrent ==5)
			nCurrent = Random.Range(1,10);

		return nCurrent;
	}*/

	SIGNAL GetRandomSignal() {
		SIGNAL[] s = {SIGNAL.Circle, SIGNAL.Triangle};
		return s[Random.Range (0, 2)];
	}

	void Behavior_Num_Start()
	{

		if (elapseTime_num == 0) {

			//m_nDigitalCurrent = Random.Range(1,10); // 1~9
			//m_nDigitalCurrent = GetRandomDigitExcept5(); // 1~9

			m_nSignalCurrent = GetRandomSignal(); // get signal
			UpdateDigitalDisplay( true , m_nSignalCurrent );
			// log the signal shown
			xMain.Instance.WriteLogIntoFile( "Showing Signal:"+m_nSignalCurrent , true );

			// log the number shown
			//xMain.Instance.WriteLogIntoFile( "Showing Num:"+m_nDigitalCurrent +",Mode:" + m_Type_Current   , true );

			m_bAnswerPress= false;

		}

		if(m_bTimePauseByGameTime == false) // v0.9
			elapseTime_num +=Time.deltaTime;

		if(m_bAnswerPress == true) // if press , don't display
			UpdateDigitalDisplay( false , m_nSignalCurrent );

		if (elapseTime_num > crossDelayTime) {
			if (!cross_random) {
				float probability = Random.value;
				if (probability < 0.75) {
					//m_nSignalCurrent = SIGNAL.Cross;
					UpdateDigitalDisplay (true, SIGNAL.Cross);
					cross_show = true;
					xMain.Instance.WriteLogIntoFile( "Showing Signal:"+m_nSignalCurrent , true );
					xMain.Instance.WriteLogIntoFile( "Cross Show Up Time:"+crossDelayTime , true );
				}
				Debug.Log (probability);
				cross_random = true;
			}
		}

		if (elapseTime_num > 1) {
			elapseTime_num = 0;
			// you didn't answer , minus score
			if(m_bAnswerPress == false && cross_show == false)
			{
				xMain.Instance.GetGameCore().BloodDamageUpdate( 0.1f,"NoPress");
				xMain.Instance.WriteLogIntoFile( "BtnPress:"+"No,"+"Signal:"+m_nSignalCurrent   , true );
				//xMain.Instance.WriteLogIntoFile( "BtnPress:"+"X,"+"Num:"+m_nDigitalCurrent +",Mode:" + m_Type_Current   , true );
			}

			// you didn't answer and cross showed up, plus crossDelayTime
			if (m_bAnswerPress == false && cross_show == true)
				crossDelayTime += deltaDelay;
			
			// set cross signal can show
			cross_random = false;
			// hide cross
			cross_show = false;

			//UpdateDigitalDisplay( false , -1);
			UpdateDigitalDisplay( false , SIGNAL.Blank);
			m_bAnswerPress = true;

			updateDelegate_num = Behavior_Num_Finish;
		}

	}

	void Behavior_Num_Finish()
	{
		//UpdateDigitalDisplay( false , -1);
		//UpdateDigitalDisplay( false , SIGNAL.Blank);
		//m_bAnswerPress = true;
	}




	// 0~1 , just change color 
	void Behaviour_Idle(){
		m_bAnswerPress = true;

		/*if (elapseTime == 0) {

			//TimerCountDownBar.value=0;

			//m_Type_Current =  (QTYPE)(  ((int)m_Type_Current + 1 )%  (int)QTYPE.COUNT);
			//m_Type_Current =  (QTYPE)m_QuestionBankList[m_QuestionCurrentCount];

			//UpdateLightDisplay( m_Type_Current   );
			//UpdateLightDisplay(   QTYPE.B );

			if (CarBillBoard_Label_Debug)
				CarBillBoard_Label_Debug.text = "color:" + m_QuestionCurrentCount + ","+  (m_Type_Current== QTYPE.A? "Black":"White")   ;

		}*/



		if(m_bTimePauseByGameTime == false) // v0.9
			elapseTime +=Time.deltaTime;


		if(  elapseTime  > 1 )
		{
			//elapseTime=0;


			m_fCurrentIdleWaitTime = Random.Range( 0.0f , 0.9f );
			//m_fCurrentIdleWaitTime = 2f;



			//			Animation Anm = CarBillBoard.gameObject.GetComponent<Animation>();
			//			Direction dir = Direction.Forward;
			//			ActiveAnimation anim0;
			//			anim0 = ActiveAnimation.Play(Anm, "Animation_Blink", dir, EnableCondition.EnableThenPlay, DisableCondition.DoNotDisable);
			//			//ActiveAnimation.Play(  Anm, AnimationOrTween.Direction.Forward);
			//			anim0.eventReceiver = gameObject;
			//			//anim0.callWhenFinished = "AnimationLoop";

			updateDelegate = Behaviour_Changing;
		}


	}


	// change the pattern
	void Behaviour_Changing(){


		// 1~2 sec
		//		if (elapseTime == 0) {			
		//			//TimerCountDownBar.value=0;
		//		}

		if(m_bTimePauseByGameTime == false) // v0.9
			elapseTime +=Time.deltaTime;


		if (elapseTime > (1+ m_fCurrentIdleWaitTime ) ) {

			m_fCurrentIdleWaitTime = 100; // stop it 

			/*if (CarBillBoard_Label_Debug_2)
				CarBillBoard_Label_Debug_2.text = "Num:"+ m_QuestionCurrentCount ;*/


			//UpdateDigitalDisplay( false , Random.Range(1,10) );
			updateDelegate_num = Behavior_Num_Start;
		}



		if (elapseTime >= 3) {

			//m_fCurrentIdleWaitTime=2;
			//UpdateLightDisplay(QTYPE.A );
			//UpdateLightDisplay(QTYPE.A );


			elapseTime=0;
			//updateDelegate=Behaviour_Changed;
			updateDelegate=Behaviour_Done;
		}

	}

	//	void Behaviour_Changed(){
	//
	//		//elapseTime +=Time.deltaTime;
	//		//TimerCountDownBar.value = 1 - elapseTime;
	//		
	//		//if (elapseTime > 2f) {
	//			updateDelegate=Behaviour_Done;
	//		//}
	//
	//	}


	void Behaviour_Done(){

		elapseTime=0;
		m_fCurrentIdleWaitTime = 0;



		if(m_QuestionCurrentCount >= m_QuestionTotal ) // finish all question
			updateDelegate=Behaviour_End;
		else
		{
			m_QuestionCurrentCount += 1;
			updateDelegate=Behaviour_Idle;
		}
	}

	// finish all the question
	void Behaviour_End(){
	}


	/*bool CheckRule_OddEven( bool bLeft   )
	{
		bool bOdd =( (m_nDigitalCurrent % 2) != 0 );

		if( bOdd )
		{
			if( bLeft) // you decide odd
				return true;
			else
				return false;
		}
		else // odd
		{
			if( bLeft) // you decide odd
				return false;
			else
				return true;
		}
	}*/


	/*bool CheckRule_Bigger( bool bLeft   )
	{


		if( m_nDigitalCurrent > 5 ) // smaller
		{
			if( bLeft) // you decide bigger
				return true;
			else
				return false;
		}
		else // odd
		{
			if( bLeft) // you decide bigger
				return false;
			else
				return true;
		}
	}*/


	bool CheckRule( bool bLeft )
	{

		if (m_nSignalCurrent == SIGNAL.Cross) {
			crossDelayTime -= deltaDelay;
			return false;
		}

		if (bLeft) {
			if (m_nSignalCurrent == SIGNAL.Circle)
				return true;
		}
		else {
			if (m_nSignalCurrent == SIGNAL.Triangle)
				return true;
		}

		return false;
	}


	public void Action_BtnLeft()
	{
		if(m_bAnswerPress) return;


		/*if( m_Type_Current == QTYPE.A)
		{
			if(  CheckRule_OddEven( true) )
			{
				// Add Score
				xMain.Instance.GetGameCore().BloodDamageUpdate( -0.1f,"L_O_OddEven");
			}
			else
			{
				// minus Score
				xMain.Instance.GetGameCore().BloodDamageUpdate( 0.1f,"L_X_OddEven");
			}
		}
		else
		{
			if(  CheckRule_Bigger( true) )
			{
				// Add Score
				xMain.Instance.GetGameCore().BloodDamageUpdate( -0.1f,"L_O_BS");
			}
			else
			{
				// minus Score
				xMain.Instance.GetGameCore().BloodDamageUpdate( 0.1f,"L_O_BS");
			}

		}*/

		if(  CheckRule(true) )
		{
			// Add Score
			xMain.Instance.GetGameCore().BloodDamageUpdate( -0.1f,"StopSignal_addBlood");
		}
		else
		{
			// minus Score
			xMain.Instance.GetGameCore().BloodDamageUpdate( 0.1f,"StopSignal_minusBlood");
		}

		// m_nSignalCurrent
		xMain.Instance.WriteLogIntoFile( "BtnPress:"+"L,"+"Signal:"+m_nSignalCurrent   , true );
		// m_nDigitalCurrent
		//xMain.Instance.WriteLogIntoFile( "BtnPress:"+"L,"+"Num:"+m_nDigitalCurrent +",Mode:" + m_Type_Current   , true );

		m_bAnswerPress = true;
		//updateDelegate_num = Behavior_Num_Finish;

	}

	public void Action_BtnRight()
	{
		if(m_bAnswerPress) return;


		/*if( m_Type_Current == QTYPE.A)
		{
			if(  CheckRule_OddEven( false) )
			{
				// Add Score
				xMain.Instance.GetGameCore().BloodDamageUpdate( -0.1f,"R_O_OddEven");
			}
			else
			{
				// minus Score
				xMain.Instance.GetGameCore().BloodDamageUpdate( 0.1f,"R_X_OddEven");
			}
		}
		else
		{
			if(  CheckRule_Bigger( false) )
			{
				// Add Score
				xMain.Instance.GetGameCore().BloodDamageUpdate( -0.1f,"R_O_BS");
			}
			else
			{
				// minus Score
				xMain.Instance.GetGameCore().BloodDamageUpdate( 0.1f,"R_X_BS");
			}

		}*/

		if(  CheckRule(false) )
		{
			// Add Score
			xMain.Instance.GetGameCore().BloodDamageUpdate( -0.1f,"StopSignal_addBlood");
		}
		else
		{
			// minus Score
			xMain.Instance.GetGameCore().BloodDamageUpdate( 0.1f,"StopSignal_minusBlood");
		}

		m_bAnswerPress = true;

		// m_nSignalCurrent
		xMain.Instance.WriteLogIntoFile( "BtnPress:"+"R,"+"Signal:"+m_nSignalCurrent   , true );
		// m_nDigitalCurrent
		//xMain.Instance.WriteLogIntoFile( "BtnPress:"+"R,"+"Num:"+m_nDigitalCurrent +",Mode:" + m_Type_Current   , true );

		//updateDelegate_num = Behavior_Num_Finish;

	}




	override public  int  GetCurrentLevel(){

		return 1;
	}

	override public  void GameStart(){

		InitSetting ();
		updateDelegate = Behaviour_Idle;

	}

	override public  void GameFinish ( bool bWinLose ){


		//xMain.Instance.WriteLogIntoFile(m_LogTag+",GameFinish-Extrascore,"+ m_Score_ExtraCredit  , true );
		if( bWinLose== true ){

			if( xMain.Instance.mRoundData.level_numguess < ( GameSetting.Instance.m_LevelList.Count -1 )  )
				xMain.Instance.mRoundData.level_numguess+=1;


			//xMain.Instance.nRoundCurrentScore_NumGuess = xMain.Instance.nRoundCurrentScore_Normal + m_Score_ExtraCredit;
			xMain.Instance.nRoundCurrentScore_NumGuess = xMain.Instance.nRoundCurrentScore_Normal ;
			// accumulate 
			xMain.Instance.mRoundData.score_numguess  += xMain.Instance.nRoundCurrentScore_NumGuess;

			xMain.Instance.RoundData_Update_Local();

		}
		else
		{

			if(xMain.Instance.m_GameLevel_AotoMode )
			if(xMain.Instance.mRoundData.level_numguess >0)
			{
				xMain.Instance.mRoundData.level_numguess-=1;
				xMain.Instance.RoundData_Update_Local();
			}


		}


		updateDelegate=null;
		updateDelegate_num=null;
	}


	void UpdateDigitalDisplay( bool bShow , int number ){

		if(number != -1)
			CarBillBoard_Label.text = ""+number;

		CarBillBoard_Label.enabled = bShow;

	}

	void UpdateDigitalDisplay( bool bShow , SIGNAL s ){

		CarBillBoard_Label.color = Color.black;

		m_nSignalCurrent = s;

		if (s == SIGNAL.Circle)
			CarBillBoard_Label.text = "O";
		else if (s == SIGNAL.Cross)
			CarBillBoard_Label.text = "X";
		else if (s == SIGNAL.Triangle)
			CarBillBoard_Label.text = "△";
		else
			CarBillBoard_Label.text = " ";

		CarBillBoard_Label.enabled = bShow;

	}


	/*void UpdateLightDisplay( QTYPE status )
	{
		m_Type_Current = status;



		if(m_Type_Current == QTYPE.A)
		{
			CarBillBoard.color = Color.black;
			CarBillBoard_Label.color = Color.white;
			//return;
			Label_LeftBtn.text = "O";
			Label_RightBtn.text = "△";
		}
		else
			if(m_Type_Current == QTYPE.B)
			{
				CarBillBoard.color = Color.white;
				CarBillBoard_Label.color = Color.black;

				Label_LeftBtn.text = "O";
				Label_RightBtn.text = "△";
			}


		//		if(number != -1)
		//		CarBillBoard_Label.text = ""+number;



		//		string sColorStr;
		//		if(m_CurrentLight == LIGHT_STATUS.RED )
		//			sColorStr="R";
		//		else
		//			sColorStr="G";
		//				
		//		xMain.Instance.WriteLogIntoFile(m_LogTag+"["+ (  cTimesTotal - mTimesTotal ) +  "]:"+sColorStr   + "," + xMain.Instance.GetGameCore().m_PathGenerator.m_sCurrentSegmentName , true );
	}*/





	void Update () {
		//UpdateCube();

		if(bTestModeStart == true)
		{
			bTestModeStart=false;
			//GameStart();

			//Init_QuestionOrder();
		}

		if(updateDelegate != null)
		{
			updateDelegate(); // function pointer
		}		


		if(updateDelegate_num != null)
		{
			updateDelegate_num(); // function pointer
		}		


		#if UNITY_EDITOR
		if (Input.GetKeyDown("1"))
			Action_BtnLeft();
		else
			if (Input.GetKeyDown("2"))
				Action_BtnRight();
		#endif
	}



}
