using UnityEngine;
using System.Collections;

public class iTweenPathTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath( "BallFly"),"time",50));
		//iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath( "BallFly"),"time",5, "easetype", iTween.EaseType.easeInOutSine    ));
		iTween.MoveTo(gameObject,iTween.Hash("path",iTweenPath.GetPath( "BallFly"),"time",5, "easetype", iTween.EaseType.easeInOutSine,"looptype",iTween.LoopType.pingPong    ));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
