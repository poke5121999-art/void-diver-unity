using System;
using System.Collections.Generic;
using GameCommon.Data;
using NemoLib.AssetManagement;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class GameVfxManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	private Dictionary<long, GameObject> _vfxDic;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnDisplayFx(NtfDisplayFx message)
	{
	}

	private void AttachUnitFollower(GameObject vfxGo, NtfDisplayFx message, UnitModel targetUnit)
	{
	}

	private IObservable<Unit> GetVfxLifetimeEnd(GameObject vfxGo)
	{
		return null;
	}

	private void OnRemoveFx(NtfRemoveFx message)
	{
	}

	private void RemoveTracked(long uniqueId, GameObject trackedGo)
	{
	}

	public GameObject SpawnVfx(string vfxName, float vfxDuration, Vector3 position, float scale = 1f, Vector3? direction = null, AnimationSpeed[] animSpeeds = null, bool isLoopVfx = false, float loopDuration = -1f)
	{
		return null;
	}
}
