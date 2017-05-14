using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AnimationOrTween;


public class Controller_Tutorial : FLEventDispatcherMono {


	int m_CurrentPage=0;
	int m_TotalPage=0;

	public GameObject m_ButtonNext;
	public GameObject m_ButtonStartGame;

	public GameObject Dialog;
	public UILabel m_DialogContents;

	public bool m_bTestCase = false;

	public List<string>   m_TextContent;

	private GameObject m_target=null;
	private string m_FuncCallWhenFinish="";
	private object m_callValue=null;







	// Use this for initialization
	void Start () {
	
	}

	
	
	public void ShowDialogContent( List<string>   TextContent   , GameObject target= null,string callWhenFinished="" , object parameter=null    )
	{
		
		
		m_target = target;
		m_FuncCallWhenFinish = callWhenFinished;
		m_callValue = parameter;
		
		if( TextContent.Count==0)
		{
			//GameStartFinish();
			return;
		}
		
		
		
//		UI_BGDark.SetActive(true);
		
		m_CurrentPage = 0;
		m_TextContent = TextContent ;
		m_TotalPage =  m_TextContent.Count;
		
		m_DialogContents.text = m_TextContent[m_CurrentPage]   ;
		
		Dialog.SetActive(true);
		
		Animation Anm = Dialog.GetComponent<Animation>();
		Direction dir = Direction.Forward;
		ActiveAnimation anim0;
		anim0 = ActiveAnimation.Play(Anm, "DialogPopUp", dir, EnableCondition.EnableThenPlay, DisableCondition.DoNotDisable);
		//ActiveAnimation.Play(  Anm, AnimationOrTween.Direction.Forward);
		anim0.eventReceiver = gameObject;
		anim0.callWhenFinished = "ShowText";
		m_DialogContents.gameObject.SetActive(false);
		
		
		
		if(m_CurrentPage==m_TotalPage-1)
		{
			m_ButtonNext.SetActive(false);
			m_ButtonStartGame.SetActive(true);			
		}
		else
		{
			m_ButtonNext.SetActive(true);
			m_ButtonStartGame.SetActive(false);
		}
		
		
	}
	void ShowText() // after pop up , show the thext content
	{
		m_DialogContents.gameObject.SetActive(true);
	}




	public void PageNext()
	{
		m_CurrentPage+=1;
		//m_TutorialPic.mainTexture = Resources.Load("Texture_Tutorial/" + PageTutorArray[ m_CurrentPage  ] ) as Texture2D;
		m_DialogContents.text = m_TextContent[m_CurrentPage]   ;
		
		if(m_CurrentPage==m_TotalPage-1)
		{
			m_ButtonNext.SetActive(false);
			m_ButtonStartGame.SetActive(true);			
		}
		
	}


	public void TutorFinish()
	{
		
		Animation Anm = Dialog.GetComponent<Animation>();
		Direction dir = Direction.Reverse;
		ActiveAnimation anim0;
		//anim0 = ActiveAnimation.Play(Anm, "DialogPopUp", dir, EnableCondition.EnableThenPlay, DisableCondition.DoNotDisable);
		anim0 = ActiveAnimation.Play(Anm, "DialogPopUp", dir, EnableCondition.EnableThenPlay, DisableCondition.DoNotDisable);
		anim0.eventReceiver = gameObject;
		anim0.callWhenFinished = "TutorFinish_Callback";
	}
	
	void TutorFinish_Callback()
	{
		//UI_BGDark.SetActive(false);
		Dialog.SetActive(false);
		//gameObject.SetActive(false);
		
		if( m_target!=null )
		{
			//m_target.SendMessage( m_FuncCallWhenFinish,true , SendMessageOptions.DontRequireReceiver);
			m_target.SendMessage( m_FuncCallWhenFinish,  m_callValue  , SendMessageOptions.DontRequireReceiver);
			//m_callValue
			// reset
			m_target =  null;
			m_FuncCallWhenFinish = "";
		}
		
	}


	// Update is called once per frame
	void Update () {
	

		if(m_bTestCase)
		{
			m_bTestCase = false;
			ShowDialogContent(m_TextContent);
		}


	}
}
