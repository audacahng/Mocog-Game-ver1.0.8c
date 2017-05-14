using UnityEngine;
using System.Collections;

public class Controller_ResultPanel : MonoBehaviour {

	public UILabel Label_Result;

	public GameObject Btn_Next;
	public GameObject Btn_Reset;

	public void ShowResult(  bool bSuccess , int nScore=0)
	{
		if(bSuccess)
		{


			if(xMain.Instance.m_CurrentGameMode == GAMEMODE.MODE_GREEREDLIGHT)
				nScore+=	 GameObject.Find("coreGameMode").GetComponent<GameMode_RedGreenLight>().GetScore_ExtraCredit();

			//nScore += xMain.Instance.CurrentGameMode_GameFinish_ResultScore();
			xMain.Instance.WriteLogIntoFile("GameScoreResult,"+nScore );


			Label_Result.text = "完成 !! \n 分數="+nScore;


			Btn_Next.SetActive(true);
			Btn_Reset.SetActive(false);
		}
		else
		{
			Label_Result.text = "失敗了";


			Btn_Next.SetActive(false);
			Btn_Reset.SetActive(true);
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
