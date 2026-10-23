using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class PlayerSaveSlotScrollerPresenter : VerticalScrollerPresenter<PlayerSaveSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private PlayerSaveSlotScrollerModel _model;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<PlayerSaveSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(PlayerSaveSlotScrollerModel model)
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

	protected override List<PlayerSaveSlotModel> BuildSlotModels()
	{
		return Enumerable.Range(0, _playerManager.PlayerSlots.Length).Select((int i) => new PlayerSaveSlotModel
		{
			PlayerSlotIndex = i
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
