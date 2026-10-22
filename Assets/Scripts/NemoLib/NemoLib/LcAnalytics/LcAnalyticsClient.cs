using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Log;
using UnityEngine.Networking;
using VContainer;

namespace NemoLib.LcAnalytics
{
	public class LcAnalyticsClient : NativeBase, IDisposable
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CSendInternalAsync_003Ed__16 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

			public LcAnalyticsEvent evt;

			public LcAnalyticsClient _003C_003E4__this;

			public CancellationToken ct;

			private UnityWebRequest _003Crequest_003E5__2;

			private UnityWebRequestAsyncOperation _003Coperation_003E5__3;

			private bool _003Caborted_003E5__4;

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

		private const string _trackPath = "/v1/track";

		private const string _contentTypeJson = "application/json";

		private const string _logSourceContext = "[LcAnalyticsClient]";

		[Inject]
		private SeriLogger _logger;

		private readonly CancellationTokenSource _cts;

		private readonly SynchronizationContext _mainContext;

		private readonly string _trackUrl;

		private readonly string _authHeader;

		private readonly int _timeoutSec;

		private bool IsConfigured => false;

		public LcAnalyticsClient(string endpoint, string writeKey, int timeoutSec = 5)
		{
		}

		public void Send(LcAnalyticsEvent evt)
		{
		}

		private void RunOnMain(Action action)
		{
		}

		private bool TryGetActiveToken(out CancellationToken token)
		{
			token = default;
			return false;
		}

		public void Dispose()
		{
		}

		[AsyncStateMachine(typeof(_003CSendInternalAsync_003Ed__16))]
		private UniTaskVoid SendInternalAsync(LcAnalyticsEvent evt, CancellationToken ct)
		{
			return default;
		}

		private static string BuildBasicAuth(string writeKey)
		{
			return null;
		}
	}
}
