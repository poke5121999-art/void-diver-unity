Shader "ArtTeam/VFX/VFX_Master_typeB_Xpan"
{
Properties
{
    _MainTex ("MainTex", 2D) = "white" {}
    _Pixelation ("Pixelation", Float) = 1
    _MaskTex ("MaskTex", 2D) = "white" {}
    _Depth ("Depth", Float) = 1
    _DissolveSharpness ("DissolveSharpness", Float) = 1
    _InvertB ("InvertB", Range(0, 1)) = 0
    [HideInInspector] _CastShadows ("_CastShadows", Float) = 1
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
    [HideInInspector] _Cull ("_Cull", Float) = 2
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
        float _AlphaToMaskAvailable;
        float4 _ScreenParams;
        float4x4 unity_MatrixVP;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c1);
            float _DissolveSharpness : packoffset(c2.y);
            float4 _MaskTex_ST : packoffset(c4);
            float _InvertB : packoffset(c5);
            float _Pixelation : packoffset(c5.y);
        }

        Texture2D<float4> _DBufferTexture0;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;

        // ---- progVertex
        #if defined(FOG_LINEAR)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_vs_2.hlsl"
        #elif 1
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_23.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_22.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_15.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_21.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_19.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_14.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_20.hlsl"
        #elif defined(FOG_LINEAR) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_13.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_18.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_11.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_17.hlsl"
        #elif defined(FOG_LINEAR)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_12.hlsl"
        #elif defined(_ALPHATEST_ON)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_10.hlsl"
        #elif defined(_DBUFFER_MRT3)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_16.hlsl"
        #elif defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_9.hlsl"
        #elif 1
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p0_ps_8.hlsl"
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
        float4 _ScreenParams;
        float4x4 unity_MatrixVP;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c1);
            float _DissolveSharpness : packoffset(c2.y);
            float4 _MaskTex_ST : packoffset(c4);
            float _Pixelation : packoffset(c5.y);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;

        // ---- progVertex
        #if 1
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p1_vs_24.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p1_ps_28.hlsl"
        #elif 1
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p1_ps_27.hlsl"
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
        float4 _ScreenParams;
        float4x4 unity_MatrixVP;
        float4x4 _PrevViewProjMatrix;
        float4x4 _NonJitteredViewProjMatrix;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_MatrixPreviousM : packoffset(c36);
            float4 unity_MotionVectorsParams : packoffset(c44);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c1);
            float _DissolveSharpness : packoffset(c2.y);
            float4 _MaskTex_ST : packoffset(c4);
            float _Pixelation : packoffset(c5.y);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;

        // ---- progVertex
        #if 1
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p2_vs_30.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p2_ps_34.hlsl"
        #elif 1
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p2_ps_33.hlsl"
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
        float4 _ScreenParams;
        float4x4 unity_MatrixVP;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c1);
            float _DissolveSharpness : packoffset(c2.y);
            float4 _MaskTex_ST : packoffset(c4);
            float _Pixelation : packoffset(c5.y);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;

        // ---- progVertex
        #if 1
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p3_vs_35.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p3_ps_39.hlsl"
        #elif defined(_ALPHATEST_ON)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p3_ps_38.hlsl"
        #elif defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p3_ps_37.hlsl"
        #elif 1
        #include "ArtTeam_VFX_VFX_Master_typeB_Xpan/p3_ps_36.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}
