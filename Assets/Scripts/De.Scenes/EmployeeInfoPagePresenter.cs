using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class EmployeeInfoPagePresenter : BasePanelPresenter
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass31_0
	{
		public EmployeeCardModel cardModel;

		internal bool _003CPurifyEmployeeAsync_003Eb__0(SalesSlot s)
		{
			return false;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COpenFireConfirmAsync_003Ed__33 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public EmployeeInfoPagePresenter _003C_003E4__this;

		public EmployeeCardModel cardModel;

		private UniTask<EmployeeFirePopupPresenter>.Awaiter _003C_003Eu__1;

		private UniTask<object>.Awaiter _003C_003Eu__2;

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
	private struct _003CPurifyEmployeeAsync_003Ed__31 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public EmployeeCardModel cardModel;

		public EmployeeInfoPagePresenter _003C_003E4__this;

		private _003C_003Ec__DisplayClass31_0 _003C_003E8__1;

		private UniTask<ConsumeConfirmPopupPresenter>.Awaiter _003C_003Eu__1;

		private UniTask<object>.Awaiter _003C_003Eu__2;

		private UniTask<bool>.Awaiter _003C_003Eu__3;

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
	private struct _003CSetEmployeeSkeletonAsync_003Ed__29 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public int employeeId;

		public EmployeeInfoPagePresenter _003C_003E4__this;

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
	private EmployeeInfoPageView _view;

	[InjectModel]
	private EmployeeInfoPageModel _model;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	private EmployeeScrollerModel _scrollerModel;

	private CompositeDisposable _disposable;

	private CompositeDisposable _inputDisposable;

	private CancellationTokenSource _skeletonCts;

	private EmployeeCardModel _selectedCardModel;

	private bool _isButtonMode;

	protected override BasePanelModel PanelModel => null;

	private EmployeeScrollerPresenter _scrollerPresenter => null;

	private void OnInject(EmployeeInfoPageModel model)
	{
	}

	private void ResetSpinePhysics()
	{
	}

	protected override void OnDestroy()
	{
	}

	private void OnSlotHover(EmployeeCardModel cardModel)
	{
	}

	private void OnSlotClick(EmployeeCardModel cardModel)
	{
	}

	private void UpdateEmployeeDisplay(EmployeeCardModel cardModel)
	{
	}

	private bool IsNearPanic(int employeeId)
	{
		return false;
	}

	private static string GetRandomText(string[] texts)
	{
		return null;
	}

	private static string GetRandomSaySfx()
	{
		return null;
	}

	private void RequestEmployeeSkeleton(int employeeId)
	{
	}

	private void CancelSkeletonLoad()
	{
	}

	[AsyncStateMachine(typeof(_003CSetEmployeeSkeletonAsync_003Ed__29))]
	private UniTaskVoid SetEmployeeSkeletonAsync(int employeeId, CancellationToken ct)
	{
		return default;
	}

	private void OnCleansingBtnClicked(EmployeeCardModel cardModel)
	{
	}

	[AsyncStateMachine(typeof(_003CPurifyEmployeeAsync_003Ed__31))]
	private UniTaskVoid PurifyEmployeeAsync(EmployeeCardModel cardModel)
	{
		return default;
	}

	private void OnFireBtnClicked(EmployeeCardModel cardModel)
	{
	}

	[AsyncStateMachine(typeof(_003COpenFireConfirmAsync_003Ed__33))]
	private UniTaskVoid OpenFireConfirmAsync(EmployeeCardModel cardModel)
	{
		return default;
	}

	private void RefreshScroller()
	{
	}

	private void RefreshCountText()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnEscapePressed()
	{
	}

	private void EnterButtonMode(EmployeeCardModel cardModel)
	{
	}

	private void ExitButtonMode()
	{
	}
}
