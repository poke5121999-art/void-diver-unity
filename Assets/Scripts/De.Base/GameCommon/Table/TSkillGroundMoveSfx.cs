using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TSkillGroundMoveSfx : ITableData<EGroundType>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static SkillGroundMoveSfxTable Table { get; set; }

		public EGroundType Pk
		{
			get
			{
				return EGroundType.None;
			}
			set
			{
			}
		}

		public EGroundType GroundType { get; set; }

		public string[] SfxNames { get; set; }

		public Action<ITableData<EGroundType>> OnDataLoaded { get; set; }

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
