using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
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

	protected override BaseScrollerModel<ArchiveSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(ArchiveScrollerModel model)
	{
	}

	protected override List<ArchiveSlotModel> BuildSlotModels()
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
