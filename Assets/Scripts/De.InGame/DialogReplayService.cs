using GameCommon.Enum;
using NemoLib.Log;
using VContainer;

public class DialogReplayService : NativeBase
{
	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private SeriLogger _logger;

	public bool HasVisibleDialog(EQuestType questType, int tableId, bool displayFullDialog)
	{
		return false;
	}

	public long[] GetVisibleDialogReplayIds(EQuestType questType, int tableId, bool displayFullDialog)
	{
		return null;
	}

	private long[] GetDialogReplayIds(int questTableId)
	{
		return null;
	}

	private long GetNextDialogReplayId(long dialogReplayId)
	{
		return 0L;
	}

	public void SetDialogReplayRecord(string message, int selectedIndex)
	{
	}

	private string ExtractLocalTextKey(string text)
	{
		return null;
	}

	private void SetCurrentDialogReplayId(long dialogReplayId)
	{
	}
}
