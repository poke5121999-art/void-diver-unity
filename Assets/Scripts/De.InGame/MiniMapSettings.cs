using UnityEngine;

public class MiniMapSettings : ScriptableObject
{
	public EResolutionType ResolutionType;

	public int MSAA;

	public string TextureDir;

	public float OrthographicSize;

	public float GetTextureSize()
	{
		return 0f;
	}

	public Vector2 GetResolution()
	{
		return default;
	}
}
