using System.Collections.Generic;
using NemoLib.Log;
using UniRx;
using Unity.Netcode;
using VContainer;

public class RoomContext : NativeBase
{
	[Inject]
	private NetworkManager _networkManager;

	[Inject]
	private SeriLogger _logger;

	public readonly Dictionary<ulong, EClientState> ClientStates;

	private CompositeDisposable _disposable;

	public ReactiveProperty<ERoomState> RxRoomState { get; }

	public ERoomState RoomState => ERoomState.None;

	public int PlayerCountMax { get; private set; }

	public bool IsSingleMode { get; private set; }

	public bool IsPrivate { get; private set; }

	private void OnRoomStateChanged(ERoomState state)
	{
	}

	public void Init(int playerCountMax = 0, bool isSingleMode = false, bool isPrivate = false)
	{
	}

	public void Clear()
	{
	}
}
