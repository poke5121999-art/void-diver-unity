using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TExit : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ExitTable Table { get; set; } = new ExitTable();

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

		public EExitType Type { get; set; }

		public int WaveId { get; set; }

		public int SpecialFieldId { get; set; }

		public string ActivatingStateHoldingSfx { get; set; }

		public string HoldingSfx { get; set; }

		public string EscapeSfx { get; set; }

		public bool DisplayOnMinimapAtStart { get; set; }

		public bool UseExitCostTable { get; set; }

		public GoodsData CustomUnlockGoodsData { get; set; }

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
			Type = TableParse.Enum<EExitType>(columns[1]);
			WaveId = TableParse.Int(columns[2]);
			SpecialFieldId = TableParse.Int(columns[3]);
			ActivatingStateHoldingSfx = TableParse.Str(columns[4]);
			HoldingSfx = TableParse.Str(columns[5]);
			EscapeSfx = TableParse.Str(columns[6]);
			DisplayOnMinimapAtStart = TableParse.Bool(columns[7]);
			UseExitCostTable = TableParse.Bool(columns[8]);
			CustomUnlockGoodsData = TableParse.Json<GoodsData>(columns[9]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Type, WaveId, SpecialFieldId, ActivatingStateHoldingSfx, HoldingSfx, EscapeSfx, DisplayOnMinimapAtStart, UseExitCostTable, CustomUnlockGoodsData);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Type,WaveId,SpecialFieldId,ActivatingStateHoldingSfx,HoldingSfx,EscapeSfx,DisplayOnMinimapAtStart,UseExitCostTable,CustomUnlockGoodsData";
		}

		public static string GetTypesCsv()
		{
			return "int,EExitType,int,int,string,string,string,bool,bool,GoodsData";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ExitTable();
			table.Load(new CsvTableLoader<int, TExit>(text));
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
				foreach (TExit row in Table.Values)
				{
					if (!row.Validate())
					{
						result = false;
					}
				}
			}
			return result;
		}

		public GoodsData GetUnlockGoodsData(int level)
		{
			if (!UseExitCostTable)
			{
				return CustomUnlockGoodsData;
			}
			if (!TExitCost.Table.TryGetValue(level, out TExitCost cost))
			{
				return default;
			}
			return cost.UnlockGoodsData;
		}

		public bool Validate()
		{
			return this.ValidateRefKey("WaveId", typeof(TWave))
				&& this.ValidateRefKey("SpecialFieldId", typeof(TSpecialField))
				&& this.ValidateRefGoodsData("CustomUnlockGoodsData")
				&& this.ValidateRefSfx("ActivatingStateHoldingSfx")
				&& this.ValidateRefSfx("HoldingSfx")
				&& this.ValidateRefSfx("EscapeSfx");
		}
	}
}
