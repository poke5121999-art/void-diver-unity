using System.Collections.Generic;
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

		public static HitBoxTable Table { get; set; } = new HitBoxTable();

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
			bool result = true;
			if (Table != null)
			{
				foreach (THitBox row in Table.Values)
				{
					if (!row.Validate())
					{
						result = false;
					}
				}
			}
			return result;
		}

		public static string ResolveExpression(string expression)
		{
			int colon = expression.IndexOf(':');
			if (colon < 0 || !int.TryParse(expression.Substring(0, colon), out int id))
			{
				return "[?id]";
			}
			if (!Table.TryGetValue(id, out THitBox hitBox))
			{
				return string.Format("[?{0}]", id);
			}
			HitBoxInfo info = hitBox.HitBoxInfo;
			string field = expression.Substring(colon + 1);
			int dot = field.IndexOf('.');
			if (dot < 0)
			{
				if (field.Equals("Duration"))
				{
					return info.duration.ToFloatString();
				}
				if (field.Equals("MultiHitInterval"))
				{
					return info.multiHitInterval.ToFloatString();
				}
				if (field.Equals("CollisionEndTime"))
				{
					return info.collisionEndTime.ToFloatString();
				}
				return "[?field]";
			}
			string head = field.Substring(0, dot);
			string tail = field.Substring(dot + 1);
			int bracket = head.IndexOf('[');
			if (bracket >= 0 && head.EndsWith("]") && int.TryParse(head.Substring(bracket + 1, head.Length - (bracket + 1) - 1), out int index) && head.Substring(0, bracket).Equals("CollisionEvents", StringComparison.OrdinalIgnoreCase))
			{
				if (info.collisionEvents != null && index < info.collisionEvents.Length && info.collisionEvents[index].TryResolveExpression(tail, out string result))
				{
					return result;
				}
			}
			return "[?field]";
		}

		public string[] ExtractExpressions()
		{
			var list = new List<string>();
			string prefix = string.Format("hitbox:{0}:", Id);
			HitBoxInfo info = HitBoxInfo;
			list.Add(prefix + "Duration");
			list.Add(prefix + "MultiHitInterval");
			list.Add(prefix + "CollisionEndTime");
			if (info != null && info.collisionEvents != null)
			{
				for (int i = 0; i < info.collisionEvents.Length; i++)
				{
					string eventPrefix = prefix + string.Format("CollisionEvents[{0}].", i);
					foreach (string expression in info.collisionEvents[i].ExtractExpressions())
					{
						list.Add(eventPrefix + expression);
					}
				}
			}
			return list.ToArray();
		}

		public bool Validate()
		{
			string errorMessage = null;
			bool valid = HitBoxInfo.Validate(out errorMessage);
			return this.Check(valid, errorMessage);
		}
	}
}
