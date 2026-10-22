using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using NemoLib.Mvp;
using NemoLib.Popup;
using VContainer;

public class ArtifactShopPopupPresenter : GamePopupPresenter
{
	private enum EItemPanelType
	{
		Inventory = 0,
		Stash = 1
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__23 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArtifactShopPopupPresenter _003C_003E4__this;

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
	private struct _003COnSlotClickAsync_003Ed__30 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public InventoryGoodsSlotModel slotModel;

		public ArtifactShopPopupPresenter _003C_003E4__this;

		private Goods _003Cgoods_003E5__2;

		private int _003Cprice_003E5__3;

		private long _003Cexp_003E5__4;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

		private UniTask<ResSellArtifact>.Awaiter _003C_003Eu__2;

		private UniTask.Awaiter _003C_003Eu__3;

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
	private ArtifactShopPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameUiEventSystem _uiEventSystem;

	private PlayerInventoryPanelModel _inventoryPanelModel;

	private StashPanelModel _stashPanelModel;

	private EItemPanelType _itemPanelType;

	private GameInputManager GameInputManager => null;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private Inventory MyInventory => null;

	private InputActions.UiActions UiActions => default;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__23))]
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

	private void SetLayoutInputEnabled(bool value)
	{
	}

	private bool ShouldAutoSwitchToStash()
	{
		return false;
	}

	private void SwapItemPanel()
	{
	}

	private void Back()
	{
	}

	[AsyncStateMachine(typeof(_003COnSlotClickAsync_003Ed__30))]
	private UniTaskVoid OnSlotClickAsync(InventoryGoodsSlotModel slotModel)
	{
		return default;
	}

	private void OnEquipmentSlotClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private int GetDealPrice(Goods goods)
	{
		return 0;
	}

	private long CalculateArtifactDealExp(Goods goods, int sellPrice)
	{
		return 0L;
	}
}
