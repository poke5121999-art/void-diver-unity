using System;
using System.Linq;
using GameCommon.Enum;
using NemoLib.Log;
using SomeNet.Collections;
using UniRx;
using VContainer;

public class GameLuaController : MonoBase
{
	private const string STEP = "Step";

	private const string START_LOUNGE_FUNC = "OnLounge";

	private const string START_STAGE_FUNC = "OnStage";

	private const string EVENT_FUNC = "OnEvent";

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private ILuaRunner _luaRunner;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private GameNpcTalkManager _npcTalkManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerManager _playerManager;

	private string CampaignLuaKey
	{
		get
		{
			if (_gameContext.CampaignId < 1)
			{
				return "Campaign/None";
			}
			return string.Format("Campaign/{0}", _gameContext.CampaignId);
		}
	}

	private ulong MyClientId => _gameManager.MyCharacterModel.SyncClientId.Value;

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameChannel.Receive<NtfStartInteraction>().Subscribe(OnStartInteraction).AddTo(this);
		_gameChannel.Receive<NtfCollisionTriggerEnter>().Subscribe(OnCollisionTriggerEnter).AddTo(this);
		_gameChannel.Receive<NtfWaveStarted>().Subscribe(OnWaveStarted).AddTo(this);
		_gameChannel.Receive<NtfWaveFinished>().Subscribe(OnWaveFinished).AddTo(this);
		_gameChannel.Receive<NtfTriggerLuaEvent>().Subscribe(OnTriggerLuaEvent).AddTo(this);
		_eventSystem.OnEventOccured.Subscribe(OnEventOccured).AddTo(this);
		_campaignManager.RxCampaignStepId.Skip(1).Subscribe(OnQuestStepChanged).AddTo(this);
		_campaignManager.RxState.Skip(1).Subscribe(OnCampaignStateChanged).AddTo(this);
		_campaignManager.OnTaskAchieved.Subscribe(OnCampaignTaskAchieved).AddTo(this);
		_missionManager.OnMissionAchieved.Subscribe(OnMissionAchieved).AddTo(this);
		_missionManager.OnMissionStepChanged.Subscribe(delegate((int missionId, int Step) _)
		{
			OnMissionStepChanged(_.missionId, _.Step);
		}).AddTo(this);
		_missionManager.OnMissionStateChanged.Subscribe(delegate((int missionId, EMissionState state) _)
		{
			OnMissionStateChanged(_.missionId, _.state);
		}).AddTo(this);
		_loungeQuestManager.OnQuestAdded.Subscribe(OnLoungeQuestAdded).AddTo(this);
		_loungeQuestManager.OnQuestUpdated.Subscribe(OnLoungeQuestUpdated).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Playing).Subscribe(delegate
		{
			OnStageStarted();
		}).AddTo(this);
		_gameManager.OnGameClosed.Subscribe(delegate
		{
			_luaRunner.StopAllScripts();
		}).AddTo(this);
	}

	private string GetMissionLuaKey(int missionId)
	{
		if (!_missionManager.HasMission)
		{
			return null;
		}
		return string.Format("Mission/{0}", missionId);
	}

	public void RunLua(string functionName, params object[] args)
	{
		RunCampaignLua(functionName, args);
		RunMissionLua(functionName, args);
		RunLoungeQuestLua(functionName, args);
	}

	public void RunLua(string luaKey, string functionName, params object[] args)
	{
		_luaRunner.RunLua(luaKey, functionName, args);
	}

	private void RunCampaignLua(string functionName, params object[] args)
	{
		_luaRunner.RunLua(CampaignLuaKey, functionName, args);
	}

	private void RunMissionLua(int missionId, string functionName, params object[] args)
	{
		_luaRunner.RunLua(GetMissionLuaKey(missionId), functionName, args);
	}

	private void RunMissionLua(string functionName, params object[] args)
	{
		if (_missionManager.HasMission)
		{
			_missionManager.MissionIds.ForEach(delegate(int id)
			{
				RunMissionLua(id, functionName, args);
			});
		}
	}

	private void RunLoungeQuestLua(string functionName, params object[] args)
	{
		_playerManager.GetLoungeQuestIds().ForEach(delegate(int id)
		{
			RunLoungeQuestLua(id, functionName, args);
		});
	}

	private void RunLoungeQuestLua(int loungeQuestId, string functionName, params object[] args)
	{
		_luaRunner.RunLua(string.Format("LoungeQuest/{0}", loungeQuestId), functionName, args);
	}

	private void OnStageStarted()
	{
		RunLua(_gameManager.IsInLounge ? "OnLounge" : "OnStage");
		if (_gameManager.IsInLounge)
		{
			InitLoungeQuestLuaStates();
		}
	}

	private void InitLoungeQuestLuaStates()
	{
		foreach (LoungeQuest loungeQuest in _playerManager.GetLoungeQuests())
		{
			RunLoungeQuestLua(loungeQuest.Id, "OnEvent", ELuaEvent.LoungeQuestStateChanged, (int)loungeQuest.State);
		}
	}

	private void OnStartInteraction(NtfStartInteraction message)
	{
		IInteractiveObject interactiveObject = _objectManager.FindInteractiveObject(message.TargetNetworkObjectId);
		if (interactiveObject is NpcObject npcObject)
		{
			OnNpcObject(npcObject);
		}
		else if (interactiveObject is InteractiveTrigger interactiveTrigger)
		{
			OnInteractiveTrigger(interactiveTrigger);
		}

		void OnInteractiveTrigger(InteractiveTrigger interactiveTrigger2)
		{
			if (!interactiveTrigger2.IsIndividual || message.SourceClientId == MyClientId)
			{
				RunLua("OnEvent", ELuaEvent.TriggerInteraction, interactiveTrigger2.TriggerId);
			}
		}
		void OnNpcObject(NpcObject npcObject2)
		{
			if (npcObject2.NpcState != ENpcState.None && message.SourceClientId == MyClientId)
			{
				RunLua("OnEvent", ELuaEvent.NpcInteraction, npcObject2.NpcId);
			}
		}
	}

	private void OnCollisionTriggerEnter(NtfCollisionTriggerEnter message)
	{
		if (_objectManager.FindCollisionObject(message.TargetNetworkObjectId) is CollisionTrigger collisionTrigger)
		{
			OnCollisionTrigger(collisionTrigger);
		}

		void OnCollisionTrigger(CollisionTrigger collisionTrigger2)
		{
			if (!collisionTrigger2.IsIndividual || message.SourceClientId == MyClientId)
			{
				RunLua("OnEvent", ELuaEvent.CollisionTriggerEnter, collisionTrigger2.TriggerId);
			}
		}
	}

	private void OnWaveStarted(NtfWaveStarted message)
	{
		RunLua("OnEvent", ELuaEvent.WaveStarted, message.WaveId);
	}

	private void OnWaveFinished(NtfWaveFinished message)
	{
		RunLua("OnEvent", message.IsClear ? ELuaEvent.WaveCleared : ELuaEvent.WaveFailed, message.WaveId);
	}

	private void OnTriggerLuaEvent(NtfTriggerLuaEvent message)
	{
		RunLua("OnEvent", ELuaEvent.Custom, message.Value);
	}

	private void OnEventOccured(IBaseEvent gameEvent)
	{
		if (!(gameEvent is MonsterKillEvent monsterKillEvent))
		{
			if (!(gameEvent is ArtifactDealEvent artifactDealEvent))
			{
				if (gameEvent is LuaDialogEvent luaDialogEvent)
				{
					_luaRunner.RunLua(luaDialogEvent.LuaKey, luaDialogEvent.FunctionName);
				}
			}
			else
			{
				RunLua("OnEvent", ELuaEvent.ArtifactDeal, artifactDealEvent.NpcId);
			}
		}
		else
		{
			RunLua("OnEvent", ELuaEvent.MonsterKill, monsterKillEvent.MonsterId);
		}
	}

	private void OnQuestStepChanged(int stepId)
	{
		if (_gameManager.StageState == EStageState.Playing)
		{
			if (stepId < 1)
			{
				RunCampaignLua("OnLounge");
			}
			else
			{
				RunCampaignLua(string.Format("{0}_{1:D5}", "Step", stepId));
			}
		}
	}

	private void OnCampaignStateChanged(ECampaignState state)
	{
		RunCampaignLua("OnEvent", ELuaEvent.CampaignStateChanged, (int)state);
	}

	private void OnCampaignTaskAchieved(int taskId)
	{
		RunCampaignLua("OnEvent", ELuaEvent.CampaignTaskAchieved, taskId);
	}

	private void OnMissionStepChanged(int missionId, int stepId)
	{
		if (_gameManager.StageState == EStageState.Playing)
		{
			RunMissionLua(missionId, string.Format("{0}_{1:D5}", "Step", stepId));
		}
	}

	private void OnMissionStateChanged(int missionId, EMissionState state)
	{
		RunMissionLua(missionId, "OnEvent", ELuaEvent.MissionStateChanged, (int)state);
	}

	private void OnMissionAchieved(int missionId)
	{
		RunMissionLua(missionId, "OnEvent", ELuaEvent.MissionTaskAchieved);
	}

	private void OnLoungeQuestAdded(LoungeQuest quest)
	{
		RunLoungeQuestLua(quest.Id, "OnEvent", ELuaEvent.LoungeQuestStateChanged, (int)quest.State);
	}

	private void OnLoungeQuestUpdated((LoungeQuest oldValue, LoungeQuest newValue) data)
	{
		if (data.oldValue != null && data.newValue != null && data.oldValue.State != data.newValue.State)
		{
			RunLoungeQuestLua(data.newValue.Id, "OnEvent", ELuaEvent.LoungeQuestStateChanged, (int)data.newValue.State);
		}
	}
}
