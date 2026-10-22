using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Scene;
using VContainer;

public class InGameQuickSlotPanelPresenter : BasePanelPresenter
{
	[View]
	private InGameQuickSlotPanelView _view;

	[InjectModel]
	private InGameQuickSlotPanelModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private GamePingManager _pingManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SceneManager _sceneManager;

	[Inject]
	private SoundManager _soundManager;

	private int _selectedSlotIndex;

	protected override BasePanelModel PanelModel => null;

	private GamePlayer Player => null;

	private InputActions.InGamePadActions InGamePadActions => default;

	private int EquipmentSwapSlotIndex => 0;

	private void Start()
	{
	}

	private void OnCharacterReady()
	{
	}

	private void InitGamePadUiActions()
	{
	}

	private void OnControlDeviceChanged(EControlDevice device)
	{
	}

	private void MoveSelectionIndicator(int slotIndex)
	{
	}

	private void BuildWeaponSwapSlot()
	{
	}

	private void OnWeaponSwapSlotEnter(bool isActiveWeapon)
	{
	}

	private void OnWeaponSwapSlotExit()
	{
	}

	private void BuildQuickSlots()
	{
	}

	private void OnWeaponSwapSlotClick(InGameWeaponSwapSlotModel model)
	{
	}

	private void OnQuickSlotEnter(int slotIndex)
	{
	}

	private void OnQuickSlotExit(int slotIndex)
	{
	}

	private void OnQuickSlotClick(InGameQuickSlotModel model)
	{
	}

	private bool TrySendUseItem(int itemId, ESkillInputEvent inputEvent)
	{
		return false;
	}

	private void OnActiveWeaponChanged()
	{
	}

	private void OnQuickSlotMoveLeft()
	{
	}

	private void OnQuickSlotMoveRight()
	{
	}

	private void OnQuickSlotUseDown()
	{
	}

	private void OnQuickSlotUseUp()
	{
	}
}
