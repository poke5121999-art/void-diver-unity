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

	public NetworkVariable<int> SyncPetId;

	public NetworkVariable<ulong> SyncOwnerClientId;

	private NetworkVariable<PlanarPos> _syncPosition;

	private NetworkVariable<PackedDir> _syncForward;

	private NetworkVariable<float> _syncSpeed;

	private NetworkVariable<EPetState> _syncState;

	private readonly ReactiveProperty<int> _rxPetId;

	private readonly ReactiveProperty<ulong> _rxOwnerClientId;

	private readonly ReactiveProperty<Vector3> _rxPosition;

	private readonly ReactiveProperty<Vector3> _rxForward;

	private readonly ReactiveProperty<float> _rxSpeed;

	private readonly ReactiveProperty<EPetState> _rxState;

	public IReadOnlyReactiveProperty<int> RxPetId => null;

	public IReadOnlyReactiveProperty<ulong> RxOwnerClientId => null;

	public IReadOnlyReactiveProperty<Vector3> RxPosition => null;

	public IReadOnlyReactiveProperty<Vector3> RxForward => null;

	public IReadOnlyReactiveProperty<float> RxSpeed => null;

	public IReadOnlyReactiveProperty<EPetState> RxState => null;

	public int PetId
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public ulong TargetClientId
	{
		get
		{
			return 0uL;
		}
		set
		{
		}
	}

	public Vector3 Position
	{
		get
		{
			return default;
		}
		set
		{
		}
	}

	public Vector3 Forward
	{
		get
		{
			return default;
		}
		set
		{
		}
	}

	public float Speed
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public EPetState State
	{
		get
		{
			return EPetState.Idle;
		}
		set
		{
		}
	}

	public void Init()
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

	private void RegisterVariableEvents()
	{
	}

	private void UnregisterVariableEvents()
	{
	}

	private void SetPetNetworkVariable<T>(ref NetworkVariable<T> networkVariable, T value) where T : struct
	{
	}

	private void RegisterVariableEvent<T>(NetworkVariable<T> syncVar, ReactiveProperty<T> rxVar) where T : struct
	{
	}

	private void RegisterVariableEvent<TWire, TValue>(NetworkVariable<TWire> syncVar, ReactiveProperty<TValue> rxVar, Func<TWire, TValue> decode) where TWire : struct
	{
	}

	private void UnregisterVariableEvent<T>(NetworkVariable<T> syncVar) where T : struct
	{
	}



}
