using System;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShaderOverrideManager : MonoBehaviour
{
	public const string BaseShaderName = "LCArt/Background/shader_DE_BG_Base";

	public static BackgroundShaderOverrideManager Instance;

	[SerializeField]
	private BackgroundShaderOverrideTable _table;

	private readonly Dictionary<Tuple<Material, Shader>, Material> _instancedMaterials = new Dictionary<Tuple<Material, Shader>, Material>();

	private readonly Dictionary<Tuple<Material, object>, Material> _ownedMaterials = new Dictionary<Tuple<Material, object>, Material>();

	private static readonly List<BackgroundShaderOverrider> _pending = new List<BackgroundShaderOverrider>();

	public BackgroundShaderOverrideTable Table => _table;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		for (int i = 0; i < _pending.Count; i++)
		{
			if (_pending[i] != null)
			{
				_pending[i].ApplyOverride();
			}
		}
		_pending.Clear();
	}

	public static void Register(BackgroundShaderOverrider overrider)
	{
		if (!(overrider == null))
		{
			if (Instance != null)
			{
				overrider.ApplyOverride();
			}
			else
			{
				_pending.Add(overrider);
			}
		}
	}

	private void OnDestroy()
	{
		foreach (Material value in _instancedMaterials.Values)
		{
			if (value != null)
			{
				UnityEngine.Object.Destroy(value);
			}
		}
		_instancedMaterials.Clear();
		foreach (Material value2 in _ownedMaterials.Values)
		{
			if (value2 != null)
			{
				UnityEngine.Object.Destroy(value2);
			}
		}
		_ownedMaterials.Clear();
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public Material GetInstancedMaterial(Material currentMaterial, Shader toShader)
	{
		Tuple<Material, Shader> key = new Tuple<Material, Shader>(currentMaterial, toShader);
		if (!_instancedMaterials.ContainsKey(key))
		{
			_instancedMaterials[key] = new Material(currentMaterial);
			_instancedMaterials[key].shader = toShader;
		}
		return _instancedMaterials[key];
	}

	public Material GetOwnedMaterial(Material source, object ownerKey)
	{
		Tuple<Material, object> key = new Tuple<Material, object>(source, ownerKey);
		if (_ownedMaterials.TryGetValue(key, out var value) && value != null)
		{
			return value;
		}
		value = new Material(source);
		_ownedMaterials[key] = value;
		return value;
	}
}
