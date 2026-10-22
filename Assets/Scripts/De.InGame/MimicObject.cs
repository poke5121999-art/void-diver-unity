using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class MimicObject : NetworkBase, IInteractiveObject, IActionStats
{
	private enum EQteZone
	{
		Cancel = 0,
		Success = 1,
		Fail = 2
	}

	private enum AnimationState
	{
		None = 0,
		Idle = 1,
		Triggered = 2
	}

	public class Args
	{
		public int MimicId;

		public Vector3 Forward;
	}

	private class QteHoldingState
	{
		public float StartTime;

		public CompositeDisposable Disposable;
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CDespawnMimicDelayedAsync_003Ed__60 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public MimicObject _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CSpawnMonsterDelayedAsync_003Ed__58 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public MimicObject _003C_003E4__this;

		public int monsterId;

		public Vector3 spawnPosition;

		public ulong summonerNetworkObjectId;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[SerializeField]
	private EMeshType _meshType;

	[SerializeField]
	private GameObject _frontObject;

	[SerializeField]
	private GameObject _backObject;

	[SerializeField]
	private Animator _frontAnimator;

	[SerializeField]
	private Animator _backAnimator;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private SeriLogger _logger;

	private readonly NetworkVariable<int> _syncMimicId;

	private readonly NetworkVariable<Vector3> _syncForward;

	private readonly NetworkVariable<bool> _syncMonsterSpawnTriggered;

	private Args _args;

	private readonly Dictionary<ulong, QteHoldingState> _qteStates;

	private CompositeDisposable _zoneSfxDisposable;

	private EQteZone _currentZone;

	private OutlineObject _outlineObject;

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public float HoldingTime => 0f;

	public EOutlineType OutlineType => EOutlineType.None;

	public string HoldingSfx => null;

	public string InteractionSfx => null;

	public int MimicId => 0;

	public Vector3 Forward => default;

	public TMimic TableData => null;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 0f;

	ulong IInteractiveObject.NetworkObjectId => 0uL;

	protected override void Awake()
	{
	}

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

	private void OnMonsterSpawnTriggeredChanged(bool oldValue, bool newValue)
	{
	}

	private void UpdateOutline()
	{
	}

	private void LookAtForward()
	{
	}

	private void SetAnimation(AnimationState state)
	{
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		return false;
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
	}

	private void OnInteractStateChanged(ulong sourceClientId)
	{
	}

	private void StartZoneSfxTracking(ulong sourceClientId)
	{
	}

	private void UpdateZoneSfx(float startTime)
	{
	}

	private void StopZoneSfxTracking()
	{
	}

	private void SpawnMonster()
	{
	}

	[AsyncStateMachine(typeof(_003CSpawnMonsterDelayedAsync_003Ed__58))]
	private UniTask SpawnMonsterDelayedAsync(int monsterId, Vector3 spawnPosition, ulong summonerNetworkObjectId)
	{
		return default;
	}

	private void DisableNavMeshObstacle()
	{
	}

	[AsyncStateMachine(typeof(_003CDespawnMimicDelayedAsync_003Ed__60))]
	private UniTask DespawnMimicDelayedAsync()
	{
		return default;
	}

	private void SpawnHitBoxes()
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
