public static class CustomScreenSpaceAmbientOcclusionConsts
{
	public const string SHADER_NAME_SSAO = "LCArt/PostProcessing/Shader_DE_PP_CustomScreenSpaceAmbientOcclusion";

	public const float NUMBER_INTENSITY_RATE = 0.25f;

	public const float NUMBER_RADIUS_RATE = 0.5f;

	public static readonly int SHADER_PROPERTY_EDITOR_VIEW = UnityEngine.Shader.PropertyToID("_EditorViewConsts");

	public static readonly int SHADER_PROPERTY_OVERRIDE_ENABLE = UnityEngine.Shader.PropertyToID("_CustomSSAOEnableOverride");

	public static readonly int SHADER_PROPERTY_OVERRIDE_COLOR = UnityEngine.Shader.PropertyToID("_CustomSSAOOverrideColor");

	public static readonly int SHADER_PROPERTY_OVERRIDE_INTENSITY = UnityEngine.Shader.PropertyToID("_CustomSSAOOverrideIntensity");

	public static readonly int SHADER_PROPERTY_OVERRIDE_RADIUS = UnityEngine.Shader.PropertyToID("_CustomSSAOOverrideRadius");
}
