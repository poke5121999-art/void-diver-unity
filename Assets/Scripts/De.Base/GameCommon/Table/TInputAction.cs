using System;
using System.Linq;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TInputAction : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static InputActionTable Table { get; set; } = new InputActionTable();

		public static GroupInputActionTable GroupTable { get; set; }

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

		public EControlDevice ControlDevice { get; set; }

		public string ActionName { get; set; }

		public int BindingIndex { get; set; }

		public string ModifierActionName { get; set; }

		public int ModifierBindingIndex { get; set; }

		public string SyncActionName { get; set; }

		public int SyncBindingIndex { get; set; }

		public bool IsRebindable { get; set; }

		public Action<ITableData<int>> OnDataLoaded { get; set; }

		[Localized]
		public string Name => LocalizationUtility.GetLocalizedText(NameKey);

		private string NameKey => string.Format("{0}_{1}_{2}", "TInputAction", "Name", Id);

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
			ControlDevice = TableParse.Enum<EControlDevice>(columns[1]);
			ActionName = TableParse.Str(columns[2]);
			BindingIndex = TableParse.Int(columns[3]);
			ModifierActionName = TableParse.Str(columns[4]);
			ModifierBindingIndex = TableParse.Int(columns[5]);
			SyncActionName = TableParse.Str(columns[6]);
			SyncBindingIndex = TableParse.Int(columns[7]);
			IsRebindable = TableParse.Bool(columns[8]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, ControlDevice, ActionName, BindingIndex, ModifierActionName, ModifierBindingIndex, SyncActionName, SyncBindingIndex, IsRebindable);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,ControlDevice,ActionName,BindingIndex,ModifierActionName,ModifierBindingIndex,SyncActionName,SyncBindingIndex,IsRebindable";
		}

		public static string GetTypesCsv()
		{
			return "int,EControlDevice,string,int,string,int,string,int,bool";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new InputActionTable();
			table.Load(new CsvTableLoader<int, TInputAction>(text));
			Table = table;
			GroupTable = new GroupInputActionTable();
			GroupTable.Init(table.Values.GroupBy(row => row.ControlDevice).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));
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
				foreach (TInputAction row in Table.Values)
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
			bool result = this.ValidateRefKey("NameKey", typeof(TLocalizedText));
			result &= ValidateInputActionName("ActionName", ActionName);
			result &= ValidateInputActionName("ModifierActionName", ModifierActionName);
			result &= ValidateInputActionName("SyncActionName", SyncActionName);
			return result;
		}

		private bool ValidateInputActionName(string fieldName, string actionName)
		{
			if (string.IsNullOrEmpty(actionName))
			{
				return true;
			}
			return true;
		}
	}
}
