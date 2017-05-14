using UnityEngine;
using System.Collections;

public class CollideContactTest : MonoBehaviour {

	public bool bTestAddForce_Left= false;
	public bool bTestAddForce_Right= false;

	public Transform explosionPrefab;
	void OnCollisionEnter(Collision collision) {



		ContactPoint contact = collision.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		//Instantiate(explosionPrefab, pos, rot) as Transform;
		Debug.Log("Pos:" + pos.x+","+pos.y+","+ pos.z);




		Instantiate(explosionPrefab, pos, rot) ;




	//	Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if(bTestAddForce_Left)
		{
			bTestAddForce_Left=false;

			GetComponent<Rigidbody>().AddForce(-200, 150, 0);

		}


		if(bTestAddForce_Right)
		{
			bTestAddForce_Right=false;

			GetComponent<Rigidbody>().AddForce(200, 150, 0);

		}



	}



}
