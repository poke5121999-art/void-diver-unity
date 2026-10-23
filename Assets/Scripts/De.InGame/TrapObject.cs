using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public abstract class TrapObject : NetworkBase, IActionStats
{
	public class Args
	{
		public int TrapId;

		public Vector3 Forward;
	}

	[SerializeField]
	private EMeshType _meshType;

	[SerializeField]
	private GameObject _frontObject;

	[SerializeField]
	private GameObject _backObject;

	[Inject]
	protected GameManager GameManager;

	[Inject]
	protected GameObjectManager ObjectManager;

	[Inject]
	protected SeriLogger Logger;

	[Inject]
	protected GameContext GameContext;

	private Args _args;

	private readonly NetworkVariable<bool> _isActivated = new NetworkVariable<bool>();

	private float _coolEndTime;

	private float _nextCheckTime;

	private readonly NetworkVariable<Vector3> _syncForward = new NetworkVariable<Vector3>();

	private readonly NetworkVariable<int> _syncTrapId = new NetworkVariable<int>();

	private float TimeNow => GameManager.TimeNow;

	public int TrapId => _syncTrapId.Value;

	private Vector3 Forward => _syncForward.Value;

	private TTrap TrapData => TTrap.Table[_syncTrapId.Value];

	private ETeamSide TeamSide => TrapData.TeamSide;

	private int VariantDifficulty => GameContext.VariantDifficulty;

	private int HitLayer
	{
		get
		{
			switch (TrapData.TeamSide)
			{
			case ETeamSide.None:
				return 4000;
			case ETeamSide.Character:
				return 1000;
			case ETeamSide.Monster:
				return 2000;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 1f;

	protected abstract void OnIsActivatedValueChanged(bool oldValue, bool newValue);

	public void Init(Args args)
	{
		AssertIfNotHost();
		_args = args;
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		_syncTrapId.Value = _args.TrapId;
		_syncForward.Value = _args.Forward;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_isActivated.OnValueChanged += OnIsActivatedValueChanged;
		ObjectManager.OnTrapObjectSpawned.OnNext(this);
		LookAtForward();
	}

	public override void OnNetworkDespawn()
	{
		_isActivated.OnValueChanged -= OnIsActivatedValueChanged;
		ObjectManager.OnTrapObjectDespawned.OnNext(this);
		base.OnNetworkDespawn();
	}

	private void LookAtForward()
	{
		switch (_meshType)
		{
		case EMeshType.D25:
			LookAsForwardAsD25();
			break;
		case EMeshType.D3:
			_frontObject.transform.forward = _syncForward.Value;
			break;
		}

		void LookAsForwardAsD25()
		{
			Vector3 vector = MathUtility.SnapToCardinalDirection(_syncForward.Value);
			if (vector == Vector3.forward)
			{
				_frontObject.SetActive(value: true);
				_backObject.SetActive(value: false);
			}
			if (vector == Vector3.back)
			{
				_frontObject.SetActive(value: false);
				_backObject.SetActive(value: true);
			}
			if (vector == Vector3.left)
			{
				_frontObject.SetActive(value: true);
				_backObject.SetActive(value: false);
				_frontObject.transform.Flip25D();
			}
			if (vector == Vector3.right)
			{
				_frontObject.SetActive(value: false);
				_backObject.SetActive(value: true);
				_backObject.transform.Flip25D();
			}
		}
	}

	private void Update()
	{
		if (base.IsHost && !(_nextCheckTime > TimeNow))
		{
			_nextCheckTime = TimeNow + 0.2f;
			if (CheckExecutable())
			{
				ExecuteAction();
			}
		}
	}

	protected virtual bool CheckExecutable()
	{
		if (TimeNow < _coolEndTime)
		{
			return false;
		}
		return GameManager.CharacterModels.Where((CharacterModel characterModel) => characterModel.State != EActionState.Dead).Where((CharacterModel characterModel) => characterModel.State != EActionState.Escaped).Any((CharacterModel characterModel) => (characterModel.Position - base.transform.position).magnitude <= 10f);
	}

	private void ExecuteAction()
	{
		AssertIfNotHost();
		if (!CheckExecutable())
		{
			return;
		}
		float actionDuration = TrapData.ActionDuration;
		float coolTime = TrapData.CoolTime;
		_isActivated.Value = true;
		_coolEndTime = TimeNow + coolTime;
		Observable.Timer(TimeSpan.FromSeconds(actionDuration)).Subscribe(delegate
		{
			_isActivated.Value = false;
		}).AddTo(this);
		SpawnHitBox();
		if (TrapData.DestroyAfterAction)
		{
			base.NetworkObject.Despawn();
		}
	}

	private void SpawnHitBox()
	{
		if (TrapData.HitBoxIds == null)
		{
			return;
		}
		Vector3 direction = _syncForward.Value;
		TrapData.HitBoxIds.ForEach(delegate(int id)
		{
			ObjectManager.SpawnHitBoxAsync(id, null, this.ToStatsData(), new HitLayerInfo
			{
				HitLayer = HitLayer,
				FactionId = 0
			}, direction, base.transform.position).Forget();
		});
	}

	public float GetStat(EStatType statType)
	{
		if (statType != EStatType.Atk)
		{
			return 0f;
		}
		return TrapData.GetBaseAtkStat(GameContext.Difficulty, GameContext.VariantDifficulty);
	}



}
