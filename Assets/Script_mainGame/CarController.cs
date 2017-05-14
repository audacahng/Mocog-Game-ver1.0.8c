using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
	
	
	public int Speed = 60;
	public int Speed_LR = 30;

	public int Speed_LowLimit =30;
	public int Speed_UpLimit =60;


	public int m_TotalDistance=0;


	public bool isMove = false;
	private bool bBigger = false;
	
	//public Road RoadGenerator;

	bool bLeftCollide = false;
	bool bRightCollide = false;


	public bool  m_bTest_LimitlessBlood=false;

	public MainGameWorld MainGameCore;
 
	float m_bTimer=0; // Speed

	//float m_bTimer_Distance=0; // Distance
	
	bool m_BtnPress_Left=false;
	bool m_BtnPress_Right=false;


	Vector3 Acc_DIR = Vector3.zero;



	public AudioClip SoundClip;
	public AudioClip BGM;
	public AudioClip BGM2;
	public AudioSource SoundSource;	




	private delegate void UpdateDelegate(); /// function prototype
	private UpdateDelegate updateDelegate;	
	float m_bTimer_5SecCount=0f;

	float m_bTimer_0point2SecCount=0f;

	public bool bTestTranslate = false;

	bool m_bResumeSlowFlag = false;
	float m_fResumeSlowCounter = 0f;


	void BehaviorCheckEvery5Sec()
	{
		m_bTimer_5SecCount+=Time.deltaTime;
		m_bTimer_0point2SecCount+=Time.deltaTime;


		if( m_bTimer_5SecCount >=1)
		{
			m_bTimer_5SecCount=0;


			xMain.Instance.WriteLogIntoFile("C-Z,"  +  ( -1* transform.position.z).ToString("0.000")  , true);
			//xMain.Instance.WriteLogIntoFile("Car-Rot," + (transform.eulerAngles.y-180)  .ToString("0.000"), true);
			
		}


		if( m_bTimer_0point2SecCount >=0.2f)
		{
			m_bTimer_0point2SecCount=0;
			xMain.Instance.WriteLogIntoFile("SKew," + (Acc_DIR.x).ToString("0.000"), true);
			xMain.Instance.WriteLogIntoFile("C-X," + transform.position.x.ToString("0.000")  , true);
		}




	}

	public void UI_BtnPress_Left()
	{
		m_BtnPress_Left = true;
	}
	public void UI_BtnRelease_Left()
	{
		m_BtnPress_Left=false;
	}

	public void UI_BtnPress_Right()
	{
		m_BtnPress_Right = true;
	}
	public void UI_BtnRelease_Right()
	{
		m_BtnPress_Right = false;
	}


	// Use this for initialization
	void Start () {
	
		MainGameCore = GameObject.Find("core").GetComponent<MainGameWorld>();

		SoundSource = gameObject.AddComponent<AudioSource>();		
		SoundSource.volume = 1f;

	}
	
	// Update is called once per frame
	//protected void FixedUpdate () 
	protected void LateUpdate () 
	{
		/*
		//if(Input.GetKeyDown(KeyCode.UpArrow))
		if(Input.GetKey(KeyCode.UpArrow))			
		{
			isMove = true;
			//Vector3 position = 
			transform.position += transform.forward*(Time.deltaTime* Speed);
		}
		else
		if(Input.GetKey(KeyCode.DownArrow))						
		{
			isMove = true;
			transform.position -= transform.forward*(Time.deltaTime* Speed);
		}
		else
			isMove = false;
		
		*/

		if(bTestTranslate == true)
		{
			bTestTranslate = false;
			isMove = false;
			//transform.position+=  Vector3.forward*(4.0f);
			transform.Translate(- Vector3.forward*(4.0f) , Space.Self);

			//Hashtable MoveSetting = iTween.Hash( "time", 2.0f , "position" , TileNext.midpoint_Tri02,"easetype","linear");
			//iTween.MoveTo(  gameObject , MoveSetting );

		}


		
		Speed_Resume();

		if(isMove)
		//transform.position += transform.forward*(Time.deltaTime* Speed*0.5f);
		{
			//if(bLeftCollide == true || bRightCollide== true) return;

			transform.position += transform.forward*(Time.deltaTime* Speed*0.25f);

			updateDelegate = BehaviorCheckEvery5Sec;
		}
		else
		{
			updateDelegate= null;
			return;
		}
	




		#if UNITY_ANDROID || UNITY_IOS
		Acc_DIR.x = Input.acceleration.x;
		Acc_DIR.y = -Input.acceleration.y;
		Acc_DIR.z = Input.acceleration.z;
		if( Acc_DIR.sqrMagnitude >1)
			Acc_DIR.Normalize();

		//transform.eulerAngles += Acc_DIR.x * Vector3.up * (Time.deltaTime*Speed) * 1.3f;
		transform.eulerAngles += Acc_DIR.x * Vector3.up * (Time.deltaTime*Speed) * 1.9f;






		#endif




		if(Input.GetKey(KeyCode.LeftArrow)  || m_BtnPress_Left)			
		{
			//if(!isMove) return;
			


			Btn_TurnLeft();
			
		}
		else
			if(Input.GetKey(KeyCode.RightArrow)  || m_BtnPress_Right )						
		{
			//if(!isMove) return;


			Btn_TurnRight();

		}		
		
		
		/*
		if(Input.GetKey(KeyCode.Space))
		{
			GameObject objGood =GameObject.Find("ObjGood");
		
			
			bBigger = !bBigger;
			if(bBigger)
			{
					objGood.BroadcastMessage("Bigger",true);
			}
			else
			{
					objGood.BroadcastMessage("Bigger",false);
			}
		}*/

		if(updateDelegate != null)
		{
			updateDelegate(); // function pointer
		}        	


		
	}


	public void Btn_TurnLeft()
	{
		if(!bLeftCollide)
		{
			//transform.position -=  Vector3.left * (Time.deltaTime*Speed_LR*0.5f);  // Shift Left


			//Vector3 eulerAngles = transform.eulerAngles;
			//eulerAngles.y+= Time.deltaTime* 20;
			//transform.eulerAngles = eulerAngles; 



			// rotate Left
			transform.eulerAngles -= Vector3.up * (Time.deltaTime*Speed*2f);


			/*if(transform.eulerAngles.y <180) 
				transform.eulerAngles = new Vector3( transform.eulerAngles.x,180,transform.eulerAngles.z);
			else
			if(transform.eulerAngles.y >220) 
					transform.eulerAngles = new Vector3( transform.eulerAngles.x,220,transform.eulerAngles.z);*/

		}
	}


	public void Btn_TurnRight()
	{
		if( !bRightCollide)
		{
		//	transform.position +=  Vector3.left * (Time.deltaTime*Speed_LR*0.5f);

			// rotate right
					Vector3 eulerAngles = transform.eulerAngles;
					eulerAngles.y+= (Time.deltaTime* Speed*2f);
					transform.eulerAngles = eulerAngles; 

			/*if(transform.eulerAngles.y <180) 
				transform.eulerAngles = new Vector3( transform.eulerAngles.x,180,transform.eulerAngles.z);
			else
				if(transform.eulerAngles.y >220) 
					transform.eulerAngles = new Vector3( transform.eulerAngles.x,220,transform.eulerAngles.z);*/

		}

	}


	void Speed_SlowDown()
	{
		if(  !isMove) return;

		if(m_bTest_LimitlessBlood) return;

		m_bResumeSlowFlag = true;
		m_fResumeSlowCounter=0f;


		if( Speed >= Speed_LowLimit)
		{
			//Speed = Speed_LowLimit+1;
			Speed-=15;

		}
	}


	void Speed_Resume()
	{
		//if( Speed > Speed_UpLimit ) return;

		if(  !isMove) return;

		if( m_bResumeSlowFlag == true){
			m_fResumeSlowCounter+=Time.deltaTime;
			if( m_fResumeSlowCounter >= 5f)
			{
				m_bResumeSlowFlag= false;
				m_fResumeSlowCounter=0f;
			}
			else
				return;
		}



		m_bTimer+=Time.deltaTime;
		//Debug.Log("m_bTimer:"+m_bTimer);

		if( m_bTimer >=0.2f )
		{
			m_bTimer=0;


			// Calculate the Distance
			m_TotalDistance +=  Speed/10;

			//Debug.Log("Speed:"+Speed +",Speed Up:"+ Speed_UpLimit);
			if( Speed <= Speed_UpLimit )
			{
				//Debug.Log("Speed Up:" +Speed );
				Speed +=1;
			}

			if(  Speed  >  (Speed_LowLimit + (Speed_UpLimit  - Speed_LowLimit) /2)   )
				MainGameCore.GameTimer_Resume();
			else
				MainGameCore.GameTimer_Pause();
		}

	}

	void OnCollisionEnter(Collision collision) {
		Speed_SlowDown();
	}

	void Collide_Left()
	{
		bLeftCollide = true;
		//transform.position +=  Vector3.left * (2.5f);
		transform.Translate( -Vector3.left * (1.5f) , Space.Self);
		transform.Translate(- Vector3.forward*(2.0f) , Space.Self);
		Debug.Log("Collide_Left");



		#if UNITY_ANDROID
		Handheld.Vibrate ();
		#endif

		BGMPlayer_Play("dead",null,false);


		if(bLeftCollide == true && bRightCollide == true)
		{
			Debug.Log("Collide LR-L");
			//transform.position+=  Vector3.forward*(4.0f);
			transform.Translate(- Vector3.forward*(2.0f) , Space.Self);
			//Speed = Speed_LowLimit-1;
			Speed = 0;

			Speed_SlowDown();
			return;
		}


		Vector3 eulerAngles = transform.eulerAngles;
		eulerAngles.y+= 15;
		transform.eulerAngles = eulerAngles; 



		Speed_SlowDown();

	}

	void Collide_Left_Stay()
	{}

	void Collide_Left_Exit()
	{

		bLeftCollide= false;

		if(m_bTest_LimitlessBlood) return;

		MainGameCore.BloodDamageUpdate(0.1f, "LC");

	}


	void Collide_Right()
	{
		#if UNITY_ANDROID
		Handheld.Vibrate ();
		#endif

		BGMPlayer_Play("dead",null,false);


		//transform.position -=  Vector3.left * (2.5f);
		transform.Translate( Vector3.left * (1.5f) , Space.Self  );
		transform.Translate(- Vector3.forward*(2.0f) , Space.Self);

		//Debug.Log("Collide_Right");
		bRightCollide = true;


		if(bLeftCollide == true && bRightCollide == true)
		{
			Debug.Log("Collide LR-R");
			//transform.Translate( -Vector3.forward*(4.0f) );
			//transform.position+=  Vector3.forward*(4.0f);
			transform.Translate(- Vector3.forward*(2.0f) , Space.Self);

			//Speed = Speed_LowLimit-1;
			Speed = 0;
			Speed_SlowDown();
			return;
			
		}

		//transform.position-=  Vector3.forward*(1.5f);
		Vector3 eulerAngles = transform.eulerAngles;
		eulerAngles.y-= 15;
		transform.eulerAngles = eulerAngles; 



		Speed_SlowDown();
	}


	void Collide_Right_Stay()
	{

	}
	
	void Collide_Right_Exit()
	{
		bRightCollide = false;

		if(m_bTest_LimitlessBlood) return;
		MainGameCore.BloodDamageUpdate(0.1f, "RC");
	}


	/*
	void AddRoadPoint( Vector3 v)
	{
		
		//RoadGenerator.points
		
		if(RoadGenerator.insertPoint < 0 || RoadGenerator.insertPoint > RoadGenerator.points.Count)
			RoadGenerator.points.Add(v);
		else
			RoadGenerator.points.Insert(RoadGenerator.insertPoint, v);
			//RoadGenerator.Refresh();
	}
	*/

	public void UI_SetSpeedUpLimit( float fValue)
	{
		Debug.Log("UI_SetSpeedUpLimit:"+fValue);
		Speed_UpLimit =(int) (fValue);
	}
	

	public void BGMPlayer_Play( string sBGM , string sBGM2 , bool bLoopMode)
	{
		SoundSource.Stop();
		
		BGM = (AudioClip)Resources.Load("Sound/"+sBGM);
		BGM2 = (AudioClip)Resources.Load("Sound/"+sBGM2);
		
		if(BGM2==null)
		{
			SoundSource.clip = BGM;
			SoundSource.loop = bLoopMode;
			SoundSource.Play();
			
//			Debug.Log("Play Sound");
		}
		else
			iTween.Stab( gameObject , iTween.Hash( "audioclip" ,BGM,"oncomplete","PlayBGMCompleted","oncompletetarget",gameObject));							
		
	}
	
	void PlayBGMCompleted()
	{
		SoundSource.clip = BGM2;
		SoundSource.loop = true;
		SoundSource.Play();
	}




}
