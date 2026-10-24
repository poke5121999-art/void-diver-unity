Shader "Hidden/ChocDino/UIFX/Blend-Fill-Color"
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
    _FillColor ("Fill Color", Color) = (1, 1, 1, 1)
    _FillColorA ("Fill Color A", Color) = (1, 1, 1, 1)
    _FillColorB ("Fill Color B", Color) = (0, 0, 0, 1)
    _FillColorTL ("Fill Color Top Left", Color) = (1, 1, 1, 1)
    _FillColorTR ("Fill Color Top Right", Color) = (1, 1, 1, 1)
    _FillColorBL ("Fill Color Bottom Left", Color) = (0, 0, 0, 1)
    _FillColorBR ("Fill Color Bottom Right", Color) = (0, 0, 0, 1)
    _ColorScaleBias ("", Vector) = (1, 0, 0, 0)
    _Strength ("Strength", Float) = 1
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    _ColorMask ("Color Mask", Float) = 15
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Name "Blend-Fill-Color"
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
        #pragma multi_compile_local COMP_BG_REPLACE COMP_CUTOUT COMP_FG_MULTIPLY COMP_FG_REPLACE
        #pragma multi_compile_local _ MODE_CORNERS MODE_HORIZONTAL MODE_VERTICAL
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
        float4 _ResultTex_ST;
        float4 _ClipRect;
        float4 _FillColor;
        float _UIMaskSoftnessX;
        float4 _FillColorA;
        float _UIMaskSoftnessY;
        float4 _FillColorB;
        float4 _FillColorTL;
        float4 _FillColorTR;
        float4 _FillColorBL;
        float4 _FillColorBR;
        float2 _ColorScaleBias;
        float _Strength;
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
        SamplerState sampler_ResultTex;

        // ---- progVertex
        #if defined(COMP_BG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_61.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_59.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_60.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_65.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_63.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_64.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_57.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_55.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_56.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_53.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_51.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_52.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_29.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_45.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_27.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_43.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_28.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_44.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_58.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_33.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_CORNERS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_49.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_31.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_47.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_32.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_VERTICAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_48.hlsl"
        #elif defined(COMP_CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_62.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_25.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_CORNERS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_41.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_23.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_39.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_24.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_VERTICAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_40.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_54.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_21.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_37.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_19.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_35.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_20.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_36.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_50.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_CORNERS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_13.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_HORIZONTAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_11.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_VERTICAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_12.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_26.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_42.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_CORNERS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_17.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_HORIZONTAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_15.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_VERTICAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_16.hlsl"
        #elif defined(COMP_CUTOUT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_30.hlsl"
        #elif defined(COMP_CUTOUT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_46.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_CORNERS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_9.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_HORIZONTAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_7.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_VERTICAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_8.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_22.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_38.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_CORNERS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_5.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_HORIZONTAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_3.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_VERTICAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_4.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_18.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_34.hlsl"
        #elif defined(COMP_BG_REPLACE)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_10.hlsl"
        #elif defined(COMP_CUTOUT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_14.hlsl"
        #elif defined(COMP_FG_MULTIPLY)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_6.hlsl"
        #elif defined(COMP_FG_REPLACE)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_2.hlsl"
        #else
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_vs_2.hlsl"
        #endif
        // ---- progFragment
        #if defined(COMP_BG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_131.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_129.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_130.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_135.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_133.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_134.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_127.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_125.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_126.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_123.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_121.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_122.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_99.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_115.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_97.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_113.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_98.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_114.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_128.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_103.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_CORNERS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_119.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_101.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_117.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_102.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_VERTICAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_118.hlsl"
        #elif defined(COMP_CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_132.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_95.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_CORNERS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_111.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_93.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_109.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_94.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_VERTICAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_110.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_124.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_91.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_CORNERS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_107.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_89.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_HORIZONTAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_105.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_90.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_VERTICAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_106.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_120.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_CORNERS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_83.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_HORIZONTAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_81.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(MODE_VERTICAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_82.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_96.hlsl"
        #elif defined(COMP_BG_REPLACE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_112.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_CORNERS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_87.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_HORIZONTAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_85.hlsl"
        #elif defined(COMP_CUTOUT) && defined(MODE_VERTICAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_86.hlsl"
        #elif defined(COMP_CUTOUT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_100.hlsl"
        #elif defined(COMP_CUTOUT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_116.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_CORNERS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_79.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_HORIZONTAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_77.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(MODE_VERTICAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_78.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_92.hlsl"
        #elif defined(COMP_FG_MULTIPLY) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_108.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_CORNERS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_75.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_HORIZONTAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_73.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(MODE_VERTICAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_74.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_88.hlsl"
        #elif defined(COMP_FG_REPLACE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_104.hlsl"
        #elif defined(COMP_BG_REPLACE)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_80.hlsl"
        #elif defined(COMP_CUTOUT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_84.hlsl"
        #elif defined(COMP_FG_MULTIPLY)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_76.hlsl"
        #elif defined(COMP_FG_REPLACE)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_72.hlsl"
        #else
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Color/p0_ps_72.hlsl"
        #endif
        ENDHLSL
    }
}
}
