using System;
using System.Runtime.CompilerServices;
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

	[CompilerGenerated]
	private Action m_OnAfterApply;

	public event Action OnAfterApply
	{
		[CompilerGenerated]
		add
		{
		}
		[CompilerGenerated]
		remove
		{
		}
	}

	private void Start()
	{
	}

	private void Apply(int areaDecorationSettingId)
	{
	}
}
