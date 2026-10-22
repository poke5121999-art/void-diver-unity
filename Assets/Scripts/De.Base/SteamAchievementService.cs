using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Log;
using Steamworks;
using VContainer;

public class SteamAchievementService : MonoBase, IAchievementService
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CInitializeAsync_003Ed__13 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public SteamAchievementService _003C_003E4__this;

		private CancellationTokenSource _003Ccts_003E5__2;

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

	[Inject]
	private SeriLogger _logger;

	private const float STORE_INTERVAL = 60f;

	private bool _isDirty;

	private float _storeTimer;

	private Callback<UserStatsReceived_t> _userStatsReceivedCallback;

	public bool IsAvailable => false;

	public bool IsInitialized { get; private set; }

	protected override void Awake()
	{
	}

	private void Update()
	{
	}

	[AsyncStateMachine(typeof(_003CInitializeAsync_003Ed__13))]
	public UniTask<bool> InitializeAsync()
	{
		return default;
	}

	public void SetProgress(int achievementId, int current, int max)
	{
	}

	public void SetAchievement(int achievementId)
	{
	}

	public int GetProgress(int achievementId)
	{
		return 0;
	}

	public bool IsAchieved(int achievementId)
	{
		return false;
	}

	public void StoreStats()
	{
	}

	public void ClearAchievement(int achievementId)
	{
	}
}
