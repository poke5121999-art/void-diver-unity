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

	public readonly Dictionary<ulong, EClientState> ClientStates = new Dictionary<ulong, EClientState>();

	private CompositeDisposable _disposable;

	public ReactiveProperty<ERoomState> RxRoomState { get; } = new ReactiveProperty<ERoomState>();

	public ERoomState RoomState => RxRoomState.Value;

	public int PlayerCountMax { get; private set; }

	public bool IsSingleMode { get; private set; }

	public bool IsPrivate { get; private set; }

	private void OnRoomStateChanged(ERoomState state)
	{
		_logger.Debug(string.Format("[RoomContext] State: {0}", state));
	}

	public void Init(int playerCountMax = 0, bool isSingleMode = false, bool isPrivate = false)
	{
		IsSingleMode = isSingleMode;
		PlayerCountMax = playerCountMax;
		IsPrivate = isPrivate;
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		RxRoomState.Subscribe(OnRoomStateChanged).AddTo(_disposable);
	}

	public void Clear()
	{
		IsSingleMode = false;
		RxRoomState.Value = ERoomState.None;
		ClientStates.Clear();
		_disposable.Dispose();
	}
}
