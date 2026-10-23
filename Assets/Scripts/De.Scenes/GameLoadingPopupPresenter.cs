using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCommon.Enum;
using GameCommon.Table;
using SomeNet.Utility;
using UnityEngine;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class GameLoadingPopupPresenter : BasePopupPresenter
{
	[View]
	private GameLoadingPopupView _view;

	[InjectModel]
	private GameLoadingPopupModel _model;

	[Inject]
	private GameManager _gameManager;

	private readonly ReactiveProperty<float> _rxProgress = new ReactiveProperty<float>(0f);

	private bool _isCompleted;

	private float _elapsedTime;

	private Tween _tween;

	private List<TLoadingText> _loadingTextPool;

	public override bool IsInputActionPopup => true;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		SetLoadingLabel();
		BuildLoadingTextPool();
		SetLoadingText();
		StartLoadingTextCycle(7f).Forget();
		_rxProgress.Subscribe(OnProgressChanged).AddTo(this);
		_gameManager.RxStageState.Where((EStageState state) => state == EStageState.Playing || state == EStageState.Intro).Subscribe(delegate
		{
			OnPlaying().Forget();
		}).AddTo(this);
		_view.Layout_LoadingSprite.gameObject.SetActive(_model.LoadingPopupType == ELoadingPopupType.Default);
		_view.Layout_LoadingSpriteEscape.gameObject.SetActive(_model.LoadingPopupType == ELoadingPopupType.EnterStage || _model.LoadingPopupType == ELoadingPopupType.EscapeSuccess);
		_view.Layout_LoadingSpriteDeath.gameObject.SetActive(_model.LoadingPopupType == ELoadingPopupType.EscapeFail);
	}

	private void SetLoadingLabel()
	{
		string text;
		bool flag = LocalizationUtility.TryGetLocalizedText("UGameLoadingPopup_Layout_LoadingText", out text);
		_view.Layout_LoadingText.text = (flag ? text : "LOADING...");
		_view.TipLayout.SetActive(flag);
	}

	private void BuildLoadingTextPool()
	{
		List<TLoadingText> list = new List<TLoadingText>();
		HashSet<int> hashSet = new HashSet<int>();
		int characterId = _model.CharacterId;
		bool CheckCharacterId(TLoadingText data)
		{
			return data.TargetCharacterId < 1 || data.TargetCharacterId == characterId;
		}
		if (TLoadingText.GroupTable.TryGetValue(ELoadingPopupType.Default, out var value))
		{
			for (int i = 0; i < value.Length; i++)
			{
				TLoadingText tLoadingText = value[i];
				if (CheckCharacterId(tLoadingText) && hashSet.Add(tLoadingText.Id))
				{
					list.Add(tLoadingText);
				}
			}
		}
		ELoadingPopupType loadingPopupType = _model.LoadingPopupType;
		if (loadingPopupType != ELoadingPopupType.Default && TLoadingText.GroupTable.TryGetValue(loadingPopupType, out var value2))
		{
			for (int j = 0; j < value2.Length; j++)
			{
				TLoadingText tLoadingText2 = value2[j];
				if (CheckCharacterId(tLoadingText2) && hashSet.Add(tLoadingText2.Id))
				{
					list.Add(tLoadingText2);
				}
			}
		}
		_loadingTextPool = list;
	}

	private void SetLoadingText()
	{
		if (_loadingTextPool == null || _loadingTextPool.Count == 0)
		{
			_view.TipLayout_TipText.text = string.Empty;
			return;
		}
		int randomInt = RandomUtility.GetRandomInt(0, _loadingTextPool.Count);
		TLoadingText tLoadingText = _loadingTextPool[randomInt];
		_view.TipLayout_TipText.text = tLoadingText.TipText;
		_loadingTextPool.RemoveAt(randomInt);
	}

	private async UniTask StartLoadingTextCycle(float intervalSeconds)
	{
		CancellationToken token = this.GetCancellationTokenOnDestroy();
		while (!_isCompleted && !token.IsCancellationRequested)
		{
			await UniTask.Delay(TimeSpan.FromSeconds(intervalSeconds), ignoreTimeScale: false, PlayerLoopTiming.Update, token);
			if (_isCompleted || token.IsCancellationRequested)
			{
				break;
			}
			SetLoadingText();
			if (_loadingTextPool == null || _loadingTextPool.Count == 0)
			{
				break;
			}
		}
	}

	private void Update()
	{
		if (!_isCompleted)
		{
			_elapsedTime += Time.fixedDeltaTime;
			if (_elapsedTime >= 5f)
			{
				_elapsedTime = 5f;
			}
			_rxProgress.Value = _elapsedTime / 5f;
		}
	}

	private async UniTask OnPlaying()
	{
		_isCompleted = true;
		DOTween.To(() => _rxProgress.Value, delegate(float x)
		{
			_rxProgress.Value = x;
		}, 1f, 0.3f);
		await UniTask.Delay(300);
		GetComponent<CanvasGroup>().DOFade(0f, 0.25f);
		await UniTask.Delay(300);
		Close();
	}

	private void OnProgressChanged(float progress)
	{
	}
}
