Shader "LCArt/UI/Shader_DE_UI_Speaker"
{
Properties
{
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1, 1, 1, 1)
    _StencilComp ("Stencil Comparison", Float) = 8
    _Stencil ("Stencil ID", Float) = 0
    _StencilOp ("Stencil Operation", Float) = 0
    _StencilWriteMask ("Stencil Write Mask", Float) = 255
    _StencilReadMask ("Stencil Read Mask", Float) = 255
    _ColorMask ("Color Mask", Float) = 15
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    [HideInInspector] [Header(Dialog Speaker Light Setting)] [Toogle] _IsTesting ("Is Testing", Float) = 1
    [HideInInspector] _AmbientColor ("Ambient Color", Color) = (0.10000000149011612, 0.20000000298023224, 0.4000000059604645, 1)
    [HideInInspector] _RimLightColor ("Rim Light Color", Color) = (1, 1, 1, 1)
    [HideInInspector] _RimLightAngle ("Rim Light Angle", Range(0, 360)) = 60
    [HideInInspector] _RimLightIntensity ("Rim Light Intensity", Range(0, 1)) = 0.009999999776482582
    [Header(Dialog Speaker Gradient Setting)] _GradientColor ("Gradient Color", Color) = (0, 0, 0, 1)
    _GradientHeightMax ("Gradient Height Max", Range(0, 1)) = 0.5
    _GradientHeightMin ("Gradient Height Min", Range(0, 1)) = 0.30000001192092896
    [Header(Dialog Speaker Etc Setting)] _TransparentAlphaThreshold ("Transparent Alpha Threshold", Range(0, 1)) = 0.6000000238418579
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Name "Default"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
        ZTest [unity_GUIZTestMode]
        ZWrite Off
        Cull Off
        Stencil {
          Ref [_Stencil]
          ReadMask [_StencilReadMask]
          WriteMask [_StencilWriteMask]
          Comp [_StencilComp]
          Pass [_StencilOp]
        }
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag

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
        float4 _TextureSampleAdd;
        float4 _GlobalDialogBaseColor;
        float4 _GlobalDialogRimLightColor;
        float _GlobalDialogRimLightAngle;
        float _GlobalDialogRimLightOffset;
        cbuffer UnityPerCamera
        {
            float4 _ProjectionParams : packoffset(c5);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerFrame
        {
            float4x4 unity_MatrixVP : packoffset(c17);
        }
        cbuffer UnityPerMaterial
        {
            float4 _Color : packoffset(c0);
            float4 _GradientColor : packoffset(c1);
            float _GradientHeightMax : packoffset(c2);
            float _GradientHeightMin : packoffset(c2.y);
            float _TransparentAlphaThreshold : packoffset(c2.z);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if 1
        #include "LCArt_UI_Shader_DE_UI_Speaker/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_UI_Shader_DE_UI_Speaker/p0_ps_3.hlsl"
        #endif
        ENDHLSL
    }
}
}
