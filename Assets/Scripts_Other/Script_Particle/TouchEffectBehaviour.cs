using UnityEngine;
using System.Collections;

public class TouchEffectBehaviour : MonoBehaviour {
	
//	int mShowSecond;
	
	float fSecStart=0f;


	public float m_TimeToKeep = 1f;
	public float m_TimeToKill = 4f;
	
	//public GameObject Emitter;
	
	enum PARTICLE_STATUS{
		PARTICLE_STATUS_SHOW,
		PARTICLE_STATUS_STOP,
		PARTICLE_STATUS_DESTROY
	};
	
	PARTICLE_STATUS m_ParticleStatus;
	
	// Use this for initialization
	void Start () {
		transform.name = "ParticleEmitter_"+ (int)Time.time;
		m_ParticleStatus = PARTICLE_STATUS.PARTICLE_STATUS_SHOW;

		fSecStart = 0;
	}
	
	// Update is called once per frame
	void Update () {
		fSecStart += Time.deltaTime;

		if(  fSecStart  > m_TimeToKeep)
			m_ParticleStatus = PARTICLE_STATUS.PARTICLE_STATUS_STOP;
		
		if(  fSecStart  > m_TimeToKill)
			m_ParticleStatus = PARTICLE_STATUS.PARTICLE_STATUS_DESTROY;		
		

		if (m_ParticleStatus == PARTICLE_STATUS.PARTICLE_STATUS_STOP)
		{
			ParticleSystem ps = transform.GetComponent<ParticleSystem> ();
			if(ps)
				ps.enableEmission= false;

			foreach (Transform child in transform) {
				ps = child.GetComponent<ParticleSystem> ();
				if(ps)
					ps.enableEmission= false;
			}


		}
		
		if (m_ParticleStatus == PARTICLE_STATUS.PARTICLE_STATUS_DESTROY)
		{
			//Destroy( this.gameObject , 3.0f);
			Destroy( this.gameObject );
		}
		
	}
		
		
		
		
		
}
