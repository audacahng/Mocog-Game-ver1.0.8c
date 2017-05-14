using UnityEngine;
using System.Collections;

public class MissionItemUnit : MonoBehaviour {


	string sNameStar_enable="icon_star_status_enable";
	string sNameStar_disable="icon_star_status_disable";


	public  UILabel lbl_MissionID;
	public UISprite spl_missionLv1;
	public UISprite spl_missionLv2;
	public UISprite spl_missionLv3;

	public UISprite[] spl_missionArray;

	public int m_Level_RoadWidth;
	public int m_Level_Speed;
	public int m_Level_RoadSeg;



	public int m_Level_ID;

	public Behavior_LevelWidthSelect   Panel_WidthSelect;

	public bool bOpen = false;


	public void AssignLevelID()
	{
		m_Level_ID = GameSetting.GetLevelID(  m_Level_RoadWidth , m_Level_Speed );
	}


	public void UpdateEnableDisableStatusByID( int nCurrentID)
	{
		if(nCurrentID  >= m_Level_ID) 
		{
			gameObject.GetComponent<UISprite>().color = Color.yellow;


			for(int i = 0 ; i < 3 ;i ++)
				spl_missionArray[i].spriteName = sNameStar_disable;


			if(nCurrentID  >= m_Level_ID)
			{

				m_Level_RoadSeg = GameSetting.GetLevel_03_RoadSeg( nCurrentID, m_Level_RoadWidth, m_Level_Speed );
				//Debug.Log("m_Level_RoadSeg="+m_Level_RoadSeg);


				for(int i = 0 ; i < 3 ;i ++)
				{
					if( i <=m_Level_RoadSeg  )
					{
						spl_missionArray[i].spriteName = sNameStar_enable;
					}
					else
					{
						spl_missionArray[i].spriteName = sNameStar_disable;
					}
				}
			}




		}
		else
		{
			gameObject.GetComponent<UISprite>().color = Color.gray;


			for(int i = 0 ; i < 3 ;i ++)
				spl_missionArray[i].spriteName = sNameStar_disable;
		}

	}





	// Width(1~3) - Speed(1~7) - RoadSeg( 1~3)
	public void SetMissionSetting(int nRoadWidthCurrent, int nSpeed , int nRoadSeg  )
	{
		m_Level_RoadWidth = nRoadWidthCurrent;
		m_Level_Speed = nSpeed;
		m_Level_RoadSeg = nRoadSeg;

	}


	public void OnClick()
	{
		if(gameObject.GetComponent<UISprite>().color == Color.gray)
			return;

		GameSetting.Instance.LevelSpeedSet_Current = m_Level_Speed;
		GameSetting.Instance.LevelRoadSeg_Current = m_Level_RoadSeg;
		GameSetting.Instance.LevelWidth_Current = m_Level_RoadWidth;
		
		if(Panel_WidthSelect)
		{
			Panel_WidthSelect.gameObject.SetActive(true);
			Panel_WidthSelect.UpdateCurrentRoadSegment(m_Level_RoadSeg);
		}


	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
