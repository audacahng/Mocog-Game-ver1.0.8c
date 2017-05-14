using UnityEngine;
using System.Collections;

public class CubeAction : MonoBehaviour {
	
	public int rotationSpeed=30;
	
	private delegate void UpdateDelegate(); /// function prototype
	private UpdateDelegate updateDelegate;
	private float elapseTime=0;
	

	void AnimationShow()
	{
		elapseTime +=Time.deltaTime;
		//Vector3 position = transform.position;
		//position.y = Mathf.Sin(elapseTime)*50;
		//transform.position = position;		
		
		Vector3 localScale = transform.localScale;
		localScale.y = Mathf.Sin(elapseTime)*10;
		localScale.x = Mathf.Sin(elapseTime)*10;
		transform.localScale = localScale;		
		
		
		if(elapseTime > 2)
			updateDelegate = DestroySelf;
	}	
	
	void DestroySelf()
	{
		updateDelegate = null;
		Destroy(gameObject,1);
	}
	
	
	// Use this for initialization
	void Start () {
		updateDelegate = null;
	}
	
    void OnTriggerEnter(Collider other) {
		
		if(updateDelegate!=null) return;
		
        //Destroy(other.gameObject);
		Debug.Log("Enter: "+this.name);
		ScoreController.AddScore(1);
		// do the delegate
		
		updateDelegate = AnimationShow;
		Destroy(GetComponent<Rigidbody>());
		
    }	
	
	
	public void UpdateCube()
	{
		Vector3 eulerAngles = transform.eulerAngles; // 
		eulerAngles.y += Time.deltaTime *rotationSpeed;
		transform.eulerAngles = eulerAngles;
	}		
	
	
	
	public void Bigger(bool bBigger)
	{
		Vector3 localScale = transform.localScale;
		if(bBigger)
		{
			localScale.y += 1;
			localScale.x += 1;
			localScale.z += 1;
		}
		else
		{
			localScale.y -= 1;
			localScale.x -= 1;
			localScale.z -= 1;			
		}
		transform.localScale = localScale;				
	}
	
	
	
	// Update is called once per frame
	void Update () {
		UpdateCube();
		
		
		
		
		if(updateDelegate != null)
		{
			updateDelegate(); // function pointer
		}		
		
		
		
		
	}
}
