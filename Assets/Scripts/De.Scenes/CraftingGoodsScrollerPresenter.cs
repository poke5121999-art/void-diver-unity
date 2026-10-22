using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class CraftingGoodsScrollerPresenter : VerticalScrollerPresenter<CraftingGoodsSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private CraftingGoodsScrollerModel _model;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<CraftingGoodsSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable.Dispose();
		base.OnDestroy();
	}

	private void OnInject(CraftingGoodsScrollerModel model)
	{
		_disposable.Dispose();
		_disposable = new CompositeDisposable();
		RefreshScroller();
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
		model.RxCraftableOnly.Skip(1).Subscribe(delegate
		{
			OnCraftableFilterChanged();
		}).AddTo(_disposable);
		_playerManager.OnOwnedGoodsChanged.Where((Unit _) => _model.RxCraftableOnly.Value).Subscribe(delegate
		{
			OnCraftableFilterChanged();
		}).AddTo(_disposable);
	}

	private void OnCraftableFilterChanged()
	{
		RefreshScroller(_model.ScrollPosition);
		RefreshNav(_model.FocusedIndex);
	}

	protected override void OnInputEnabled()
	{
		RefreshNav(_model.FocusedIndex);
		Scroller.SetScrollPositionImmediately(_model.ScrollPosition);
		base.OnInputEnabled();
	}

	protected override void OnSelect(int index)
	{
		base.OnSelect(index);
		_model.FocusedIndex = index;
		_model.ScrollPosition = Scroller.ScrollPosition;
	}

	public CraftingGoodsSlotModel FindSlotModel(int craftingId)
	{
		return CellViewModels.FirstOrDefault((CraftingGoodsSlotModel _) => _.CraftingId == craftingId);
	}

	public void NavigateToItem(int craftingId)
	{
		_model.FilterType = EGoodsType.None;
		_model.RxCraftableOnly.Value = false;
		RefreshScroller(_model.ScrollPosition);
		int num = CellViewModels.FindIndex((CraftingGoodsSlotModel m) => m.CraftingId == craftingId);
		if (num >= 0)
		{
			_model.FocusedIndex = num;
			_model.ScrollPosition = (float)num * _cellSize;
			RefreshScroller(_model.ScrollPosition);
			RefreshNav(num);
		}
	}

	public void ChangeFilter(EGoodsType subType)
	{
		_model.FilterType = subType;
		RefreshScroller(_model.ScrollPosition);
		RefreshNav(_model.FocusedIndex);
	}

	protected override List<CraftingGoodsSlotModel> BuildSlotModels()
	{
		IEnumerable<int> source = TCrafting.GroupTable[_model.CraftingGroupId].Where((TCrafting _) => _playerManager.CheckStateConditions(_.UnlockConditions)).Select((TCrafting _) => _.Id).OrderBy((int _) => _);
		switch (_model.FilterType)
		{
		case EGoodsType.Consumable:
		case EGoodsType.Weapon:
		case EGoodsType.Accessory:
			source = source.Where((int _) => TCrafting.Table[_].ResultGoodsData.Type == _model.FilterType);
			break;
		case EGoodsType.Misc:
			source = source.Where((int _) => TCrafting.Table[_].ResultGoodsData.Type switch
			{
				EGoodsType.Valuable => true,
				EGoodsType.Misc => true,
				EGoodsType.Note => true,
				EGoodsType.Blueprint => true,
				EGoodsType.Bag => true,
				_ => false,
			});
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case EGoodsType.None:
			break;
		}
		if (_model.RxCraftableOnly.Value)
		{
			source = source.Where((int _) => _playerManager.HasEnoughGoods(TCrafting.Table[_].MaterialGoodsDatas));
		}
		return source.Select((int _) => new CraftingGoodsSlotModel
		{
			CraftingId = _
		}).ToList();
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		baseCell.SetData(CellViewModels[dataIndex]);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return _cellSize;
	}
}
