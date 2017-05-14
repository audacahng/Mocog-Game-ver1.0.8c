using UnityEngine;
using System.Collections;

public class GameMode_Memory : GameMode {



	enum MODE_NEMTYPE{
		MODE_NEM_DIGIT,
		MODE_NEM_SHAPE,
		MODE_NEM_COLOR,
		MODE_NEM_MIX,
		MODE_COUNT
	};
	MODE_NEMTYPE m_CurrentMemoryType = MODE_NEMTYPE.MODE_NEM_DIGIT;

	enum MODE_PATTERN{
		PTN_01,
		PTN_02,
		PTN_03,
		PTN_04,
		PTN_05,
		PTN_06,
		PATTERN_COUNT
	};



	//const int const_N_ToNext = 3; // when you answer in 3 times continuely , N will be N+1
	const int const_N_ToNext = 2; // when you answer in 3 times continuely , N will be N+1
	int N_acc_Success =0; // record it continuesly	

	
	const int const_N_ToNext_fail = 2; // when you answer in 2 times continuely with wrong answer, N will be N-1
	int N_acc_fail =0; // record it continuesly	

	
	int N =1;

	int m_Count=0;// Current Array Size - 


	enum COMPARE{
		CMP_SKIP,  // don't satisfy the N , so skip it 
		CMP_FALSE, // compare in N, the N is different
		CMP_TRUE  // compare in N , the N is the same, ClickInTime ? Correct: Wrong
	}
	COMPARE m_CMP_Current = COMPARE.CMP_SKIP;


	ArrayList m_ArrQList = new ArrayList(); 

	int nArrIndex_Start=0;  // N - Slide Window
	int nArrIndex_last = 0; //nArrIndex_Start + N;

	COMPARE check_N(){

		Debug.Log("check_N:" +"S="+nArrIndex_Start+",L="+nArrIndex_last+",A.C=" +m_Count);

		if( m_Count <= nArrIndex_last )
			return COMPARE.CMP_SKIP;



		if((int)m_ArrQList[ nArrIndex_Start ] == (int)m_ArrQList[ nArrIndex_last ] ) {
			Debug.Log("check_N:==" +"A[S]="+(int)m_ArrQList[ nArrIndex_Start ]+",A[L]="+(int)m_ArrQList[ nArrIndex_last ] );
			return COMPARE.CMP_TRUE;
		}
		else
		{
			Debug.Log("check_N:!=" +"A[S]="+(int)m_ArrQList[ nArrIndex_Start ]+",A[L]="+(int)m_ArrQList[ nArrIndex_last ] );
			return COMPARE.CMP_FALSE;
		}
	}

	void move_N_Window(){
		if( m_Count < nArrIndex_last) return; // don't slide the window

		if( m_Count == (nArrIndex_last + 1 )){
			nArrIndex_Start += 1;
			update_IndexLast();
		}
	}
	
	void change_N_Window_Size(){

		Debug.Log("change_N_Window_Size-a:"+ nArrIndex_Start+","+ nArrIndex_last);

		nArrIndex_Start = nArrIndex_last;
		update_IndexLast();

		Debug.Log("change_N_Window_Size-b:"+ nArrIndex_Start+","+ nArrIndex_last);
	}

	private void update_IndexLast(){
		nArrIndex_last = nArrIndex_Start + N;
	}

	// time 
	const float m_TimeToShowQuestion = 1.7f ;
	const float m_TimeToInterval = 0.3f;
	
	
	string m_LogTag = "[Mode_MEM]";



	/// <summary>
	/// UI Part
	/// </summary>
	public GameObject ReactiveButton_Memory;


	public UISprite CarBillBoard;
	//public UISprite CarBillBoard_Pattern;
	public UILabel CarBillBoard_Label; // number show 

	public UILabel CarBillBoard_Label_Debug; // number show 
	public UILabel CarBillBoard_Label_Debug_2; // number show 

	public UISprite CarBillBoard_Pattern;

	public GameObject ReactiveButton_ModeSelect;
	public GameObject ReactiveButton_ModeSelect_DIGIT;
	public GameObject ReactiveButton_ModeSelect_SHAPE;
	public GameObject ReactiveButton_ModeSelect_COLOR;

	
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






	void InitUI(){

		ReactiveButton_ModeSelect.SetActive (true);
		ReactiveButton_Memory.SetActive (false);
		CarBillBoard.gameObject.SetActive (false);
		CarBillBoard_Label.gameObject.SetActive (false);

		CarBillBoard_Pattern.gameObject.SetActive(false);

		CarBillBoard_Label_Debug.gameObject.SetActive (true);
		CarBillBoard_Label_Debug_2.gameObject.SetActive (true);
	}

	void InitUI_GameStart(){

		ReactiveButton_ModeSelect.SetActive (false);
		ReactiveButton_Memory.SetActive (true);
		CarBillBoard.gameObject.SetActive (true);
		CarBillBoard_Label.gameObject.SetActive (true);

	}


	void UI_ShowHideQuestion(bool bShow ){
		if (bShow == true) 
		{
			CarBillBoard.gameObject.SetActive (true);
		}
		else
		{
			CarBillBoard.gameObject.SetActive (false);
		}
		
	}

	override public  void InitSetting (){
	}



	void Start()
	{
		InitUI();
	}

	
	override public  int  GetCurrentLevel(){
		return 1;
	}
	
	override public  void GameStart(){

		InitUI_GameStart ();


// Add the pre set here
//		m_ArrQList.Add( MODE_PATTERN.PTN_01);
//		m_ArrQList.Add( MODE_PATTERN.PTN_02);
//		m_ArrQList.Add( MODE_PATTERN.PTN_03);



		xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",GameType,"+m_CurrentMemoryType  , true );
		updateDelegate = Behaviour_Start;

		//CarBillBoard_Label_Debug_2.text = "N=" + N;
		UpdateDebugLog ("", "N=" + N );

		m_ArrQList.Clear ();	
		update_IndexLast();
	}

	//void UpdatePatternDisplay(MODE_NEMTYPE mMemoryType , MODE_PATTERN mPattern)
	void UpdatePatternDisplay(MODE_NEMTYPE mMemoryType , MODE_PATTERN mPattern, bool bDisplayPattern=true){
		//mM
		if (mMemoryType == MODE_NEMTYPE.MODE_NEM_DIGIT) {

			CarBillBoard_Label.gameObject.SetActive(bDisplayPattern);


			if(mPattern == MODE_PATTERN.PTN_01)
				CarBillBoard_Label.text = "1";
			else
			if(mPattern == MODE_PATTERN.PTN_02)
				CarBillBoard_Label.text = "2";
			else
			if(mPattern == MODE_PATTERN.PTN_03)
				CarBillBoard_Label.text = "3";
			else
			if(mPattern == MODE_PATTERN.PTN_04)
				CarBillBoard_Label.text = "4";
			else
			if(mPattern == MODE_PATTERN.PTN_05)
				CarBillBoard_Label.text = "6";
			else
			if(mPattern == MODE_PATTERN.PTN_06)
				CarBillBoard_Label.text = "8";

			xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",Pattern,"+CarBillBoard_Label.text  , true );
		}
		else
		if (mMemoryType == MODE_NEMTYPE.MODE_NEM_COLOR) {

			CarBillBoard.gameObject.SetActive(bDisplayPattern);

			if(mPattern == MODE_PATTERN.PTN_01 || mPattern == MODE_PATTERN.PTN_02)
				CarBillBoard.color = Color.black;
			else
			if(mPattern == MODE_PATTERN.PTN_03 || mPattern == MODE_PATTERN.PTN_04)
					CarBillBoard.color = Color.white;
			else
			if(mPattern == MODE_PATTERN.PTN_05 || mPattern == MODE_PATTERN.PTN_06)
					CarBillBoard.color = Color.gray;

			xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",Pattern,"+CarBillBoard.color  , true );

		}
		else
		if (mMemoryType == MODE_NEMTYPE.MODE_NEM_SHAPE) {
			CarBillBoard_Pattern.gameObject.SetActive(bDisplayPattern);
			//CarBillBoard_Pattern.UpdateVisibility

			if(mPattern == MODE_PATTERN.PTN_01)
				CarBillBoard_Pattern.spriteName = "Ptn01";
				
			else
				if(mPattern == MODE_PATTERN.PTN_02)
				CarBillBoard_Pattern.spriteName = "Ptn02";
			else
				if(mPattern == MODE_PATTERN.PTN_03)
				CarBillBoard_Pattern.spriteName = "Ptn03";
			else
				if(mPattern == MODE_PATTERN.PTN_04)
				CarBillBoard_Pattern.spriteName = "Ptn04";
			else
				if(mPattern == MODE_PATTERN.PTN_05)
				CarBillBoard_Pattern.spriteName = "Ptn05";
			else
				if(mPattern == MODE_PATTERN.PTN_06)
				CarBillBoard_Pattern.spriteName = "Ptn06";


		}





	}


	void UpdateDebugLog(string info01="" , string infor02=""){
		if (info01 != "")
		{
			//CarBillBoard_Label_Debug.text = info01;
			CarBillBoard_Label_Debug.text = "";
		}
		
		if (infor02 != "")
		{
			CarBillBoard_Label_Debug_2.text = infor02;
		}
	}


	void Behaviour_Start(){

		elapseTime = 0;

		UI_ShowHideQuestion(true);



		//MODE_PATTERN.PATTERN_COUNT

		MODE_PATTERN Pattern_Current = (MODE_PATTERN) Random.Range ( 0 , 6);  // 0~5

		if(bTestModeStart) // debug mode
			Pattern_Current = (MODE_PATTERN) Random.Range ( 0 , 2);  // 0~5

		m_ArrQList.Add( Pattern_Current) ;
		m_Count+=1; // array element 



		//Debug.Log ( "m_Pattern_Current= "+ Pattern_Current);
		Debug.Log ( "m_Pattern_Current= "+ m_ArrQList[m_Count-1]   );
		//UpdatePatternDisplay (m_CurrentMemoryType, Pattern_Current);
		UpdatePatternDisplay (m_CurrentMemoryType,   (MODE_PATTERN)m_ArrQList[m_Count-1] ,false );


		m_CMP_Current = check_N(); // to check the Current Window
		Debug.Log(m_CMP_Current);


		if(nArrIndex_last < m_Count )
			//UpdateDebugLog("","N="+N+",CMP="+m_CMP_Current+"," +m_ArrQList[ nArrIndex_Start] +"=="+m_ArrQList[ nArrIndex_last]+ "?" );
			UpdateDebugLog("",""+N );
		else
			//UpdateDebugLog("","N="+N+",CMP="+m_CMP_Current+"," +m_ArrQList[ nArrIndex_Start]  );
			UpdateDebugLog("",""+N );

		updateDelegate = Behaviour_DisplayQuestion;
	}


	void Behaviour_DisplayQuestion(){

		if(m_bTimePauseByGameTime == false) // v0.9
			elapseTime +=Time.deltaTime;


		if(elapseTime >=1.0)
			UpdatePatternDisplay (m_CurrentMemoryType,   (MODE_PATTERN)m_ArrQList[m_Count-1]);



		if(elapseTime > m_TimeToShowQuestion)
		{
			elapseTime=0;

			//To check if this is the answer  
			checkPlayerDecision(m_CMP_Current, false); // player didn't press the button

			// checkPoint-1 : Time is up
			updateDelegate = Behaviour_DisplayQuestion_Wait;

		}

	}

	void Behaviour_DisplayQuestion_Wait(){

		if(elapseTime==0){
			UI_ShowHideQuestion(false);
			move_N_Window();
		}

		if(m_bTimePauseByGameTime == false) // v0.9
			elapseTime +=Time.deltaTime;
		
		if(elapseTime > m_TimeToShowQuestion)
		{
			elapseTime=0;

			updateDelegate = Behaviour_Start;

		}		

	}

	void Behaviour_Finish(){
	}

	


	
	override public  void GameFinish ( bool bWinLose ){

		if(bWinLose== true  ){
			
			if( xMain.Instance.mRoundData.level_memory < ( GameSetting.Instance.m_LevelList.Count -1 )  )
				xMain.Instance.mRoundData.level_memory+=1;
			
			
			//xMain.Instance.nRoundCurrentScore_NumGuess = xMain.Instance.nRoundCurrentScore_Normal + m_Score_ExtraCredit;
			xMain.Instance.nRoundCurrentScore_Memory = xMain.Instance.nRoundCurrentScore_Normal ;
			// accumulate 
			xMain.Instance.mRoundData.score_memory  += xMain.Instance.nRoundCurrentScore_Memory;
			
			xMain.Instance.RoundData_Update_Local();
			
		}
		else
		{
			
			if(xMain.Instance.m_GameLevel_AotoMode )
				if(xMain.Instance.mRoundData.level_memory >0)
			{
				xMain.Instance.mRoundData.level_memory-=1;
				xMain.Instance.RoundData_Update_Local();
			}
			
			
		}


		updateDelegate = null;


	}


	public void BTNAction_ModeSelect_01(){
		m_CurrentMemoryType = MODE_NEMTYPE.MODE_NEM_DIGIT;
		CarBillBoard_Label_Debug.text = "數字";
	}

	public void BTNAction_ModeSelect_02(){
		m_CurrentMemoryType = MODE_NEMTYPE.MODE_NEM_SHAPE;
		CarBillBoard_Label_Debug.text = "形狀";

		CarBillBoard_Pattern.gameObject.SetActive(true);
	}

	public void BTNAction_ModeSelect_03(){
		m_CurrentMemoryType = MODE_NEMTYPE.MODE_NEM_COLOR;
		CarBillBoard_Label_Debug.text = "顏色";
	}


	void rule_failPunishment(){

		if( N >1)
			N-=1;

		change_N_Window_Size();

		//N_acc_Success = 0 ;
		//N_acc_fail = 0 ;

		//N_acc = 0 ; // reset 
	}


	void rule_SuccessBonus(){

		xMain.Instance.GetGameCore().BloodDamageUpdate( -0.1f,"RightAnswer");
		xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",success,N="+N, true );

	}


	bool IsPatternMatch(MODE_PATTERN pattern_Current, MODE_PATTERN  pattern_Target){ 

		if (m_CurrentMemoryType != MODE_NEMTYPE.MODE_NEM_COLOR )
		{
			if(pattern_Current == pattern_Target) return true;
		}
		else{

			if(  (pattern_Current == MODE_PATTERN.PTN_01 || pattern_Current == MODE_PATTERN.PTN_02) && (pattern_Target == MODE_PATTERN.PTN_01 || pattern_Target == MODE_PATTERN.PTN_02))
				return true;

			if(  (pattern_Current == MODE_PATTERN.PTN_03 || pattern_Current == MODE_PATTERN.PTN_04) && (pattern_Target == MODE_PATTERN.PTN_03 || pattern_Target == MODE_PATTERN.PTN_04))
				return true;

			if(  (pattern_Current == MODE_PATTERN.PTN_05 || pattern_Current == MODE_PATTERN.PTN_06) && (pattern_Target == MODE_PATTERN.PTN_05 || pattern_Target == MODE_PATTERN.PTN_06))
				return true;

		}

		return false;
	}

	void checkPlayerDecision(COMPARE nCmp,  bool bPress  ){

		Debug.Log("checkPlayerDecision:"+ nCmp+","+ bPress);

		//Compare True
		//	Press: correct , reset windows
		//	Didn't Press: wrong , reset
		//Comapre False , Compare Skip
		//	Press: wrong
		//	Didn't Press: Do nothing,


		if( nCmp == COMPARE.CMP_TRUE  ) // when the pattern is correct
		{ 
			if(bPress)
			{ 
				// correct , Accumunalate the success N
				N_acc_Success += 1;
				N_acc_fail=0;

				if( N_acc_Success == const_N_ToNext )  // success 3  times , N will +1
				{
					// N +1 ;
					N_acc_Success = 0;
					N_acc_fail = 0;
					N+=1;
					change_N_Window_Size();

					rule_SuccessBonus();
					//UpdateDebugLog("N+1! N="+ N  );
					UpdateDebugLog(""+ N  );
				}
					//N_acc = 0 ; // reset 
			}
			else  // player didn't press it when it's in N , so it result in fail
			{
					N_acc_fail +=1;
					N_acc_Success = 0;
					
					xMain.Instance.GetGameCore().BloodDamageUpdate( 0.05f,"Fail_NoPress");

					if( N_acc_fail == const_N_ToNext_fail)
					{
						N_acc_Success=0;
						N_acc_fail=0;
						rule_failPunishment();
					}
					xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",fail,"+"didnt press,N="+N  , true );

			}
		}
		else 
		{
			if(bPress)// wrong press 
			{

				N_acc_fail +=1;
				N_acc_Success=0;

				xMain.Instance.GetGameCore().BloodDamageUpdate( 0.05f,"WrongPress");

				if( N_acc_fail == const_N_ToNext_fail)
				{
					N_acc_Success=0;
					N_acc_fail=0;
					rule_failPunishment();	
				}
				/// Log
				if( nCmp == COMPARE.CMP_FALSE)
					xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",fail,"+"wrong pattern,N="+N , true );
				else
					xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",fail,"+"wrong N,N="+N , true );
			}
		}

		if(nCmp != COMPARE.CMP_SKIP)
		{
			if(nArrIndex_last < m_Count )
				//UpdateDebugLog("A[S]="+ m_ArrQList[ nArrIndex_Start] +", A[L]="+m_ArrQList[ nArrIndex_last]+",\n N="+ N  +",S="+nArrIndex_Start+",L="+ nArrIndex_last+"," +m_CMP_Current+",N_acc="+N_acc_Success );
				Debug.Log("A[S]="+ m_ArrQList[ nArrIndex_Start] +", A[L]="+m_ArrQList[ nArrIndex_last]+",\n N="+ N  +",S="+nArrIndex_Start+",L="+ nArrIndex_last+"," +m_CMP_Current+",N_acc="+N_acc_Success );
			else
				//UpdateDebugLog("A[S]="+ m_ArrQList[ nArrIndex_Start] +",\n N="+ N  +",S="+nArrIndex_Start+",L="+ nArrIndex_last +","+ m_CMP_Current+",N_acc="+N_acc_Success );
				Debug.Log("A[S]="+ m_ArrQList[ nArrIndex_Start] +",\n N="+ N  +",S="+nArrIndex_Start+",L="+ nArrIndex_last +","+ m_CMP_Current+",N_acc="+N_acc_Success );
		}
		else
			//UpdateDebugLog("SKIP:"+nArrIndex_Start+","+nArrIndex_last + "");
			Debug.Log("SKIP:"+nArrIndex_Start+","+nArrIndex_last + "");




	}


	public void BTNAction_PressToAnswer(){


		xMain.Instance.WriteLogIntoFile(   m_LogTag+ ",BtnPress"  , true );

		if (updateDelegate == Behaviour_DisplayQuestion)
		{
			Debug.Log("BTNAction_PressToAnswer:" +"S="+nArrIndex_last+",L=" + nArrIndex_last + ",Arr.C="+ m_Count );

			checkPlayerDecision( m_CMP_Current, true);
			elapseTime=0;
			updateDelegate = Behaviour_DisplayQuestion_Wait;
		}
	}



	// Update is called once per frame
	void Update () {
	
		
//		if(bTestModeStart == true)
//		{
//			bTestModeStart=false;
//			GameStart();
//		}
		
		if(updateDelegate != null)
		{
			updateDelegate(); // function pointer
		}		
		
	}


}
