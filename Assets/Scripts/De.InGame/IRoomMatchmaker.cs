using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using UniRx;

public interface IRoomMatchmaker
{
	MatchingLobby JoinedLobby { get; }

	string LoungeCode { get; }

	bool IsAlreadyJoined { get; }

	bool IsPrivateLobby { get; }

	Subject<Unit> OnCreateLobby { get; }

	Subject<bool> OnJoinLobby { get; }

	Subject<Unit> OnLeaveLobby { get; }

	Subject<MatchingLobby> OnCurrentLobbyDataUpdated { get; }

	UniTask<ECreateLobbyResult> CreateLobby(MatchingLobbyOptions options);

	UniTask<int> JoinLobby(ulong lobbyId);

	void LeaveLobby();

	void SetJoinable(bool isJoinable);

	void UpdateGameStarted(bool isStarted);

	void UpdateLobbyType(ERoomPublishType publishType);

	void UpdateLobbyTitle(string title);

	void UpdateLobbyDifficulty(ERoomDifficulty difficulty);

	void UpdateLobbySettings(string title, ERoomPublishType publishType, ERoomDifficulty difficulty);

	UniTask<MatchingLobby> GetLobbyAsync(ulong lobbyId);

	UniTask<MatchingLobby[]> GetLobbyListAsync(bool blockOtherAreas, bool showPlayingLounge);
}
