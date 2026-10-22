using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class CharacterInfoPanelPresenter : BasePanelPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CSetEquipmentAsync_003Ed__34 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CharacterInfoPanelPresenter _003C_003E4__this;

		private Cysharp.Threading.Tasks.YieldAwaitable.Awaiter _003C_003Eu__1;

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
	private struct _003CUpdateSkinsAsync_003Ed__33 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CharacterInfoPanelPresenter _003C_003E4__this;

		private Character _003Ccharacter_003E5__2;

		private int _003CweaponId_003E5__3;

		private UniTask<UnitSkeletonData>.Awaiter _003C_003Eu__1;

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
	private CharacterInfoPanelModel _model;

	[View]
	private CharacterInfoPanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameInputManager _inputManager;

	private readonly string _toggleKey;

	private CompositeDisposable _disposable;

	private CompositeDisposable _inputDisposable;

	protected override BasePanelModel PanelModel => null;

	private InputActions.UiActions UiActions => default;

	private CharacterModel CharacterModel => null;

	private TCharacter CharacterData => null;

	private int AccessorySlotCount => 0;

	private int ArtifactSlotCount => 0;

	private bool ShowAllStats => false;

	private void OnInject(CharacterInfoPanelModel model)
	{
	}

	private void OnDisable()
	{
	}

	protected override void OnDestroy()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnCharacterInfoPanelToggleClick()
	{
	}

	private void SetInfo()
	{
	}

	private void UpdateStats()
	{
	}

	private void UpdateBuffStats()
	{
	}

	[AsyncStateMachine(typeof(_003CUpdateSkinsAsync_003Ed__33))]
	private UniTask UpdateSkinsAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CSetEquipmentAsync_003Ed__34))]
	private UniTask SetEquipmentAsync()
	{
		return default;
	}
}
