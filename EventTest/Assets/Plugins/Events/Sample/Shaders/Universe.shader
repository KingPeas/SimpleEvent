Shader "KingDOM/Universe" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SecondTex("Alternative (RGB)", 2D) = "white" {}
	}

	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			uniform sampler2D  _MainTex;
			uniform sampler2D  _SecondTex;

			float4 frag(v2f_img i) : COLOR {
				float4 mcoord = tex2D(_MainTex, i.uv);
				float4 scoord = tex2D(_SecondTex, i.uv);
				
				float val = abs(fmod(_Time.x,2.0) - 1);
				float4 color = lerp(mcoord, scoord, val);

				return color;
			}
			ENDCG
		}
	}
}