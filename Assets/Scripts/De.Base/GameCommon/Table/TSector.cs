using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TSector : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static SectorTable Table { get; set; } = new SectorTable();

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

		public string Memo { get; set; }

		public ESectorAreaType AreaType { get; set; }

		public EThemeType ThemeType { get; set; }

		public string Bgm { get; set; }

		public string CombatBgm { get; set; }

		public float StressIntervalPercent { get; set; }

		public ZonePointData[] ZonePointDatas { get; set; }

		public MonsterSpawnGroupData[] MonsterSpawnGroupDatas { get; set; }

		public DropRewardSpawnGroupData[] DropRewardSpawnGroupDatas { get; set; }

		public RewardBoxSpawnGroupData[] RewardBoxSpawnGroupDatas { get; set; }

		public TrapSpawnGroupData[] TrapSpawnGroupDatas { get; set; }

		public ExitSpawnGroupData[] ExitSpawnGroupDatas { get; set; }

		public SkillExecutorSpawnGroupData[] SkillExecutorSpawnGroupDatas { get; set; }

		public WaveExecutorSpawnGroupData[] WaveExecutorSpawnGroupDatas { get; set; }

		public PhoneBoothSpawnData[] PhoneBoothSpawnDatas { get; set; }

		public NpcSpawnData[] NpcSpawnDatas { get; set; }

		public GuestSpawnData[] GuestSpawnDatas { get; set; }

		public DisposableObjectSpawnGroupData[] DisposableObjectSpawnGroupDatas { get; set; }

		public BreakablePropSpawnGroupData[] BreakablePropSpawnGroupDatas { get; set; }

		public EntranceSpawnGroupData[] EntranceSpawnGroupDatas { get; set; }

		public CollisionTriggerSpawnData[] CollisionTriggerSpawnDatas { get; set; }

		public TextMarkerSpawnData[] TextMarkerSpawnDatas { get; set; }

		public PortalSpawnGroupData[] PortalSpawnGroupDatas { get; set; }

		public PortalExitData[] PortalExitDatas { get; set; }

		public SpecialFieldSpawnGroupData[] SpecialFieldSpawnGroupDatas { get; set; }

		public LockedAreaObjectSpawnData[] LockedAreaObjectSpawnDatas { get; set; }

		public AreaDecorationObjectSpawnData[] AreaDecorationObjectSpawnDatas { get; set; }

		public MimicSpawnGroupData[] MimicSpawnGroupDatas { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

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
			Memo = TableParse.Str(columns[1]);
			AreaType = TableParse.Enum<ESectorAreaType>(columns[2]);
			ThemeType = TableParse.Enum<EThemeType>(columns[3]);
			Bgm = TableParse.Str(columns[4]);
			CombatBgm = TableParse.Str(columns[5]);
			StressIntervalPercent = TableParse.Float(columns[6]);
			ZonePointDatas = TableParse.Json<ZonePointData[]>(columns[7]);
			MonsterSpawnGroupDatas = TableParse.Json<MonsterSpawnGroupData[]>(columns[8]);
			DropRewardSpawnGroupDatas = TableParse.Json<DropRewardSpawnGroupData[]>(columns[9]);
			RewardBoxSpawnGroupDatas = TableParse.Json<RewardBoxSpawnGroupData[]>(columns[10]);
			TrapSpawnGroupDatas = TableParse.Json<TrapSpawnGroupData[]>(columns[11]);
			ExitSpawnGroupDatas = TableParse.Json<ExitSpawnGroupData[]>(columns[12]);
			SkillExecutorSpawnGroupDatas = TableParse.Json<SkillExecutorSpawnGroupData[]>(columns[13]);
			WaveExecutorSpawnGroupDatas = TableParse.Json<WaveExecutorSpawnGroupData[]>(columns[14]);
			PhoneBoothSpawnDatas = TableParse.Json<PhoneBoothSpawnData[]>(columns[15]);
			NpcSpawnDatas = TableParse.Json<NpcSpawnData[]>(columns[16]);
			GuestSpawnDatas = TableParse.Json<GuestSpawnData[]>(columns[17]);
			DisposableObjectSpawnGroupDatas = TableParse.Json<DisposableObjectSpawnGroupData[]>(columns[18]);
			BreakablePropSpawnGroupDatas = TableParse.Json<BreakablePropSpawnGroupData[]>(columns[19]);
			EntranceSpawnGroupDatas = TableParse.Json<EntranceSpawnGroupData[]>(columns[20]);
			CollisionTriggerSpawnDatas = TableParse.Json<CollisionTriggerSpawnData[]>(columns[21]);
			TextMarkerSpawnDatas = TableParse.Json<TextMarkerSpawnData[]>(columns[22]);
			PortalSpawnGroupDatas = TableParse.Json<PortalSpawnGroupData[]>(columns[23]);
			PortalExitDatas = TableParse.Json<PortalExitData[]>(columns[24]);
			SpecialFieldSpawnGroupDatas = TableParse.Json<SpecialFieldSpawnGroupData[]>(columns[25]);
			LockedAreaObjectSpawnDatas = TableParse.Json<LockedAreaObjectSpawnData[]>(columns[26]);
			AreaDecorationObjectSpawnDatas = TableParse.Json<AreaDecorationObjectSpawnData[]>(columns[27]);
			MimicSpawnGroupDatas = TableParse.Json<MimicSpawnGroupData[]>(columns[28]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Memo, AreaType, ThemeType, Bgm, CombatBgm, StressIntervalPercent, ZonePointDatas, MonsterSpawnGroupDatas, DropRewardSpawnGroupDatas, RewardBoxSpawnGroupDatas, TrapSpawnGroupDatas, ExitSpawnGroupDatas, SkillExecutorSpawnGroupDatas, WaveExecutorSpawnGroupDatas, PhoneBoothSpawnDatas, NpcSpawnDatas, GuestSpawnDatas, DisposableObjectSpawnGroupDatas, BreakablePropSpawnGroupDatas, EntranceSpawnGroupDatas, CollisionTriggerSpawnDatas, TextMarkerSpawnDatas, PortalSpawnGroupDatas, PortalExitDatas, SpecialFieldSpawnGroupDatas, LockedAreaObjectSpawnDatas, AreaDecorationObjectSpawnDatas, MimicSpawnGroupDatas);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Memo,AreaType,ThemeType,Bgm,CombatBgm,StressIntervalPercent,ZonePointDatas,MonsterSpawnGroupDatas,DropRewardSpawnGroupDatas,RewardBoxSpawnGroupDatas,TrapSpawnGroupDatas,ExitSpawnGroupDatas,SkillExecutorSpawnGroupDatas,WaveExecutorSpawnGroupDatas,PhoneBoothSpawnDatas,NpcSpawnDatas,GuestSpawnDatas,DisposableObjectSpawnGroupDatas,BreakablePropSpawnGroupDatas,EntranceSpawnGroupDatas,CollisionTriggerSpawnDatas,TextMarkerSpawnDatas,PortalSpawnGroupDatas,PortalExitDatas,SpecialFieldSpawnGroupDatas,LockedAreaObjectSpawnDatas,AreaDecorationObjectSpawnDatas,MimicSpawnGroupDatas";
		}

		public static string GetTypesCsv()
		{
			return "int,string,ESectorAreaType,EThemeType,string,string,float,ZonePointData[],MonsterSpawnGroupData[],DropRewardSpawnGroupData[],RewardBoxSpawnGroupData[],TrapSpawnGroupData[],ExitSpawnGroupData[],SkillExecutorSpawnGroupData[],WaveExecutorSpawnGroupData[],PhoneBoothSpawnData[],NpcSpawnData[],GuestSpawnData[],DisposableObjectSpawnGroupData[],BreakablePropSpawnGroupData[],EntranceSpawnGroupData[],CollisionTriggerSpawnData[],TextMarkerSpawnData[],PortalSpawnGroupData[],PortalExitData[],SpecialFieldSpawnGroupData[],LockedAreaObjectSpawnData[],AreaDecorationObjectSpawnData[],MimicSpawnGroupData[]";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new SectorTable();
			table.Load(new CsvTableLoader<int, TSector>(text));
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
				foreach (TSector row in Table.Values)
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
			bool isValid = true;
			if (ZonePointDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(ZonePointDatas, delegate(ZonePointData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (MonsterSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(MonsterSpawnGroupDatas, delegate(MonsterSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (DropRewardSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(DropRewardSpawnGroupDatas, delegate(DropRewardSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (RewardBoxSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(RewardBoxSpawnGroupDatas, delegate(RewardBoxSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (TrapSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(TrapSpawnGroupDatas, delegate(TrapSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (MimicSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(MimicSpawnGroupDatas, delegate(MimicSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (ExitSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(ExitSpawnGroupDatas, delegate(ExitSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (SkillExecutorSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(SkillExecutorSpawnGroupDatas, delegate(SkillExecutorSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (WaveExecutorSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(WaveExecutorSpawnGroupDatas, delegate(WaveExecutorSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (PhoneBoothSpawnDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(PhoneBoothSpawnDatas, delegate(PhoneBoothSpawnData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (NpcSpawnDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(NpcSpawnDatas, delegate(NpcSpawnData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (DisposableObjectSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(DisposableObjectSpawnGroupDatas, delegate(DisposableObjectSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (BreakablePropSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(BreakablePropSpawnGroupDatas, delegate(BreakablePropSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (EntranceSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(EntranceSpawnGroupDatas, delegate(EntranceSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (CollisionTriggerSpawnDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(CollisionTriggerSpawnDatas, delegate(CollisionTriggerSpawnData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (TextMarkerSpawnDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(TextMarkerSpawnDatas, delegate(TextMarkerSpawnData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (PortalSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(PortalSpawnGroupDatas, delegate(PortalSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (PortalExitDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(PortalExitDatas, delegate(PortalExitData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			if (SpecialFieldSpawnGroupDatas != null)
			{
				SomeNet.Collections.Extension.ForEach(SpecialFieldSpawnGroupDatas, delegate(SpecialFieldSpawnGroupData data)
				{
					string errorMessage = null;
					if (!data.Validate(out errorMessage))
					{
						this.LogError(errorMessage);
						isValid = false;
					}
				});
			}
			return isValid;
		}
	}
}
