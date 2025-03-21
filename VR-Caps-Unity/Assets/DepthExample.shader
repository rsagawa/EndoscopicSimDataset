﻿// https://mitchmcclellan.com/depth-based-post-processing-in-hdrp/

Shader "Hidden/Shader/DepthExample"
{
    SubShader
    {
        Pass
        {
            Name "DepthExample"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
            #pragma fragment CustomPostProcess
            #pragma vertex Vert

            #pragma target 4.5
            #pragma only_renderers d3d11 playstation xboxone vulkan metal switch

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/NormalBuffer.hlsl"

            struct Attributes
            {
                uint vertexID : SV_VertexID;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 texcoord   : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
                output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
                return output;
            }

            // List of properties to control your post process effect
            float4 _Params;
            #define _Distance _Params.w
            TEXTURE2D_X(_InputTexture);
            TEXTURE2D(_DepthTexture);
            // UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

            float4 CustomPostProcess(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                uint2 positionSS = input.texcoord * _ScreenSize.xy;

                float3 inColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;

                // Depth
                float depth = LoadCameraDepth(positionSS);
                // float depth = SampleCustomDepth(input.texcoord)
                float linearEyeDepth = LinearEyeDepth(depth, _ZBufferParams);//////
                //float coc = saturate(_Distance / linearEyeDepth);
                float linear01Depth = Linear01Depth(depth, _ZBufferParams);//////


                //return coc;
                // return linearEyeDepth;

                //謎の定数　K=0.034901962
                //距離をK倍してメートル単位ぐらい
                // return linearEyeDepth * 0.034901962;
                // return linearEyeDepth;
                return depth;
                // return linear01Depth;
                // return _ZBufferParams;
            }

            ENDHLSL
        }
    }

        Fallback Off
}