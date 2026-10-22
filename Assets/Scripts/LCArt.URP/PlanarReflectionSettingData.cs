internal class PlanarReflectionSettingData
{
	private int _maxLod = UnityEngine.QualitySettings.maximumLODLevel;

	private float _lodBias = UnityEngine.QualitySettings.lodBias;

	public void Set()
	{
		UnityEngine.GL.invertCulling = true;
		UnityEngine.QualitySettings.maximumLODLevel = 1;
		UnityEngine.QualitySettings.lodBias = _lodBias * 0.5f;
	}

	public void Restore()
	{
		UnityEngine.GL.invertCulling = false;
		UnityEngine.QualitySettings.maximumLODLevel = _maxLod;
		UnityEngine.QualitySettings.lodBias = _lodBias;
	}
}
