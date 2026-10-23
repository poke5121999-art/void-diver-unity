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

		public static SpecialFieldTable Table { get; set; } = new SpecialFieldTable();

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
			CharacterBuffId = TableParse.Int(columns[2]);
			MonsterBuffId = TableParse.Int(columns[3]);
			CharacterStackBuffId = TableParse.Int(columns[4]);
			CharacterStackBuffInterval = TableParse.Float(columns[5]);
			PreventCharacterStackBuffExpire = TableParse.Bool(columns[6]);
			MonsterStackBuffId = TableParse.Int(columns[7]);
			MonsterStackBuffInterval = TableParse.Float(columns[8]);
			PreventMonsterStackBuffExpire = TableParse.Bool(columns[9]);
			EffectDelay = TableParse.Float(columns[10]);
			Duration = TableParse.Float(columns[11]);
			DestroyTrigger = TableParse.Enum<ESpecialFieldEvent>(columns[12]);
			DestroyImmediatelyOnTrigger = TableParse.Bool(columns[13]);
			Atk = TableParse.Float(columns[14]);
			HitBoxIdsOnDestroy = TableParse.Array<int>(columns[15]);
			FixedYPos = TableParse.Float(columns[16]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, PrefabName, CharacterBuffId, MonsterBuffId, CharacterStackBuffId, CharacterStackBuffInterval, PreventCharacterStackBuffExpire, MonsterStackBuffId, MonsterStackBuffInterval, PreventMonsterStackBuffExpire, EffectDelay, Duration, DestroyTrigger, DestroyImmediatelyOnTrigger, Atk, HitBoxIdsOnDestroy, FixedYPos);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,PrefabName,CharacterBuffId,MonsterBuffId,CharacterStackBuffId,CharacterStackBuffInterval,PreventCharacterStackBuffExpire,MonsterStackBuffId,MonsterStackBuffInterval,PreventMonsterStackBuffExpire,EffectDelay,Duration,DestroyTrigger,DestroyImmediatelyOnTrigger,Atk,HitBoxIdsOnDestroy,FixedYPos";
		}

		public static string GetTypesCsv()
		{
			return "int,string,int,int,int,float,bool,int,float,bool,float,float,ESpecialFieldEvent,bool,float,int[],float";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new SpecialFieldTable();
			table.Load(new CsvTableLoader<int, TSpecialField>(text));
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
				foreach (TSpecialField row in Table.Values)
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
			return this.ValidateRefKey("CharacterBuffId", typeof(TBuff))
				&& this.ValidateRefKey("MonsterBuffId", typeof(TBuff))
				&& this.ValidateRefKey("CharacterStackBuffId", typeof(TBuff))
				&& this.ValidateRefKey("MonsterStackBuffId", typeof(TBuff))
				&& this.ValidateRefKey("HitBoxIdsOnDestroy", typeof(THitBox));
		}
	}
}
