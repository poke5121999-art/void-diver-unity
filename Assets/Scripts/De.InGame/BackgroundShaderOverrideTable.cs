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
	private List<Entry> _entries;

	private Dictionary<string, Entry> _cache;

	public bool UseMaterialPropertyBlock => false;

	private void BuildCache()
	{
	}

	private bool TryGetEntry(string levelName, out Entry result)
	{
		result = default;
		return false;
	}

	public bool TryGetBackgroundType(string levelName, out EBackgroundShaderType type)
	{
		type = default;
		return false;
	}
}
