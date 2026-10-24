using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TLoadingText : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static LoadingTextTable Table { get; set; } = new LoadingTextTable();

		public static GroupLoadingTextTable GroupTable { get; set; } = new GroupLoadingTextTable();

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

		public ELoadingPopupType LoadingPopupCondition { get; set; }

		public int TargetCharacterId { get; set; }

		public int NeedUserLevel { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string TipText => LocalizationUtility.GetLocalizedText(TipTextKey);

		private string TipTextKey => string.Format("{0}_{1}_{2}", "TLoadingText", "TipText", Id);

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
			LoadingPopupCondition = TableParse.Enum<ELoadingPopupType>(columns[1]);
			TargetCharacterId = TableParse.Int(columns[2]);
			NeedUserLevel = TableParse.Int(columns[3]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, LoadingPopupCondition, TargetCharacterId, NeedUserLevel);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,LoadingPopupCondition,TargetCharacterId,NeedUserLevel";
		}

		public static string GetTypesCsv()
		{
			return "int,ELoadingPopupType,int,int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new LoadingTextTable();
			table.Load(new CsvTableLoader<int, TLoadingText>(text));
			Table = table;
			GroupTable = new GroupLoadingTextTable();
			GroupTable.Init(table.Values.GroupBy(row => row.LoadingPopupCondition).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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
				foreach (TLoadingText row in Table.Values)
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
			return this.ValidateRefKey("TipTextKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("TargetCharacterId", typeof(TCharacter));
		}
	}
}
