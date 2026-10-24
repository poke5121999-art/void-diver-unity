Shader "Hidden/ChocDino/UIFX/Blend-LongShadow"
{
Properties
{
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    [PerRendererData] _ResultTex ("Sprite Texture", 2D) = "white" {}
    [PerRendererData] _SourceTex ("Source Texture", 2D) = "white" {}
    _StencilComp ("Stencil Comparison", Float) = 8
    _Stencil ("Stencil ID", Float) = 0
    _StencilOp ("Stencil Operation", Float) = 0
    _StencilWriteMask ("Stencil Write Mask", Float) = 255
    _StencilReadMask ("Stencil Read Mask", Float) = 255
    _SourceAlpha ("Source Alpha", Float) = 1
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    _ColorMask ("Color Mask", Float) = 15
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Name "Blend-LongShadow"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend One OneMinusSrcAlpha
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
        #pragma multi_compile_local _ STYLE_CUTOUT STYLE_NORMAL STYLE_SHADOW
        #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
        #pragma multi_compile_local _ UNITY_UI_CLIP_RECT

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
        float4 _MainTex_ST;
        float4 _ResultTex_ST;
        float4 _ClipRect;
        float _SourceAlpha;
        float _UIMaskSoftnessX;
        float _UIMaskSoftnessY;
        cbuffer UnityPerCamera
        {
            float4 _ScreenParams : packoffset(c6);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerFrame
        {
            float4x4 glstate_matrix_projection : packoffset(c5);
            float4x4 unity_MatrixVP : packoffset(c17);
        }

        Texture2D<float4> _ResultTex;
        Texture2D<float4> _SourceTex;
        SamplerState sampler_ResultTex;
        SamplerState sampler_SourceTex;

        // ---- progVertex
        #if defined(STYLE_CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_16.hlsl"
        #elif defined(STYLE_NORMAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_15.hlsl"
        #elif defined(STYLE_SHADOW) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_17.hlsl"
        #elif defined(STYLE_CUTOUT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_8.hlsl"
        #elif defined(STYLE_CUTOUT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_12.hlsl"
        #elif defined(STYLE_NORMAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_7.hlsl"
        #elif defined(STYLE_NORMAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_11.hlsl"
        #elif defined(STYLE_SHADOW) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_9.hlsl"
        #elif defined(STYLE_SHADOW) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_13.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_14.hlsl"
        #elif defined(STYLE_CUTOUT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_4.hlsl"
        #elif defined(STYLE_NORMAL)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_3.hlsl"
        #elif defined(STYLE_SHADOW)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_5.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_6.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_10.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_vs_2.hlsl"
        #endif
        // ---- progFragment
        #if defined(STYLE_CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_38.hlsl"
        #elif defined(STYLE_NORMAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_37.hlsl"
        #elif defined(STYLE_SHADOW) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_39.hlsl"
        #elif defined(STYLE_CUTOUT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_30.hlsl"
        #elif defined(STYLE_CUTOUT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_34.hlsl"
        #elif defined(STYLE_NORMAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_29.hlsl"
        #elif defined(STYLE_NORMAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_33.hlsl"
        #elif defined(STYLE_SHADOW) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_31.hlsl"
        #elif defined(STYLE_SHADOW) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_35.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_36.hlsl"
        #elif defined(STYLE_CUTOUT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_26.hlsl"
        #elif defined(STYLE_NORMAL)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_25.hlsl"
        #elif defined(STYLE_SHADOW)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_27.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_28.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_32.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Blend-LongShadow/p0_ps_24.hlsl"
        #endif
        ENDHLSL
    }
}
}
