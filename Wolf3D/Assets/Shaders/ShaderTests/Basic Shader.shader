// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Basic Shader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_SecondTex("Texture", 2D) = "white" {}
		_Tween("Tween", Range(0, 0.1)) = 1
		_Color("Color", Color) = (1, 1, 1, 1)
	}

		SubShader
		{
			Tags
			{
			//This makes our texture render after the opaque geometry in the scene. If you don't do this step then you will encounter weird rendering bugs when your custom shader is overlapping opaque geometry.
			"Queue" = "Transparent"
			"PreviewType" = "Plane"
			}

			Pass
			{
			/* Here we apply transparency to our shader by defining a blend mode (how each pixel will blend with the pixel beneath it on the screen).
			Here we will be using an Alpha Blend. This means you take the source colour and multiply it by the source alpha, then you add that with the destination colour multiplied by 1-source alpha.
			So the final equation will look like this: SrcColor * SrcAlpha + DstColor * OneMinusSrcAlpha. */
			Blend SrcAlpha OneMinusSrcAlpha

			//Additive Blending.
			//Blend One One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			//Defines what information we're getting from each vertex on the mesh. So here, we are getting the Vector3 position of the vertex in the mesh's local coordinate system as well as its 2D uv coordinate;
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			//Defines what information we're passing into the fragment function.
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			//Takes appdata struct as a parameter and returns a v2f. Looks at the position of the vertex in the mesh.
			v2f vert(appdata v)
			{
				v2f o;	//Initialise a v2f called o.
				/* Assigns the vertex variable using the mul() function. With the mul() function we perform matrix multiplication
				on the local vertex position to take it from a point relative to the object, in the world, to a point on the screen. UnityObjectToClipPos() does the same thing. */
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;	//Pass the uv value of the vertex along with the vertex position.
				return o;	//Return the v2f.
			}

			//Need to define _MainTex as a sampler2D in the scope of the CGPROGRAM otherwise the shader won't compile.
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;	//Finds the actual size of a pixel for the display it is being rendered on, ranging from 0 to 1.
			sampler2D _SecondTex;
			float4 _Color;
			float _Tween;

			float4 box(sampler2D tex, float2 uv, float4 size)
			{
				float4 c = tex2D(tex, uv + float2(-size.x, size.y)) + tex2D(tex, uv + float2(0, size.y)) + tex2D(tex, uv + float2(size.x, size.y)) + tex2D(tex, uv + float2(-size.x, 0)) +
					tex2D(tex, uv + float2(0, 0)) + tex2D(tex, uv + float2(size.x, 0)) + tex2D(tex, uv + float2(-size.x, -size.y)) + tex2D(tex, uv + float2(0, -size.y)) +
					tex2D(tex, uv + float2(size.x, -size.y));

				return c / 9;
			}

			//Takes our v2f struct and returns a color in the form of a float4 variable. It converts potential pixels into colours on the screen.
			//We can't change the position of the point on the screen as that was locked in during the vert() function. But what we can do now is return a colour for that point on the screen.
			float4 frag(v2f i) : SV_Target
			{
				/* Uses the uv coordinates to determine the colours on the screen, so it will return a gradient of colours. This may seem weird, because a triangle only has three verticies,
				so this vertex shader will only execute 3 times, so then how is it able to produce thousands of colours? The answer...interpolation. The vertex shader can produce thousands of fragments,
				and therefore thousands of colours as well by linearly interpolating its values over those generated fragments. */
				//return float4(i.uv.r, i.uv.g, 1, 1);

				//Uses the tex2D() function to get the color from the texture specified in _MainTex at the provided UV coordinates.
				//float4 color = tex2D(_MainTex, i.uv);


				float4 color = tex2D(_MainTex, i.uv) * float4 (i.uv.r, i.uv.g, 1, 1);


				//float4 color = tex2D(_MainTex, i.uv) * (1-_Tween) + tex2D(_SecondTex, i.uv) * _Tween;


				//float4 color1 = tex2D(_MainTex, i.uv);
				//float4 color2 = tex2D(_SecondTex, i.uv);
				//float4 color = lerp(color1, color2, _Tween);


				//float4 color = tex2D(_MainTex, i.uv * 2);


				//float4 color = tex2D(_MainTex, i.uv * 2) * float4 (i.uv.r, i.uv.g, 1, 1);


				//float4 color = tex2D(_MainTex, i.uv);
				//float luminance = 0.3 * color.r + 0.59 * color.g + 0.11 * color.b;
				//float4 grayscale = float4 (luminance, luminance, luminance, color.a);
				//return grayscale;


				//float4 color = tex2D(_MainTex, i.uv);
				//float luminance = 0.3 * color.r + 0.59 * color.g + 0.11 * color.b;
				//float4 grayscale = float4 (luminance, luminance, luminance, color.a) * _Color;
				//return grayscale;


				//float4 color = tex2D(_MainTex, i.uv);
				//return color * color.a;


				//float2 disp = tex2D(_SecondTex, i.uv).xy;
				//disp = ((disp * 2) - 1) * _Tween;
				//float4 color = tex2D(_MainTex, i.uv + disp);


				//float2 distuv = float2(i.uv.x + _Time.x * 2, i.uv.y + _Time.x * 2);
				//float2 disp = tex2D(_SecondTex, distuv).xy;
				//disp = ((disp * 2) - 1) * _Tween;
				//float4 color = tex2D(_MainTex, i.uv + disp);


				//float4 color = box(_MainTex, i.uv, _MainTex_TexelSize);


				return color;
			}
			ENDCG
		}
	}
}