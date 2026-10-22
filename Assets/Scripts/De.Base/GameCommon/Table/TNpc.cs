using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TNpc : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static NpcTable Table { get; set; }

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

		public ENpcType Type { get; set; }

		public int BitFlagIndex { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public string DefaultImageName { get; set; }

		public bool ShowNewContentPopup { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => null;

		[Localized]
		public string Desc => null;

		[Localized]
		public string SubName => null;

		[Localized]
		public string ToastText => null;

		[Localized]
		public string DefaultDialog => null;

		private string NameKey => null;

		private string DescKey => null;

		private string SubNameKey => null;

		private string ToastTextKey => null;

		private string DefaultDialogKey => null;

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
			Type = TableParse.Enum<ENpcType>(columns[1]);
			BitFlagIndex = TableParse.Int(columns[2]);
			UnlockConditions = TableParse.Json<EventCondition[]>(columns[3]);
			DefaultImageName = TableParse.Str(columns[4]);
			ShowNewContentPopup = TableParse.Bool(columns[5]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Type, BitFlagIndex, UnlockConditions, DefaultImageName, ShowNewContentPopup);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Type,BitFlagIndex,UnlockConditions,DefaultImageName,ShowNewContentPopup";
		}

		public static string GetTypesCsv()
		{
			return "int,ENpcType,int,EventCondition[],string,bool";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new NpcTable();
			table.Load(new CsvTableLoader<int, TNpc>(text));
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
