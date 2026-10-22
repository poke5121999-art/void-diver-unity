Shader "Hidden/Shader_DE_PP_UI_Sanity"
{
Properties
{
    _UIErrorCodeIntensity ("UIErrorCodeIntensity", Float) = 0
    _UIErrorCodeFlickerStepCutoff ("UIErrorCodeFlickerStepCutoff", Range(0, 1)) = 0.699999988079071
    _UIErrorCodeGlitchDegree ("UIErrorCodeGlitchDegree", Float) = 0.009999999776482582
    _UIErrorCodeGlitchScale ("UIErrorCodeGlitchScale", Float) = 200
    [HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
}
SubShader
{
    Tags { "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalFullscreenSubTarget" }
    Pass
    {
        Name "DrawProcedural"
        Tags { "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalFullscreenSubTarget" }
        ZTest Off
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag

        // 3Dmigoto decompiler conventions used by the per-variant code
#define cmp -
#define VD_COL(M, c) (float4((M)[0][c], (M)[1][c], (M)[2][c], (M)[3][c]))
int vd_asint(float v) { return asint(v); } int2 vd_asint(float2 v) { return asint(v); } int3 vd_asint(float3 v) { return asint(v); } int4 vd_asint(float4 v) { return asint(v); }
int vd_asint(uint v) { return asint(v); } int2 vd_asint(uint2 v) { return asint(v); } int3 vd_asint(uint3 v) { return asint(v); } int4 vd_asint(uint4 v) { return asint(v); }
int vd_asint(int v) { return v; } int2 vd_asint(int2 v) { return v; } int3 vd_asint(int3 v) { return v; } int4 vd_asint(int4 v) { return v; }
uint vd_asuint(float v) { return asuint(v); } uint2 vd_asuint(float2 v) { return asuint(v); } uint3 vd_asuint(float3 v) { return asuint(v); } uint4 vd_asuint(float4 v) { return asuint(v); }
uint vd_asuint(int v) { return asuint(v); } uint2 vd_asuint(int2 v) { return asuint(v); } uint3 vd_asuint(int3 v) { return asuint(v); } uint4 vd_asuint(int4 v) { return asuint(v); }
uint vd_asuint(uint v) { return v; } uint2 vd_asuint(uint2 v) { return v; } uint3 vd_asuint(uint3 v) { return v; } uint4 vd_asuint(uint4 v) { return v; }
float vd_asfloat(int v) { return asfloat(v); } float2 vd_asfloat(int2 v) { return asfloat(v); } float3 vd_asfloat(int3 v) { return asfloat(v); } float4 vd_asfloat(int4 v) { return asfloat(v); }
float vd_asfloat(uint v) { return asfloat(v); } float2 vd_asfloat(uint2 v) { return asfloat(v); } float3 vd_asfloat(uint3 v) { return asfloat(v); } float4 vd_asfloat(uint4 v) { return asfloat(v); }
float vd_asfloat(float v) { return v; } float2 vd_asfloat(float2 v) { return v; } float3 vd_asfloat(float3 v) { return v; } float4 vd_asfloat(float4 v) { return v; }

        // $Globals: bound per variable by name
        float2 _GlobalMipBias;
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixInvVP;
        float _FlipY;
        cbuffer UnityPerMaterial
        {
            float _UIErrorCodeFlickerStepCutoff : packoffset(c0);
            float _UIErrorCodeGlitchDegree : packoffset(c0.y);
            float _UIErrorCodeGlitchScale : packoffset(c0.z);
            float _UIErrorCodeIntensity : packoffset(c0.w);
        }

        Texture2D<float4> _CustomScreenUIBuffer;
        SamplerState sampler_CustomScreenUIBuffer;

        // ---- progVertex
        #if 1
        #include "Hidden_Shader_DE_PP_UI_Sanity/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "Hidden_Shader_DE_PP_UI_Sanity/p0_ps_3.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Blit"
        Tags { "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalFullscreenSubTarget" }
        ZTest Off
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag

        // 3Dmigoto decompiler conventions used by the per-variant code
#define cmp -
#define VD_COL(M, c) (float4((M)[0][c], (M)[1][c], (M)[2][c], (M)[3][c]))
int vd_asint(float v) { return asint(v); } int2 vd_asint(float2 v) { return asint(v); } int3 vd_asint(float3 v) { return asint(v); } int4 vd_asint(float4 v) { return asint(v); }
int vd_asint(uint v) { return asint(v); } int2 vd_asint(uint2 v) { return asint(v); } int3 vd_asint(uint3 v) { return asint(v); } int4 vd_asint(uint4 v) { return asint(v); }
int vd_asint(int v) { return v; } int2 vd_asint(int2 v) { return v; } int3 vd_asint(int3 v) { return v; } int4 vd_asint(int4 v) { return v; }
uint vd_asuint(float v) { return asuint(v); } uint2 vd_asuint(float2 v) { return asuint(v); } uint3 vd_asuint(float3 v) { return asuint(v); } uint4 vd_asuint(float4 v) { return asuint(v); }
uint vd_asuint(int v) { return asuint(v); } uint2 vd_asuint(int2 v) { return asuint(v); } uint3 vd_asuint(int3 v) { return asuint(v); } uint4 vd_asuint(int4 v) { return asuint(v); }
uint vd_asuint(uint v) { return v; } uint2 vd_asuint(uint2 v) { return v; } uint3 vd_asuint(uint3 v) { return v; } uint4 vd_asuint(uint4 v) { return v; }
float vd_asfloat(int v) { return asfloat(v); } float2 vd_asfloat(int2 v) { return asfloat(v); } float3 vd_asfloat(int3 v) { return asfloat(v); } float4 vd_asfloat(int4 v) { return asfloat(v); }
float vd_asfloat(uint v) { return asfloat(v); } float2 vd_asfloat(uint2 v) { return asfloat(v); } float3 vd_asfloat(uint3 v) { return asfloat(v); } float4 vd_asfloat(uint4 v) { return asfloat(v); }
float vd_asfloat(float v) { return v; } float2 vd_asfloat(float2 v) { return v; } float3 vd_asfloat(float3 v) { return v; } float4 vd_asfloat(float4 v) { return v; }

        // $Globals: bound per variable by name
        float2 _GlobalMipBias;
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixInvVP;
        float _FlipY;
        cbuffer UnityPerMaterial
        {
            float _UIErrorCodeFlickerStepCutoff : packoffset(c0);
            float _UIErrorCodeGlitchDegree : packoffset(c0.y);
            float _UIErrorCodeGlitchScale : packoffset(c0.z);
            float _UIErrorCodeIntensity : packoffset(c0.w);
        }

        Texture2D<float4> _CustomScreenUIBuffer;
        SamplerState sampler_CustomScreenUIBuffer;

        // ---- progVertex
        #if 1
        #include "Hidden_Shader_DE_PP_UI_Sanity/p1_vs_4.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "Hidden_Shader_DE_PP_UI_Sanity/p1_ps_3.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.Rendering.Fullscreen.ShaderGraph.FullscreenShaderGUI"
}
