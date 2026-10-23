using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TBag : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static BagTable Table { get; set; }

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

		public EBagType Type { get; set; }

		public int SlotCount { get; set; }

		public int Worth { get; set; }

		public int SellPrice { get; set; }

		public EGoodsGradeType Grade { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		[Localized]
		public string Desc => GetDesc();

		private string NameKey => string.Format("{0}_{1}_{2}", "TBag", "Name", Id);

		private string DescFormatKey => string.Format("{0}_{1}_{2}", "TBag", "Desc", Id);

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
			Type = TableParse.Enum<EBagType>(columns[2]);
			SlotCount = TableParse.Int(columns[3]);
			Worth = TableParse.Int(columns[4]);
			SellPrice = TableParse.Int(columns[5]);
			Grade = TableParse.Enum<EGoodsGradeType>(columns[6]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, BitFlagIndex, Type, SlotCount, Worth, SellPrice, Grade);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,BitFlagIndex,Type,SlotCount,Worth,SellPrice,Grade";
		}

		public static string GetTypesCsv()
		{
			return "int,int,EBagType,int,int,int,EGoodsGradeType";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new BagTable();
			table.Load(new CsvTableLoader<int, TBag>(text));
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

		private string GetDesc()
		{
			if (TLocalizedText.Table.TryGetValue(DescFormatKey, out var data))
			{
				return data.FormatWithExpression();
			}
			return DescFormatKey;
		}

		public bool Validate()
		{
			return false;
		}
	}
}
