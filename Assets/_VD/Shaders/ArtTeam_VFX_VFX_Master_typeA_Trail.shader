Shader "ArtTeam/VFX/VFX_Master_typeA_Trail"
{
Properties
{
    _AlphaThreshold ("AlphaThreshold", Float) = 0.5
    _isColor ("isColor", Range(0, 1)) = 1
    _Emmisive ("Emmisive", Float) = 1
    _MainColor ("MainColor", Color) = (1, 1, 1, 1)
    _MainTex ("MainTex", 2D) = "white" {}
    _MainTexFlowSpeed ("MainTexFlowSpeed", Float) = 0
    _Dissolve ("Dissolve", Range(0, 1)) = 1
    _DissolveTex ("DissolveTex", 2D) = "white" {}
    _DissovePower ("DissovePower", Float) = 1
    _DeformTex ("DeformTex", 2D) = "white" {}
    _MaskTex ("MaskTex", 2D) = "white" {}
    _DeformStrength ("DeformStrength", Float) = 0
    _DeformSpeed ("DeformSpeed", Float) = 0
    _DeformDirection ("DeformDirection", Vector) = (0, 0, 0, 0)
    _ScreenAlphaStrength ("ScreenAlphaStrength", Range(0, 1)) = 0
    _ScreenAlphaSmoothness ("ScreenAlphaSmoothness", Range(0, 1)) = 0.5
    _ScreenAlphaDissolve ("ScreenAlphaDissolve", Range(0, 1)) = 0.5
    _FresnelMask_Threshold ("FresnelMask_Threshold", Range(0, 1)) = 1
    _FresnelMask_Offset ("FresnelMask_Offset", Range(0, 1)) = 0
    [HideInInspector] _CastShadows ("_CastShadows", Float) = 0
    [HideInInspector] _Surface ("_Surface", Float) = 1
    [HideInInspector] _Blend ("_Blend", Float) = 0
    [HideInInspector] _AlphaClip ("_AlphaClip", Float) = 0
    [HideInInspector] _SrcBlend ("_SrcBlend", Float) = 1
    [HideInInspector] _DstBlend ("_DstBlend", Float) = 0
    [HideInInspector] _SrcBlendAlpha ("_SrcBlendAlpha", Float) = 1
    [HideInInspector] _DstBlendAlpha ("_DstBlendAlpha", Float) = 0
    [HideInInspector] [ToggleUI] _ZWrite ("_ZWrite", Float) = 0
    [HideInInspector] _ZWriteControl ("_ZWriteControl", Float) = 0
    [HideInInspector] _ZTest ("_ZTest", Float) = 4
    [HideInInspector] _Cull ("_Cull", Float) = 0
    [HideInInspector] _AlphaToMask ("_AlphaToMask", Float) = 0
    [HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
    [HideInInspector] _QueueControl ("_QueueControl", Float) = -1
    [HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
}
SubShader
{
    Tags { "DisableBatching" = "False" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalUnlitSubTarget" "UniversalMaterialType" = "Unlit" }
    Pass
    {
        Name "Universal Forward"
        Tags { "DisableBatching" = "False" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalUnlitSubTarget" "UniversalMaterialType" = "Unlit" }
        Blend [_SrcBlend] [_DstBlend], [_SrcBlendAlpha] [_DstBlendAlpha]
        AlphaToMask [_AlphaToMask]
        ZTest [_ZTest]
        ZWrite [_ZWrite]
        Cull [_Cull]
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ FOG_LINEAR
        #pragma multi_compile_local _ _ALPHATEST_ON
        #pragma multi_compile _ _DBUFFER_MRT3
        #pragma multi_compile _ _SURFACE_TYPE_TRANSPARENT

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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float _AlphaToMaskAvailable;
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 unity_OrthoParams;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c1);
            float4 _DeformTex_ST : packoffset(c3);
            float4 _MaskTex_ST : packoffset(c5);
            float2 _DeformDirection : packoffset(c6);
            float4 _MainColor : packoffset(c7);
            float4 _DissolveTex_ST : packoffset(c9);
            float _DissovePower : packoffset(c10);
            float _isColor : packoffset(c10.y);
            float _ScreenAlphaStrength : packoffset(c10.z);
            float _ScreenAlphaSmoothness : packoffset(c10.w);
            float _ScreenAlphaDissolve : packoffset(c11);
            float _FresnelMask_Threshold : packoffset(c11.y);
            float _FresnelMask_Offset : packoffset(c11.z);
            float _AlphaThreshold : packoffset(c11.w);
            float _Emmisive : packoffset(c12);
            float _DeformSpeed : packoffset(c12.y);
            float _DeformStrength : packoffset(c12.z);
            float _Dissolve : packoffset(c12.w);
            float _MainTexFlowSpeed : packoffset(c13);
        }

        Texture2D<float4> _DBufferTexture0;
        Texture2D<float4> _DeformTex;
        Texture2D<float4> _DissolveTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        SamplerState sampler_DeformTex;
        SamplerState sampler_DissolveTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;

        // ---- progVertex
        #if defined(FOG_LINEAR)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_vs_2.hlsl"
        #elif 1
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_28.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_27.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_20.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_26.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_24.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_19.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_25.hlsl"
        #elif defined(FOG_LINEAR) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_18.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_23.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_16.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_22.hlsl"
        #elif defined(FOG_LINEAR)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_17.hlsl"
        #elif defined(_ALPHATEST_ON)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_15.hlsl"
        #elif defined(_DBUFFER_MRT3)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_21.hlsl"
        #elif defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_14.hlsl"
        #elif 1
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p0_ps_13.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DepthOnly"
        Tags { "DisableBatching" = "False" "LightMode" = "DepthOnly" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalUnlitSubTarget" "UniversalMaterialType" = "Unlit" }
        ColorMask R
        Cull [_Cull]
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local _ _ALPHATEST_ON

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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c1);
            float4 _DeformTex_ST : packoffset(c3);
            float4 _MaskTex_ST : packoffset(c5);
            float2 _DeformDirection : packoffset(c6);
            float4 _DissolveTex_ST : packoffset(c9);
            float _DissovePower : packoffset(c10);
            float _ScreenAlphaStrength : packoffset(c10.z);
            float _ScreenAlphaSmoothness : packoffset(c10.w);
            float _ScreenAlphaDissolve : packoffset(c11);
            float _FresnelMask_Threshold : packoffset(c11.y);
            float _FresnelMask_Offset : packoffset(c11.z);
            float _AlphaThreshold : packoffset(c11.w);
            float _DeformSpeed : packoffset(c12.y);
            float _DeformStrength : packoffset(c12.z);
            float _Dissolve : packoffset(c12.w);
            float _MainTexFlowSpeed : packoffset(c13);
        }

        Texture2D<float4> _DeformTex;
        Texture2D<float4> _DissolveTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        SamplerState sampler_DeformTex;
        SamplerState sampler_DissolveTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;

        // ---- progVertex
        #if 1
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p1_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p1_ps_32.hlsl"
        #elif 1
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p1_ps_31.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "MotionVectors"
        Tags { "DisableBatching" = "False" "LightMode" = "MOTIONVECTORS" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalUnlitSubTarget" "UniversalMaterialType" = "Unlit" }
        ColorMask RG
        Cull [_Cull]
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local _ _ALPHATEST_ON

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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 _PrevViewProjMatrix;
        float4x4 _NonJitteredViewProjMatrix;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4x4 unity_MatrixPreviousM : packoffset(c36);
            float4 unity_MotionVectorsParams : packoffset(c44);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c1);
            float4 _DeformTex_ST : packoffset(c3);
            float4 _MaskTex_ST : packoffset(c5);
            float2 _DeformDirection : packoffset(c6);
            float4 _DissolveTex_ST : packoffset(c9);
            float _DissovePower : packoffset(c10);
            float _ScreenAlphaStrength : packoffset(c10.z);
            float _ScreenAlphaSmoothness : packoffset(c10.w);
            float _ScreenAlphaDissolve : packoffset(c11);
            float _FresnelMask_Threshold : packoffset(c11.y);
            float _FresnelMask_Offset : packoffset(c11.z);
            float _AlphaThreshold : packoffset(c11.w);
            float _DeformSpeed : packoffset(c12.y);
            float _DeformStrength : packoffset(c12.z);
            float _Dissolve : packoffset(c12.w);
            float _MainTexFlowSpeed : packoffset(c13);
        }

        Texture2D<float4> _DeformTex;
        Texture2D<float4> _DissolveTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        SamplerState sampler_DeformTex;
        SamplerState sampler_DissolveTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;

        // ---- progVertex
        #if 1
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p2_vs_34.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p2_ps_38.hlsl"
        #elif 1
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p2_ps_37.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "DepthNormalsOnly"
        Tags { "DisableBatching" = "False" "LightMode" = "DepthNormalsOnly" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalUnlitSubTarget" "UniversalMaterialType" = "Unlit" }
        Cull [_Cull]
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local _ _ALPHATEST_ON
        #pragma multi_compile _ _SURFACE_TYPE_TRANSPARENT

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
        float4 _ScaledScreenParams;
        float2 _GlobalMipBias;
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c1);
            float4 _DeformTex_ST : packoffset(c3);
            float4 _MaskTex_ST : packoffset(c5);
            float2 _DeformDirection : packoffset(c6);
            float4 _DissolveTex_ST : packoffset(c9);
            float _DissovePower : packoffset(c10);
            float _ScreenAlphaStrength : packoffset(c10.z);
            float _ScreenAlphaSmoothness : packoffset(c10.w);
            float _ScreenAlphaDissolve : packoffset(c11);
            float _FresnelMask_Threshold : packoffset(c11.y);
            float _FresnelMask_Offset : packoffset(c11.z);
            float _AlphaThreshold : packoffset(c11.w);
            float _DeformSpeed : packoffset(c12.y);
            float _DeformStrength : packoffset(c12.z);
            float _Dissolve : packoffset(c12.w);
            float _MainTexFlowSpeed : packoffset(c13);
        }

        Texture2D<float4> _DeformTex;
        Texture2D<float4> _DissolveTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        SamplerState sampler_DeformTex;
        SamplerState sampler_DissolveTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;

        // ---- progVertex
        #if 1
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p3_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p3_ps_42.hlsl"
        #elif defined(_ALPHATEST_ON)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p3_ps_41.hlsl"
        #elif defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p3_ps_40.hlsl"
        #elif 1
        #include "ArtTeam_VFX_VFX_Master_typeA_Trail/p3_ps_39.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}
