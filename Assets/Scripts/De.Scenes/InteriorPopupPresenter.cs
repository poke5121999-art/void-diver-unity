using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine;
using VContainer;

public class InteriorPopupPresenter : GamePopupPresenter
{
	private enum EFocusPanel
	{
		Area = 0,
		Interior = 1
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass45_0
	{
		public InteriorPopupPresenter _003C_003E4__this;

		public TInteriorShop shop;

		internal void _003COnBuyInterior_003Eb__0()
		{
		}

		internal void _003COnBuyInterior_003Eb__1()
		{
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CConfirmPurchase_003Ed__46 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public InteriorPopupPresenter _003C_003E4__this;

		public TInteriorShop shop;

		private UniTask<ConsumeConfirmPopupPresenter>.Awaiter _003C_003Eu__1;

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
	private struct _003COnBuyInterior_003Ed__45 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public InteriorPopupPresenter _003C_003E4__this;

		public TInteriorShop shop;

		private _003C_003Ec__DisplayClass45_0 _003C_003E8__1;

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
	private struct _003COnOpenAsync_003Ed__27 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public InteriorPopupPresenter _003C_003E4__this;

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
	private struct _003COnUnlockedInteriorClick_003Ed__44 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public InteriorPopupPresenter _003C_003E4__this;

		public TInteriorShop shop;

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
	private struct _003CPlayInteriorTransition_003Ed__49 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public InteriorPopupPresenter _003C_003E4__this;

		public Action onApply;

		private BlockScope _003CblockScope_003E5__2;

		private CanvasGroup _003CcanvasGroup_003E5__3;

		private TaskAwaiter _003C_003Eu__1;

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
	private struct _003CSendBuyInteriorRequest_003Ed__47 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public InteriorPopupPresenter _003C_003E4__this;

		public int interiorShopId;

		private UniTask<ResBuyInterior>.Awaiter _003C_003Eu__1;

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
	private InteriorPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameLoungeManager _gameLoungeManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private GameMapManager _mapManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SoundManager _soundManager;

	private TextSlotScrollerPresenter _areaScroller;

	private InteriorSlotScrollerPresenter _interiorScroller;

	private TextSlotScrollerModel _areaScrollerModel;

	private InteriorSlotScrollerModel _interiorScrollerModel;

	private int _selectedAreaId;

	private int[] _areaIds;

	private TextSlotModel _selectedAreaTextSlot;

	private TextSlotModel _focusedAreaTextSlot;

	private EFocusPanel _focusPanel;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => null;

	private GameInputManager GameInputManager => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__27))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void FocusPanel(EFocusPanel panel)
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

	private void InitAreaScroller()
	{
	}

	private void OnAreaSlotClick(TextSlotModel slot)
	{
	}

	private void OnAreaSlotEnter(TextSlotModel slot)
	{
	}

	private void OnAreaSlotExit(TextSlotModel slot)
	{
	}

	private void InitInteriorScroller()
	{
	}

	private void ShowInteriorList(bool show)
	{
	}

	private void RefreshInteriorList()
	{
	}

	private InteriorSlotModel[] SortInteriorItems(TInteriorShop[] interiors)
	{
		return null;
	}

	private static int GetGroupOrder(TInteriorShop shop)
	{
		return 0;
	}

	private bool IsInteriorUnlocked(TInteriorShop shop)
	{
		return false;
	}

	private bool HasEnoughCost(int consumeGold, int consumeCoin)
	{
		return false;
	}

	private void OnInteriorSlotClick(InteriorSlotModel slotModel)
	{
	}

	[AsyncStateMachine(typeof(_003COnUnlockedInteriorClick_003Ed__44))]
	private UniTaskVoid OnUnlockedInteriorClick(TInteriorShop shop)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003COnBuyInterior_003Ed__45))]
	private UniTaskVoid OnBuyInterior(TInteriorShop shop)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CConfirmPurchase_003Ed__46))]
	private UniTask<bool> ConfirmPurchase(TInteriorShop shop)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CSendBuyInteriorRequest_003Ed__47))]
	private UniTask<bool> SendBuyInteriorRequest(int interiorShopId)
	{
		return default;
	}

	private static string GetInteriorName(TInteriorShop shop)
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CPlayInteriorTransition_003Ed__49))]
	private UniTask PlayInteriorTransition(Action onApply)
	{
		return default;
	}
}
