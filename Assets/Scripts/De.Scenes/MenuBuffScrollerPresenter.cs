using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;

public class MenuBuffScrollerPresenter : VerticalScrollerPresenter<MenuBuffSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private MenuBuffScrollerModel _model;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<MenuBuffSlotModel> ScrollerModel => _model;

	private CharacterModel CharacterModel => _model.CharacterModel;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(MenuBuffScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		UpdateSlots();
		_model.CharacterModel.RxBuffs.ObserveChange().DelayFrame(1).Subscribe(delegate
		{
			UpdateSlots();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override List<MenuBuffSlotModel> BuildSlotModels()
	{
		List<MenuBuffSlotModel> list = new List<MenuBuffSlotModel>();
		foreach (UnitBuff value in _model.CharacterModel.RxBuffs.Values)
		{
			if (TBuff.Table[value.Id].ShowBuffIcon && value.IsActive && value.StackCount > 0)
			{
				list.Add(new MenuBuffSlotModel
				{
					UnitBuff = value
				});
			}
		}
		return list;
	}

	private void UpdateSlots()
	{
		RefreshScroller();
		_model.RxVisibleCount.Value = CellViewModels.Count;
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
