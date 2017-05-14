Shader "Custom/toonOverlay" {
	
	Properties 
	{ 
	_Color ("Main Color", Color) = (1,1,1,1) 
	_Emission ("Emmisive Color", Color) = (0,0,0,0) 
	_Blend ("Blend", Range (0, 1)) = 0.5 
	_MainTex ("Base (RGB)", 2D) = "white" 
	_EnvMap ("EnvMap", 2D) = "white" { TexGen SphereMap }
	} 
	
	
	SubShader { 

	Material 
	{
	 	Diffuse [_MainTex] 
	 	Emission [_Emission] 
	 	Ambient [_MainTex] 
	} 

	Pass { 
		Lighting On

     SetTexture [_EnvMap] { 
         constantColor (0.5, 0.5, 0.5, [_Blend])
         combine texture lerp (constant) constant
     }
 
        SetTexture [_MainTex] { 
         combine texture * previous DOUBLE
        } 
 
        SetTexture [_MainTex] { 
          constantColor[_Color]
          combine previous * constant
        }
  } 
}

SubShader {
  Pass { 
        SetTexture [_MainTex] { 
         combine texture
        } 
 
        SetTexture [_MainTex] { 
          constantColor[_Color]
          combine previous * constant
        }
  } 
}

//Fallback "Mobile/Unlit (Supports Lightmap)" 
Fallback "Depth" 

}
