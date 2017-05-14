using UnityEngine;
using System.Collections;

public abstract class GameMode:MonoBehaviour {



	public abstract void InitSetting ();

	public abstract int  GetCurrentLevel ();
	
	public abstract void GameStart();

	public abstract void GameFinish ( bool bWinLose );

	public bool m_bTimePauseByGameTime;



}
