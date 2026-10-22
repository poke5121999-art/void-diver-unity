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

public class CharacterSettingPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnOpenAsync_003Ed__30 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CharacterSettingPopupPresenter _003C_003E4__this;

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
	private struct _003CUpdateSkinAsync_003Ed__45 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CharacterSettingPopupPresenter _003C_003E4__this;

		private int _003CnewSkinPresetId_003E5__2;

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

	[View]
	private CharacterSettingPopupView _view;

	[InjectModel]
	private CharacterSettingPopupModel _model;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	private CharacterSettingScrollerModel _characterSettingScrollerModel;

	private Dictionary<int, Character> _characters;

	private int _selectedCharacterId;

	private int[] _characterSkinIds;

	private int _selectedSkinId;

	private int _currentSkinPresetId;

	private GameInputManager GameInputManager => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private InputActions.UiPadActions UiPadActions => default;

	private GamePlayer MyPlayer => null;

	private Character SelectedCharacter => null;

	private int ActiveSkillSlotCount => 0;

	public override string OpenSfx => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__30))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	private void InitGamePadNav()
	{
	}

	protected override void OnInputEnabled()
	{
	}

	private void OnOptionLeftClick()
	{
	}

	private void OnOptionRightClick()
	{
	}

	private void OnTabLeftClick()
	{
	}

	private void OnTabRightClick()
	{
	}

	private void SetCharacter(int id)
	{
	}

	private void SetSkin(int skinId)
	{
	}

	private void SetSkinPreset(int skinPresetId)
	{
	}

	private void UpdateSkinUi()
	{
	}

	private void UpdatePagenationCells(int presetCount, int currentIndex)
	{
	}

	private void UpdateCharacterUis()
	{
	}

	private void UpdateSkillInfo()
	{
	}

	[AsyncStateMachine(typeof(_003CUpdateSkinAsync_003Ed__45))]
	private UniTask UpdateSkinAsync()
	{
		return default;
	}

	private void OnCharacterUpdated(Character character)
	{
	}

	private void OnCharacterSlotClick(CharacterSettingSlotModel model)
	{
	}

	private void OnSkillSettingButtonClick()
	{
	}

	private void OnMoveToPrevSkin()
	{
	}

	private void OnMoveToNextSkin()
	{
	}

	private void OnMoveToPrevSkinPreset()
	{
	}

	private void OnMoveToNextSkinPreset()
	{
	}
}
