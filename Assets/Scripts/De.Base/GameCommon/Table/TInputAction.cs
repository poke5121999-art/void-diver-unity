using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TInputAction : ITableData<int>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static InputActionTable Table { get; set; }

		public static GroupInputActionTable GroupTable { get; set; }

		public int Pk
		{
			get
			{
				return 0;
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
		public string Name => null;

		private string NameKey => null;

		public static event Action OnTableChanged
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public void Parse(string[] columns)
		{
		}

		public string ToCsv()
		{
			return null;
		}

		public static void SaveCsv(string filePath)
		{
		}

		public static string GetColumnsCsv()
		{
			return null;
		}

		public static string GetTypesCsv()
		{
			return null;
		}

		public static void LoadFromPath(string filePath)
		{
		}

		public static void LoadFromText(string text)
		{
		}

		public static void InvokeTableChangeEvent()
		{
		}

		public static bool ValidateTable()
		{
			return false;
		}

		public bool Validate()
		{
			return false;
		}

		private bool ValidateInputActionName(string fieldName, string actionName)
		{
			return false;
		}
	}
}
