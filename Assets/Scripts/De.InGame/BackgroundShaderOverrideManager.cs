using System;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShaderOverrideManager : MonoBehaviour
{
	public const string BaseShaderName = "LCArt/Background/shader_DE_BG_Base";

	public static BackgroundShaderOverrideManager Instance;

	[SerializeField]
	private BackgroundShaderOverrideTable _table;

	private readonly Dictionary<Tuple<Material, Shader>, Material> _instancedMaterials;

	private readonly Dictionary<Tuple<Material, object>, Material> _ownedMaterials;

	private static readonly List<BackgroundShaderOverrider> _pending;

	public BackgroundShaderOverrideTable Table => null;

	private void Awake()
	{
	}

	public static void Register(BackgroundShaderOverrider overrider)
	{
	}

	private void OnDestroy()
	{
	}

	public Material GetInstancedMaterial(Material currentMaterial, Shader toShader)
	{
		return null;
	}

	public Material GetOwnedMaterial(Material source, object ownerKey)
	{
		return null;
	}
}
