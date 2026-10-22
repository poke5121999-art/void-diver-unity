using System;
using System.Collections.Generic;
using GameCommon.Data;
using NemoLib.AssetManagement;
using NemoLib.Log;
using Cysharp.Threading.Tasks;
using NemoLib;
using UniRx;
using UniRx.Triggers;
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

	private Dictionary<long, GameObject> _vfxDic = new Dictionary<long, GameObject>();

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameChannel.Receive<NtfDisplayFx>().Subscribe(OnDisplayFx).AddTo(this);
		_gameChannel.Receive<NtfRemoveFx>().Subscribe(OnRemoveFx).AddTo(this);
	}

	private void OnDisplayFx(NtfDisplayFx message)
	{
		Vector3 position = ((message.Position != null) ? message.Position.ToVector3() : Vector3.zero);
		Vector3 value = ((message.Direction != null) ? message.Direction.ToVector3() : Vector3.zero);
		if (string.IsNullOrEmpty(message.Vfx))
		{
			return;
		}
		GameObject vfxGo = SpawnVfx(message.Vfx, message.VfxDuration, position, message.VfxScale, value, message.AnimSpeeds, message.IsLoopVfx, message.LoopDuration);
		if (vfxGo == null)
		{
			return;
		}
		if (message.TargetNetworkObjectId != 0)
		{
			UnitModel unitModel = _objectManager.FindUnitModel(message.TargetNetworkObjectId);
			if (!(unitModel != null))
			{
				_objectManager.OnUnitSpawned.Where((UnitController controller) => controller.NetworkObjectId == message.TargetNetworkObjectId).Take(1).TakeUntil(GetVfxLifetimeEnd(vfxGo))
					.Subscribe(delegate(UnitController controller)
					{
						AttachUnitFollower(vfxGo, message, controller.GetComponent<UnitModel>());
					})
					.AddTo(this);
			}
			else
			{
				AttachUnitFollower(vfxGo, message, unitModel);
			}
		}
		if (message.UniqueId > 0)
		{
			long uniqueId = message.UniqueId;
			_vfxDic[uniqueId] = vfxGo;
			PoolableObject component = vfxGo.GetComponent<PoolableObject>();
			if (!(component != null))
			{
				vfxGo.OnDestroyAsObservable().Subscribe(delegate
				{
					RemoveTracked(uniqueId, vfxGo);
				}).AddTo(vfxGo);
			}
			else
			{
				component.OnDespawned.Take(1).Subscribe(delegate
				{
					RemoveTracked(uniqueId, vfxGo);
				}).AddTo(this);
			}
		}
		if (message.SkillOwnerNetworkObjectId != 0)
		{
			UnitModel unitModel2 = _objectManager.FindUnitModel(message.SkillOwnerNetworkObjectId);
			if (!(unitModel2 == null))
			{
				SkillVfx component2 = vfxGo.GetComponent<SkillVfx>();
				if (component2 != null)
				{
					component2.Init(unitModel2.gameObject, null, Vector3.zero);
				}
			}
		}
	}

	private void AttachUnitFollower(GameObject vfxGo, NtfDisplayFx message, UnitModel targetUnit)
	{
		if (!(vfxGo == null) && !(targetUnit == null))
		{
			UnitFollower orAddComponent = vfxGo.GetOrAddComponent<UnitFollower>();
			orAddComponent.Init(targetUnit, message.BoneType, (message.TargetPositionOffset != null) ? message.TargetPositionOffset.ToVector3() : Vector3.zero, message.InheritMoveDir, message.DestroyOnUnitFollowerTargetDeath, message.UpdateByAimDir);
		}
	}

	private IObservable<Unit> GetVfxLifetimeEnd(GameObject vfxGo)
	{
		PoolableObject component = vfxGo.GetComponent<PoolableObject>();
		if (component != null)
		{
			return component.OnDespawned.AsUnitObservable();
		}
		if (vfxGo == null)
		{
			return Observable.ReturnUnit();
		}
		return vfxGo.GetOrAddComponent<ObservableDestroyTrigger>().OnDestroyAsObservable();
	}

	private void OnRemoveFx(NtfRemoveFx message)
	{
		if (_vfxDic.TryGetValue(message.UniqueId, out var value))
		{
			value.DespawnOrDestroy();
		}
	}

	private void RemoveTracked(long uniqueId, GameObject trackedGo)
	{
		if (_vfxDic.TryGetValue(uniqueId, out var value) && value == trackedGo)
		{
			_vfxDic.Remove(uniqueId);
		}
	}

	public GameObject SpawnVfx(string vfxName, float vfxDuration, Vector3 position, float scale = 1f, Vector3? direction = null, AnimationSpeed[] animSpeeds = null, bool isLoopVfx = false, float loopDuration = -1f)
	{
		GameObject gameObject = _assetManager.SpawnVfx(vfxName);
		if (gameObject == null)
		{
			Debug.LogWarning("Vfx is not exist: " + vfxName);
			return null;
		}
		gameObject.transform.position = position;
		gameObject.transform.localScale = Vector3.one * scale;
		if (direction.HasValue && direction.Value != Vector3.zero)
		{
			gameObject.transform.rotation = Quaternion.LookRotation(direction.Value, Vector3.up);
		}
		if (isLoopVfx || (animSpeeds != null && animSpeeds.Length > 0))
		{
			gameObject.GetOrAddComponent<SpeedVfxObject>().SetSpeed(animSpeeds, isLoopVfx, loopDuration).Forget();
		}
		if (vfxDuration >= 0f)
		{
			gameObject.DespawnOrDestroy(vfxDuration);
		}
		return gameObject;
	}
}
