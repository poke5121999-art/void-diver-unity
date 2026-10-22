using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TArtifactDealCard : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ArtifactDealCardTable Table { get; set; }

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

		public int Weight { get; set; }

		public EArtifactDealCardRarityType RarityType { get; set; }

		public int SuccessPercent { get; set; }

		public ArtifactDealCardEffect[] SuccessEffects { get; set; }

		public ArtifactDealCardEffect[] FailEffects { get; set; }

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

		private bool ValidateEffectPrefixRefs(ArtifactDealCardEffect[] effects, string fieldName)
		{
			return false;
		}
	}
}
