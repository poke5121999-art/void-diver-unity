using System;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using SomeNet.Table;
using UnityEngine;

namespace GameCommon.Table
{
	[Serializable]
	public class TBuffVfx : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static BuffVfxTable Table { get; set; }

		public int Pk
		{
			get
			{
				return BuffId;
			}
			set
			{
			}
		}

		public int BuffId { get; set; }

		public string Vfx { get; set; }

		public float VfxDuration { get; set; }

		public EUnitBoneType VfxBoneType { get; set; }

		public Vector3 VfxOffset { get; set; }

		public string Sfx { get; set; }

		public string DotSfx { get; set; }

		public string DotVfx { get; set; }

		public EUnitBoneType DotVfxBoneType { get; set; }

		public Vector3 DotVfxOffset { get; set; }

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
			BuffId = TableParse.Int(columns[0]);
			Vfx = TableParse.Str(columns[1]);
			VfxDuration = TableParse.Float(columns[2]);
			VfxBoneType = TableParse.Enum<EUnitBoneType>(columns[3]);
			VfxOffset = TableParse.Json<Vector3>(columns[4]);
			Sfx = TableParse.Str(columns[5]);
			DotSfx = TableParse.Str(columns[6]);
			DotVfx = TableParse.Str(columns[7]);
			DotVfxBoneType = TableParse.Enum<EUnitBoneType>(columns[8]);
			DotVfxOffset = TableParse.Json<Vector3>(columns[9]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(BuffId, Vfx, VfxDuration, VfxBoneType, VfxOffset, Sfx, DotSfx, DotVfx, DotVfxBoneType, DotVfxOffset);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "BuffId,Vfx,VfxDuration,VfxBoneType,VfxOffset,Sfx,DotSfx,DotVfx,DotVfxBoneType,DotVfxOffset";
		}

		public static string GetTypesCsv()
		{
			return "int,string,float,EUnitBoneType,Vector3,string,string,string,EUnitBoneType,Vector3";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new BuffVfxTable();
			table.Load(new CsvTableLoader<int, TBuffVfx>(text));
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
