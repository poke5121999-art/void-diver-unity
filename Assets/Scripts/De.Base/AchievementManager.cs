using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Table;
using NemoLib.Log;
using UniRx;
using VContainer;

public class AchievementManager : NativeBase, IDisposable
{
	public class AchievementProgress
	{
		public int Id { get; set; }

		public long Progress { get; set; }

		public long Goal { get; set; }
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CInitializeAsync_003Ed__8 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public AchievementManager _003C_003E4__this;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private IAchievementService _achievementService;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private AccountRecordManager _accountRecordManager;

	private List<AchievementProgress> _progresses;

	private bool _isInitialized;

	private CompositeDisposable _disposable;

	[AsyncStateMachine(typeof(_003CInitializeAsync_003Ed__8))]
	public UniTask<bool> InitializeAsync()
	{
		return default;
	}

	void IDisposable.Dispose()
	{
	}

	private long GetProgress(TAchievement achievement)
	{
		return 0L;
	}

	private void OnGameEventOccured(IBaseEvent triggerEvent)
	{
	}
}
