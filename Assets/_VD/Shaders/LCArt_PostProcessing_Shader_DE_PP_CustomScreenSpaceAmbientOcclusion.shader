Shader "LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion"
{
Properties
{
}
SubShader
{
    Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
    Pass
    {
        Name "SSAO_Occlusion"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        ZTest Always
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_local _INTERLEAVED_GRADIENT
        #pragma multi_compile_local _ _ORTHOGRAPHIC

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
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 _ZBufferParams;
        float4 _RTHandleScale;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixInvV;
        float4x4 unity_MatrixInvP;
        float4 _BlitScaleBias;
        float4 _CameraDepthTexture_TexelSize;
        float4 _CameraNormalsTexture_TexelSize;
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float _GlobalBackgroundSeeThroughEnable;
        float4 _SSAOParams;
        float4 _CameraViewTopLeftCorner[2];
        float4x4 _CameraViewProjections[2];
        float4 _SourceSize;
        float4 _ProjectionParams2;
        float4 _CameraViewXExtent[2];
        float4 _CameraViewYExtent[2];
        float4 _CameraViewZExtent[2];
        float _CustomSSAOEnableOverride;
        float _CustomSSAOOverrideIntensity;
        float _CustomSSAOOverrideRadius;
        float4 _EditorViewConsts;
        int _SSAOSourceMode;
        int _SSAOSampleCount;

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _CameraNormalsTexture;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_PlaneSightRenderTarget;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if defined(_INTERLEAVED_GRADIENT) && defined(_ORTHOGRAPHIC)
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p0_ps_5.hlsl"
        #elif defined(_INTERLEAVED_GRADIENT)
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p0_ps_4.hlsl"
        #else
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p0_ps_4.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "SSAO_Bilateral_HorizontalBlur"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
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
        float4 _SSAOParams;
        float4 _SourceSize;

        Texture2D<float4> _BlitTexture;
        SamplerState sampler_BlitTexture;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p1_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p1_ps_6.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "SSAO_Bilateral_VerticalBlur"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
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
        float4 _SSAOParams;
        float4 _SourceSize;

        Texture2D<float4> _BlitTexture;
        SamplerState sampler_BlitTexture;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p2_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p2_ps_7.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "SSAO_Bilateral_FinalBlur"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
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
        float4 _SSAOParams;
        float4 _SourceSize;

        Texture2D<float4> _BlitTexture;
        SamplerState sampler_BlitTexture;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p3_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p3_ps_8.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "SSAO_Bilateral_FinalBlur_AfterOpaque"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        Blend DstColor SrcColor
        ZTest NotEqual
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
        float4 _SSAOParams;
        float4 _SourceSize;
        float4 _CustomColor;
        float _CustomSSAOEnableOverride;
        float4 _CustomSSAOOverrideColor;

        Texture2D<float4> _BlitTexture;
        SamplerState sampler_BlitTexture;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p4_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p4_ps_9.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "SSAO_Gaussian_HorizontalBlur"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
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
        float4 _SSAOParams;
        float4 _SourceSize;

        Texture2D<float4> _BlitTexture;
        SamplerState sampler_BlitTexture;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p5_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p5_ps_10.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "SSAO_Gaussian_VerticalBlur"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
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
        float4 _SSAOParams;
        float4 _SourceSize;

        Texture2D<float4> _BlitTexture;
        SamplerState sampler_BlitTexture;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p6_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p6_ps_11.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "SSAO_Gaussian_VerticalBlur_AfterOpaque"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        Blend DstColor SrcColor
        ZTest NotEqual
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
        float4 _SSAOParams;
        float4 _SourceSize;
        float4 _CustomColor;
        float _CustomSSAOEnableOverride;
        float4 _CustomSSAOOverrideColor;

        Texture2D<float4> _BlitTexture;
        SamplerState sampler_BlitTexture;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p7_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p7_ps_12.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "SSAO_Kawase"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
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
        float _KawaseBlurIteration;
        int _LastKawasePass;
        float4 _SSAOParams;
        float4 _SourceSize;

        Texture2D<float4> _BlitTexture;
        SamplerState sampler_BlitTexture;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p8_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p8_ps_13.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "SSAO_Kawase_AfterOpaque"
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        Blend DstColor SrcColor
        ZTest NotEqual
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
        float _KawaseBlurIteration;
        int _LastKawasePass;
        float4 _SSAOParams;
        float4 _SourceSize;
        float4 _CustomColor;
        float _CustomSSAOEnableOverride;
        float4 _CustomSSAOOverrideColor;

        Texture2D<float4> _BlitTexture;
        SamplerState sampler_BlitTexture;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p9_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_CustomScreenSpaceAmbientOcclusion/p9_ps_14.hlsl"
        #endif
        ENDHLSL
    }
}
}
