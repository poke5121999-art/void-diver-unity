using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TEntrance : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static EntranceTable Table { get; set; }

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

		public EEntranceType EntranceType { get; set; }

		public bool IsOpened { get; set; }

		public bool HasKeyInteraction { get; set; }

		public EKeyItemType[] KeyItemTypes { get; set; }

		public int[] KeyItemIds { get; set; }

		public int KeySuccessHitBoxId { get; set; }

		public bool KeyFailRetryAble { get; set; }

		public int KeyFailHitboxId { get; set; }

		public float HitBoxAtk { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TEntrance", "Name", Id);

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
			EntranceType = TableParse.Enum<EEntranceType>(columns[2]);
			IsOpened = TableParse.Bool(columns[3]);
			HasKeyInteraction = TableParse.Bool(columns[4]);
			KeyItemTypes = TableParse.Array<EKeyItemType>(columns[5]);
			KeyItemIds = TableParse.Array<int>(columns[6]);
			KeySuccessHitBoxId = TableParse.Int(columns[7]);
			KeyFailRetryAble = TableParse.Bool(columns[8]);
			KeyFailHitboxId = TableParse.Int(columns[9]);
			HitBoxAtk = TableParse.Float(columns[10]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, PrefabName, EntranceType, IsOpened, HasKeyInteraction, KeyItemTypes, KeyItemIds, KeySuccessHitBoxId, KeyFailRetryAble, KeyFailHitboxId, HitBoxAtk);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,PrefabName,EntranceType,IsOpened,HasKeyInteraction,KeyItemTypes,KeyItemIds,KeySuccessHitBoxId,KeyFailRetryAble,KeyFailHitboxId,HitBoxAtk";
		}

		public static string GetTypesCsv()
		{
			return "int,string,EEntranceType,bool,bool,EKeyItemType[],int[],int,bool,int,float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new EntranceTable();
			table.Load(new CsvTableLoader<int, TEntrance>(text));
			Table = table;
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
