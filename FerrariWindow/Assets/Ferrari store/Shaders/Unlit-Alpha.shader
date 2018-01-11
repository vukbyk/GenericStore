// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Custom/Unlit/Transparent" {
Properties 
{
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_SecondTex("Ignore mask", 2D) = "white" {}
	_TintTex("Tint from shoulder glow", 2D) = "white" {}
	_Tween("Tween", Range(0, 1)) = 0
	_TweenTint("Tween", Range(0, 1)) = 0
}

SubShader {
	Tags 
	{
		"Queue"="Transparent" 
		"IgnoreProjector"="True" 
		"RenderType"="Transparent"
	}
	LOD 100
	
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	
	Pass 
	{  
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata_t 
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoordtint : TEXCOORD2 ;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoordtint : TEXCOORD2 ;
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _SecondTex;
			float4 _SecondTex_ST;
			sampler2D _TintTex;
			float4 _TintTex_ST;
			float _Tween;
			float _TweenTint;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord1 = TRANSFORM_TEX(v.texcoord1, _SecondTex);
				o.texcoordtint = TRANSFORM_TEX(v.texcoordtint, _TintTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//float4 color1 = tex2D(_MainTex, i.uv);
				fixed4 color1 = tex2D(_MainTex, i.texcoord);
				float4 color2 = tex2D(_SecondTex, i.texcoord1);
				float4 colortint = tex2D(_TintTex, i.texcoordtint);
				// float4 col.a = lerp(color1.a, color2.r, _Tween);
				color1.a =  color1.a +  (_Tween * -color2.r );
				color1.r += colortint.r * _TweenTint;

				

				UNITY_APPLY_FOG(i.fogCoord, col);
				return color1;
			}
		ENDCG
	}
}

}

