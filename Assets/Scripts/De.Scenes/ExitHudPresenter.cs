using System;
using System.Linq;
using GameCommon.Data;
using GameCommon.Table;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class ExitHudPresenter : Presenter
{
	[SerializeField]
	private ExitObject _exitObject;

	[View]
	private ExitHudView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameContext _gameContext;

	private bool _isInitialized;

	private bool _isExitActivated;

	private CompositeDisposable _buffDisposable;

	private void Start()
	{
		_view.Layout.SetActive(value: false);
		_view.TimerText.gameObject.SetActive(value: false);
		_gameManager.RxStageState.Subscribe(OnStageStateChanged).AddTo(this);
	}

	private void OnDestroy()
	{
		_buffDisposable?.Dispose();
	}

	private void OnStageStateChanged(EStageState state)
	{
		if (_isInitialized || state != EStageState.Playing)
		{
			return;
		}
		Goods unlockGoods = GetUnlockGoods();
		bool flag = unlockGoods != null && unlockGoods.Count > 0;
		_view.Layout.SetActive(flag);
		if (flag)
		{
			_view.Coin_Img.sprite = _assetManager.GetIconGoods(unlockGoods.Type, unlockGoods.Id);
			UpdateCountText();
		}
		_gameManager.MyInventory.OnInventoryChanged.Subscribe(delegate
		{
			UpdateCountText();
		}).AddTo(this);
		_view.EscapeBuffStackGauge.gameObject.SetActive(value: false);
		_exitObject.RxExitState.Subscribe(OnExitStateChanged).AddTo(this);
		if (_exitObject is SafeExit safeExit)
		{
			safeExit.RxStateRemainTime.Subscribe(OnRemainTimeChanged).AddTo(this);
		}
		_gameManager.RxViewTargetClientId.Where((ulong id) => id != ulong.MaxValue).Subscribe(OnViewTargetChanged).AddTo(this);
		_isInitialized = true;
	}

	private Goods GetUnlockGoods()
	{
		TExit tExit = TExit.Table[_exitObject.ExitId];
		int level = _gameContext.Players[_gameManager.LocalClientId].Profile.Level;
		return tExit.GetUnlockGoodsData(level).ToGoodsAsBase();
	}

	private void SetupUnlockGoodsLayout()
	{
		Goods unlockGoods = GetUnlockGoods();
		bool flag = unlockGoods != null && unlockGoods.Count > 0;
		_view.Layout.SetActive(flag);
		if (flag)
		{
			_view.Coin_Img.sprite = _assetManager.GetIconGoods(unlockGoods.Type, unlockGoods.Id);
			UpdateCountText();
		}
	}

	private void UpdateCountText()
	{
		Goods unlockGoods = GetUnlockGoods();
		if (unlockGoods != null && unlockGoods.Count >= 1)
		{
			long goodsCount = _gameManager.MyInventory.GetGoodsCount(unlockGoods.Type, unlockGoods.Id);
			string text = unlockGoods.Count.ToString();
			if (goodsCount < unlockGoods.Count)
			{
				text = text.WithColor("#a45646");
			}
			_view.TextCount.text = text;
		}
	}

	private void OnExitStateChanged(EExitState exitState)
	{
		_isExitActivated = exitState == EExitState.Activated;
		if (!_isExitActivated)
		{
			_view.EscapeBuffStackGauge.gameObject.SetActive(value: false);
			if (exitState == EExitState.Finished)
			{
				_view.Layout.SetActive(value: false);
				_view.TimerText.gameObject.SetActive(value: false);
			}
		}
	}

	private void OnRemainTimeChanged(float remainTime)
	{
		_view.TimerText.gameObject.SetActive(remainTime > 0f);
		if (remainTime > 0f)
		{
			_view.TimerText.text = TLocalizedText.SecondsLeftFormat.Format((int)Math.Ceiling(remainTime));
		}
	}

	private void OnViewTargetChanged(ulong targetClientId)
	{
		_buffDisposable?.Dispose();
		_buffDisposable = new CompositeDisposable();
		CharacterModel targetModel = _gameManager.FindCharacterModel(targetClientId);
		if (targetModel == null)
		{
			_view.EscapeBuffStackGauge.gameObject.SetActive(value: false);
			return;
		}
		targetModel.RxBuffs.ObserveChange().Subscribe(delegate
		{
			UpdateEscapeBuffGauge(targetModel);
		}).AddTo(_buffDisposable);
		UpdateEscapeBuffGauge(targetModel);
	}

	private void UpdateEscapeBuffGauge(CharacterModel targetModel)
	{
		if (this == null || _view == null)
		{
			return;
		}
		if (_isExitActivated)
		{
			foreach (BuffEffectInfo<EscapeOnActiveEffect> buffEffect in targetModel.GetBuffEffects<EscapeOnActiveEffect>(checkIsActive: false))
			{
				TBuff tBuff = TBuff.Table[buffEffect.BuffId];
				if (tBuff.Triggers != null)
				{
					FloatValueCompareBuffTrigger floatValueCompareBuffTrigger = tBuff.Triggers.OfType<FloatValueCompareBuffTrigger>().FirstOrDefault((FloatValueCompareBuffTrigger t) => t.TriggerType == EBuffTrigger.Stack);
					if (floatValueCompareBuffTrigger != null && !(floatValueCompareBuffTrigger.CompareValue <= 0f))
					{
						_view.EscapeBuffStackGauge.gameObject.SetActive(value: true);
						_view.EscapeBuffStackGauge.value = (float)buffEffect.Stack / floatValueCompareBuffTrigger.CompareValue;
						return;
					}
				}
			}
		}
		_view.EscapeBuffStackGauge.gameObject.SetActive(value: false);
	}
}
