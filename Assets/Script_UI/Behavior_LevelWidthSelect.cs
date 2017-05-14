using UnityEngine;
using System.Collections;


// Road Segment Setting 
public class Behavior_LevelWidthSelect : MonoBehaviour {

	public GameObject []RoadSegArray;



	public void UpdateCurrentRoadSegment( int nRoadSeg)
	{
		for( int i = 0 ; i < RoadSegArray.Length ; i++)
		{
			if( i <=  nRoadSeg)
			{
				RoadSegArray[i].SetActive(true);
			}
			else
				RoadSegArray[i].SetActive(false);
		}
	}

	// Use this for initialization
	void Start () {
	
	}


	public void PanelClose()
	{
		gameObject.SetActive(false);
	}


	public void RunGame_WidthEasy()
	{
		//GameSetting.Instance.LevelWidth_Current = 0;
		GameSetting.Instance.LevelRoadSeg_Current=0;
		GameObject.Find("core").SendMessage("RunTheGame");

	}

	public void RunGame_WidthNormal()
	{
		//GameSetting.Instance.LevelWidth_Current = 1;
		GameSetting.Instance.LevelRoadSeg_Current=1;
		GameObject.Find("core").SendMessage("RunTheGame");
	}

	public void RunGame_WidthDiffucult()
	{
		//GameSetting.Instance.LevelWidth_Current = 2;
		GameSetting.Instance.LevelRoadSeg_Current=2;
		GameObject.Find("core").SendMessage("RunTheGame");

	}

	// Update is called once per frame
	void Update () {
	
	}
}
