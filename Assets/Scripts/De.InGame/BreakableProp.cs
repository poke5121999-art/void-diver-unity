using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using Unity.Netcode;
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

	private readonly NetworkVariable<Vector3> _syncForward;

	private readonly NetworkVariable<int> _syncBreakablePropId;

	private readonly NetworkVariable<int> _remainHitsToBreak;

	private GameHost GameHost => null;

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public int HitLayer => 0;

	public bool IsHitable => false;

	public float HitRadius => 0f;

	public int BreakablePropId => 0;

	private Vector3 Forward => default;

	private TBreakableProp TableData => null;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 0f;

	public GameObjectManager ObjectManager => null;

	public GameObject GameObject => null;

	ulong IActionExecutable.NetworkObjectId => 0uL;

	public void Init(Args args)
	{
	}

	public override void OnHostSpawn()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public void OnDamage()
	{
	}

	private void DropRewardOnDestroy()
	{
	}

	private void ExecuteActionOnDestroy()
	{
	}

	private void LookAtForward()
	{
	}

	public float GetStat(EStatType statType)
	{
		return 0f;
	}



}
