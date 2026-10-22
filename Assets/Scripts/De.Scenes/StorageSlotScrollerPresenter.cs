using System.Collections.Generic;
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

	protected override BaseScrollerModel<StorageSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(StorageSlotScrollerModel model)
	{
	}

	protected override List<StorageSlotModel> BuildSlotModels()
	{
		return null;
	}

	private void OnStorageChanged()
	{
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		return null;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 0f;
	}
}
