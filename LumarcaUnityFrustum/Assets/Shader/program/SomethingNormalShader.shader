// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SomethingNormalShader" {
	Properties {
	_Pulse0Pos ("Pulse0Pos", Vector) = (0, 0, 0, 0)
    _Pulse0Mod ("Pulse0Mod", Float) = 1
	_Pulse1Pos ("Pulse1Pos", Vector) = (0, 0, 0, 0)
    _Pulse1Mod ("Pulse1Mod", Float) = 1
	_Pulse2Pos ("Pulse2Pos", Vector) = (0, 0, 0, 0)
    _Pulse2Mod ("Pulse2Mod", Float) = 1
	_Pulse3Pos ("Pulse3Pos", Vector) = (0, 0, 0, 0)
    _Pulse3Mod ("Pulse3Mod", Float) = 1
	_Pulse4Pos ("Pulse4Pos", Vector) = (0, 0, 0, 0)
    _Pulse4Mod ("Pulse4Mod", Float) = 1
    
    _BeatPos   ("BeatPos",  Float) = 1
    _SymbolPos ("SymbolPos",  Float) = 1
    _SymbolFade ("SymbolFade",  Float) = 1
    
    
    _CircPos ("CircPos",  Vector) = (0, 400, 0, 0)
    _CircRad ("CircRad",  Float) = 1
    
    _Tolerance ("Tolerance", Float) = 0.1
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
				
				float3 _Pulse0Pos;
				float _Pulse0Mod;
				float3 _Pulse1Pos;
				float _Pulse1Mod;
				float3 _Pulse2Pos;
				float _Pulse2Mod;
				float3 _Pulse3Pos;
				float _Pulse3Mod;
				float3 _Pulse4Pos;
				float _Pulse4Mod;
				
				float _BeatPos;
				float _SymbolPos;
				float _SymbolFade;
				
				float4 _CircPos;
				float _CircRad;
				
				float _Tolerance;
		 
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
		            output.position_in_world_space = mul(unity_ObjectToWorld, input.vertex);
		               // transformation of input.vertex from object 
		               // coordinates to world coordinates;
		            return output;
		         }
		         
//		         bool equals(float a, float b){
//		         	return (abs(a-b) < _Tolerance);
//		         }
		         
		         bool equals(float2 a, float2 b){
		         	return (distance(a, b)) < _Tolerance;
		         }
		         
		      
	 
		         float4 frag(vertexOutput input) : COLOR 
		         {
		         	//HOOK
		         	
		         	if(equals(_Pulse0Pos.xz, input.position_in_world_space.xz)){
						return float4(1, 1, 1, _Pulse0Mod);
		         	}
		         	if(equals(_Pulse1Pos.xz, input.position_in_world_space.xz)){
						return float4(1, 1, 1, _Pulse1Mod);
		         	}
		         	if(equals(_Pulse2Pos.xz, input.position_in_world_space.xz)){
						return float4(1, 1, 1, _Pulse2Mod);
		         	}
		         	if(equals(_Pulse3Pos.xz, input.position_in_world_space.xz)){
						return float4(1, 1, 1, _Pulse3Mod);
		         	}
		         	if(equals(_Pulse4Pos.xz, input.position_in_world_space.xz)){
						return float4(1, 1, 1, _Pulse4Mod);
		         	}
		         	
		         	//BEAT
		         	if(_BeatPos > input.position_in_world_space.y){
						return float4(1, 0, 0, 1);
		         	}
		         	
		         	//SYMBOL
		         	if(_SymbolPos > input.position_in_world_space.y){
						return float4(0, 1, 1, _SymbolFade);
		         	}
		         	
		         	if(
		         		distance(input.position_in_world_space, _CircPos) < _CircRad &&
		         		distance(input.position_in_world_space, _CircPos) > _CircRad * 0.0){
						return float4(1, 1, 0, 0.5);
		         	}
		      		     
					return float4(1, 0, 0, 0); 
		         }
	         
			ENDCG
		    }
		}
} 