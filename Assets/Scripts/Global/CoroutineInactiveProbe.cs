using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

public static class CoroutineInactiveProbe
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CEmitDeferred_003Ed__4 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public string objectName;

		public string managedStack;

		private Cysharp.Threading.Tasks.YieldAwaitable.Awaiter _003C_003Eu__1;

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

	private const string TargetMessage = "Coroutine couldn't be started";

	private static readonly HashSet<string> _loggedObjects;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Install()
	{
	}

	private static void OnLogMessageReceived(string message, string stackTrace, LogType type)
	{
	}

	[AsyncStateMachine(typeof(_003CEmitDeferred_003Ed__4))]
	private static UniTaskVoid EmitDeferred(string objectName, string managedStack)
	{
		return default;
	}

	private static string ExtractObjectName(string message)
	{
		return null;
	}
}
