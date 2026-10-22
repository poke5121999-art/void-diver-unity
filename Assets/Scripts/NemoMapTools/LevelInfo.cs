using System;
using UnityEngine;

[Serializable]
public class LevelInfo
{
	public string name = "New Level";

	public float baseHeight;

	public bool isVisible = true;

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
		this.name = name;
		this.baseHeight = baseHeight;
		this.xOffset = xOffset;
		this.zOffset = zOffset;
		this.isVisible = isVisible;
		this.isAnActiveLevel = isAnActiveLevel;
		this.isGridActiveLevel = isGridActiveLevel;
		this.isExcludedFromBake = isExcludedFromBake;
	}

	public Vector3 GetOffset()
	{
		return new Vector3(xOffset, baseHeight, zOffset);
	}
}
