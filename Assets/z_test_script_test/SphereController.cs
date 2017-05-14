using UnityEngine;
using System.Collections;


//------------------------------
// class definition
//------------------------------
public class SphereController : MonoBehaviour {

	// Use this for initialization
	public float rotationSpeed =10;
	
	
	private delegate void UpdateDelegate(); /// function prototype
	private UpdateDelegate updateDelegate;
	private float elapseTime;
	

	protected void OnMouseUpAsButton()
	{
		elapseTime = 0; 
		updateDelegate = UpdateSphere;
	}
	
	
	
	public void UpdateSphere()
	{
		//localScale.y = Mathf.Sin(Time.time);
		
		elapseTime +=Time.deltaTime;
		if(elapseTime > 2 ) // small state machine !
		{
			updateDelegate = null;
		}
		
		Vector3 position = transform.position;
		position.y = Mathf.Sin(Time.time);
		transform.position = position;		
		
		
		//Vector3 scale = sphere.transform.localScale;
		//scale.y =  Mathf.PingPong(Time.time,4);
		//sphere.transform.localScale = scale;
	}	
		
	
	
	
//------------------------------
// public mono method
//------------------------------	
	
	
	
	
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
		
		updateDelegate = UpdateSphere;
		
		
	}
	
	// Update is called once per frame
	protected void Update () 
	{
		//Debug.Log("Hellow from Update");	
		
//		Vector3 eulerAngles = transform.eulerAngles; // 
//		eulerAngles.y += Time.deltaTime *rotationSpeed;
//		transform.eulerAngles = eulerAngles;
		
		
		//UpdateSphere();
		
		if(updateDelegate != null)
		{
			updateDelegate(); // function pointer
		}
		
		
	}
}
