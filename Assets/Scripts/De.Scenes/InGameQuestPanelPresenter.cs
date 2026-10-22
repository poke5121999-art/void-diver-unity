using System.Collections.Generic;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class InGameQuestPanelPresenter : Presenter
{
	[View]
	private InGameQuestPanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private AssetManager _assetManager;

	private GameObject _campaignSlotGo;

	private Dictionary<int, GameObject> _loungeQuestSlotGos;

	private Dictionary<int, GameObject> _missionSlotGos;

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void BuildCampaignSlot()
	{
	}

	private void DestroyCampaignSlot()
	{
	}

	private void BuildLoungeQuestSlots()
	{
	}

	private void AddLoungeQuestSlot(int loungeQuestId)
	{
	}

	private void RemoveLoungeQuestSlot(int loungeQuestId)
	{
	}

	private void BuildMissionSlots()
	{
	}

	private void AddMissionSlot(int missionId)
	{
	}

	private void UpdateMissionHeader()
	{
	}

	private void RebuildAllSlots()
	{
	}

	private void OnStageLoadCompleted()
	{
	}

	private void OnCampaignStarted((int campaignId, EDifficulty difficulty) campaignData)
	{
	}

	private void OnCampaignClosed(Unit unit)
	{
	}

	private void OnLoungeQuestAdded(LoungeQuest loungeQuest)
	{
	}

	private void OnLoungeQuestRemoved(LoungeQuest loungeQuest)
	{
	}

	private void OnLoungeQuestUpdated((LoungeQuest oldValue, LoungeQuest newValue) quest)
	{
	}

	private void RefreshLoungeQuestSlot(LoungeQuest loungeQuest)
	{
	}

	private void OnMissionStarted()
	{
	}

	private void OnMissionUpdated(int missionId)
	{
	}
}
