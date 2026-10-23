using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using NemoLib.AssetManagement;
using System.Collections.Generic;
using GameCommon.Enum;
using GameCommon.Table;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class SpecialField : NetworkBase, IActionStats
{
	public class Args
	{
		public ulong SourceNetworkObjectId;

		public int SpecialFieldId;
	}

	private Args _args;

	[SerializeField]
	private Collider _collider;

	[SerializeField]
	private Animator _vfxAnimator;

	[SerializeField]
	private string _spawnSfx;

	[SerializeField]
	private string _destroySfx;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameEventSystem _eventSystem;

	private readonly Dictionary<ulong, UnitController> _unitControllersInField = new Dictionary<ulong, UnitController>();

	private readonly NetworkVariable<int> _syncSpecialFieldId = new NetworkVariable<int>();

	private bool _isPendingDestroy;

	private GameHost GameHost => NetworkHost as GameHost;

	public Vector3 Position => base.transform.position;

	public int SpecialFieldId => _syncSpecialFieldId.Value;

	private TSpecialField TableData => TSpecialField.Table[_syncSpecialFieldId.Value];

	public ulong SourceNetworkObjectId => _args.SourceNetworkObjectId;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 1f;

	public void Init(Args args)
	{
		if (base.IsSpawned)
		{
			UnityEngine.Debug.LogError("SpecialField.Init() should be called before spawning the object.");
			return;
		}
		_args = args;
	}

	protected override void Awake()
	{
		base.Awake();
		_collider.enabled = false;
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		_syncSpecialFieldId.Value = _args.SpecialFieldId;
		_collider.OnTriggerEnterAsObservable().Subscribe(OnColliderEnter).AddTo(this);
		_collider.OnTriggerExitAsObservable().Subscribe(OnColliderExit).AddTo(this);
		_eventSystem.OnEventOccured.Subscribe(OnGameEventOccured).AddTo(this);
		if (_spawnSfx.HasValue())
		{
			GameHost.SendToAll(new NtfPlaySfx
			{
				Sfx = _spawnSfx,
				Position = base.transform.position.ToProtoVector3()
			});
		}
		Observable.Timer(TimeSpan.FromSeconds(TableData.EffectDelay)).Subscribe(delegate
		{
			_collider.enabled = true;
		}).AddTo(base.Disposable);
		if (!(TableData.Duration <= 0f))
		{
			Observable.Timer(TimeSpan.FromSeconds(TableData.Duration + TableData.EffectDelay)).Subscribe(delegate
			{
				Despawn(immediate: false);
			}).AddTo(base.Disposable);
		}
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_objectManager.OnUnitDespawned.Subscribe(OnUnitDespawned).AddTo(this);
		_objectManager.OnSpecialFieldSpawned.OnNext(this);
	}

	public override void OnNetworkDespawn()
	{
		_isPendingDestroy = true;
		_objectManager.OnSpecialFieldDespawned.OnNext(this);
		_unitControllersInField.Values.ToList().ForEach(OnUnitExited);
		base.OnNetworkDespawn();
	}

	private void OnColliderEnter(Collider other)
	{
		if (_isPendingDestroy)
		{
			return;
		}
		AssertIfNotHost();
		UnitController component = other.GetComponent<UnitController>();
		if (component != null)
		{
			OnUnitEntered(component);
		}
		Transform parent = other.transform.parent;
		HitBox hitBox = ((parent == null) ? null : parent.GetComponent<HitBox>());
		if (hitBox != null)
		{
			if (hitBox.HasElementalType(EElementalType.Fire))
			{
				Trigger(ESpecialFieldEvent.FireHit);
			}
			if (hitBox.HasElementalType(EElementalType.Water))
			{
				Trigger(ESpecialFieldEvent.WindHit);
			}
			if (hitBox.HasElementalType(EElementalType.Wind))
			{
				Trigger(ESpecialFieldEvent.WindHit);
			}
		}
	}

	private void OnColliderExit(Collider other)
	{
		AssertIfNotHost();
		UnitController component = other.GetComponent<UnitController>();
		if (component != null && _unitControllersInField.Remove(component.NetworkObjectId))
		{
			component.HandleUnitEvent(new ExitSpecialFieldEvent
			{
				NetworkObjectId = base.NetworkObjectId
			});
		}
	}

	private void OnGameEventOccured(IBaseEvent triggerEvent)
	{
		if (triggerEvent is WaveFinishedEvent waveFinishedEvent && waveFinishedEvent.NetworkObjectId == _args.SourceNetworkObjectId)
		{
			Trigger(ESpecialFieldEvent.WaveClear);
		}
	}

	private void OnUnitDespawned(UnitController unitController)
	{
		_unitControllersInField.Remove(unitController.NetworkObjectId);
	}

	public void RescanUnit(UnitController unitController)
	{
		if (!_isPendingDestroy && _collider.enabled && (_collider.ClosestPoint(unitController.transform.position) - unitController.transform.position).sqrMagnitude <= Mathf.Epsilon)
		{
			OnUnitEntered(unitController);
		}
	}

	private void OnUnitEntered(UnitController unitController)
	{
		if (_unitControllersInField.TryAdd(unitController.NetworkObjectId, unitController))
		{
			unitController.HandleUnitEvent(new EnterSpecialFieldEvent
			{
				NetworkObjectId = base.NetworkObjectId
			});
		}
	}

	private void OnUnitExited(UnitController unitController)
	{
		if (_unitControllersInField.Remove(unitController.NetworkObjectId))
		{
			unitController.HandleUnitEvent(new ExitSpecialFieldEvent
			{
				NetworkObjectId = base.NetworkObjectId
			});
		}
	}

	private void Trigger(ESpecialFieldEvent eventType)
	{
		if (_isPendingDestroy)
		{
			return;
		}
		AssertIfNotHost();
		if (TableData.DestroyTrigger != eventType)
		{
			return;
		}
		if (_destroySfx.HasValue())
		{
			GameHost.SendToAll(new NtfPlaySfx
			{
				Sfx = _destroySfx,
				Position = base.transform.position.ToProtoVector3()
			});
		}
		TableData.HitBoxIdsOnDestroy?.ForEach(delegate(int id)
		{
			_objectManager.SpawnHitBoxAsync(id, null, this.ToStatsData(), new HitLayerInfo
			{
				HitLayer = 4000
			}, base.transform.forward, base.transform.position).Forget();
		});
		Despawn(TableData.DestroyImmediatelyOnTrigger);
	}

	private void Despawn(bool immediate)
	{
		AssertIfNotHost();
		if (!immediate && _vfxAnimator != null)
		{
			if (!_isPendingDestroy)
			{
				_isPendingDestroy = true;
				_vfxAnimator.SetTrigger("EndTrigger");
			}
		}
		else
		{
			Despawn(0f, destroy: true);
		}
	}

	public int GetBuffId(UnitModel targetModel)
	{
		if (!(targetModel is CharacterModel))
		{
			if (targetModel is MonsterModel)
			{
				return TableData.MonsterBuffId;
			}
			return 0;
		}
		return TableData.CharacterBuffId;
	}

	public int GetStackBuffId(UnitModel targetModel)
	{
		if (!(targetModel is CharacterModel))
		{
			if (targetModel is MonsterModel)
			{
				return TableData.MonsterStackBuffId;
			}
			return 0;
		}
		return TableData.CharacterStackBuffId;
	}

	public float GetStackBuffInterval(UnitModel targetModel)
	{
		if (!(targetModel is CharacterModel))
		{
			if (targetModel is MonsterModel)
			{
				return TableData.MonsterStackBuffInterval;
			}
			return 0f;
		}
		return TableData.CharacterStackBuffInterval;
	}

	public bool GetPreventStackBuffExpire(UnitModel targetModel)
	{
		if (!(targetModel is CharacterModel))
		{
			if (targetModel is MonsterModel)
			{
				return TableData.PreventMonsterStackBuffExpire;
			}
			return false;
		}
		return TableData.PreventCharacterStackBuffExpire;
	}

	private void OnAnimationEnded()
	{
		Despawn(0f, destroy: true);
	}

	public float GetStat(EStatType statType)
	{
		if (statType != EStatType.Atk)
		{
			return 0f;
		}
		return TableData.Atk;
	}



}
