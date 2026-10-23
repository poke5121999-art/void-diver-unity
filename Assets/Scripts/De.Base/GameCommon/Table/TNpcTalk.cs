using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TNpcTalk : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static NpcTalkTable Table { get; set; } = new NpcTalkTable();

		public int Pk
		{
			get
			{
				return Id;
			}
			set
			{
			}
		}

		public int Id { get; set; }

		public int BitFlagIndex { get; set; }

		public int NpcId { get; set; }

		public EventCondition[] AddConditions { get; set; }

		public EventCondition[] RemoveConditions { get; set; }

		public string FunctionName { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

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
			Id = TableParse.Int(columns[0]);
			BitFlagIndex = TableParse.Int(columns[1]);
			NpcId = TableParse.Int(columns[2]);
			AddConditions = TableParse.Json<EventCondition[]>(columns[3]);
			RemoveConditions = TableParse.Json<EventCondition[]>(columns[4]);
			FunctionName = TableParse.Str(columns[5]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, BitFlagIndex, NpcId, AddConditions, RemoveConditions, FunctionName);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,BitFlagIndex,NpcId,AddConditions,RemoveConditions,FunctionName";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,EventCondition[],EventCondition[],string";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new NpcTalkTable();
			table.Load(new CsvTableLoader<int, TNpcTalk>(text));
			Table = table;
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
				foreach (TNpcTalk row in Table.Values)
				{
					if (!row.Validate())
					{
						result = false;
					}
				}
			}
			return result;
		}

		public bool Validate()
		{
			return this.ValidateRefKey("NpcId", typeof(TNpc))
				&& this.ValidateRefEventCondition("AddConditions")
				&& this.ValidateRefEventCondition("RemoveConditions");
		}
	}
}
