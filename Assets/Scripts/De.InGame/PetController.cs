using De.InGame.Fsm;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class PetController : NetworkBase
{
	public class Args
	{
		public int PetId;

		public ulong OwnerClientId;
	}

	private Args _args;

	[SerializeField]
	private PetModel _petModel;

	[SerializeField]
	private PetView _petView;

	[SerializeField]
	private float _teleportDistance;

	[SerializeField]
	private float _walkStartDistance;

	[SerializeField]
	private float _walkStartDistanceInCombat;

	[SerializeField]
	private float _runStartDistance;

	[SerializeField]
	private float _runStartDistanceInCombat;

	[SerializeField]
	private float _desiredDistance;

	[SerializeField]
	private float _desiredDistanceInCombat;

	[SerializeField]
	private float _backOffDistance;

	[SerializeField]
	private float _backOffDistanceInCombat;

	[SerializeField]
	private float _walkSpeed;

	[SerializeField]
	private float _runSpeed;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameCombatSystem _combatSystem;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private SeriLogger _logger;

	private CharacterModel _characterModel;

	private CompositeDisposable _characterDisposable;

	private PetStateMachine _stateMachine;

	public PetModel PetModel => null;

	public PetView PetView => null;

	public bool IsInCombat => false;

	public float WalkSpeed => 0f;

	public float RunSpeed => 0f;

	public float WalkStartDistance => 0f;

	public float RunStartDistance => 0f;

	public float DesiredDistance => 0f;

	public float BackOffDistance => 0f;

	public Vector3 OwnerPosition => default;

	public float DistanceToOwner => 0f;

	public Subject<string> OnShowBubbleText { get; }

	public Subject<string> OnShowMonologueText { get; }

	public Subject<int> OnShowEmoji { get; }

	public void Init(Args args)
	{
	}

	public override void OnHostSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	public override void OnDestroy()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	private void InitPlayer()
	{
	}

	private void FixedUpdate()
	{
	}

	public void MoveToTarget(Vector3 position, float speed)
	{
	}

	private void OnCharacterSpawned(CharacterController characterController)
	{
	}

	private void OnCharacterDespawned(CharacterController characterController)
	{
	}

	private void SetCharacterModel(CharacterModel characterModel)
	{
	}

	private void OnCharacterStateChanged(EActionState state)
	{
	}

	private void OnCombat(bool isInCombat)
	{
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
