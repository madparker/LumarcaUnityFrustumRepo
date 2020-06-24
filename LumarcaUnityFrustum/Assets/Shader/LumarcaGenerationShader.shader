// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/LumarcaGenerationShader" {
Properties {
    _Mode ("Mode", Int) = 0
    _TotalWidth ("Total Width", Int) = 1024
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

			int _Mode;
			int _TotalWidth;
	 
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
	         
	         float Map(float num, float oldMin, float oldMax, float newMin, float newMax){
				float oldRange = oldMax - oldMin;
				float newRange = newMax - newMin;

				float oldNum = num - oldMin;

				float percent = oldNum/oldRange;

				return (percent * newRange) + newMin;
			}
	         
			float GetColorPart1(float f, float min, float max){
				float val = Map(f, min, max, 0, 255); 

				float d = (floor(val)/255);

				return d;
			}

			float GetColorPart2(float f, float min, float max){
				float part1 = GetColorPart1(f, min, max);

				float val = Map(f, min, max, 0, 255)/255; 

				val = val - part1;

				return val * 255;
			}
 
	         float4 frag(vertexOutput input) : COLOR 
	         {
	         	float4 result = float4(1, 0, 0, 1); 
	         	
	         	if(_Mode == 0){
	         		result.x = GetColorPart1(input.position_in_world_space.x, -_TotalWidth/2, _TotalWidth/2);
	         		result.y = GetColorPart1(input.position_in_world_space.y, -_TotalWidth/2, _TotalWidth/2);
	         		result.z = GetColorPart1(input.position_in_world_space.z, -_TotalWidth/2, _TotalWidth/2);
	         	} else {
	         		result.x = GetColorPart2(input.position_in_world_space.x, -_TotalWidth/2, _TotalWidth/2);
	         		result.y = GetColorPart2(input.position_in_world_space.y, -_TotalWidth/2, _TotalWidth/2);
	         		result.z = GetColorPart2(input.position_in_world_space.z, -_TotalWidth/2, _TotalWidth/2);
	         	}

				return result; 
	         }
         
		ENDCG
	    }
	}

//Fallback "VertexLit"
} 