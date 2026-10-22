using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using Spine.Unity;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class TycoonMainPagePresenter : BasePanelPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CCancelArtifactAsync_003Ed__52 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public TycoonMainPagePresenter _003C_003E4__this;

		private UniTask<TycoonCancelConfirmPopupPresenter>.Awaiter _003C_003Eu__1;

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
	private struct _003COpenArtifactSettingAsync_003Ed__47 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public TycoonMainPagePresenter _003C_003E4__this;

		private UniTask<TycoonArtifactSettingPopupPresenter>.Awaiter _003C_003Eu__1;

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
	private struct _003COpenEmployeeSettingAsync_003Ed__49 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public TycoonMainPagePresenter _003C_003E4__this;

		private UniTask<EmployeeSettingPopupPresenter>.Awaiter _003C_003Eu__1;

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
	private struct _003CProcessResultAsync_003Ed__55 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public TycoonMainPagePresenter _003C_003E4__this;

		private TycoonFinishResult _003Cresult_003E5__2;

		private UniTask<TycoonFinishResult>.Awaiter _003C_003Eu__1;

		private UniTask<TycoonResultPopupPresenter>.Awaiter _003C_003Eu__2;

		private UniTask<object>.Awaiter _003C_003Eu__3;

		private UniTask<EmployeeLevelUpPopupPresenter>.Awaiter _003C_003Eu__4;

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
	private struct _003CSetEmployeeSkeletonAsync_003Ed__41 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public TycoonMainPagePresenter _003C_003E4__this;

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

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CUpdateEmployeeDisplayAsync_003Ed__37 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public SalesSlot slot;

		public TycoonMainPagePresenter _003C_003E4__this;

		public Employee employee;

		public CancellationToken ct;

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

	[View]
	private TycoonMainPageView _view;

	[InjectModel]
	private TycoonMainPageModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameUiEventSystem _uiEventSystem;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameEventSystem _eventSystem;

	private SalesSlotScrollerModel _scrollerModel;

	private SalesSlotModel _selectedSlot;

	private CompositeDisposable _disposable;

	private readonly List<Button> _activeButtons;

	private CompositeDisposable _inputDisposable;

	private CancellationTokenSource _skeletonCts;

	private bool _isButtonMode;

	private bool _suppressSlotPopup;

	protected override BasePanelModel PanelModel => null;

	private SalesSlotScrollerPresenter _scrollerPresenter => null;

	private SalesSlot SelectedSlot => null;

	private Employee SelectedEmployee => null;

	private bool IsCurrentSlotStarted => false;

	private void OnInject(TycoonMainPageModel model)
	{
	}

	protected override void OnDestroy()
	{
	}

	private void CancelSkeletonLoad()
	{
	}

	private void OnSlotSelected(SalesSlotModel slotModel)
	{
	}

	private void TryOpenSlotPopup()
	{
	}

	private void ResetDisplay()
	{
	}

	private void ResetSpinePhysics()
	{
	}

	private void UpdateDisplay()
	{
	}

	[AsyncStateMachine(typeof(_003CUpdateEmployeeDisplayAsync_003Ed__37))]
	private UniTaskVoid UpdateEmployeeDisplayAsync(SalesSlot slot, Employee employee, CancellationToken ct)
	{
		return default;
	}

	private void UpdateBubbleText(SalesSlot slot, Employee employee, TEmployee employeeData)
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

	[AsyncStateMachine(typeof(_003CSetEmployeeSkeletonAsync_003Ed__41))]
	private UniTask SetEmployeeSkeletonAsync(SkeletonGraphic skeleton, int npcId, int employeeId, CancellationToken ct)
	{
		return default;
	}

	private void UpdateArtifactDisplay(SalesSlot slot, Employee employee)
	{
	}

	private void UpdateArtifactGradeVfx(EGoodsGradeType grade)
	{
	}

	private void UpdateCompleteBubbleGold(SalesSlot slot, Employee employee)
	{
	}

	private void UpdateButtons(SalesSlot slot, Employee employee)
	{
	}

	private void OnSetArtifactClick()
	{
	}

	[AsyncStateMachine(typeof(_003COpenArtifactSettingAsync_003Ed__47))]
	private UniTaskVoid OpenArtifactSettingAsync()
	{
		return default;
	}

	private void OnSettingEmployeeClick()
	{
	}

	[AsyncStateMachine(typeof(_003COpenEmployeeSettingAsync_003Ed__49))]
	private UniTaskVoid OpenEmployeeSettingAsync()
	{
		return default;
	}

	private void OnChangeEmployeeClick()
	{
	}

	private void OnCancelArtifactClick()
	{
	}

	[AsyncStateMachine(typeof(_003CCancelArtifactAsync_003Ed__52))]
	private UniTaskVoid CancelArtifactAsync()
	{
		return default;
	}

	private void OnStartClick()
	{
	}

	private void OnResultClick()
	{
	}

	[AsyncStateMachine(typeof(_003CProcessResultAsync_003Ed__55))]
	private UniTaskVoid ProcessResultAsync()
	{
		return default;
	}

	private void TriggerNegotiationTycoonAchievement(TycoonFinishResult result)
	{
	}

	private List<SalesSlotModel> BuildSalesSlotModels()
	{
		return null;
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnScrollerOutOfRight()
	{
	}

	private void OnButtonsOutOfLeft()
	{
	}

	private void RefreshButtonNav()
	{
	}
}
