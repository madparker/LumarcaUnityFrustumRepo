Shader "Hidden/FadeInOut" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_FadeAmt ("FadeAmount", Range (0, 1)) = 0
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _FadeAmt;

			float4 frag(v2f_img i) : COLOR {
				float4 c = tex2D(_MainTex, i.uv);

				float3 black = float3(1, 1, 1); 
				
				float4 result = c;
				result.rgb = lerp(c.rgb, black, _FadeAmt);
				return result;
			}
			ENDCG
		}
	}
}