Shader "Hidden/ChocDino/UIFX/Blend-Frame"
{
Properties
{
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    [PerRendererData] _ResultTex ("Sprite Texture", 2D) = "white" {}
    [PerRendererData] _SourceTex ("Source Texture", 2D) = "white" {}
    [PerRendererData] _FillTex ("Fill Texture", 2D) = "white" {}
    [PerRendererData] _BorderFillTex ("Border Fill Texture", 2D) = "white" {}
    _StencilComp ("Stencil Comparison", Float) = 8
    _Stencil ("Stencil ID", Float) = 0
    _StencilOp ("Stencil Operation", Float) = 0
    _StencilWriteMask ("Stencil Write Mask", Float) = 255
    _StencilReadMask ("Stencil Read Mask", Float) = 255
    _CutoutAlpha ("Cutout Alpha", Float) = 1
    _FillSoft ("Fill Soft", Float) = 1
    _FillColor ("Fill Color", Color) = (0.20000000298023224, 0.20000000298023224, 0.20000000298023224, 1)
    _GradientAxisParams ("Axis", Vector) = (1, 0, 0, 0)
    _GradientParams ("Gradient", Vector) = (1, 0, 0, 0)
    _EdgeRounding ("Edge Rounding", Vector) = (0, 0, 0, 0)
    _BorderColor ("Border Color", Color) = (0.800000011920929, 0.800000011920929, 0.800000011920929, 1)
    _BorderGradientAxisParams ("Border Axis", Vector) = (1, 0, 0, 0)
    _BorderGradientParams ("Border Gradient", Vector) = (1, 0, 0, 0)
    _BorderSizeSoft ("Border Size,Soft", Vector) = (4, 1, 0, 0)
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    _ColorMask ("Color Mask", Float) = 15
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Name "Blend-Frame"
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
        #pragma multi_compile_local _ BORDER
        #pragma multi_compile_local _ CUTOUT
        #pragma multi_compile_local _ GRADIENT_RADIAL
        #pragma multi_compile_local _ GRADIENT_RADIAL_BORDER
        #pragma multi_compile_local _ SHAPE_CIRCLE SHAPE_ROUNDRECT
        #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
        #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
        #pragma multi_compile_local _ USE_BORDER_TEXTURE
        #pragma multi_compile_local _ USE_TEXTURE

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
        float4 _Rect_ST;
        float4 _ClipRect;
        float _UIMaskSoftnessX;
        float _CutoutAlpha;
        float _UIMaskSoftnessY;
        float4 _EdgeRounding;
        float4 _FillColor;
        float _FillSoft;
        float4 _GradientAxisParams;
        float4 _BorderColor;
        float2 _BorderSizeSoft;
        float4 _GradientParams;
        float4 _BorderGradientAxisParams;
        float4 _BorderGradientParams;
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

        Texture2D<float4> _BorderFillTex;
        Texture2D<float4> _FillTex;
        Texture2D<float4> _SourceTex;
        SamplerState sampler_BorderFillTex;
        SamplerState sampler_FillTex;
        SamplerState sampler_SourceTex;

        // ---- progVertex
        #if defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_713.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_777.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_705.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_709.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_329.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_521.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_769.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_773.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_393.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_585.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_649.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_712.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_776.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_711.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_775.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_681.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_745.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_697.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_761.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_701.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_321.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_325.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_513.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_517.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_137.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_765.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_385.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_389.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_577.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_581.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_201.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_641.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_645.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_265.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_457.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_704.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_708.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_328.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_520.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_768.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_772.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_392.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_584.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_648.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_703.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_707.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_327.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_519.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_767.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_771.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_391.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_583.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_647.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_710.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_774.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_673.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_677.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_297.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_489.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_737.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_741.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_361.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_553.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_617.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_680.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_744.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_679.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_743.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_689.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_693.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_313.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_505.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_753.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_757.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_377.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_569.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_633.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_696.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_760.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_695.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_759.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_665.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_729.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_317.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_509.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_129.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_133.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_381.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_573.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_193.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_197.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_637.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_257.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_261.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_449.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_453.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_73.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_700.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_320.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_324.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_512.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_516.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_136.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_764.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_384.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_388.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_576.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_580.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_200.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_640.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_644.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_264.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_456.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_699.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_319.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_323.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_511.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_515.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_135.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_763.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_383.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_387.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_575.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_579.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_199.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_639.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_643.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_263.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_455.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_702.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_706.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_326.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_518.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_766.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_770.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_390.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_582.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_646.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_669.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_289.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_293.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_481.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_485.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_105.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_733.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_353.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_357.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_545.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_549.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_169.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_609.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_613.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_233.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_425.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_672.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_676.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_296.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_488.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_736.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_740.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_360.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_552.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_616.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_671.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_675.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_295.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_487.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_735.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_739.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_359.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_551.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_615.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_678.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_742.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_685.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_305.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_309.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_497.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_501.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_121.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_749.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_369.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_373.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_561.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_565.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_185.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_625.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_629.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_249.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_441.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_688.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_692.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_312.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_504.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_752.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_756.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_376.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_568.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_632.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_687.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_691.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_311.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_503.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_751.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_755.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_375.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_567.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_631.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_694.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_758.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_657.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_661.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_281.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_473.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_721.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_725.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_345.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_537.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_601.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_664.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_728.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_663.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_727.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_125.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_189.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_253.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_445.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_65.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_69.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_316.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_508.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_128.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_132.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_380.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_572.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_192.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_196.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_636.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_256.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_260.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_448.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_452.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_72.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_315.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_507.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_127.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_131.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_379.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_571.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_191.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_195.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_635.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_255.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_259.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_447.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_451.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_71.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_698.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_318.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_322.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_510.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_514.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_134.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_762.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_382.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_386.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_574.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_578.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_198.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_638.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_642.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_262.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_454.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_285.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_477.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_97.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_101.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_349.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_541.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_161.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_165.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_605.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_225.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_229.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_417.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_421.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_41.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_668.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_288.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_292.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_480.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_484.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_104.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_732.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_352.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_356.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_544.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_548.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_168.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_608.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_612.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_232.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_424.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_667.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_287.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_291.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_479.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_483.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_103.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_731.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_351.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_355.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_543.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_547.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_167.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_607.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_611.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_231.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_423.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_670.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_674.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_294.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_486.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_734.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_738.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_358.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_550.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_614.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_301.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_493.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_113.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_117.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_365.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_557.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_177.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_181.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_621.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_241.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_245.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_433.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_437.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_57.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_684.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_304.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_308.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_496.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_500.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_120.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_748.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_368.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_372.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_560.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_564.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_184.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_624.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_628.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_248.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_440.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_683.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_303.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_307.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_495.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_499.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_119.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_747.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_367.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_371.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_559.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_563.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_183.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_623.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_627.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_247.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_439.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_686.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_690.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_310.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_502.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_750.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_754.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_374.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_566.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_630.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_653.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_273.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_277.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_465.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_469.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_89.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_717.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_337.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_341.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_529.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_533.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_153.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_593.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_597.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_217.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_409.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_656.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_660.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_280.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_472.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_720.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_724.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_344.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_536.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_600.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_655.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_659.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_279.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_471.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_719.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_723.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_343.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_535.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_599.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_662.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_726.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_61.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_124.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_188.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_252.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_444.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_64.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_68.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_123.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_187.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_251.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_443.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_63.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_67.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_314.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_506.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_126.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_130.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_378.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_570.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_190.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_194.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_634.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_254.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_258.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_446.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_450.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_70.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_93.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_157.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_221.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_413.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_33.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_37.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_284.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_476.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_96.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_100.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_348.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_540.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_160.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_164.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_604.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_224.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_228.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_416.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_420.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_40.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_283.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_475.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_95.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_99.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_347.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_539.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_159.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_163.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_603.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_223.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_227.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_415.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_419.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_39.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_666.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_286.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_290.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_478.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_482.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_102.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_730.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_350.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_354.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_542.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_546.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_166.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_606.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_610.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_230.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_422.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_109.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_173.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_237.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_429.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_49.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_53.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_300.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_492.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_112.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_116.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_364.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_556.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_176.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_180.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_620.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_240.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_244.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_432.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_436.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_56.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_299.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_491.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_111.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_115.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_363.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_555.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_175.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_179.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_619.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_239.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_243.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_431.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_435.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_55.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_682.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_302.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_306.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_494.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_498.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_118.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_746.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_366.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_370.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_558.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_562.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_182.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_622.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_626.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_246.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_438.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_269.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_461.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_81.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_85.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_333.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_525.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_145.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_149.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_589.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_209.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_213.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_401.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_405.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_25.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_652.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_272.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_276.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_464.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_468.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_88.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_716.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_336.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_340.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_528.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_532.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_152.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_592.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_596.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_216.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_408.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_651.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_271.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_275.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_463.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_467.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_87.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_715.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_335.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_339.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_527.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_531.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_151.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_591.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_595.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_215.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_407.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_654.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_658.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_278.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_470.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_718.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_722.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_342.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_534.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_598.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_60.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_59.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_122.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_186.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_250.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_442.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_62.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_66.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_29.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_92.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_156.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_220.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_412.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_32.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_36.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_91.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_155.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_219.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_411.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_31.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_35.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_282.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_474.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_94.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_98.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_346.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_538.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_158.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_162.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_602.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_222.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_226.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_414.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_418.hlsl"
        #elif defined(BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_38.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_45.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_108.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_172.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_236.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_428.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_48.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_52.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_107.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_171.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_235.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_427.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_47.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_51.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_298.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_490.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_110.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_114.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_362.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_554.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_174.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_178.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_618.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_238.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_242.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_430.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_434.hlsl"
        #elif defined(CUTOUT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_54.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_77.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_141.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_205.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_397.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_17.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_21.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_268.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_460.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_80.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_84.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_332.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_524.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_144.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_148.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_588.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_208.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_212.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_400.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_404.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_24.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_267.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_459.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_79.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_83.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_331.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_523.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_143.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_147.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_587.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_207.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_211.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_399.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_403.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_23.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_650.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_270.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_274.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_462.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_466.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_86.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_714.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_334.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_338.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_526.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_530.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_150.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_590.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_594.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_214.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_406.hlsl"
        #elif defined(BORDER) && defined(CUTOUT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_58.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_28.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_27.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_90.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_154.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_218.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_410.hlsl"
        #elif defined(BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_30.hlsl"
        #elif defined(BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_34.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_44.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_43.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_106.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_170.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_234.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_426.hlsl"
        #elif defined(CUTOUT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_46.hlsl"
        #elif defined(CUTOUT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_50.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_13.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_76.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_140.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_204.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_396.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_16.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_20.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_75.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_139.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_203.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_395.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_15.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_19.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_266.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_458.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_78.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_82.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_330.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_522.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_142.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_146.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_586.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_206.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_210.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_398.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_402.hlsl"
        #elif defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_22.hlsl"
        #elif defined(BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_26.hlsl"
        #elif defined(CUTOUT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_42.hlsl"
        #elif defined(GRADIENT_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_12.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_11.hlsl"
        #elif defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_74.hlsl"
        #elif defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_138.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_202.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_394.hlsl"
        #elif defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_14.hlsl"
        #elif defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_18.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_vs_10.hlsl"
        #endif
        // ---- progFragment
        #if defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1705.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1769.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1697.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1701.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1321.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1513.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1761.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1765.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1385.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1577.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1641.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1704.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1768.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1703.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1767.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1673.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1737.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1689.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1753.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1693.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1313.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1317.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1505.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1509.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1129.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1757.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1377.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1381.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1569.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1573.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1193.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1633.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1637.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1257.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1449.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1696.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1700.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1320.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1512.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1760.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1764.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1384.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1576.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1640.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1695.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1699.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1319.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1511.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1759.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1763.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1383.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1575.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1639.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1702.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1766.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1665.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1669.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1289.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1481.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1729.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1733.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1353.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1545.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1609.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1672.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1736.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1671.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1735.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1681.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1685.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1305.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1497.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1745.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1749.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1369.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1561.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1625.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1688.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1752.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1687.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1751.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1657.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1721.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1309.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1501.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1121.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1125.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1373.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1565.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1185.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1189.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1629.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1249.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1253.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1441.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1445.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1065.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1692.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1312.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1316.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1504.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1508.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1128.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1756.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1376.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1380.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1568.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1572.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1192.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1632.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1636.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1256.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1448.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1691.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1311.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1315.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1503.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1507.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1127.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1755.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1375.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1379.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1567.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1571.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1191.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1631.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1635.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1255.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1447.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1694.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1698.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1318.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1510.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1758.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1762.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1382.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1574.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1638.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1661.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1281.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1285.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1473.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1477.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1097.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1725.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1345.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1349.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1537.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1541.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1161.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1601.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1605.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1225.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1417.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1664.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1668.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1288.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1480.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1728.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1732.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1352.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1544.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1608.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1663.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1667.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1287.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1479.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1727.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1731.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1351.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1543.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1607.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1670.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1734.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1677.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1297.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1301.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1489.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1493.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1113.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1741.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1361.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1365.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1553.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1557.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1177.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1617.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1621.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1241.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1433.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1680.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1684.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1304.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1496.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1744.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1748.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1368.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1560.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1624.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1679.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1683.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1303.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1495.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1743.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1747.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1367.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1559.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1623.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1686.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1750.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1649.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1653.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1273.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1465.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1713.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1717.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1337.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1529.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1593.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1656.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1720.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1655.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1719.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1117.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1181.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1245.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1437.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1057.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1061.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1308.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1500.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1120.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1124.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1372.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1564.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1184.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1188.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1628.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1248.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1252.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1440.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1444.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1064.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1307.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1499.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1119.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1123.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1371.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1563.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1183.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1187.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1627.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1247.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1251.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1439.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1443.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1063.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1690.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1310.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1314.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1502.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1506.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1126.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1754.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1374.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1378.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1566.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1570.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1190.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1630.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1634.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1254.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1446.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1277.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1469.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1089.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1093.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1341.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1533.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1153.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1157.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1597.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1217.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1221.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1409.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1413.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1033.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1660.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1280.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1284.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1472.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1476.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1096.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1724.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1344.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1348.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1536.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1540.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1160.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1600.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1604.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1224.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1416.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1659.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1279.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1283.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1471.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1475.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1095.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1723.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1343.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1347.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1535.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1539.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1159.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1599.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1603.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1223.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1415.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1662.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1666.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1286.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1478.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1726.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1730.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1350.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1542.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1606.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1293.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1485.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1105.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1109.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1357.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1549.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1169.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1173.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1613.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1233.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1237.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1425.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1429.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1049.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1676.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1296.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1300.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1488.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1492.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1112.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1740.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1360.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1364.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1552.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1556.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1176.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1616.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1620.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1240.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1432.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1675.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1295.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1299.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1487.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1491.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1111.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1739.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1359.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1363.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1551.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1555.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1175.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1615.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1619.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1239.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1431.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1678.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1682.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1302.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1494.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1742.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1746.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1366.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1558.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1622.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1645.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1265.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1269.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1457.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1461.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1081.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1709.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1329.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1333.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1521.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1525.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1145.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1585.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1589.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1209.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1401.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1648.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1652.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1272.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1464.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1712.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1716.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1336.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1528.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1592.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1647.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1651.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1271.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1463.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1711.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1715.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1335.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1527.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1591.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1654.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1718.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1053.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1116.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1180.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1244.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1436.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1056.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1060.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1115.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1179.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1243.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1435.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1055.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1059.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1306.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1498.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1118.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1122.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1370.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1562.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1182.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1186.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1626.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1246.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1250.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1438.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1442.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1062.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1085.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1149.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1213.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1405.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1025.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1029.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1276.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1468.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1088.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1092.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1340.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1532.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1152.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1156.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1596.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1216.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1220.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1408.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1412.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1032.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1275.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1467.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1087.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1091.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1339.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1531.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1151.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1155.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1595.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1215.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1219.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1407.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1411.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1031.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1658.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1278.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1282.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1470.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1474.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1094.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1722.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1342.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1346.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1534.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1538.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1158.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1598.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1602.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1222.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1414.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1101.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1165.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1229.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1421.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1041.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1045.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1292.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1484.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1104.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1108.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1356.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1548.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1168.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1172.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1612.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1232.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1236.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1424.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1428.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1048.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1291.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1483.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1103.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1107.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1355.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1547.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1167.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1171.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1611.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1231.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1235.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1423.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1427.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1047.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1674.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1294.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1298.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1486.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1490.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1110.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1738.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1358.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1362.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1550.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1554.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1174.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1614.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1618.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1238.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1430.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1261.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1453.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1073.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1077.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1325.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1517.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1137.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1141.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1581.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1201.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1205.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1393.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1397.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1017.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1644.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1264.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1268.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1456.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1460.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1080.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1708.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1328.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1332.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1520.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1524.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1144.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1584.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1588.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1208.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1400.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1643.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1263.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1267.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1455.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1459.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1079.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1707.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1327.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1331.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1519.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1523.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1143.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1583.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1587.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1207.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1399.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1646.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1650.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1270.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1462.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1710.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1714.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1334.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1526.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1590.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1052.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1051.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1114.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1178.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1242.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1434.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1054.hlsl"
        #elif defined(BORDER) && defined(CUTOUT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1058.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1021.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1084.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1148.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1212.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1404.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1024.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1028.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1083.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1147.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1211.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1403.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1023.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1027.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1274.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1466.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1086.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1090.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1338.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1530.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1150.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1154.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1594.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1214.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1218.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1406.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1410.hlsl"
        #elif defined(BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1030.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1037.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1100.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1164.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1228.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1420.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1040.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1044.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1099.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1163.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1227.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1419.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1039.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1043.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1290.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1482.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1102.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1106.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1354.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1546.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1166.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1170.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1610.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1230.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1234.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1422.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1426.hlsl"
        #elif defined(CUTOUT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1046.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1069.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1133.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1197.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1389.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1009.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1013.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1260.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1452.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1072.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1076.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1324.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1516.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1136.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1140.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1580.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1200.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1204.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1392.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1396.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1016.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1259.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1451.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1071.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1075.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1323.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1515.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1135.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1139.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1579.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1199.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1203.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1391.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1395.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1015.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1642.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1262.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1266.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1454.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1458.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1078.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1706.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1326.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1330.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1518.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1522.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1142.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1582.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1586.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1206.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1398.hlsl"
        #elif defined(BORDER) && defined(CUTOUT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1050.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1020.hlsl"
        #elif defined(BORDER) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1019.hlsl"
        #elif defined(BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1082.hlsl"
        #elif defined(BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1146.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1210.hlsl"
        #elif defined(BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1402.hlsl"
        #elif defined(BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1022.hlsl"
        #elif defined(BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1026.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1036.hlsl"
        #elif defined(CUTOUT) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1035.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1098.hlsl"
        #elif defined(CUTOUT) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1162.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1226.hlsl"
        #elif defined(CUTOUT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1418.hlsl"
        #elif defined(CUTOUT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1038.hlsl"
        #elif defined(CUTOUT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1042.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1005.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1068.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1132.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1196.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1388.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1008.hlsl"
        #elif defined(GRADIENT_RADIAL) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1012.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1067.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1131.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1195.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1387.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1007.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1011.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1258.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1450.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1070.hlsl"
        #elif defined(SHAPE_CIRCLE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1074.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1322.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1514.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1134.hlsl"
        #elif defined(SHAPE_ROUNDRECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1138.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1578.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1198.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1202.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1390.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1394.hlsl"
        #elif defined(USE_BORDER_TEXTURE) && defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1014.hlsl"
        #elif defined(BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1018.hlsl"
        #elif defined(CUTOUT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1034.hlsl"
        #elif defined(GRADIENT_RADIAL)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1004.hlsl"
        #elif defined(GRADIENT_RADIAL_BORDER)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1003.hlsl"
        #elif defined(SHAPE_CIRCLE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1066.hlsl"
        #elif defined(SHAPE_ROUNDRECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1130.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1194.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1386.hlsl"
        #elif defined(USE_BORDER_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1006.hlsl"
        #elif defined(USE_TEXTURE)
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1010.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Blend-Frame/p0_ps_1002.hlsl"
        #endif
        ENDHLSL
    }
}
}
