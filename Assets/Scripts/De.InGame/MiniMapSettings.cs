using UnityEngine;

public class MiniMapSettings : ScriptableObject
{
	public EResolutionType ResolutionType = EResolutionType.X1024;

	public int MSAA = 1;

	public string TextureDir;

	public float OrthographicSize;

	public float GetTextureSize()
	{
		switch (ResolutionType)
		{
		case EResolutionType.X4096:
			return 4096f;
		case EResolutionType.X2048:
			return 2048f;
		case EResolutionType.X512:
			return 512f;
		case EResolutionType.X256:
			return 256f;
		default:
			return 1024f;
		}
	}

	public Vector2 GetResolution()
	{
		float textureSize = GetTextureSize();
		return new Vector2(textureSize, textureSize);
	}
}
