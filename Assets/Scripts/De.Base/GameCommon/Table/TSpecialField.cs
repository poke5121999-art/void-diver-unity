using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TSpecialField : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static SpecialFieldTable Table { get; set; }

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

		public int CharacterBuffId { get; set; }

		public int MonsterBuffId { get; set; }

		public int CharacterStackBuffId { get; set; }

		public float CharacterStackBuffInterval { get; set; }

		public bool PreventCharacterStackBuffExpire { get; set; }

		public int MonsterStackBuffId { get; set; }

		public float MonsterStackBuffInterval { get; set; }

		public bool PreventMonsterStackBuffExpire { get; set; }

		public float EffectDelay { get; set; }

		public float Duration { get; set; }

		public ESpecialFieldEvent DestroyTrigger { get; set; }

		public bool DestroyImmediatelyOnTrigger { get; set; }

		public float Atk { get; set; }

		public int[] HitBoxIdsOnDestroy { get; set; }

		public float FixedYPos { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

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
