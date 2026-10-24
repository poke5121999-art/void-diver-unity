Shader "Hidden/ChocDino/UIFX/Blend-Fill-Gradient"
{
Properties
{
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    [PerRendererData] _ResultTex ("Sprite Texture", 2D) = "white" {}
    _StencilComp ("Stencil Comparison", Float) = 8
    _Stencil ("Stencil ID", Float) = 0
    _StencilOp ("Stencil Operation", Float) = 0
    _StencilWriteMask ("Stencil Write Mask", Float) = 255
    _StencilReadMask ("Stencil Read Mask", Float) = 255
    _GradientAxisParams ("Axis", Vector) = (1, 0, 0, 0)
    _GradientLinearStartLine ("Start Line", Vector) = (0, 0, 0, 0)
    _GradientLinearParams ("Linear Params", Vector) = (0, 0, 0, 0)
    _GradientTransform ("Transform", Vector) = (1, 0, 0, 0)
    _GradientRadial ("Transform", Vector) = (0, 0, 1, 0)
    _GradientDither ("Dither", Float) = 0
    _Strength ("Strength", Float) = 1
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    _ColorMask ("Color Mask", Float) = 15
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Name "Blend-Fill-Gradient"
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
        #pragma multi_compile_local _ BLEND_ALPHABLEND BLEND_BACKGROUND BLEND_DARKEN BLEND_LIGHTEN BLEND_MULTIPLY BLEND_REPLACE_ALPHA
        #pragma multi_compile_local _ DITHER
        #pragma multi_compile_local _ GRADIENT_COLORSPACE_PERCEPTUAL
        #pragma multi_compile_local GRADIENT_LERP_LINEAR GRADIENT_LERP_SMOOTH GRADIENT_LERP_STEP GRADIENT_LERP_STEPAA
        #pragma multi_compile_local GRADIENT_SHAPE_AXIS GRADIENT_SHAPE_CONIC GRADIENT_SHAPE_LINEAR GRADIENT_SHAPE_RADIAL
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
        float4 _ResultTex_TexelSize;
        float4 _ClipRect;
        int _GradientColorCount;
        int _GradientAlphaCount;
        float _UIMaskSoftnessX;
        float4 _GradientColors[8];
        float _UIMaskSoftnessY;
        float4 _GradientAlphas[8];
        float4 _GradientTransform;
        float _GradientDither;
        float _Strength;
        float4 _GradientAxisParams;
        float4 _GradientLinearStartLine;
        float4 _GradientRadial;
        float4 _GradientLinearParams;
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
        #if defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1626.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1629.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1627.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1628.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1622.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1625.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1623.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1624.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1630.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1633.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1631.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1632.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1634.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1637.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1635.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1636.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1786.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1789.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1787.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1788.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1782.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1785.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1783.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1784.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1790.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1793.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1791.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1792.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1794.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1797.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1795.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1796.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1722.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1725.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1723.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1724.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1718.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1721.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1719.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1720.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1726.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1729.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1727.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1728.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1730.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1733.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1731.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1732.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1690.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1693.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1691.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1692.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1686.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1689.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1687.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1688.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1694.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1697.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1695.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1696.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1698.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1701.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1699.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1700.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1658.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1661.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1659.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1660.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1654.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1657.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1655.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1656.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1662.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1665.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1663.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1664.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1666.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1669.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1667.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1668.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1754.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1757.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1755.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1756.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1750.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1753.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1751.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1752.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1758.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1761.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1759.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1760.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1762.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1765.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1763.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1764.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_730.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1178.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_733.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1181.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_731.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1179.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_732.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1180.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_726.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1174.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_729.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1177.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_727.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1175.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_728.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1176.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_734.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1182.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_737.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1185.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_735.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1183.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_736.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1184.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_738.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1186.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_741.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1189.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_739.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1187.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_740.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1188.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1610.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1613.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1611.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1612.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1606.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1609.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1607.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1608.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1614.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1617.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1615.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1616.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1618.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1621.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1619.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1620.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1402.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1405.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1403.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1404.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1398.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1401.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1399.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1400.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1406.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1409.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1407.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1408.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1410.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1413.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1411.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1412.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_890.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1338.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_893.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1341.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_891.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1339.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_892.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1340.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_886.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1334.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_889.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1337.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_887.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1335.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_888.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1336.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_894.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1342.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_897.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1345.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_895.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1343.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_896.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1344.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_898.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1346.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_901.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1349.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_899.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1347.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_900.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1348.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1770.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1773.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1771.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1772.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1766.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1769.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1767.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1768.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1774.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1777.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1775.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1776.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1778.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1781.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1779.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1780.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1562.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1565.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1563.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1564.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1558.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1561.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1559.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1560.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1566.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1569.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1567.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1568.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1570.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1573.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1571.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1572.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_826.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1274.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_829.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1277.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_827.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1275.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_828.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1276.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_822.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1270.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_825.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1273.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_823.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1271.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_824.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1272.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_830.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1278.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_833.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1281.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_831.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1279.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_832.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1280.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_834.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1282.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_837.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1285.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_835.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1283.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_836.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1284.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1706.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1709.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1707.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1708.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1702.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1705.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1703.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1704.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1710.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1713.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1711.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1712.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1714.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1717.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1715.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1716.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1498.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1501.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1499.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1500.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1494.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1497.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1495.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1496.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1502.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1505.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1503.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1504.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1506.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1509.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1507.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1508.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_794.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1242.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_797.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1245.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_795.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1243.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_796.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1244.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_790.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1238.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_793.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1241.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_791.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1239.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_792.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1240.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_798.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1246.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_801.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1249.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_799.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1247.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_800.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1248.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_802.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1250.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_805.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1253.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_803.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1251.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_804.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1252.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1674.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1677.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1675.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1676.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1670.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1673.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1671.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1672.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1678.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1681.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1679.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1680.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1682.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1685.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1683.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1684.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1466.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1469.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1467.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1468.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1462.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1465.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1463.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1464.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1470.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1473.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1471.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1472.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1474.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1477.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1475.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1476.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_762.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1210.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_765.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1213.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_763.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1211.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_764.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1212.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_758.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1206.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_761.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1209.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_759.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1207.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_760.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1208.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_766.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1214.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_769.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1217.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_767.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1215.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_768.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1216.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_770.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1218.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_773.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1221.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_771.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1219.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_772.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1220.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1642.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1645.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1643.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1644.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1638.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1641.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1639.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1640.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1646.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1649.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1647.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1648.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1650.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1653.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1651.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1652.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1434.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1437.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1435.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1436.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1430.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1433.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1431.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1432.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1438.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1441.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1439.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1440.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1442.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1445.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1443.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1444.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_858.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1306.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_861.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1309.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_859.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1307.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_860.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1308.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_854.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1302.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_857.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1305.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_855.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1303.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_856.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1304.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_862.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1310.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_865.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1313.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_863.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1311.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_864.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1312.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_866.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1314.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_869.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1317.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_867.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1315.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_868.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1316.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1738.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1741.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1739.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1740.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1734.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1737.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1735.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1736.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1742.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1745.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1743.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1744.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1746.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1749.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1747.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1748.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1530.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1533.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1531.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1532.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1526.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1529.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1527.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1528.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1534.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1537.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1535.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1536.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1538.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1541.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1539.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1540.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1594.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1597.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1595.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1596.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1590.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1593.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1591.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1592.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1598.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1601.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1599.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1600.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1602.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1605.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1603.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1604.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_282.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_285.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_283.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_284.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_278.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_281.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_279.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_280.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_286.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_289.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_287.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_288.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_290.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_293.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_291.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_292.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_714.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1162.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_717.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1165.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_715.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1163.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_716.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1164.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_710.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1158.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_713.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1161.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_711.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1159.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_712.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1160.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_718.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1166.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_721.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1169.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_719.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1167.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_720.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1168.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_722.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1170.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_725.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1173.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_723.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1171.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_724.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1172.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_506.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_954.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_509.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_957.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_507.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_955.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_508.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_956.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_502.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_950.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_505.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_953.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_503.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_951.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_504.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_952.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_510.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_958.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_513.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_961.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_511.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_959.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_512.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_960.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_514.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_962.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_517.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_965.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_515.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_963.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_516.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_964.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1386.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1389.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1387.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1388.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1382.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1385.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1383.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1384.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1390.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1393.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1391.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1392.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1394.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1397.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1395.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1396.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_442.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_445.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_443.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_444.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_438.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_441.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_439.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_440.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_446.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_449.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_447.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_448.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_450.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_453.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_451.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_452.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_874.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1322.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_877.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1325.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_875.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1323.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_876.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1324.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_870.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1318.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_873.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1321.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_871.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1319.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_872.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1320.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_878.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1326.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_881.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1329.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_879.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1327.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_880.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1328.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_882.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1330.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_885.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1333.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_883.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1331.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_884.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1332.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_666.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1114.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_669.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1117.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_667.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1115.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_668.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1116.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_662.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1110.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_665.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1113.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_663.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1111.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_664.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1112.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_670.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1118.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_673.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1121.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_671.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1119.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_672.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1120.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_674.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1122.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_677.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1125.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_675.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1123.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_676.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1124.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1546.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1549.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1547.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1548.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1542.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1545.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1543.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1544.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1550.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1553.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1551.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1552.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1554.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1557.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1555.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1556.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_378.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_381.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_379.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_380.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_374.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_377.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_375.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_376.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_382.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_385.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_383.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_384.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_386.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_389.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_387.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_388.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_810.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1258.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_813.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1261.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_811.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1259.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_812.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1260.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_806.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1254.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_809.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1257.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_807.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1255.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_808.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1256.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_814.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1262.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_817.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1265.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_815.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1263.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_816.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1264.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_818.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1266.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_821.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1269.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_819.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1267.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_820.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1268.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_602.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1050.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_605.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1053.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_603.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1051.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_604.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1052.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_598.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1046.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_601.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1049.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_599.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1047.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_600.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1048.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_606.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1054.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_609.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1057.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_607.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1055.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_608.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1056.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_610.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1058.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_613.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1061.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_611.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1059.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_612.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1060.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1482.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1485.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1483.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1484.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1478.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1481.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1479.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1480.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1486.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1489.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1487.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1488.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1490.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1493.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1491.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1492.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_346.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_349.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_347.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_348.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_342.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_345.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_343.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_344.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_350.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_353.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_351.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_352.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_354.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_357.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_355.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_356.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_778.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1226.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_781.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1229.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_779.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1227.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_780.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1228.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_774.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1222.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_777.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1225.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_775.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1223.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_776.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1224.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_782.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1230.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_785.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1233.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_783.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1231.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_784.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1232.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_786.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1234.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_789.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1237.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_787.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1235.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_788.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1236.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_570.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1018.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_573.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1021.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_571.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1019.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_572.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1020.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_566.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1014.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_569.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1017.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_567.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1015.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_568.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1016.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_574.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1022.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_577.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1025.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_575.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1023.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_576.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1024.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_578.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1026.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_581.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1029.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_579.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1027.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_580.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1028.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1450.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1453.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1451.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1452.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1446.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1449.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1447.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1448.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1454.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1457.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1455.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1456.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1458.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1461.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1459.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1460.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_314.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_317.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_315.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_316.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_310.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_313.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_311.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_312.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_318.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_321.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_319.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_320.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_322.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_325.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_323.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_324.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_746.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1194.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_749.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1197.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_747.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1195.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_748.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1196.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_742.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1190.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_745.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1193.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_743.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1191.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_744.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1192.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_750.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1198.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_753.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1201.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_751.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1199.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_752.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1200.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_754.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1202.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_757.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1205.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_755.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1203.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_756.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1204.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_538.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_986.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_541.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_989.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_539.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_987.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_540.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_988.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_534.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_982.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_537.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_985.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_535.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_983.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_536.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_984.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_542.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_990.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_545.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_993.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_543.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_991.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_544.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_992.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_546.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_994.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_549.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_997.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_547.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_995.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_548.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_996.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1418.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1421.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1419.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1420.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1414.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1417.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1415.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1416.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1422.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1425.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1423.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1424.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1426.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1429.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1427.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1428.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_410.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_413.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_411.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_412.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_406.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_409.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_407.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_408.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_414.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_417.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_415.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_416.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_418.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_421.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_419.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_420.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_842.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1290.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_845.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1293.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_843.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1291.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_844.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1292.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_838.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1286.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_841.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1289.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_839.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1287.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_840.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1288.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_846.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1294.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_849.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1297.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_847.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1295.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_848.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1296.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_850.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1298.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_853.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1301.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_851.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1299.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_852.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1300.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_634.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1082.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_637.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1085.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_635.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1083.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_636.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1084.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_630.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1078.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_633.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1081.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_631.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1079.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_632.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1080.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_638.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1086.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_641.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1089.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_639.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1087.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_640.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1088.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_642.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1090.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_645.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1093.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_643.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1091.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_644.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1092.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1514.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1517.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1515.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1516.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1510.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1513.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1511.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1512.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1518.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1521.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1519.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1520.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1522.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1525.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1523.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1524.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_698.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1146.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_701.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1149.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_699.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1147.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_700.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1148.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_694.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1142.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_697.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1145.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_695.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1143.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_696.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1144.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_702.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1150.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_705.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1153.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_703.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1151.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_704.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1152.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_706.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1154.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_709.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1157.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_707.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1155.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_708.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1156.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1578.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1581.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1579.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1580.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1574.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1577.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1575.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1576.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1582.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1585.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1583.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1584.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1586.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1589.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1587.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1588.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1370.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1373.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1371.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1372.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1366.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1369.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1367.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1368.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1374.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1377.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1375.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1376.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1378.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1381.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1379.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1380.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_266.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_269.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_267.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_268.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_262.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_265.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_263.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_264.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_270.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_273.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_271.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_272.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_274.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_277.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_275.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_276.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_58.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_61.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_59.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_60.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_54.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_57.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_55.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_56.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_62.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_65.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_63.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_64.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_66.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_69.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_67.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_68.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_490.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_938.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_493.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_941.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_491.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_939.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_492.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_940.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_486.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_934.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_489.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_937.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_487.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_935.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_488.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_936.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_494.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_942.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_497.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_945.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_495.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_943.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_496.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_944.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_498.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_946.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_501.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_949.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_499.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_947.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_500.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_948.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_426.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_429.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_427.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_428.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_422.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_425.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_423.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_424.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_430.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_433.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_431.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_432.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_434.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_437.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_435.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_436.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_218.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_221.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_219.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_220.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_214.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_217.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_215.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_216.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_222.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_225.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_223.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_224.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_226.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_229.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_227.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_228.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_650.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1098.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_653.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1101.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_651.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1099.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_652.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1100.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_646.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1094.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_649.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1097.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_647.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1095.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_648.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1096.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_654.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1102.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_657.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1105.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_655.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1103.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_656.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1104.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_658.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1106.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_661.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1109.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_659.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1107.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_660.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1108.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_362.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_365.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_363.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_364.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_358.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_361.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_359.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_360.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_366.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_369.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_367.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_368.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_370.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_373.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_371.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_372.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_154.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_157.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_155.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_156.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_150.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_153.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_151.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_152.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_158.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_161.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_159.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_160.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_162.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_165.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_163.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_164.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_586.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1034.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_589.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1037.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_587.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1035.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_588.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1036.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_582.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1030.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_585.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1033.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_583.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1031.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_584.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1032.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_590.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1038.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_593.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1041.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_591.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1039.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_592.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1040.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_594.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1042.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_597.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1045.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_595.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1043.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_596.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1044.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_330.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_333.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_331.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_332.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_326.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_329.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_327.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_328.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_334.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_337.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_335.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_336.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_338.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_341.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_339.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_340.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_122.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_125.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_123.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_124.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_118.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_121.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_119.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_120.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_126.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_129.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_127.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_128.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_130.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_133.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_131.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_132.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_554.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1002.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_557.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1005.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_555.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1003.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_556.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1004.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_550.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_998.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_553.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1001.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_551.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_999.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_552.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1000.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_558.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1006.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_561.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1009.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_559.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1007.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_560.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1008.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_562.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1010.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_565.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1013.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_563.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1011.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_564.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1012.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_298.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_301.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_299.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_300.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_294.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_297.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_295.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_296.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_302.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_305.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_303.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_304.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_306.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_309.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_307.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_308.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_90.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_93.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_91.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_92.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_86.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_89.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_87.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_88.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_94.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_97.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_95.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_96.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_98.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_101.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_99.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_100.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_522.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_970.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_525.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_973.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_523.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_971.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_524.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_972.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_518.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_966.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_521.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_969.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_519.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_967.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_520.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_968.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_526.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_974.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_529.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_977.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_527.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_975.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_528.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_976.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_530.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_978.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_533.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_981.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_531.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_979.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_532.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_980.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_394.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_397.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_395.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_396.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_390.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_393.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_391.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_392.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_398.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_401.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_399.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_400.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_402.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_405.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_403.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_404.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_186.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_189.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_187.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_188.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_182.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_185.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_183.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_184.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_190.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_193.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_191.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_192.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_194.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_197.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_195.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_196.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_618.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1066.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_621.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1069.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_619.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1067.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_620.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1068.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_614.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1062.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_617.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1065.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_615.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1063.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_616.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1064.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_622.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1070.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_625.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1073.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_623.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1071.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_624.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1072.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_626.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1074.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_629.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1077.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_627.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1075.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_628.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1076.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_250.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_253.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_251.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_252.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_246.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_249.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_247.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_248.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_254.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_257.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_255.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_256.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_258.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_261.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_259.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_260.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_682.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1130.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_685.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1133.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_683.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1131.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_684.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1132.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_678.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1126.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_681.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1129.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_679.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1127.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_680.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1128.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_686.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1134.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_689.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1137.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_687.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1135.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_688.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1136.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_690.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1138.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_693.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1141.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_691.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1139.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_692.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1140.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_474.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_922.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_477.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_925.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_475.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_923.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_476.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_924.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_470.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_918.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_473.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_921.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_471.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_919.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_472.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_920.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_478.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_926.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_481.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_929.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_479.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_927.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_480.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_928.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_482.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_930.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_485.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_933.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_483.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_931.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_484.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_932.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1354.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1357.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1355.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1356.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1350.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1353.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1351.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1352.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1358.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1361.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1359.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1360.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1362.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1365.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1363.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_1364.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_42.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_45.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_43.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_44.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_38.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_41.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_39.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_40.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_46.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_49.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_47.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_48.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_50.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_53.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_51.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_52.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_202.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_205.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_203.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_204.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_198.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_201.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_199.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_200.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_206.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_209.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_207.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_208.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_210.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_213.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_211.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_212.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_138.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_141.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_139.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_140.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_134.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_137.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_135.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_136.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_142.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_145.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_143.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_144.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_146.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_149.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_147.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_148.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_106.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_109.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_107.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_108.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_102.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_105.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_103.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_104.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_110.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_113.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_111.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_112.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_114.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_117.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_115.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_116.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_74.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_77.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_75.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_76.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_70.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_73.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_71.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_72.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_78.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_81.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_79.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_80.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_82.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_85.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_83.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_84.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_170.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_173.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_171.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_172.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_166.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_169.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_167.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_168.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_174.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_177.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_175.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_176.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_178.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_181.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_179.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_180.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_234.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_237.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_235.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_236.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_230.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_233.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_231.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_232.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_238.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_241.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_239.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_240.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_242.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_245.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_243.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_244.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_26.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_29.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_27.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_28.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_22.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_25.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_23.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_24.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_30.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_33.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_31.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_32.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_34.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_37.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_35.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_36.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_458.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_906.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_461.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_909.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_459.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_907.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_460.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_908.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_454.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_902.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_457.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_905.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_455.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_903.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_456.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_904.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_462.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_910.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_465.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_913.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_463.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_911.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_464.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_912.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_466.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_914.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_469.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_917.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_467.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_915.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_468.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_916.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_10.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_13.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_11.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_12.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_6.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_9.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_7.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_8.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_14.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_17.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_15.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_16.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_18.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_21.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_19.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_20.hlsl"
        #else
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_vs_20.hlsl"
        #endif
        // ---- progFragment
        #if defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3446.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3449.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3447.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3448.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3442.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3445.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3443.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3444.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3450.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3453.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3451.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3452.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3454.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3457.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3455.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3456.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3606.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3609.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3607.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3608.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3602.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3605.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3603.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3604.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3610.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3613.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3611.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3612.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3614.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3617.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3615.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3616.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3542.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3545.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3543.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3544.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3538.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3541.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3539.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3540.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3546.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3549.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3547.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3548.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3550.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3553.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3551.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3552.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3510.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3513.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3511.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3512.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3506.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3509.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3507.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3508.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3514.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3517.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3515.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3516.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3518.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3521.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3519.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3520.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3478.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3481.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3479.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3480.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3474.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3477.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3475.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3476.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3482.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3485.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3483.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3484.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3486.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3489.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3487.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3488.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3574.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3577.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3575.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3576.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3570.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3573.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3571.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3572.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3578.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3581.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3579.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3580.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3582.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3585.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3583.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3584.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2550.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2998.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2553.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3001.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2551.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2999.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2552.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3000.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2546.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2994.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2549.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2997.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2547.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2995.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2548.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2996.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2554.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3002.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2557.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3005.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2555.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3003.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2556.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3004.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2558.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3006.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2561.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3009.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2559.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3007.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2560.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3008.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3430.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3433.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3431.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3432.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3426.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3429.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3427.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3428.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3434.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3437.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3435.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3436.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3438.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3441.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3439.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3440.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3222.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3225.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3223.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3224.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3218.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3221.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3219.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3220.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3226.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3229.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3227.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3228.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3230.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3233.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3231.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3232.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2710.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3158.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2713.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3161.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2711.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3159.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2712.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3160.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2706.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3154.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2709.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3157.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2707.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3155.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2708.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3156.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2714.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3162.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2717.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3165.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2715.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3163.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2716.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3164.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2718.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3166.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2721.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3169.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2719.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3167.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2720.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3168.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3590.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3593.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3591.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3592.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3586.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3589.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3587.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3588.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3594.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3597.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3595.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3596.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3598.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3601.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3599.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3600.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3382.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3385.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3383.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3384.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3378.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3381.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3379.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3380.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3386.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3389.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3387.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3388.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3390.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3393.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3391.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3392.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2646.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3094.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2649.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3097.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2647.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3095.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2648.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3096.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2642.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3090.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2645.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3093.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2643.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3091.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2644.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3092.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2650.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3098.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2653.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3101.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2651.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3099.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2652.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3100.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2654.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3102.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2657.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3105.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2655.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3103.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2656.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3104.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3526.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3529.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3527.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3528.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3522.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3525.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3523.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3524.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3530.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3533.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3531.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3532.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3534.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3537.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3535.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3536.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3318.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3321.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3319.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3320.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3314.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3317.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3315.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3316.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3322.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3325.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3323.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3324.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3326.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3329.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3327.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3328.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2614.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3062.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2617.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3065.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2615.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3063.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2616.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3064.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2610.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3058.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2613.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3061.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2611.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3059.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2612.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3060.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2618.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3066.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2621.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3069.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2619.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3067.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2620.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3068.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2622.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3070.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2625.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3073.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2623.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3071.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2624.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3072.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3494.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3497.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3495.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3496.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3490.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3493.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3491.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3492.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3498.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3501.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3499.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3500.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3502.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3505.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3503.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3504.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3286.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3289.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3287.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3288.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3282.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3285.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3283.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3284.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3290.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3293.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3291.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3292.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3294.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3297.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3295.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3296.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2582.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3030.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2585.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3033.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2583.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3031.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2584.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3032.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2578.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3026.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2581.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3029.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2579.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3027.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2580.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3028.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2586.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3034.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2589.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3037.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2587.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3035.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2588.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3036.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2590.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3038.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2593.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3041.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2591.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3039.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2592.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3040.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3462.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3465.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3463.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3464.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3458.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3461.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3459.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3460.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3466.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3469.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3467.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3468.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3470.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3473.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3471.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3472.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3254.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3257.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3255.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3256.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3250.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3253.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3251.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3252.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3258.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3261.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3259.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3260.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3262.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3265.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3263.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3264.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2678.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3126.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2681.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3129.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2679.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3127.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2680.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3128.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2674.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3122.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2677.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3125.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2675.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3123.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2676.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3124.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2682.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3130.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2685.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3133.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2683.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3131.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2684.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3132.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2686.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3134.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2689.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3137.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2687.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3135.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2688.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3136.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3558.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3561.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3559.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3560.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3554.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3557.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3555.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3556.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3562.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3565.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3563.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3564.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3566.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3569.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3567.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3568.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3350.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3353.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3351.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3352.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3346.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3349.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3347.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3348.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3354.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3357.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3355.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3356.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3358.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3361.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3359.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3360.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3414.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3417.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3415.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3416.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3410.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3413.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3411.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3412.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3418.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3421.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3419.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3420.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3422.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3425.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3423.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3424.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2102.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2105.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2103.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2104.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2098.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2101.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2099.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2100.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2106.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2109.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2107.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2108.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2110.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2113.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2111.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2112.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2534.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2982.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2537.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2985.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2535.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2983.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2536.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2984.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2530.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2978.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2533.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2981.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2531.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2979.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2532.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2980.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2538.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2986.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2541.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2989.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2539.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2987.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2540.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2988.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2542.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2990.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2545.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2993.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2543.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2991.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2544.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2992.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2326.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2774.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2329.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2777.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2327.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2775.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2328.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2776.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2322.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2770.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2325.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2773.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2323.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2771.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2324.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2772.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2330.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2778.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2333.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2781.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2331.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2779.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2332.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2780.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2334.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2782.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2337.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2785.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2335.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2783.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2336.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2784.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3206.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3209.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3207.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3208.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3202.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3205.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3203.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3204.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3210.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3213.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3211.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3212.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3214.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3217.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3215.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3216.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2262.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2265.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2263.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2264.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2258.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2261.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2259.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2260.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2266.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2269.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2267.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2268.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2270.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2273.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2271.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2272.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2694.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3142.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2697.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3145.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2695.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3143.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2696.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3144.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2690.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3138.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2693.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3141.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2691.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3139.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2692.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3140.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2698.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3146.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2701.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3149.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2699.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3147.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2700.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3148.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2702.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3150.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2705.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3153.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2703.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3151.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2704.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3152.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2486.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2934.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2489.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2937.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2487.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2935.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2488.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2936.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2482.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2930.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2485.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2933.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2483.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2931.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2484.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2932.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2490.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2938.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2493.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2941.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2491.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2939.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2492.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2940.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2494.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2942.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2497.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2945.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2495.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2943.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2496.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2944.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3366.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3369.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3367.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3368.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3362.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3365.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3363.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3364.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3370.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3373.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3371.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3372.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3374.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3377.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3375.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3376.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2198.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2201.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2199.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2200.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2194.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2197.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2195.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2196.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2202.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2205.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2203.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2204.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2206.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2209.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2207.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2208.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2630.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3078.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2633.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3081.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2631.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3079.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2632.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3080.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2626.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3074.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2629.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3077.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2627.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3075.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2628.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3076.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2634.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3082.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2637.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3085.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2635.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3083.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2636.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3084.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2638.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3086.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2641.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3089.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2639.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3087.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2640.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3088.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2422.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2870.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2425.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2873.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2423.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2871.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2424.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2872.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2418.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2866.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2421.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2869.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2419.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2867.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2420.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2868.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2426.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2874.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2429.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2877.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2427.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2875.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2428.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2876.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2430.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2878.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2433.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2881.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2431.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2879.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2432.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2880.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3302.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3305.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3303.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3304.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3298.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3301.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3299.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3300.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3306.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3309.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3307.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3308.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3310.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3313.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3311.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3312.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2166.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2169.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2167.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2168.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2162.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2165.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2163.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2164.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2170.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2173.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2171.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2172.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2174.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2177.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2175.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2176.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2598.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3046.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2601.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3049.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2599.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3047.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2600.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3048.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2594.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3042.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2597.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3045.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2595.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3043.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2596.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3044.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2602.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3050.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2605.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3053.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2603.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3051.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2604.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3052.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2606.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3054.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2609.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3057.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2607.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3055.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2608.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3056.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2390.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2838.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2393.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2841.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2391.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2839.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2392.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2840.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2386.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2834.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2389.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2837.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2387.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2835.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2388.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2836.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2394.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2842.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2397.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2845.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2395.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2843.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2396.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2844.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2398.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2846.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2401.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2849.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2399.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2847.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2400.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2848.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3270.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3273.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3271.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3272.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3266.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3269.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3267.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3268.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3274.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3277.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3275.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3276.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3278.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3281.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3279.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3280.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2134.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2137.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2135.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2136.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2130.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2133.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2131.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2132.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2138.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2141.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2139.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2140.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2142.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2145.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2143.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2144.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2566.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3014.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2569.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3017.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2567.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3015.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2568.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3016.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2562.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3010.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2565.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3013.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2563.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3011.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2564.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3012.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2570.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3018.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2573.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3021.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2571.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3019.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2572.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3020.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2574.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3022.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2577.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3025.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2575.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3023.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2576.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3024.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2358.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2806.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2361.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2809.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2359.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2807.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2360.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2808.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2354.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2802.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2357.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2805.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2355.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2803.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2356.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2804.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2362.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2810.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2365.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2813.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2363.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2811.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2364.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2812.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2366.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2814.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2369.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2817.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2367.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2815.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2368.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2816.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3238.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3241.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3239.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3240.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3234.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3237.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3235.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3236.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3242.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3245.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3243.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3244.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3246.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3249.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3247.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3248.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2230.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2233.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2231.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2232.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2226.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2229.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2227.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2228.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2234.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2237.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2235.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2236.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2238.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2241.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2239.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2240.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2662.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3110.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2665.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3113.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2663.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3111.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2664.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3112.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2658.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3106.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2661.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3109.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2659.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3107.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2660.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3108.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2666.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3114.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2669.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3117.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2667.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3115.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2668.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3116.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2670.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3118.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2673.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3121.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2671.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3119.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2672.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3120.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2454.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2902.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2457.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2905.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2455.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2903.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2456.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2904.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2450.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2898.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2453.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2901.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2451.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2899.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2452.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2900.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2458.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2906.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2461.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2909.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2459.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2907.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2460.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2908.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2462.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2910.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2465.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2913.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2463.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2911.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2464.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2912.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3334.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3337.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3335.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3336.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3330.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3333.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3331.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3332.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3338.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3341.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3339.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3340.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3342.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3345.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3343.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3344.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2518.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2966.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2521.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2969.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2519.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2967.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2520.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2968.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2514.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2962.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2517.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2965.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2515.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2963.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2516.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2964.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2522.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2970.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2525.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2973.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2523.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2971.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2524.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2972.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2526.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2974.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2529.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2977.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2527.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2975.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2528.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2976.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3398.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3401.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3399.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3400.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3394.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3397.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3395.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3396.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3402.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3405.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3403.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3404.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3406.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3409.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3407.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3408.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3190.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3193.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3191.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3192.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3186.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3189.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3187.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3188.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3194.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3197.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3195.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3196.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3198.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3201.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3199.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3200.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2086.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2089.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2087.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2088.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2082.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2085.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2083.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2084.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2090.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2093.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2091.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2092.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2094.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2097.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2095.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2096.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1878.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1881.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1879.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1880.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1874.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1877.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1875.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1876.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1882.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1885.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1883.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1884.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1886.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1889.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1887.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1888.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2310.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2758.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2313.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2761.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2311.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2759.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2312.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2760.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2306.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2754.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2309.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2757.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2307.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2755.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2308.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2756.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2314.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2762.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2317.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2765.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2315.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2763.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2316.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2764.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2318.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2766.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2321.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2769.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2319.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2767.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2320.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2768.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2246.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2249.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2247.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2248.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2242.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2245.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2243.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2244.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2250.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2253.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2251.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2252.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2254.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2257.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2255.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2256.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2038.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2041.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2039.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2040.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2034.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2037.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2035.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2036.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2042.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2045.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2043.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2044.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2046.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2049.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2047.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2048.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2470.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2918.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2473.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2921.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2471.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2919.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2472.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2920.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2466.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2914.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2469.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2917.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2467.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2915.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2468.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2916.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2474.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2922.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2477.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2925.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2475.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2923.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2476.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2924.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2478.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2926.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2481.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2929.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2479.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2927.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2480.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2928.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2182.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2185.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2183.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2184.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2178.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2181.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2179.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2180.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2186.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2189.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2187.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2188.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2190.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2193.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2191.hlsl"
        #elif defined(BLEND_DARKEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2192.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1974.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1977.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1975.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1976.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1970.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1973.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1971.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1972.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1978.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1981.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1979.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1980.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1982.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1985.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1983.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1984.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2406.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2854.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2409.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2857.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2407.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2855.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2408.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2856.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2402.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2850.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2405.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2853.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2403.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2851.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2404.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2852.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2410.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2858.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2413.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2861.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2411.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2859.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2412.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2860.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2414.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2862.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2417.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2865.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2415.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2863.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2416.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2864.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2150.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2153.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2151.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2152.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2146.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2149.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2147.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2148.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2154.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2157.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2155.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2156.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2158.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2161.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2159.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2160.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1942.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1945.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1943.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1944.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1938.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1941.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1939.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1940.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1946.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1949.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1947.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1948.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1950.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1953.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1951.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1952.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2374.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2822.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2377.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2825.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2375.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2823.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2376.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2824.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2370.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2818.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2373.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2821.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2371.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2819.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2372.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2820.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2378.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2826.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2381.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2829.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2379.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2827.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2380.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2828.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2382.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2830.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2385.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2833.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2383.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2831.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2384.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2832.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2118.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2121.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2119.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2120.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2114.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2117.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2115.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2116.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2122.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2125.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2123.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2124.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2126.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2129.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2127.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2128.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1910.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1913.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1911.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1912.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1906.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1909.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1907.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1908.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1914.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1917.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1915.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1916.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1918.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1921.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1919.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1920.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2342.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2790.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2345.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2793.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2343.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2791.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2344.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2792.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2338.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2786.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2341.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2789.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2339.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2787.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2340.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2788.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2346.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2794.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2349.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2797.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2347.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2795.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2348.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2796.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2350.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2798.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2353.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2801.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2351.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2799.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2352.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2800.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2214.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2217.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2215.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2216.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2210.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2213.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2211.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2212.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2218.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2221.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2219.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2220.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2222.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2225.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2223.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2224.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2006.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2009.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2007.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2008.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2002.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2005.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2003.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2004.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2010.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2013.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2011.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2012.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2014.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2017.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2015.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2016.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2438.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2886.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2441.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2889.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2439.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2887.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2440.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2888.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2434.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2882.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2437.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2885.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2435.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2883.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2436.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2884.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2442.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2890.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2445.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2893.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2443.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2891.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2444.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2892.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2446.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2894.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2449.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2897.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2447.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2895.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2448.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2896.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2070.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2073.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2071.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2072.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2066.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2069.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2067.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2068.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2074.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2077.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2075.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2076.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2078.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2081.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2079.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2080.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2502.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2950.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2505.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2953.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2503.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2951.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2504.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2952.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2498.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2946.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2501.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2949.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2499.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2947.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2500.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2948.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2506.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2954.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2509.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2957.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2507.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2955.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2508.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2956.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2510.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2958.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2513.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2961.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2511.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2959.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2512.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2960.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2294.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2742.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2297.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2745.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2295.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2743.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2296.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2744.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2290.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2738.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2293.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2741.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2291.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2739.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2292.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2740.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2298.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2746.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2301.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2749.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2299.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2747.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2300.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2748.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2302.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2750.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2305.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2753.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2303.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2751.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2304.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2752.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3174.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3177.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3175.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3176.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3170.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3173.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3171.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3172.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3178.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3181.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3179.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3180.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3182.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3185.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3183.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_3184.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1862.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1865.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1863.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1864.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1858.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1861.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1859.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1860.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1866.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1869.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1867.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1868.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1870.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1873.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1871.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1872.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2022.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2025.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2023.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2024.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2018.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2021.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2019.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2020.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2026.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2029.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2027.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2028.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2030.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2033.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2031.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2032.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1958.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1961.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1959.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1960.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1954.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1957.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1955.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1956.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1962.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1965.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1963.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1964.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1966.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1969.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1967.hlsl"
        #elif defined(BLEND_DARKEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1968.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1926.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1929.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1927.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1928.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1922.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1925.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1923.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1924.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1930.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1933.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1931.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1932.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1934.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1937.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1935.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1936.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1894.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1897.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1895.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1896.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1890.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1893.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1891.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1892.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1898.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1901.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1899.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1900.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1902.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1905.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1903.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1904.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1990.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1993.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1991.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1992.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1986.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1989.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1987.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1988.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1994.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1997.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1995.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1996.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1998.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2001.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1999.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2000.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2054.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2057.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2055.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2056.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2050.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2053.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2051.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2052.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2058.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2061.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2059.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2060.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2062.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2065.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2063.hlsl"
        #elif defined(DITHER) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2064.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1846.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1849.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1847.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1848.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1842.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1845.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1843.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1844.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1850.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1853.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1851.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1852.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1854.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1857.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1855.hlsl"
        #elif defined(GRADIENT_COLORSPACE_PERCEPTUAL) && defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1856.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2278.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2726.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2281.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2729.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2279.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2727.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2280.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2728.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2274.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2722.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2277.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2725.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2275.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2723.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2276.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2724.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2282.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2730.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2285.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2733.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2283.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2731.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2284.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2732.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2286.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2734.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2289.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2737.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2287.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2735.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2288.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_2736.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1830.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1833.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1831.hlsl"
        #elif defined(GRADIENT_LERP_LINEAR) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1832.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1826.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1829.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1827.hlsl"
        #elif defined(GRADIENT_LERP_SMOOTH) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1828.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1834.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1837.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1835.hlsl"
        #elif defined(GRADIENT_LERP_STEP) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1836.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_AXIS)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1838.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_CONIC)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1841.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_LINEAR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1839.hlsl"
        #elif defined(GRADIENT_LERP_STEPAA) && defined(GRADIENT_SHAPE_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1840.hlsl"
        #else
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Gradient/p0_ps_1840.hlsl"
        #endif
        ENDHLSL
    }
}
}
