using System;
using System.Linq;
using GameCommon.Table;
using DarkTonic.MasterAudio;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Scene;
using UniRx;
using UnityEngine;
using VContainer;

public class GameSoundManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameMapManager _mapManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private SceneManager _sceneManager;

	[Inject]
	private GameWaveManager _waveManager;

	[Inject]
	private GameCombatSystem _combatSystem;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	private CompositeDisposable _hostPlayerDisposable;

	private ulong _clientId = ulong.MaxValue;

	private PlaySoundResult _dangerSoundResult;

	private PlaySoundResult _anomalyAmbientResult;

	private bool _isBgmPausedForDying;

	private bool _isBgmPausedForDespair;

	private string _overrideBgm;

	private IDisposable _overrideBgmStopDisposable;

	private bool IsInCombat
	{
		get
		{
			if (!(_waveManager.CurrentWave != null))
			{
				return _combatSystem.IsBossEncountered;
			}
			return true;
		}
	}

	private int LoungeBgmId => _gameManager.HostPlayer?.RxProfile.Value?.BgmId ?? _playerManager.BgmId;

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChanged).AddTo(this);
		_mapManager.RxCurrentSectorId.Subscribe(delegate
		{
			UpdateInGameBgm();
		}).AddTo(this);
		_waveManager.RxCurrentWave.Subscribe(delegate
		{
			UpdateInGameBgm();
		}).AddTo(this);
		_combatSystem.OnBossEncountered.Subscribe(delegate
		{
			UpdateInGameBgm();
		}).AddTo(this);
		_combatSystem.OnBossDeparted.Subscribe(delegate
		{
			UpdateInGameBgm();
		}).AddTo(this);
		_gameContext.RxPlayers.ObserveAdd().Where((DictionaryAddEvent<ulong, GamePlayer> e) => e.Value.IsHostPlayer).Subscribe(delegate(DictionaryAddEvent<ulong, GamePlayer> e)
		{
			OnHostPlayerAdded(e.Value);
		})
			.AddTo(this);
		_gameContext.RxPlayers.ObserveRemove().Where((DictionaryRemoveEvent<ulong, GamePlayer> e) => e.Value.IsHostPlayer).Subscribe(delegate
		{
			OnHostPlayerRemoved();
		})
			.AddTo(this);
		_gameManager.RxStageState.Subscribe(OnStageStateChanged).AddTo(this);
		_gameChannel.Receive<NtfPlaySfx>().Subscribe(OnPlaySfx).AddTo(this);
		_gameChannel.Receive<NtfDisplayFx>().Subscribe(OnDisplayFx).AddTo(this);
		_sceneManager.RxSceneLoaded.Where((string scene) => scene == "Scene/StartScene.unity").Subscribe(delegate
		{
			OnStartSceneLoaded();
		}).AddTo(this);
	}

	private void OnStageStateChanged(EStageState state)
	{
		switch (state)
		{
		case EStageState.SceneLoad:
			_soundManager.MuteBus("SFX_GAME");
			return;
		default:
			return;
		case EStageState.Closed:
			OnStageClosed();
			break;
		case EStageState.Playing:
			break;
		}
		_soundManager.UnmuteBus("SFX_GAME");
		OnUpdateAnomalyAmbientSfx();
		void OnStageClosed()
		{
			_disposable?.Dispose();
			_overrideBgmStopDisposable?.Dispose();
			_overrideBgmStopDisposable = null;
		}
	}

	private void OnHostPlayerAdded(GamePlayer hostPlayer)
	{
		_hostPlayerDisposable?.Dispose();
		_hostPlayerDisposable = new CompositeDisposable();
		hostPlayer.RxProfile.Pairwise().Where((Pair<Profile> pair) => pair.Previous.BgmId != pair.Current.BgmId).Subscribe(delegate
		{
			UpdateInGameBgm();
		})
			.AddTo(_hostPlayerDisposable);
	}

	private void OnHostPlayerRemoved()
	{
		_hostPlayerDisposable?.Dispose();
		_hostPlayerDisposable = null;
	}

	private void OnStartSceneLoaded()
	{
		_isBgmPausedForDying = false;
		_soundManager.PlayBgm("Title");
	}

	private void OnViewTargetChanged(ulong clientId)
	{
		if (_clientId == clientId)
		{
			return;
		}
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_clientId = clientId;
		_isBgmPausedForDying = false;
		_isBgmPausedForDespair = false;
		CharacterModel characterModel = _gameManager.FindCharacterModel(clientId);
		if (characterModel == null)
		{
			_soundManager.SetFollowingTarget(null);
			return;
		}
		_soundManager.SetFollowingTarget(characterModel.gameObject);
		characterModel.RxState.Subscribe(OnStateChanged).AddTo(_disposable);
		characterModel.RxHp.Subscribe(delegate(int hp)
		{
			if (characterModel.State != EActionState.Dead && characterModel.State != EActionState.Escaped)
			{
				if ((float)hp > characterModel.HpMax * TConst.VignetteHpRatio.FloatValue)
				{
					if (_dangerSoundResult != null)
					{
						_dangerSoundResult.StopSound();
						_dangerSoundResult = null;
					}
				}
				else if (hp >= 1 && _dangerSoundResult == null)
				{
					_dangerSoundResult = _soundManager.PlaySfx("DangerAlert");
				}
			}
		}).AddTo(_disposable);
		characterModel.RxBuffs.ObserveAdd().Where((DictionaryAddEvent<int, UnitBuff> e) => TBuff.Table[e.Key].IsDespairEffect).Subscribe(delegate
		{
			OnEnterDespair();
		})
			.AddTo(_disposable);
		UpdateInGameBgm();
	}

	private void OnStateChanged(EActionState state)
	{
		if (state != EActionState.Dying)
		{
			if (state == EActionState.Dead || state == EActionState.Revive)
			{
				if (_isBgmPausedForDying)
				{
					_isBgmPausedForDying = false;
					_soundManager.UnpauseBgm();
				}
				if (state == EActionState.Dead)
				{
					StopDangerSound();
					return;
				}
			}
			else if (_isBgmPausedForDying)
			{
				_isBgmPausedForDying = false;
			}
			if (state == EActionState.Escaped)
			{
				StopDangerSound();
			}
		}
		else if (!_isBgmPausedForDying)
		{
			_isBgmPausedForDying = true;
			if (_soundManager.IsBgmPlaying())
			{
				_soundManager.PauseBgm();
			}
		}
		void StopDangerSound()
		{
			if (_dangerSoundResult != null)
			{
				_dangerSoundResult.StopSound();
			}
			_dangerSoundResult = null;
		}
	}

	private void OnPlaySfx(NtfPlaySfx message)
	{
		if (message == null)
		{
			return;
		}
		if (message.TargetNetworkObjectId == 0L)
		{
			if (message.Position == null)
			{
				_soundManager.PlaySfx(message.Sfx);
			}
			else
			{
				_soundManager.PlaySfx3D(message.Sfx, message.Position.ToVector3());
			}
			return;
		}
		UnitModel unitModel = _objectManager.FindUnitModel(message.TargetNetworkObjectId);
		if (!(unitModel == null))
		{
			_soundManager.PlaySfx3D(message.Sfx, unitModel.transform);
		}
	}

	private void OnDisplayFx(NtfDisplayFx message)
	{
		if (message == null || string.IsNullOrEmpty(message.Sfx))
		{
			return;
		}
		if (message.TargetNetworkObjectId == 0L)
		{
			_soundManager.PlaySfx3D(message.Sfx, message.Position.ToVector3());
			return;
		}
		UnitModel unitModel = _objectManager.FindUnitModel(message.TargetNetworkObjectId);
		if (!(unitModel == null))
		{
			_soundManager.PlaySfx3D(message.Sfx, unitModel.transform);
		}
	}

	private void UpdateInGameBgm()
	{
		if (_isBgmPausedForDying || _isBgmPausedForDespair)
		{
			return;
		}
		string text;
		if (_overrideBgm == null)
		{
			int sectorId = _mapManager.CurrentSectorId;
			if (sectorId < 0)
			{
				return;
			}
			text = (_gameManager.IsInLounge ? GetLoungeBgm() : GetStageBgm());
			if (string.IsNullOrEmpty(text))
			{
				if (_soundManager.IsBgmPlaying())
				{
					_soundManager.StopBgm();
				}
				return;
			}
			string GetLoungeBgm()
			{
				if (_gameManager.IsTutorial)
				{
					return string.Empty;
				}
				return TLoungeBgm.Table[LoungeBgmId]?.Bgm;
			}
			string GetStageBgm()
			{
				TSector tSector = TSector.Table[sectorId];
				MonsterModel monsterModel = _combatSystem.AggroedBossModels.Where((MonsterModel m) => !string.IsNullOrEmpty(TMonster.Table[m.MonsterId].BossBgm)).MaxBy((MonsterModel m) => TMonster.Table[m.MonsterId].BgmPriority);
				if (!(monsterModel != null))
				{
					if (_waveManager.CurrentWave == null && !_combatSystem.IsBossEncountered)
					{
						return tSector.Bgm;
					}
					return tSector.CombatBgm;
				}
				return TMonster.Table[monsterModel.MonsterId].BossBgm;
			}
		}
		else
		{
			text = _overrideBgm;
		}
		_soundManager.PlayBgm(text);
	}

	private void OnUpdateAnomalyAmbientSfx()
	{
		if (_gameManager.StageState == EStageState.Playing && !_gameManager.IsInLounge)
		{
			if (!string.IsNullOrEmpty(TAnomaly.Table[_gameContext.AnomalyType].AmbientSfx))
			{
				if (_anomalyAmbientResult == null)
				{
					_anomalyAmbientResult = _soundManager.PlaySfx(TAnomaly.Table[_gameContext.AnomalyType].AmbientSfx);
				}
				return;
			}
		}
		if (_anomalyAmbientResult != null)
		{
			_anomalyAmbientResult.StopSound();
			_anomalyAmbientResult = null;
		}
	}

	public void OnCutsceneStart()
	{
		_soundManager.MuteBus("SFX_GAME");
		_soundManager.MuteBus("SFX_SYSTEM");
	}

	public void OnCutsceneEnd()
	{
		_soundManager.StopSfxAll();
		_soundManager.UnmuteBus("SFX_GAME");
		_soundManager.UnmuteBus("SFX_SYSTEM");
	}

	public void PlayOverrideBgm(string bgm)
	{
		_overrideBgmStopDisposable?.Dispose();
		_overrideBgmStopDisposable = null;
		_overrideBgm = bgm;
		UpdateInGameBgm();
	}

	public void StopOverrideBgm(float seconds = 0f)
	{
		_overrideBgmStopDisposable?.Dispose();
		_overrideBgmStopDisposable = null;
		if (seconds > 0f)
		{
			_overrideBgmStopDisposable = Observable.Timer(TimeSpan.FromSeconds(seconds)).Subscribe(delegate
			{
				_overrideBgm = null;
				UpdateInGameBgm();
			});
		}
		else
		{
			_overrideBgm = null;
			UpdateInGameBgm();
		}
	}

	private void OnEnterDespair()
	{
		if (_isBgmPausedForDespair)
		{
			return;
		}
		_isBgmPausedForDespair = true;
		if (!_gameManager.IsTutorial)
		{
			_soundManager.StopSfxAll();
		}
		if (_anomalyAmbientResult != null)
		{
			_anomalyAmbientResult.StopSound();
		}
		_anomalyAmbientResult = null;
		if (_soundManager.IsBgmPlaying())
		{
			_soundManager.PauseBgm();
		}
		_soundManager.PlaySfx("San_Despair");
		Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
		{
			_isBgmPausedForDespair = false;
			_soundManager.UnpauseBgm();
			OnUpdateAnomalyAmbientSfx();
		}).AddTo(_disposable);
	}
}
