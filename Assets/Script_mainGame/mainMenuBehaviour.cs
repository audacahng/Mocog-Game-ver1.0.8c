using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;
using System.Threading.Tasks;

public class mainMenuBehaviour : MonoBehaviour {


	public GameObject UI_LevelSelectPanel;

	public GameObject UI_ScoreBoard;

	public GameObject UI_Control_BtnScoreBoard;
	public GameObject UI_main_LogOutBtn;

	public UILabel m_CurrentLogInUser;

	public GameObject  Controller_GameModeSelect;

	/// User Panel - start
	public GameObject  Control_UserInfoPanel;
	public GameObject Control_UserInfoPanel_ShowFunc;
	public GameObject Control_UserInfoPanel_LogIn;


	public GameObject Control_LogUploadPanel;
	public UILabel Control_LogUploadPanel_Status;
	public UIButton Control_LogUploadPanel_Button;



	// LogOut
	public GameObject Control_UserInfoPanel_LogOutVarify; // for log out confirm
	public UIInput Control_UserInfoPanel_LogOutVarify_Passwd;
	public UIButton Control_UserInfoPanel_LogOutVarify_LogOutBtn;
	public UILabel  Control_UserInfoPanel_LogOutVarify_WarningInfo;

	// Sign Up
	public GameObject Control_UserInfoPanel_SignUp;
	public UIInput 	Control_UserInfoPanel_SignUp_EMail;
	public UIInput Control_UserInfoPanel_SignUp_Passwd;
	public UIPopupList  Control_UserInfoPanel_SignUp_Gender;
	public UILabel  Control_UserInfoPanel_SignUp_WarningInfo;

	public UIPopupList  Control_UserInfoPanel_SignUp_Education;
	public UIInput 	Control_UserInfoPanel_SignUp_Age;

	public UIButton Control_UserInfoPanel_SignUp_FuncBtn;



	public UIInput Control_UserInfoPanel_LogIn_EMail;
	public UIInput Control_UserInfoPanel_LogIn_Passwd;
	public UILabel  Control_UserInfoPanel_LogIn_WarningInfo;

	public UIButton Control_UserInfoPanel_LogIn_FuncBtn;

	/// User Panel - end
	/// 
//	public bool m_GameLevel_AotoMode = true; // V2 : 



	// Use this for initialization
	void Start () {
	
		xMain.Instance.WriteLogIntoFile("LogStart,");
		xMain.Instance.WriteLogIntoFile("device id,"+SystemInfo.deviceUniqueIdentifier);

		UI_LevelSelectPanel.SetActive(true);
		UI_LevelSelectPanel.SetActive(false);

		UI_ScoreBoard.SetActive(false);

		Control_LogUploadPanel.SetActive(false);

		CheckExistUser();
		//StartCoroutine( xMain.Instance.QueryTest() );
	}


	public void CheckExistUser()
	{
		Debug.Log("CheckExistUser");
		if(  ParseUser.CurrentUser == null)
		{
			if(xMain.Instance.m_bGuestPlayerMode == false)
			{
				// Show Panel: Sign Up and LogIn button
				Controller_GameModeSelect.SetActive(false);
				Control_UserInfoPanel.SetActive(true);
				Control_UserInfoPanel_ShowFunc.SetActive(true);
				
				Debug.Log("ParseUser.CurrentUser == null");

			}
			else{
				UI_Control_BtnScoreBoard.SetActive(false);
				UI_main_LogOutBtn.SetActive(false);
				m_CurrentLogInUser.text= "guest";
			}
		}
		else
		{
			xMain.Instance.m_GameUserID =ParseUser.CurrentUser.Username ;
			xMain.Instance.SetPlayerID(  ParseUser.CurrentUser.Username );

			Controller_GameModeSelect.SetActive(true);
			Control_UserInfoPanel.SetActive(false);
			Control_UserInfoPanel_ShowFunc.SetActive(false);

			//m_CurrentLogInUser.text = ParseUser.CurrentUser.Email;
			m_CurrentLogInUser.text = ParseUser.CurrentUser.Username;

			// Hide Panel , go into main menu , buffer user name and show it on the top-right side.
			Debug.Log("ParseUser.CurrentUser Exist:"+ParseUser.CurrentUser.Username);


			StartCoroutine (  xMain.Instance.QueryTest() );


			Check_LogUploadStart();
		}

		Control_UserInfoPanel_SignUp.SetActive(false);
		Control_UserInfoPanel_LogIn.SetActive(false);

		// Parse
		//Control_LogUploadPanel

	}


	 void Check_LogUploadStart()
	{
		Debug.Log("Check_LogUploadStart");

		int nCount = xMain.Instance.SQLite_TableContentCount("logrecord"  ) ;
		//if(   nCount >2 )
		if(   nCount >20000 )  // 
		{
			if(xMain.CheckForInternetConnection() == true )
			{
				LogUploadStart();
			}
			else
			{
				Control_LogUploadPanel.SetActive(true);
				Control_LogUploadPanel_Status.text = "手機上的Log數量大於" +nCount + ",請點擊上傳,並確保網路可以連線";
			}
		}
	}

	public void LogUploadStart()
	{
		if(Control_LogUploadPanel_Button)
			Control_LogUploadPanel_Button.gameObject.SetActive(false);

		if(Control_LogUploadPanel_Status)
			Control_LogUploadPanel_Status.text = "上傳中";

		if ( xMain.Instance.WriteLogIntoFile_UploadToServer() ==false) 
		{
			Control_LogUploadPanel_Status.text = "請連上網路後再試一次";
			Control_LogUploadPanel_Button.gameObject.SetActive(true);
		}
		else
		{
			Control_LogUploadPanel_Status.text = "Success";
			Control_LogUploadPanel_Button.gameObject.SetActive(true);
			Control_LogUploadPanel.SetActive(false);
		}

	}


	public void ParseUser_ShowPanel_SignUp()
	{


		/// show all the detail panel
		if(  Control_UserInfoPanel_SignUp.activeSelf == false)
		{
			Control_UserInfoPanel_ShowFunc.SetActive(false);
			Control_UserInfoPanel_SignUp.SetActive(true);
		}
		else
		{
			Control_UserInfoPanel_ShowFunc.SetActive(true);
			Control_UserInfoPanel_SignUp.SetActive(false);
		}


	}


	public void ParseUser_ShowPanel_LogIn()
	{
		//ParseUser.LogOut();
		if( Control_UserInfoPanel_LogIn.activeSelf == false)
		{
			Control_UserInfoPanel_ShowFunc.SetActive(false);
			Control_UserInfoPanel_LogIn.SetActive(true);
		}
		else
		{
			Control_UserInfoPanel_ShowFunc.SetActive(true);
			Control_UserInfoPanel_LogIn.SetActive(false);
		}

		//var currentUser = ParseUser.CurrentUser; // this will now be null

	}

	public void ParseUser_SignUp()
	{
		if(Control_UserInfoPanel_SignUp_EMail.label.text =="Press here to type") Control_UserInfoPanel_SignUp_EMail.label.text ="";
		if(Control_UserInfoPanel_SignUp_Passwd.label.text =="Press here to type") Control_UserInfoPanel_SignUp_Passwd.label.text ="";

		if(Control_UserInfoPanel_SignUp_EMail.label.text =="" || Control_UserInfoPanel_SignUp_Passwd.label.text=="")
		{
			Control_UserInfoPanel_SignUp_WarningInfo.text="請正確輸入";
		}
		else
		{
			Control_UserInfoPanel_SignUp_FuncBtn.enabled = false;
			StartCoroutine(   Async_SignUpTask() );
		}


	}

	IEnumerator Async_SignUpTask()
	{

		var user = new ParseUser()
		{
			Username = Control_UserInfoPanel_SignUp_EMail.label.text,
			//Email = Control_UserInfoPanel_SignUp_EMail.label.text,
			Password = Control_UserInfoPanel_SignUp_Passwd.label.text,
		};
		
		// other fields can be set just like with ParseObject
		user["gender"] = Control_UserInfoPanel_SignUp_Gender.value;

		user["Age"] =int.Parse( Control_UserInfoPanel_SignUp_Age.label.text );
		user["Education"] =Control_UserInfoPanel_SignUp_Education.value;

		//public UIPopupList  Control_UserInfoPanel_SignUp_Education;
		//public UIInput 	Control_UserInfoPanel_SignUp_Age;

		Control_UserInfoPanel_SignUp_WarningInfo.text="註冊中..";

		Task signUpTask = user.SignUpAsync();

		while(! signUpTask.IsCompleted     ) yield return null;


		Control_UserInfoPanel_SignUp_FuncBtn.enabled = true;

		if(  signUpTask.IsFaulted )
		{
			Debug.Log("Sign Up Fail");
			Control_UserInfoPanel_SignUp_WarningInfo.text="註冊失敗";

		}
		else
		if(  signUpTask.IsCanceled)
		{
			Debug.Log("Sign Up Cancel");
			Control_UserInfoPanel_SignUp_WarningInfo.text="Cancel!";
		}
		else
		if(  signUpTask.IsCompleted)
		{
			Debug.Log("Success");
			Control_UserInfoPanel_SignUp_WarningInfo.text="Success!";

			//if(  SQLite_TableContentCount("roundlog"  ) ==0 )
			//SQLite_InsertDefault( "roundlog");
			xMain.Instance.SQLite_InsertRoundWithUserid( Control_UserInfoPanel_SignUp_EMail.label.text );
		


			CheckExistUser();
		}



	}





	public void ParseUser_LogIn()
	{
		Debug.Log("LogIn...");


		//Press here to type
		if(Control_UserInfoPanel_LogIn_EMail.label.text =="Press here to type") Control_UserInfoPanel_LogIn_EMail.label.text ="";
		if(Control_UserInfoPanel_LogIn_Passwd.label.text =="Press here to type") Control_UserInfoPanel_LogIn_Passwd.label.text ="";
		
		if(Control_UserInfoPanel_LogIn_EMail.label.text =="" || Control_UserInfoPanel_SignUp_Passwd.label.text=="")
		{
			Control_UserInfoPanel_LogIn_WarningInfo.text="請正確輸入";
		}
		else
		{
			Control_UserInfoPanel_LogIn_FuncBtn.enabled = false;
			Control_UserInfoPanel_LogIn_WarningInfo.text="登入中";
			StartCoroutine(   Async_LogIn() );
		}
		//Async_LogIn();
	}


	public void ParseUser_LogIn_guestPlay()
	{
		Debug.Log("LogIn...");
		//Control_UserInfoPanel_LogIn_FuncBtn.enabled = false;

		xMain.Instance.m_bGuestPlayerMode = true;
		UI_ScoreBoard.SetActive(false);
		UI_Control_BtnScoreBoard.SetActive(false);

		//Control_UserInfoPanel_SignUp_EMail.label.text = "guest";
		m_CurrentLogInUser.text= "guest";
		//UI_main_LogOutBtn.SetActive(false);


		//StartCoroutine(   Async_LogIn() );
		//Async_LogIn();

		Controller_GameModeSelect.SetActive(true);
		Control_UserInfoPanel.SetActive(false);
		Control_UserInfoPanel_ShowFunc.SetActive(false);
		
		Control_UserInfoPanel_SignUp.SetActive(false);
		Control_UserInfoPanel_LogIn.SetActive(false);



	}


	IEnumerator Async_LogIn()
	{
		//Control_UserInfoPanel_LogIn_EMail.label.text;
		//Control_UserInfoPanel_LogIn_Passwd.label.text;
		//Control_UserInfoPanel_LogIn_WarningInfo.text;



		Debug.Log("LogInAsync: "+Control_UserInfoPanel_LogIn_EMail.label.text+","+Control_UserInfoPanel_LogIn_Passwd.label.text);

		string str_emal = Control_UserInfoPanel_LogIn_EMail.label.text;
		string str_password = (string)Control_UserInfoPanel_LogIn_Passwd.label.text ;
		Debug.Log("LogInAsync: convert-1");


		Task AsyncLogIn = ParseUser.LogInAsync(str_emal   , str_password );
		while( !AsyncLogIn.IsCompleted ) yield return null;

		Control_UserInfoPanel_LogIn_FuncBtn.enabled = true;

		if (AsyncLogIn.IsFaulted || AsyncLogIn.IsCanceled)
		{
			Debug.Log("LogIn Fail");
			Control_UserInfoPanel_LogIn_WarningInfo.text="請檢查帳號密碼";
		}
		else
		{
			Debug.Log("LogIn Success");
			Control_UserInfoPanel_LogIn_WarningInfo.text="";
			CheckExistUser();
		}

//		// str_emal   , str_password
//		ParseUser.LogInAsync(str_emal   , str_password ).
//			ContinueWith(t =>
//			{
//			if (t.IsFaulted || t.IsCanceled)
//			{
//				Debug.Log("LogIn Fail");
//				Control_UserInfoPanel_LogIn_WarningInfo.text="Fail!";
//			}
//			else
//			{
//				Debug.Log("LogIn Success");
//				Control_UserInfoPanel_LogIn_WarningInfo.text="Success!";
//				CheckExistUser();
//			}
//			}
//			);

	}



		

	
	
	IEnumerator Async_LogOut()
	{
		//Control_UserInfoPanel_LogIn_EMail.label.text;
		//Control_UserInfoPanel_LogIn_Passwd.label.text;
		//Control_UserInfoPanel_LogIn_WarningInfo.text;
		
		
		Task LogInTask =  ParseUser.LogInAsync(ParseUser.CurrentUser.Username   , Control_UserInfoPanel_LogOutVarify_Passwd.label.text );
		
		Debug.Log("[LogOut] LogInAsync: "+ParseUser.CurrentUser.Username+","+Control_UserInfoPanel_LogOutVarify_Passwd.label.text);
		
		while(! LogInTask.IsCompleted      ) yield return null;
		
		Debug.Log("[LogOut] LogIn...Result is ..."+ LogInTask.IsCompleted+"," +LogInTask.IsCanceled+","+ LogInTask.IsFaulted  );
		//Control_UserInfoPanel_LogIn_FuncBtn.enabled = true;

		Control_UserInfoPanel_LogOutVarify_LogOutBtn.gameObject.SetActive(true);

		if(  LogInTask.IsFaulted )
		{
			Debug.Log("LogIn Fail");
			Control_UserInfoPanel_LogOutVarify_WarningInfo.text = "登出失敗";
			Control_UserInfoPanel_LogIn_WarningInfo.text="Fail!";
		}
		else
			if(  LogInTask.IsCompleted)
		{
			LogOutProcess();
		}
		
		
	}


	void LogOutProcess()
	{
		Control_UserInfoPanel.SetActive(false);
		Control_UserInfoPanel_LogOutVarify.SetActive(false);
		Control_UserInfoPanel_LogOutVarify_WarningInfo.text = "";
		
		Debug.Log("LogIn Success, start to [LogOut]");
		Control_UserInfoPanel_LogIn_WarningInfo.text="";
		Control_UserInfoPanel_LogIn_EMail.label.text="";
		Control_UserInfoPanel_LogIn_Passwd.label.text="";

		//ParseUser.LogOut(); #somehow this crashes unity when work with the self-hosted parse server
		//https://github.com/ParsePlatform/Parse-SDK-dotNET/issues/140 (use LogOutAsync as a temp workaround)
		//ParseUser.LogOutAsync();
		ParseUser_LogOut();
		
		
		xMain.Instance.mRoundData.level_normal=0;
		xMain.Instance.mRoundData.level_redgreen=0;
		xMain.Instance.mRoundData.score_normal=0;
		xMain.Instance.mRoundData.score_redgreen=0;
		xMain.Instance.mRoundData.score_total=0;
		xMain.Instance.mRoundData.userid="";
		
		
		CheckExistUser();
	}

	public void QuitApp()
	{
		Application.Quit();
	}
	public void ParseUser_LogOutVarifyPanel_Close()
	{
		Control_UserInfoPanel.SetActive(false);
		Control_UserInfoPanel_LogOutVarify.SetActive(false);
	}

	public void ParseUser_LogOutVarifyPanel()
	{


//		#if  UNITY_IOS
//
//			//ParseUser.CurrentUser = null;
//			LogOutProcess();
//		#else
//			Control_UserInfoPanel.SetActive(true);
//			Control_UserInfoPanel_LogOutVarify.SetActive(true);
//		#endif

		xMain.Instance.m_bGuestPlayerMode= false;
		LogOutProcess();

		//xMain.Instance.m_bGuestPlayerMode
		//m_bGuestPlayerMode



	}

	public void ParseUser_LogOut()
	{

		// enter the password before LogOut
		//ParseUser.LogOut();
		//CheckExistUser();



		Control_UserInfoPanel_LogOutVarify_LogOutBtn.gameObject.SetActive(false);
		Control_UserInfoPanel_LogOutVarify_WarningInfo.text = "登出中";
		////ParseUser.Password;
		//ParseUser.CurrentUser.

		StartCoroutine(   Async_LogOut() );

	}


	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
		
		#if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.Escape))
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}


	public void RunTheGame()
	{

		Application.LoadLevel("drivingScene");
	}


	public void ModeSelect_Normal()
	{
		xMain.Instance.m_CurrentGameMode = GAMEMODE.MODE_NORMAL;

		RunGame ();
	}

	void RunGame()
	{
		if (xMain.Instance.m_GameLevel_AotoMode == true) {
			
			Debug.Log(" Enter the level:" + xMain.Instance.mRoundData.level_normal);
			RunTheGame();
			return;
		}

		UI_LevelSelectPanel.SetActive(true);
		UI_LevelSelectPanel.GetComponent< Controller_UILevelSelectPanel>().UpdateCurrentLevel();
	}


	public void ModeSelect_RedGreenLight()
	{
		xMain.Instance.m_CurrentGameMode = GAMEMODE.MODE_GREEREDLIGHT;

		RunGame ();
	}


	public void ModeSelect_DecideDigital()
	{
		xMain.Instance.m_CurrentGameMode = GAMEMODE.MODE_DECIDE_DIGITAL;
		RunGame ();
	}
	
	public void ModeSelect_Memory()
	{
		xMain.Instance.m_CurrentGameMode = GAMEMODE.MODE_MEMORY;
		RunGame ();
	}




	public void ShowScoreBoard()
	{
		//WriteLogIntoFile_UploadToServer
		UI_ScoreBoard.SetActive(true);
		UI_ScoreBoard.GetComponent<Controller_ScoreBoard>().UpdataRankSetting();

		xMain.Instance.WriteLogIntoFile_UploadToServer();

	}





}
