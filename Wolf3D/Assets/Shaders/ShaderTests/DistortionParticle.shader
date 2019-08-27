Shader "Unlit/DistortionParticle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Magnitude ("Magnitude", Float) = 1
    }

    SubShader
    {
        Tags
		{
			"RenderType"="Opaque"
			"Queue" = "Transparent"
		}

        Pass
        {
			Blend One One	//Additive Blending.
			Cull Off	//Turn off culling.
			ZWrite Off	//Do not write to the z buffer.
			ZTest Always	//Always render this in front.

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata	//What we get from the object.
            {
                float4 vertex : POSITION;	//Current vertex position.
                float2 uv : TEXCOORD0;	//Texture/uv coordinates.
				float4 color : COLOR;	//Color values of the uv coordinates.
            };

            struct v2f	//What is converted into a fragment.
            {
                float2 uv : TEXCOORD0;	//Pass in vertex uv coordinates.
                float4 vertex : SV_POSITION;	//Pass in the position of the vertex in screen space.
				float alpha : TEXCOORD1;	//Pass in the alpha value of the vertex.
				float4 projPos : TEXCOORD2;
            };

            sampler2D _MainTex;
			sampler2D_float _CameraDepthTexture;	//Retrieve the camera's depth texture for use in our custom z-culling.
            float4 _MainTex_ST;
			float _Magnitude;	//The magnitude value selected in the inspector.

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);	//Convert the vertex object coordinates into world coordinates.
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);	//Make sure the texture fits the uv coordinates by scaling and offsetting.
				o.alpha = v.color.a;	//Pass in the alpha channel of the vertex. This way particle systems and sprite renderers, used as distortion effects, can transistion in and out nicely.
				o.projPos = ComputeScreenPos(o.vertex);	//This takes the object's clip/world space position (computed using UnityObjectToClipPos()) and return that vertex's position on the screen. This will be used to sample the scene's depth texture.
				COMPUTE_EYEDEPTH(o.projPos.z);	//Pass along the vertex depth in the camera's depth range.

                return o;
            }

            float2 frag (v2f i) : SV_Target
            {
				//This will give us the depth value of the scene in the camera's depth range.
				float sceneEyeDepth = DECODE_EYEDEPTH(tex2D(_CameraDepthTexture, i.projPos.xy / i.projPos.w));	//Here we sample the camera's depth texture using the projection/screen corrected texture coordinates. Then we'll use the DECODE_EYEDEPTH() macro to re-map the sampled value into the camera's depth range.
				float zCull = sceneEyeDepth > i.projPos.z;	//Once we have the depth value of the fragment and the scene, we compare the two. If the scene's depth value is less than the fragment's depth value then the resulting number will be 0, that means the scene is closer to the camera and so we discard this fragment. Otherwise, the fragment will be close, and so the resulting value is 1.
				float3 data = UnpackNormal(tex2D(_MainTex, i.uv)).xyz;	//Unpack the normal map values in the texture so that they are in the range -1 to 1.
				float scale = data.b * i.alpha * _Magnitude;
				return data.rg * scale * zCull;	//The final color value returned is the unpacked normal's red and green value multiplied by the unpacked normal's blue value, the vertex alpha value and the magnitude (this allows us the set how much distortion we want right in the inspector).
				//It is also multiplied by the zCull value which will either be a value of 1 or 0. This essentially recreates z-culling in our shader so if the fragment is blocked by the scene, then we multiply by 0 and the fragment is discarded, otherwise we multiply by 1 and the fragment is rendered,
            }
            ENDCG
        }
    }
}
