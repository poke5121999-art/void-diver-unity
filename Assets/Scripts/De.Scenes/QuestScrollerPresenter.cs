using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class QuestScrollerPresenter : VerticalScrollerPresenter<QuestSlotModel>
{
	public enum EFilterType
	{
		None = 0,
		AllQuestsInProgress = 1,
		ClearedCampaign = 2,
		ClearedMission = 3,
		ClearedLounge = 4
	}

	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private QuestScrollerModel _model;

	private CompositeDisposable _disposable;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private PlayerManager _playerManager;

	protected override BaseScrollerModel<QuestSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(QuestScrollerModel model)
	{
	}

	protected override List<QuestSlotModel> BuildSlotModels()
	{
		return null;
	}

	private List<QuestSlotModel> CreateFilteredSlotModels()
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
