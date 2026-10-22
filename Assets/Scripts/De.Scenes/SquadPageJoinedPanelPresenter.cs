using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class SquadPageJoinedPanelPresenter : BasePanelPresenter
{
	public enum EFocusPanel
	{
		None = 0,
		Base = 1,
		Cards = 2
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnSettingButtonClickAsync_003Ed__23 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public SquadPageJoinedPanelPresenter _003C_003E4__this;

		private UniTask<RoomSettingPopupPresenter>.Awaiter _003C_003Eu__1;

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

	[InjectModel]
	private SquadPageJoinedPanelModel _model;

	[View]
	private SquadPageJoinedPanelView _view;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private ToastManager _toastManager;

	private CompositeDisposable _disposable;

	private CompositeDisposable _cardDisposable;

	private readonly List<SquadMemberCardModel> _cardModels;

	private SquadMemberCardPresenter.EFocusPanel _currentCardFocusPanel;

	private bool _isCodeVisible;

	protected override BasePanelModel PanelModel => null;

	private EFocusPanel FocusedPanel => EFocusPanel.None;

	protected override void OnDestroy()
	{
	}

	private void OnInject(SquadPageJoinedPanelModel model)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnJoinLobby()
	{
	}

	[AsyncStateMachine(typeof(_003COnSettingButtonClickAsync_003Ed__23))]
	private UniTask OnSettingButtonClickAsync()
	{
		return default;
	}

	private void OnLoungeCodeClick()
	{
	}

	private void OnOnOffButtonClick()
	{
	}

	private void UpdateLoungeCode()
	{
	}

	private void UpdateLobby()
	{
	}

	private void OnLobbyDataUpdated(MatchingLobby lobby)
	{
	}

	private void UpdateMemberCards()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void FocusPanel(EFocusPanel panel, ENavDir entryDir)
	{
	}

	private void FocusCard(int index, ENavDir entryDir)
	{
	}

	private void OnBaseOutOfBottom()
	{
	}

	private void OnCardOutOfTop(int index)
	{
	}

	private void OnCardOutOfBottom(int index)
	{
	}

	private void OnParadoxSlotEnter(ParadoxSimpleSlotModel slotModel)
	{
	}

	private void OnParadoxSlotExit(ParadoxSimpleSlotModel slotModel)
	{
	}

	private void OnSkillSlotEnter(SkillSlotModel slotModel)
	{
	}

	private void OnSkillSlotExit(SkillSlotModel slotModel)
	{
	}
}
