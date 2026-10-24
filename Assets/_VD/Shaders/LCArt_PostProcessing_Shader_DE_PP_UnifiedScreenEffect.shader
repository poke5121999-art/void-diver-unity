Shader "LCArt/PostProcessing/Shader_DE_PP_UnifiedScreenEffect"
{
Properties
{
    [NoScaleOffset] _DynamicTexture ("DynamicTexture", 2D) = "white" {}
    [NoScaleOffset] _NoiseTexture ("NoiseTexture", 2D) = "white" {}
    _SightColor ("SightColor", Color) = (0, 0, 0, 0.501960813999176)
    _SightDissolveAlpha ("SightDissolveAlpha", Float) = 0.10000000149011612
    _LowHpColor ("LowHpColor", Color) = (1, 0, 0, 0.501960813999176)
}
SubShader
{
    Tags { "RenderPipeline" = "UniversalPipeline" }
    Pass
    {
        Name "DrawProcedural"
        Tags { "RenderPipeline" = "UniversalPipeline" }
        ZTest Off
        ZWrite Off
        Cull Off
        HLSLPROGRAM
        #pragma target 4.0
        #pragma vertex vert
        #pragma fragment frag

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
        float2 _GlobalMipBias;
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 _ZBufferParams;
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixInvVP;
        float4 _ScreenSize;
        float _RGBSplitDegree;
        float _RGBSplitIntensity;
        float _MotionSickRotationAngle;
        float _MotionSickRotationDuration;
        float _MotionSickPulseDuration;
        float _MotionSickPulseDegree;
        float _MotionSickShakeDegree;
        float _MotionSickIntensity;
        float _GlitchPulseFlickerStepCutoff;
        float _GlitchPulseDegree;
        float _GlitchPulseScale;
        float _GlitchPulseStepCutoff;
        float _GlitchPulseIntensity;
        float _PsycheDiveOffset;
        float _PsycheDiveIntensity;
        float _CinematicDissolveProgress;
        float _CinematicDissolveEdgeWidth;
        float4 _CinematicDissolveMaskParams;
        float4 _CinematicDissolveTargetParams;
        float _SightVignetteIntensity;
        float _LowHPVignetteIntensity;
        float _GlobalIsCurrentlyInStage;
        float _FlipY;
        float _WeatherIntensity;
        float _WeatherIndoorMask;
        float4 _GlobalOverallMapBoundMin;
        float4 _GlobalOverallMapBoundMax;
        float4 _WeatherFogColor;
        float2 _WeatherFogVignetteCenter;
        float _WeatherFogVignetteStart;
        float _WeatherFogVignetteEnd;
        float _WeatherFogVignetteIntensity;
        float _WeatherFogBlendMode;
        float _WeatherFogNoiseScale;
        float _WeatherFogNoiseSpeed;
        float _WeatherFogNoiseIntensity;
        float _WeatherFogIndoorFactor;
        float _WeatherStreakAngle;
        float _WeatherStreakSpeed;
        float _WeatherStreakDensity;
        float _WeatherStreakProbability;
        float _WeatherStreakThickness;
        float _WeatherStreakLength;
        float _WeatherStreakBrightness;
        float4 _WeatherStreakColor;
        float _WeatherStreakBlendMode;
        float _WeatherStreakCellShiftSpeed;
        float2 _WeatherStreakCameraParallax;
        float _WeatherSplashScale;
        float _WeatherSplashSpeed;
        float _WeatherSplashIntensity;
        float4 _WeatherSplashColor;
        float _WeatherSplashBlendMode;
        float _WeatherSplashSize;
        float _WeatherSplashCellShiftSpeed;
        float _WeatherHazeSpeed;
        float _WeatherHazeStrength;
        float _WeatherHazeBlurBlend;
        float _WeatherHazeNoiseScale;
        float _WeatherHazeVignetteStart;
        float _WeatherHazeVignetteEnd;
        float _WeatherBurnIntensity;
        float4 _WeatherBurnColor;
        float _WeatherBurnBlendMode;
        float _WeatherBurnSpeed;
        float2 _WeatherBurnDirection;
        float _WeatherBurnNoiseScale;
        float _WeatherBurnVignetteStart;
        float _WeatherBurnVignetteEnd;
        float _WeatherFrostOverlayIntensity;
        float _WeatherFrostOverlayStart;
        float _WeatherFrostOverlayEnd;
        float _WeatherFrostOverlayNoiseScale;
        float4 _WeatherFrostOverlayColor;
        cbuffer UnityPerMaterial
        {
            float4 _SightColor : packoffset(c0);
            float4 _LowHpColor : packoffset(c1);
            float _SightDissolveAlpha : packoffset(c4);
        }

        Texture2D<float4> _BlitTexture;
        Texture2D<float4> _CameraDepthTexture;
        Texture2D<float4> _CameraNormalsTexture;
        Texture2D<float4> _CinematicDissolveMask;
        Texture2D<float4> _CinematicDissolveTarget;
        Texture2D<float4> _DynamicTexture;
        Texture2D<float4> _GlobalWaterAreaMap;
        Texture2D<float4> _NoiseTexture;
        SamplerState sampler_CameraDepthTexture;
        SamplerState sampler_CameraNormalsTexture;
        SamplerState sampler_DynamicTexture;
        SamplerState sampler_GlobalWaterAreaMap;
        SamplerState sampler_vd_linear_clamp;
        SamplerState sampler_vd_linear_repeat;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedScreenEffect/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_UnifiedScreenEffect/p0_ps_3.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.Rendering.Fullscreen.ShaderGraph.FullscreenShaderGUI"
}
