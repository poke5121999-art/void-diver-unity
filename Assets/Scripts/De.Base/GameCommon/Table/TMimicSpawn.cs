using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TMimicSpawn : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static MimicSpawnTable Table { get; set; }

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

		public int MimicId { get; set; }

		public int RewardBoxId { get; set; }

		public float MimicPercent { get; set; }

		public float RewardBoxPercent { get; set; }

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
			MimicId = TableParse.Int(columns[1]);
			RewardBoxId = TableParse.Int(columns[2]);
			MimicPercent = TableParse.Float(columns[3]);
			RewardBoxPercent = TableParse.Float(columns[4]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, MimicId, RewardBoxId, MimicPercent, RewardBoxPercent);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,MimicId,RewardBoxId,MimicPercent,RewardBoxPercent";
		}

		public static string GetTypesCsv()
		{
			return "int,int,int,float,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new MimicSpawnTable();
			table.Load(new CsvTableLoader<int, TMimicSpawn>(text));
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
