using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TNpcFunction : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static NpcFunctionTable Table { get; set; }

		public static GroupNpcFunctionTable GroupTable { get; set; }

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

		public int NpcId { get; set; }

		public int BitFlagIndex { get; set; }

		public EventCondition[] UnlockConditions { get; set; }

		public ENpcFunctionType Type { get; set; }

		public int Value1 { get; set; }

		public int Value2 { get; set; }

		public EGoodsType GoodsType { get; set; }

		public float Percent { get; set; }

		public bool GuestInteractable { get; set; }

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

		public string ToLocalString()
		{
			return null;
		}

		public bool Validate()
		{
			return false;
		}

		private bool ValidateValue1Ref()
		{
			return false;
		}
	}
}
