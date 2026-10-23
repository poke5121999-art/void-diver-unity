using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TMimic : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static MimicTable Table { get; set; } = new MimicTable();

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

		public float HoldingTime { get; set; }

		public float QteCancelZoneEndRatio { get; set; }

		public float QteSuccessZoneEndRatio { get; set; }

		public string QteCancelZoneSfx { get; set; }

		public string QteSuccessZoneSfx { get; set; }

		public string QteFailZoneSfx { get; set; }

		public float HitBoxAtk { get; set; }

		public int[] HitBoxIds { get; set; }

		public int MonsterId { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TMimic", "Name", Id);

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
			HoldingTime = TableParse.Float(columns[2]);
			QteCancelZoneEndRatio = TableParse.Float(columns[3]);
			QteSuccessZoneEndRatio = TableParse.Float(columns[4]);
			QteCancelZoneSfx = TableParse.Str(columns[5]);
			QteSuccessZoneSfx = TableParse.Str(columns[6]);
			QteFailZoneSfx = TableParse.Str(columns[7]);
			HitBoxAtk = TableParse.Float(columns[8]);
			HitBoxIds = TableParse.Array<int>(columns[9]);
			MonsterId = TableParse.Int(columns[10]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, PrefabName, HoldingTime, QteCancelZoneEndRatio, QteSuccessZoneEndRatio, QteCancelZoneSfx, QteSuccessZoneSfx, QteFailZoneSfx, HitBoxAtk, HitBoxIds, MonsterId);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,PrefabName,HoldingTime,QteCancelZoneEndRatio,QteSuccessZoneEndRatio,QteCancelZoneSfx,QteSuccessZoneSfx,QteFailZoneSfx,HitBoxAtk,HitBoxIds,MonsterId";
		}

		public static string GetTypesCsv()
		{
			return "int,string,float,float,float,string,string,string,float,int[],int";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new MimicTable();
			table.Load(new CsvTableLoader<int, TMimic>(text));
			Table = table;
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
				foreach (TMimic row in Table.Values)
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
			return this.Check(HoldingTime > 0f, string.Format("{0}: {1} must be > 0", "HoldingTime", HoldingTime))
				&& this.Check(QteCancelZoneEndRatio < QteSuccessZoneEndRatio, string.Format("{0}: {1} must be < {2}: {3}", "QteCancelZoneEndRatio", QteCancelZoneEndRatio, "QteSuccessZoneEndRatio", QteSuccessZoneEndRatio))
				&& this.ValidateRefKey("NameKey", typeof(TLocalizedText))
				&& this.ValidateRefKey("MonsterId", typeof(TMonster))
				&& this.ValidateRefKey("HitBoxIds", typeof(THitBox))
				&& this.ValidateRefSfx("QteCancelZoneSfx")
				&& this.ValidateRefSfx("QteSuccessZoneSfx")
				&& this.ValidateRefSfx("QteFailZoneSfx");
		}
	}
}
