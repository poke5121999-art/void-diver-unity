using System.Collections.Generic;
using NemoLib.Log;
using VContainer;

public class GameNpcTalkManager : MonoBase
{
	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameNpcDialogManager _npcDialogManager;

	public readonly HashSet<int> ActiveTalkIds;

	private readonly Dictionary<string, int> _talkIdByDialogKey;

	private readonly HashSet<int> _nextActiveTalkIds;

	private readonly List<int> _addedTalkIds;

	private readonly List<int> _removedTalkIds;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void Clear()
	{
	}

	public bool IsNewDialog(string dialogKey)
	{
		return false;
	}

	public void MarkDialogViewed(string dialogKey)
	{
	}

	private void EvaluateAll()
	{
	}
}
