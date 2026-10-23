using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TMonsterSpawn : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static MonsterSpawnTable Table { get; set; } = new MonsterSpawnTable();

		public static GroupMonsterSpawnTable GroupTable { get; set; }

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

		public int GroupId { get; set; }

		public ELightCondition LightCondition { get; set; }

		public int Probability { get; set; }

		public int MonsterId { get; set; }

		public int[] BuffIds { get; set; }

		public int DropRewardGroupId { get; set; }

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
			GroupId = TableParse.Int(columns[1]);
			LightCondition = TableParse.Enum<ELightCondition>(columns[2]);
			Probability = TableParse.Int(columns[3]);
			MonsterId = TableParse.Int(columns[4]);
			BuffIds = TableParse.Array<int>(columns[5]);
			DropRewardGroupId = TableParse.Int(columns[6]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, GroupId, LightCondition, Probability, MonsterId, BuffIds, DropRewardGroupId);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,GroupId,LightCondition,Probability,MonsterId,BuffIds,DropRewardGroupId";
		}

		public static string GetTypesCsv()
		{
			return "int,int,ELightCondition,int,int,int[],int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new MonsterSpawnTable();
			table.Load(new CsvTableLoader<int, TMonsterSpawn>(text));
			Table = table;
			GroupTable = new GroupMonsterSpawnTable();
			GroupTable.Init(table.Values.GroupBy(row => row.GroupId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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
				foreach (TMonsterSpawn row in Table.Values)
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
			return this.ValidateSpawnableMonster("MonsterId")
				&& this.ValidateRefKey("BuffIds", typeof(TBuff))
				&& this.ValidateRefGroupKey("DropRewardGroupId", typeof(TDropReward));
		}
	}
}
