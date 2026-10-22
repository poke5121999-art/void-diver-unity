using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class TalentInfoPanelPresenter : BasePanelPresenter
{
	private enum EFocusPanel
	{
		None = 0,
		Base = 1,
		UnlockMaterial = 2
	}

	[InjectModel]
	private TalentInfoPanelModel _model;

	[View]
	private TalentInfoPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SoundManager _soundManager;

	private UnlockMaterialPanelModel _unlockMaterialPanelModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(TalentInfoPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_unlockMaterialPanelModel = new UnlockMaterialPanelModel();
		_view.Upgrade_Button.OnClickWithSfx().Subscribe(delegate
		{
			OnUpgradeClick();
		}).AddTo(_disposable);
		_playerManager.RxTalent.Subscribe(delegate
		{
			OnRefresh();
		}).AddTo(_disposable);
		InitGamePadNav();
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void OnRefresh()
	{
		int talentId = _model.TalentId;
		_view.Empty.SetActive(talentId < 1);
		_view.Info.SetActive(talentId > 0);
		_view.Upgrade.SetActive(talentId > 0 && !_playerManager.HasTalent(talentId));
		if (talentId >= 1)
		{
			TTalent tTalent = TTalent.Table[talentId];
			TalentTooltipModel talentTooltipModel = new TalentTooltipModel();
			talentTooltipModel.TalentId = talentId;
			_view.TalentTooltip.Inject(talentTooltipModel);
			_unlockMaterialPanelModel.Materials = tTalent.MaterialItemDatas?.Select((GoodsData d) => d.ToGoodsAsBase()).ToArray();
			_unlockMaterialPanelModel.NeedGold = tTalent.NeedGold;
			_unlockMaterialPanelModel.NeedCoin = tTalent.NeedCoin;
			_unlockMaterialPanelModel.NeedRealCoin = tTalent.NeedRealCoin;
			_unlockMaterialPanelModel.UnlockConditions = tTalent.UnlockConditions;
			_view.UnlockMaterialPanel.Inject(_unlockMaterialPanelModel);
		}
	}

	private void InitGamePadNav()
	{
		ClearNav();
		List<Selectable> items = new List<Selectable> { _view.Upgrade_Button };
		SetNavVertical(items);
		_model.OutOfTop.Where((Unit _) => HasUnlockMaterials()).Subscribe(delegate
		{
			FocusPanel(EFocusPanel.UnlockMaterial, ENavDir.Up);
		}).AddTo(_disposable);
		_unlockMaterialPanelModel.OutOfBottom.Subscribe(delegate
		{
			FocusPanel(EFocusPanel.Base, ENavDir.Down);
		}).AddTo(_disposable);
	}

	private bool HasUnlockMaterials()
	{
		return _unlockMaterialPanelModel.Materials.Length != 0;
	}

	private void FocusPanel(EFocusPanel panel, ENavDir entryDir)
	{
		BasePanelModel panelModel = GetPanelModel();
		if (panelModel != null)
		{
			panelModel.EntryDirection = entryDir;
		}
		if (panel == EFocusPanel.Base)
		{
			ResumeNav();
		}
		else
		{
			SuspendNav();
		}
		_unlockMaterialPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.UnlockMaterial;
		BasePanelModel GetPanelModel()
		{
			return panel switch
			{
				EFocusPanel.Base => _model, 
				EFocusPanel.UnlockMaterial => _unlockMaterialPanelModel, 
				_ => null, 
			};
		}
	}

	protected override void OnInputDisabled()
	{
		_unlockMaterialPanelModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void OnUpgradeClick()
	{
		if (_model.TalentId >= 1)
		{
			TryUnlockTalent(_model.TalentId);
		}
	}

	private void TryUnlockTalent(int talentId)
	{
		TTalent tTalent = TTalent.Table[talentId];
		string text;
		if (_playerManager.HasTalent(talentId))
		{
			text = TLocalizedText.AlreadyDeveloped.LocalText;
		}
		else
		{
			EventCondition failedCondition = _playerManager.GetFailedCondition(tTalent.GetUnlockConditions());
			if (failedCondition != default(EventCondition))
			{
				if (failedCondition.Type == EEventConditionType.UserLevel)
				{
					text = TLocalizedText.NotEnoughReputationLevel.LocalText;
				}
				else if (failedCondition.Type != EEventConditionType.Talent)
				{
					text = failedCondition.ToDescription();
				}
				else
				{
					text = TLocalizedText.RequiresPrerequisiteTrait.LocalText;
				}
				_toastManager.NoticeFail(text);
				return;
			}
			if (_playerManager.HasEnoughGoods(tTalent.MaterialGoodsDatas))
			{
				UnlockTalentAsync(talentId).Forget();
				return;
			}
			text = TLocalizedText.NotEnoughCraftMaterials.LocalText;
		}
		_toastManager.NoticeFail(text);
	}

	private async UniTask UnlockTalentAsync(int talentId)
	{
		TalentConfirmPopupPresenter talentConfirmPopupPresenter = await _popupManager.OpenAsync<TalentConfirmPopupPresenter>(new TalentConfirmPopupModel { TalentId = talentId });
		object obj = await talentConfirmPopupPresenter.Task;
		if (!(obj is bool flag) || !flag)
		{
			return;
		}
		IEnumerable<Goods> consumeGoods = TTalent.Table[talentId].MaterialItemDatas?.Select((GoodsData d) => d.ToGoodsAsBase()).ToArray();
		string bagConsumeFailToast = _playerManager.GetBagConsumeFailToast(consumeGoods);
		if (bagConsumeFailToast != null)
		{
			_toastManager.NoticeFail(bagConsumeFailToast);
		}
		ResUnlockTalent resUnlockTalent = await _gameChannel.RequestAsync<ResUnlockTalent>(new ReqUnlockTalent { TalentId = talentId });
		if (resUnlockTalent != null && resUnlockTalent.IsSuccess)
		{
			TTalent tTalent = TTalent.Table[talentId];
			_toastManager.Notice(TLocalizedText.TechTreeCompletedFormat.Format(tTalent.Name));
			_soundManager.PlaySfx("Talent_Success");
		}
	}
}
