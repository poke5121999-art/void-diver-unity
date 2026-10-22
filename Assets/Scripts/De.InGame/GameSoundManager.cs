using System;
using DarkTonic.MasterAudio;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Scene;
using UniRx;
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

	private ulong _clientId;

	private PlaySoundResult _dangerSoundResult;

	private PlaySoundResult _anomalyAmbientResult;

	private bool _isBgmPausedForDying;

	private bool _isBgmPausedForDespair;

	private string _overrideBgm;

	private IDisposable _overrideBgmStopDisposable;

	private bool IsInCombat => false;

	private int LoungeBgmId => 0;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnStageStateChanged(EStageState state)
	{
	}

	private void OnHostPlayerAdded(GamePlayer hostPlayer)
	{
	}

	private void OnHostPlayerRemoved()
	{
	}

	private void OnStartSceneLoaded()
	{
	}

	private void OnViewTargetChanged(ulong clientId)
	{
	}

	private void OnStateChanged(EActionState state)
	{
	}

	private void OnPlaySfx(NtfPlaySfx message)
	{
	}

	private void OnDisplayFx(NtfDisplayFx message)
	{
	}

	private void UpdateInGameBgm()
	{
	}

	private void OnUpdateAnomalyAmbientSfx()
	{
	}

	public void OnCutsceneStart()
	{
	}

	public void OnCutsceneEnd()
	{
	}

	public void PlayOverrideBgm(string bgm)
	{
	}

	public void StopOverrideBgm(float seconds = 0f)
	{
	}

	private void OnEnterDespair()
	{
	}
}
