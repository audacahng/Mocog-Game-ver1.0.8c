using UnityEngine;
using System.Collections;

public class MaterialAnimation : MonoBehaviour {
	
	
	float m_OffsetY=0;
	public float m_OffsetY_inteval=0.1f;
	float elapseTime=0 ;
	public bool bXAxis= true;
	
	/*

		int m_Texture_Width= BillboardAtlas.texture.width;
		int m_Texture_Height= BillboardAtlas.texture.height;
		BillboardPlane.renderer.material = BillboardAtlas.spriteMaterial;
		BillboardPlane.renderer.material.mainTextureScale= new Vector2( (BillboardSets[lv].inner.width )/m_Texture_Width, (BillboardSets[lv].inner.height )/m_Texture_Height);				
		
/// the coordination system of NGUI Altlas is different with material itself , so we have to do the convertion 
		BillboardPlane.renderer.material.mainTextureOffset = new Vector2( (BillboardSets[lv].inner.x   ) /m_Texture_Width ,(m_Texture_Height- BillboardSets[lv].inner.y - BillboardSets[lv].inner.height) /m_Texture_Height);			 	 
	 
	 
	  * */
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		elapseTime +=Time.deltaTime;
		
		if(elapseTime >=0.1f )
		{
			elapseTime=0;
			m_OffsetY +=m_OffsetY_inteval;
			if(bXAxis)
				gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2( m_OffsetY, 0   );
			else
				gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2( 0, m_OffsetY   );
			
		}
	
	}
	
	
	
	
}
