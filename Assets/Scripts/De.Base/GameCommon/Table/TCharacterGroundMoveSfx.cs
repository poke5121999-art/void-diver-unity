using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCharacterGroundMoveSfx : ITableData<EGroundType>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static CharacterGroundMoveSfxTable Table { get; set; }

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

		public string RunLeft { get; set; }

		public string RunRight { get; set; }

		public string WalkLeft { get; set; }

		public string WalkRight { get; set; }

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
