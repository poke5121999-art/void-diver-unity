using System.Collections.Generic;
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

public class CraftingPopupPresenter : GamePopupPresenter
{
	private enum EToggleGoodsSubType
	{
		All = 0,
		Weapon = 1,
		Accessory = 2,
		Consumable = 3,
		Misc = 4
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass54_0
	{
		public long count;

		internal Goods _003CTryCraftAsync_003Eb__0(GoodsData _)
		{
			return null;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__34 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CraftingPopupPresenter _003C_003E4__this;

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
	private struct _003CTryCraftAsync_003Ed__54 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public CraftingPopupPresenter _003C_003E4__this;

		private _003C_003Ec__DisplayClass54_0 _003C_003E8__1;

		private int _003CcraftingId_003E5__2;

		private UniTask<CraftingConfirmPopupPresenter>.Awaiter _003C_003Eu__1;

		private UniTask<object>.Awaiter _003C_003Eu__2;

		private UniTask<ResCraftingGoods>.Awaiter _003C_003Eu__3;

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
	private CraftingPopupView _view;

	[InjectModel]
	private CraftingPopupModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameChannel _gameChannel;

	private CraftingGoodsScrollerModel _scrollerModel;

	private CraftingMaterialPanelModel _materialPanelModel;

	private CraftingGoodsSlotModel _selectedSlot;

	private CraftingGoodsSlotModel _focusedSlot;

	private readonly ReactiveProperty<int> _rxSelectedCraftingId;

	private int _currentToggleIndex;

	private readonly Dictionary<int, (int FocusedIndex, float ScrollPosition)> _tabFocusCache;

	private readonly Subject<Goods> _craftingNavigateSubject;

	private GameInputManager GameInputManager => null;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	private CraftingGoodsScrollerPresenter CraftingGoodsScrollerPresenter => null;

	private PlayerInventory MyInventory => null;

	private bool AllowAccessStash => false;

	private bool HasEnoughMaterials => false;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__34))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void OnCraftableToggleChanged(bool isOn)
	{
	}

	private void UpdateAvailable()
	{
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

	private void UpdateSelectedObject(CraftingGoodsSlotModel slot)
	{
	}

	private void UpdateInfo(CraftingGoodsSlotModel slot)
	{
	}

	private void OnMaterialConfirm()
	{
	}

	private void OnTabLeft()
	{
	}

	private void OnTabRight()
	{
	}

	private void ChangeToggle(int index)
	{
	}

	private void InitFilterToggles()
	{
	}

	private void OnSelectedCraftingIdChanged(int craftingId)
	{
	}

	private void OnCraftingNavigate(Goods goods)
	{
	}

	private void OnSlotEnter(CraftingGoodsSlotModel slot)
	{
	}

	private void OnSlotExit(CraftingGoodsSlotModel slot)
	{
	}

	private void OnSlotClick(CraftingGoodsSlotModel slot)
	{
	}

	private bool CheckCraftable(int craftingId)
	{
		return false;
	}

	[AsyncStateMachine(typeof(_003CTryCraftAsync_003Ed__54))]
	private UniTask<bool> TryCraftAsync()
	{
		return default;
	}
}
