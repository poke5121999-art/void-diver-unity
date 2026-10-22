using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class TalentInfoPanelPresenter : BasePanelPresenter
{
	private enum EFocusPanel
	{
		None = 0,
		Base = 1,
		UnlockMaterial = 2
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CUnlockTalentAsync_003Ed__21 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public TalentInfoPanelPresenter _003C_003E4__this;

		public int talentId;

		private UniTask<TalentConfirmPopupPresenter>.Awaiter _003C_003Eu__1;

		private UniTask<object>.Awaiter _003C_003Eu__2;

		private UniTask<ResUnlockTalent>.Awaiter _003C_003Eu__3;

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
	private TalentInfoPanelModel _model;

	[View]
	private TalentInfoPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SoundManager _soundManager;

	private UnlockMaterialPanelModel _unlockMaterialPanelModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(TalentInfoPanelModel model)
	{
	}

	private void OnRefresh()
	{
	}

	private void InitGamePadNav()
	{
	}

	private bool HasUnlockMaterials()
	{
		return false;
	}

	private void FocusPanel(EFocusPanel panel, ENavDir entryDir)
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnUpgradeClick()
	{
	}

	private void TryUnlockTalent(int talentId)
	{
	}

	[AsyncStateMachine(typeof(_003CUnlockTalentAsync_003Ed__21))]
	private UniTask UnlockTalentAsync(int talentId)
	{
		return default;
	}
}
