using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class ArchiveScrollerPresenter : VerticalScrollerPresenter<ArchiveSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private ArchiveScrollerModel _model;

	private CompositeDisposable _disposable;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private PlayerManager _playerManager;

	protected override BaseScrollerModel<ArchiveSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(ArchiveScrollerModel model)
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

	protected override List<ArchiveSlotModel> BuildSlotModels()
	{
		List<ArchiveSlotModel> list = new List<ArchiveSlotModel>();
		foreach (EArchiveCategory value in System.Enum.GetValues(typeof(EArchiveCategory)))
		{
			if (value != EArchiveCategory.None)
			{
				list.Add(new ArchiveSlotModel
				{
					Category = value
				});
			}
		}
		return list;
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
