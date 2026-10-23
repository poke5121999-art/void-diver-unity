using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TTrap : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static TrapTable Table { get; set; }

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

		public ETeamSide TeamSide { get; set; }

		public float ActionDuration { get; set; }

		public bool DestroyAfterAction { get; set; }

		public float CoolTime { get; set; }

		public float HitBoxAtk { get; set; }

		public int[] HitBoxIds { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TTrap", "Name", Id);

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
			TeamSide = TableParse.Enum<ETeamSide>(columns[1]);
			ActionDuration = TableParse.Float(columns[2]);
			DestroyAfterAction = TableParse.Bool(columns[3]);
			CoolTime = TableParse.Float(columns[4]);
			HitBoxAtk = TableParse.Float(columns[5]);
			HitBoxIds = TableParse.Array<int>(columns[6]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, TeamSide, ActionDuration, DestroyAfterAction, CoolTime, HitBoxAtk, HitBoxIds);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,TeamSide,ActionDuration,DestroyAfterAction,CoolTime,HitBoxAtk,HitBoxIds";
		}

		public static string GetTypesCsv()
		{
			return "int,ETeamSide,float,bool,float,float,int[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new TrapTable();
			table.Load(new CsvTableLoader<int, TTrap>(text));
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
