using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Log;
using VContainer;

public class AppManager : MonoBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CAuthenticateAsync_003Ed__9 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public EPlatform platform;

		public AppManager _003C_003E4__this;

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
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private SeriLogger _logger;

	public AppVersion Version;

	public string AccountId { get; private set; }

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	[AsyncStateMachine(typeof(_003CAuthenticateAsync_003Ed__9))]
	public UniTask<bool> AuthenticateAsync(EPlatform platform)
	{
		return default;
	}

	private bool AuthenticateLocal()
	{
		return false;
	}

	private bool AuthenticateSteam()
	{
		return false;
	}

	public bool IsOnline()
	{
		return false;
	}

	public static void Quit()
	{
	}
}
