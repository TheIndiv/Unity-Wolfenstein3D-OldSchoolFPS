Shader "Custom/SurfaceNormals"
{
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
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				//Flat Shading.
				//nointerpolation float3 normal : NORMAL;

				//Smooth Shading.
				float3 normal : NORMAL;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				//o.normal = v.normal;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				//float3 normal = normalize(i.normal);
				//float3 color = (normal + 1) * 0.5;
				//return fixed4(color.rgb, 0);

				return saturate(dot(i.normal, _WorldSpaceLightPos0));
			}

			ENDCG
		}
    }
}
