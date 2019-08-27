Shader "Unlit/Hologram"
{
    Properties
    {
        _MainTex ("Albedo Texture", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1, 1, 1, 1)
		_Transparency("Transparency", Range(0.0, 1.0)) = 0.5
		_CutoutThresh("Cutout Threshold", Range(0.0, 1.0)) = 0.2
		_Distance("Distance", Float) = 1
		_Amplitude("Amplitude", Float) = 1
		_Speed("Speed", Float) = 1
		_Amount("Amount", Range(0.0, 1.0)) = 1
    }
    SubShader
    {
		//The 'Queue' tag determines which order the object will be drawn in. Here we set it to 'Transparent' meaning that it will be drawn ontop of opaque objects.
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
		
		//We are not going to write to the depth buffer.
		ZWrite Off
		//After everything has been rendered, we need to specify how we are going to blend them altogether. This is traditional transparency so we are going to blend everything using the alpha channel.
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			
			//Structs are objects that can contain variables which we can call.
			//This struct holds data about the mesh.
            struct appdata
            {
				/* Make variable of type float4 which is a packed array which means it contains 4 floating point numbers. The ': POSITION' at the end is a semantic binding.
				This tells our shader how it is going to be used in the rendering. So, here we are specifying that it is a position in the object's local coordinate space. */
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;	//Pass in the UVs for the model. Semantic binding tells the shader that this is going to be used as texture coordinates.
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;	//UV coordinates.
                UNITY_FOG_COORDS(1)
				//The semantic binding here tells the shader that this is going to be a screen-space position (because we convert the vertex positions from object coords into screen coords in the vert() function.
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _TintColor;
			float _Transparency;
			float _CutoutThresh;
			float _Distance;
			float _Amplitude;
			float _Speed;
			float _Amount;

			//Pass the appdata data into the vert() function. Note: You don't have to pass in the vaiables as a struct you could just pass in multiple variables. However, it is just easier to pass in structs.
			//This is where you deal with all the verticies of the mesh before sending them off to the fragment function.
            v2f vert (appdata v)
            {
				//Create a new v2f struct. This is what we are going to return from the vert() funciton into the frag() function.
                v2f o;
				//_Time.y is the current time in seconds.
				v.vertex.x += sin(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount;
				/* Here we pass in the mesh vertex into the UnityObjectToClipPos() function which will return the vertex position in screen space as opposed to world space.
				When this function is called, unity will perform multiple matrix transformations in order to get it into screen space form. So, first it will go from local/object space -> world space -> view/eye space -> clip space -> screen space. */
                o.vertex = UnityObjectToClipPos(v.vertex);
				//Now we need to transform the texture. So we get the vertex uv data as well as the main texture and apply the transformations set in the inspector window, this is where the 'Tiling' and 'Offset' options are applied.
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
				//Now that we have built up our model in the appropriate coordinate system, with its UVs, we are now going to send the struct to the fragment function (frag()) so that it can be turned into pixels on the screen.
                return o;
            }

			//It is going to take in a v2f struct called i. It is bound to SV_Target which is a render target. So, this function is going to output to a render target (which in our case is the frame buffer from the screen).
			//The fragment function is where you work with all the fragments (a.k.a the pixels themselves, their color, alpha, etc).
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture. Takes the color of the texture at the specified uv coordinates and outputs that to the frame buffer.
                fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
				col.a = _Transparency;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
				//Discard certain pixels. It tests to see if the red value of the pixel is less than a certain threshold as definied by the _CutoutThresh property.
				clip(col.r - _CutoutThresh);
				//Below if the equivalent of clip().
				//if (col.r < _CutoutThresh) discard;
                return col;
            }
            ENDCG
        }
    }
}
