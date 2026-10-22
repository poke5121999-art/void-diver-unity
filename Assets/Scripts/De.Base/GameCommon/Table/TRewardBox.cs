using System;
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
				return 0;
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
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public void Parse(string[] columns)
		{
		}

		public string ToCsv()
		{
			return null;
		}

		public static void SaveCsv(string filePath)
		{
		}

		public static string GetColumnsCsv()
		{
			return null;
		}

		public static string GetTypesCsv()
		{
			return null;
		}

		public static void LoadFromPath(string filePath)
		{
		}

		public static void LoadFromText(string text)
		{
		}

		public static void InvokeTableChangeEvent()
		{
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
