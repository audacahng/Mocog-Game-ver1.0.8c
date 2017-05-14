using UnityEngine;
using System.Collections;

public class RoadTestAdjustSlider : MonoBehaviour {

	public PATH_SEGMENT  m_eRoadType = PATH_SEGMENT.STRAIGNT;
	//public bool bRoadWidthSetting=false;
	///GameObject m_PathGenerator;
	// AdjustRoadWidth



	public UILabel m_Label;

	public string m_sValueUnit="%";

	public float value_lowLimit=0f;
	public float value_UpLimit=1f;

	public float value_init =1f;


	public GameObject Target;
	public string TargetFuncName;


	float UpdateCurrentValueShow()
	{
		// 1. find the UILabel
		float fValue = ( value_lowLimit + (value_UpLimit - value_lowLimit )  * gameObject.GetComponent<UISlider>().value );
		m_Label.text =  fValue + m_sValueUnit;

		return fValue;

	}


	// Use this for initialization
	void Start () {
//		m_PathGenerator = GameObject.Find("PathGenerator");

		gameObject.GetComponent<UISlider>().value=  (value_init -value_lowLimit)  /  (value_UpLimit - value_lowLimit )  ;
		UpdateCurrentValueShow();
	}


	public void OnValueChange()
	{
		//Debug.Log(gameObject.GetComponent<UISlider>().value);
		//gameObject.GetComponent<UISlider>().value;

		UpdateCurrentValueShow();

		float fValueToSend = UpdateCurrentValueShow();
		//Debug.Log(  fValueToSend );

		//	m_PathGenerator.GetComponent<PathGenerator>().m_fRoadWidth = value_lowLimit+  (value_UpLimit - value_lowLimit )  * gameObject.GetComponent<UISlider>().value ;
		Target.SendMessage( TargetFuncName , fValueToSend);

		/*
		GameObject PathGen = m_PathGenerator.GetComponent<PathGenerator>().PathGen;
		if(PathGen)
			PathGen.GetComponent<PathiGenCS>().AdjustRoadWidth(   gameObject.GetComponent<UISlider>().value  );
*/


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
