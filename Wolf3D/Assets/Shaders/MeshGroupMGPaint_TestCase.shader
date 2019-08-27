Shader "MeshGroup/MGPaint"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DrawPosition("_TestPosition", Vector) = (1,1,1,1)
	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 4.0
				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float2 uv2 : TEXCOORD1;
					float4 localPos : TEXCOORD2;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					float4 localPos : TEXCOORD2;
					float3 normal : NORMAL;
				};

				float4 _DrawPositions[1023];
				float4 _DrawColors[1023];
				float4 _DrawNormals[1023];

				int _NumDrawPositions;

				sampler2D _MainTex;

				v2f vert(appdata v)
				{
					v2f o;
					o.normal = v.normal;
					o.vertex = (float4(v.uv2.x - 0.5, v.uv2.y - 0.5, 0,0.5)); //Use uv coordinates in vertex positions to draw in UV space
					o.uv = v.uv;
					o.localPos = v.vertex;

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{

					float3 result = 0;
					for (int drawPoint = 0; drawPoint < _NumDrawPositions; drawPoint++)
					{
						if (dot(_DrawNormals[drawPoint], i.normal) >= 0.0)
						{
							float radius = _DrawPositions[drawPoint].w;
							float3 color = _DrawColors[drawPoint].rgb;
							float alpha = _DrawColors[drawPoint].a * 0.2;
							//result += color * (radius - min(distance(i.localPos.xyz, _DrawPositions[drawPoint].xyz), radius)) * alpha;
						}
					}

					return float4(result.x, result.y, result.z, 1.0);
				}
				ENDCG
			}
		}
}