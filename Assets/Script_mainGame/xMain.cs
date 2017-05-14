using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
	

using System.Threading;
//using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;


using Parse;
using System.Threading.Tasks;


public enum GAMEMODE
{
	MODE_NORMAL,
	MODE_GREEREDLIGHT,
	MODE_DECIDE_DIGITAL,
	MODE_MEMORY,
};



namespace ELG{
	
	public enum SQLITE3_TYPE
	{
		SQLITE_INTEGER = 1,
		SQLITE_FLOAT = 2,
		SQLITE_BLOB = 4,
		SQLITE_NULL = 5,
		SQLITE_TEXT = 3,		
	};
};	


public class xMain : SingletonMonoBehaviour<xMain> {



	#region  table_common operation
	private SQLiteDB db = null;
	private string db_filename="db_NCUGame_v009.sqlite"; // v006 add 2 more column and 1 backup column
	private string db_filenameWithPath="";
	private string queryDeleteCmd = "DROP TABLE IF EXISTS ?;";
	private string queryEmptyCmd = " DELETE FROM ?;";
	private string querySelectCmd = "SELECT * FROM ?;";	
	#endregion


	private string table_mainrecord = "mainrecord";
	//private string table_achievementSet= "achievementSet";
	private string table_roundlog= "roundlog";
	//roundlog


	#region db sql command

	/// table - roundlog 
	private string queryCreate_roundlog="CREATE  TABLE  IF NOT EXISTS roundlog " +
		"(id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  DEFAULT 0, " +
			"TimeRecord INTEGER NOT NULL  DEFAULT 0, " +
			"score_normal INTEGER NOT NULL  DEFAULT 0, " +
			"score_redgreen INTEGER NOT NULL  DEFAULT 0, " +
			"score_numguess INTEGER NOT NULL  DEFAULT 0, " +
			"score_memory INTEGER NOT NULL  DEFAULT 0, " +

			"score_total INTEGER NOT NULL  DEFAULT 0, " +

			"level_normal INTEGER NOT NULL  DEFAULT 0, " +
			"level_redgreen INTEGER NOT NULL  DEFAULT 0, " +
			"level_numguess INTEGER NOT NULL  DEFAULT 0, " +
			"level_memory INTEGER NOT NULL  DEFAULT 0, " +

			"status_upload INTEGER NOT NULL  DEFAULT 0, " +
			"userid TEXT NOT NULL  DEFAULT nulldata);";
	
	/*private string queryInsert_roundlog="INSERT INTO roundlog " +
		"(TimeStart,TimeEnd,point_health,point_science,point_culture,point_civil,point_mine,comment,replaylog) " +
		"VALUES (?,?,?,?,?,?,?,?,?);";*/	
	
	private string query_roundlog = "SELECT id ,TimeRecord,score_normal,score_redgreen,score_numguess,score_memory,score_total,level_normal,level_redgreen,level_numguess,level_memory,status_upload,userid FROM roundlog order by id desc";


	/// main record
	private string queryCreate_logrecord="CREATE TABLE IF NOT EXISTS logrecord " +
		"(id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  DEFAULT 0, " +
		"TimeRecord INTEGER NOT NULL  DEFAULT 0, " +
		"logPath TEXT NOT NULL  DEFAULT nulldata, " +
		"status_upload INTEGER NOT NULL  DEFAULT 0, " +
		"comment TEXT NOT NULL  DEFAULT nulldata);";

	private string query_logrecord = "SELECT id ,TimeRecord,logPath,status_upload,comment FROM logrecord order by id desc";

	#endregion  of db sql command



	public bool m_bTutorMode = false; // <TutorMode>

	public bool m_bGuestPlayerMode= false;


	public GameObject GameCore;
	public GameObject GameCore_GameMode;

	//public PlayerData m_PlayerData = new PlayerData();
	//public PlayerData m_PlayerDataNewRound = new PlayerData();

	public string m_GameUserID="0001";
	public string m_PlayerID;

	public string m_LogStringBuffer="";


	public int nRoundCurrentScore_Normal=0; // <RankingFix>-0
	public int nRoundCurrentScore_RedGreen=0; // <RankingFix>-0
	public int nRoundCurrentScore_NumGuess=0;
	public int nRoundCurrentScore_Memory=0;



	public GAMEMODE m_CurrentGameMode = GAMEMODE.MODE_NORMAL;




	public GameData mRoundData= new GameData();
	public List< GameData> RankDataList = new List<GameData>();

	public bool m_GameLevel_AotoMode = true; // V2 : 




	void Awake() {
		Debug.Log("xMain - Awake()");
		DontDestroyOnLoad(transform.gameObject);




		//m_PlayerID = System.DateTime.Now.ToString("yyyyMMdd-HHmmss-fff")+"_"+m_GameUserID;
		SetPlayerID (m_GameUserID);

		if(db==null)
			InitDB();	




//		int nMyIndex = 12; // level 13
//		Debug.Log ("Total Level:"+m_LevelList.Count()+":"+ m_LevelList[nMyIndex].Area);


//		#if UNITY_IOS
		//			gameObject.AddComponent<LocalNotificationiOS>();
//		#end
//




	//	StartCoroutine( UpdateTest());
	}





	// Use this for initialization
	void Start () {
		//StartCoroutine( QueryTest());
	}


	public IEnumerator QueryTest()
	{
		RoundData_Query_Local();
		yield return StartCoroutine (RoundData_Query_Server() );
	}

	IEnumerator UpdateTest()
	{
		yield return StartCoroutine( RoundData_Update_Server() );
		yield return StartCoroutine( RoundData_QueryRanksOnline() );
	}



	public void SetPlayerID(  string strID , string gametype=null , string level=null   )
	{
		m_PlayerID = System.DateTime.Now.ToString("yyyyMMdd-HHmmss-fff")+"_"+strID;

		if(gametype != null)
			m_PlayerID +="_"+gametype;

		if(level !=null)
			m_PlayerID +="_" + level;

	}

	public void RoundData_Query_Local()
	{

		Debug.Log("RoundData_Query_Local");

		// check if exist 
		//SQLite_InsertRoundWithUserid

		// test query , if not exist then insert default
		//mRoundData.userid

		string strExcep = "where userid=" +"\""+Parse.ParseUser.CurrentUser.Username + "\"" ;
		int nCount  = SQLite_TableContentCount(  "roundlog" , strExcep );
		if(nCount ==0)
			SQLite_InsertRoundWithUserid(  Parse.ParseUser.CurrentUser.Username );


			//mRoundData
		mRoundData.userid = (string)SQLite_TableContentColumnValue( "roundlog", "userid",ELG.SQLITE3_TYPE.SQLITE_TEXT ,strExcep);
		mRoundData.score_normal = (int)SQLite_TableContentColumnValue( "roundlog", "score_normal",ELG.SQLITE3_TYPE.SQLITE_INTEGER,strExcep );
		mRoundData.score_redgreen = (int)SQLite_TableContentColumnValue( "roundlog", "score_redgreen",ELG.SQLITE3_TYPE.SQLITE_INTEGER,strExcep );
		mRoundData.score_numguess = (int)SQLite_TableContentColumnValue( "roundlog", "score_numguess",ELG.SQLITE3_TYPE.SQLITE_INTEGER,strExcep );
		mRoundData.score_memory = (int)SQLite_TableContentColumnValue( "roundlog", "score_memory",ELG.SQLITE3_TYPE.SQLITE_INTEGER,strExcep );


		mRoundData.score_total = (int)SQLite_TableContentColumnValue( "roundlog", "score_total",ELG.SQLITE3_TYPE.SQLITE_INTEGER ,strExcep );
			
		mRoundData.level_normal = (int)SQLite_TableContentColumnValue( "roundlog", "level_normal",ELG.SQLITE3_TYPE.SQLITE_INTEGER , strExcep );
		mRoundData.level_redgreen = (int)SQLite_TableContentColumnValue( "roundlog", "level_redgreen",ELG.SQLITE3_TYPE.SQLITE_INTEGER ,strExcep );
		mRoundData.level_numguess = (int)SQLite_TableContentColumnValue( "roundlog", "level_numguess",ELG.SQLITE3_TYPE.SQLITE_INTEGER ,strExcep );
		mRoundData.level_memory = (int)SQLite_TableContentColumnValue( "roundlog", "level_memory",ELG.SQLITE3_TYPE.SQLITE_INTEGER ,strExcep );

	}



	IEnumerator RoundData_Query_Server()
	{
		Debug.Log("Async_RoundData_Query_Server");

		var query = ParseObject.GetQuery("roundlog")
			//.WhereEqualTo("userid",   ParseUser.CurrentUser.Username     ).OrderByDescending("createdAt");
			.WhereEqualTo("userid",   ParseUser.CurrentUser.Username     );
		//query = query.Limit(1);
		
		IEnumerable<ParseObject> results;

		var qTask =query.FindAsync();
		while(! qTask.IsCompleted) yield return null;
		
		results = qTask.Result;

		if(  results.Count () >0  )
		{
			ParseObject result = results.First();

			int nScoreNormal = result.Get<int>("score_normal");
			int nScoreRedGreen = result.Get<int>("score_redgreen");
			int nScoreNumguess = result.Get<int>("score_numguess");
			int nScoreMemory = result.Get<int>("score_memory");


			int nLevel_normal = result.Get<int>("level_normal");
			int nlevel_redgreen = result.Get<int>("level_redgreen");
			int nlevel_numguess = result.Get<int>("level_numguess");
			int nlevel_memory = result.Get<int>("level_memory");


			Debug.Log( "AsyncQuery:"+ nScoreNormal+","+nScoreRedGreen +"," +nScoreNumguess+","+nScoreMemory+"," + nLevel_normal +"," + nlevel_redgreen+","+nlevel_numguess+","+nlevel_memory  );




			if(m_GameLevel_AotoMode) // V2
			{
				//mRoundData.level_normal = nLevel_normal;
				//mRoundData.level_redgreen = nlevel_redgreen;
			}
			else
			{
				if(nLevel_normal >   mRoundData.level_normal )
					mRoundData.level_normal = nLevel_normal;
				
				if(nlevel_redgreen >   mRoundData.level_redgreen )
					mRoundData.level_redgreen = nlevel_redgreen;

				if(nlevel_numguess >   mRoundData.level_numguess )
					mRoundData.level_numguess = nlevel_numguess;

				if(nlevel_memory >   mRoundData.level_memory )
					mRoundData.level_memory = nlevel_memory;
			}




			if(nScoreNormal >   mRoundData.score_normal )
				mRoundData.score_normal = nScoreNormal;

			if(nScoreRedGreen >   mRoundData.score_redgreen )
				mRoundData.score_redgreen = nScoreRedGreen;

			if(nScoreNumguess >   mRoundData.score_numguess )
				mRoundData.score_numguess = nScoreNumguess;

			if(nScoreMemory >   mRoundData.score_memory )
				mRoundData.score_memory = nScoreMemory;


			mRoundData.score_total = mRoundData.score_normal + mRoundData.score_redgreen + mRoundData.score_numguess + mRoundData.score_memory;

			mRoundData.userid = Parse.ParseUser.CurrentUser.Username;

			RoundData_Update_Local();

			StartCoroutine( RoundData_Update_Server() );
		}
		else
		{
			// no data on server , create new one
			Debug.Log("RoundData_Update_Server-Not Exist, so insert ");
			var roundLogTable = new ParseObject("roundlog");
			roundLogTable["userid"] = ParseUser.CurrentUser.Username;
			roundLogTable["score_normal"]=mRoundData.score_normal;
			roundLogTable["score_redgreen"]= mRoundData.score_redgreen;
			roundLogTable["score_numguess"]= mRoundData.score_numguess;
			roundLogTable["score_memory"]= mRoundData.score_memory;


			roundLogTable["score_total"]= mRoundData.score_normal+ mRoundData.score_redgreen + mRoundData.score_numguess + mRoundData.score_memory;
			roundLogTable["level_normal"]= mRoundData.level_normal;
			roundLogTable["level_redgreen"]= mRoundData.level_redgreen;
			roundLogTable["level_numguess"]= mRoundData.level_numguess;
			roundLogTable["level_memory"]= mRoundData.level_memory;
			
			//roundLogTable["LogDate"] = "";
			Task saveTask = roundLogTable.SaveAsync();
			
		}
		
		
	}
	




	 

	public IEnumerator  RoundData_QueryRanksOnline()
	{
		Debug.Log("Async_RoundData_QueryRanksOnline");
		//var query = ParseObject.GetQuery("roundlog")	.WhereEqualTo("userid",   ParseUser.CurrentUser.Username     ).OrderByDescending("createdAt");
		var query = ParseObject.GetQuery("roundlog")	.OrderByDescending("score_total");

		query = query.Limit(5);
		
		IEnumerable<ParseObject> results;
		
		var qTask =query.FindAsync();

		while(! qTask.IsCompleted) yield return null;
		results = qTask.Result;

		RankDataList = new List<GameData>();

		foreach( var result in results)
		{
			GameData mData = new GameData();
			mData.score_normal= result.Get<int>("score_normal");
			mData.score_redgreen= result.Get<int>("score_redgreen");
			mData.score_numguess= result.Get<int>("score_numguess");
			mData.score_memory= result.Get<int>("score_memory");

			mData.score_total= result.Get<int>("score_total");


			mData.level_normal = result.Get<int>("level_normal");
			mData.level_redgreen = result.Get<int>("level_redgreen");
			mData.level_numguess = result.Get<int>("level_numguess");
			mData.level_memory = result.Get<int>("level_memory");


			mData.userid= result.Get<string>("userid");
			RankDataList.Add( mData );
		}

		Debug.Log("RankData Count:"+ RankDataList.Count() );
//		Debug.Log( "AsyncQuery:"+ nScoreNormal+","+nScoreRedGreen +"," + nLevel_normal +"," + nlevel_redgreen+"."  );
	}



	public void RoundData_Update_Local()// <RankingFix>-3
	{
		if(m_bGuestPlayerMode == true)return;

		string queryGetID = "SELECT id FROM roundlog"+ " where userid="+ "\"" +  mRoundData.userid+ "\"" +" order by id desc limit 1";
		Debug.Log("RoundData_Update_Local:"+queryGetID);
		sqlite_DBInit();                       
		SQLiteQuery qr;		
			
		//string queryGetID = "SELECT id FROM roundlog order by id desc limit 1";
			
		qr = new SQLiteQuery(db, queryGetID  ); 
		qr.Step(); 
		int result =qr.GetInteger("id");
			//Debug.Log ("result-"+result);
		qr.Release ();

		Debug.Log ("mRoundData.level_normal = "+ mRoundData.level_normal );
			
		int nScoreTotal = mRoundData.score_redgreen+ mRoundData.score_normal + mRoundData.score_numguess + mRoundData.score_memory;

		string queryUpdateCommentByID = "update roundlog set"
			+" score_normal="+mRoundData.score_normal
			+",score_redgreen="+mRoundData.score_redgreen
			+",score_numguess="+mRoundData.score_numguess
			+",score_memory="+mRoundData.score_memory
			+",score_total="+  nScoreTotal
			+",level_normal="+mRoundData.level_normal 
			+",level_redgreen="+mRoundData.level_redgreen 
			+",level_numguess="+mRoundData.level_numguess 
			+",level_memory="+mRoundData.level_memory 
			+" where id="+result;

		Debug.Log (queryUpdateCommentByID);
			qr = new SQLiteQuery(db, queryUpdateCommentByID  ); 
			qr.Step(); 
			qr.Release ();
			
			db.Close();



	}

	//public void RoundData_Update_Server()
	public IEnumerator RoundData_Update_Server()
	{
		Debug.Log("RoundData_Update_Server");
		yield return StartCoroutine(  Async_RoundData_Update_Server() );
	}

	IEnumerator Async_RoundData_Update_Server()
	{


		// update to server 
		if(CheckForInternetConnection())
		{
			// 1. try to query and check if exist 


			var query = ParseObject.GetQuery("roundlog")
				.WhereEqualTo("userid",   ParseUser.CurrentUser.Username     );
			query = query.Limit(1);

			IEnumerable<ParseObject> results;
			var qTask =query.FindAsync();
			while(! qTask.IsCompleted) yield return null;
			
			results = qTask.Result;
			if(results.Count() !=0)
			{
				Debug.Log("RoundData_Update_Server-Exist, so update");
				ParseObject result = results.First();

				result["score_normal"] = mRoundData.score_normal;
				result["score_redgreen"] = mRoundData.score_redgreen;
				result["score_numguess"] = mRoundData.score_numguess;
				result["score_memory"] = mRoundData.score_memory;

				result["score_total"] = mRoundData.score_normal + mRoundData.score_redgreen;
				result["level_normal"] = mRoundData.level_normal;
				result["level_redgreen"] = mRoundData.level_redgreen;
				result["level_numguess"] = mRoundData.level_numguess;
				result["level_memory"] = mRoundData.level_memory;
				result.SaveAsync();
			}
			else
			{
				Debug.Log("RoundData_Update_Server-Not Exist, so insert ");
				var roundLogTable = new ParseObject("roundlog");
				roundLogTable["userid"] = ParseUser.CurrentUser.Username;
				roundLogTable["score_normal"]=mRoundData.score_normal;
				roundLogTable["score_redgreen"]= mRoundData.score_redgreen;
				roundLogTable["score_numguess"]= mRoundData.score_numguess;
				roundLogTable["score_memory"]= mRoundData.score_memory;

				roundLogTable["score_total"]= mRoundData.score_normal+ mRoundData.score_redgreen + mRoundData.score_numguess + mRoundData.score_memory;
				roundLogTable["level_normal"]= mRoundData.level_normal;
				roundLogTable["level_redgreen"]= mRoundData.level_redgreen;
				roundLogTable["level_numguess"]= mRoundData.level_numguess;
				roundLogTable["level_memory"]= mRoundData.level_memory;
				
				//roundLogTable["LogDate"] = "";
				Task saveTask = roundLogTable.SaveAsync();
			}


		}
	}


	 


	
	// Update is called once per frame
	void Update () {
	
	}




	public MainGameWorld GetGameCore()
	{
		if( GameCore == null)
			GameCore = GameObject.Find("core");



		return GameCore.GetComponent<MainGameWorld>();
	}



	void db_CreateAllTables()
	{
		try{
			
			// initialize database
			db.Open( db_filenameWithPath);                               

			
			//Init all Tables;
			SQLiteQuery qr;				

			qr = new SQLiteQuery(db,queryCreate_roundlog); 
			qr.Step();	 qr.Release();
			// insert a blank
			

			Debug.Log("queryCreate_logrecord : "+queryCreate_logrecord);
			qr = new SQLiteQuery(db,  queryCreate_logrecord); 
			qr.Step();	 qr.Release();

			
			
			db.Close();
		} catch (Exception e){
			//log += 	"\nTest Fail with Exception " + e.ToString();
			//log += 	"\n on WebPlayer it must give an exception, it's normal.";
		}		
		
	}



	void InitDB()
	{

		Debug.Log("InitDB-1~:"+db_filename);
		//Debug.Log("queryCreate_itemset:"+queryCreate_itemset);
		
		db = new SQLiteDB();
		db_filenameWithPath = Application.persistentDataPath + "/"+db_filename;	
		Debug.Log("InitDB-2:"+db_filenameWithPath);				


		db_CreateAllTables();

		//if(  SQLite_TableContentCount("roundlog"  ) ==0 )
			//SQLite_InsertDefault( "roundlog");



		/*
		if(  SQLite_TableContentCount("mainrecord"  ) ==0 )
			SQLite_InsertDefault( "mainrecord");
		

		string []TableInit = new string[7];
		TableInit[0] = "itemset";
		TableInit[1] = "eventSet";
		TableInit[2] = "achievementSet";
		TableInit[3] = "eventInGameSet";
		TableInit[4] = "AvatarSet";
		TableInit[5] = "Questionary";
		TableInit[6] = "encyclopedia";
		
		
		//encyclopedia
		//Questionary		
		for(int i = 0 ; i < TableInit.Length ; i++ )
		{
			if( SQLite_TableContentCount( TableInit[i] )   ==0  )
			{
				//Debug.Log("init first time:"+TableInit[i]);
				List<TableInfo> ColumnList ;	
				ColumnList = xMain.Instance.SQLite_TableInfo( TableInit[i] );						
				string path;
				path = Application.dataPath +@"/"+"Resources"+ @"/database/"+"table_"+  TableInit[i] +".txt";
				

				xMain.Instance.SQLite_ImportData( TableInit[i] , path, ColumnList);			
			}
			
		}*/
		
	}
	
	
	
	private void sqlite_DBInit()
	{
		if( db == null )
			InitDB	();
		
		db.Close();
		db.Open( db_filenameWithPath);                            
		
	}



	public void SQLCommand_Update_roundlog()
	{


	}


	public void SQLCommand_Insert_logrecord(string path)
	{
		
		if(db==null)
			InitDB();
		
		db.Close();
		db.Open( db_filenameWithPath);
		SQLiteQuery qr;

		int unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;


		//private string query_logrecord = "SELECT id ,TimeRecord,logPath,status_upload,comment FROM logrecord order by id desc";
		string sqlCmd = "insert into "+"logrecord" + "(" +"logPath, TimeRecord" + ")"  + " Values(" +"\""+path +"\" ,"+ unixTimestamp +  ")" ;
		Debug.Log(sqlCmd);

		qr = new SQLiteQuery(db, sqlCmd  ); // tableName
		qr.Step();
		qr.Release();
		db.Close();


	}


	public bool SQLite_InsertRoundWithUserid( string userid )
	{

		string sqlCmd = "insert into roundlog (userid) Values(" +"\""+userid + "\""+ ")"   ;
		Debug.Log("SQLite_InsertRoundWithUserid:"+sqlCmd);

		if(db==null)
			InitDB();
		
		db.Close();
		db.Open( db_filenameWithPath);
		SQLiteQuery qr;
		

		//Debug.Log(sqlCmd);
		
		qr = new SQLiteQuery(db, sqlCmd ); // tableName
		qr.Step();
		qr.Release();    		
		db.Close();		
		return true;
	}

	public bool SQLite_InsertCmd( string cmd )
	{
		
		if(db==null)
			InitDB();
		
		db.Close();
		db.Open( db_filenameWithPath);
		SQLiteQuery qr;
		
		string sqlCmd = cmd;
		//Debug.Log(sqlCmd);
		
		qr = new SQLiteQuery(db, sqlCmd  ); // tableName
		qr.Step();
		qr.Release();    		
		db.Close();		
		return true;
	}


	public bool SQLite_InsertDefault( string tableName )
	{
		
		if(db==null)
			InitDB();
		
		db.Close();
		db.Open( db_filenameWithPath);
		SQLiteQuery qr;
		
		string sqlCmd = "insert into "+tableName + " DEFAULT VALUES " ;
		//Debug.Log(sqlCmd);
		
		qr = new SQLiteQuery(db, sqlCmd  ); // tableName
		qr.Step();
		qr.Release();    		
		db.Close();		
		return true;
	}





	public int   SQLite_TableContentCount( string tableName  , string excepStr=""   )
	{		
		if(db==null)
			InitDB();
		
		db.Close();
		db.Open( db_filenameWithPath);
		SQLiteQuery qr;
		
		
		// 		Delete table if exists
		string sStr="select " + " Count(*) " + " from "+ tableName+" " +excepStr+ " order by id asc " ;		
		//		Debug.Log("SQLite_TableContent:"+sStr );
		
		qr = new SQLiteQuery(db, sStr  ); // tableName
		//qr.sDebugString
		
		int nCount =0;
		qr.Step();
		nCount = qr.GetInteger( "Count(*)");
		
		qr.Release();    		
		db.Close();
		
		
		return nCount;
	}	


	public object   SQLite_TableContentColumnValue( string tableName, string columnName  ,ELG.SQLITE3_TYPE eColumnType, string excepStr=""   )
	{
		
		if(db==null)
			InitDB();
		
		db.Close();
		db.Open( db_filenameWithPath);
		SQLiteQuery qr;
		
		string sStr="select " +"\""+ columnName+"\"" + " from "+ tableName+" " +excepStr+ " order by id asc " ;		
		//Debug.Log("SQLite_TableContent:"+sStr );
		
		qr = new SQLiteQuery(db, sStr  ); // tableName
		//qr.sDebugString
		
		object Count ;
		qr.Step();
		
		if(eColumnType == ELG.SQLITE3_TYPE.SQLITE_INTEGER  )		
			Count =  (object)qr.GetInteger( columnName );
		else
			if(eColumnType == ELG.SQLITE3_TYPE.SQLITE_TEXT  )		
				Count =  (object)qr.GetString( columnName );
		else
			if(eColumnType == ELG.SQLITE3_TYPE.SQLITE_FLOAT  )		
				Count =  (object)qr.GetDouble( columnName );
		else
			if(eColumnType == ELG.SQLITE3_TYPE.SQLITE_BLOB  )		
				Count =  (object)qr.GetBlob( columnName );
		else
			Count = null;
		
		qr.Release();    		
		db.Close();
		
		return Count;
	}		


	/*
	public void UploadLogDataToServer(string sData)
	{

		//UI_TunePanel_UploadStatus.text = "uploading...";
		
		//ServerUrl
		JSONObject  j = new JSONObject( JSONObject.Type.OBJECT);
		j.AddField("method", 0.5f);
		
		
		j.AddField("data", sData );
		
		
		string encodingStr = j.Print();
		
		WWW www ;
		
		WWWForm form = new WWWForm();
		form.AddField("Save", encodingStr);

		string ServerUrl = "http://el-idea.com/NCUGameServer/NCUGameServer.php";

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
//			UI_TunePanel_UploadStatus.text = "Done";
			
		} else {
			Debug.Log("WWW Error: "+ www.error);
//			UI_TunePanel_UploadStatus.text = "Fail:" + www.error;
		}    
	}
	*/
	
	
	public void WriteLogIntoFile( string msg , bool bTimeLog = false)
	{
		//Debug.Log("Data Path="+Application.persistentDataPath);
		//string path = Application.persistentDataPath + @"/"+m_PlayerID+".csv";
		//UploadLogDataToServer( path );
		//Debug.Log("Pfad: " + path);

		if(bTimeLog)
			msg = System.DateTime.Now.ToString ("hh:mm:ss:fff") + ","+  msg   +"\n";
		else
			msg  = msg+"\n";
			//msg +=   ( ","+  System.DateTime.Now.ToString ("hh:mm:ss:fff")  );


		//msg
		m_LogStringBuffer += msg;
		return;

		/*
		if(!File.Exists(path)) {
			//StreamWriter sw =System.IO.File.WriteAllText(path,"aha");  
			System.IO.File.WriteAllText(path,msg+"\n");  
			
		}
		else
		{
			/// Append
			System.IO.File.AppendAllText(path,msg+"\n");
		}
		*/

	}



	public void WriteLogIntoFile_DumpToFile( )
	{

		if(xMain.Instance.m_bGuestPlayerMode == true) return;

		if(m_LogStringBuffer=="" )return;

		Debug.Log("WriteLogIntoFile_DumpToFile:  Data Path="+Application.persistentDataPath);
		string path = Application.persistentDataPath + @"/"+m_PlayerID+".csv";

//		if(bTimeLog)
//			msg = System.DateTime.Now.ToString ("hh:mm:ss:fff") + ","+  msg;
				
		
		if(!File.Exists(path)) {
			//StreamWriter sw =System.IO.File.WriteAllText(path,"aha");  
			System.IO.File.WriteAllText(path,m_LogStringBuffer+"\n");  
			
		}
		else
		{
			/// Append
			System.IO.File.AppendAllText(path,m_LogStringBuffer+"\n");
		}

		//SQLCommand_Insert_logrecord(path);
		SQLCommand_Insert_logrecord(m_PlayerID+".csv");

		m_LogStringBuffer= "";


		// TEST: Upload files 

		/*
		byte[] data = File.ReadAllBytes( path );
		ParseFile file = new ParseFile(m_PlayerID+".csv"  , data);
		//Task saveTask = file.SaveAsync();
		
		var LogsTable = new ParseObject("LogFiles");
		LogsTable["LogUser"] = ParseUser.CurrentUser.Username;
		LogsTable["LogFiles"] = file;
		LogsTable["LogDate"] = "";
		Task saveTask = LogsTable.SaveAsync();*/


		WriteLogIntoFile_UploadToServer();

	}



	public bool WriteLogIntoFile_UploadToServer()
	{
		if(xMain.Instance.m_bGuestPlayerMode == true) return false;
		//if(Network.TestConnection() == ConnectionTesterStatus.Undetermined)
		//	return false;
		if(CheckForInternetConnection() == false )
			return false;

		Debug.Log("WriteLogIntoFile_UploadToServer");
		// upload .
		// update local database table.
		if(db==null)
			InitDB();
		
		db.Close();
		db.Open( db_filenameWithPath);
		SQLiteQuery qr;
		qr = new SQLiteQuery(db, query_logrecord  ); // tableName

		List <logbuffer>LogBufferList = new List<logbuffer>();


		while( qr.Step() )
		{
			//TimeRecord,logPath,status_upload,comment
			logbuffer mLogBuffer= new logbuffer();
			mLogBuffer.nID =qr.GetInteger(  "id" );
			mLogBuffer.filename =qr.GetString(  "logPath" );
			LogBufferList.Add( mLogBuffer );
		}
		qr.Release();    		

		//int nID =  qr.GetInteger(  "id" );
		//string filename = qr.GetString(  "logPath" );
		//Debug.Log("LogBufferList:"+ LogBufferList.Count);

		db.Close();
		db.Open( db_filenameWithPath);

		for( int i = 0 ; i < LogBufferList.Count ; i++  )
		{
			
			//byte[] data = File.ReadAllBytes( path );
			string path = Application.persistentDataPath +"/"+  LogBufferList[i].filename;
			byte[] data = File.ReadAllBytes( path );
			
			ParseFile file = new ParseFile(LogBufferList[i].filename   , data);
			//Task saveTask = file.SaveAsync();
			//Debug.Log("SaveAsync-upload:"+LogBufferList[i].filename );
			
			var LogsTable = new ParseObject("LogFiles");
			LogsTable["LogUser"] = ParseUser.CurrentUser.Username;
			LogsTable["LogFiles"] = file;
			LogsTable["LogDate"] = "";
			//Task saveTask = LogsTable.SaveAsync() ;

			int mID =  LogBufferList[i].nID;

			LogsTable.SaveAsync().ContinueWith( 
					t =>{
						//Debug.Log("SaveAsync-Done!:"+mID );

						db.Close();
						db.Open( db_filenameWithPath);
						qr = new SQLiteQuery(db,  "DELETE FROM logrecord where id= "+ mID); 
						qr.Step(); qr.Release();				
					//		Debug.Log("SaveAsync-Done:"+ "DELETE Finish");
					}
			) ;
		}



		//db.Open( db_filenameWithPath);
		//qr = new SQLiteQuery(db,  "DELETE FROM logrecord;"); 
		//qr.Step(); qr.Release();				
		db.Close();


		return true;

	}




	public static bool CheckForInternetConnection()
	{
		try
		{
			using (var client = new WebClient())
				using (var stream = client.OpenRead("http://www.google.com"))
			{
				return true;
			}
		}
		catch
		{
			return false;
		}
	}


}




public class TableInfo{
	public string columnName;
	public string columnType;
	public object columnValue;
	
	public int nColumnLength;
	
};


public class GameData{
	public int score_normal=0;
	public int score_redgreen=0;
	public int score_numguess=0;
	public int score_memory=0;

	public int score_total=0;
	public int level_normal=0;
	public int level_redgreen=0;
	public int level_numguess=0;
	public int level_memory=0;


	public string userid="";
};



public class logbuffer{
	public int nID;
	public string filename;
};



