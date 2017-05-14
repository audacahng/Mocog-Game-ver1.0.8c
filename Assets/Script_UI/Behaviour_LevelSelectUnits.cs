using UnityEngine;
using System.Collections;

public class Behaviour_LevelSelectUnits : MonoBehaviour {


	//UILabel [] Label_MissionName = new UILabel[3];
	public MissionItemUnit[] MissionItemBtn = new MissionItemUnit[7];

	//public int LevelSpeedSet ;
	public int nLevelRoadSeg =0 ;
	public int nLevelWidth=0;

	public int nLevel_ID=0;

	//public int[] LevelRoadSeg = new int[3];

	public Behavior_LevelWidthSelect   Panel_WidthSelect;

	string sNameStar_enable="icon_star_status_enable";
	string sNameStar_disable="icon_star_status_disable";

	public void AssignLevelID()
	{
		nLevel_ID = GameSetting.GetLevelID(  nLevelWidth , nLevelRoadSeg );
	}


	public void UpdateLevelStatus(   int nSegLevel=0 )
	{
		nLevelRoadSeg = nSegLevel;

		int n_SpeedLevel = 7; 
		for(int i = 0 ; i < n_SpeedLevel ; i++)
		{

			//MissionItemBtn[i].lbl_MissionID.text = (1+i) + "-" + (1+LevelRoadSeg[0] );
			MissionItemBtn[i].lbl_MissionID.text = (1+nLevelWidth ) + "-" + (1+i);
			MissionItemBtn[i].lbl_MissionID.fontSize =32;
			MissionItemBtn[i].lbl_MissionID.color = Color.white;


			//MissionItemBtn[i].spl_missionLv1.spriteName = sNameStar_enable;
			//MissionItemBtn[i].spl_missionLv2.spriteName = sNameStar_enable;
			//MissionItemBtn[i].spl_missionLv3.spriteName = sNameStar_enable;


			//MissionItemBtn[i].SetMissionSetting( i, nLevelRoadSeg , 0);
			MissionItemBtn[i].SetMissionSetting( nLevelWidth, i, nLevelRoadSeg  );
			

		}



	}






	/*
	public void BtnPress_01()
	{
		GameSetting.Instance.LevelSpeedSet_Current = LevelSpeedSet;
		GameSetting.Instance.LevelRoadSeg_Current = LevelRoadSeg[0];
		GameSetting.Instance.LevelWidth_Current = 0;

		if(Panel_WidthSelect)
			Panel_WidthSelect.gameObject.SetActive(true);
		//GameObject.Find("core").SendMessage("RunTheGame");
	}

	public void BtnPress_02()
	{

		GameSetting.Instance.LevelSpeedSet_Current = LevelSpeedSet;
		GameSetting.Instance.LevelRoadSeg_Current = LevelRoadSeg[1];
		GameSetting.Instance.LevelWidth_Current = 0;

		if(Panel_WidthSelect)
			Panel_WidthSelect.gameObject.SetActive(true);
		//GameObject.Find("core").SendMessage("RunTheGame");

	}

	public void BtnPress_03()
	{
		GameSetting.Instance.LevelSpeedSet_Current = LevelSpeedSet;
		GameSetting.Instance.LevelRoadSeg_Current = LevelRoadSeg[2];
		GameSetting.Instance.LevelWidth_Current = 0;


		if(Panel_WidthSelect)
			Panel_WidthSelect.gameObject.SetActive(true);
		//GameObject.Find("core").SendMessage("RunTheGame");
	}
	*/





	GameObject FindChildByName( Transform parent , string name)
	{

		foreach (Transform childObj in parent){
			if (childObj.name == name  ){
				return childObj.gameObject;
			}
		}

		return null;
	}



}


 