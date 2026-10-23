using System;
using UnityEngine;
using NemoLib;
using GameCommon.Table;
using Febucci.UI;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class InGameWavePanelPresenter : Presenter
{
	[View]
	private InGameWavePanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameWaveManager _waveManager;

	[Inject]
	private SeriLogger _logger;

	private GameWave _currentWave;

	private CompositeDisposable _disposable;

	private const string _alertColor = "#c46e5e";

	private void Start()
	{
		_waveManager.RxCurrentWave.Subscribe(OnCurrentWaveChange).AddTo(this);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
		_disposable = null;
	}

	private void Update()
	{
		if (_currentWave == null)
		{
			return;
		}
		IInteractiveObject sourceObject = _currentWave.SourceObject;
		if (sourceObject is WaveExit waveExit)
		{
			float num = Mathf.Max(waveExit.StateRemainTime, 0f);
			switch (waveExit.ExitState)
			{
			case EExitState.Activating:
				_view.TimeText.text = num.ToTimerString();
				break;
			case EExitState.Activated:
				_view.TimeText.text = num.ToTimerString().WithColor("#c46e5e");
				break;
			}
		}
		else if (sourceObject is WaveExecutor)
		{
			float num2 = Mathf.Max(_currentWave.FinishTime - _gameManager.TimeNow, 0f);
			_view.TimeText.text = num2.ToTimerString();
		}
	}

	private void OnCurrentWaveChange(GameWave wave)
	{
		_currentWave = wave;
		if (wave == null)
		{
			_view.Anchor.SetActive(false);
			return;
		}
		_view.Anchor.SetActive(true);
		_disposable?.Dispose();
		IInteractiveObject sourceObject = wave.SourceObject;
		if (sourceObject is WaveExit waveExit)
		{
			_disposable = new CompositeDisposable();
			waveExit.RxExitState.Subscribe(OnExitStateChanged).AddTo(_disposable);
			OnExitStateChanged(waveExit.ExitState);
		}
		else if (sourceObject is WaveExecutor)
		{
			_view.NoticeText.GetComponent<TextAnimator_TMP>().SetText(TLocalizedText.EnemiesIncoming.LocalText);
		}
	}

	private void OnExitStateChanged(EExitState state)
	{
		string text;
		switch (state)
		{
		case EExitState.Activating:
			text = TLocalizedText.DivingBoothCalled.LocalText;
			break;
		case EExitState.Activated:
			text = TLocalizedText.BoardDivingBooth.LocalText.WithColor("#c46e5e");
			break;
		default:
			_view.NoticeText.text = "";
			_view.TimeText.text = string.Empty;
			_view.Anchor.SetActive(false);
			return;
		}
		_view.NoticeText.GetComponent<TextAnimator_TMP>().SetText(text);
		_view.Anchor.SetActive(true);
	}
}
