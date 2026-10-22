using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class BgmSlotScrollerPresenter : VerticalScrollerPresenter<BgmSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private BgmSlotScrollerModel _model;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<BgmSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(BgmSlotScrollerModel model)
	{
	}

	protected override List<BgmSlotModel> BuildSlotModels()
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
