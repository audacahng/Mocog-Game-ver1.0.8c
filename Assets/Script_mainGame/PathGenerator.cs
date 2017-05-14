using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathGenerator : MonoBehaviour {


	public Transform startPoint;

	public GameObject[] pathiGenExamples;
	public GameObject startingPath;

	public GameObject PathGen;
	public List<float> pathPieceFrequency_bak ;

	private int m_RoadLevel=0;

	public float m_fRoadWidth=1f;


	public string m_sCurrentSegmentName="";

//	var pathiGenExamples : GameObject[];
//	var startingPath : GameObject[];

	void Awake()
	{

//		var newPathStyle = Instantiate (selectedPathStyle, startPoint.transform.position, startPoint.transform.rotation);
//		var newPathStart = Instantiate (selectedPathStart, startPoint.transform.position, startPoint.transform.rotation);
		/*
		PathGen = GameObject.Instantiate( pathiGenExamples[0], startPoint.position , startPoint.rotation) as GameObject;
		GameObject StartPathSegment = GameObject.Instantiate( startingPath, startPoint.position , startPoint.rotation) as GameObject;

		PathGen.GetComponent<PathiGenCS>().PathPool.Enqueue(StartPathSegment);*/

	//	GameObject tempNext = GameObject.Find("POINT roadEnd");
		//if(tempNext) Debug.Log("tempNext.name="+tempNext.transform.parent.name);

		GenerateRoad_Easy();
	}



	public void GenerateRoad_Easy()
	{
		m_RoadLevel = 0 ;
		GenerateRoad();
	}

	public void GenerateRoad_Difficult()
	{
		m_RoadLevel = 1 ;
		GenerateRoad();
	}

	public void GenerateRoad()
	{
		pathPieceFrequency_bak = new List<float>()  ;
		bool bFirstTimeGenerate = true;

		if(PathGen!=null)
		{
			Debug.Log("Re-Generate new PathGen");
			bFirstTimeGenerate= false;
			pathPieceFrequency_bak = new List<float>(  PathGen.GetComponent<PathiGenCS>().pathPieceFrequency ) ;

			PathGen.GetComponent<PathiGenCS>().CleanAllPathes();
		//	pathPieceFrequency_bak = PathGen.GetComponent<PathiGenCS>().pathPieceFrequency;

			// Before Destroy , BackUp the Setting 
			GameObject.Destroy( PathGen);
		}
		else
			bFirstTimeGenerate = true;


		// Reset all the Road;
		//PathGen = GameObject.Instantiate( pathiGenExamples[0], startPoint.position , startPoint.rotation) as GameObject;
		PathGen = GameObject.Instantiate( pathiGenExamples[1], startPoint.position , startPoint.rotation) as GameObject;

		if(bFirstTimeGenerate == false)
		{
//			foreach( float bakValue in pathPieceFrequency_bak)
//				Debug.Log("value: "+bakValue);

			PathGen.GetComponent<PathiGenCS>().pathPieceFrequency = new List<float>(pathPieceFrequency_bak);
			Debug.Log("----------");

//			foreach( float bakValue in PathGen.GetComponent<PathiGenCS>().pathPieceFrequency)
//				Debug.Log("valueAfter: "+bakValue);

		}
			

		GameObject StartPathSegment = GameObject.Instantiate( startingPath, startPoint.position , startPoint.rotation) as GameObject;
		
		PathGen.GetComponent<PathiGenCS>().PathPool.Enqueue(StartPathSegment);
		StartPathSegment.transform.localScale = new Vector3( m_fRoadWidth,1,1);
		//m_fRoadWidth


		//m_fRoadWidth
		PathGen.GetComponent<PathiGenCS>().AdjustRoadWidth( m_fRoadWidth );

		if(m_RoadLevel ==0)
			PathGen.GetComponent<PathiGenCS>().GenerateRoad_Easy();
		else
			PathGen.GetComponent<PathiGenCS>().GenerateRoad_Difficult();
	}


	public void AdjustWidth( float fValue)
	{
		Debug.Log("AdjustWidth:" + fValue);
		m_fRoadWidth = fValue;
	}

	public void AdjustProb_Straight(float fValue)
	{
		PathGen.GetComponent<PathiGenCS>().AdjustProb_Straight( fValue);
	}

	public void AdjustProb_ShiftStraight(float fValue)
	{
		PathGen.GetComponent<PathiGenCS>().AdjustProb_ShiftStraight( fValue);
	}



	public void AdjustProb_15(float fValue)
	{
		PathGen.GetComponent<PathiGenCS>().AdjustProb_15( fValue);
	}

	public void AdjustProb_30(float fValue)
	{
		PathGen.GetComponent<PathiGenCS>().AdjustProb_30( fValue);
	}

	public void AdjustProb_45(float fValue)
	{
		PathGen.GetComponent<PathiGenCS>().AdjustProb_45( fValue);
	}

	public void AdjustProb_60(float fValue)
	{
		PathGen.GetComponent<PathiGenCS>().AdjustProb_60( fValue);
	}


	public string GetRoadProbData()
	{
		string sProb="Prob:";
		for( int i = 0 ; i < (int)PATH_SEGMENT.COUNT ; i++)
		{
			sProb+= ( (PATH_SEGMENT)i ).ToString() + ":";
			sProb += PathGen.GetComponent<PathiGenCS>().pathPieceFrequency[i] + ",";
		}

		sProb+=";" + "Width:" +m_fRoadWidth + ";";


		return sProb;



	}


	public void StartAutoGenerator()
	{
		if(PathGen)
			PathGen.GetComponent<PathiGenCS>().m_bStartRecycle = true;
	}


	public void StopAutoGenerator()
	{
		if(PathGen)
			PathGen.GetComponent<PathiGenCS>().m_bStartRecycle = false;
	}


	//PathGen
	public void CurrentPathSemgent(GameObject value)
	{

		if(value == null) return;

		m_sCurrentSegmentName = value.transform.parent.gameObject.name;

		//Debug.Log("CurrentPathSemgent_Enter:"+    value.transform.parent.gameObject.name );
		if(PathGen)
			PathGen.GetComponent<PathiGenCS>().CheckCurrentSegment(  value.transform.parent.gameObject );
		//CheckCurrentSegment

		//PathGen.GetComponent<PathiGenCS>()

	}

	public void CurrentPathSemgent_Stay()
	{

	}

	public void CurrentPathSemgent_Exit()
	{

	}




	// Update is called once per frame
	void Update () {
	
	}
}
