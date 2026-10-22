using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using VContainer;

public class GameLoungeManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private SeriLogger _logger;

	public Subject<int> OnAreaUnlocked { get; }

	public Subject<(EAreaDecorationType areaDecorationType, int oldAreaDecorationSettingId, int newAreaDecorationSettingId)> OnAreaDecorationChanged { get; }

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnPrepareLoad()
	{
	}

	public void CheatResetInterior()
	{
	}

	private void OnPlayerEntered(GamePlayer player)
	{
	}

	public bool IsAreaUnlocked(int areaId)
	{
		return false;
	}

	private void InitAreaFlags()
	{
	}

	public void UnlockArea(int areaId)
	{
	}

	private void SendAreaFlags()
	{
	}

	private void OnNtfUpdateArea(NtfUpdateArea message)
	{
	}

	public bool IsAreaDecorationUnlocked(int areaDecorationSettingId)
	{
		return false;
	}

	public int GetActiveAreaDecoration(int groupId)
	{
		return 0;
	}

	private void InitAreaDecorationState()
	{
	}

	public void UnlockAreaDecoration(int areaDecorationSettingId)
	{
	}

	public void SetActiveAreaDecoration(int areaDecorationSettingId)
	{
	}

	private void SendAreaDecorationState()
	{
	}

	private void OnNtfUpdateAreaDecoration(NtfUpdateAreaDecoration message)
	{
	}

	public int GetNpcLevel(int npcId)
	{
		return 0;
	}

	public void InitNpcLevelState()
	{
	}

	public void SetNpcLevel(int npcId, int level)
	{
	}

	private void SendNpcLevelState()
	{
	}

	private void OnNtfUpdateNpcLevel(NtfUpdateNpcLevel message)
	{
	}
}
