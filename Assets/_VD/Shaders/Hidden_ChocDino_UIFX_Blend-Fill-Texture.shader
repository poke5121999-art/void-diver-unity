Shader "Hidden/ChocDino/UIFX/Blend-Fill-Texture"
{
Properties
{
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    [PerRendererData] _ResultTex ("Sprite Texture", 2D) = "white" {}
    [PerRendererData] _FillTex ("Fill Texture", 2D) = "clear" {}
    _StencilComp ("Stencil Comparison", Float) = 8
    _Stencil ("Stencil ID", Float) = 0
    _StencilOp ("Stencil Operation", Float) = 0
    _StencilWriteMask ("Stencil Write Mask", Float) = 255
    _StencilReadMask ("Stencil Read Mask", Float) = 255
    _Color ("Color", Color) = (1, 1, 1, 1)
    _Strength ("Strength", Float) = 1
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    _ColorMask ("Color Mask", Float) = 15
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "OutputsPremultipliedAlpha" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderType" = "Transparent" }
    Pass
    {
        Name "Blend-Fill-Texture"
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
        #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
        #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
        #pragma multi_compile_local _ WRAP_CLAMP WRAP_MIRROR WRAP_REPEAT

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
        float4x4 _FillTex_Matrix;
        float4 _ClipRect;
        float _UIMaskSoftnessX;
        float _UIMaskSoftnessY;
        float4 _Color;
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

        Texture2D<float4> _FillTex;
        Texture2D<float4> _ResultTex;
        SamplerState sampler_FillTex;
        SamplerState sampler_ResultTex;
        SamplerState sampler_vd_linear_clamp;
        SamplerState sampler_vd_linear_mirror;
        SamplerState sampler_vd_linear_repeat;

        // ---- progVertex
        #if defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_94.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_108.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_101.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_99.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_113.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_106.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_97.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_111.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_104.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_96.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_110.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_103.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_95.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_109.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_102.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_98.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_112.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_105.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_87.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_38.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_52.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_45.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_66.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_80.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_73.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_92.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_43.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_57.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_50.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_71.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_85.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_78.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_90.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_41.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_55.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_48.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_69.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_83.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_76.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_89.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_40.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_54.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_47.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_68.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_82.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_75.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_88.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_39.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_53.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_46.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_67.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_81.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_74.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_91.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_42.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_56.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_49.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_70.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_84.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_77.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_93.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_107.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_100.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_31.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_59.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_10.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_24.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_17.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_36.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_64.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_15.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_29.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_22.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_34.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_62.hlsl"
        #elif defined(BLEND_DARKEN) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_13.hlsl"
        #elif defined(BLEND_DARKEN) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_27.hlsl"
        #elif defined(BLEND_DARKEN) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_20.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_33.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_61.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_12.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_26.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_19.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_32.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_60.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_11.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_25.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_18.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_35.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_63.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_14.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_28.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_21.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_86.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_37.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_51.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_44.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_65.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_79.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_72.hlsl"
        #elif defined(BLEND_ALPHABLEND)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_3.hlsl"
        #elif defined(BLEND_BACKGROUND)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_8.hlsl"
        #elif defined(BLEND_DARKEN)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_6.hlsl"
        #elif defined(BLEND_LIGHTEN)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_5.hlsl"
        #elif defined(BLEND_MULTIPLY)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_4.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_7.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_30.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_58.hlsl"
        #elif defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_9.hlsl"
        #elif defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_23.hlsl"
        #elif defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_16.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_vs_2.hlsl"
        #endif
        // ---- progFragment
        #if defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_214.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_228.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_221.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_219.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_233.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_226.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_217.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_231.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_224.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_216.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_230.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_223.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_215.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_229.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_222.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_218.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_232.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_225.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_207.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_158.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_172.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_165.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_186.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_200.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_193.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_212.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_163.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_177.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_170.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_191.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_205.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_198.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_210.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_161.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_175.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_168.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_189.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_203.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_196.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_209.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_160.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_174.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_167.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_188.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_202.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_195.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_208.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_159.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_173.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_166.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_187.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_201.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_194.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_211.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_162.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_176.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_169.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_190.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_204.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_197.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_213.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_227.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_220.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_151.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_179.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_130.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_144.hlsl"
        #elif defined(BLEND_ALPHABLEND) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_137.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_156.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_184.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_135.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_149.hlsl"
        #elif defined(BLEND_BACKGROUND) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_142.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_154.hlsl"
        #elif defined(BLEND_DARKEN) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_182.hlsl"
        #elif defined(BLEND_DARKEN) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_133.hlsl"
        #elif defined(BLEND_DARKEN) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_147.hlsl"
        #elif defined(BLEND_DARKEN) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_140.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_153.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_181.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_132.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_146.hlsl"
        #elif defined(BLEND_LIGHTEN) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_139.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_152.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_180.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_131.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_145.hlsl"
        #elif defined(BLEND_MULTIPLY) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_138.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_155.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_183.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_134.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_148.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_141.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_206.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_157.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_171.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_164.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_185.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_199.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_192.hlsl"
        #elif defined(BLEND_ALPHABLEND)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_123.hlsl"
        #elif defined(BLEND_BACKGROUND)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_128.hlsl"
        #elif defined(BLEND_DARKEN)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_126.hlsl"
        #elif defined(BLEND_LIGHTEN)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_125.hlsl"
        #elif defined(BLEND_MULTIPLY)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_124.hlsl"
        #elif defined(BLEND_REPLACE_ALPHA)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_127.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_150.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_178.hlsl"
        #elif defined(WRAP_CLAMP)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_129.hlsl"
        #elif defined(WRAP_MIRROR)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_143.hlsl"
        #elif defined(WRAP_REPEAT)
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_136.hlsl"
        #elif 1
        #include "Hidden_ChocDino_UIFX_Blend-Fill-Texture/p0_ps_122.hlsl"
        #endif
        ENDHLSL
    }
}
}
