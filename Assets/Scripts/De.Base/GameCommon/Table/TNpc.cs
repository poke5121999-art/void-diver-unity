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

		public static NpcTable Table { get; set; } = new NpcTable();

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
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => LocalizationUtility.GetLocalizedText(DescKey);

		[Localized]
		public string SubName => LocalizationUtility.GetLocalizedText(SubNameKey);

		[Localized]
		public string ToastText => LocalizationUtility.GetLocalizedText(ToastTextKey);

		[Localized]
		public string DefaultDialog => LocalizationUtility.GetLocalizedText(DefaultDialogKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TNpc", "Name", Id);

		private string DescKey => string.Format("{0}_{1}_{2}", "TNpc", "Desc", Id);

		private string SubNameKey => string.Format("{0}_{1}_{2}", "TNpc", "SubName", Id);

		private string ToastTextKey => string.Format("{0}_{1}_{2}", "TNpc", "ToastText", Id);

		private string DefaultDialogKey => string.Format("{0}_{1}_{2}", "TNpc", "DefaultDialog", Id);

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
			bool result = true;
			if (Table != null)
			{
				foreach (TNpc row in Table.Values)
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
			return this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DescKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("SubNameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("ToastTextKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("DefaultDialogKey", typeof(TLocalizedText))
				&& this.ValidateRefEventCondition("UnlockConditions");
		}
	}
}
