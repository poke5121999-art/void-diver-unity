using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class TycoonResultPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__10 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public TycoonResultPopupPresenter _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

		private Cysharp.Threading.Tasks.YieldAwaitable.Awaiter _003C_003Eu__2;

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

	[View]
	private TycoonResultPopupView _view;

	[InjectModel]
	private TycoonResultPopupModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SoundManager _soundManager;

	private GameInputManager GameInputManager => null;

	private InputActions.UiActions UiActions => default;

	public override bool IsInputActionPopup => false;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__10))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void InitTitle()
	{
	}

	private void InitEmployeeResult()
	{
	}

	private void InitArtifact()
	{
	}

	private void InitSellResult()
	{
	}

	private void SetupPriceDelta(bool hasSold)
	{
	}

	private void OnConfirm()
	{
	}

	private void InitGamePadNav()
	{
	}
}
