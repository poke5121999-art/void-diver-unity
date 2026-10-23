using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TSkillExecutor : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static SkillExecutorTable Table { get; set; }

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

		public string Memo { get; set; }

		public string PrefabName { get; set; }

		public int UseLimit { get; set; }

		public EStressState[] InteractStressConditions { get; set; }

		public string MiniMapMarker { get; set; }

		public ActionEvent[] ActionEvents { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TSkillExecutor", "Name", Id);

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
			Memo = TableParse.Str(columns[1]);
			PrefabName = TableParse.Str(columns[2]);
			UseLimit = TableParse.Int(columns[3]);
			InteractStressConditions = TableParse.Array<EStressState>(columns[4]);
			MiniMapMarker = TableParse.Str(columns[5]);
			ActionEvents = TableParse.Json<ActionEvent[]>(columns[6]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Memo, PrefabName, UseLimit, InteractStressConditions, MiniMapMarker, ActionEvents);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Memo,PrefabName,UseLimit,InteractStressConditions,MiniMapMarker,ActionEvents";
		}

		public static string GetTypesCsv()
		{
			return "int,string,string,int,EStressState[],string,ActionEvent[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new SkillExecutorTable();
			table.Load(new CsvTableLoader<int, TSkillExecutor>(text));
			Table = table;
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

		public bool Validate()
		{
			return false;
		}
	}
}
