using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class DisposableObject : NetworkBase, IInteractiveObject
{
	public class Args
	{
		public int DisposableObjectId;

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

	private Args _args;

	private readonly NetworkVariable<Vector3> _syncForward = new NetworkVariable<Vector3>();

	private readonly NetworkVariable<int> _syncDisposableObjectId = new NetworkVariable<int>();

	private readonly NetworkVariable<bool> _syncIsInteracting = new NetworkVariable<bool>();

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public float HoldingTime => TableData.HoldingTime;

	public EOutlineType OutlineType => EOutlineType.Default;

	public string HoldingSfx => _holdingSfx;

	public string InteractionSfx => _interactionSfx;

	public int DisposableObjectId => _syncDisposableObjectId.Value;

	private Vector3 Forward => _syncForward.Value;

	private TDisposableObject TableData => TDisposableObject.Table[_syncDisposableObjectId.Value];

	ulong IInteractiveObject.NetworkObjectId => base.NetworkObjectId;

	public void Init(Args args)
	{
		if (base.IsSpawned)
		{
			_logger.Error("RewardBox.Init() should be called before spawning the object.");
			return;
		}
		_args = args;
	}

	public override void OnHostSpawn()
	{
		_syncDisposableObjectId.Value = _args.DisposableObjectId;
		_syncForward.Value = _args.Forward;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
		InGameUtility.LookAtForward(_meshType, _syncForward.Value, _frontObject, _backObject);
	}

	public override void OnNetworkDespawn()
	{
		_objectManager.OnInteractiveObjectDespawned.OnNext(this);
		base.OnNetworkDespawn();
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		return !_syncIsInteracting.Value;
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
		if (base.IsHost)
		{
			_syncIsInteracting.Value = true;
		}
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
		if (!base.IsHost)
		{
			return;
		}
		if (TableData.DropRewardGroupId > 0)
		{
			DropRewardTable table = TDropReward.Table;
			int campaignId = _gameContext.CampaignId;
			int[] missionIds = _missionManager.MissionIds.ToArray();
			EDifficulty difficulty = _gameContext.Difficulty;
			var partyWeaponTypes = _gameContext.GetPartyWeaponTypes();
			foreach (Goods goods in table.PickRewards(TableData.DropRewardGroupId, campaignId, missionIds, difficulty, partyWeaponTypes))
			{
				_objectManager.SpawnDropGoods(goods, base.transform.position);
			}
		}
		if (TableData.AggroSpreadRange > 0f)
		{
			CharacterModel sourceModel = _gameManager.CharacterModels.FirstOrDefault((CharacterModel c) => c.ClientId == sourceClientId);
			foreach (UnitModel unit in _objectManager.UnitModels
				.Where((UnitModel u) => u is MonsterModel)
				.Where((UnitModel u) => Vector3.Distance(base.transform.position, u.transform.position) <= TableData.AggroSpreadRange))
			{
				unit.GetComponent<MonsterController>().HandleUnitEvent(new ReceiveAggroSpreadEvent
				{
					Target = sourceModel,
					ChainDepth = 0
				});
			}
		}
		base.NetworkObject.Despawn(destroy: true);
	}

	private void LookAtForward()
	{
		InGameUtility.LookAtForward(_meshType, _syncForward.Value, _frontObject, _backObject);
	}



}
