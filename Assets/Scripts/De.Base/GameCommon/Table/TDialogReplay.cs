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

		public static DialogReplayTable Table { get; set; }

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
			return false;
		}

		public static long MakeKey(string localizedKey)
		{
			return 0L;
		}

		public static long MakeKey(EQuestType questType, int tableId, int index)
		{
			return 0L;
		}

		public string GetSpeakerName(string playerName, int playerCharacterId)
		{
			return null;
		}

		public bool Validate()
		{
			return false;
		}

		private bool ValidateQuestTableId()
		{
			return false;
		}

		private bool ValidateSpeakerTableId()
		{
			return false;
		}

		private bool ValidateNextIdRefs()
		{
			return false;
		}

		private bool ValidateSelectionsCount()
		{
			return false;
		}

		private static bool ValidateNoCycles()
		{
			return false;
		}

		private static bool Visit(long id, HashSet<long> path, HashSet<long> safe)
		{
			return false;
		}
	}
}
