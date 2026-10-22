using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Newtonsoft.Json;

namespace Base.Utility
{
	public static class TableHelper
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CLoadEncryptedTablesAsync_003Ed__3 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			private UniTask.Awaiter _003C_003Eu__1;

			private void MoveNext()
			{
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		public static readonly JsonConverter[] JsonConverters;

		public static void LoadTables()
		{
		}

		public static void LoadEncryptedTables()
		{
		}

		[AsyncStateMachine(typeof(_003CLoadEncryptedTablesAsync_003Ed__3))]
		public static UniTask LoadEncryptedTablesAsync()
		{
			return default;
		}

		public static void LoadLocalizedTables()
		{
		}

		public static void LoadTable(Type dataType, string filePath)
		{
		}

		public static void LoadDecryptedTable(Type dataType, string filePath, byte key)
		{
		}
	}
}
