using System.Collections.Generic;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using VContainer;

public class GameLoungeQuestManager : MonoBase
{
	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private PlayerManager _playerManager;

	private bool _isPlayerInitialized;

	private IDictionary<int, LoungeQuest> LoungeQuests => null;

	public Subject<LoungeQuest> OnQuestAdded { get; }

	public Subject<(LoungeQuest oldValue, LoungeQuest newValue)> OnQuestUpdated { get; }

	public Subject<LoungeQuest> OnQuestRemoved { get; }

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	public bool TryGetQuest(int questId, out LoungeQuest loungeQuest)
	{
		loungeQuest = null;
		return false;
	}

	public void AddQuest(int questId)
	{
	}

	public void RemoveQuest(int questId)
	{
	}

	private void SetQuest(LoungeQuest loungeQuest)
	{
	}

	public void SetDisplayInHud(int questId, bool displayInHud)
	{
	}

	public void SetQuestState(int questId, ELoungeQuestState state)
	{
	}

	public void CompleteQuest(int questId)
	{
	}

	public int[] GetActiveQuestIds()
	{
		return null;
	}

	public int[] GetDisplayLoungeQuestIds()
	{
		return null;
	}

	public bool HasAnyActiveQuest()
	{
		return false;
	}

	public long GetQuestTaskProgress(int taskId)
	{
		return 0L;
	}

	public int GetLoungeQuestCompletedTaskCount(int questId)
	{
		return 0;
	}

	public int GetLoungeQuestTotalTaskCount(int questId)
	{
		return 0;
	}

	public bool IsAllQuestTaskCompleted(int questId)
	{
		return false;
	}

	public ELoungeQuestState GetQuestState(int questId)
	{
		return ELoungeQuestState.None;
	}

	public void SetCurrentDialogReplayId(long dialogReplayId)
	{
	}

	private void OnGameEventOccured(IBaseEvent gameEvent)
	{
	}
}
