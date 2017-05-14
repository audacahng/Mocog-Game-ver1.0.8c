using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public enum PathStyle { useArrayOrder, usePathPieceFrequency, useRandomFromArray };

public enum   PATH_SEGMENT
{
	STRAIGNT,
	STRAIGNT_LONG,
	SHIFT_LEFT01,
	SHIFT_LEFT02,
	SHIFT_LEFT03,

	SHIFT_RIGHT01,
	SHIFT_RIGHT02,
	SHIFT_RIGHT03,

	LEFT_15,
	RIGHT_15,

	LEFT_30,
	RIGHT_30,

	LEFT_45,
	RIGHT_45,

	LEFT_60,
	RIGHT_60,
	COUNT

};


public class PathiGenCS : MonoBehaviour {


	int   []Degree = new int[ (int)PATH_SEGMENT.COUNT  ];

	public PathStyle m_PathStyle  ;


	//public GameObject[] pathPieces ;			//This is an array of all the prefabs that will be used to create the generated path.
	public List<GameObject> pathPieces = new List<GameObject>();
	
	//public float[]  pathPieceFrequency   ;		//The size of this array must be equal to the size of the pathPieces array for the individual frequency values to line up properly.
	public List<float> pathPieceFrequency = new List<float>();


	public float totalFreq  = 0;		//This value is used to calculate the total value of all the frequency numbers in the pathPieceFrequency array
	
	public int pathPieceLimit ;				//Sets a limit to how many pathPieces can be in the scene at a time.
	public float newPathTime ;				//The delay, in seconds, before a new path piece is generated.


	public GameObject selectedPath ;
	int y ;					//This variable is used to determine the next pathPiece to select if the pathStyle is set to "useArrayOrder"

	public  Queue<GameObject> PathPool = new Queue<GameObject>();


	int ObjCount=0;

	public bool m_bStartRecycle=false;


	public float m_fRoadWidth = 1.0f;

	public int m_CurrentDegree=0;  // 90~-90



	public void CheckCurrentSegment( GameObject currentPath)
	{
		//Check the position 

		//ObjCount
		if(currentPath.GetComponent<PathSegmentBehaviour>() ==null) return;

//		Debug.Log ("currentPath:"+currentPath.name);
//		Debug.Log ("currentPath ID:"+currentPath.GetComponent<PathSegmentBehaviour>().id);

		if( ( ObjCount   -   currentPath.GetComponent<PathSegmentBehaviour>().id )  < 3 )
		{
			GeneratePath();
		}
	}




	IEnumerator initRoad()
	{

		//var x : int;	//This variable will be used in the following loop
		int x=0;	//This variable will be used in the following loop
		
		//If the pathPieceLimit is set to 0, the pathGenerator will treat it as no limit and will run infinitely

		Debug.Log("INIT the ROAD Generate:"+pathPieceLimit);
		if (pathPieceLimit != 0) {
			
			while (x <= pathPieceLimit){

//				Debug.Log("INIT the ROAD Generate:"+x);
				//yield return new WaitForSeconds(newPathTime);
				yield return new WaitForSeconds(0);
				GeneratePath();
				x++;
			}
		}
		
		//else 
		{


			{
				while (true){

					yield  return new WaitForSeconds(newPathTime);

					if(!m_bStartRecycle  ) continue;

					GeneratePath();

					if( PathPool.Count  > 120  )
					{
						//GameObject ObjToDestroy = PathPool[ PathPool.Count-1  ];
						//Debug.Log("Destroy Obj:"+ObjToDestroy.name);
						//PathPool.Remove( ObjToDestroy);


						GameObject.Destroy( PathPool.Dequeue() );
					}
				}
			}
		}

	}


	public void CleanAllPathes()
	{
		if(PathPool.Count==0  ) return;


		//while( PathPool.Count >0 && PathPool.Peek() )
		while( PathPool.Count >0   )
		{
			GameObject.Destroy( PathPool.Dequeue() );
//			Debug.Log("CleanAllPathes");
		}

	}



	public void AdjustProb_Straight( float fValue)
	{
		Debug.Log("AdjustProb_Straight:"+fValue);
		pathPieceFrequency[  (int)PATH_SEGMENT.STRAIGNT   ]= fValue;
		pathPieceFrequency[  (int)PATH_SEGMENT.STRAIGNT_LONG   ]= fValue;


		
	}

	public void AdjustProb_ShiftStraight( float fValue)
	{
		pathPieceFrequency[  (int)PATH_SEGMENT.SHIFT_LEFT01   ]= fValue;
		pathPieceFrequency[  (int)PATH_SEGMENT.SHIFT_LEFT02   ]= fValue;
		pathPieceFrequency[  (int)PATH_SEGMENT.SHIFT_LEFT03   ]= fValue;
		
		pathPieceFrequency[  (int)PATH_SEGMENT.SHIFT_RIGHT01  ]= fValue;
		pathPieceFrequency[  (int)PATH_SEGMENT.SHIFT_RIGHT02  ]= fValue;
		pathPieceFrequency[  (int)PATH_SEGMENT.SHIFT_RIGHT03   ]= fValue;
	}
	


	public void AdjustProb_15( float fValue)
	{
 
		pathPieceFrequency[  (int)PATH_SEGMENT.LEFT_15   ]= fValue;
		pathPieceFrequency[  (int)PATH_SEGMENT.RIGHT_15   ]= fValue;
 
	}

	public void AdjustProb_30( float fValue)
	{
		pathPieceFrequency[  (int)PATH_SEGMENT.LEFT_30   ]= fValue;
		pathPieceFrequency[  (int)PATH_SEGMENT.RIGHT_30   ]= fValue;		
	}

	public void AdjustProb_45( float fValue)
	{
		pathPieceFrequency[  (int)PATH_SEGMENT.LEFT_45   ]= fValue;
		pathPieceFrequency[  (int)PATH_SEGMENT.RIGHT_45   ]= fValue;				
	}

	public void AdjustProb_60( float fValue)
	{
		pathPieceFrequency[  (int)PATH_SEGMENT.LEFT_60   ]= fValue;
		pathPieceFrequency[  (int)PATH_SEGMENT.RIGHT_60   ]= fValue;				
	}


	public void GenerateRoad_Easy()
	{

		/*
		pathPieceFrequency[0] = 1; // straight
		pathPieceFrequency[1] = 1; // straightLong
		pathPieceFrequency[2] = 0.5f; // left 1
		pathPieceFrequency[3] = 0.5f; // left 2
		pathPieceFrequency[4] = 0.5f; // left 3
		pathPieceFrequency[5] = 0.5f; // right 1
		pathPieceFrequency[6] = 0.5f; // right 2
		pathPieceFrequency[7] = 0.5f; // right 3

		for(int i = 0 ; i <  pathPieceFrequency.Count ; i++)
		{
			if(i < 2)
				pathPieceFrequency[i]= 1;
			else
				pathPieceFrequency[i]= 0.2f ;
		}
		*/


		GenerateRoad();
	}

	public void GenerateRoad_Difficult()
	{
		/*
		pathPieceFrequency[0] = 0.1f; // straight
		pathPieceFrequency[1] = 0.1f; // straightLong
		pathPieceFrequency[2] = 0.9f; // left 1
		pathPieceFrequency[3] = 0.9f; // left 2
		pathPieceFrequency[4] = 0.8f; // left 3
		pathPieceFrequency[5] = 0.9f; // right 1
		pathPieceFrequency[6] = 0.8f; // right 2
		pathPieceFrequency[7] = 0.9f; // right 3
		*/

		/*
		for(int i = 0 ; i <  pathPieceFrequency.Count ; i++)
		{
			if(i < 2)
				pathPieceFrequency[i]= 1;
			else
				pathPieceFrequency[i]= 0.7f ;
			
		}*/


		GenerateRoad();
	}

	public void GenerateRoad()
	{
		if (m_PathStyle == PathStyle.usePathPieceFrequency) {
			
			//Add up all frequency values to determine TotalFrequency value
			for (int i = 0; i < pathPieceFrequency.Count; i++) {
				totalFreq += pathPieceFrequency[i];
			}
		}
		StartCoroutine( initRoad() );


	}


	// Use this for initialization
	void Start () {
	
		Degree[0]=0;
		Degree[1]=0;
		Degree[2]=0;
		Degree[3]=0;
		Degree[4]=0;
		Degree[5]=0;
		Degree[6]=0;
		Degree[7]=0;

		Degree[8]=15;
		Degree[9]=-15;
		Degree[10]=30;
		Degree[11]=-30;
		Degree[12]=45;
		Degree[13]=-45;

		Degree[14]=60;
		Degree[15]=-60;

		//initRoad();

		 


	}
	
	// Update is called once per frame
	void Update () {
	
	}





	void GeneratePath () {
		
		GameObject pathSpawnPoint = GameObject.Find("POINT roadEnd");			//Find the end connector of the last path piece generated and set it as the spawn point for the next path piece.
		//	Debug.Log("Spawn at:" +(pathSpawnPoint as Transform).name );
		
		
		
		Destroy(GameObject.Find("POINT roadEnd"));						//Destroy the end connector once it is no longer needed
		
		
		//PATH PIECE FREQUENCY --- Use pathPieceFrequency to determine next path piece
		if (m_PathStyle == PathStyle.usePathPieceFrequency) {
			float roll = Random.Range(0,totalFreq);				//Choose a number between 0 and the total frequency values

			int index=-1;
			int n =0;
			//Set the index value.  This will be used to choose which pathPiece is generated
			for ( n = 0; n < pathPieceFrequency.Count; n++) {
				if (roll <= pathPieceFrequency[n]) { 
					index = n; break; 
				}
				roll -= pathPieceFrequency[n];
			}
			
			if (index == -1) 
				//index = pathPieceFrequency.Count-1;	//In case the roll manages to be above the highest value, the roll is automatically set to the highest value
				index = 0;	

			//if( m_CurrentDegree +Degree[index]  > 90)
			if( m_CurrentDegree +Degree[index]  > 80)
			{
				index=0;
			}
			else
			//if( m_CurrentDegree +Degree[index]  <-90)
			if( m_CurrentDegree +Degree[index]  <-80)
			{
				index=0;
			}

			m_CurrentDegree = m_CurrentDegree + Degree[index];

			selectedPath = pathPieces[index];							//Sets the selectedPath piece based on the index value
			
		}
		
		/*
		//RANDOM FROM ARRAY --- Randomly pick a pathPiece from the array
		else if (pathStyle == pathStyle.useRandomFromArray) {
			
			selectedPath = pathPieces[Random.Range(0, pathPieces.Length)];						//Picks a pathPiece from the pathPieces array
			
		}
		
		
		//ARRAY ORDER --- Generate path pieces in the order they appear in the pathPieces array.  The order will cycle back to the beginning once it reaches the end
		else if (pathStyle == pathStyle.useArrayOrder) {
			
			if (y >= pathPieces.Length) {
				y = 0;
			}
			else {
				selectedPath = pathPieces[y];
				y++;
			}	
		}
		*/

//		Debug.Log("selectedPath:"+selectedPath.name);

		GameObject newRoad = Instantiate (selectedPath, pathSpawnPoint.transform.position, pathSpawnPoint.transform.rotation) as GameObject;
		newRoad.name = newRoad.name + "_"+ObjCount;
		//newRoad.transform.localScale = new Vector3(  m_fRoadWidth , newRoad.transform.localScale.y , newRoad.transform.localScale.z);
//		Debug.Log("m_fRoadWidth = "+m_fRoadWidth);
		newRoad.transform.localScale = new Vector3(  m_fRoadWidth , m_fRoadWidth , m_fRoadWidth);

		newRoad.AddComponent<PathSegmentBehaviour>();
		newRoad.GetComponent<PathSegmentBehaviour>().id = ObjCount;
		//PathSegmentBehaviour


		ObjCount+=1;
		PathPool.Enqueue(newRoad);
	}


	public void AdjustRoadWidth( float fWidth)
	{


		m_fRoadWidth = fWidth;
		Debug.Log("Width:" + m_fRoadWidth);

		/*
		foreach ( GameObject PathObj in PathPool)
		{
			PathObj.transform.localScale = new Vector3(  m_fRoadWidth , PathObj.transform.localScale.y , PathObj.transform.localScale.z);
		}*/


	}
	//{}



}
