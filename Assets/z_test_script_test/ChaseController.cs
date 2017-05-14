using UnityEngine;
using System.Collections;

public class ChaseController : MonoBehaviour {
	
	
	public GameObject  target;
	public int CameraHeight =6;
	
	static private ChaseController chaseController;
	
	
	void Awake()
	{
		chaseController = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	
	void LateUpdate()
	{
		if(target==null)
			return;
		
		Vector3 position = target.transform.position - target.transform.forward*8;
		position.y = CameraHeight;
		transform.position = position;
		
		transform.LookAt( target.transform.position );
		
		//ScoreController.
		//ScoreController.AddScore(1);
		//scoreController.AddScore(1);
		
	}
	
	
	
	/*
	static public void UpdatePosition(Vector3 target)
	{
		
		if(target==null)
			return;
		
		Vector3 position = target.transform.position - target.transform.forward*8;
		position.y = CameraHeight;
		transform.position = position;
		
		transform.LookAt( target.transform.position );		
	}
	*/
	
	
	
	
}
