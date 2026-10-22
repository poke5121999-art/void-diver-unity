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

	private readonly NetworkVariable<bool> _isActivated;

	private float _coolEndTime;

	private float _nextCheckTime;

	private readonly NetworkVariable<Vector3> _syncForward;

	private readonly NetworkVariable<int> _syncTrapId;

	private float TimeNow => 0f;

	public int TrapId => 0;

	private Vector3 Forward => default;

	private TTrap TrapData => null;

	private ETeamSide TeamSide => ETeamSide.None;

	private int VariantDifficulty => 0;

	private int HitLayer => 0;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 0f;

	protected abstract void OnIsActivatedValueChanged(bool oldValue, bool newValue);

	public void Init(Args args)
	{
	}

	public override void OnHostSpawn()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	private void LookAtForward()
	{
	}

	private void Update()
	{
	}

	protected virtual bool CheckExecutable()
	{
		return false;
	}

	private void ExecuteAction()
	{
	}

	private void SpawnHitBox()
	{
	}

	public float GetStat(EStatType statType)
	{
		return 0f;
	}

	protected override void __initializeVariables()
	{
	}

	protected override void __initializeRpcs()
	{
	}

	protected internal override string __getTypeName()
	{
		return null;
	}
}
