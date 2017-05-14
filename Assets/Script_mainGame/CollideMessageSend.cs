using UnityEngine;
using System.Collections;

public class CollideMessageSend : MonoBehaviour {

	public GameObject Core;

	public string FunctionName;



void OnCollisionEnter(Collision collision) {

//		Debug.Log(  collision.transform.parent.name  +"Enter");

		Core.SendMessage( FunctionName );

		/*
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal, Color.white);
	}*/
}


void OnCollisionStay(Collision collisionInfo) {

//		Debug.Log(  collisionInfo.gameObject.name  +"Stay");
		Core.SendMessage( FunctionName+"_Stay" );

		/*foreach (ContactPoint contact in collisionInfo.contacts) {
			Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
		}*/
}


void OnCollisionExit(Collision collisionInfo) {



//		Debug.Log(  collisionInfo.transform.parent.name  +"Exit");
		Core.SendMessage( FunctionName+"_Exit" );
	//	print("No longer in contact with " + collisionInfo.transform.name);
}



//void On
void OnTriggerEnter(Collider other) 
{
		//Core.SendMessage( FunctionName );
		Core.SendMessage( FunctionName  , other.gameObject);
}

void OnTriggerStay(Collider other) 
{
		Core.SendMessage( FunctionName+"_Stay" );
		
}


void OnTriggerExit(Collider other) {
		Core.SendMessage( FunctionName+"_Exit" );

}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
