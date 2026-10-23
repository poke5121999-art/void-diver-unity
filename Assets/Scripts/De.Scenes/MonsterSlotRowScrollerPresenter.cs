using System;
using System.Linq;
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
	private int _slotsPerRow = 7;

	[InjectModel]
	private MonsterSlotRowScrollerModel _model;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<MonsterSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return _slotsPerRow;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(MonsterSlotRowScrollerModel model)
	{
		_disposable?.Dispose();
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
	}

	protected override MonsterSlotModel[] BuildSlotModels()
	{
		return _model.MonsterIds?.Select((int _) => new MonsterSlotModel
		{
			MonsterId = _
		}).ToArray() ?? Array.Empty<MonsterSlotModel>();
	}

	protected override MonsterSlotRowModel[] BuildRowModels(MonsterSlotModel[] slots)
	{
		int num = (int)Math.Ceiling((float)slots.Length / (float)_slotsPerRow);
		MonsterSlotRowModel[] array = new MonsterSlotRowModel[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = new MonsterSlotRowModel
			{
				SlotModels = slots.Skip(i * _slotsPerRow).Take(_slotsPerRow).ToArray()
			};
		}
		return array;
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
