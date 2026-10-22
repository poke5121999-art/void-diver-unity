Shader "LCArt/Character/Shader_DE_Character_FakeShadowCaster_Billboard"
{
Properties
{
    _FakeShadowVisibility ("Visibility", Float) = 1
}
SubShader
{
    Tags { "Queue" = "Geometry+10" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
    Pass
    {
        Name "FakeShadowCast"
        Tags { "LightMode" = "ObjectShadowCaster" "Queue" = "Geometry+10" "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" }
        BlendOp Max
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _ _DE_EDITOR_CAMERA
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
        float3 _PlaneBoundMaxPos;
        float _GlobalPlaneSystemEnable;
        float _GlobalPlanePointBufferEnable;
        float _FakeShadowGroundY;
        float _GlobalIsRuntime;
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
        cbuffer UnityPerMaterial
        {
            float _FakeShadowVisibility : packoffset(c0);
        }

        Texture2D<float4> _MainTex;
        Texture2D<float4> _PlanePointBuffer;
        Texture2D<float4> _PlaneSightRenderTarget;
        SamplerState sampler_MainTex;
        SamplerState sampler_PlanePointBuffer;
        SamplerState sampler_PlaneSightRenderTarget;

        // ---- progVertex
        #if defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_Shader_DE_Character_FakeShadowCaster_Billboard/p0_vs_5.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_Shader_DE_Character_FakeShadowCaster_Billboard/p0_vs_4.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_Shader_DE_Character_FakeShadowCaster_Billboard/p0_vs_3.hlsl"
        #elif 1
        #include "LCArt_Character_Shader_DE_Character_FakeShadowCaster_Billboard/p0_vs_2.hlsl"
        #endif
        // ---- progFragment
        #if defined(_DE_EDITOR_CAMERA) && defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_Shader_DE_Character_FakeShadowCaster_Billboard/p0_ps_10.hlsl"
        #elif defined(_DE_EDITOR_CAMERA)
        #include "LCArt_Character_Shader_DE_Character_FakeShadowCaster_Billboard/p0_ps_9.hlsl"
        #elif defined(_DE_PRERENDERING_CAMERA)
        #include "LCArt_Character_Shader_DE_Character_FakeShadowCaster_Billboard/p0_ps_8.hlsl"
        #elif 1
        #include "LCArt_Character_Shader_DE_Character_FakeShadowCaster_Billboard/p0_ps_7.hlsl"
        #endif
        ENDHLSL
    }
}
}
