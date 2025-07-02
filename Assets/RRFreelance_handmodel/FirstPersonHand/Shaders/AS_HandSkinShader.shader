// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RRFreelance/AS_HandSkinShader"
{
	Properties
	{
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
		_Base_Albedo("Base_Albedo", 2D) = "white" {}
		_Base_Tint("Base_Tint", Color) = (1,1,1,0)
		_MetalSmooth("Metal-Smooth", 2D) = "white" {}
		_SmoothnessAdd("SmoothnessAdd", Range( -1 , 1)) = 0
		_NormalMap("NormalMap", 2D) = "bump" {}
		_OptionalMask("OptionalMask", 2D) = "black" {}
		_sssTint("sssTint", Color) = (0,0,0,0)
		_NailToneSmoothAdd("NailTone-SmoothAdd", Color) = (0,0,0,0)
		_PalmToneSmoothMult("PalmTone-SmoothMult", Color) = (0,0,0,0)
		_Tattoo("Tattoo", 2D) = "black" {}
		_TattooTintFadeA("TattooTint-Fade(A)", Color) = (0.07850347,0.07920894,0.08088237,0.909)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Translucency;
		};

		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform sampler2D _Base_Albedo;
		uniform float4 _Base_Albedo_ST;
		uniform float4 _Base_Tint;
		uniform sampler2D _OptionalMask;
		uniform float4 _OptionalMask_ST;
		uniform float4 _PalmToneSmoothMult;
		uniform float4 _NailToneSmoothAdd;
		uniform sampler2D _Tattoo;
		uniform float4 _Tattoo_ST;
		uniform float4 _TattooTintFadeA;
		uniform sampler2D _MetalSmooth;
		uniform float4 _MetalSmooth_ST;
		uniform float _SmoothnessAdd;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;
		uniform float4 _sssTint;

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			#if !DIRECTIONAL
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + c;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			o.Normal = UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) );
			float2 uv_Base_Albedo = i.uv_texcoord * _Base_Albedo_ST.xy + _Base_Albedo_ST.zw;
			float4 tex2DNode1 = tex2D( _Base_Albedo, uv_Base_Albedo );
			float2 uv_OptionalMask = i.uv_texcoord * _OptionalMask_ST.xy + _OptionalMask_ST.zw;
			float4 tex2DNode7 = tex2D( _OptionalMask, uv_OptionalMask );
			float4 temp_output_20_0 = ( tex2DNode7.b * _PalmToneSmoothMult * tex2DNode1 );
			float4 lerpResult19 = lerp( ( tex2DNode1 * _Base_Tint ) , ( tex2DNode1 + temp_output_20_0 + temp_output_20_0 ) , tex2DNode7.b);
			float4 temp_cast_0 = (0.5).xxxx;
			float4 lerpResult21 = lerp( lerpResult19 , ( tex2DNode7.g * _NailToneSmoothAdd * pow( tex2DNode1 , temp_cast_0 ) ) , tex2DNode7.g);
			float2 uv_Tattoo = i.uv_texcoord * _Tattoo_ST.xy + _Tattoo_ST.zw;
			float4 tex2DNode37 = tex2D( _Tattoo, uv_Tattoo );
			float4 lerpResult36 = lerp( lerpResult21 , ( tex2DNode37 * _TattooTintFadeA ) , ( tex2DNode37 * _TattooTintFadeA.a ));
			o.Albedo = lerpResult36.rgb;
			float2 uv_MetalSmooth = i.uv_texcoord * _MetalSmooth_ST.xy + _MetalSmooth_ST.zw;
			float4 tex2DNode4 = tex2D( _MetalSmooth, uv_MetalSmooth );
			o.Metallic = tex2DNode4.r;
			float clampResult24 = clamp( ( ( tex2DNode7.b * _PalmToneSmoothMult.a * _PalmToneSmoothMult.a ) + ( tex2DNode7.g * _NailToneSmoothAdd.a ) + ( tex2DNode4.a + _SmoothnessAdd ) ) , 0.0 , 1.0 );
			o.Smoothness = clampResult24;
			o.Translucency = ( tex2DNode7.r * _sssTint ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
254;64;1203;979;-384.2834;648.9933;1.108386;True;False
Node;AmplifyShaderEditor.ColorNode;15;-493.4711,611.4734;Float;False;Property;_PalmToneSmoothMult;PalmTone-SmoothMult;14;0;Create;True;0;0;False;0;0,0,0,0;0.7132353,0.6361248,0.5768815,0.109;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-559.9107,403.2025;Float;True;Property;_OptionalMask;OptionalMask;11;0;Create;True;0;0;False;0;None;768ccbcf836c44446a567a06a753319d;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-664.7747,-336.2899;Float;True;Property;_Base_Albedo;Base_Albedo;6;0;Create;True;0;0;False;0;None;42d7d2dc72a17674d8c7465fe85f3ea1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;93.06075,339.6524;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-2.75795,960.9005;Float;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-468.2296,-104.9784;Float;False;Property;_Base_Tint;Base_Tint;7;0;Create;True;0;0;False;0;1,1,1,0;0.7352941,0.5794356,0.4379325,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-554.7065,70.14758;Float;True;Property;_MetalSmooth;Metal-Smooth;8;0;Create;True;0;0;False;0;None;b05552131450365489c69db56720fb27;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-533.8904,280.0416;Float;False;Property;_SmoothnessAdd;SmoothnessAdd;9;0;Create;True;0;0;False;0;0;0.01;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-471.712,972.5991;Float;False;Property;_NailToneSmoothAdd;NailTone-SmoothAdd;13;0;Create;True;0;0;False;0;0,0,0,0;0.7205882,0.5516227,0.5404412,0.128;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-56.37962,-212.5566;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;33;172.8594,868.3171;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;320.7151,238.4878;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-109.0783,201.5849;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;450.8839,442.6782;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;37;826.696,-546.4875;Float;True;Property;_Tattoo;Tattoo;15;0;Create;True;0;0;False;0;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;181.1305,1057.461;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;397.7692,661.102;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;38;878.6655,-329.5238;Float;False;Property;_TattooTintFadeA;TattooTint-Fade(A);16;0;Create;True;0;0;False;0;0.07850347,0.07920894,0.08088237,0.909;0.07850347,0.07920894,0.08088237,0.784;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;19;403.0866,-68.24194;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;841.7479,408.3146;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;21;800.3695,-117.6804;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;1263.123,-202.0627;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;1213.356,-363.0296;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;8;-487.671,794.7775;Float;False;Property;_sssTint;sssTint;12;0;Create;True;0;0;False;0;0,0,0,0;1,0.4558824,0.4558824,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;36;1462.463,-312.9396;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;18;681.2692,132.246;Float;True;Property;_NormalMap;NormalMap;10;0;Create;True;0;0;False;0;None;3af75e3549193544b86b12a379f2a490;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;123.8485,523.403;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;24;1065.487,397.33;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1824.45,333.8653;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;RRFreelance/AS_HandSkinShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;0;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;7;3
WireConnection;20;1;15;0
WireConnection;20;2;1;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;33;0;1;0
WireConnection;33;1;34;0
WireConnection;28;0;1;0
WireConnection;28;1;20;0
WireConnection;28;2;20;0
WireConnection;23;0;4;4
WireConnection;23;1;5;0
WireConnection;25;0;7;3
WireConnection;25;1;15;4
WireConnection;25;2;15;4
WireConnection;26;0;7;2
WireConnection;26;1;10;4
WireConnection;22;0;7;2
WireConnection;22;1;10;0
WireConnection;22;2;33;0
WireConnection;19;0;3;0
WireConnection;19;1;28;0
WireConnection;19;2;7;3
WireConnection;27;0;25;0
WireConnection;27;1;26;0
WireConnection;27;2;23;0
WireConnection;21;0;19;0
WireConnection;21;1;22;0
WireConnection;21;2;7;2
WireConnection;39;0;37;0
WireConnection;39;1;38;0
WireConnection;40;0;37;0
WireConnection;40;1;38;4
WireConnection;36;0;21;0
WireConnection;36;1;39;0
WireConnection;36;2;40;0
WireConnection;11;0;7;1
WireConnection;11;1;8;0
WireConnection;24;0;27;0
WireConnection;0;0;36;0
WireConnection;0;1;18;0
WireConnection;0;3;4;0
WireConnection;0;4;24;0
WireConnection;0;7;11;0
ASEEND*/
//CHKSM=2A6C6320EBEB8DA3BF1659C92F24F3008160A53D