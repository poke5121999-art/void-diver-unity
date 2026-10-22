using UnityEngine;

public class BackgroundShaderOverrider : MonoBehaviour
{
	[SerializeField]
	private string _levelName;

	private static readonly int _backgroundTypeId;

	private static MaterialPropertyBlock _mpb;

	private void Start()
	{
	}

	public void ApplyOverride()
	{
	}

	private void ApplyBackgroundType(string baseShaderName, EBackgroundShaderType backgroundType)
	{
	}

	private void ApplyBackgroundTypeMPB(string baseShaderName, EBackgroundShaderType backgroundType)
	{
	}

	public void SetLevelName(string levelName)
	{
	}
}
