using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class CampaignDetailPanelPresenter : BasePanelPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CStartCampaign_003Ed__32 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CampaignDetailPanelPresenter _003C_003E4__this;

		private EAnomalyType _003CanomalyType_003E5__2;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private CampaignDetailPanelView _view;

	[InjectModel]
	private CampaignDetailPanelModel _model;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameInputManager _gameInputManager;

	[SerializeField]
	private CampaignDetailBackgroundController _backgroundController;

	private CompositeDisposable _disposable;

	private int _firstRewardCount;

	private int _rewardCount;

	protected override BasePanelModel PanelModel => null;

	private InputActions.UiPadActions UiPadActions => default;

	private EDifficulty Difficulty
	{
		get
		{
			return EDifficulty.Easy;
		}
		set
		{
		}
	}

	private void OnInject(CampaignDetailPanelModel model)
	{
	}

	protected override void OnDestroy()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void OnOptionLeftClick()
	{
	}

	private void OnOptionRightClick()
	{
	}

	private void UpdateDifficultyUI()
	{
	}

	private void BuildFirstRewardGoods(Goods[] rewards, bool isNormalCampaign, bool isCleared)
	{
	}

	private void BuildRewardGoods(Goods[] rewards)
	{
	}

	[AsyncStateMachine(typeof(_003CStartCampaign_003Ed__32))]
	private UniTask StartCampaign()
	{
		return default;
	}

	private void DecreaseDifficulty()
	{
	}

	private void IncreaseDifficulty()
	{
	}
}
