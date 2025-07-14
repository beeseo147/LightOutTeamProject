Shader "Universal Renderer PipeLine/OutlineShader"
{
    Properties {
        // _MainTex ("Base (RGB)", 2D) = "white" {}
        // _Color ("Color", Color) = (1, 1, 1, 1)
        // _OutlineThickness ("Outline Thickness", Float) = 1.0
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
		_Outline ("Outline Thickness", Range (0, 1)) = .1
    }
CGINCLUDE
#include "UnityCG.cginc"
 
struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
 
struct v2f {
	float4 pos : POSITION;
	float4 color : COLOR;
};
 
uniform float _Outline;
uniform float4 _OutlineColor;
 
v2f vert(appdata v) {
	v2f o;

	v.vertex *= ( 1 + _Outline);

	o.pos = UnityObjectToClipPos(v.vertex);
 
	o.color = _OutlineColor;
	return o;
}
ENDCG

    SubShader {
        Tags { "DisableBatching" = "True" }
		Pass {
			Name "OUTLINE"
			Tags {"LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			half4 frag(v2f i) :COLOR { return i.color; }
			ENDCG
		}
        // Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        // Cull Off
        // Blend One OneMinusSrcAlpha
       
        // Pass {
 
        //     CGPROGRAM
        //     #pragma vertex vert
        //     #pragma fragment frag
        //     #include "UnityCG.cginc"
 
        //     sampler2D _MainTex;
 
        //     struct v2f {
        //         float4 pos : SV_POSITION;
        //         half2 uv : TEXCOORD0;
        //     };
 
        //     v2f vert(appdata_base v) {
        //         v2f o;
        //         o.pos = UnityObjectToClipPos(v.vertex);
        //         o.uv = v.texcoord;
        //         return o;
        //     }
 
        //     fixed4 _Color;
        //     float _OutlineThickness;
        //     float4 _MainTex_TexelSize;
 
        //     fixed4 frag(v2f i) : COLOR
        //     {
        //         half4 c = tex2D(_MainTex, i.uv);
        //         half4 outlineC = _Color;

        //         c.rgb *= c.a;
        //         outlineC.a *= ceil(c.a);
        //         outlineC.rgb *= outlineC.a;
 
        //         fixed alpha_up = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y * _OutlineThickness)).a;
        //         fixed alpha_down = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y * _OutlineThickness)).a;
        //         fixed alpha_right = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x * _OutlineThickness, 0)).a;
        //         fixed alpha_left = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x * _OutlineThickness, 0)).a;
 
        //         return lerp(outlineC, c, ceil(alpha_up * alpha_down * alpha_right * alpha_left));
        //     }  
 
        //     ENDCG
        // }
    }
    FallBack "Diffuse"
}
