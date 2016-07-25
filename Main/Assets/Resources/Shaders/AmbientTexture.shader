Shader "Custom/AmbientTexture" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
		Lighting Off
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			struct v2f{
				float4 pos : POSITION;
				float4 color : COLOR;
				float4 uv_MainTex : TEXCOORD0;
			};
			v2f vert(appdata_full v){
				v2f o;
				o.color=v.color;
				o.uv_MainTex = v.texcoord;
				o.pos=mul(UNITY_MATRIX_MVP,v.vertex);
				return o;
			}
			float4 frag(v2f v):COLOR
			{
				fixed4 c = tex2D (_MainTex, v.uv_MainTex) * UNITY_LIGHTMODEL_AMBIENT;
				return c;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
