﻿Shader "Custom/Sample 3D Texture" {
	Properties{
		_MainTex("Texture", 3D) = "" {}
	}
		SubShader{
		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma exclude_renderers flash gles

#include "UnityCG.cginc"

	struct vs_input {
		float4 vertex : POSITION;
	};

	struct ps_input {
		float4 pos : SV_POSITION;
		float3 uv : TEXCOORD0;
	};


	ps_input vert(vs_input v)
	{
		ps_input o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.vertex.xyz*0.5 + 0.5;
		return o;
	}

	sampler3D _MainTex;

	float4 frag(ps_input i) : COLOR
	{
		return tex3D(_MainTex, i.uv);
	}

		ENDCG

	}
	}

		Fallback "VertexLit"
}