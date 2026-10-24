Shader "Hidden/LCArt/Outline/StencilWrite"
{
Properties
{
    _MainTex ("Main Texture", 2D) = "white" {}
    _OpticalRatioYThreshold ("Optical Ratio Y Threshold", Float) = 0
    _OpticalRatioYPositiveRatio ("Optical Ratio Y Positive Ratio", Float) = 0.10000000149011612
    _OpticalRatioYNegativeUnit ("Optical Ratio Y Negative Unit", Float) = 1.5
    _OutlineFlattenViewZ ("Flatten View Z", Float) = -10
}
SubShader
{
    Tags { "Queue" = "Overlay-100" "RenderType" = "Transparent" }
    Pass
    {
        Tags { "Queue" = "Overlay-100" "RenderType" = "Transparent" }
        ColorMask 0
        ZTest Always
        ZWrite Off
        Cull Off
        Stencil {
          Ref 16
          ReadMask 16
          WriteMask 16
          Pass Replace
        }
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local _ _BILLBOARD

        // The bytecode is D3D11, so it baked in D3D's screen conventions; lines that differ on OpenGL sit under
// UNITY_UV_STARTS_AT_TOP (uvflip.py). URP's API headers define it for every API but OpenGL Core / GLES3, the same set that
// has UNITY_NEAR_CLIP_VALUE 1.0 (reversed Z); these programs include none of them.
#if !defined(UNITY_UV_STARTS_AT_TOP) && !defined(SHADER_API_GLCORE) && !defined(SHADER_API_GLES3) && !defined(SHADER_API_GLES)
#define UNITY_UV_STARTS_AT_TOP 1
#endif
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
        float3 _WorldSpaceCameraPos;
        float4x4 glstate_matrix_projection;
        float4x4 unity_MatrixV;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float _OpticalRatioYThreshold : packoffset(c1);
            float _OpticalRatioYPositiveRatio : packoffset(c1.y);
            float _OpticalRatioYNegativeUnit : packoffset(c1.z);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if defined(_BILLBOARD)
        #include "Hidden_LCArt_Outline_StencilWrite/p0_vs_3.hlsl"
        #elif 1
        #include "Hidden_LCArt_Outline_StencilWrite/p0_vs_2.hlsl"
        #endif
        // ---- progFragment
        #if defined(_BILLBOARD)
        #include "Hidden_LCArt_Outline_StencilWrite/p0_ps_6.hlsl"
        #elif 1
        #include "Hidden_LCArt_Outline_StencilWrite/p0_ps_5.hlsl"
        #endif
        ENDHLSL
    }
}
}
