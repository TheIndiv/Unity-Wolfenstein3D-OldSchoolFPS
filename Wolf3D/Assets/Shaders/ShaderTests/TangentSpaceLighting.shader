Shader "Custom/TangentSapceLighting"
{
	Properties
	{
		_NormalTex("Normal Map", 2D) = "bump"
	}

    SubShader
    {
		Pass
		{
			Tags
			{
				"RenderType" = "Opaque"
				"LightMode" = "ForwardBase"
			}
			LOD 200

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 tangentSpaceLight : TEXCOORD1;
			};

			sampler2D _NormalTex;
			float4 _NormalTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float3 normal = UnityObjectToWorldNormal(v.normal);

				//Ensures the texture correctly fits the uv mesh coordinates by correctly scaling and offsetting it.
				o.uv = TRANSFORM_TEX(v.uv, _NormalTex);

				float3 tangent = UnityObjectToWorldNormal(v.tangent);
				float3 bitangent = cross(tangent, normal);

				//Convert the light direction from world space into tangent space in order to avoid multiple multiplications in the fragment function.
				o.tangentSpaceLight = float3(dot(tangent, _WorldSpaceLightPos0), dot(bitangent, _WorldSpaceLightPos0), dot(normal, _WorldSpaceLightPos0));
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float3 tangentNormal = tex2D(_NormalTex, i.uv) * 2 - 1;
				return dot(tangentNormal, i.tangentSpaceLight);
			}

			ENDCG
		}
    }
}
