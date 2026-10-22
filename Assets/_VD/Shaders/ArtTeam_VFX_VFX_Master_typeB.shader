Shader "ArtTeam/VFX/VFX_Master_typeB"
{
Properties
{
    _MainTex ("MainTex", 2D) = "white" {}
    _Pixelation ("Pixelation", Float) = 1
    _invertB ("invertB", Range(0, 1)) = 0
    _MaskTex ("MaskTex", 2D) = "white" {}
    _AlphaClipThreshold ("AlphaClipThreshold", Range(0, 1)) = 0.5
    _FresnelPower ("FresnelPower", Float) = 2
    _isFresnel ("isFresnel", Range(0, 1)) = 0
    [KeywordEnum(Always, Equal, NotEqual)] _PLANESYSTEMCULLINGTYPE ("PlaneSystemCullingType", Float) = 0
    [ToggleUI] _PlaneSystemUsingLightBuffer ("PlaneSystemUsingLightBuffer", Float) = 0
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
        #pragma multi_compile_local _PLANESYSTEMCULLINGTYPE_ALWAYS _PLANESYSTEMCULLINGTYPE_EQUAL
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
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 _ScreenParams;
        float4 unity_OrthoParams;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float _GlobalIsRuntime;
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlanePointBufferEnable;
        float _GlobalPlaneSystemEnable;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float _PlaneSystemUsingLightBuffer : packoffset(c0);
            float4 _MaskTex_ST : packoffset(c2);
            float4 _MainTex_ST : packoffset(c4);
            float _invertB : packoffset(c5);
            float _Pixelation : packoffset(c5.y);
            float _AlphaClipThreshold : packoffset(c5.z);
            float _FresnelPower : packoffset(c5.w);
            float _isFresnel : packoffset(c6);
        }

        Texture2D<float4> _DBufferTexture0;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_vs_3.hlsl"
        #elif defined(FOG_LINEAR) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_vs_4.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_vs_1.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_vs_2.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_vs_2.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_44.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_42.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_43.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_32.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_40.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_41.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_38.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_30.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_31.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_39.hlsl"
        #elif defined(FOG_LINEAR) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_28.hlsl"
        #elif defined(FOG_LINEAR) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_29.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_36.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_37.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_26.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_34.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_35.hlsl"
        #elif defined(FOG_LINEAR) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_27.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_24.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_25.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_33.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_22.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_EQUAL) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_23.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_21.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeB/p0_ps_21.hlsl"
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
        #pragma multi_compile_local _PLANESYSTEMCULLINGTYPE_ALWAYS _PLANESYSTEMCULLINGTYPE_EQUAL

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
        float3 _WorldSpaceCameraPos;
        float4 _ScreenParams;
        float4x4 unity_MatrixVP;
        float _GlobalIsRuntime;
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlanePointBufferEnable;
        float _GlobalPlaneSystemEnable;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }
        cbuffer UnityPerMaterial
        {
            float _PlaneSystemUsingLightBuffer : packoffset(c0);
            float4 _MaskTex_ST : packoffset(c2);
            float4 _MainTex_ST : packoffset(c4);
            float _Pixelation : packoffset(c5.y);
            float _AlphaClipThreshold : packoffset(c5.z);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p1_vs_45.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p1_vs_46.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeB/p1_vs_46.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p1_ps_52.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p1_ps_53.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p1_ps_50.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p1_ps_51.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeB/p1_ps_51.hlsl"
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
        #pragma multi_compile_local _PLANESYSTEMCULLINGTYPE_ALWAYS _PLANESYSTEMCULLINGTYPE_EQUAL

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
        float3 _WorldSpaceCameraPos;
        float4 _ScreenParams;
        float4x4 unity_MatrixVP;
        float4x4 _PrevViewProjMatrix;
        float4x4 _NonJitteredViewProjMatrix;
        float _GlobalIsRuntime;
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlanePointBufferEnable;
        float _GlobalPlaneSystemEnable;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_MatrixPreviousM : packoffset(c36);
            float4 unity_MotionVectorsParams : packoffset(c44);
        }
        cbuffer UnityPerMaterial
        {
            float _PlaneSystemUsingLightBuffer : packoffset(c0);
            float4 _MaskTex_ST : packoffset(c2);
            float4 _MainTex_ST : packoffset(c4);
            float _Pixelation : packoffset(c5.y);
            float _AlphaClipThreshold : packoffset(c5.z);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p2_vs_54.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p2_vs_55.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeB/p2_vs_55.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p2_ps_61.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p2_ps_62.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p2_ps_59.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p2_ps_60.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeB/p2_ps_60.hlsl"
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
        #pragma multi_compile_local _PLANESYSTEMCULLINGTYPE_ALWAYS _PLANESYSTEMCULLINGTYPE_EQUAL
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
        float3 _WorldSpaceCameraPos;
        float4 _ScreenParams;
        float4x4 unity_MatrixVP;
        float _GlobalIsRuntime;
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlanePointBufferEnable;
        float _GlobalPlaneSystemEnable;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float _PlaneSystemUsingLightBuffer : packoffset(c0);
            float4 _MaskTex_ST : packoffset(c2);
            float4 _MainTex_ST : packoffset(c4);
            float _Pixelation : packoffset(c5.y);
            float _AlphaClipThreshold : packoffset(c5.z);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p3_vs_63.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p3_vs_64.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeB/p3_vs_64.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p3_ps_70.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p3_ps_68.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeB/p3_ps_69.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p3_ps_66.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_EQUAL) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeB/p3_ps_67.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeB/p3_ps_65.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeB/p3_ps_65.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}
