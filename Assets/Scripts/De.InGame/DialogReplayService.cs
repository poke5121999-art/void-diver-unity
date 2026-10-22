using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GameCommon.Enum;
using GameCommon.Table;
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
		long[] visibleDialogReplayIds = GetVisibleDialogReplayIds(questType, tableId, displayFullDialog);
		if (visibleDialogReplayIds == null)
		{
			return false;
		}
		return visibleDialogReplayIds.Length > 0;
	}

	public long[] GetVisibleDialogReplayIds(EQuestType questType, int tableId, bool displayFullDialog)
	{
		long[] dialogReplayIds = GetDialogReplayIds(tableId);
		if (displayFullDialog || !dialogReplayIds.Any())
		{
			return dialogReplayIds;
		}
		long currentDialogReplayId = GetVisibleDialogReplayMaxId();
		if (currentDialogReplayId < 0)
		{
			return null;
		}
		return dialogReplayIds.Where((long id) => id <= currentDialogReplayId).ToArray();

		long GetVisibleDialogReplayMaxId()
		{
			switch (questType)
			{
			case EQuestType.Campaign:
				if (_campaignManager.CampaignId != tableId)
				{
					return -1L;
				}
				return _campaignManager.CurrentDialogReplayId;
			case EQuestType.Mission:
			{
				if (!_missionManager.Missions.TryGetValue(tableId, out var mission))
				{
					return -1L;
				}
				return mission.CurrentDialogReplayId;
			}
			case EQuestType.Lounge:
			{
				if (!_loungeQuestManager.TryGetQuest(tableId, out var loungeQuest))
				{
					return -1L;
				}
				return loungeQuest.CurrentDialogReplayId;
			}
			default:
				return -1L;
			}
		}
	}

	private long[] GetDialogReplayIds(int questTableId)
	{
		if (!TDialogReplay.GroupTable.TryGetValue(questTableId, out var _))
		{
			return Array.Empty<long>();
		}
		TDialogReplay tDialogReplay = TDialogReplay.GroupTable[questTableId].First();
		List<long> ids = new List<long>();
		HashSet<long> visited = new HashSet<long>();
		Collect(tDialogReplay.Id);
		return ids.ToArray();

		void Collect(long id)
		{
			while (true)
			{
				if (!visited.Add(id))
				{
					_logger.Error(string.Format("[DialogReplay] Cycle 감지: questTableId={0}, id={1}", questTableId, id));
					return;
				}
				ids.Add(id);
				id = GetNextDialogReplayId(id);
				if (id < 0)
				{
					break;
				}
			}
		}
	}

	private long GetNextDialogReplayId(long dialogReplayId)
	{
		TDialogReplay tDialogReplay = TDialogReplay.Table[dialogReplayId];
		if (tDialogReplay.NextIds == null || tDialogReplay.NextIds.Length == 0)
		{
			return -1L;
		}
		long result = tDialogReplay.NextIds.FirstOrDefault();
		if (tDialogReplay.Selections != null && tDialogReplay.Selections.Length != 0 && _playerManager.DialogReplayRecords.TryGetValue(dialogReplayId, out var value) && value.SelectedIndex < tDialogReplay.Selections.Length)
		{
			return tDialogReplay.NextIds[value.SelectedIndex];
		}
		return result;
	}

	public void SetDialogReplayRecord(string message, int selectedIndex)
	{
		string text = ExtractLocalTextKey(message);
		long dialogReplayId = TDialogReplay.MakeKey(text);
		SetCurrentDialogReplayId(dialogReplayId);
		if (selectedIndex >= 0)
		{
			_playerManager.SetDialogReplayRecord(text, selectedIndex);
		}
	}

	private string ExtractLocalTextKey(string text)
	{
		Match match = new Regex("<Key:([A-Za-z0-9_]+)>(.*)", RegexOptions.Compiled | RegexOptions.Singleline).Match(text);
		if (!match.Success)
		{
			_logger.Error("\"" + text + "\" (expected format: <Key:####>)");
			return text;
		}
		return match.Groups[1].Value;
	}

	private void SetCurrentDialogReplayId(long dialogReplayId)
	{
		if (!TDialogReplay.Table.TryGetValue(dialogReplayId, out var value))
		{
			return;
		}
		switch (value.QuestType)
		{
		case EQuestType.Campaign:
			_campaignManager.SetCurrentDialogReplayId(dialogReplayId);
			break;
		case EQuestType.Mission:
			_missionManager.SetCurrentDialogReplayId(dialogReplayId);
			break;
		case EQuestType.Lounge:
			_loungeQuestManager.SetCurrentDialogReplayId(dialogReplayId);
			break;
		}
	}
}
