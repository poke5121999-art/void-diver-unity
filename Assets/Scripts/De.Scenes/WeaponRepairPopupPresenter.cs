using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class WeaponRepairPopupPresenter : GamePopupPresenter
{
	private enum EFocusPanel
	{
		Inventory = 0,
		RepairAll = 1,
		RepairPanel = 2
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnConfirmRepairAsync_003Ed__34 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public Goods goods;

		public WeaponRepairPopupPresenter _003C_003E4__this;

		private UniTask<RepairConfirmPopupPresenter>.Awaiter _003C_003Eu__1;

		private UniTask<object>.Awaiter _003C_003Eu__2;

		private UniTask<ResRepairGoods>.Awaiter _003C_003Eu__3;

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
	private struct _003COnOpenAsync_003Ed__20 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public WeaponRepairPopupPresenter _003C_003E4__this;

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
	private struct _003COnRepairAllAsync_003Ed__29 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public WeaponRepairPopupPresenter _003C_003E4__this;

		private UniTask<RepairConfirmPopupPresenter>.Awaiter _003C_003Eu__1;

		private UniTask<object>.Awaiter _003C_003Eu__2;

		private UniTask<ResRepairAllGoods>.Awaiter _003C_003Eu__3;

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
	private WeaponRepairPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private ToastManager _toastManager;

	private PlayerInventoryPanelModel _inventoryPanelModel;

	private WeaponRepairPanelModel _repairPanelModel;

	private readonly ReactiveProperty<InventoryGoodsSlotModel> _rxSelectedSlotModel;

	private int _totalRepairCost;

	private readonly List<(EGoodsSlotCategory Category, int SlotIndex)> _repairableSlots;

	private EFocusPanel _focusPanel;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__20))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void InitGamePadNav()
	{
	}

	private void FocusPanel(EFocusPanel panel)
	{
	}

	protected override void OnNavOutOfBounds(ENavDir dir)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void UpdateRepairAllUi()
	{
	}

	private void CollectRepairableSlots()
	{
	}

	private void OnRepairAllClicked()
	{
	}

	[AsyncStateMachine(typeof(_003COnRepairAllAsync_003Ed__29))]
	private UniTaskVoid OnRepairAllAsync()
	{
		return default;
	}

	private void OnSelectedSlotChanged(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnEquipmentSlotClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnConfirmRepair(Goods goods)
	{
	}

	[AsyncStateMachine(typeof(_003COnConfirmRepairAsync_003Ed__34))]
	private UniTaskVoid OnConfirmRepairAsync(Goods goods)
	{
		return default;
	}

	private void ClearSelection()
	{
	}

	private void Back()
	{
	}
}
