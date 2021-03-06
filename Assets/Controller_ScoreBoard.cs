﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class Controller_ScoreBoard : MonoBehaviour {

	public GameObject ScoreBarUnit_Template;
	public GameObject scrollmenu_pivot;
	public List<GameObject>  m_RankSet= new List<GameObject>();

	//public UILabel m_mainScoreTitle;


	public UILabel m_myRank;
	public UILabel m_myName;
	public UILabel m_myScore_normal;
	public UILabel m_myScore_redgreen;
	public UILabel m_myScore_numguess;
	public UILabel m_myScore_memory;

	public UILabel m_myScore_total;


	public UILabel m_LabelLoadingStatus;

	// Use this for initialization
	void Reposition()
	{
		
		scrollmenu_pivot.GetComponent<UITable>().repositionNow = true;
		scrollmenu_pivot.GetComponent<UITable>().gameObject.SetActive(false);
		scrollmenu_pivot.GetComponent<UITable>().gameObject.SetActive(true);
		
		//scrollmenu_scrollBar.GetComponent<UIScrollBar>().value=0;
		scrollmenu_pivot.GetComponent<UITable>().gameObject.transform.parent.GetComponent<UIScrollView>().gameObject.SendMessage("OnVerticalBar");
		
		Debug.Log("Controller_UILevelSelectPanel-Start");
		
		//UpdateCurrentLevel();
	}

	public void UpdataRankSetting()
	{
		m_LabelLoadingStatus.text = "載入中，請確保網路已連線";

		m_myRank.text = "";
		m_myName.text = "";
		m_myScore_normal.text= "";
		m_myScore_redgreen.text = "";
		m_myScore_numguess.text = "";
		m_myScore_memory.text ="";

		m_myScore_total.text= "";

		GameObject ObjToDestroy;
		// reset-> Detroy all 
		for(int i=0 ; i< m_RankSet.Count; i++)
		{
			ObjToDestroy = m_RankSet[i];
			GameObject.DestroyImmediate(  ObjToDestroy);
		}
		m_RankSet.Clear();

		StartCoroutine(LoadScoreBoardOnline());
	}

	IEnumerator LoadScoreBoardOnline()
	{
		yield return StartCoroutine(   	xMain.Instance.RoundData_QueryRanksOnline() );





		GameObject newRankUnit;
		for(int i = 0 ; i < xMain.Instance.RankDataList.Count ; i++ )
		{
			Debug.Log(i);


			newRankUnit	= GameObject.Instantiate( ScoreBarUnit_Template) as GameObject;
			newRankUnit.name = "Rank_"+i;
			
			newRankUnit.SetActive(true);
			//newLevelBar.transform.localPosition = Vector3.zero;
			newRankUnit.transform.parent = scrollmenu_pivot.transform;
			newRankUnit.transform.localScale = Vector3.one;
			
			
			//newLevelBar.GetComponent<Behaviour_LevelSelectUnits>().LevelSpeedSet = i;
			//newLevelBar.GetComponent<Behaviour_LevelSelectUnits>().nLevelRoadSeg = i;
			newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mRank.text = " "+ (i+1); // rank count from 1
			//EC 2016-11-29 Cannot tell why division of 100 was necessary
			//OK, if not dividing 100, the score became way bigger 
			newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mName.text = xMain.Instance.RankDataList[i].userid;
			newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mScoreNormal.text = (xMain.Instance.RankDataList[i].score_normal/100).ToString();
			newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mScoreRedGreen.text = (xMain.Instance.RankDataList[i].score_redgreen/100).ToString();
			newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mScoreNumGuess.text = (xMain.Instance.RankDataList[i].score_numguess/100).ToString();
			newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mScoreMemory.text = (xMain.Instance.RankDataList[i].score_memory/100).ToString();

			newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mScoreRedTotal.text = (xMain.Instance.RankDataList[i].score_total/100).ToString();
			
			
			/*newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mScoreNormal.text = (xMain.Instance.RankDataList[i].score_normal).ToString();
			newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mScoreRedGreen.text = (xMain.Instance.RankDataList[i].score_redgreen).ToString();
			newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mScoreNumGuess.text = (xMain.Instance.RankDataList[i].score_numguess).ToString();
			newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mScoreMemory.text = (xMain.Instance.RankDataList[i].score_memory).ToString();

			newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mScoreRedTotal.text = (xMain.Instance.RankDataList[i].score_total).ToString();*/
			
			//newRankUnit.GetComponent<Unit_ScoreBoardUnit>().text = xMain.Instance.RankDataList[i].score_redgreen;

			if(newRankUnit.GetComponent<Unit_ScoreBoardUnit>().mName.text == Parse.ParseUser.CurrentUser.Username)
				newRankUnit.GetComponent<UISprite>().color = Color.yellow;
			else
				newRankUnit.GetComponent<UISprite>().color = Color.white;

			m_RankSet.Add(newRankUnit);


		}


		Reposition();
		m_LabelLoadingStatus.text="";




		m_myRank.text = "-";
		m_myName.text = Parse.ParseUser.CurrentUser.Username;
		m_myScore_normal.text= xMain.Instance.mRoundData.score_normal/100 +"";
		m_myScore_redgreen.text =xMain.Instance.mRoundData.score_redgreen/100+"";
		m_myScore_numguess.text =xMain.Instance.mRoundData.score_numguess/100+"";
		m_myScore_memory.text =xMain.Instance.mRoundData.score_memory/100+"";
		m_myScore_total.text=xMain.Instance.mRoundData.score_total/100+"";
		/*m_myScore_normal.text= xMain.Instance.mRoundData.score_normal +"";
		m_myScore_redgreen.text =xMain.Instance.mRoundData.score_redgreen+"";
		m_myScore_numguess.text =xMain.Instance.mRoundData.score_numguess+"";
		m_myScore_memory.text =xMain.Instance.mRoundData.score_memory+"";
		m_myScore_total.text=xMain.Instance.mRoundData.score_total+"";*/

	}

	public void CloseScorePanel()
	{

		gameObject.SetActive(false);

	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
