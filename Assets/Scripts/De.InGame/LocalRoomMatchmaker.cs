using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using UniRx;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using VContainer;

public class LocalRoomMatchmaker : MonoBase, IRoomMatchmaker
{
	[Inject]
	private NetworkManager _networkManager;

	private IRoomMatchmaker _roomMatchmakerImplementation;

	public Subject<Unit> OnCreateLobby { get; } = new Subject<Unit>();

	public Subject<bool> OnJoinLobby { get; } = new Subject<bool>();

	public Subject<Unit> OnLeaveLobby { get; } = new Subject<Unit>();

	public Subject<MatchingLobby> OnCurrentLobbyDataUpdated { get; } = new Subject<MatchingLobby>();

	public string LoungeCode => ((UnityTransport)_networkManager.NetworkConfig.NetworkTransport).ConnectionData.Address;

	public MatchingLobby JoinedLobby => null;

	public bool IsAlreadyJoined => false;

	public bool IsPrivateLobby => false;

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	async UniTask<ECreateLobbyResult> IRoomMatchmaker.CreateLobby(MatchingLobbyOptions options)
	{
		await UniTask.CompletedTask;
		OnCreateLobby.OnNext(Unit.Default);
		OnJoinLobby.OnNext(true);
		return ECreateLobbyResult.Success;
	}

	async UniTask<int> IRoomMatchmaker.JoinLobby(ulong lobbyId)
	{
		await UniTask.CompletedTask;
		OnJoinLobby.OnNext(false);
		return 0;
	}

	void IRoomMatchmaker.LeaveLobby()
	{
		OnLeaveLobby.OnNext(Unit.Default);
	}

	public void SetJoinable(bool isJoinable)
	{
	}

	public void UpdateGameStarted(bool isStarted)
	{
	}

	public void UpdateLobbyType(ERoomPublishType publishType)
	{
	}

	public void UpdateLobbyTitle(string title)
	{
	}

	public void UpdateLobbyDifficulty(ERoomDifficulty difficulty)
	{
	}

	public void UpdateLobbySettings(string title, ERoomPublishType publishType, ERoomDifficulty difficulty)
	{
	}

	async UniTask<MatchingLobby> IRoomMatchmaker.GetLobbyAsync(ulong lobbyId)
	{
		await UniTask.CompletedTask;
		return null;
	}

	async UniTask<MatchingLobby[]> IRoomMatchmaker.GetLobbyListAsync(bool blockOtherAreas, bool showPlayingLounge)
	{
		await UniTask.CompletedTask;
		return null;
	}
}
