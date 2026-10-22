Shader "LCArt/Deco/shader_DE_Deco_StickyDecal"
{
Properties
{
    _MainTex ("Texture", 2D) = "white" {}
    _DecalOffset ("Decal Offset (Only Use X and Z)", Vector) = (0, 0, 0, 0)
    [Header (In Game Death Effect Setting)] [Space (10)] _DissolveProgress ("Death Progress", Range(0, 1)) = 0
}
SubShader
{
    LOD 100
    Tags { "IgnoreProjector" = "true" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
    Pass
    {
        Name "FORWARD"
        Tags { "IgnoreProjector" = "true" "LightMode" = "SpineShadow" "Queue" = "AlphaTest" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        Blend One One
        BlendOp Min
        ZWrite Off
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
        float3 _WorldSpaceCameraPos;
        float4 _ProjectionParams;
        float4 _ZBufferParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixVP;
        float3 _PlaneBoundMinPos;
        float3 _PlaneBoundMaxPos;
        cbuffer UnityPerDraw
        {
            float4x4 unity_ObjectToWorld : packoffset(c0);
            float4x4 unity_WorldToObject : packoffset(c4);
        }
        cbuffer UnityPerMaterial
        {
            float4 _DecalOffset : packoffset(c0);
            float _DissolveProgress : packoffset(c1);
            float4 _MainTex_ST : packoffset(c2);
        }

        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlaneSightRenderTarget;
        SamplerState sampler_vd_point_clamp;

        // ---- progVertex
        #if 1
        #include "LCArt_Deco_shader_DE_Deco_StickyDecal/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_Deco_shader_DE_Deco_StickyDecal/p0_ps_2.hlsl"
        #endif
        ENDHLSL
    }
}
}
