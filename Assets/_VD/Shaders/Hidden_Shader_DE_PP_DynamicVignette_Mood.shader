Shader "Hidden/Shader_DE_PP_DynamicVignette_Mood"
{
Properties
{
    [NoScaleOffset] _DynamicTexture ("DynamicTexture", 2D) = "white" {}
    [NoScaleOffset] _NoiseTexture ("NoiseTexture", 2D) = "white" {}
    _SightColor ("SightColor", Color) = (0, 0, 0, 0.501960813999176)
    _SightDissolveAlpha ("SightDissolveAlpha", Float) = 0.10000000149011612
    _LowHpColor ("LowHpColor", Color) = (1, 0, 0, 0.501960813999176)
    [HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
    [HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
}
SubShader
{
    Tags { "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalFullscreenSubTarget" }
    Pass
    {
        Name "DrawProcedural"
        Tags { "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalFullscreenSubTarget" }
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest Off
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
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixInvVP;
        float _MoodSightVignetteIntensity;
        float _MoodLowHPVignetteIntensity;
        float _FlipY;
        cbuffer UnityPerMaterial
        {
            float4 _SightColor : packoffset(c0);
            float4 _LowHpColor : packoffset(c2);
            float _SightDissolveAlpha : packoffset(c4);
        }

        Texture2D<float4> _DynamicTexture;
        Texture2D<float4> _NoiseTexture;
        SamplerState sampler_DynamicTexture;
        SamplerState sampler_NoiseTexture;

        // ---- progVertex
        #if 1
        #include "Hidden_Shader_DE_PP_DynamicVignette_Mood/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "Hidden_Shader_DE_PP_DynamicVignette_Mood/p0_ps_3.hlsl"
        #endif
        ENDHLSL
    }
    Pass
    {
        Name "Blit"
        Tags { "RenderPipeline" = "UniversalPipeline" "ShaderGraphShader" = "true" "ShaderGraphTargetId" = "UniversalFullscreenSubTarget" }
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest Off
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
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixInvVP;
        float _MoodSightVignetteIntensity;
        float _MoodLowHPVignetteIntensity;
        float _FlipY;
        cbuffer UnityPerMaterial
        {
            float4 _SightColor : packoffset(c0);
            float4 _LowHpColor : packoffset(c2);
            float _SightDissolveAlpha : packoffset(c4);
        }

        Texture2D<float4> _DynamicTexture;
        Texture2D<float4> _NoiseTexture;
        SamplerState sampler_DynamicTexture;
        SamplerState sampler_NoiseTexture;

        // ---- progVertex
        #if 1
        #include "Hidden_Shader_DE_PP_DynamicVignette_Mood/p1_vs_4.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "Hidden_Shader_DE_PP_DynamicVignette_Mood/p1_ps_3.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.Rendering.Fullscreen.ShaderGraph.FullscreenShaderGUI"
}
