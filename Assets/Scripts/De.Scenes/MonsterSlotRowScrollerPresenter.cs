using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;

public class MonsterSlotRowScrollerPresenter : GridScrollerPresenter<MonsterSlotModel, MonsterSlotRowModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[SerializeField]
	private int _slotsPerRow;

	[InjectModel]
	private MonsterSlotRowScrollerModel _model;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<MonsterSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(MonsterSlotRowScrollerModel model)
	{
	}

	protected override MonsterSlotModel[] BuildSlotModels()
	{
		return null;
	}

	protected override MonsterSlotRowModel[] BuildRowModels(MonsterSlotModel[] slots)
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
