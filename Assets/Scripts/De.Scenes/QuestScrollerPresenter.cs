using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using GameCommon.Enum;
using GameCommon.Table;
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

	protected override BaseScrollerModel<QuestSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(QuestScrollerModel model)
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

	protected override List<QuestSlotModel> BuildSlotModels()
	{
		return CreateFilteredSlotModels();
	}

	private List<QuestSlotModel> CreateFilteredSlotModels()
	{
		switch (_model.FilterType)
		{
		case EFilterType.AllQuestsInProgress:
			return CreateAllQuestsInProgressSlotModels();
		case EFilterType.ClearedCampaign:
			return CreateClearedCampaignSlotModels();
		case EFilterType.ClearedMission:
			return CreateClearedMissionSlotModels();
		case EFilterType.ClearedLounge:
			return CreateClearedLoungeSlotModels();
		default:
			return new List<QuestSlotModel>();
		}
		List<QuestSlotModel> CreateAllQuestsInProgressSlotModels()
		{
			List<QuestSlotModel> list = new List<QuestSlotModel>();
			if (_gameContext.CampaignId < 1)
			{
				foreach (int missionId in _missionManager.MissionIds)
				{
					if (_missionManager.Missions.TryGetValue(missionId, out var value) && value.IsStarted)
					{
						list.Add(new QuestSlotModel
						{
							QuestType = EQuestType.Mission,
							TableId = missionId
						});
					}
				}
				int[] array = _loungeQuestManager.GetActiveQuestIds().Where((int _) => TLoungeQuest.Table[_].Display).ToArray();
				foreach (int tableId in array)
				{
					list.Add(new QuestSlotModel
					{
						QuestType = EQuestType.Lounge,
						TableId = tableId
					});
				}
			}
			else
			{
				list.Add(new QuestSlotModel
				{
					QuestType = EQuestType.Campaign,
					TableId = _gameContext.CampaignId
				});
			}
			return list;
		}
		List<QuestSlotModel> CreateClearedCampaignSlotModels()
		{
			return TCampaign.Table.Keys.Where((int id) => _playerManager.IsClearedCampaign(id)).Select((int _) => new QuestSlotModel
			{
				QuestType = EQuestType.Campaign,
				TableId = _
			}).OrderBy((QuestSlotModel _) => _.TableId)
				.ToList();
		}
		List<QuestSlotModel> CreateClearedMissionSlotModels()
		{
			return TMission.Table.Keys.Where((int id) => _playerManager.IsClearedMission(id)).Select((int _) => new QuestSlotModel
			{
				QuestType = EQuestType.Mission,
				TableId = _
			}).OrderBy((QuestSlotModel _) => _.TableId)
				.ToList();
		}
		List<QuestSlotModel> CreateClearedLoungeSlotModels()
		{
			return TLoungeQuest.Table.Keys.Where((int id) => _playerManager.IsClearedLoungeQuest(id)).Select((int _) => new QuestSlotModel
			{
				QuestType = EQuestType.Lounge,
				TableId = _
			}).OrderBy((QuestSlotModel _) => _.TableId)
				.ToList();
		}
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		baseCell.SetData(base.CellViewModels[dataIndex]);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return _cellSize;
	}
}
