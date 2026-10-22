Shader "LCArt/PostProcessing/Shader_DE_PP_Sanity_Mood"
{
Properties
{
    _RGBSplitDegree ("RGBSplitDegree", Float) = 0.02500000037252903
    _MotionSickRotationDuration ("MotionSickRotationDuration", Float) = 8
    _MotionSickRotationAngle ("MotionSickRotationAngle", Float) = 1
    _MotionSickPulseDuration ("MotionSickPulseDuration", Float) = 5
    _MotionSickPulseDegree ("MotionSickPulseDegree", Float) = 0.009999999776482582
    _MotionSickShakeDegree ("MotionSickShakeDegree", Float) = 0.0005000000237487257
    _GlitchPulseDegree ("GlitchPulseDegree", Float) = 0.004999999888241291
    _GlitchPulseScale ("GlitchPulseScale", Float) = 50
    _GlitchPulseStepCutoff ("GlitchPulseStepCutoff", Float) = 0.5
    _GlitchPulseFlickerStepCutoff ("GlitchPulseFlickerStepCutoff", Float) = 0.019999999552965164
    _PsycheDiveOffset ("PsycheDiveOffset", Float) = 0.03999999910593033
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
        float4 _TimeParameters;
        float3 _WorldSpaceCameraPos;
        float4 unity_OrthoParams;
        float4x4 unity_MatrixV;
        float4x4 unity_MatrixInvVP;
        float4 _ScreenSize;
        float _MoodRGBSplitIntensity;
        float _MoodMotionSickIntensity;
        float _MoodGlitchPulseIntensity;
        float _MoodPsycheDiveIntensity;
        float _FlipY;
        cbuffer UnityPerMaterial
        {
            float _RGBSplitDegree : packoffset(c0);
            float _MotionSickRotationAngle : packoffset(c0.y);
            float _MotionSickRotationDuration : packoffset(c0.z);
            float _MotionSickPulseDuration : packoffset(c0.w);
            float _MotionSickPulseDegree : packoffset(c1);
            float _MotionSickShakeDegree : packoffset(c1.y);
            float _GlitchPulseFlickerStepCutoff : packoffset(c1.z);
            float _GlitchPulseDegree : packoffset(c1.w);
            float _GlitchPulseScale : packoffset(c2);
            float _GlitchPulseStepCutoff : packoffset(c2.y);
            float _PsycheDiveOffset : packoffset(c2.z);
        }

        Texture2D<float4> _BlitTexture;

        // ---- progVertex
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_Sanity_Mood/p0_vs_1.hlsl"
        #endif
        // ---- progFragment
        #if 1
        #include "LCArt_PostProcessing_Shader_DE_PP_Sanity_Mood/p0_ps_3.hlsl"
        #endif
        ENDHLSL
    }
}
Fallback "Hidden/Shader Graph/FallbackError"
CustomEditor "UnityEditor.Rendering.Fullscreen.ShaderGraph.FullscreenShaderGUI"
}
