// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/BAMStripeColorShaderUnlitLeftRight"{

Properties {
    _StripeSize ("Stripe Size", Float) = 0.1
    _Color1("Color 1", Color) = (1, 0, 0)
    _Color2("Color 2", Color) = (0, 1, 0)
    _Color3("Color 3", Color) = (1, 0, 1)
    _Color4("Color 4", Color) = (1, 1, 0)
    _Color5("Color 5", Color) = (1, 0, 1)
    _Color6("Color 6", Color) = (1, 1, 0)
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
			
			float _StripeSize;
			float4 _Color1;
			float4 _Color2;
			float4 _Color3;
			float4 _Color4;
			float4 _Color5;
			float4 _Color6;
	 
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
	         	int diff = input.position_in_world_space.y * (1/_StripeSize) % 3;


	         	if(input.position_in_world_space.x <= 0){
		         	if(diff == 0)
		     			return _Color1;
		         	else {
			         	if(input.position_in_world_space.z <= 0)
			     			return _Color5;
			     		else 
			     			return _Color2;
		     		}
	     		} else {
	     			if(diff == 0)
		     			return _Color3;
		         	else {
			         	if(input.position_in_world_space.z <= 0)
			     			return _Color6;
			     		else 
			     			return _Color4;
		     		}
	     		}

	         }
         
		ENDCG
	    }
	}

//Fallback "VertexLit"
} 