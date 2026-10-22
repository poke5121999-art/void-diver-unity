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

	private readonly Dictionary<ulong, UnitController> _unitControllersInField;

	private readonly NetworkVariable<int> _syncSpecialFieldId;

	private bool _isPendingDestroy;

	private GameHost GameHost => null;

	public Vector3 Position => default;

	public int SpecialFieldId => 0;

	private TSpecialField TableData => null;

	public ulong SourceNetworkObjectId => 0uL;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 0f;

	public void Init(Args args)
	{
	}

	protected override void Awake()
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

	private void OnColliderEnter(Collider other)
	{
	}

	private void OnColliderExit(Collider other)
	{
	}

	private void OnGameEventOccured(IBaseEvent triggerEvent)
	{
	}

	private void OnUnitDespawned(UnitController unitController)
	{
	}

	public void RescanUnit(UnitController unitController)
	{
	}

	private void OnUnitEntered(UnitController unitController)
	{
	}

	private void OnUnitExited(UnitController unitController)
	{
	}

	private void Trigger(ESpecialFieldEvent eventType)
	{
	}

	private void Despawn(bool immediate)
	{
	}

	public int GetBuffId(UnitModel targetModel)
	{
		return 0;
	}

	public int GetStackBuffId(UnitModel targetModel)
	{
		return 0;
	}

	public float GetStackBuffInterval(UnitModel targetModel)
	{
		return 0f;
	}

	public bool GetPreventStackBuffExpire(UnitModel targetModel)
	{
		return false;
	}

	private void OnAnimationEnded()
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
