using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;


public class GameSetting : SingletonMonoBehaviour<GameSetting>  {

	public int []Level_Speed = new int[7]{30,40,50,60,70,80,90};

	public float [,] Level_RoadSeg = new float[3,6]{  
		{ 0.6f , 0f, 0.4f , 0.3f, 0f, 0f  } ,
		{ 0.4f , 0.1f, 0f , 0f, 0.5f, 0f  } , 
		{    0f , 0.1f, 0f , 0f, 0.3f, 0.4f  } , 
	};





	public List<GameLevel> m_LevelList = new List<GameLevel> ();


	public float [] Level_Width = new float[3]{1,0.7f,0.55f};

	public int LevelSpeedSet_Current =0;
	public int LevelRoadSeg_Current =0;
	public int LevelWidth_Current =0;

	public int mMax_RoadSeg = 3;
	public int mMax_Speed = 7;
	public int mMax_Width = 3;

	public int Level_CurrentMaxLevel = 28; // 1~63

	public void LogCurrentSetting()
	{

		xMain.Instance.WriteLogIntoFile( "[Setting] Speed," +Level_Speed[LevelSpeedSet_Current]  );

		xMain.Instance.WriteLogIntoFile( "[Setting] RoadSeg," +Level_RoadSeg[LevelRoadSeg_Current,0] + " "+ 
		                                Level_RoadSeg[LevelRoadSeg_Current,1] +" "+
		                                Level_RoadSeg[LevelRoadSeg_Current,2] +" "+
		                                Level_RoadSeg[LevelRoadSeg_Current,3] +" "+
		                                Level_RoadSeg[LevelRoadSeg_Current,4] +" "+
		                                Level_RoadSeg[LevelRoadSeg_Current,5] 
		                                );

		xMain.Instance.WriteLogIntoFile( "[Setting] Width," +Level_Width[LevelWidth_Current]  );

		xMain.Instance.WriteLogIntoFile( "[Setting] GameMode," + xMain.Instance.m_CurrentGameMode );

		string strGameMode="";

		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_GREEREDLIGHT)
			strGameMode = "RG";
		else
		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_NORMAL)
			strGameMode = "N";
		else
		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_DECIDE_DIGITAL)
			strGameMode = "DIGI";
		else
		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_MEMORY)
			strGameMode = "MEM";
		else
			strGameMode = "ERR";



		string strLevel = ((int)LevelWidth_Current +1 ) + "_"+  ((int)LevelSpeedSet_Current +1)+ "_" + ((int)LevelRoadSeg_Current +1);

		xMain.Instance.SetPlayerID( xMain.Instance.m_GameUserID,strGameMode,strLevel  );
	}

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);

		LoadGameLevelSetting ();
	}


	void LoadGameLevelSetting()
	{

		TextAsset gdata = (TextAsset)Resources.Load( "gamelevel" , typeof(TextAsset) ) ;
		StreamReader reader = new StreamReader(    GenerateStreamFromString(gdata.text )  );		
		string text;
		text = reader.ReadLine();// header
		text = reader.ReadLine();
		
		while(  text !=null )
		{
			//		string text = reader.ReadLine();
			string[] ParsArray = text.Split(',');
			string sData="";
			
			
			GameLevel LevelUnit = new GameLevel();
			
			
			
			LevelUnit.keyID = int.Parse(ParsArray[0] );
			LevelUnit.Area =  int.Parse(ParsArray[1] );
			LevelUnit.Layer = int.Parse(ParsArray[2] );
			LevelUnit.speed = int.Parse(ParsArray[3] );
			LevelUnit.curve = int.Parse(ParsArray[4] );
			LevelUnit.width =  Convert.ToSingle(ParsArray[5] );
			
			m_LevelList.Add( LevelUnit);
			
			
			text = reader.ReadLine();
		}


	}

	public Stream GenerateStreamFromString(string s)
	{
		MemoryStream stream = new MemoryStream();
		StreamWriter writer = new StreamWriter(stream);
		writer.Write(s);
		writer.Flush();
		stream.Position = 0;
		return stream;
	}	


	// Use this for initialization
	void Start () {
	
	}


	public static int GetLevel_01_Width( int currentLevel)
	{
		return currentLevel / (3*7);
	}

	public static int GetLevel_02_Speed( int currentLevel)
	{
		return (currentLevel   - GetLevel_01_Width( currentLevel ) *(3*7) ) / 3;
	}

	public static int GetLevel_03_RoadSeg( int currentLevel , int nWidth, int nSpeed)
	{
		//Debug.Log(    (currentLevel - nWidth*21  )  - 3*nSpeed  );
		return   (  (currentLevel - nWidth*21  )  - 3*nSpeed );
	}

	public static int GetLevelID(int nWidth , int nSpeed , int nRoadSeg = 0)
	{
		int nBackToNumber = nWidth*(3*7) + nSpeed*( 3) + nRoadSeg;
		return nBackToNumber;
	}



	// Update is called once per frame
	void Update () {
	
	}
}


// V2
public class GameLevel{
	public int keyID;
	public int Area;
	public int Layer;
	public int speed;
	public int curve;
	public float width;
	
	
}
