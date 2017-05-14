using UnityEngine;
using System.Collections;

public class MoveTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		Hashtable MoveSetting = iTween.Hash( "time", 0.45f ,"islocal",false, "position" ,GameObject.Find("UI_LifeBar").transform.position ,"easetype","linear","oncomplete", "OnMoveToBarComplete", "oncompletetarget", gameObject  );
		iTween.MoveTo(  gameObject , MoveSetting );


	}

	void OnMoveToBarComplete()
	{
		Hashtable MoveSetting = iTween.Hash( "time", 0.45f ,"islocal",false, "position" ,GameObject.Find("UI_Controller_Left").transform.position ,"easetype","linear","oncomplete", "OnMoveToAnotherBarComplete", "oncompletetarget", gameObject  );
		iTween.MoveTo(  gameObject , MoveSetting );
	}

	void OnMoveToAnotherBarComplete()
	{
		Hashtable MoveSetting = iTween.Hash( "time", 0.45f ,"islocal",false, "position" ,GameObject.Find("UI_LifeBar").transform.position ,"easetype","linear","oncomplete", "OnMoveToBarComplete", "oncompletetarget", gameObject  );
		iTween.MoveTo(  gameObject , MoveSetting );

	}


	// Update is called once per frame
	void Update () {
	
	}
}
