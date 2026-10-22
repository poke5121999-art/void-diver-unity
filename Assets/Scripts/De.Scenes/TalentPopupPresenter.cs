using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TalentPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__23 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public TalentPopupPresenter _003C_003E4__this;

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

	[InjectModel]
	private TalentPopupModel _model;

	[View]
	private TalentPopupView _view;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	private Dictionary<(int SourceTalentId, int TargetTalentId), UINodeLink> _nodeLinks;

	private Dictionary<int, Transform> _talentSlotTfs;

	private Dictionary<int, TalentSlotModel> _talentSlotModels;

	private TalentSlotModel _focusedSlot;

	private TalentSlotModel _selectedSlot;

	private TalentInfoPanelModel _infoPanelModel;

	private GameInputManager GameInputManager => null;

	private int NumberOfSlotsEachRow => 0;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private void Start()
	{
	}

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__23))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void InitInfoPanel()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void OnButtonSelected(int talentId)
	{
	}

	private void ScrollToSelectable(Button button)
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

	private void BuildTalentSlots()
	{
	}

	private void UpdateNodeLinks()
	{
	}

	private void UpdateSelectedObject(TalentSlotModel slotModel)
	{
	}

	private void UpdateInfo(TalentSlotModel slotModel)
	{
	}

	private void OnTalentUpdated(Talent talent)
	{
	}

	private void OnSlotEnter(TalentSlotModel slotModel)
	{
	}

	private void OnSlotExit(TalentSlotModel slotModel)
	{
	}

	private void OnSlotClick(TalentSlotModel slotModel)
	{
	}
}
