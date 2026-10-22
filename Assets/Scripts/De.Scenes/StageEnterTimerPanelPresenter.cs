using System;
using DG.Tweening;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class StageEnterTimerPanelPresenter : Presenter
{
	[View]
	private StageEnterTimerPanelView _view;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameManager _gameManager;

	private bool _isTimerOn;

	private float _enterTime;

	private bool _isFading;

	private void Start()
	{
		_gameChannel.Receive<NtfAllPlayerReadyToEnterStage>().Subscribe(StartTimer).AddTo(this);
		_gameChannel.Receive<NtfCanceledEnterStage>().Subscribe(delegate
		{
			CancelTimer();
		}).AddTo(this);
		_view.Anchor.SetActive(false);
	}

	private void StartTimer(NtfAllPlayerReadyToEnterStage message)
	{
		_isTimerOn = true;
		_enterTime = message.EnterTime;
		_view.NoticeText.text = TLocalizedText.DepartingSoon.LocalText;
		_view.Anchor.SetActive(true);
		_soundManager.PlaySfx("CountDown");
	}

	private void CancelTimer()
	{
		_isTimerOn = false;
		_enterTime = -1f;
		_view.Anchor.SetActive(false);
		_soundManager.StopSfxAll("CountDown");
		_soundManager.PlaySfx("Fail");
		if (_isFading)
		{
			_view.BG_Enter.DOFade(0f, 0.5f);
			_isFading = false;
		}
	}

	private void FixedUpdate()
	{
		if (!_isTimerOn)
		{
			return;
		}
		if (_enterTime > 0f)
		{
			float num = _enterTime - _gameManager.TimeNow;
			if (num <= 0.5f && !_isFading)
			{
				_view.BG_Enter.DOFade(1f, Mathf.Min(num, 0.5f));
				_isFading = true;
			}
			int num2 = (int)Math.Floor(num / 60f);
			int num3 = (int)Math.Floor(num % 60f);
			int num4 = (int)Math.Floor((num - (float)Math.Floor(num)) * 100f);
			_view.TimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", num2, num3, num4);
		}
		else
		{
			_view.Anchor.SetActive(false);
			_isTimerOn = false;
			_isFading = false;
		}
	}
}
