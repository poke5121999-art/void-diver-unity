using System;
using System.Runtime.CompilerServices;
using GameCommon.Data;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class THitBox : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static HitBoxTable Table { get; set; }

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

		public HitBoxInfo HitBoxInfo { get; set; }

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
			HitBoxInfo = TableParse.Json<HitBoxInfo>(columns[2]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Memo, HitBoxInfo);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Memo,HitBoxInfo";
		}

		public static string GetTypesCsv()
		{
			return "int,string,HitBoxInfo";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new HitBoxTable();
			table.Load(new CsvTableLoader<int, THitBox>(text));
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

		public static string ResolveExpression(string expression)
		{
			return null;
		}

		public string[] ExtractExpressions()
		{
			return null;
		}

		public bool Validate()
		{
			return false;
		}
	}
}
