Shader "TerrainForMobile/2TexturesUnlit" {
Properties {
   _Color ("Main Color", Color) = (1,1,1,1)
	_Layer1 ("Layer1 (RGB)", 2D) = "white" {}
	_Layer2 ("Layer2 (RGB)", 2D) = "white" {}
	//_MainTex ("Mask (RGB)", 2D) = "white" {}
}
SubShader {Tags { "RenderType"="Opaque" }
	LOD 80
	
	Pass {
		

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		fixed3 _Color;
		sampler2D _Layer1 ;
		sampler2D _Layer2 ;
		//sampler2D _MainTex;

		struct v2f {
			fixed4  pos : SV_POSITION;
			float2  uv[3] : TEXCOORD0;
		};

		fixed4 _Layer1_ST;
		fixed4 _Layer2_ST;
		//fixed4 _MainTex_ST;

		v2f vert (appdata_base v)
		{
			v2f o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			//o.uv[0] = TRANSFORM_TEX (v.texcoord, _MainTex);
			o.uv[1] = TRANSFORM_TEX (v.texcoord, _Layer1);
			o.uv[2] = TRANSFORM_TEX (v.texcoord, _Layer2);
			
			return o;
		}

		fixed4 frag (v2f i) : COLOR
		{
			//fixed4 Mask = tex2D( _MainTex, i.uv[0].xy );
			fixed4 MaskA = tex2D( _Layer2, i.uv[1].xy );
			
    		fixed4 c;
			//c.xyz = (tex2D( _Layer1, i.uv[1].xy ) * Mask.r + tex2D( _Layer2, i.uv[2].xy ) * Mask.g ) * _Color.rgb * 2;
			c.xyz = (tex2D( _Layer1, i.uv[1].xy ) * (1- MaskA.a) + tex2D( _Layer2, i.uv[2].xy ) * MaskA.a ) * _Color.rgb * 2;
			c.w = 0;
			return c;
		}
		ENDCG
    }
}
} 