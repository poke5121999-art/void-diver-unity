using System.Collections.Generic;
using System.Linq;
using GameCommon.Table;
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

	protected override BaseScrollerModel<BgmSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(BgmSlotScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		RefreshScroller();
		model.RxIsInputEnabled.Where((bool enabled) => enabled).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool enabled) => !enabled).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override List<BgmSlotModel> BuildSlotModels()
	{
		return (from id in TLoungeBgm.Table.Keys
			orderby _playerManager.IsLoungeBgmUnlocked(id) descending, id
			select new BgmSlotModel
			{
				BgmId = id
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
