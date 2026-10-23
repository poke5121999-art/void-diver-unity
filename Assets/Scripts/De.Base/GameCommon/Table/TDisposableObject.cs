using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TDisposableObject : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static DisposableObjectTable Table { get; set; } = new DisposableObjectTable();

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

		public string PrefabName { get; set; }

		public int DropRewardGroupId { get; set; }

		public float HoldingTime { get; set; }

		public float AggroSpreadRange { get; set; }

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
			PrefabName = TableParse.Str(columns[1]);
			DropRewardGroupId = TableParse.Int(columns[2]);
			HoldingTime = TableParse.Float(columns[3]);
			AggroSpreadRange = TableParse.Float(columns[4]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, PrefabName, DropRewardGroupId, HoldingTime, AggroSpreadRange);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,PrefabName,DropRewardGroupId,HoldingTime,AggroSpreadRange";
		}

		public static string GetTypesCsv()
		{
			return "int,string,int,float,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new DisposableObjectTable();
			table.Load(new CsvTableLoader<int, TDisposableObject>(text));
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
				foreach (TDisposableObject row in Table.Values)
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
			return this.ValidateRefGroupKey("DropRewardGroupId", typeof(TDropReward));
		}
	}
}
