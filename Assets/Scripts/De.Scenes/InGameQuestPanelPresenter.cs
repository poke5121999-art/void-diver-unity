using System.Collections.Generic;
using System.Linq;
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

	private Dictionary<int, GameObject> _loungeQuestSlotGos = new Dictionary<int, GameObject>();

	private Dictionary<int, GameObject> _missionSlotGos = new Dictionary<int, GameObject>();

	private void Start()
	{
		_campaignManager.OnCampaignStarted.Subscribe(OnCampaignStarted).AddTo(this);
		_campaignManager.OnCampaignClosed.Subscribe(OnCampaignClosed).AddTo(this);
		_campaignManager.RxState.Where((ECampaignState _) => _ == ECampaignState.Completed).Subscribe(delegate
		{
			DestroyCampaignSlot();
		}).AddTo(this);
		_loungeQuestManager.OnQuestAdded.Subscribe(OnLoungeQuestAdded).AddTo(this);
		_loungeQuestManager.OnQuestUpdated.Subscribe(OnLoungeQuestUpdated).AddTo(this);
		_loungeQuestManager.OnQuestRemoved.Subscribe(OnLoungeQuestRemoved).AddTo(this);
		_missionManager.OnMissionStarted.Subscribe(delegate
		{
			OnMissionStarted();
		}).AddTo(this);
		_missionManager.OnMissionUpdated.Subscribe(OnMissionUpdated).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.LoadCompleted).Subscribe(delegate
		{
			RebuildAllSlots();
		}).AddTo(this);
	}

	private void OnEnable()
	{
		RebuildAllSlots();
	}

	private void BuildCampaignSlot()
	{
		if (_campaignSlotGo != null)
		{
			Object.Destroy(_campaignSlotGo);
		}
		if (_gameContext.CampaignId >= 1 && _campaignManager.State != ECampaignState.Completed)
		{
			_campaignSlotGo = Object.Instantiate(_view.HudCampaignQuestSlot);
			_campaignSlotGo.SetActive(value: true);
			HudCampaignQuestSlotPresenter component = _campaignSlotGo.GetComponent<HudCampaignQuestSlotPresenter>();
			HudCampaignQuestSlotModel model = new HudCampaignQuestSlotModel
			{
				CampaignId = _gameContext.CampaignId
			};
			component.Inject(model);
			_campaignSlotGo.transform.SetParent(_view.CampaignAndLoungeRoot.transform, worldPositionStays: false);
			_campaignSlotGo.transform.SetSiblingIndex(0);
		}
	}

	private void DestroyCampaignSlot()
	{
		if (!(_campaignSlotGo == null))
		{
			Object.Destroy(_campaignSlotGo);
			_campaignSlotGo = null;
		}
	}

	private void BuildLoungeQuestSlots()
	{
		foreach (GameObject value in _loungeQuestSlotGos.Values)
		{
			Object.Destroy(value);
		}
		_loungeQuestSlotGos.Clear();
		foreach (int displayLoungeQuestId in _loungeQuestManager.GetDisplayLoungeQuestIds())
		{
			AddLoungeQuestSlot(displayLoungeQuestId);
		}
	}

	private void AddLoungeQuestSlot(int loungeQuestId)
	{
		if (!_loungeQuestSlotGos.ContainsKey(loungeQuestId))
		{
			GameObject gameObject = Object.Instantiate(_view.HudLoungeQuestSlot);
			gameObject.SetActive(value: true);
			HudLoungeQuestSlotPresenter component = gameObject.GetComponent<HudLoungeQuestSlotPresenter>();
			HudLoungeQuestSlotModel model = new HudLoungeQuestSlotModel
			{
				LoungeQuestId = loungeQuestId
			};
			component.Inject(model);
			gameObject.transform.SetParent(_view.CampaignAndLoungeRoot.transform, worldPositionStays: false);
			gameObject.transform.SetSiblingIndex(100);
			_loungeQuestSlotGos.Add(loungeQuestId, gameObject);
		}
	}

	private void RemoveLoungeQuestSlot(int loungeQuestId)
	{
		if (_loungeQuestSlotGos.TryGetValue(loungeQuestId, out var value))
		{
			Object.Destroy(value);
			_loungeQuestSlotGos.Remove(loungeQuestId);
		}
	}

	private void BuildMissionSlots()
	{
		if (_gameManager.IsInLounge)
		{
			return;
		}
		_view.Mission.gameObject.SetActive(value: true);
		foreach (GameObject value in _missionSlotGos.Values)
		{
			Object.Destroy(value);
		}
		_missionSlotGos.Clear();
		foreach (int item in _missionManager.MissionIds.OrderBy((int _) => _).ToArray())
		{
			AddMissionSlot(item);
		}
		UpdateMissionHeader();
	}

	private void AddMissionSlot(int missionId)
	{
		if (_missionSlotGos.ContainsKey(missionId) || !_missionManager.Missions[missionId].IsStarted)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate(_view.HudMissionQuestSlot);
		gameObject.SetActive(value: true);
		HudMissionQuestSlotPresenter component = gameObject.GetComponent<HudMissionQuestSlotPresenter>();
		HudMissionQuestSlotModel model = new HudMissionQuestSlotModel
		{
			MissionId = missionId
		};
		component.Inject(model);
		gameObject.transform.SetParent(_view.Mission.transform, worldPositionStays: false);
		_missionSlotGos.Add(missionId, gameObject);
		UpdateMissionHeader();
	}

	private void UpdateMissionHeader()
	{
		_view.Mission_Caption.gameObject.SetActive(_missionSlotGos.Count > 0);
	}

	private void RebuildAllSlots()
	{
		BuildCampaignSlot();
		BuildLoungeQuestSlots();
		BuildMissionSlots();
	}

	private void OnStageLoadCompleted()
	{
		RebuildAllSlots();
	}

	private void OnCampaignStarted((int campaignId, EDifficulty difficulty) campaignData)
	{
		BuildCampaignSlot();
	}

	private void OnCampaignClosed(Unit unit)
	{
		DestroyCampaignSlot();
	}

	private void OnLoungeQuestAdded(LoungeQuest loungeQuest)
	{
		RefreshLoungeQuestSlot(loungeQuest);
	}

	private void OnLoungeQuestRemoved(LoungeQuest loungeQuest)
	{
		RemoveLoungeQuestSlot(loungeQuest.Id);
	}

	private void OnLoungeQuestUpdated((LoungeQuest oldValue, LoungeQuest newValue) quest)
	{
		RefreshLoungeQuestSlot(quest.newValue);
	}

	private void RefreshLoungeQuestSlot(LoungeQuest loungeQuest)
	{
		if (loungeQuest.DisplayInHud && loungeQuest.IsActive)
		{
			AddLoungeQuestSlot(loungeQuest.Id);
		}
		else
		{
			RemoveLoungeQuestSlot(loungeQuest.Id);
		}
	}

	private void OnMissionStarted()
	{
		BuildMissionSlots();
	}

	private void OnMissionUpdated(int missionId)
	{
		AddMissionSlot(missionId);
	}
}
