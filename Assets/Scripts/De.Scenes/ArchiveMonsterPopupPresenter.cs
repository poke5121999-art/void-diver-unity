using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class ArchiveMonsterPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__17 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArchiveMonsterPopupPresenter _003C_003E4__this;

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

	[InjectModel]
	private ArchiveMonsterPopupModel _model;

	[View]
	private ArchiveMonsterPopupView _view;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PlayerManager _playerManager;

	private MonsterSlotRowScrollerModel _scrollerModel;

	private MonsterInfoPanelModel _monsterPanelModel;

	private CompositeDisposable _panelDisposable;

	private MonsterSlotModel _selectedSlot;

	private MonsterSlotModel _focusedSlot;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	public override string OpenSfx => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__17))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private int[] BuildMonsterIds()
	{
		return null;
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnBack()
	{
	}

	private void OnSlotClick(MonsterSlotModel slotModel)
	{
	}

	private void OnSlotEnter(MonsterSlotModel slotModel)
	{
	}

	private void OnSlotExit(MonsterSlotModel slotModel)
	{
	}

	private void UpdateInfo(MonsterSlotModel slotModel)
	{
	}

	private void UpdateSelectedObject(MonsterSlotModel slotModel)
	{
	}
}
