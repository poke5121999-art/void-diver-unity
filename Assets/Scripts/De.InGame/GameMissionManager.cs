using System.Collections.Generic;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using VContainer;

public class GameMissionManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerManager _playerManager;

	public readonly Dictionary<int, Mission> Missions;

	public readonly Subject<(int missionId, int Step)> OnMissionStepChanged;

	public readonly Subject<int> OnMissionAchieved;

	public readonly Subject<(int missionId, EMissionState state)> OnMissionStateChanged;

	public readonly Subject<Unit> OnMissionStarted;

	public readonly Subject<int> OnMissionUpdated;

	public IEnumerable<int> MissionIds => null;

	public bool HasMission => false;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnStageSceneLoad()
	{
	}

	private void OnStageClosed()
	{
	}

	private void OnStartMission(NtfStartMission message)
	{
	}

	private void OnEndMission(NtfEndMission message)
	{
	}

	private void OnGameClosed(Unit _)
	{
	}

	private void OnGameEventOccured(IBaseEvent triggerEvent)
	{
	}

	public int GetMissionStep(int missionId)
	{
		return 0;
	}

	public void SetMissionStep(int missionId, int stepId)
	{
	}

	public EMissionState GetMissionState(int missionId)
	{
		return EMissionState.None;
	}

	public void SetMissionState(int missionId, EMissionState state)
	{
	}

	public void CompleteMission(int missionId)
	{
	}

	private int[] PickRandomMissionIds()
	{
		return null;
	}

	public void SetCurrentDialogReplayId(long dialogReplayId)
	{
	}
}
