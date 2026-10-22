using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class StashPanelPresenter : BasePanelPresenter
{
	public enum EFocusPanel
	{
		None = 0,
		Filter = 1,
		Scroller = 2,
		Empty = 3
	}

	[InjectModel]
	private StashPanelModel _model;

	[View]
	private StashPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private InventoryGoodsSlotModel _cachedSelectedSlot;

	private StashScrollerModel _scrollerModel;

	private EmptyPanelModel _emptyPanelModel;

	private StashFilterPanelModel _filterPanelModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	private GamePlayer Player => _gameManager.MyPlayer;

	private EFocusPanel FocusedPanel => _model.FocusPanel;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void Start()
	{
		_view.Page_PrevButton.OnClickWithSfx().Subscribe(delegate
		{
			OnPrevPageButtonClick();
		}).AddTo(this);
		_view.Page_NextButton.OnClickWithSfx().Subscribe(delegate
		{
			OnNextPageButtonClick();
		}).AddTo(this);
	}

	protected void OnInject(StashPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_filterPanelModel = null;
		if (_view.StashFilterPanel != null)
		{
			_view.StashFilterPanel.gameObject.SetActive(model.EnableFilterButtons);
			if (model.EnableFilterButtons)
			{
				_filterPanelModel = new StashFilterPanelModel();
				_filterPanelModel.RxAllowedSubTypes.Value = model.AllowedSubtypes;
				_view.StashFilterPanel.Inject(_filterPanelModel);
			}
		}
		_scrollerModel = new StashScrollerModel
		{
			RxAllowedSubTypes = new ReactiveProperty<EGoodsType[]>(model.AllowedSubtypes),
			ArtifactFilterTypes = model.ArtifactFilterTypes
		};
		if (_filterPanelModel != null)
		{
			_filterPanelModel.RxAllowedSubTypes.Subscribe(delegate(EGoodsType[] types)
			{
				_scrollerModel.RxAllowedSubTypes.Value = types;
			}).AddTo(_disposable);
			_filterPanelModel.OnUserToggled.Subscribe(delegate(EGoodsType[] types)
			{
				_model.OnFilterChanged.OnNext(types);
			}).AddTo(_disposable);
			_model.OnFilterRestoreRequest.Subscribe(delegate(EGoodsType[] types)
			{
				_filterPanelModel.RxAllowedSubTypes.Value = types;
			}).AddTo(_disposable);
		}
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
		_scrollerModel.OnSlotDrag.Subscribe(delegate(InventoryGoodsSlotModel slot)
		{
			_model.OnSlotDrag.OnNext(slot);
		}).AddTo(_disposable);
		_scrollerModel.OnSlotEndDrag.Subscribe(delegate(InventoryGoodsSlotModel slot)
		{
			_model.OnSlotEndDrag.OnNext(slot);
		}).AddTo(_disposable);
		_view.Scroller.Inject(_scrollerModel);
		_emptyPanelModel = new EmptyPanelModel
		{
			Text = TLocalizedText.NoAvailableGoods.LocalText
		};
		_view.EmptyPanel.Inject(_emptyPanelModel);
		UpdatePagination();
		UpdateSlotCounts();
		_playerManager.OnStashChanged.Subscribe(delegate
		{
			OnStashChanged();
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

	private void OnPageChanged(int pageIndex)
	{
		UpdatePagination();
	}

	private void OnPageCountChanged(int pageCount)
	{
		UpdatePagination();
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

	private void OnPrevPageButtonClick()
	{
		if (_scrollerModel.RxPageIndex.Value > 0)
		{
			_scrollerModel.OnMovePage.OnNext(_scrollerModel.RxPageIndex.Value - 1);
		}
	}

	private void OnNextPageButtonClick()
	{
		if (_scrollerModel.RxPageIndex.Value < _scrollerModel.PageCount - 1)
		{
			_scrollerModel.OnMovePage.OnNext(_scrollerModel.RxPageIndex.Value + 1);
		}
	}

	private void OnFocusedSlotChanged(InventoryGoodsSlotModel slotModel)
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

	private void OnSlotsEmpty(bool isEmpty)
	{
		_view.EmptyPanel.gameObject.SetActive(isEmpty);
		if (!IsInputEnabled)
		{
			return;
		}
		if (isEmpty)
		{
			if (_model.FocusPanel == EFocusPanel.Scroller)
			{
				_emptyPanelModel.EntryDirection = ENavDir.None;
				_model.FocusPanel = EFocusPanel.Empty;
				if (_filterPanelModel != null)
				{
					_filterPanelModel.RxIsInputEnabled.Value = false;
				}
				_scrollerModel.RxIsInputEnabled.Value = false;
				_emptyPanelModel.RxIsInputEnabled.Value = true;
			}
		}
		else if (_model.FocusPanel == EFocusPanel.Empty)
		{
			Observable.NextFrame().Subscribe(delegate
			{
				FocusScrollerIfEmptyFocused();
			}).AddTo(_disposable);
		}
	}

	private void FocusScrollerIfEmptyFocused()
	{
		if (IsInputEnabled && _model.FocusPanel == EFocusPanel.Empty && !_scrollerModel.RxIsEmpty.Value)
		{
			_scrollerModel.EntryDirection = ENavDir.None;
			_model.FocusPanel = EFocusPanel.Scroller;
			if (_filterPanelModel != null)
			{
				_filterPanelModel.RxIsInputEnabled.Value = false;
			}
			_scrollerModel.RxIsInputEnabled.Value = true;
			_emptyPanelModel.RxIsInputEnabled.Value = false;
		}
	}

	private void OnStashChanged()
	{
		UpdateSlotCounts();
		RefreshSelectedSlot();
	}

	private void RefreshSelectedSlot()
	{
		InventoryGoodsSlotModel value = _model.RxFocusedSlotModel.Value;
		if (value == null)
		{
			return;
		}
		if (!_playerManager.StashSlots.TryGetValue(value.GoodsSlot.SlotIndex, out var value2))
		{
			OnFocusedSlotChanged(null);
		}
		else if (value2.Id != value.GoodsSlot.Id || value2.Count != value.GoodsSlot.Count)
		{
			OnFocusedSlotChanged(new InventoryGoodsSlotModel(EGoodsSlotCategory.StashSlot, value2));
		}
	}

	private void UpdateSlotCounts()
	{
		int num = _playerManager.StashSlots.Values.Count((GoodsSlot slot) => !slot.IsEmpty);
		_view.TItle_GoodsCountText.text = string.Format("{0}/{1}", num, _gameManager.MyPlayer.StashSlotCount);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		_scrollerModel.OutOfLeft.Subscribe(delegate
		{
			PanelModel.OutOfLeft.OnNext(Unit.Default);
		}).AddTo(_disposable);
		_scrollerModel.OutOfRight.Subscribe(delegate
		{
			PanelModel.OutOfRight.OnNext(Unit.Default);
		}).AddTo(_disposable);
		_emptyPanelModel.OutOfLeft.Subscribe(delegate
		{
			PanelModel.OutOfLeft.OnNext(Unit.Default);
		}).AddTo(_disposable);
		_emptyPanelModel.OutOfRight.Subscribe(delegate
		{
			PanelModel.OutOfRight.OnNext(Unit.Default);
		}).AddTo(_disposable);
		if (_filterPanelModel != null)
		{
			_filterPanelModel.OutOfBottom.Subscribe(OnFilterPanelOutOfBottom).AddTo(_disposable);
			_scrollerModel.OutOfTop.Subscribe(OnScrollerOutOfTop).AddTo(_disposable);
			_emptyPanelModel.OutOfTop.Subscribe(OnEmptyPanelOutOfTop).AddTo(_disposable);
			_filterPanelModel.OutOfLeft.Subscribe(delegate
			{
				PanelModel.OutOfLeft.OnNext(Unit.Default);
			}).AddTo(_disposable);
			_filterPanelModel.OutOfRight.Subscribe(delegate
			{
				PanelModel.OutOfRight.OnNext(Unit.Default);
			}).AddTo(_disposable);
		}
	}

	private void FocusPanel(EFocusPanel panel, ENavDir dir)
	{
		switch (panel)
		{
		case EFocusPanel.Filter:
			if (_filterPanelModel != null)
			{
				_filterPanelModel.EntryDirection = dir;
			}
			break;
		case EFocusPanel.Scroller:
			_scrollerModel.EntryDirection = dir;
			break;
		case EFocusPanel.Empty:
			_emptyPanelModel.EntryDirection = dir;
			break;
		}
		_model.FocusPanel = panel;
		if (_filterPanelModel != null)
		{
			_filterPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.Filter;
		}
		_scrollerModel.RxIsInputEnabled.Value = panel == EFocusPanel.Scroller;
		_emptyPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.Empty;
	}

	private void OnFilterPanelOutOfBottom(Unit _)
	{
		EFocusPanel panel = (_scrollerModel.RxIsEmpty.Value ? EFocusPanel.Empty : EFocusPanel.Scroller);
		FocusPanel(panel, ENavDir.Down);
	}

	private void OnScrollerOutOfTop(Unit _)
	{
		FocusPanel(EFocusPanel.Filter, ENavDir.Up);
	}

	private void OnEmptyPanelOutOfTop(Unit _)
	{
		FocusPanel(EFocusPanel.Filter, ENavDir.Up);
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
		SuspendNav();
	}

	protected override void OnInputDisabled()
	{
		if (_filterPanelModel != null)
		{
			_filterPanelModel.RxIsInputEnabled.Value = false;
		}
		_scrollerModel.RxIsInputEnabled.Value = false;
		_emptyPanelModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}
}
