using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class ArchiveGoodsPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__19 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArchiveGoodsPopupPresenter _003C_003E4__this;

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
	private ArchiveGoodsPopupModel _model;

	[View]
	private ArchiveGoodsPopupView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	private GoodsSlotRowScrollerModel _scrollerModel;

	private EquipmentInfoPanelModel _equipmentPanelModel;

	private ItemInfoPanelModel _itemPanelModel;

	private CompositeDisposable _panelDisposable;

	private GoodsSlotModel _selectedSlot;

	private GoodsSlotModel _focusedSlot;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	public override string OpenSfx => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__19))]
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

	private void OnShowNote()
	{
	}

	private string GetTitle()
	{
		return null;
	}

	private string GetSubTitle()
	{
		return null;
	}

	private Goods[] BuildGoodsArray()
	{
		return null;
	}

	private void OnSlotClick(GoodsSlotModel slotModel)
	{
	}

	private void OnSlotEnter(GoodsSlotModel slotModel)
	{
	}

	private void OnSlotExit(GoodsSlotModel slotModel)
	{
	}

	private void UpdateInfo(GoodsSlotModel slotModel)
	{
	}

	private void UpdateSelectedObject(GoodsSlotModel slotModel)
	{
	}

	private void SetActivePanelInputEnabled(bool value)
	{
	}
}
