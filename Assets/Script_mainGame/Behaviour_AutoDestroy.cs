using UnityEngine;
using System.Collections;

public class Behaviour_AutoDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("AutoDestroy");
	}

	IEnumerator AutoDestroy()
	{
		while(true)
		{
				yield return new WaitForSeconds(0.5f);
				GameObject.Destroy(this.gameObject);
				break;

		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
