using GameCommon.Table;
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
	private float _teleportDistance = 12.1f;

	[SerializeField]
	private float _walkStartDistance = 1.8f;

	[SerializeField]
	private float _walkStartDistanceInCombat = 3f;

	[SerializeField]
	private float _runStartDistance = 4f;

	[SerializeField]
	private float _runStartDistanceInCombat = 6f;

	[SerializeField]
	private float _desiredDistance = 1.2f;

	[SerializeField]
	private float _desiredDistanceInCombat = 2.5f;

	[SerializeField]
	private float _backOffDistance = 0.6f;

	[SerializeField]
	private float _backOffDistanceInCombat = 2f;

	[SerializeField]
	private float _walkSpeed = 1.8f;

	[SerializeField]
	private float _runSpeed = 4f;

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

	public PetModel PetModel => _petModel;

	public PetView PetView => _petView;

	public bool IsInCombat => _combatSystem.IsInCombat;

	public float WalkSpeed => _walkSpeed;

	public float RunSpeed => _runSpeed;

	public float WalkStartDistance => _combatSystem.IsInCombat ? _walkStartDistanceInCombat : _walkStartDistance;

	public float RunStartDistance => _combatSystem.IsInCombat ? _runStartDistanceInCombat : _runStartDistance;

	public float DesiredDistance => _combatSystem.IsInCombat ? _desiredDistanceInCombat : _desiredDistance;

	public float BackOffDistance => _combatSystem.IsInCombat ? _backOffDistanceInCombat : _backOffDistance;

	public Vector3 OwnerPosition => (_characterModel != null) ? _characterModel.transform.position : Vector3.zero;

	public float DistanceToOwner
	{
		get
		{
			if (!(_characterModel != null))
			{
				return 0f;
			}
			return Vector3.Distance(base.transform.position, _characterModel.transform.position);
		}
	}

	public Subject<string> OnShowBubbleText { get; } = new Subject<string>();

	public Subject<string> OnShowMonologueText { get; } = new Subject<string>();

	public Subject<int> OnShowEmoji { get; } = new Subject<int>();

	public void Init(Args args)
	{
		_args = args;
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		_petModel.SyncPetId.Value = _args.PetId;
		_petModel.SyncOwnerClientId.Value = _args.OwnerClientId;
	}

	public override void OnNetworkDespawn()
	{
		_objectManager.OnPetDespawned.OnNext(this);
		base.OnNetworkDespawn();
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_characterDisposable?.Dispose();
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_objectManager.OnPetSpawned.OnNext(this);
		ulong value = _petModel.SyncOwnerClientId.Value;
		if (_gameContext.Players.TryGetValue(value, out var _))
		{
			InitPlayer();
		}
		else
		{
			_logger.Error(string.Format("Player with clientId {0} not found.", value));
		}
	}

	private void InitPlayer()
	{
		ulong clientId = 0uL;
		base.gameObject.name = string.Format("[Pet] {0}", _petModel.SyncPetId.Value);
		_petModel.Init();
		_petView.Init();
		if (IsHost)
		{
			_stateMachine = new PetStateMachine(this);
		}
		_combatSystem.RxIsInCombat.Subscribe(OnCombat).AddTo(this);
		clientId = _petModel.SyncOwnerClientId.Value;
		SetCharacterModel(_gameManager.FindCharacterModel(clientId));
		_gameManager.OnCharacterSpawned.Where((CharacterController c) => c.ClientId == clientId).Subscribe(OnCharacterSpawned).AddTo(this);
		_gameManager.OnCharacterDespawned.Where((CharacterController c) => c.ClientId == clientId).Subscribe(OnCharacterDespawned).AddTo(this);
	}

	private void FixedUpdate()
	{
		if (IsHost)
		{
			if (_characterModel == null)
			{
				return;
			}
			if (DistanceToOwner > _teleportDistance)
			{
				_stateMachine.TriggerEvent(new PetTeleportEvent());
			}
			_stateMachine.OnUpdate();
		}
	}

	public void MoveToTarget(Vector3 position, float speed)
	{
		Vector3 vector = position - base.transform.position;
		vector.y = 0f;
		_petModel.Forward = vector.normalized;
		_petView.UpdateAgentPath(position, speed);
	}

	private void OnCharacterSpawned(CharacterController characterController)
	{
		SetCharacterModel(characterController.GetComponent<CharacterModel>());
	}

	private void OnCharacterDespawned(CharacterController characterController)
	{
		SetCharacterModel(null);
	}

	private void SetCharacterModel(CharacterModel characterModel)
	{
		_characterDisposable?.Dispose();
		_characterModel = characterModel;
		if (characterModel != null)
		{
			_characterDisposable = new CompositeDisposable();
			characterModel.RxState.Subscribe(OnCharacterStateChanged).AddTo(_characterDisposable);
		}
	}

	private void OnCharacterStateChanged(EActionState state)
	{
		if (state == EActionState.Revive)
		{
			OnShowEmoji.OnNext(2);
		}
		else if (state == EActionState.Dead)
		{
			OnShowEmoji.OnNext(5);
		}
	}

	private void OnCombat(bool isInCombat)
	{
		if (!isInCombat || _gameManager.IsInLounge)
		{
			return;
		}
		TPet tPet = TPet.Table[_petModel.RxPetId.Value];
		OnShowBubbleText.OnNext(tPet.CombatText);
		_soundManager.PlaySfx(tPet.CombatSfx);
		if (IsHost)
		{
			_stateMachine.TriggerEvent(new PetCombatEnterEvent());
		}
	}
}
