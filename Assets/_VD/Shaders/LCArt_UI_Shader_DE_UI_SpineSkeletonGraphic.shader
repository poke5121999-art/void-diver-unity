Shader "LCArt/UI/Shader_DE_UI_SpineSkeletonGraphic"
{
Properties
{
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1, 1, 1, 1)
    [Header (Spine Setting)] [Space (10)] [Toggle] _PMAVertexColorInput ("PMA Vertex Color Input", Float) = 1
    [Toggle] _IsVertexColorAlphaUsed ("Use Vertex Color Alpha", Float) = 1
    [Toggle] _UseDarkColorMask ("Use Spine Tint Black", Float) = 0
    [Header (Mask Setting)] [Space (10)] _MaskTex ("Mask Texture", 2D) = "black" {}
    _MaskColor0 ("Mask Color 0", Color) = (1, 0, 0, 1)
    _MaskColor1 ("Mask Color 1", Color) = (0, 1, 0, 1)
    _MaskIntensity ("Mask Intensity", Range(0, 1)) = 0
    [HDR] [Header (Hit Effect Setting)] [Space (10)] _EffectColor ("Effect Color", Color) = (1, 1, 1, 1)
    _Effect ("Effect", Range(0, 1)) = 0
    [Header (Dissolve Setting)] [Space (10)] _DissolveProgress ("Dissolve Progress", Range(0, 1)) = 0
    _DissolveColor ("Dissolve Color", Color) = (0, 0, 0, 1)
    [HideInInspector] [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Float) = 8
    [HideInInspector] _Stencil ("Stencil ID", Float) = 0
    [HideInInspector] [Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", Float) = 0
    [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
    [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
    [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
    Pass
    {
        Name "Normal"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
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
        float4x4 unity_MatrixVP;
        float4 _TextureSampleAdd;
        float4 _ClipRect;
        float _UIVertexColorAlwaysGammaSpace;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c0);
            float4 _Color : packoffset(c1);
            float _PMAVertexColorInput : packoffset(c4.y);
            float _IsVertexColorAlphaUsed : packoffset(c4.z);
            float _UseDarkColorMask : packoffset(c4.w);
        }

        Texture2D<float4> _MainTex;
        SamplerState sampler_MainTex;

        // ---- progVertex
        #if 1
        #include "LCArt_UI_Shader_DE_UI_SpineSkeletonGraphic/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_UI_Shader_DE_UI_SpineSkeletonGraphic/p0_ps_3.hlsl"
        #endif
        ENDHLSL
    }
}
}
