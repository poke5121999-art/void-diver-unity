public static class RenderFeatureGate
{
	public static bool IsOpen(string disableGlobalProperty)
	{
		if (string.IsNullOrEmpty(disableGlobalProperty))
		{
			return true;
		}
		return UnityEngine.Shader.GetGlobalFloat(disableGlobalProperty) < 0.5f;
	}
}
