using System;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using Unity.Netcode;
using UniRx;
using UnityEngine;
using VContainer;

public class BreakableProp : NetworkBase, IHitable, IActionExecutable, IActionStats
{
	public class Args
	{
		public int BreakablePropId;

		public Vector3 Forward;
	}

	[SerializeField]
	private EMeshType _meshType;

	[SerializeField]
	private GameObject _frontObject;

	[SerializeField]
	private GameObject _backObject;

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[SerializeField]
	private string _hitSfx;

	[SerializeField]
	private string _destroySfx;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private GameSoundManager _soundManager;

	private Args _args;

	private readonly NetworkVariable<Vector3> _syncForward = new NetworkVariable<Vector3>(default(Vector3));

	private readonly NetworkVariable<int> _syncBreakablePropId = new NetworkVariable<int>(0);

	private readonly NetworkVariable<int> _remainHitsToBreak = new NetworkVariable<int>(0);

	private GameHost GameHost => NetworkHost as GameHost;

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public int HitLayer => 3000;

	public bool IsHitable => true;

	public float HitRadius => 0f;

	public int BreakablePropId => _syncBreakablePropId.Value;

	private Vector3 Forward => _syncForward.Value;

	private TBreakableProp TableData => TBreakableProp.Table[_syncBreakablePropId.Value];

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 1f;

	public GameObjectManager ObjectManager => _objectManager;

	public GameObject GameObject => base.gameObject;

	ulong IActionExecutable.NetworkObjectId => base.NetworkObjectId;

	public void Init(Args args)
	{
		if (base.IsSpawned)
		{
			_logger.Error("RewardBox.Init() should be called before spawning the object.");
		}
		else
		{
			_args = args;
		}
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		_syncBreakablePropId.Value = _args.BreakablePropId;
		_syncForward.Value = _args.Forward;
		_remainHitsToBreak.Value = TableData.HitsToBreak;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		LookAtForward();
	}

	public void OnDamage()
	{
		AssertIfNotHost();
		ProtoVector3 position = base.transform.position.ToProtoVector3();
		_remainHitsToBreak.Value--;
		if (_remainHitsToBreak.Value < 1)
		{
			DropRewardOnDestroy();
			ExecuteActionOnDestroy();
			if (_destroySfx.HasValue())
			{
				GameHost.SendToAll(new NtfPlaySfx
				{
					Sfx = _destroySfx,
					Position = position
				});
			}
			base.gameObject.SetActive(value: false);
			ActionEvent[] actionEventsOnDestroy = TableData.ActionEventsOnDestroy;
			float delay = ((actionEventsOnDestroy == null) ? 0f : actionEventsOnDestroy.Max((ActionEvent _) => _.startTime + 0.1f));
			Despawn(delay);
		}
		else if (_hitSfx.HasValue())
		{
			GameHost.SendToAll(new NtfPlaySfx
			{
				Sfx = _hitSfx,
				Position = position
			});
		}
	}

	private void DropRewardOnDestroy()
	{
		if (TableData.DropRewardGroupId < 1)
		{
			return;
		}
		TDropReward.Table.PickRewards(TableData.DropRewardGroupId, _gameContext.CampaignId, _missionManager.MissionIds.ToArray(), _gameContext.Difficulty, _gameContext.GetPartyWeaponTypes()).ForEach(delegate(Goods _)
		{
			_objectManager.SpawnDropGoods(_, base.transform.position);
		});
	}

	private void ExecuteActionOnDestroy()
	{
		if (TableData.ActionEventsOnDestroy == null)
		{
			return;
		}
		HitLayerInfo hitLayerInfo = this.GetHitLayerInfo();
		TableData.ActionEventsOnDestroy.ForEach(delegate(ActionEvent _)
		{
			Observable.Timer(TimeSpan.FromSeconds(_.startTime)).Subscribe(delegate
			{
				this.ExecuteAction(_, hitLayerInfo);
			}).AddTo(Disposable);
		});
	}

	private void LookAtForward()
	{
		InGameUtility.LookAtForward(_meshType, _syncForward.Value, _frontObject, _backObject);
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
