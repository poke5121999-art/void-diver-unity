Shader "ArtTeam/VFX/VFX_Master_typeA"
{
Properties
{
    _AlphaThreshold ("AlphaThreshold", Float) = 0.5
    _Pixelation ("Pixelation", Float) = 1
    _isColor ("isColor", Range(0, 1)) = 1
    _MainColor ("MainColor", Color) = (1, 1, 1, 1)
    _Main_Alpha ("Main_Alpha", Float) = 1
    _MainTex ("MainTex", 2D) = "white" {}
    [ToggleUI] _Main_Radial ("Main_Radial", Float) = 0
    [ToggleUI] _Use_Main_TilingOffset ("Use_Main_TilingOffset", Float) = 0
    [Toggle(_USE_SECONDARYCOLOR)] _USE_SECONDARYCOLOR ("Use_SecondaryColor(B)", Float) = 0
    _Main_Offset_UV1xy ("Main_Offset_UV1xy", Vector) = (0, 0, 0, 0)
    _Main_Rotate ("Main_Rotate", Float) = 0
    _DissolveTex ("DissolveTex", 2D) = "white" {}
    _Dissolve_Rotate ("Dissolve_Rotate", Float) = 0
    [ToggleUI] _Use_Dissolve_Green ("Use_Dissolve_Green", Float) = 0
    _DissolveDirection ("DissolveDirection", Vector) = (0, 0, 0, 0)
    _Dissolve_Offset_UV2y ("Dissolve_Offset_UV2y", Float) = 0
    [ToggleUI] _Use_Dissolve_Alpha ("Use_Dissolve_Alpha", Float) = 0
    [ToggleUI] _Use_Dissolve_Progress_Property ("Use_Dissolve_Progress_Property", Float) = 0
    _Dissolve_Progress_UV2w ("Dissolve_Progress_UV2w", Range(-1, 1)) = 1
    _Dissolve_Sharp ("Dissolve_Sharp", Range(0, 1)) = 0
    _DissovePower ("DissovePower", Float) = 1
    [ToggleUI] _DissolveEdge_Use ("DissolveEdge_Use", Float) = 0
    _DissolveEdge_Wide ("DissolveEdge_Wide", Float) = 0
    _DissolveEdge_Sharp ("DissolveEdge_Sharp", Range(0, 1)) = 0
    [HDR] _DissolveEdge_Color ("DissolveEdge_Color", Color) = (32, 19.99941062927246, 14.913209915161133, 0)
    [Toggle(_DEFORM_USE)] _DEFORM_USE ("Deform_Use", Float) = 1
    _DeformTex ("DeformTex", 2D) = "white" {}
    _DeformDirection ("DeformDirection", Vector) = (0, 0, 0, 0)
    _Deform_Time_Offset_UV2y ("Deform_Time_Offset_UV2y", Float) = 0
    _Deform_Strength_UV2x ("Deform_Strength_UV2x", Float) = 0
    [ToggleUI] _Use_Deform_Str_Property ("Use_Deform_Str_Property", Float) = 0
    [Toggle(_VERTEXDISPLACEMENT_USE)] _VERTEXDISPLACEMENT_USE ("VertexDisplacement_Use", Float) = 0
    _Vertex_Tex ("Vertex_Tex", 2D) = "white" {}
    [ToggleUI] _with_Deform_Strength ("with_Deform_Strength", Float) = 0
    _Vertex_Intensity_UV2x ("Vertex_Intensity_UV2x", Float) = 0
    [Toggle(_MASK_USE)] _MASK_USE ("Mask_Use", Float) = 1
    _MaskTex ("MaskTex", 2D) = "white" {}
    _Mask_Rotate ("Mask_Rotate", Float) = 0
    _FresnelMask_Threshold ("FresnelMask_Threshold", Range(0, 1)) = 1
    _FresnelMask_Offset ("FresnelMask_Offset", Range(0, 1)) = 0
    [ToggleUI] _Fresnel_Reverse ("Fresnel_Reverse", Float) = 0
    [ToggleUI] _Depth_Use ("Depth_Use", Float) = 0
    [ToggleUI] _Depth_Reverse ("Depth_Reverse", Float) = 0
    _Depth_Offset ("Depth_Offset", Float) = 0
    _Depth_Sharp ("Depth_Sharp", Range(0, 1)) = 0
    [KeywordEnum(Always, Equal, NotEqual)] _PLANESYSTEMCULLINGTYPE ("PlaneSystemCullingType", Float) = 0
    [ToggleUI] _PlaneSystemUsingLightBuffer ("PlaneSystemUsingLightBuffer", Float) = 0
    [HideInInspector] [ToggleUI] _Boundary_Use ("Boundary_Use", Float) = 0
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
        #pragma multi_compile_local _ _DEFORM_USE
        #pragma multi_compile_local _ _MASK_USE
        #pragma multi_compile_local _PLANESYSTEMCULLINGTYPE_ALWAYS _PLANESYSTEMCULLINGTYPE_EQUAL
        #pragma multi_compile _ _SURFACE_TYPE_TRANSPARENT
        #pragma multi_compile_local _ _USE_SECONDARYCOLOR
        #pragma multi_compile_local _ _VERTEXDISPLACEMENT_USE

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
        float4 _ScreenParams;
        float4 _ZBufferParams;
        float4 unity_OrthoParams;
        float4 _RTHandleScale;
        float4 unity_FogParams;
        float4 unity_FogColor;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4 _CameraDepthTexture_TexelSize;
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlanePointBufferEnable;
        float _GlobalPlaneSystemEnable;
        float _GlobalIsRuntime;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c2);
            float4 _DeformTex_ST : packoffset(c4);
            float4 _MaskTex_ST : packoffset(c6);
            float2 _DeformDirection : packoffset(c7);
            float4 _MainColor : packoffset(c8);
            float4 _DissolveTex_ST : packoffset(c10);
            float _DissovePower : packoffset(c11);
            float _isColor : packoffset(c11.y);
            float _Pixelation : packoffset(c11.z);
            float _FresnelMask_Threshold : packoffset(c11.w);
            float _FresnelMask_Offset : packoffset(c12);
            float _AlphaThreshold : packoffset(c12.y);
            float2 _DissolveDirection : packoffset(c12.z);
            float _Fresnel_Reverse : packoffset(c13);
            float _Depth_Offset : packoffset(c13.y);
            float _Depth_Use : packoffset(c13.z);
            float _Depth_Sharp : packoffset(c13.w);
            float _Depth_Reverse : packoffset(c14);
            float _Main_Rotate : packoffset(c14.y);
            float _PlaneSystemUsingLightBuffer : packoffset(c14.z);
            float _Use_Dissolve_Green : packoffset(c14.w);
            float _Dissolve_Rotate : packoffset(c15);
            float _Mask_Rotate : packoffset(c15.y);
            float _Vertex_Intensity_UV2x : packoffset(c15.z);
            float4 _Vertex_Tex_ST : packoffset(c17);
            float _DissolveEdge_Use : packoffset(c18.y);
            float _DissolveEdge_Wide : packoffset(c18.z);
            float _DissolveEdge_Sharp : packoffset(c18.w);
            float4 _DissolveEdge_Color : packoffset(c19);
            float _Deform_Time_Offset_UV2y : packoffset(c20);
            float _Deform_Strength_UV2x : packoffset(c20.y);
            float _Main_Alpha : packoffset(c20.z);
            float2 _Main_Offset_UV1xy : packoffset(c21);
            float _Dissolve_Offset_UV2y : packoffset(c21.z);
            float _Dissolve_Progress_UV2w : packoffset(c21.w);
            float _Use_Dissolve_Progress_Property : packoffset(c22);
            float _Use_Main_TilingOffset : packoffset(c22.y);
            float _Use_Deform_Str_Property : packoffset(c22.z);
            float _Use_Dissolve_Alpha : packoffset(c22.w);
            float _Dissolve_Sharp : packoffset(c23);
            float _Main_Radial : packoffset(c23.y);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _DBufferTexture0;
        Texture2D<float4> _DeformTex;
        Texture2D<float4> _DissolveTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        Texture2D<float4> _SecColorRamp;
        Texture2D<float4> _Vertex_Tex;
        SamplerState sampler_DeformTex;
        SamplerState sampler_DissolveTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;
        SamplerState sampler_SecColorRamp;
        SamplerState sampler_vd_linear_repeat;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_20.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_23.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_18.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_19.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_16.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_21.hlsl"
        #elif defined(FOG_LINEAR) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_22.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_9.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_12.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_15.hlsl"
        #elif defined(FOG_LINEAR) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_17.hlsl"
        #elif defined(FOG_LINEAR) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_14.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_7.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_8.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_5.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_10.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_11.hlsl"
        #elif defined(FOG_LINEAR) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_13.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_4.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_6.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_3.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_2.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_vs_2.hlsl"
        #endif
        // ---- progFragment
        #if defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_140.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_139.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_133.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_137.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_141.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_100.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_129.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_131.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_120.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_132.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_136.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_138.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_135.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_99.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_93.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_97.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_101.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_127.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_128.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_130.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_89.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_91.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_119.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_113.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_117.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_121.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_80.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_109.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_111.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_134.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_92.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_96.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_98.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_95.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_123.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_125.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_126.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_87.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_88.hlsl"
        #elif defined(FOG_LINEAR) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_90.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_112.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_116.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_118.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_115.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_79.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_73.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_77.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_81.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_107.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_108.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_110.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_69.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_71.hlsl"
        #elif defined(FOG_LINEAR) && defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_94.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_124.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_83.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_85.hlsl"
        #elif defined(FOG_LINEAR) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_86.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_114.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_72.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_76.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_78.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_75.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_103.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_105.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_106.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_67.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_68.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_70.hlsl"
        #elif defined(FOG_LINEAR) && defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_122.hlsl"
        #elif defined(FOG_LINEAR) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_84.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_74.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_104.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_63.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_65.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_66.hlsl"
        #elif defined(FOG_LINEAR) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_82.hlsl"
        #elif defined(_DBUFFER_MRT3) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_102.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_64.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_62.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeA/p0_ps_62.hlsl"
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
        #pragma multi_compile_local _ _DEFORM_USE
        #pragma multi_compile_local _ _MASK_USE
        #pragma multi_compile_local _PLANESYSTEMCULLINGTYPE_ALWAYS _PLANESYSTEMCULLINGTYPE_EQUAL
        #pragma multi_compile_local _ _USE_SECONDARYCOLOR
        #pragma multi_compile_local _ _VERTEXDISPLACEMENT_USE

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
        float4 _ScreenParams;
        float4 _ZBufferParams;
        float4 unity_OrthoParams;
        float4 _RTHandleScale;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4 _CameraDepthTexture_TexelSize;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c2);
            float4 _DeformTex_ST : packoffset(c4);
            float4 _MaskTex_ST : packoffset(c6);
            float2 _DeformDirection : packoffset(c7);
            float4 _DissolveTex_ST : packoffset(c10);
            float _DissovePower : packoffset(c11);
            float _Pixelation : packoffset(c11.z);
            float _FresnelMask_Threshold : packoffset(c11.w);
            float _FresnelMask_Offset : packoffset(c12);
            float _AlphaThreshold : packoffset(c12.y);
            float2 _DissolveDirection : packoffset(c12.z);
            float _Fresnel_Reverse : packoffset(c13);
            float _Depth_Offset : packoffset(c13.y);
            float _Depth_Use : packoffset(c13.z);
            float _Depth_Sharp : packoffset(c13.w);
            float _Depth_Reverse : packoffset(c14);
            float _Main_Rotate : packoffset(c14.y);
            float _Use_Dissolve_Green : packoffset(c14.w);
            float _Dissolve_Rotate : packoffset(c15);
            float _Mask_Rotate : packoffset(c15.y);
            float _Vertex_Intensity_UV2x : packoffset(c15.z);
            float4 _Vertex_Tex_ST : packoffset(c17);
            float _Deform_Time_Offset_UV2y : packoffset(c20);
            float _Deform_Strength_UV2x : packoffset(c20.y);
            float _Main_Alpha : packoffset(c20.z);
            float2 _Main_Offset_UV1xy : packoffset(c21);
            float _Dissolve_Offset_UV2y : packoffset(c21.z);
            float _Dissolve_Progress_UV2w : packoffset(c21.w);
            float _Use_Dissolve_Progress_Property : packoffset(c22);
            float _Use_Main_TilingOffset : packoffset(c22.y);
            float _Use_Deform_Str_Property : packoffset(c22.z);
            float _Use_Dissolve_Alpha : packoffset(c22.w);
            float _Dissolve_Sharp : packoffset(c23);
            float _Main_Radial : packoffset(c23.y);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _DeformTex;
        Texture2D<float4> _DissolveTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        Texture2D<float4> _Vertex_Tex;
        SamplerState sampler_DeformTex;
        SamplerState sampler_DissolveTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;
        SamplerState sampler_vd_linear_repeat;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_149.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_152.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_147.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_148.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_145.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_150.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_151.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_144.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_146.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_143.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_142.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_vs_142.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_172.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_171.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_169.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_173.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_163.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_165.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_168.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_170.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_167.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_161.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_162.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_164.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_166.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_159.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_160.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_158.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeA/p1_ps_158.hlsl"
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
        #pragma multi_compile_local _ _DEFORM_USE
        #pragma multi_compile_local _ _MASK_USE
        #pragma multi_compile_local _PLANESYSTEMCULLINGTYPE_ALWAYS _PLANESYSTEMCULLINGTYPE_EQUAL
        #pragma multi_compile_local _ _USE_SECONDARYCOLOR
        #pragma multi_compile_local _ _VERTEXDISPLACEMENT_USE

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
        float4 _ScreenParams;
        float4 _ZBufferParams;
        float4 unity_OrthoParams;
        float4 _RTHandleScale;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4x4 _PrevViewProjMatrix;
        float4x4 _NonJitteredViewProjMatrix;
        float4 _CameraDepthTexture_TexelSize;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
            float4x4 unity_MatrixPreviousM : packoffset(c36);
            float4 unity_MotionVectorsParams : packoffset(c44);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c2);
            float4 _DeformTex_ST : packoffset(c4);
            float4 _MaskTex_ST : packoffset(c6);
            float2 _DeformDirection : packoffset(c7);
            float4 _DissolveTex_ST : packoffset(c10);
            float _DissovePower : packoffset(c11);
            float _Pixelation : packoffset(c11.z);
            float _FresnelMask_Threshold : packoffset(c11.w);
            float _FresnelMask_Offset : packoffset(c12);
            float _AlphaThreshold : packoffset(c12.y);
            float2 _DissolveDirection : packoffset(c12.z);
            float _Fresnel_Reverse : packoffset(c13);
            float _Depth_Offset : packoffset(c13.y);
            float _Depth_Use : packoffset(c13.z);
            float _Depth_Sharp : packoffset(c13.w);
            float _Depth_Reverse : packoffset(c14);
            float _Main_Rotate : packoffset(c14.y);
            float _Use_Dissolve_Green : packoffset(c14.w);
            float _Dissolve_Rotate : packoffset(c15);
            float _Mask_Rotate : packoffset(c15.y);
            float _Vertex_Intensity_UV2x : packoffset(c15.z);
            float4 _Vertex_Tex_ST : packoffset(c17);
            float _Deform_Time_Offset_UV2y : packoffset(c20);
            float _Deform_Strength_UV2x : packoffset(c20.y);
            float _Main_Alpha : packoffset(c20.z);
            float2 _Main_Offset_UV1xy : packoffset(c21);
            float _Dissolve_Offset_UV2y : packoffset(c21.z);
            float _Dissolve_Progress_UV2w : packoffset(c21.w);
            float _Use_Dissolve_Progress_Property : packoffset(c22);
            float _Use_Main_TilingOffset : packoffset(c22.y);
            float _Use_Deform_Str_Property : packoffset(c22.z);
            float _Use_Dissolve_Alpha : packoffset(c22.w);
            float _Dissolve_Sharp : packoffset(c23);
            float _Main_Radial : packoffset(c23.y);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _DeformTex;
        Texture2D<float4> _DissolveTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        Texture2D<float4> _Vertex_Tex;
        SamplerState sampler_DeformTex;
        SamplerState sampler_DissolveTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;
        SamplerState sampler_vd_linear_repeat;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_183.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_186.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_181.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_182.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_179.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_184.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_185.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_178.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_180.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_177.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_176.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_vs_176.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_206.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_205.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_203.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_207.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_197.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_199.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_202.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_204.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_201.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_195.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_196.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_198.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_200.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_193.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_194.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_192.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeA/p2_ps_192.hlsl"
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
        #pragma multi_compile_local _ _DEFORM_USE
        #pragma multi_compile_local _ _MASK_USE
        #pragma multi_compile_local _PLANESYSTEMCULLINGTYPE_ALWAYS _PLANESYSTEMCULLINGTYPE_EQUAL
        #pragma multi_compile _ _SURFACE_TYPE_TRANSPARENT
        #pragma multi_compile_local _ _USE_SECONDARYCOLOR
        #pragma multi_compile_local _ _VERTEXDISPLACEMENT_USE

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
        float4 _ScreenParams;
        float4 _ZBufferParams;
        float4 unity_OrthoParams;
        float4 _RTHandleScale;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float4x4 unity_MatrixInvVP;
        float4 _CameraDepthTexture_TexelSize;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _MainTex_ST : packoffset(c2);
            float4 _DeformTex_ST : packoffset(c4);
            float4 _MaskTex_ST : packoffset(c6);
            float2 _DeformDirection : packoffset(c7);
            float4 _DissolveTex_ST : packoffset(c10);
            float _DissovePower : packoffset(c11);
            float _Pixelation : packoffset(c11.z);
            float _FresnelMask_Threshold : packoffset(c11.w);
            float _FresnelMask_Offset : packoffset(c12);
            float _AlphaThreshold : packoffset(c12.y);
            float2 _DissolveDirection : packoffset(c12.z);
            float _Fresnel_Reverse : packoffset(c13);
            float _Depth_Offset : packoffset(c13.y);
            float _Depth_Use : packoffset(c13.z);
            float _Depth_Sharp : packoffset(c13.w);
            float _Depth_Reverse : packoffset(c14);
            float _Main_Rotate : packoffset(c14.y);
            float _Use_Dissolve_Green : packoffset(c14.w);
            float _Dissolve_Rotate : packoffset(c15);
            float _Mask_Rotate : packoffset(c15.y);
            float _Vertex_Intensity_UV2x : packoffset(c15.z);
            float4 _Vertex_Tex_ST : packoffset(c17);
            float _Deform_Time_Offset_UV2y : packoffset(c20);
            float _Deform_Strength_UV2x : packoffset(c20.y);
            float _Main_Alpha : packoffset(c20.z);
            float2 _Main_Offset_UV1xy : packoffset(c21);
            float _Dissolve_Offset_UV2y : packoffset(c21.z);
            float _Dissolve_Progress_UV2w : packoffset(c21.w);
            float _Use_Dissolve_Progress_Property : packoffset(c22);
            float _Use_Main_TilingOffset : packoffset(c22.y);
            float _Use_Deform_Str_Property : packoffset(c22.z);
            float _Use_Dissolve_Alpha : packoffset(c22.w);
            float _Dissolve_Sharp : packoffset(c23);
            float _Main_Radial : packoffset(c23.y);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _DeformTex;
        Texture2D<float4> _DissolveTex;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _MaskTex;
        Texture2D<float4> _Vertex_Tex;
        SamplerState sampler_DeformTex;
        SamplerState sampler_DissolveTex;
        SamplerState sampler_MainTex;
        SamplerState sampler_MaskTex;
        SamplerState sampler_vd_linear_repeat;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_149.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_152.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_147.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_148.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_145.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_150.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_151.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_144.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_146.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_143.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_142.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_vs_142.hlsl"
        #endif
        // ---- progFragment
        #if defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_226.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_225.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_219.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_223.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_227.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_215.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_217.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_218.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_222.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_224.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_USE_SECONDARYCOLOR)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_221.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_213.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_EQUAL) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_214.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(_VERTEXDISPLACEMENT_USE)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_216.hlsl"
        #elif defined(_ALPHATEST_ON) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_220.hlsl"
        #elif defined(_DEFORM_USE) && defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_209.hlsl"
        #elif defined(_DEFORM_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_211.hlsl"
        #elif defined(_MASK_USE) && defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_212.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS) && defined(_SURFACE_TYPE_TRANSPARENT)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_210.hlsl"
        #elif defined(_PLANESYSTEMCULLINGTYPE_ALWAYS)
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_208.hlsl"
        #else
        #include "ArtTeam_VFX_VFX_Master_typeA/p3_ps_208.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}
