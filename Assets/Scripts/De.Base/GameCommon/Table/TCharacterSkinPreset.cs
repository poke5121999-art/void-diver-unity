using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TCharacterSkinPreset : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static CharacterSkinPresetTable Table { get; set; }

		public static GroupCharacterSkinPresetTable GroupTable { get; set; }

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

		public int CharacterSkinId { get; set; }

		public int SkinBaseId { get; set; }

		public int EyeSkinId { get; set; }

		public int HairSkinId { get; set; }

		public int CostumeSkinId { get; set; }

		public int AccSkinId { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		public int CharacterId => 0;

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

		public static int GetDefaultSkinPresetId(int characterId)
		{
			return 0;
		}

		public bool Validate()
		{
			return false;
		}
	}
}
