using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TBreakableProp : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static BreakablePropTable Table { get; set; }

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

		public int HitsToBreak { get; set; }

		public int DropRewardGroupId { get; set; }

		public int Atk { get; set; }

		public ActionEvent[] ActionEventsOnDestroy { get; set; }

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
			Memo = TableParse.Str(columns[1]);
			PrefabName = TableParse.Str(columns[2]);
			HitsToBreak = TableParse.Int(columns[3]);
			DropRewardGroupId = TableParse.Int(columns[4]);
			Atk = TableParse.Int(columns[5]);
			ActionEventsOnDestroy = TableParse.Json<ActionEvent[]>(columns[6]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Memo, PrefabName, HitsToBreak, DropRewardGroupId, Atk, ActionEventsOnDestroy);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Memo,PrefabName,HitsToBreak,DropRewardGroupId,Atk,ActionEventsOnDestroy";
		}

		public static string GetTypesCsv()
		{
			return "int,string,string,int,int,int,ActionEvent[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new BreakablePropTable();
			table.Load(new CsvTableLoader<int, TBreakableProp>(text));
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
