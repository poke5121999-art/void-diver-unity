using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class PlayerRestoreSlotScrollerPresenter : VerticalScrollerPresenter<PlayerRestoreSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private PlayerRestoreSlotScrollerModel _model;

	[Inject]
	private StorageService _storageService;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<PlayerRestoreSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(PlayerRestoreSlotScrollerModel model)
	{
	}

	protected override List<PlayerRestoreSlotModel> BuildSlotModels()
	{
		return null;
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
