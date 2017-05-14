using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{	
	public Transform target;
	Quaternion rotation;
	
	
	void Start(){
		//iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
		
		//iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "time", 1, "delay", 1));// easeinoutsine  pingPong
		//iTween.MoveBy(gameObject, iTween.Hash("y", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "time", 1, "delay", 3));
		
		
		 //iTween.RotateTo(gameObject,iTween.Hash("rotation",target.position,"easeType",iTween.EaseType.easeInOutSine,"loopType", "loop","time",0.1f ,"delay", 6));
		// iTween.RotateTo(gameObject,iTween.Hash("x",target.rotation.x,"y",target.rotation.y,"easeType",iTween.EaseType.easeInOutSine,"loopType", "loop","time",0.1f ,"delay", 3));
	}
	
	
	void Update()
	{
		//transform.LookAt(target);

		//rotation  = Quaternion.Lerp(transform.rotation, target.rotation, Time.time * 1);
		//transform.rotation = new Quaternion(rotation.x,  rotation.y,  transform.rotation.z,0);
		
		/*
		Vector3 relativePos = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation( relativePos);
		transform.rotation =   new Quaternion( transform.rotation.x,  rotation.y,transform.rotation.z,rotation.w);
		*/
		
	}
	
	
	void OnGUI()
	{
			if( GUI.Button( new Rect(  0,0,180,20 ), "rotate"   )  )
			{
				 //iTween.RotateTo(gameObject,iTween.Hash("x",target.rotation.x,"y",target.rotation.y,"easeType",iTween.EaseType.easeInOutSine,"loopType", "loop","time",0.1f ,"delay", 3));	
				//iTween.RotateBy(

			//iTween.RotateBy(	gameObject, iTween.Hash( "x", .25,"y", 0f, "easeType", "easeInOutBack",  "delay", .0 )  );			
			iTween.RotateBy(	gameObject, iTween.Hash( "x", .05,"y", 0f,  "delay", .0 )  );			
			
			}
		
	}
	
	
}

