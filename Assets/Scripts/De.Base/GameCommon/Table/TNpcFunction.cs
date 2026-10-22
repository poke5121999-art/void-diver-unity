using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TNpcFunction : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static NpcFunctionTable Table { get; set; }

		public static GroupNpcFunctionTable GroupTable { get; set; }

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

		public int NpcId { get; set; }

		public int BitFlagIndex { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public ENpcFunctionType Type { get; set; }

		public int Value1 { get; set; }

		public int Value2 { get; set; }

		public EGoodsType GoodsType { get; set; }

		public float Percent { get; set; }

		public bool GuestInteractable { get; set; }

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
			NpcId = TableParse.Int(columns[1]);
			BitFlagIndex = TableParse.Int(columns[2]);
			UnlockConditions = TableParse.Json<EventCondition[]>(columns[3]);
			Type = TableParse.Enum<ENpcFunctionType>(columns[4]);
			Value1 = TableParse.Int(columns[5]);
			Value2 = TableParse.Int(columns[6]);
			GoodsType = TableParse.Enum<EGoodsType>(columns[7]);
			Percent = TableParse.Float(columns[8]);
			GuestInteractable = TableParse.Bool(columns[9]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, NpcId, BitFlagIndex, UnlockConditions, Type, Value1, Value2, GoodsType, Percent, GuestInteractable);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,NpcId,BitFlagIndex,UnlockConditions,Type,Value1,Value2,GoodsType,Percent,GuestInteractable";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,EventCondition[],ENpcFunctionType,int,int,EGoodsType,float,bool";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new NpcFunctionTable();
			table.Load(new CsvTableLoader<int, TNpcFunction>(text));
			Table = table;
			GroupTable = new GroupNpcFunctionTable();
			GroupTable.Init(table.Values.GroupBy(row => row.NpcId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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

		public string ToLocalString()
		{
			return null;
		}

		public bool Validate()
		{
			return false;
		}

		private bool ValidateValue1Ref()
		{
			return false;
		}
	}
}
