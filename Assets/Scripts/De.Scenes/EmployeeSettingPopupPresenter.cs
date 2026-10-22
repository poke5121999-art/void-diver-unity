using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using Spine.Unity;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public class EmployeeSettingPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CConfirmAndChangeEmployeeAsync_003Ed__30 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public EmployeeSettingPopupPresenter _003C_003E4__this;

		public Employee newEmployee;

		private UniTask<EmployeeChangeConfirmPopupPresenter>.Awaiter _003C_003Eu__1;

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
	private struct _003CConfirmAndSelectEmployeeAsync_003Ed__29 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public EmployeeSettingPopupPresenter _003C_003E4__this;

		public Employee employee;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__22 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public EmployeeSettingPopupPresenter _003C_003E4__this;

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
	private struct _003CSetEmployeeSkeletonAsync_003Ed__37 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public EmployeeSettingPopupPresenter _003C_003E4__this;

		public int npcId;

		public CancellationToken ct;

		public SkeletonGraphic skeleton;

		public int employeeId;

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
	private EmployeeSettingPopupView _view;

	[InjectModel]
	private EmployeeSettingPopupModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	private EmployeeScrollerModel _scrollerModel;

	private int _originalEmployeeId;

	private Employee _selectedEmployee;

	private CancellationTokenSource _skeletonCts;

	private bool _isButtonMode;

	private GameInputManager GameInputManager => null;

	private InputActions.UiActions UiActions => default;

	public override bool IsInputActionPopup => false;

	private EmployeeScrollerPresenter _scrollerPresenter => null;

	private SalesSlot SalesSlot => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__22))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	public override void Close(object result = null)
	{
	}

	private void RefreshSkeletonToken()
	{
	}

	private void CancelSkeletonLoad()
	{
	}

	private void OnSlotClick(EmployeeCardModel cardModel)
	{
	}

	[AsyncStateMachine(typeof(_003CConfirmAndSelectEmployeeAsync_003Ed__29))]
	private UniTaskVoid ConfirmAndSelectEmployeeAsync(Employee employee)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CConfirmAndChangeEmployeeAsync_003Ed__30))]
	private UniTaskVoid ConfirmAndChangeEmployeeAsync(Employee newEmployee)
	{
		return default;
	}

	private void ApplyEmployeeChange(Employee newEmployee)
	{
	}

	private void OnSlotHover(EmployeeCardModel cardModel)
	{
	}

	private void SelectInitialEmployee()
	{
	}

	private void OnEmptyEmployee()
	{
	}

	private void UpdateEmployeeLayout()
	{
	}

	private void SetEmployeeDisplay(Employee employee, TextMeshProUGUI nameText, TextMeshProUGUI lvText, Slider expSlider, Slider panicSlider, Image panicGauge, Image conditionFace, SkeletonGraphic skeleton)
	{
	}

	[AsyncStateMachine(typeof(_003CSetEmployeeSkeletonAsync_003Ed__37))]
	private UniTaskVoid SetEmployeeSkeletonAsync(SkeletonGraphic skeleton, int npcId, int employeeId, CancellationToken ct)
	{
		return default;
	}

	private void RefreshScroller()
	{
	}

	private void OnScrollerOutOfRight()
	{
	}

	private void OnMoveScrollStarted(InputAction.CallbackContext ctx)
	{
	}

	private void ExitButtonMode()
	{
	}
}
