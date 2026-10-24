using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NemoLib.Log;
using Steamworks;
using UnityEngine;
using VContainer;

public class SteamAchievementService : MonoBase, IAchievementService
{
	[Inject]
	private SeriLogger _logger;

	private const float STORE_INTERVAL = 60f;

	private bool _isDirty;

	private float _storeTimer;

	private Callback<UserStatsReceived_t> _userStatsReceivedCallback;

	public bool IsAvailable => SteamManager.Initialized;

	public bool IsInitialized { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if (_isDirty)
		{
			_storeTimer += Time.deltaTime;
			if (_storeTimer >= 60f)
			{
				StoreStats();
			}
		}
	}

	public async UniTask<bool> InitializeAsync()
	{
		if (IsInitialized)
		{
			return true;
		}
		if (!SteamManager.Initialized)
		{
			_logger.Warning("[SteamAchievementService] Steam이 초기화되지 않았습니다.");
			return false;
		}
		UniTaskCompletionSource tcs = new UniTaskCompletionSource();
		_userStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(delegate(UserStatsReceived_t callback)
		{
			if (callback.m_eResult != EResult.k_EResultOK)
			{
				tcs.TrySetException(new Exception(string.Format("Stats load failed: {0}", callback.m_eResult)));
			}
			else
			{
				IsInitialized = true;
				tcs.TrySetResult();
			}
		});
		SteamUserStats.RequestCurrentStats();
		try
		{
			using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10.0)))
			{
				await tcs.Task.AttachExternalCancellation(cts.Token);
			}
			// Callback<T>.Dispose MethodRef is only in MoveNext's metadata-init list, no call site follows it (machine code checked @0x1801d6440)
			return true;
		}
		catch (Exception ex)
		{
			_logger.Warning("[SteamAchievementService] InitializeAsync 실패: " + ex.Message);
			return false;
		}
	}

	public void SetProgress(int achievementId, int current, int max)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (IsAchieved(achievementId))
		{
			return;
		}
		SteamUserStats.SetStat(achievementId.ToString(), current);
		_isDirty = true;
		if (max <= current)
		{
			SetAchievement(achievementId);
		}
	}

	public void SetAchievement(int achievementId)
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.SetAchievement(achievementId.ToString());
			StoreStats();
		}
	}

	public int GetProgress(int achievementId)
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.GetStat(achievementId.ToString(), out int pData);
			return pData;
		}
		return 0;
	}

	public bool IsAchieved(int achievementId)
	{
		bool pbAchieved = false;
		if (SteamManager.Initialized)
		{
			SteamUserStats.GetAchievement(achievementId.ToString(), out pbAchieved);
			return pbAchieved;
		}
		return false;
	}

	public void StoreStats()
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.StoreStats();
			_isDirty = false;
			_storeTimer = 0f;
		}
	}

	public void ClearAchievement(int achievementId)
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.ClearAchievement(achievementId.ToString());
			SteamUserStats.SetStat(achievementId.ToString(), 0);
			StoreStats();
		}
	}
}
