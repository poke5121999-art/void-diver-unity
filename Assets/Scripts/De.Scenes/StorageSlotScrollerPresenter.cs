using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class StorageSlotScrollerPresenter : VerticalScrollerPresenter<StorageSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private StorageSlotScrollerModel _model;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<StorageSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(StorageSlotScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		RefreshScroller();
		_playerManager.OnStorageChanged.Subscribe(delegate
		{
			OnStorageChanged();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override List<StorageSlotModel> BuildSlotModels()
	{
		return _playerManager.StorageSlots.Values.Select((GoodsSlot slot) => new StorageSlotModel
		{
			Index = slot.SlotIndex,
			Goods = slot.Goods
		}).ToList();
	}

	private void OnStorageChanged()
	{
		RefreshScroller(base.Scroller.ScrollPosition);
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		baseCell.SetData(base.CellViewModels[dataIndex]);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return _cellSize;
	}
}
