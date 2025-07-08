Shader "Custom/RevealingShader"
{
    Properties
    {
        _MainTex ("메인 텍스처", 2D) = "white" {}
        _Color ("색상", Color) = (1,1,1,1)
        _LightPos ("손전등 위치", Vector) = (0,0,0,1)
        _FlashlightRange ("손전등 범위", Range(0.1, 10)) = 5
        _FlashlightIntensity ("손전등 강도", Range(0, 2)) = 1
        _RevealThreshold ("노출 임계값", Range(0, 1)) = 0.1
        _LightDir ("손전등 방향", Vector) = (0,0,0,1)
        _ConeAngle ("손전등 각도", Range(0, 180)) = 30
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline"
        }
        
        LOD 200
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _Color;
                float4 _LightPos;
                float _FlashlightRange;
                float _FlashlightIntensity;
                float _RevealThreshold;
                float4 _LightDir;
                float _ConeAngle;
            CBUFFER_END
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
                
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                half4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv) * _Color;
                float flashlightEffect = 0;

                float3 lightToSurface = input.positionWS - _LightPos.xyz;
                float dist = length(lightToSurface);

                if (dist <= _FlashlightRange)
                {
                    float3 lightDirection = normalize(_LightDir.xyz);
                    float3 surfaceToLight = normalize(lightToSurface);
                    float angle = acos(dot(-surfaceToLight, lightDirection)) * 180.0 / 3.14159;
                    if (angle <= _ConeAngle)
                    {
                        float dotProduct = dot(input.normalWS, -surfaceToLight);
                        if (dotProduct > 0)
                        {
                            float distanceAttenuation = 1 - saturate(dist / _FlashlightRange);
                            float coneAttenuation = 1 - saturate(angle / _ConeAngle);
                            float normalAttenuation = pow(dotProduct, 2);
                            flashlightEffect = distanceAttenuation * coneAttenuation * normalAttenuation * _FlashlightIntensity;
                        }
                    }
                }
                
                float finalAlpha = saturate(flashlightEffect / _RevealThreshold);
                return half4(albedo.rgb, finalAlpha * albedo.a);
            }
            ENDHLSL
        }
    }
    
    FallBack "Universal Render Pipeline/Lit"
} 