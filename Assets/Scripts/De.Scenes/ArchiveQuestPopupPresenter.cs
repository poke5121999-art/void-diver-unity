using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using NemoLib.Popup;
using VContainer;

public class ArchiveQuestPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__17 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArchiveQuestPopupPresenter _003C_003E4__this;

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
	private ArchiveQuestPopupModel _model;

	[View]
	private ArchiveQuestPopupView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private DialogReplayService _dialogReplayService;

	private QuestScrollerModel _scrollerModel;

	private QuestInfoPanelModel _panelModel;

	private QuestSlotModel _selectedSlot;

	private QuestSlotModel _focusedSlot;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	public override string OpenSfx => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__17))]
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

	private void OnBack()
	{
	}

	private void OnSlotClick(QuestSlotModel slotModel)
	{
	}

	private void OnSlotEnter(QuestSlotModel slotModel)
	{
	}

	private void OnSlotExit(QuestSlotModel slotModel)
	{
	}

	private void OnShowDialog()
	{
	}

	private void UpdateSelectedObject(QuestSlotModel slotModel)
	{
	}

	private void UpdateInfo(QuestSlotModel slotModel)
	{
	}

	private string GetSubTitle(QuestScrollerPresenter.EFilterType filterType)
	{
		return null;
	}
}
