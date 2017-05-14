using UnityEngine;
using System.Collections;


//------------------------------
// class definition
//------------------------------
public class CylinderController : MonoBehaviour {

	// Use this for initialization
	public float rotationSpeed =10;

	
	
	
	
	
//------------------------------
// public mono method
//------------------------------	
	
	public void UpdateCylinder()
	{
		//Vector3 eulerAngles = cylinder.transform.eulerAngles; // 
		//eulerAngles.x += Time.deltaTime *rotationSpeed;
		//cylinder.transform.eulerAngles = eulerAngles;
		
		
		Vector3 localScale = transform.localScale;
		localScale.y = Mathf.PingPong(Time.time,2);
		transform.localScale = localScale;
		
	}		
	
	
//------------------------------
// protected mono method
//------------------------------	
	protected void Awake()
	{
		//timeSpeed = 10;
		Debug.Log("Hellow from Awake:"+transform.name );
	}
	
	
	
	
	protected void Start () 
	{
		Debug.Log("Hellow from Start"+transform.name);	
	}
	
	// Update is called once per frame
	protected void Update () 
	{
		//Debug.Log("Hellow from Update");	
		
//		Vector3 eulerAngles = transform.eulerAngles; // 
//		eulerAngles.y += Time.deltaTime *rotationSpeed;
//		transform.eulerAngles = eulerAngles;
		
	}
}
