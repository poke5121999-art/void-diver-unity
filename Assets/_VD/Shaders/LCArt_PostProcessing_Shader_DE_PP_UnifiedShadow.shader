Shader "LCArt/PostProcessing/Shader_DE_PP_UnifiedShadow"
{
Properties
{
    _FakeShadowVisibility ("Visibility", Float) = 1
}
SubShader
{
    LOD 100
    Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
    Pass
    {
        Name "ScreenSpaceShadowCompute"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        ZTest Always
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ _DE_FAKE_SHADOW_BLOB

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
        float4 unity_DeltaTime;
        float4x4 glstate_matrix_projection;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixInvV;
        float4x4 unity_MatrixInvP;
        float4 _BlitScaleBias;
        float _ScreenSpaceShadowPointCount;
        float _ScreenSpaceShadowPointRanges[8];
        float4 _ScreenSpaceShadowPointPositions[8];
        float _GlobalShadowStandardY;
        float _GlobalScreenSpaceShadowAlpha;
        float _SSShadowStepCount;
        float _SSShadowFrameIndex;
        float _SSShadowIsFirstFrame;
        float4x4 _SSShadowPrevVPMatrix;
        float4 _ShadowRTSize;
        float _GlobalIsCurrentlyInStage;

        Texture2D<float4> _CustomDepthBuffer;
        Texture2D<float4> _PrevShadowTexture;
        Texture2D<float4> _ScreenSpaceShadowMask;
        SamplerState sampler_CustomDepthBuffer;
        SamplerState sampler_vd_linear_clamp;

        // ---- progVertex
        #if defined(_DE_FAKE_SHADOW_BLOB)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p0_vs_2.hlsl"
        #elif 1
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(_DE_FAKE_SHADOW_BLOB)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p0_ps_6.hlsl"
        #elif 1
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p0_ps_5.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "FakeShadowCast"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        BlendOp Max
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ _DE_EDITOR_CAMERA
        #pragma multi_compile _ _DE_FAKE_SHADOW_PLANE_SIGHT
        #pragma multi_compile _ _DE_PRERENDERING_CAMERA

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
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float3 _PlaneBoundMinPos;
        float _FakeShadowGroundY;
        float3 _PlaneBoundMaxPos;
        float _GlobalBoxAreaEnable;
        float4 _GlobalBoxAreaCenters[8];
        float _GlobalPlaneSystemEnable;
        float4 _GlobalBoxAreaExtents[8];
        float _GlobalBoxAreaCoverCharacters[8];
        float _GlobalPlanePointBufferEnable;
        float _GlobalIsRuntime;
        float _FakeShadowVisibility;
        float4 _MainTex_ST;
        cbuffer FakeShadowProps
        {
            float _FakeShadowIsPoint : packoffset(c0);
            float4 _FakeShadowLightData : packoffset(c1);
            float _FakeShadowPointRange : packoffset(c2);
            float _FakeShadowAlpha : packoffset(c2.y);
            float _FakeShadowScale : packoffset(c2.z);
        }
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(_DE_EDITOR_CAMERA) && defined(_DE_FAKE_SHADOW_PLANE_SIGHT) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_vs_27.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_FAKE_SHADOW_PLANE_SIGHT)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_vs_25.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_vs_19.hlsl"
        #elif defined(_DE_FAKE_SHADOW_PLANE_SIGHT) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_vs_23.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_vs_17.hlsl"
        #elif defined(_DE_FAKE_SHADOW_PLANE_SIGHT)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_vs_21.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_vs_15.hlsl"
        #elif 1
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_vs_13.hlsl"
        #endif
        // ---- progFragment
        #if defined(_DE_EDITOR_CAMERA) && defined(_DE_FAKE_SHADOW_PLANE_SIGHT) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_ps_47.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_FAKE_SHADOW_PLANE_SIGHT)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_ps_45.hlsl"
        #elif defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_ps_39.hlsl"
        #elif defined(_DE_FAKE_SHADOW_PLANE_SIGHT) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_ps_43.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_ps_37.hlsl"
        #elif defined(_DE_FAKE_SHADOW_PLANE_SIGHT)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_ps_41.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_ps_35.hlsl"
        #elif 1
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p1_ps_33.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "ScreenSpaceShadowApply"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        Blend DstColor Zero
        ZTest Always
        ZWrite Off
        Cull Off
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
        float4 _BlitScaleBias;
        float4 _GlobalShadowColor;

        Texture2D<float4> _BlitTexture;
        SamplerState sampler_vd_linear_clamp;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p2_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedShadow/p2_ps_50.hlsl"
        #endif
        ENDHLSL
    }
}
}
