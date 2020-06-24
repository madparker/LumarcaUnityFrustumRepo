// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/HeightColor" {
Properties {
    _TotalHeight ("Total Height", Float) = 0.762
}


SubShader {
 
    Tags { "RenderType"="Transparent" "Queue"="Transparent" }
    LOD 200
    Blend SrcAlpha OneMinusSrcAlpha
    ZTest Less
 
    Pass {
  
            Cull Off
		CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members srcPos)
			#pragma exclude_renderers d3d11 xbox360
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			float _TotalHeight;
	 
			struct v2f {
			    float4  pos : SV_POSITION;
			    float2  uv : TEXCOORD0;
			    float4 srcPos;
			};
			
			struct vertexInput {
				float4 vertex : POSITION;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 position_in_world_space : TEXCOORD0;
			};
			
	         vertexOutput vert(vertexInput input) 
	         {
	            vertexOutput output; 
	 
	            output.pos =  UnityObjectToClipPos(input.vertex);
	            output.position_in_world_space = 
	               mul(unity_ObjectToWorld, input.vertex);
	               // transformation of input.vertex from object 
	               // coordinates to world coordinates;
	            return output;
	         }
 
	         float4 frag(vertexOutput input) : COLOR 
	         {
	         	float diff = (input.position_in_world_space.y + _TotalHeight/2)/_TotalHeight;
	         
				return float4(1-diff, 0, diff, 1); 
	         }
         
		ENDCG
	    }
	}

//Fallback "VertexLit"
} 