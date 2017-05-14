Shader "billboards\normal" {
   Properties {
      _MainTex ("Texture Image", 2D) = "white" {}
   }
   SubShader {
      Pass {   
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         // User-specified uniforms            
         uniform sampler2D _MainTex;        
 
         struct vertexInput {
            float4 vertex : POSITION;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            output.pos = mul(UNITY_MATRIX_P, 
              mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0))
              + float4(input.vertex.x, input.vertex.y, 0.0, 0.0));
 
            output.tex = float4(input.vertex.x + 0.5, 
               input.vertex.y + 0.5, 0.0, 0.0);
 
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            return tex2D(_MainTex, float2(input.tex));   
         }
 
         ENDCG
      }
   }
}