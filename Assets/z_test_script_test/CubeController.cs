using UnityEngine;
using System.Collections;


//------------------------------
// class definition
//------------------------------
public class CubeController : MonoBehaviour {

	// Use this for initialization
	public float rotationSpeed =10;

	
	
//------------------------------
// public mono method
//------------------------------	
	
	public void UpdateCube()
	{
		Vector3 eulerAngles = transform.eulerAngles; // 
		eulerAngles.y += Time.deltaTime *rotationSpeed;
		transform.eulerAngles = eulerAngles;
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
