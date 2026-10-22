using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TRewardBox : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static RewardBoxTable Table { get; set; }

		public static GroupRewardBoxTable GroupTable { get; set; }

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

		public int GroupId { get; set; }

		public int Weight { get; set; }

		public int DropRewardGroupId { get; set; }

		public float HitboxTriggerPercent { get; set; }

		public int[] HitBoxIds { get; set; }

		public float HitBoxAtk { get; set; }

		public bool HasKeyInteraction { get; set; }

		public EKeyItemType[] KeyItemTypes { get; set; }

		public int[] KeyItemIds { get; set; }

		public int KeySuccessHitBoxId { get; set; }

		public bool KeyFailRetryAble { get; set; }

		public int KeyFailHitboxId { get; set; }

		public EStressState[] InteractStressConditions { get; set; }

		public string StressFailPrefabName { get; set; }

		public EOutlineType OutlineType { get; set; }

		public float HoldingTime { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => null;

		private string NameKey => null;

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
			GroupId = TableParse.Int(columns[2]);
			Weight = TableParse.Int(columns[3]);
			DropRewardGroupId = TableParse.Int(columns[4]);
			HitboxTriggerPercent = TableParse.Float(columns[5]);
			HitBoxIds = TableParse.Array<int>(columns[6]);
			HitBoxAtk = TableParse.Float(columns[7]);
			HasKeyInteraction = TableParse.Bool(columns[8]);
			KeyItemTypes = TableParse.Array<EKeyItemType>(columns[9]);
			KeyItemIds = TableParse.Array<int>(columns[10]);
			KeySuccessHitBoxId = TableParse.Int(columns[11]);
			KeyFailRetryAble = TableParse.Bool(columns[12]);
			KeyFailHitboxId = TableParse.Int(columns[13]);
			InteractStressConditions = TableParse.Array<EStressState>(columns[14]);
			StressFailPrefabName = TableParse.Str(columns[15]);
			OutlineType = TableParse.Enum<EOutlineType>(columns[16]);
			HoldingTime = TableParse.Float(columns[17]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, PrefabName, GroupId, Weight, DropRewardGroupId, HitboxTriggerPercent, HitBoxIds, HitBoxAtk, HasKeyInteraction, KeyItemTypes, KeyItemIds, KeySuccessHitBoxId, KeyFailRetryAble, KeyFailHitboxId, InteractStressConditions, StressFailPrefabName, OutlineType, HoldingTime);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,PrefabName,GroupId,Weight,DropRewardGroupId,HitboxTriggerPercent,HitBoxIds,HitBoxAtk,HasKeyInteraction,KeyItemTypes,KeyItemIds,KeySuccessHitBoxId,KeyFailRetryAble,KeyFailHitboxId,InteractStressConditions,StressFailPrefabName,OutlineType,HoldingTime";
		}

		public static string GetTypesCsv()
		{
			return "int,string,int,int,int,float,int[],float,bool,EKeyItemType[],int[],int,bool,int,EStressState[],string,EOutlineType,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new RewardBoxTable();
			table.Load(new CsvTableLoader<int, TRewardBox>(text));
			Table = table;
			GroupTable = new GroupRewardBoxTable();
			GroupTable.Init(table.Values.GroupBy(row => row.GroupId).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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
