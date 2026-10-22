using System;
using UnityEngine;

[Serializable]
public class LevelInfo
{
	public string name;

	public float baseHeight;

	public bool isVisible;

	public bool isAnActiveLevel;

	public bool isGridActiveLevel;

	public bool isExcludedFromBake;

	[HideInInspector]
	public float xOffset;

	[HideInInspector]
	public float zOffset;

	public LevelInfo()
	{
	}

	public LevelInfo(string name, float baseHeight, float xOffset = 0f, float zOffset = 0f, bool isVisible = true, bool isAnActiveLevel = false, bool isGridActiveLevel = false, bool isExcludedFromBake = false)
	{
	}

	public Vector3 GetOffset()
	{
		return default;
	}
}
