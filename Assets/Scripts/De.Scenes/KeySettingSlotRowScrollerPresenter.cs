using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;

public class KeySettingSlotRowScrollerPresenter : GridScrollerPresenter<KeySettingSlotModel, KeySettingSlotRowModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[SerializeField]
	private int _slotsPerRow;

	[InjectModel]
	private KeySettingSlotRowScrollerModel _model;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<KeySettingSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(KeySettingSlotRowScrollerModel model)
	{
	}

	protected override KeySettingSlotModel[] BuildSlotModels()
	{
		return null;
	}

	protected override KeySettingSlotRowModel[] BuildRowModels(KeySettingSlotModel[] slots)
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
