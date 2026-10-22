Shader "LCArt/TMPFont/Shader_DE_TMP_SDF_Lit"
{
Properties
{
    _FaceTex ("Face Texture", 2D) = "white" {}
    _FaceUVSpeedX ("Face UV Speed X", Range(-5, 5)) = 0
    _FaceUVSpeedY ("Face UV Speed Y", Range(-5, 5)) = 0
    [HDR] _FaceColor ("Face Color", Color) = (1, 1, 1, 1)
    _FaceDilate ("Face Dilate", Range(-1, 1)) = 0
    [HDR] _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
    _OutlineTex ("Outline Texture", 2D) = "white" {}
    _OutlineUVSpeedX ("Outline UV Speed X", Range(-5, 5)) = 0
    _OutlineUVSpeedY ("Outline UV Speed Y", Range(-5, 5)) = 0
    _OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
    _OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
    _Bevel ("Bevel", Range(0, 1)) = 0.5
    _BevelOffset ("Bevel Offset", Range(-0.5, 0.5)) = 0
    _BevelWidth ("Bevel Width", Range(-0.5, 0.5)) = 0
    _BevelClamp ("Bevel Clamp", Range(0, 1)) = 0
    _BevelRoundness ("Bevel Roundness", Range(0, 1)) = 0
    _LightAngle ("Light Angle", Range(0, 6.2831854820251465)) = 3.1415998935699463
    [HDR] _SpecularColor ("Specular", Color) = (1, 1, 1, 1)
    _SpecularPower ("Specular", Range(0, 4)) = 2
    _Reflectivity ("Reflectivity", Range(5, 15)) = 10
    _Diffuse ("Diffuse", Range(0, 1)) = 0.5
    _Ambient ("Ambient", Range(1, 0)) = 0.5
    _BumpMap ("Normal map", 2D) = "bump" {}
    _BumpOutline ("Bump Outline", Range(0, 1)) = 0
    _BumpFace ("Bump Face", Range(0, 1)) = 0
    _ReflectFaceColor ("Reflection Color", Color) = (0, 0, 0, 1)
    _ReflectOutlineColor ("Reflection Color", Color) = (0, 0, 0, 1)
    _Cube ("Reflection Cubemap", Cube) = "black" {}
    _EnvMatrixRotation ("Texture Rotation", Vector) = (0, 0, 0, 0)
    [HDR] _UnderlayColor ("Border Color", Color) = (0, 0, 0, 0.5)
    _UnderlayOffsetX ("Border OffsetX", Range(-1, 1)) = 0
    _UnderlayOffsetY ("Border OffsetY", Range(-1, 1)) = 0
    _UnderlayDilate ("Border Dilate", Range(-1, 1)) = 0
    _UnderlaySoftness ("Border Softness", Range(0, 1)) = 0
    [HDR] _GlowColor ("Color", Color) = (0, 1, 0, 0.5)
    _GlowOffset ("Offset", Range(-1, 1)) = 0
    _GlowInner ("Inner", Range(0, 1)) = 0.05000000074505806
    _GlowOuter ("Outer", Range(0, 1)) = 0.05000000074505806
    _GlowPower ("Falloff", Range(1, 0)) = 0.75
    _WeightNormal ("Weight Normal", Float) = 0
    _WeightBold ("Weight Bold", Float) = 0.5
    _ShaderFlags ("Flags", Float) = 0
    _ScaleRatioA ("Scale RatioA", Float) = 1
    _ScaleRatioB ("Scale RatioB", Float) = 1
    _ScaleRatioC ("Scale RatioC", Float) = 1
    _MainTex ("Font Atlas", 2D) = "white" {}
    _TextureWidth ("Texture Width", Float) = 512
    _TextureHeight ("Texture Height", Float) = 512
    _GradientScale ("Gradient Scale", Float) = 5
    _ScaleX ("Scale X", Float) = 1
    _ScaleY ("Scale Y", Float) = 1
    _PerspectiveFilter ("Perspective Correction", Range(0, 1)) = 0.875
    _Sharpness ("Sharpness", Range(-1, 1)) = 0
    _VertexOffsetX ("Vertex OffsetX", Float) = 0
    _VertexOffsetY ("Vertex OffsetY", Float) = 0
    _MaskCoord ("Mask Coordinates", Vector) = (0, 0, 32767, 32767)
    _ClipRect ("Clip Rect", Vector) = (-32767, -32767, 32767, 32767)
    _MaskSoftnessX ("Mask SoftnessX", Float) = 0
    _MaskSoftnessY ("Mask SoftnessY", Float) = 0
    _StencilComp ("Stencil Comparison", Float) = 8
    _Stencil ("Stencil ID", Float) = 0
    _StencilOp ("Stencil Operation", Float) = 0
    _StencilWriteMask ("Stencil Write Mask", Float) = 255
    _StencilReadMask ("Stencil Read Mask", Float) = 255
    _CullMode ("Cull Mode", Float) = 0
    _ColorMask ("Color Mask", Float) = 15
}
SubShader
{
    Tags { "IgnoreProjector" = "true" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
    Pass
    {
        Name "TextMeshPro SDF"
        Tags { "IgnoreProjector" = "true" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
        Blend One OneMinusSrcAlpha
        ColorMask [_ColorMask]
        Cull [_CullMode]
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
        #pragma multi_compile_local _ UNDERLAY_ON
        #pragma multi_compile _ UNITY_UI_ALPHACLIP
        #pragma multi_compile _ UNITY_UI_CLIP_RECT
        #pragma multi_compile _ _ADDITIONAL_LIGHTS
        #pragma multi_compile _ _CLUSTER_LIGHT_LOOP

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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float4 _MainLightPosition;
        float4 _MainLightColor;
        float4 _AdditionalLightsCount;
        float4 _Time;
        float4 _FPParams0;
        float4 _FPParams1;
        float4 _FPParams2;
        float3 _WorldSpaceCameraPos;
        float4 _ScreenParams;
        float4 unity_OrthoParams;
        float4 _ScaleBiasRt;
        float4x4 glstate_matrix_projection;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        cbuffer AdditionalLights
        {
            float4 _AdditionalLightsPosition[256] : packoffset(c0);
            float4 _AdditionalLightsColor[256] : packoffset(c256);
            float4 _AdditionalLightsAttenuation[256] : packoffset(c512);
            float4 _AdditionalLightsSpotDir[256] : packoffset(c768);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4 unity_LightData : packoffset(c11);
            float4 unity_LightIndices[2] : packoffset(c12);
        }
        cbuffer UnityPerMaterial
        {
            float _FaceUVSpeedX : packoffset(c0);
            float _FaceUVSpeedY : packoffset(c0.y);
            float4 _FaceColor : packoffset(c1);
            float _FaceDilate : packoffset(c2);
            float _OutlineSoftness : packoffset(c2.y);
            float _OutlineUVSpeedX : packoffset(c2.z);
            float _OutlineUVSpeedY : packoffset(c2.w);
            float4 _OutlineColor : packoffset(c3);
            float _OutlineWidth : packoffset(c4);
            float4x4 _EnvMatrix : packoffset(c9);
            float4 _UnderlayColor : packoffset(c16);
            float _UnderlayOffsetX : packoffset(c17);
            float _UnderlayOffsetY : packoffset(c17.y);
            float _UnderlayDilate : packoffset(c17.z);
            float _UnderlaySoftness : packoffset(c17.w);
            float _WeightNormal : packoffset(c20.y);
            float _WeightBold : packoffset(c20.z);
            float _ScaleRatioA : packoffset(c20.w);
            float _ScaleRatioC : packoffset(c21.y);
            float _VertexOffsetX : packoffset(c21.z);
            float _VertexOffsetY : packoffset(c21.w);
            float4 _ClipRect : packoffset(c23);
            float _MaskSoftnessX : packoffset(c24);
            float _MaskSoftnessY : packoffset(c24.y);
            float _TextureWidth : packoffset(c24.z);
            float _TextureHeight : packoffset(c24.w);
            float _GradientScale : packoffset(c25);
            float _ScaleX : packoffset(c25.y);
            float _ScaleY : packoffset(c25.z);
            float _PerspectiveFilter : packoffset(c25.w);
            float _Sharpness : packoffset(c26);
            float4 _FaceTex_ST : packoffset(c27);
            float4 _OutlineTex_ST : packoffset(c28);
        }
        cbuffer urp_TileBuffer
        {
            float4 urp_Tiles[4096] : packoffset(c0);
        }
        cbuffer urp_ZBinBuffer
        {
            float4 urp_ZBins[1024] : packoffset(c0);
        }

        Texture2D<float4> _FaceTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _OutlineTex;
        SamplerState sampler_FaceTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_OutlineTex;

        // ---- progVertex
        #if defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_4.hlsl"
        #elif defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_19.hlsl"
        #elif defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_8.hlsl"
        #elif defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_15.hlsl"
        #elif defined(UNDERLAY_ON) && defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_6.hlsl"
        #elif defined(UNDERLAY_ON) && defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_17.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_5.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_18.hlsl"
        #elif defined(UNDERLAY_ON) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_10.hlsl"
        #elif defined(UNDERLAY_ON) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_13.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_9.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_14.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_7.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_16.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_11.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_12.hlsl"
        #else
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_vs_12.hlsl"
        #endif
        // ---- progFragment
        #if defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_24.hlsl"
        #elif defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_39.hlsl"
        #elif defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_28.hlsl"
        #elif defined(UNDERLAY_ON) && defined(UNITY_UI_ALPHACLIP) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_35.hlsl"
        #elif defined(UNDERLAY_ON) && defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_26.hlsl"
        #elif defined(UNDERLAY_ON) && defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_37.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_25.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_38.hlsl"
        #elif defined(UNDERLAY_ON) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_30.hlsl"
        #elif defined(UNDERLAY_ON) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_33.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_29.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_34.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_27.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_36.hlsl"
        #elif defined(_ADDITIONAL_LIGHTS)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_31.hlsl"
        #elif defined(_CLUSTER_LIGHT_LOOP)
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_32.hlsl"
        #else
        #include "LCArt_TMPFont_Shader_DE_TMP_SDF_Lit/p0_ps_32.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Universal Render Pipeline/FallbackError"
CustomEditor "TMPro.EditorUtilities.TMP_SDFShaderGUI"
}
