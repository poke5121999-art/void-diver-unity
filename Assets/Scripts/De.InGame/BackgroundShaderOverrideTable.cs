using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuntimeShaderOverrideTable", menuName = "LCArt/Runtime Shader Override Table")]
public class BackgroundShaderOverrideTable : ScriptableObject
{
	[Serializable]
	public struct Entry
	{
		public string LevelName;

		public EBackgroundShaderType BackgroundType;
	}

	[Tooltip("실험용: 머티리얼 인스턴스 대신 MPB로 _BackgroundType 세팅 (SRP Batcher 제외됨 — A/B 비교용)")]
	[SerializeField]
	private bool _useMaterialPropertyBlock;

	[SerializeField]
	private List<Entry> _entries = new List<Entry>();

	private Dictionary<string, Entry> _cache;

	public bool UseMaterialPropertyBlock => _useMaterialPropertyBlock;

	private void BuildCache()
	{
		if (_cache != null)
		{
			return;
		}
		_cache = new Dictionary<string, Entry>(StringComparer.OrdinalIgnoreCase);
		foreach (Entry entry in _entries)
		{
			if (!string.IsNullOrEmpty(entry.LevelName))
			{
				_cache[entry.LevelName] = entry;
			}
		}
	}

	private bool TryGetEntry(string levelName, out Entry result)
	{
		result = default;
		if (string.IsNullOrEmpty(levelName))
		{
			return false;
		}
		BuildCache();
		if (_cache.TryGetValue(levelName, out result))
		{
			return true;
		}
		foreach (Entry entry in _entries)
		{
			if (!string.IsNullOrEmpty(entry.LevelName)
				&& (levelName.Contains(entry.LevelName, StringComparison.OrdinalIgnoreCase)
					|| entry.LevelName.Contains(levelName, StringComparison.OrdinalIgnoreCase)))
			{
				result = entry;
				return true;
			}
		}
		return false;
	}

	public bool TryGetBackgroundType(string levelName, out EBackgroundShaderType type)
	{
		type = EBackgroundShaderType.Base;
		if (TryGetEntry(levelName, out var result))
		{
			type = result.BackgroundType;
			return true;
		}
		return false;
	}
}
