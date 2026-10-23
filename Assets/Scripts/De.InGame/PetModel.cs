using System;
using NemoLib.Log;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class PetModel : NetworkBase
{
	[Inject]
	private SeriLogger _logger;

	public NetworkVariable<int> SyncPetId = new NetworkVariable<int>();

	public NetworkVariable<ulong> SyncOwnerClientId = new NetworkVariable<ulong>(ulong.MaxValue);

	private NetworkVariable<PlanarPos> _syncPosition = new NetworkVariable<PlanarPos>();

	private NetworkVariable<PackedDir> _syncForward = new NetworkVariable<PackedDir>();

	private NetworkVariable<float> _syncSpeed = new NetworkVariable<float>();

	private NetworkVariable<EPetState> _syncState = new NetworkVariable<EPetState>();

	private readonly ReactiveProperty<int> _rxPetId = new ReactiveProperty<int>();

	private readonly ReactiveProperty<ulong> _rxOwnerClientId = new ReactiveProperty<ulong>();

	private readonly ReactiveProperty<Vector3> _rxPosition = new ReactiveProperty<Vector3>();

	private readonly ReactiveProperty<Vector3> _rxForward = new ReactiveProperty<Vector3>();

	private readonly ReactiveProperty<float> _rxSpeed = new ReactiveProperty<float>();

	private readonly ReactiveProperty<EPetState> _rxState = new ReactiveProperty<EPetState>();

	public IReadOnlyReactiveProperty<int> RxPetId => _rxPetId;

	public IReadOnlyReactiveProperty<ulong> RxOwnerClientId => _rxOwnerClientId;

	public IReadOnlyReactiveProperty<Vector3> RxPosition => _rxPosition;

	public IReadOnlyReactiveProperty<Vector3> RxForward => _rxForward;

	public IReadOnlyReactiveProperty<float> RxSpeed => _rxSpeed;

	public IReadOnlyReactiveProperty<EPetState> RxState => _rxState;

	public int PetId
	{
		get
		{
			return _rxPetId.Value;
		}
		set
		{
			SetPetNetworkVariable(ref SyncPetId, value);
		}
	}

	public ulong TargetClientId
	{
		get
		{
			return _rxOwnerClientId.Value;
		}
		set
		{
			SetPetNetworkVariable(ref SyncOwnerClientId, value);
		}
	}

	public Vector3 Position
	{
		get
		{
			return _rxPosition.Value;
		}
		set
		{
			SetPetNetworkVariable(ref _syncPosition, new PlanarPos(value));
		}
	}

	public Vector3 Forward
	{
		get
		{
			return _rxForward.Value;
		}
		set
		{
			SetPetNetworkVariable(ref _syncForward, new PackedDir(value));
		}
	}

	public float Speed
	{
		get
		{
			return _rxSpeed.Value;
		}
		set
		{
			SetPetNetworkVariable(ref _syncSpeed, value);
		}
	}

	public EPetState State
	{
		get
		{
			return _rxState.Value;
		}
		set
		{
			SetPetNetworkVariable(ref _syncState, value);
		}
	}

	public void Init()
	{
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		SetPetNetworkVariable(ref _syncPosition, new PlanarPos(base.transform.position));
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		if (!IsHost)
		{
			_rxPosition.Value = _syncPosition.Value.ToVector3();
			base.transform.position = _syncPosition.Value.ToVector3();
		}
		RegisterVariableEvents();
	}

	public override void OnNetworkDespawn()
	{
		UnregisterVariableEvents();
		base.OnNetworkDespawn();
	}

	private void RegisterVariableEvents()
	{
		RegisterVariableEvent(SyncPetId, _rxPetId);
		RegisterVariableEvent(SyncOwnerClientId, _rxOwnerClientId);
		RegisterVariableEvent(_syncPosition, _rxPosition, (PlanarPos v) => v.ToVector3());
		RegisterVariableEvent(_syncForward, _rxForward, (PackedDir v) => v.ToVector3());
		RegisterVariableEvent(_syncSpeed, _rxSpeed);
		RegisterVariableEvent(_syncState, _rxState);
	}

	private void UnregisterVariableEvents()
	{
		UnregisterVariableEvent(SyncPetId);
		UnregisterVariableEvent(SyncOwnerClientId);
		UnregisterVariableEvent(_syncPosition);
		UnregisterVariableEvent(_syncForward);
		UnregisterVariableEvent(_syncSpeed);
		UnregisterVariableEvent(_syncState);
	}

	private void SetPetNetworkVariable<T>(ref NetworkVariable<T> networkVariable, T value) where T : struct
	{
		// TODO(decomp): generic body has no address (0x0); host-only write inferred from callers @0x0
		if (IsHost && networkVariable != null)
		{
			networkVariable.Value = value;
		}
	}

	private void RegisterVariableEvent<T>(NetworkVariable<T> syncVar, ReactiveProperty<T> rxVar) where T : struct
	{
		// TODO(decomp): generic body has no address (0x0); mirrors the decode overload with identity @0x0
		rxVar.Value = syncVar.Value;
		syncVar.OnValueChanged += delegate(T oldValue, T newValue)
		{
			rxVar.Value = newValue;
		};
	}

	private void RegisterVariableEvent<TWire, TValue>(NetworkVariable<TWire> syncVar, ReactiveProperty<TValue> rxVar, Func<TWire, TValue> decode) where TWire : struct
	{
		// TODO(decomp): generic body has no address (0x0); reconstructed from <>c__DisplayClass51_0`2 shape @0x0
		rxVar.Value = decode(syncVar.Value);
		syncVar.OnValueChanged += delegate(TWire oldValue, TWire newValue)
		{
			rxVar.Value = decode(newValue);
		};
	}

	private void UnregisterVariableEvent<T>(NetworkVariable<T> syncVar) where T : struct
	{
		// TODO(decomp): generic body has no address (0x0); handlers are lambdas so the delegate list is cleared @0x0
		if (syncVar != null)
		{
			syncVar.OnValueChanged = null;
		}
	}
}
