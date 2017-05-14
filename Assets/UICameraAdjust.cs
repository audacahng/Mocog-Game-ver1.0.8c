using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UICamera))]
public class UICameraAdjust : MonoBehaviour {

	public float standard_width = 1920;
	public float standard_height = 1080f;
	
	
	float device_width = 0f;
	float device_height = 0f;
	
	
	
	void Awake()
	{
		device_width = Screen.width;
		device_height = Screen.height;
		SetCameraSize();
		
		
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	private void SetCameraSize()
	{
		
		//Debug.Log( "screen:"  )
		
		float adjustor = 0f;
		float standard_aspect = standard_width / standard_height;
		float device_aspect = device_width / device_height;
		
		if (device_aspect < standard_aspect)
		{
			adjustor = standard_aspect / device_aspect;
			GetComponent<Camera>().orthographicSize = adjustor;
		}
	}
	

}
