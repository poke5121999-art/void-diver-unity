using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using VContainer;

public class EmployeeLevelUpPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__12 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public EmployeeLevelUpPopupPresenter _003C_003E4__this;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CSetEmployeeSkeletonAsync_003Ed__19 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public EmployeeLevelUpPopupPresenter _003C_003E4__this;

		public CancellationToken ct;

		private UniTask<UnitSkeletonData>.Awaiter _003C_003Eu__1;

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
	private EmployeeLevelUpPopupView _view;

	[InjectModel]
	private EmployeeLevelUpPopupModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private SoundManager _soundManager;

	private CancellationTokenSource _skeletonCts;

	private GameInputManager GameInputManager => null;

	private InputActions.UiActions UiActions => default;

	public override bool IsInputActionPopup => false;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__12))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void ResetSpinePhysics()
	{
	}

	public override void Close(object result = null)
	{
	}

	private void CancelSkeletonLoad()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	private void InitTitle()
	{
	}

	private void InitBubbleText()
	{
	}

	[AsyncStateMachine(typeof(_003CSetEmployeeSkeletonAsync_003Ed__19))]
	private UniTaskVoid SetEmployeeSkeletonAsync(CancellationToken ct)
	{
		return default;
	}

	private static string GetRandomText(string[] texts)
	{
		return null;
	}

	private void InitGamePadNav()
	{
	}
}
