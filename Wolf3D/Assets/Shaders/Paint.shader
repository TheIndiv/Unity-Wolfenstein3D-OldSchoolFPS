Shader "Custom/Paint"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_PaintColor("Paint Color", Color) = (0.8, 0, 0.8, 1)
		_Freq("Frequency", Float) = 5
		_Threshold("Threshold", Range(0,1)) = 0.3
		_Amp("Noise Amplitude", Range(0,1)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert

		#include "noise-simplex/noiseSimplex.cginc"

		sampler2D _MainTex;
		float4 _PaintColor;
		float _Freq;
		float _Threshold;
		float _Amp;

		struct Input
		{
			float2 uv_MainTex;
			float4 color : COLOR;
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			float noise = 0;
			float paint = IN.color.a;

			if (paint > 0.01) noise = snoise(IN.worldPos * _Freq) * _Amp + paint;

			if (paint + noise > _Threshold) {
				o.Albedo = IN.color.rgb * 0.95 + float3(1,1,1);
				//o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
				o.Gloss = 0;
				o.Specular = 0;
			}
			else
			{
				half4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb;
			}
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}