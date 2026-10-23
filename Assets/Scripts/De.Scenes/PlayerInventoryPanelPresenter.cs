using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PlayerInventoryPanelPresenter : BasePanelPresenter
{
	public enum EFocusPanel
	{
		None = 0,
		Equipment = 1,
		Scroller = 2,
		Empty = 3,
		Safe = 4
	}

	[InjectModel]
	private PlayerInventoryPanelModel _model;

	[View]
	private PlayerInventoryPanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	private bool _isSafeInventoryUnlocked;

	private InventoryGoodsSlotModel _cachedSelectedSlot;

	private PlayerInventoryScrollerModel _scrollerModel;

	private EquipmentInventoryPanelModel _equipmentModel;

	private EmptyPanelModel _emptyPanelModel;

	private SafeInventoryPanelModel _safeModel;

	private CompositeDisposable _disposable;

	private CompositeDisposable _corruptionDisposable;

	protected override BasePanelModel PanelModel => _model;

	private PlayerInventory Inventory => _model.PlayerInventory;

	private int SlotCount => _model.PlayerInventory.SlotCount;

	private EFocusPanel FocusedPanel
	{
		get
		{
			if (_model == null)
			{
				return EFocusPanel.None;
			}
			return _model.FocusPanel;
		}
	}

	private void Start()
	{
		_view.Page_PrevButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnPrevPageButtonClick();
		}).AddTo(this);
		_view.Page_NextButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnNextPageButtonClick();
		}).AddTo(this);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
		_corruptionDisposable?.Dispose();
	}

	protected void OnInject(PlayerInventoryPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_view.Caption.text = TLocalizedText.Inventory.LocalText;
		_isSafeInventoryUnlocked = CheckSafeInventoryUnlocked();
		InitInventoryScroller();
		InitEmptyPanel();
		InitEquipmentInventoryPanel(model);
		InitSafeInventoryPanel(model);
		UpdatePagination();
		UpdateSlotCounts();
		model.RxHighlightCategory.Subscribe(delegate(EGoodsSlotCategory category)
		{
			_equipmentModel.RxHighlightCategory.Value = category;
		}).AddTo(_disposable);
		_model.PlayerInventory.OnInventoryChanged.Subscribe(OnInventoryChanged).AddTo(_disposable);
		_gameManager.OnMyCharacterSpawned.Subscribe(BindCorruption).AddTo(_disposable);
		BindCorruption(_gameManager.MyCharacterModel);
		InitGamePadNav();
		_model.RxIsInputEnabled.Where((bool enabled) => enabled).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		_model.RxIsInputEnabled.Where((bool enabled) => !enabled).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		EFocusPanel panel;
		if (_model == null || _model.FocusPanel == EFocusPanel.None)
		{
			panel = EFocusPanel.Scroller;
		}
		else
		{
			panel = _model.FocusPanel;
		}
		if (panel == EFocusPanel.Scroller && _scrollerModel.RxIsEmpty.Value)
		{
			panel = EFocusPanel.Empty;
		}
		FocusPanel(panel, _model.EntryDirection);
	}

	protected override void OnInputDisabled()
	{
		_equipmentModel.RxIsInputEnabled.Value = false;
		_scrollerModel.RxIsInputEnabled.Value = false;
		_emptyPanelModel.RxIsInputEnabled.Value = false;
		if (_isSafeInventoryUnlocked)
		{
			_safeModel.RxIsInputEnabled.Value = false;
		}
		base.OnInputDisabled();
	}

	private void InitEquipmentInventoryPanel(PlayerInventoryPanelModel model)
	{
		_equipmentModel = new EquipmentInventoryPanelModel();
		_equipmentModel.RxFocusedEquipmentSlotModel.Skip(1).Subscribe(SetFocusedEquipmentSlot).AddTo(_disposable);
		_equipmentModel.OnEquipmentSlotClick.Subscribe(model.OnEquipmentSlotClick).AddTo(_disposable);
		_equipmentModel.OnEquipmentSlotLeftClick.Subscribe(model.OnEquipmentSlotLeftClick).AddTo(_disposable);
		_equipmentModel.OnEquipmentSlotRightClick.Subscribe(model.OnEquipmentSlotRightClick).AddTo(_disposable);
		_equipmentModel.OnEquipmentSlotDrag.Subscribe(model.OnEquipmentSlotDrag).AddTo(_disposable);
		_equipmentModel.OnEquipmentSlotEndDrag.Subscribe(model.OnEquipmentSlotEndDrag).AddTo(_disposable);
		_view.EquipmentInventoryPanel.Inject(_equipmentModel);
	}

	private void InitSafeInventoryPanel(PlayerInventoryPanelModel model)
	{
		_view.SafeInventoryPanel.gameObject.SetActive(_isSafeInventoryUnlocked);
		if (_isSafeInventoryUnlocked)
		{
			_safeModel = new SafeInventoryPanelModel
			{
				PlayerInventory = model.PlayerInventory,
				AllowedSubTypes = model.AllowedSubTypes,
				ArtifactFilterTypes = model.ArtifactFilterTypes,
				KeyItemFilterTypes = model.KeyItemFilterTypes,
				HideGoods = (model.DisplayScope == EGoodsDisplayScope.AllOwned)
			};
			_safeModel.RxFocusedSafeSlotModel.Skip(1).Subscribe(OnSafeSlotFocused).AddTo(_disposable);
			_safeModel.OnSafeSlotClick.Subscribe(model.OnSafeSlotClick).AddTo(_disposable);
			_safeModel.OnSafeSlotLeftClick.Subscribe(model.OnSafeSlotLeftClick).AddTo(_disposable);
			_safeModel.OnSafeSlotRightClick.Subscribe(model.OnSafeSlotRightClick).AddTo(_disposable);
			_safeModel.OnSafeSlotDrag.Subscribe(model.OnSafeSlotDrag).AddTo(_disposable);
			_safeModel.OnSafeSlotEndDrag.Subscribe(model.OnSafeSlotEndDrag).AddTo(_disposable);
			_view.SafeInventoryPanel.Inject(_safeModel);
		}
	}

	private bool CheckSafeInventoryUnlocked()
	{
		return _playerManager.CheckStateConditions(TFeatureUnlock.Table[EFeatureUnlock.SafeInventory].UnlockConditions);
	}

	private void SetFocusedSlot(InventoryGoodsSlotModel slotModel)
	{
		_cachedSelectedSlot = slotModel;
		_model.RxFocusedSlotModel.SetValueAndForceNotify(slotModel);
		if (slotModel == null || slotModel.IsSlotEmpty)
		{
			_view.GoodsTooltip.gameObject.SetActive(value: false);
			return;
		}
		_view.GoodsTooltip.gameObject.SetActive(value: true);
		GoodsTooltipPresenter component = _view.GoodsTooltip.GetComponent<GoodsTooltipPresenter>();
		GoodsTooltipModel goodsTooltipModel = new GoodsTooltipModel();
		goodsTooltipModel.Goods = slotModel.GoodsSlot.Goods;
		goodsTooltipModel.IsArtifactShop = _model.IsArtifactShop;
		goodsTooltipModel.IsArtifactDeal = _model.IsArtifactDeal;
		component.Inject(goodsTooltipModel);
	}

	private void SetFocusedEquipmentSlot(InventoryGoodsSlotModel slotModel)
	{
		_model.RxFocusedEquipmentSlotModel.SetValueAndForceNotify(slotModel);
		if (slotModel == null || slotModel.IsSlotEmpty)
		{
			_view.GoodsTooltip.gameObject.SetActive(value: false);
			return;
		}
		_view.GoodsTooltip.gameObject.SetActive(value: true);
		GoodsTooltipPresenter component = _view.GoodsTooltip.GetComponent<GoodsTooltipPresenter>();
		GoodsTooltipModel goodsTooltipModel = new GoodsTooltipModel();
		goodsTooltipModel.Goods = slotModel.GoodsSlot.Goods;
		component.Inject(goodsTooltipModel);
	}

	private void OnSafeSlotFocused(InventoryGoodsSlotModel slotModel)
	{
		_model.RxFocusedSafeSlotModel.SetValueAndForceNotify(slotModel);
		if (slotModel == null || slotModel.IsSlotEmpty)
		{
			_view.GoodsTooltip.gameObject.SetActive(value: false);
			return;
		}
		_view.GoodsTooltip.gameObject.SetActive(value: true);
		GoodsTooltipPresenter component = _view.GoodsTooltip.GetComponent<GoodsTooltipPresenter>();
		GoodsTooltipModel goodsTooltipModel = new GoodsTooltipModel();
		goodsTooltipModel.Goods = slotModel.GoodsSlot.Goods;
		component.Inject(goodsTooltipModel);
	}

	private void OnPageChanged(int pageIndex)
	{
		if (pageIndex >= 0)
		{
			UpdatePagination();
		}
	}

	private void OnPageCountChanged(int pageCount)
	{
		if (pageCount >= 0)
		{
			UpdatePagination();
		}
	}

	private void UpdatePagination()
	{
		int pageIndex = _scrollerModel.PageIndex;
		int pageCount = _scrollerModel.PageCount;
		_view.Page_PrevButton.gameObject.SetActive(pageIndex != 0);
		_view.Page_NextButton.gameObject.SetActive(pageIndex != pageCount - 1);
		for (int i = 0; i < _view.Page_PagenationCells.Length; i++)
		{
			SetPageUi(i);
		}
		void SetPageUi(int index)
		{
			GameObject gameObject = _view.Page_PagenationCells[index];
			if (index < pageCount)
			{
				gameObject.SetActive(value: true);
				Image image = gameObject.FindChildComponent<Image>("Icon");
				TextMeshProUGUI textMeshProUGUI = gameObject.FindChildComponent<TextMeshProUGUI>("Text");
				image.gameObject.SetActive(index != pageIndex);
				textMeshProUGUI.gameObject.SetActive(index == pageIndex);
				textMeshProUGUI.text = string.Format("{0}", pageIndex + 1);
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
	}

	private void UpdateSlotCounts()
	{
		int num = _model.PlayerInventory.Slots.Values.Count((GoodsSlot slot) => !slot.IsEmpty);
		_view.GoodsCountText.text = string.Format("{0}/{1}", num, _model.PlayerInventory.SlotCount);
	}

	private void InitEmptyPanel()
	{
		_emptyPanelModel = new EmptyPanelModel
		{
			Text = TLocalizedText.NoAvailableGoods.LocalText
		};
		_view.EmptyPanel.Inject(_emptyPanelModel);
	}

	private void InitInventoryScroller()
	{
		_scrollerModel = new PlayerInventoryScrollerModel
		{
			PlayerInventory = _model.PlayerInventory,
			AllowedSubTypes = _model.AllowedSubTypes,
			ArtifactFilterTypes = _model.ArtifactFilterTypes,
			KeyItemFilterTypes = _model.KeyItemFilterTypes,
			DisplayScope = _model.DisplayScope
		};
		_scrollerModel.RxIsEmpty.Subscribe(OnSlotsEmpty).AddTo(_disposable);
		_scrollerModel.RxPageIndex.Skip(1).Subscribe(OnPageChanged).AddTo(_disposable);
		_scrollerModel.RxPageCount.Skip(1).Subscribe(OnPageCountChanged).AddTo(_disposable);
		_scrollerModel.RxFocusedSlotModel.Skip(1).Subscribe(OnFocusedSlotChanged).AddTo(_disposable);
		_scrollerModel.OnSlotClick.Subscribe(delegate(InventoryGoodsSlotModel slot)
		{
			_model.OnSlotClick.OnNext(slot);
		}).AddTo(_disposable);
		_scrollerModel.OnSlotLeftClick.Subscribe(delegate(InventoryGoodsSlotModel slot)
		{
			_model.OnSlotLeftClick.OnNext(slot);
		}).AddTo(_disposable);
		_scrollerModel.OnSlotRightClick.Subscribe(delegate(InventoryGoodsSlotModel slot)
		{
			_model.OnSlotRightClick.OnNext(slot);
		}).AddTo(_disposable);
		_scrollerModel.OnSlotDrag.Subscribe(_model.OnSlotDrag).AddTo(_disposable);
		_scrollerModel.OnSlotEndDrag.Subscribe(_model.OnSlotEndDrag).AddTo(_disposable);
		_view.Scroller.Inject(_scrollerModel);
	}

	private void OnPrevPageButtonClick()
	{
		if (_scrollerModel.PageIndex > 0)
		{
			_scrollerModel.OnMovePage.OnNext(_scrollerModel.PageIndex - 1);
		}
	}

	private void OnNextPageButtonClick()
	{
		if (_scrollerModel.PageIndex < _scrollerModel.PageCount - 1)
		{
			_scrollerModel.OnMovePage.OnNext(_scrollerModel.PageIndex + 1);
		}
	}

	private void OnFocusedSlotChanged(InventoryGoodsSlotModel slotModel)
	{
		SetFocusedSlot(slotModel);
	}

	private void OnInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
		UpdateSlotCounts();
	}

	private void OnSlotsEmpty(bool isEmpty)
	{
		_view.EmptyPanel.gameObject.SetActive(isEmpty);
	}

	private void BindCorruption(CharacterModel characterModel)
	{
		_corruptionDisposable?.Dispose();
		_corruptionDisposable = new CompositeDisposable();
		if (!(characterModel == null))
		{
			characterModel.RxCorruption.Subscribe(delegate
			{
				OnCorruptionChanged();
			}).AddTo(_corruptionDisposable);
			characterModel.RxCorruptionMax.Subscribe(delegate
			{
				OnCorruptionChanged();
			}).AddTo(_corruptionDisposable);
		}
	}

	private void OnCorruptionChanged()
	{
		CharacterModel myCharacterModel = _gameManager.MyCharacterModel;
		if (myCharacterModel == null)
		{
			return;
		}
		int corruption = myCharacterModel.Corruption;
		float corruptionMax = myCharacterModel.CorruptionMax;
		ECorruptionState corruptionState = myCharacterModel.CorruptionState;
		for (int i = 0; i < _view.Corruption_Normal_Value_Current.Length; i++)
		{
			_view.Corruption_Normal_Value_Current[i].SetActive(i == (int)corruptionState);
			if (i == (int)corruptionState)
			{
				_view.Corruption_Normal_Value_Current[i].GetComponent<TextMeshProUGUI>().text = corruption.ToString();
			}
		}
		for (int j = 0; j < _view.Corruption_Normal_Value_Icon.Length; j++)
		{
			_view.Corruption_Normal_Value_Icon[j].SetActive(j == (int)corruptionState);
		}
		_view.Corruption_Normal_Value_Max.text = string.Format("/{0}", corruptionMax);
	}

	private void InitGamePadNav()
	{
		_equipmentModel.OutOfBottom.Subscribe(delegate
		{
			FocusScrollerOrEmpty(ENavDir.Down);
		}).AddTo(_disposable);
		_scrollerModel.OutOfTop.Subscribe(delegate
		{
			FocusPanel(EFocusPanel.Equipment, ENavDir.Up);
		}).AddTo(_disposable);
		_emptyPanelModel.OutOfTop.Subscribe(delegate
		{
			FocusPanel(EFocusPanel.Equipment, ENavDir.Up);
		}).AddTo(_disposable);
		if (!_isSafeInventoryUnlocked)
		{
			_scrollerModel.OutOfBottom.Subscribe(delegate
			{
				TriggerOutOfBottom();
			}).AddTo(_disposable);
			_emptyPanelModel.OutOfBottom.Subscribe(delegate
			{
				TriggerOutOfBottom();
			}).AddTo(_disposable);
		}
		else
		{
			_scrollerModel.OutOfBottom.Subscribe(delegate
			{
				FocusPanel(EFocusPanel.Safe, ENavDir.Down);
			}).AddTo(_disposable);
			_emptyPanelModel.OutOfBottom.Subscribe(delegate
			{
				FocusPanel(EFocusPanel.Safe, ENavDir.Down);
			}).AddTo(_disposable);
			_safeModel.OutOfTop.Subscribe(delegate
			{
				FocusScrollerOrEmpty(ENavDir.Up);
			}).AddTo(_disposable);
			_safeModel.OutOfRight.Subscribe(delegate
			{
				TriggerOutOfRight();
			}).AddTo(_disposable);
		}
		_equipmentModel.OutOfRight.Subscribe(delegate
		{
			TriggerOutOfRight();
		}).AddTo(_disposable);
		_scrollerModel.OutOfRight.Subscribe(delegate
		{
			TriggerOutOfRight();
		}).AddTo(_disposable);
		_emptyPanelModel.OutOfRight.Subscribe(delegate
		{
			TriggerOutOfRight();
		}).AddTo(_disposable);
	}

	private void FocusScrollerOrEmpty(ENavDir dir)
	{
		FocusPanel(_scrollerModel.RxIsEmpty.Value ? EFocusPanel.Empty : EFocusPanel.Scroller, dir);
	}

	private void FocusPanel(EFocusPanel panel, ENavDir dir)
	{
		switch (panel)
		{
		case EFocusPanel.Equipment:
			_equipmentModel.EntryDirection = dir;
			break;
		case EFocusPanel.Scroller:
			_scrollerModel.EntryDirection = dir;
			break;
		case EFocusPanel.Empty:
			_emptyPanelModel.EntryDirection = dir;
			break;
		case EFocusPanel.Safe:
			if (_isSafeInventoryUnlocked)
			{
				_safeModel.EntryDirection = dir;
			}
			break;
		}
		SetPanel();

		void SetPanel()
		{
			_model.FocusPanel = panel;
			_equipmentModel.RxIsInputEnabled.Value = panel == EFocusPanel.Equipment;
			_scrollerModel.RxIsInputEnabled.Value = panel == EFocusPanel.Scroller;
			_emptyPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.Empty;
			if (_isSafeInventoryUnlocked)
			{
				_safeModel.RxIsInputEnabled.Value = panel == EFocusPanel.Safe;
			}
		}
	}
}
