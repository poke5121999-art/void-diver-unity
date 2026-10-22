using UnityEngine;
using UniRx.Triggers;
using NemoLib.Popup;
using GameCommon.Table;
using GameCommon.Enum;
using System.Linq;
using System;
using System.Collections.Generic;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.EventSystems;
using VContainer;

public class InGameStatusPanelPresenter : BasePanelPresenter
{
	[View]
	private InGameStatusPanelView _view;

	[Model]
	private InGameStatusPanelModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private GamePingManager _pingManager;

	[Inject]
	private ToastManager _toastManager;

	private CompositeDisposable _disposable;

	private readonly Dictionary<int, InGameBuffSlotPresenter> _buffSlots = new Dictionary<int, InGameBuffSlotPresenter>();

	protected override BasePanelModel PanelModel => _model;

	private CharacterModel CharacterModel => _model.CharacterModel;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void Start()
	{
		_gameManager.RxViewTargetClientId.Where((ulong id) => id != ulong.MaxValue).Subscribe(OnViewTargetClientIdChanged).AddTo(this);
		_view.Contents.OnPointerDownAsObservable().Subscribe(OnContentsPointerDown).AddTo(this);
	}

	private void OnViewTargetClientIdChanged(ulong targetClientId)
	{
		CharacterModel characterModel = _gameManager.FindCharacterModel(targetClientId);
		if (characterModel == null)
		{
			_logger.Warning(string.Format("targetClientId {0} is not found in PlayerCharacters", targetClientId));
			return;
		}
		_model.CharacterModel = characterModel;
		RefreshUis(characterModel);
	}

	private void OnContentsPointerDown(PointerEventData eventData)
	{
		if (!_inputController.IsPingPressed)
		{
			return;
		}
		if (_gameManager.MyCharacterModel.IsHitable)
		{
			EPingResult ePingResult = _pingManager.TrySendPingByStatus();
			if (ePingResult != EPingResult.Ok)
			{
				_toastManager.Notice(ePingResult.ToLocalText());
			}
		}
		else
		{
			_toastManager.NoticeFail(TLocalizedText.ActionNotAvailableWhileDead.LocalText);
		}
	}

	private void RefreshUis(CharacterModel characterModel)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		OnUpdateMaxHp();
		UpdateLightFuelUi();
		_model.CharacterModel.RxHp.Subscribe(delegate(int _)
		{
			UpdateHpUi();
		}).AddTo(_disposable);
		_model.CharacterModel.RxShield.Subscribe(delegate(int _)
		{
			UpdateHpUi();
		}).AddTo(_disposable);
		_model.CharacterModel.RxHpMax.Subscribe(delegate(EStatType _)
		{
			OnUpdateMaxHp();
		}).AddTo(_disposable);
		_model.CharacterModel.RxLightFuel.Subscribe(delegate(int _)
		{
			UpdateLightFuelUi();
		}).AddTo(_disposable);
		_model.CharacterModel.RxBuffs.ObserveChange().Subscribe(delegate(int _)
		{
			UpdateAllBuffSlots();
		}).AddTo(_disposable);
		UpdateAllBuffSlots();
		_view.StressLine.GetComponent<StressLinePresenter>().Inject(new StressLineModel
		{
			CharacterModel = _model.CharacterModel
		});
	}

	private void UpdateAllBuffSlots()
	{
		if (_model.CharacterModel == null)
		{
			return;
		}
		HashSet<int> hashSet = new HashSet<int>();
		foreach (KeyValuePair<int, UnitBuff> item in _model.CharacterModel.RxBuffs)
		{
			UnitBuff value = item.Value;
			if (TBuff.Table[value.Id].ShowBuffIcon && value.IsActive && value.StackCount > 0)
			{
				hashSet.Add(value.Id);
				EnsureBuffSlot(value.Id);
			}
		}
		foreach (KeyValuePair<int, InGameBuffSlotPresenter> item2 in _buffSlots.ToList())
		{
			int key = item2.Key;
			if (hashSet.Contains(key))
			{
				continue;
			}
			if (_model.CharacterModel.Buffs.ContainsKey(key))
			{
				UnitBuff unitBuff = _model.CharacterModel.Buffs[key];
				if (TBuff.Table[unitBuff.Id].ShowBuffIcon && unitBuff.IsActive && unitBuff.StackCount > 0)
				{
					continue;
				}
			}
			HideBuffSlot(key);
		}
	}

	private void EnsureBuffSlot(int buffId)
	{
		if (!_buffSlots.TryGetValue(buffId, out var value))
		{
			GameObject gameObject = _assetManager.InstantiateBuffSlot();
			value = gameObject.GetComponent<InGameBuffSlotPresenter>();
			value.transform.parent = _view.HpSlider_StatusEffectTags.transform;
			value.transform.localPosition = Vector3.zero;
			value.transform.localScale = Vector3.one;
			value.gameObject.name = string.Format("Buff_{0}", buffId);
			_buffSlots.Add(buffId, value);
			InGameBuffSlotModel inGameBuffSlotModel = new InGameBuffSlotModel();
			inGameBuffSlotModel.BuffId = buffId;
			inGameBuffSlotModel.TargetUnitModel = _model.CharacterModel;
			inGameBuffSlotModel.OnEnter.Subscribe(OnBuffSlotEnter).AddTo(this);
			inGameBuffSlotModel.OnExit.Subscribe(OnBuffSlotExit).AddTo(this);
			value.Inject(inGameBuffSlotModel);
		}
		else if (!value.gameObject.activeSelf)
		{
			value.gameObject.SetActive(true);
			value.Refresh();
		}
	}

	private void HideBuffSlot(int buffId)
	{
		if (_buffSlots.TryGetValue(buffId, out var value) && value.gameObject.activeSelf)
		{
			value.gameObject.SetActive(false);
		}
	}

	private void UpdateHpUi()
	{
		int num = Mathf.Max(_model.CharacterModel.Hp, 0);
		int num2 = (int)_model.CharacterModel.HpMax;
		int shield = _model.CharacterModel.Shield;
		if (num2 >= 1)
		{
			_view.HpSlider.GetComponent<HpAfterImageTweenGauge>().SetHpWithGuard(num, num2, shield);
			_view.HpSlider_Hp_Value.text = num.ToString();
			_view.HpSlider_Hp_Max.text = string.Format("/ {0}", num2);
			_view.HpSlider_Hp_Shield.SetActive(shield > 0);
			_view.HpSlider_Hp_Shield_Value.text = shield.ToString();
		}
	}

	private void OnUpdateMaxHp()
	{
		if (!(_model.CharacterModel.HpMax <= 0f))
		{
			UpdateHpUi();
		}
	}

	private void UpdateLightFuelUi()
	{
		int intValue = TConst.LightFuelMax.IntValue;
		float num = MathUtility.DivideIntClamp(_model.CharacterModel.LightFuel, intValue);
		_view.BrightnessSlider.value = num;
		_view.BrightnessSlider_Text.text = string.Format("{0}%", (int)Math.Round(num * 100f));
		_view.BrightnessSlider_HeadLight.color = new Color(1f, 1f, 1f, num);
	}

	private void OnBuffSlotEnter(InGameBuffSlotModel model)
	{
		_view.BuffTooltip.gameObject.SetActive(true);
		_view.BuffTooltip.Inject(new BuffTooltipModel
		{
			BuffId = model.BuffId
		});
	}

	private void OnBuffSlotExit(InGameBuffSlotModel model)
	{
		if (_view.BuffTooltip.gameObject.activeSelf && _view.BuffTooltip.BuffId == model.BuffId)
		{
			_view.BuffTooltip.gameObject.SetActive(false);
		}
	}
}
