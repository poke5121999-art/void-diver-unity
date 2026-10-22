using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using VContainer;

public class TycoonArtifactSettingPopupPresenter : GamePopupPresenter
{
	private enum EItemPanelType
	{
		Inventory = 0,
		Stash = 1
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CConfirmAndSelectArtifactAsync_003Ed__24 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public TycoonArtifactSettingPopupPresenter _003C_003E4__this;

		public InventoryGoodsSlotModel slotModel;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

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
	private struct _003COnOpenAsync_003Ed__17 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public TycoonArtifactSettingPopupPresenter _003C_003E4__this;

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
	private TycoonArtifactSettingPopupView _view;

	[InjectModel]
	private TycoonArtifactSettingPopupModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	private PlayerInventoryPanelModel _inventoryPanelModel;

	private StashPanelModel _stashPanelModel;

	private InventoryGoodsSlotModel _selectedSlotModel;

	private EItemPanelType _itemPanelType;

	private GameInputManager GameInputManager => null;

	private InputActions.UiActions UiActions => default;

	public override bool IsInputActionPopup => false;

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

	private void InitInventoryPanel()
	{
	}

	private void InitStashPanel()
	{
	}

	private void OnInventorySlotClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnStashSlotClick(InventoryGoodsSlotModel slotModel)
	{
	}

	[AsyncStateMachine(typeof(_003CConfirmAndSelectArtifactAsync_003Ed__24))]
	private UniTaskVoid ConfirmAndSelectArtifactAsync(InventoryGoodsSlotModel slotModel)
	{
		return default;
	}

	private void SetSelectedArtifact(InventoryGoodsSlotModel slotModel)
	{
	}

	private void ClearSelectedArtifact()
	{
	}

	private bool ShouldAutoSwitchToStash()
	{
		return false;
	}

	private void SwapItemPanel()
	{
	}
}
