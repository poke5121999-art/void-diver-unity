using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Log;
using Steamworks;
using UnityEngine;
using UniRx;
using VContainer;

public class SteamRoomMatchmaker : MonoBase, IRoomMatchmaker
{
	[Inject]
	private SeriLogger _logger;

	protected Callback<LobbyCreated_t> CallbackLobbyCreated;

	protected Callback<LobbyMatchList_t> CallbackLobbyMatchList;

	protected Callback<LobbyEnter_t> CallbackLobbyEnter;

	protected Callback<LobbyDataUpdate_t> CallbackLobbyDataUpdate;

	protected Callback<LobbyKicked_t> CallbackLobbyKicked;

	private TaskCompletionSource<ulong> _joinLobbyTcs;

	private TaskCompletionSource<MatchingLobby[]> _getLobbyListTcs;

	private TaskCompletionSource<MatchingLobby> _requestLobbyDataTcs;

	private CSteamID _requestLobbyId = CSteamID.Nil;

	private readonly Dictionary<CSteamID, MatchingLobby> _cachedLobbies = new Dictionary<CSteamID, MatchingLobby>();

	private TaskCompletionSource<LobbyCreated_t> _createLobbyTcs;

	public MatchingLobby JoinedLobby { get; private set; }

	private CSteamID JoinedLobbyId
	{
		get
		{
			if (JoinedLobby == null)
			{
				return CSteamID.Nil;
			}
			return new CSteamID(JoinedLobby.LobbyId);
		}
	}

	public Subject<Unit> OnCreateLobby { get; } = new Subject<Unit>();

	public Subject<bool> OnJoinLobby { get; } = new Subject<bool>();

	public Subject<Unit> OnLeaveLobby { get; } = new Subject<Unit>();

	public Subject<MatchingLobby> OnCurrentLobbyDataUpdated { get; } = new Subject<MatchingLobby>();

	public string LoungeCode => JoinedLobbyId.ToString();

	public bool IsAlreadyJoined => JoinedLobbyId != CSteamID.Nil;

	public bool IsPrivateLobby
	{
		get
		{
			if (JoinedLobby == null)
			{
				return false;
			}
			return JoinedLobby.PublishType == ERoomPublishType.Private;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		CallbackLobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
		CallbackLobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
		CallbackLobbyMatchList = Callback<LobbyMatchList_t>.Create(OnLobbyList);
		CallbackLobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyUpdated);
		CallbackLobbyKicked = Callback<LobbyKicked_t>.Create(OnLobbyKicked);
		if (SteamManager.Initialized)
		{
			SteamNetworkingUtils.InitRelayNetworkAccess();
		}
	}

	async UniTask<ECreateLobbyResult> IRoomMatchmaker.CreateLobby(MatchingLobbyOptions options)
	{
		if (!SteamManager.Initialized)
		{
			_logger.Error("CreateLobbySteam is not initialized.");
			return ECreateLobbyResult.NotInitialized;
		}
		if (JoinedLobbyId != CSteamID.Nil)
		{
			_logger.Warning(string.Format("Already in a lobby {0}, leaving it first.", JoinedLobbyId));
			return ECreateLobbyResult.AlreadyJoined;
		}
		_createLobbyTcs = new TaskCompletionSource<LobbyCreated_t>();
		SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, options.PlayerCountMax);
		LobbyCreated_t lobbyCreated_t = await _createLobbyTcs.Task;
		CSteamID cSteamID = new CSteamID(lobbyCreated_t.m_ulSteamIDLobby);
		_createLobbyTcs = null;
		if (lobbyCreated_t.m_eResult != EResult.k_EResultOK || !cSteamID.IsValid())
		{
			_logger.Warning(string.Format("Failed to create matching lobby. Result: {0}, lobbyId {1} is invalid.", lobbyCreated_t.m_eResult, cSteamID));
			return (lobbyCreated_t.m_eResult != EResult.k_EResultTimeout) ? ECreateLobbyResult.Failed : ECreateLobbyResult.Timeout;
		}
		SteamNetworkPingLocation_t result;
		SteamNetworkingUtils.GetLocalPingLocation(out result);
		SteamNetworkingUtils.ConvertPingLocationToString(ref result, out var pszBuf, 512);
		string personaName = SteamFriends.GetPersonaName();
		MatchingLobby matchingLobby = new MatchingLobby();
		matchingLobby.LobbyId = cSteamID.m_SteamID;
		matchingLobby.HostId = SteamUser.GetSteamID().m_SteamID;
		matchingLobby.AccountName = personaName;
		matchingLobby.PlayerName = options.PlayerName;
		matchingLobby.Title = options.Title;
		matchingLobby.PlayerCountMax = options.PlayerCountMax;
		matchingLobby.PingLocationString = pszBuf;
		matchingLobby.Version = Application.version;
		matchingLobby.HostLevel = options.PlayerLevel;
		matchingLobby.RoomDifficulty = options.RoomDifficulty;
		matchingLobby.PublishType = options.PublishType;
		SteamMatchmaking.SetLobbyData(cSteamID, "MatchingLobby", SomeNet.Utility.JsonUtility.Serialize(matchingLobby));
		SteamMatchmaking.SetLobbyMemberLimit(cSteamID, options.PlayerCountMax);
		SteamMatchmaking.SetLobbyData(cSteamID, "Version", matchingLobby.Version);
		SteamMatchmaking.SetLobbyData(cSteamID, "HostLevel", matchingLobby.HostLevel.ToString());
		SteamMatchmaking.SetLobbyData(cSteamID, "Difficulty", matchingLobby.RoomDifficulty.ToString());
		SteamMatchmaking.SetLobbyData(cSteamID, "RoomPublish", matchingLobby.PublishType.ToString());
		SteamMatchmaking.SetLobbyData(cSteamID, "GameStarted", "0");
		JoinedLobby = matchingLobby;
		OnCreateLobby.OnNext(Unit.Default);
		OnJoinLobby.OnNext(true);
		return ECreateLobbyResult.Success;
	}

	private void OnLobbyCreated(LobbyCreated_t result)
	{
		if (_createLobbyTcs != null)
		{
			_createLobbyTcs.TrySetResult(result);
		}
	}

	async UniTask<int> IRoomMatchmaker.JoinLobby(ulong lobbyId)
	{
		CSteamID steamLobbyId = new CSteamID(lobbyId);
		if (!SteamManager.Initialized)
		{
			_logger.Error("JoinLobbySteam is not initialized.");
			return 3001;
		}
		MatchingLobby lobby = await GetLobbyAsync(lobbyId);
		if (lobby == null)
		{
			_logger.Info(string.Format("Failed to get lobby data for lobby {0}", steamLobbyId));
			return 3001;
		}
		if (lobby.Version != Application.version)
		{
			_logger.Warning("Lobby version mismatch. Lobby: " + lobby.Version + ", Client: " + Application.version);
			return 3006;
		}
		_joinLobbyTcs = new TaskCompletionSource<ulong>();
		SteamMatchmaking.JoinLobby(steamLobbyId);
		ulong num = await _joinLobbyTcs.Task;
		_joinLobbyTcs = null;
		if (new CSteamID(num).IsValid())
		{
			JoinedLobby = lobby;
			OnJoinLobby.OnNext(false);
			return 0;
		}
		return 3001;
	}

	private void OnLobbyEntered(LobbyEnter_t result)
	{
		if (result.m_EChatRoomEnterResponse == 1)
		{
			if (_joinLobbyTcs != null)
			{
				_joinLobbyTcs.TrySetResult(result.m_ulSteamIDLobby);
			}
		}
		else if (_joinLobbyTcs != null)
		{
			_joinLobbyTcs.TrySetResult(0uL);
		}
	}

	void IRoomMatchmaker.LeaveLobby()
	{
		if (JoinedLobbyId != CSteamID.Nil)
		{
			OnLeaveLobby.OnNext(Unit.Default);
			SteamMatchmaking.LeaveLobby(JoinedLobbyId);
			JoinedLobby = null;
		}
	}

	private void OnLobbyKicked(LobbyKicked_t pCallback)
	{
		if (new CSteamID(pCallback.m_ulSteamIDLobby) == JoinedLobbyId)
		{
			OnLeaveLobby.OnNext(Unit.Default);
			JoinedLobby = null;
		}
	}

	public async UniTask<MatchingLobby> GetLobbyAsync(ulong lobbyId)
	{
		CSteamID cSteamID = new CSteamID(lobbyId);
		_requestLobbyDataTcs = new TaskCompletionSource<MatchingLobby>();
		_requestLobbyId = cSteamID;
		SteamMatchmaking.RequestLobbyData(cSteamID);
		_logger.Debug(string.Format("Requesting lobby data for lobby {0}...", cSteamID), Color.green);
		MatchingLobby result = await _requestLobbyDataTcs.Task;
		_requestLobbyDataTcs = null;
		_requestLobbyId = CSteamID.Nil;
		return result;
	}

	async UniTask<MatchingLobby[]> IRoomMatchmaker.GetLobbyListAsync(bool blockOtherAreas, bool showPlayingLounge)
	{
		if (!SteamManager.Initialized)
		{
			_logger.Error("GetLobbyListAsyncSteam is not initialized.");
			return null;
		}
		if (_getLobbyListTcs == null)
		{
			_cachedLobbies.Clear();
			_getLobbyListTcs = new TaskCompletionSource<MatchingLobby[]>();
			SteamMatchmaking.AddRequestLobbyListDistanceFilter(blockOtherAreas ? ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault : ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide);
			SteamMatchmaking.AddRequestLobbyListStringFilter("Version", Application.version, ELobbyComparison.k_ELobbyComparisonEqual);
			SteamMatchmaking.AddRequestLobbyListStringFilter("RoomPublish", string.Format("{0}", ERoomPublishType.Public), ELobbyComparison.k_ELobbyComparisonEqual);
			if (!showPlayingLounge)
			{
				SteamMatchmaking.AddRequestLobbyListStringFilter("GameStarted", "0", ELobbyComparison.k_ELobbyComparisonEqual);
			}
			SteamMatchmaking.RequestLobbyList();
			MatchingLobby[] result = await _getLobbyListTcs.Task;
			_getLobbyListTcs = null;
			return result;
		}
		return await _getLobbyListTcs.Task;
	}

	private void OnLobbyList(LobbyMatchList_t result)
	{
		if (result.m_nLobbiesMatching == 0)
		{
			if (_getLobbyListTcs != null)
			{
				_getLobbyListTcs.TrySetResult(Array.Empty<MatchingLobby>());
			}
			return;
		}
		for (int i = 0; i < result.m_nLobbiesMatching; i++)
		{
			CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(i);
			_cachedLobbies.Add(lobbyByIndex, null);
			SteamMatchmaking.RequestLobbyData(lobbyByIndex);
		}
	}

	private void OnLobbyUpdated(LobbyDataUpdate_t result)
	{
		CSteamID cSteamID = new CSteamID(result.m_ulSteamIDLobby);
		MatchingLobby matchingLobby = null;
		if (result.m_bSuccess != 0)
		{
			matchingLobby = SomeNet.Utility.JsonUtility.Deserialize<MatchingLobby>(SteamMatchmaking.GetLobbyData(cSteamID, "MatchingLobby"));
			if (matchingLobby != null)
			{
				matchingLobby.MemberCount = SteamMatchmaking.GetNumLobbyMembers(cSteamID);
				matchingLobby.PingTime = GetPingTime(matchingLobby.PingLocationString);
				if (JoinedLobbyId == cSteamID)
				{
					JoinedLobby = matchingLobby;
				}
			}
		}
		if (_requestLobbyDataTcs != null && cSteamID == _requestLobbyId)
		{
			_requestLobbyDataTcs.TrySetResult(matchingLobby);
		}
		if (_cachedLobbies.ContainsKey(cSteamID))
		{
			if (result.m_bSuccess == 0 || matchingLobby == null)
			{
				_cachedLobbies.Remove(cSteamID);
			}
			else
			{
				_cachedLobbies[cSteamID] = matchingLobby;
			}
			if (_cachedLobbies.Count == 0)
			{
				if (_getLobbyListTcs != null)
				{
					_getLobbyListTcs.TrySetResult(Array.Empty<MatchingLobby>());
				}
			}
			else if (_cachedLobbies.Values.All((MatchingLobby l) => l != null) && _getLobbyListTcs != null)
			{
				_getLobbyListTcs.TrySetResult(_cachedLobbies.Values.ToArray());
			}
		}
		if (JoinedLobbyId == cSteamID && result.m_bSuccess != 0 && matchingLobby != null)
		{
			OnCurrentLobbyDataUpdated.OnNext(matchingLobby);
		}
	}

	public void SetJoinable(bool isJoinable)
	{
		SteamMatchmaking.SetLobbyJoinable(JoinedLobbyId, isJoinable);
	}

	private bool IsLobbyOwner()
	{
		return JoinedLobbyId != CSteamID.Nil && SteamMatchmaking.GetLobbyOwner(JoinedLobbyId) == SteamUser.GetSteamID();
	}

	private MatchingLobby ReadLobbyData()
	{
		MatchingLobby matchingLobby = SomeNet.Utility.JsonUtility.Deserialize<MatchingLobby>(SteamMatchmaking.GetLobbyData(JoinedLobbyId, "MatchingLobby"));
		if (matchingLobby == null)
		{
			matchingLobby = new MatchingLobby();
		}
		return matchingLobby;
	}

	public void UpdateLobbyType(ERoomPublishType publishType)
	{
		if (IsLobbyOwner())
		{
			MatchingLobby matchingLobby = ReadLobbyData();
			matchingLobby.PublishType = publishType;
			SteamMatchmaking.SetLobbyData(JoinedLobbyId, "MatchingLobby", SomeNet.Utility.JsonUtility.Serialize(matchingLobby));
			SteamMatchmaking.SetLobbyData(JoinedLobbyId, "RoomPublish", publishType.ToString());
		}
	}

	public void UpdateLobbyTitle(string title)
	{
		if (IsLobbyOwner())
		{
			if (string.IsNullOrEmpty(title))
			{
				title = string.Empty;
			}
			MatchingLobby matchingLobby = ReadLobbyData();
			matchingLobby.Title = title;
			SteamMatchmaking.SetLobbyData(JoinedLobbyId, "MatchingLobby", SomeNet.Utility.JsonUtility.Serialize(matchingLobby));
		}
	}

	public void UpdateLobbyDifficulty(ERoomDifficulty difficulty)
	{
		if (IsLobbyOwner())
		{
			MatchingLobby matchingLobby = ReadLobbyData();
			matchingLobby.RoomDifficulty = difficulty;
			SteamMatchmaking.SetLobbyData(JoinedLobbyId, "MatchingLobby", SomeNet.Utility.JsonUtility.Serialize(matchingLobby));
			SteamMatchmaking.SetLobbyData(JoinedLobbyId, "Difficulty", difficulty.ToString());
		}
	}

	public void UpdateLobbySettings(string title, ERoomPublishType publishType, ERoomDifficulty difficulty)
	{
		if (IsLobbyOwner())
		{
			MatchingLobby matchingLobby = ReadLobbyData();
			if (string.IsNullOrEmpty(title))
			{
				title = string.Empty;
			}
			matchingLobby.Title = title;
			matchingLobby.PublishType = publishType;
			matchingLobby.RoomDifficulty = difficulty;
			SteamMatchmaking.SetLobbyData(JoinedLobbyId, "MatchingLobby", SomeNet.Utility.JsonUtility.Serialize(matchingLobby));
			SteamMatchmaking.SetLobbyData(JoinedLobbyId, "RoomPublish", publishType.ToString());
			SteamMatchmaking.SetLobbyData(JoinedLobbyId, "Difficulty", difficulty.ToString());
		}
	}

	public void UpdateGameStarted(bool isStarted)
	{
		if (IsLobbyOwner())
		{
			MatchingLobby matchingLobby = ReadLobbyData();
			matchingLobby.IsGameStarted = isStarted;
			SteamMatchmaking.SetLobbyData(JoinedLobbyId, "MatchingLobby", SomeNet.Utility.JsonUtility.Serialize(matchingLobby));
			SteamMatchmaking.SetLobbyData(JoinedLobbyId, "GameStarted", isStarted ? "1" : "0");
		}
	}

	private int GetPingTime(string pingLocationString)
	{
		SteamNetworkPingLocation_t result;
		SteamNetworkingUtils.GetLocalPingLocation(out result);
		SteamNetworkPingLocation_t result2;
		SteamNetworkingUtils.ParsePingLocationString(pingLocationString, out result2);
		return SteamNetworkingUtils.EstimatePingTimeBetweenTwoLocations(ref result, ref result2);
	}
}
