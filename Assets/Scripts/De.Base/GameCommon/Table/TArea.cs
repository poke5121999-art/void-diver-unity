using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;
using UnityEngine;

namespace GameCommon.Table
{
	[Serializable]
	public class TArea : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static AreaTable Table { get; set; } = new AreaTable();

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

		public int BitFlagIndex { get; set; }

		public bool DefaultUnlocked { get; set; }

		public Vector3 CameraPos { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TArea", "Name", Id);

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
			BitFlagIndex = TableParse.Int(columns[1]);
			DefaultUnlocked = TableParse.Bool(columns[2]);
			CameraPos = TableParse.Json<Vector3>(columns[3]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, BitFlagIndex, DefaultUnlocked, CameraPos);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,BitFlagIndex,DefaultUnlocked,CameraPos";
		}

		public static string GetTypesCsv()
		{
			return "int,int,bool,Vector3";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new AreaTable();
			table.Load(new CsvTableLoader<int, TArea>(text));
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
				foreach (TArea row in Table.Values)
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
			return this.ValidateRefKey("NameKey", typeof(TLocalizedText));
		}
	}
}
