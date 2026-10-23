using System;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(MeshRenderer))]
public class SkinnedAreaObject : MonoBase
{
	[Inject]
	private GameLoungeManager _gameLoungeManager;

	[Inject]
	private GameContext _gameContext;

	[Tooltip("TAreaDecoration.GroupId")]
	[SerializeField]
	private int _groupId;

	[SerializeField]
	private MaterialPreset[] _materialPresets;

	private MeshRenderer _meshRenderer;

	public event Action OnAfterApply;

	private void Start()
	{
		if (_groupId < 1)
		{
			return;
		}
		_meshRenderer = GetComponent<MeshRenderer>();
		_gameLoungeManager.OnAreaDecorationChanged.Where(_ => TAreaDecoration.Table[_.newAreaDecorationSettingId].GroupId == _groupId).Subscribe(_ => Apply(_.newAreaDecorationSettingId)).AddTo(this);
		if (_gameContext.ActiveAreaDecorations != null && _gameContext.ActiveAreaDecorations.TryGetValue(_groupId, out var value))
		{
			Apply(value);
		}
	}

	private void Apply(int areaDecorationSettingId)
	{
		int index = TAreaDecoration.Table[areaDecorationSettingId].Index;
		if (_materialPresets == null || index < 0 || index >= _materialPresets.Length)
		{
			return;
		}
		MaterialPreset materialPreset = _materialPresets[index];
		if (materialPreset == null || materialPreset.Materials == null)
		{
			return;
		}
		Material[] sharedMaterials = _meshRenderer.sharedMaterials;
		int num = Mathf.Min(sharedMaterials.Length, materialPreset.Materials.Length);
		for (int i = 0; i < num; i++)
		{
			if (materialPreset.Materials[i] != null)
			{
				sharedMaterials[i] = materialPreset.Materials[i];
			}
		}
		_meshRenderer.materials = sharedMaterials;
		this.OnAfterApply?.Invoke();
	}
}
