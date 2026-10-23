using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TDialogReplay : ITableData<long>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static DialogReplayTable Table { get; set; } = new DialogReplayTable();

		public static GroupDialogReplayTable GroupTable { get; set; }

		public long Pk
		{
			get
			{
				return Id;
			}
			set
			{
			}
		}

		public long Id { get; set; }

		public EQuestType QuestType { get; set; }

		public int QuestTableId { get; set; }

		public ESpeaker Speaker { get; set; }

		public int SpeakerTableId { get; set; }

		[Localized]
		public string Message { get; set; }

		[Localized]
		public string[] Selections { get; set; }

		public long[] NextIds { get; set; }

		public bool IsEndSequence { get; set; }

		public Action<ITableData<long>> OnDataLoaded { get; set; }

		public static event Action OnTableChanged
		{
			[CompilerGenerated]
			add
			{
				m_OnTableChanged += value;
			}
			[CompilerGenerated]
			remove
			{
				m_OnTableChanged -= value;
			}
		}

		public void Parse(string[] columns)
		{
			Id = TableParse.Long(columns[0]);
			QuestType = TableParse.Enum<EQuestType>(columns[1]);
			QuestTableId = TableParse.Int(columns[2]);
			Speaker = TableParse.Enum<ESpeaker>(columns[3]);
			SpeakerTableId = TableParse.Int(columns[4]);
			Message = TableParse.Str(columns[5]);
			Selections = TableParse.Array<string>(columns[6]);
			NextIds = TableParse.Array<long>(columns[7]);
			IsEndSequence = TableParse.Bool(columns[8]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, QuestType, QuestTableId, Speaker, SpeakerTableId, Message, Selections, NextIds, IsEndSequence);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,QuestType,QuestTableId,Speaker,SpeakerTableId,Message:Localized,Selections:Localized,NextIds,IsEndSequence";
		}

		public static string GetTypesCsv()
		{
			return "long,EQuestType,int,ESpeaker,int,string,string[],long[],bool";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new DialogReplayTable();
			table.Load(new CsvTableLoader<long, TDialogReplay>(text));
			Table = table;
			GroupTable = new GroupDialogReplayTable();
			GroupTable.Init(table.Values.GroupBy(row => row.QuestTableId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
			InvokeTableChangeEvent();
		}

		public static void InvokeTableChangeEvent()
		{
			m_OnTableChanged?.Invoke();
		}

		public static bool ValidateTable()
		{
			bool result = true;
			if (Table != null)
			{
				foreach (TDialogReplay row in Table.Values)
				{
					if (!row.Validate())
					{
						result = false;
					}
				}
			}
			return result;
		}

		public static long MakeKey(string localizedKey)
		{
			if (string.IsNullOrEmpty(localizedKey))
			{
				return -1L;
			}
			string[] parts = localizedKey.Split('_');
			if (parts.Length < 3)
			{
				return -1L;
			}
			long prefix = (parts[0] == "LQ") ? 1000000 : ((parts[0] == "LM") ? 2000000 : ((parts[0] == "LL") ? 3000000 : 0));
			if (int.TryParse(parts[1], out int tableId) && int.TryParse(parts[2], out int index))
			{
				return (tableId + prefix) * 10000 + index;
			}
			return -1L;
		}

		public static long MakeKey(EQuestType questType, int tableId, int index)
		{
			return ((long)(int)questType * 1000000 + tableId) * 10000 + index;
		}

		public string GetSpeakerName(string playerName, int playerCharacterId)
		{
			switch (Speaker)
			{
			case ESpeaker.Player:
				return playerName;
			case ESpeaker.Character:
				return TCharacter.Table[SpeakerTableId].Name;
			case ESpeaker.Npc:
				return TNpc.Table[SpeakerTableId].Name;
			case ESpeaker.Monster:
				return TMonster.Table[SpeakerTableId].Name;
			case ESpeaker.PlayerCharacter:
				return TCharacter.Table[playerCharacterId].Name;
			default:
				return Speaker.ToString();
			}
		}

		public bool Validate()
		{
			bool result = ValidateQuestTableId() & ValidateSpeakerTableId() & ValidateNextIdRefs();
			result &= ValidateSelectionsCount();
			if (Id == System.Linq.Enumerable.Min(Table.Keys))
			{
				result &= ValidateNoCycles();
			}
			return result;
		}

		private bool ValidateQuestTableId()
		{
			Type tableType;
			switch (QuestType)
			{
			case EQuestType.Campaign:
				tableType = typeof(TCampaign);
				break;
			case EQuestType.Mission:
				tableType = typeof(TMission);
				break;
			case EQuestType.Lounge:
				tableType = typeof(TLoungeQuest);
				break;
			default:
				return true;
			}
			return this.ValidateRefKey("QuestTableId", tableType);
		}

		private bool ValidateSpeakerTableId()
		{
			Type tableType;
			switch (Speaker)
			{
			case ESpeaker.Character:
				tableType = typeof(TCharacter);
				break;
			case ESpeaker.Npc:
				tableType = typeof(TNpc);
				break;
			case ESpeaker.Monster:
				tableType = typeof(TMonster);
				break;
			default:
				return true;
			}
			return this.ValidateRefKey("SpeakerTableId", tableType);
		}

		private bool ValidateNextIdRefs()
		{
			if (NextIds == null || NextIds.Length == 0)
			{
				return true;
			}
			bool result = true;
			for (int i = 0; i < NextIds.Length; i++)
			{
				long nextId = NextIds[i];
				if (nextId >= 0 && !Table.ContainsKey(nextId))
				{
					this.LogError(string.Format("[NextIds] {0} not in TDialogReplay.Table", nextId));
					result = false;
				}
			}
			return result;
		}

		private bool ValidateSelectionsCount()
		{
			if (Selections != null && Selections.Length != 0)
			{
				int nextIdsLength = ((NextIds != null) ? NextIds.Length : 0);
				if (nextIdsLength < Selections.Length)
				{
					this.LogError(string.Format("[Selections] NextIds.Length({0}) < Selections.Length({1})", nextIdsLength, Selections.Length));
					return false;
				}
			}
			return true;
		}

		private static bool ValidateNoCycles()
		{
			HashSet<long> safe = new HashSet<long>();
			bool result = true;
			foreach (long id in Table.Keys)
			{
				if (!safe.Contains(id))
				{
					HashSet<long> path = new HashSet<long>();
					if (!Visit(id, path, safe))
					{
						result = false;
					}
				}
			}
			return result;
		}

		private static bool Visit(long id, HashSet<long> path, HashSet<long> safe)
		{
			if (safe.Contains(id))
			{
				return true;
			}
			if (!path.Add(id))
			{
				if (Table.TryGetValue(id, out var cycleRow))
				{
					cycleRow.LogError(string.Format("[Cycle] NextIds 체인이 {0}로 되돌아옴", id));
				}
				return false;
			}
			if (Table.TryGetValue(id, out var row) && row.NextIds != null)
			{
				for (int i = 0; i < row.NextIds.Length; i++)
				{
					long nextId = row.NextIds[i];
					if (nextId >= 0 && !Visit(nextId, path, safe))
					{
						path.Remove(id);
						return false;
					}
				}
			}
			path.Remove(id);
			safe.Add(id);
			return true;
		}
	}
}
