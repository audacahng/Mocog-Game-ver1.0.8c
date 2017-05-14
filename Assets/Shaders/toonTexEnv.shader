Shader "Custom/toonTexEnv" {
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        //_Color ("Main Color", Color) = (1,1,1,1)
        _Blend ("Blend", Range (0, 1)) = 0.5 
        _MainTex ("Texture", 2D) = "white" { }
        _EnvTex ("Reflection", 2D) = "white" { TexGen SphereMap}
        _SkinOverlay ("Main Overlay", 2D) = "white" { }
    }

    

    Category
    {
 		Material 
 		{
                Diffuse [_Color]
                Ambient [_Color]
        }    
   
        Lighting On
        ZWrite On
        Cull Back
		//SeparateSpecular On
		
        SubShader
        {
        	 //Tags {"Queue" = "Overlay" }
        		Tags {RenderType=Opaque}
        		
            Pass
            {
                SetTexture [_MainTex]
                {
                    ////combine texture lerp(texture) previous
                    ////combine texture lerp(texture) previous
                    //combine primary 
                   combine texture * primary DOUBLE, texture * primary 
                   
                   //constantColor [_Color]
					//Combine Texture * constant
					
         			////constantColor (0.5, 0.5, 0.5, [_Blend])
         			////combine texture lerp (constant) constant                    
                }                

                SetTexture [_EnvTex]
                {
                    ////combine texture //Double
                     ////combine texture lerp (texture) previous
                   // constantColor (0.5, 0.5, 0.5, [_Blend])
                     //combine texture lerp(constant) previous
                     //combine previous +-  texture
                     combine previous *  texture DOUBLE
                }





            }
            
            Pass 
            {
            		//Lighting On
             		////Blend One One
             		Blend DstColor SrcColor
             		//Blend  SrcColor DstColor
	              	////Blend DstColor Zero  
    	            ////Blend OneMinusDstColor One
        	        ////Blend SrcAlpha OneMinusSrcAlpha     // Alpha blending
             		
             		SetTexture [_SkinOverlay]
             		{
             			//combine  previous lerp (texture)  texture
             			//combine  texture lerp (texture)  previous
             			//combine  texture +-  previous
             			combine texture
             		}
            }        
            
                
                        
        }
    }
}
