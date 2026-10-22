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

public class EmployeeHirePagePresenter : BasePanelPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COpenHireConfirmAsync_003Ed__25 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public EmployeeCardModel cardModel;

		public EmployeeHirePagePresenter _003C_003E4__this;

		private UniTask<TycoonHireConfirmPopupPresenter>.Awaiter _003C_003Eu__1;

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
	private struct _003CSetEmployeeSkeletonAsync_003Ed__24 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public int employeeId;

		public EmployeeHirePagePresenter _003C_003E4__this;

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
	private EmployeeHirePageView _view;

	[InjectModel]
	private EmployeeHirePageModel _model;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SoundManager _soundManager;

	private EmployeeScrollerModel _scrollerModel;

	private CompositeDisposable _disposable;

	private CompositeDisposable _inputDisposable;

	private CancellationTokenSource _skeletonCts;

	protected override BasePanelModel PanelModel => null;

	private EmployeeScrollerPresenter _scrollerPresenter => null;

	private void OnInject(EmployeeHirePageModel model)
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

	private void UpdateBubbleText(int employeeId)
	{
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

	[AsyncStateMachine(typeof(_003CSetEmployeeSkeletonAsync_003Ed__24))]
	private UniTaskVoid SetEmployeeSkeletonAsync(int employeeId, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003COpenHireConfirmAsync_003Ed__25))]
	private UniTaskVoid OpenHireConfirmAsync(EmployeeCardModel cardModel)
	{
		return default;
	}

	private void RefreshScroller()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}
}
