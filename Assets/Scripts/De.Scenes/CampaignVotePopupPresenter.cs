using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using VContainer;

public class CampaignVotePopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__26 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CampaignVotePopupPresenter _003C_003E4__this;

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
	private CampaignVotePopupView _view;

	[InjectModel]
	private CampaignVotePopupModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _gameCampaignManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	private readonly List<CampaignVoteStateModel> _voteStates;

	private bool _isClosing;

	private float _voteTimeLeft;

	private bool _isVoteTimerRunning;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private ulong ClientId => 0uL;

	private bool HasVoted => false;

	private int VoteTimeLimit => 0;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__26))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	protected override void Update()
	{
	}

	private void StartVoteTimer()
	{
	}

	private void UpdateVoteTimer()
	{
	}

	private void StopVoteTimer()
	{
	}

	private void InitGamePadNav()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	private void InitUis()
	{
	}

	private void OnYesButtonClick()
	{
	}

	private void OnNoButtonClick()
	{
	}

	private void OnVoteCampaign(NtfVoteCampaign message)
	{
	}

	public override void Close(object result = null)
	{
	}
}
