using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public GameObject cube;
	public GameObject cylinder;
	public GameObject sphere;
	
	public float rotationSpeed =10;
	
	private CubeController cc;
	private CylinderController cyc;
	
	protected void Awake()
	{
		//timeSpeed = 10;
		//Debug.Log("Hellow from Awake:"+transform.name );
	}
	
	
	// Use this for initialization
	void Start () {
	
		cc = ( CubeController) cube.GetComponent( typeof(CubeController) );
		cyc = (CylinderController) cylinder.GetComponent( typeof(CylinderController));
	}
	
	// Update is called once per frame
	void Update () {
	
		if(cc != null)		
		{
			cc.UpdateCube();
		}
		
		//cc.UpdateCube();

		
		//cyc.UpdateCylinder();
		//cyc.BroadcastMessage("UpdateCylinder"); /// send message to this gameobject ,and all it's children ,  maybe no body answer, but no crash !
		cylinder.BroadcastMessage("UpdateCylinder"); /// send message to this gameobject ,and all it's children ,  maybe no body answer, but no crash !		
		
		
		//UpdateCylinder();
		//UpdateCube();
		//UpdateSphere();
		
		
	}
	
	

	

	

		
}
