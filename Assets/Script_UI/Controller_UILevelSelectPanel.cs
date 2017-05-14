using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller_UILevelSelectPanel : MonoBehaviour {

	public GameObject LevelSelectBar_Template;
	public GameObject scrollmenu_pivot;

	public GameObject scrollmenu_scrollBar;


	public List<GameObject>  m_LevelSet= new List<GameObject>();

	public bool debug_bRunTest =false;
	public int debug_nLevelValue =0;

	 // Width(1~3) - Speed(1~7) - RoadSeg( 1~3)
	public void UpdateCurrentLevel()
	{
		//int nCurrentLV = GameSetting.Instance.Level_CurrentMaxLevel; // 1~63
		int nCurrentLV=0;



		if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_NORMAL)
			nCurrentLV = xMain.Instance.mRoundData.level_normal;
		else
			nCurrentLV = xMain.Instance.mRoundData.level_redgreen;



		/*
		int nR_Width = nCurrentLV / (3*7);
		int nR_Speed =   ( nCurrentLV -  nR_Width*(3*7)  ) / 3;
		int nR_RoadSeg = (nCurrentLV - nR_Width*(3*7))  - 3*nR_Speed;
		Debug.Log( nCurrentLV + "="+ nR_Width + "~" + nR_Speed + "~" + nR_RoadSeg  );


		// return
		int nBackToNumber = nR_Width*(3*7) + nR_Speed*( 3) + nR_RoadSeg;
		Debug.Log("recover:" + nBackToNumber);*/


		for(int i =0 ; i <  m_LevelSet.Count ; i++) // RoadSegment
		{

			for(int j = 0 ;  j < 7 ; j++   )  // speed
			{
				//m_LevelSet[i].GetComponent< Behaviour_LevelSelectUnits>().MissionItemBtn[j]
				m_LevelSet[i].GetComponent< Behaviour_LevelSelectUnits>().MissionItemBtn[j].AssignLevelID();
				m_LevelSet[i].GetComponent< Behaviour_LevelSelectUnits>().MissionItemBtn[j].UpdateEnableDisableStatusByID( nCurrentLV);

			}


		}


	}


	// Use this for initialization
	void Awake () {

		int nWidthCount = GameSetting.Instance.Level_Width.Length;

		GameObject newLevelBar;

		// 7x3 *3  



		int nLevel_RoadSeg = 3 ;
		//for(int i = 0 ; i < nLevel_Speed ; i++ )
		for(int i = 0 ; i < nLevel_RoadSeg ; i++ )
		{
				newLevelBar	= GameObject.Instantiate( LevelSelectBar_Template) as GameObject;
				newLevelBar	.name = "LevelWidth_"+i;

				newLevelBar.SetActive(true);
				//newLevelBar.transform.localPosition = Vector3.zero;
				newLevelBar.transform.parent = scrollmenu_pivot.transform;
				newLevelBar.transform.localScale = Vector3.one;


				//newLevelBar.GetComponent<Behaviour_LevelSelectUnits>().LevelSpeedSet = i;
				//newLevelBar.GetComponent<Behaviour_LevelSelectUnits>().nLevelRoadSeg = i;
				newLevelBar.GetComponent<Behaviour_LevelSelectUnits>().nLevelWidth = i;
				newLevelBar.GetComponent<Behaviour_LevelSelectUnits>().UpdateLevelStatus( i );
				newLevelBar.GetComponent<Behaviour_LevelSelectUnits>().AssignLevelID();
			

				m_LevelSet.Add(newLevelBar);
		}

		//xMain.Instance.WriteLogIntoFile("testttttt!!!");


		Debug.Log("Controller_UILevelSelectPanel-Awake");
		//scrollmenu_scrollBar.GetComponent<UIScrollBar>()

		UpdateCurrentLevel ();
	}

	void Start()
	{

		scrollmenu_pivot.GetComponent<UITable>().repositionNow = true;
		scrollmenu_pivot.GetComponent<UITable>().gameObject.SetActive(false);
		scrollmenu_pivot.GetComponent<UITable>().gameObject.SetActive(true);

		scrollmenu_scrollBar.GetComponent<UIScrollBar>().value=0;
		scrollmenu_pivot.GetComponent<UITable>().gameObject.transform.parent.GetComponent<UIScrollView>().gameObject.SendMessage("OnVerticalBar");

		Debug.Log("Controller_UILevelSelectPanel-Start");

		//UpdateCurrentLevel();
	}


	public void BtnCloseBehavoir()
	{
		gameObject.SetActive(false);


	}



	
	// Update is called once per frame
	void Update () {
	
		if(debug_bRunTest)
		{
			debug_bRunTest = false;
			GameSetting.Instance.Level_CurrentMaxLevel =debug_nLevelValue;

			UpdateCurrentLevel();



			//debug_nLevelValue




		}


	}
}
