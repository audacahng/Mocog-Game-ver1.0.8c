using UnityEngine;
using System.Collections;

public class CameraMovControllrt : MonoBehaviour {
	
	public Texture2D textureAssign;
	Color col;
	
	// Use this for initialization
	void Start () {
		
		
		iTween.ShakePosition(gameObject, iTween.Hash("x", 1, "onComplete", "ShakeComplete", "onCompleteTarget", gameObject));				

		col = Color.black;
		//iTween.CameraFadeAdd(textureAssign);
		//iTween.CameraFadeFrom(1,4.0f);
		
		//yield  return new  WaitForSeconds(4);
		 //iTween.ColorTo(gameObject, col, 4.0f);
		//iTween.CameraFadeTo(1, 4.0f);
		

		
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnFinishFadeIn()
	{
		Debug.Log(" Finish Fade in");
		
	}
	
	
	void ShakeComplete()
	{
			Debug.Log(" ShakeComplete");
	}
	
	
}
