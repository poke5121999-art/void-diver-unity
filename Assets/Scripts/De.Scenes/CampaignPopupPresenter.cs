using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using VContainer;

public class CampaignPopupPresenter : GamePopupPresenter
{
	private enum ELayout
	{
		None = 0,
		List = 1,
		Detail = 2
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__25 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CampaignPopupPresenter _003C_003E4__this;

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
	private CampaignPopupView _view;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private PlayerManager _playerManager;

	private ELayout _layout;

	private ECampaignCategory _category;

	private int _campaignId;

	private CampaignListPanelModel _listPanelModel;

	private CampaignDetailPanelModel _detailPanelModel;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	public override string OpenSfx => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__25))]
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

	private void SetLayout(ELayout layout)
	{
	}

	private void OnBack()
	{
	}

	private void OnCampaignSlotClick(int campaignId)
	{
	}

	private void OnTabLeft()
	{
	}

	private void OnTabRight()
	{
	}

	private void OnCategoryToggleValueChange(ECampaignCategory category)
	{
	}
}
