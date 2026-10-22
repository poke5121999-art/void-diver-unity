Shader "ArtTeam/UI/Shader_DE_UI_VFXMaster"
{
Properties
{
    _Color ("Tint", Color) = (1, 1, 1, 1)
    _StencilComp ("Stencil Comparison", Float) = 8
    _Stencil ("Stencil ID", Float) = 0
    _StencilOp ("Stencil Operation", Float) = 0
    _StencilWriteMask ("Stencil Write Mask", Float) = 255
    _StencilReadMask ("Stencil Read Mask", Float) = 255
    _ColorMask ("Color Mask", Float) = 15
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    [HideInInspector] _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
    [HideInInspector] _AlphaCutoff ("Alpha Cutoff ", Range(0, 1)) = 0.5
    [Header(________________________)] [Enum(Additive,1,AlphaBlend,10)] [Space (8)] _BlendMode ("BlendMode", Float) = 10
    [Enum(Default,0,UIRect9Slice,1)] _UVType ("UVType", Float) = 0
    _AlphaClip ("AlphaClip", Range(0, 1)) = 0
    [PerRendererData] _MainTex ("_MainTex", 2D) = "white" {}
    [HDR] _MainColor ("MainColor", Color) = (1, 1, 1, 1)
    [Enum(A,0,R,1)] _Main_Alpha_Ch ("Main_Alpha_Ch", Float) = 0
    _MainTex_Contrast ("MainTex_Contrast", Float) = 1
    _Main_Alpha_Contrast ("Main_Alpha_Contrast", Float) = 1
    [ToggleUI] _DissolveAlphaControll ("DissolveAlphaControll", Float) = 0
    _Main_TileOffset ("Main_Tile/Offset", Vector) = (1, 1, 0, 0)
    _Main_Rotate ("Main_Rotate", Float) = 0
    [ToggleUI] _Main_Radial ("Main_Radial", Float) = 1
    _Main_Radial_Tilling ("Main_Radial_Tilling", Vector) = (1, 1, 0, 0)
    [ToggleUI] _Use_Main_Panning ("Use_Main_Panning", Float) = 1
    _Main_Pan_Speed ("Main_Pan_Speed", Vector) = (0, 0, 0, 0)
    [Header (________Dissolve________)] [Space (8)] [Toggle( _USE_DISSOLVE_ON )] _Use_Dissolve ("Use_Dissolve", Float) = 1
    _DissolveTex ("DissolveTex", 2D) = "white" {}
    [ToggleUI] _Dissolve_ReverseTex ("Dissolve_ReverseTex", Float) = 0
    [ToggleUI] _Dissolve_Radial ("Dissolve_Radial", Float) = 0
    _Dissolve_Radial_Tilling ("Dissolve_Radial_Tilling", Vector) = (1, 1, 0, 0)
    _Dissolve_Contrast ("Dissolve_Contrast", Float) = 1
    _Dissolve_Rotate ("Dissolve_Rotate", Float) = 0
    _Dissolve_Alpha ("Dissolve_Alpha", Range(0, 1)) = 1
    _Dissolve_Smooth ("Dissolve_Smooth", Range(0, 1)) = 0
    _Dissolve_Pan_Speed ("Dissolve_Pan_Speed", Vector) = (0, 0, 0, 0)
    [Header (________Deform________)] [Space (8)] [Toggle( _USE_DEFORM_ON )] _Use_Deform ("Use_Deform", Float) = 0
    _DeformTex ("DeformTex", 2D) = "bump" {}
    [ToggleUI] _Deform_Radial ("Deform_Radial", Float) = 0
    _Deform_Radial_Tilling ("Deform_Radial_Tilling", Vector) = (1, 1, 0, 0)
    _Deform_Rotate ("Deform_Rotate", Float) = 0
    _Deform_Strength ("Deform_Strength", Float) = 0
    _Deform_Pan_Speed ("Deform_Pan_Speed", Vector) = (0, 0, 0, 0)
    [Enum(Linear,0,Beam,1,Radial,2)] [Space (12)] _DeformMask_Type ("DeformMask_Type", Float) = 0
    _Deform_Mask_OffsetStrength ("Deform_Mask_Offset/Strength", Vector) = (0, 0, 0, 0)
    _Deform_Mask_Smooth ("Deform_Mask_Smooth", Range(0, 1)) = 0
    _Deform_Mask_Rotate ("Deform_Mask_Rotate", Float) = 0
    [Header (_________Mask________)] [Space (8)] [Toggle( _USE_MASK_ON )] _Use_Mask ("Use_Mask", Float) = 0
    _Mask_Tex ("Mask_Tex", 2D) = "white" {}
    _Mask_Rotate ("Mask_Rotate", Float) = 0
    _Mask_Contrast ("Mask_Contrast", Float) = 1
    _Mask_Smooth ("Mask_Smooth", Range(0, 1)) = 0
    _Mask_White ("Mask_White", Range(0, 1)) = 0
    [HideInInspector] _UIRect ("UIRect", Vector) = (0, 0, 0, 0)
    [HideInInspector] _OriginalSize ("OriginalSize", Vector) = (0, 0, 0, 0)
}
SubShader
{
    Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
    Pass
    {
        Name "Default"
        Tags { "CanUseSpriteAtlas" = "true" "IgnoreProjector" = "true" "PreviewType" = "Plane" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
        Blend SrcAlpha [_BlendMode]
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
        #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
        #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
        #pragma multi_compile_local _ _USE_DEFORM_ON
        #pragma multi_compile_local _ _USE_DISSOLVE_ON
        #pragma multi_compile_local _ _USE_MASK_ON

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
        float4 _Time;
        float4 _ScreenParams;
        float4x4 glstate_matrix_projection;
        float4x4 unity_MatrixVP;
        float4 _ClipRect;
        float _UIMaskSoftnessX;
        float _UIMaskSoftnessY;
        int _UIVertexColorAlwaysGammaSpace;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float4 _DeformTex_ST : packoffset(c0);
            float4 _MainColor : packoffset(c1);
            float4 _DissolveTex_ST : packoffset(c2);
            float4 _Main_TileOffset : packoffset(c3);
            float4 _UIRect : packoffset(c4);
            float4 _Mask_Tex_ST : packoffset(c5);
            float2 _Dissolve_Pan_Speed : packoffset(c6);
            float2 _Dissolve_Radial_Tilling : packoffset(c6.z);
            float2 _Deform_Radial_Tilling : packoffset(c7);
            float2 _OriginalSize : packoffset(c7.z);
            float2 _Deform_Mask_OffsetStrength : packoffset(c8);
            float2 _Deform_Pan_Speed : packoffset(c8.z);
            float2 _Main_Pan_Speed : packoffset(c9);
            float2 _Main_Radial_Tilling : packoffset(c9.z);
            float _Dissolve_Smooth : packoffset(c10);
            float _Mask_Rotate : packoffset(c10.y);
            float _Dissolve_Contrast : packoffset(c10.z);
            float _Dissolve_Rotate : packoffset(c10.w);
            float _Mask_Contrast : packoffset(c11);
            float _Dissolve_Radial : packoffset(c11.y);
            float _Mask_Smooth : packoffset(c11.z);
            float _Dissolve_ReverseTex : packoffset(c11.w);
            float _Dissolve_Alpha : packoffset(c12);
            float _Main_Alpha_Contrast : packoffset(c12.y);
            float _DissolveAlphaControll : packoffset(c12.w);
            float _MainTex_Contrast : packoffset(c13);
            float _Deform_Mask_Rotate : packoffset(c13.y);
            float _Mask_White : packoffset(c13.z);
            int _DeformMask_Type : packoffset(c13.w);
            float _Deform_Mask_Smooth : packoffset(c14);
            float _Deform_Strength : packoffset(c14.y);
            float _Deform_Rotate : packoffset(c14.z);
            float _UVType : packoffset(c14.w);
            float _Deform_Radial : packoffset(c15);
            float _Main_Rotate : packoffset(c15.y);
            float _Main_Radial : packoffset(c15.z);
            float _Use_Main_Panning : packoffset(c15.w);
            float _Main_Alpha_Ch : packoffset(c16);
            float _AlphaClip : packoffset(c16.y);
            float4 _Color : packoffset(c17);
        }

        Texture2D<float4> _DeformTex;
        Texture2D<float4> _DissolveTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _Mask_Tex;
        SamplerState sampler_DeformTex;
        SamplerState sampler_DissolveTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_Mask_Tex;

        // ---- progVertex
        #if defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_28.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_25.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_27.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_14.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_21.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_23.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_24.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_26.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_11.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_13.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_18.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_20.hlsl"
        #elif defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_7.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_22.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_DEFORM_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_9.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_10.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_12.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_16.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_17.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_19.hlsl"
        #elif defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_4.hlsl"
        #elif defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_6.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_8.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_15.hlsl"
        #elif defined(_USE_DEFORM_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_2.hlsl"
        #elif defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_3.hlsl"
        #elif defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_5.hlsl"
        #elif 1
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_70.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_67.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_69.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_56.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_63.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_65.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_66.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_68.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_53.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_55.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_60.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_62.hlsl"
        #elif defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_49.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(UNITY_UI_CLIP_RECT)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_64.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_DEFORM_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_51.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_52.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_54.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_DEFORM_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_58.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_59.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_61.hlsl"
        #elif defined(_USE_DEFORM_ON) && defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_46.hlsl"
        #elif defined(_USE_DISSOLVE_ON) && defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_48.hlsl"
        #elif defined(UNITY_UI_ALPHACLIP)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_50.hlsl"
        #elif defined(UNITY_UI_CLIP_RECT)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_57.hlsl"
        #elif defined(_USE_DEFORM_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_44.hlsl"
        #elif defined(_USE_DISSOLVE_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_45.hlsl"
        #elif defined(_USE_MASK_ON)
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_47.hlsl"
        #elif 1
        #include "ArtTeam_UI_Shader_DE_UI_VFXMaster/p0_ps_43.hlsl"
        #endif
        ENDHLSL
    }
}
CustomEditor "AmplifyShaderEditor.MaterialInspector"
}
