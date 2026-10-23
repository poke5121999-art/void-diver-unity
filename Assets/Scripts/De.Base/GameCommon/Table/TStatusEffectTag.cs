using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;
using UnityEngine;

namespace GameCommon.Table
{
	[Serializable]
	public class TStatusEffectTag : ITableData<EStatusEffectTag>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static StatusEffectTagTable Table { get; set; } = new StatusEffectTagTable();

		public EStatusEffectTag Pk
		{
			get
			{
				return Tag;
			}
			set
			{
			}
		}

		public EStatusEffectTag Tag { get; set; }

		public string Vfx { get; set; }

		public float VfxDuration { get; set; }

		public EUnitBoneType VfxBoneType { get; set; }

		public Vector3 VfxOffset { get; set; }

		public bool VfxScaleByColliderRadius { get; set; }

		public string Sfx { get; set; }

		public string DotSfx { get; set; }

		public string DotVfx { get; set; }

		public EUnitBoneType DotVfxBoneType { get; set; }

		public Vector3 DotVfxOffset { get; set; }

		public Action<ITableData<EStatusEffectTag>> OnDataLoaded { get; set; }

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
			Tag = TableParse.Enum<EStatusEffectTag>(columns[0]);
			Vfx = TableParse.Str(columns[1]);
			VfxDuration = TableParse.Float(columns[2]);
			VfxBoneType = TableParse.Enum<EUnitBoneType>(columns[3]);
			VfxOffset = TableParse.Json<Vector3>(columns[4]);
			VfxScaleByColliderRadius = TableParse.Bool(columns[5]);
			Sfx = TableParse.Str(columns[6]);
			DotSfx = TableParse.Str(columns[7]);
			DotVfx = TableParse.Str(columns[8]);
			DotVfxBoneType = TableParse.Enum<EUnitBoneType>(columns[9]);
			DotVfxOffset = TableParse.Json<Vector3>(columns[10]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Tag, Vfx, VfxDuration, VfxBoneType, VfxOffset, VfxScaleByColliderRadius, Sfx, DotSfx, DotVfx, DotVfxBoneType, DotVfxOffset);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Tag,Vfx,VfxDuration,VfxBoneType,VfxOffset,VfxScaleByColliderRadius,Sfx,DotSfx,DotVfx,DotVfxBoneType,DotVfxOffset";
		}

		public static string GetTypesCsv()
		{
			return "EStatusEffectTag,string,float,EUnitBoneType,Vector3,bool,string,string,string,EUnitBoneType,Vector3";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new StatusEffectTagTable();
			table.Load(new CsvTableLoader<EStatusEffectTag, TStatusEffectTag>(text));
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
				foreach (TStatusEffectTag row in Table.Values)
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
			return this.ValidateRefSfx("Sfx")
				&& this.ValidateRefSfx("DotSfx");
		}
	}
}
