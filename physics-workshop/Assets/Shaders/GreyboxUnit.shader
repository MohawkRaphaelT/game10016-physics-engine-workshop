// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

/*
	MIT License

	Copyright(c) 2017 Raphael Tetreault

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files(the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions :

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/

Shader "Custom/GreyboxUnit" {
	Properties{
		_ColorA("ColorA", Color) = (1,1,1,1)
		_ColorB("ColorB", Color) = (0,0,0,1)
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_UnitSize("Unit", float) = 1.0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		//sampler2D _MainTex;

	struct Input {
		//float2 uv_MainTex;
		float3 worldPos;
	};

	fixed4 _ColorA;
	fixed4 _ColorB;
	half _Glossiness;
	half _Metallic;
	float _UnitSize;

	// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
	// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
	// #pragma instancing_options assumeuniformscaling
	UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		// Get rotation from matrix
		// https://stackoverflow.com/questions/54170722/simply-get-the-scaling-of-an-object-inside-the-cg-shader
		half3 ObjectScale()
		{
			return half3(
				length(unity_ObjectToWorld._m00_m10_m20),
				length(unity_ObjectToWorld._m01_m11_m21),
				length(unity_ObjectToWorld._m02_m12_m22)
				);
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// Get local position of surface
			//float3 localPos = mul(unity_WorldToObject, IN.worldPos);
			float3 objPos = mul(unity_WorldToObject, IN.worldPos).xyz;
			// This nudges objects right on the line between 2 colors to one side, reducing
			// the occurance of tearing when planes "z-fight" with at the partitioning.
			objPos += float3(0.000001, 0.000001, 0.000001);
			// Subtract world position to get local position
			// Post #4 line 55: https://forum.unity.com/threads/possible-to-get-objects-rotation-value-in-unity_objecttoworld.528359/
			objPos -= mul(unity_WorldToObject, unity_ObjectToWorld._m03_m13_m23);
			// Apply local object scale
			objPos *= ObjectScale();
			// Wrap position into 0f to 2f range (multiplied by _UnitSize for parameter range).
			// We use this to figure out which color to use in the following calculation.
			float3 pos = fmod(objPos, (float3(_UnitSize, _UnitSize, _UnitSize) * 2));

			// Left operation: use abs() to prevent negative values. > _UnitSize splits
			//		the range of values into 2 disctinct parts.
			// Right operation: mirror volumes if negative. This prevent adjacent colors
			//		along axis lines.
			bool x = (abs(pos.x) > _UnitSize) ^ (pos.x > 0);
			bool y = (abs(pos.y) > _UnitSize) ^ (pos.y > 0);
			bool z = (abs(pos.z) > _UnitSize) ^ (pos.z > 0);

			// Select color based on xyz values to appropriate unit grid.
			fixed4 c = (x^y^z) ? _ColorA : _ColorB;

			// Use our colors.
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			// Use other parameters from material editor.
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
	ENDCG
	}
		FallBack "Diffuse"
}