using System;
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
				return 0L;
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
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public void Parse(string[] columns)
		{
		}

		public string ToCsv()
		{
			return null;
		}

		public static void SaveCsv(string filePath)
		{
		}

		public static string GetColumnsCsv()
		{
			return null;
		}

		public static string GetTypesCsv()
		{
			return null;
		}

		public static void LoadFromPath(string filePath)
		{
		}

		public static void LoadFromText(string text)
		{
		}

		public static void InvokeTableChangeEvent()
		{
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
