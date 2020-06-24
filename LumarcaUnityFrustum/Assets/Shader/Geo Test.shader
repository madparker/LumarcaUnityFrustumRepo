// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Geo Test"
{


	SubShader 
	{
		Pass
		{
			Tags { "RenderType"="Opaque" }
			LOD 200
		
			CGPROGRAM
			#pragma exclude_renderers d3d11 xbox360
			#pragma vertex vert
			#pragma fragment frag
//			#pragma geometry geo
			#pragma target 3.0
			#include "UnityCG.cginc"
			
				// **************************************************************
				// Data structures												*
				// **************************************************************
				struct GS_INPUT
				{
					float4	pos		: POSITION;
					float3	normal	: NORMAL;
					float2  tex0	: TEXCOORD0;
				};

				struct FS_INPUT
				{
					float4	pos		: POSITION;
					float2  tex0	: TEXCOORD0;
				};


				// **************************************************************
				// Vars															*
				// **************************************************************

//				float _Size;
//				float4x4 _VP;
//				Texture2D _SpriteTex;
//				SamplerState sampler_SpriteTex;

				// **************************************************************
				// Shader Programs												*
				// **************************************************************

				// Vertex Shader ------------------------------------------------
				GS_INPUT vert(appdata_base v)
				{
					GS_INPUT output = (GS_INPUT)0;

					output.pos =  mul(unity_ObjectToWorld, v.vertex);
					output.normal = v.normal;
					output.tex0 = float2(0, 0);

					return output;
				}



				// Geometry Shader -----------------------------------------------------
//				[maxvertexcount(4)]
//				void geo(point GS_INPUT p[1], inout TriangleStream<FS_INPUT> triStream)
//				{
//					float3 up = float3(0, 1, 0);
//					float3 look = _WorldSpaceCameraPos - p[0].pos;
//					look.y = 0;
//					look = normalize(look);
//					float3 right = cross(up, look);
//					
//					float halfS = 0.5f * _Size;
//							
//					float4 v[4];
//					v[0] = float4(p[0].pos + halfS * right - halfS * up, 1.0f);
//					v[1] = float4(p[0].pos + halfS * right + halfS * up, 1.0f);
//					v[2] = float4(p[0].pos - halfS * right - halfS * up, 1.0f);
//					v[3] = float4(p[0].pos - halfS * right + halfS * up, 1.0f);
//
//					float4x4 vp = mul(UNITY_MATRIX_MVP, _World2Object);
//					FS_INPUT pIn;
//					pIn.pos = mul(vp, v[0]);
//					pIn.tex0 = float2(1.0f, 0.0f);
//					triStream.Append(pIn);
//
//					pIn.pos =  mul(vp, v[1]);
//					pIn.tex0 = float2(1.0f, 1.0f);
//					triStream.Append(pIn);
//
//					pIn.pos =  mul(vp, v[2]);
//					pIn.tex0 = float2(0.0f, 0.0f);
//					triStream.Append(pIn);
//
//					pIn.pos =  mul(vp, v[3]);
//					pIn.tex0 = float2(0.0f, 1.0f);
//					triStream.Append(pIn);
//				}



				// Fragment Shader -----------------------------------------------
				float4 frag(FS_INPUT input) : COLOR
				{
					return float4(1, 0, 0, 1); 
				}

			ENDCG
		}
	} 
}
