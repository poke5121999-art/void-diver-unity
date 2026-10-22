using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputActions : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	public struct PlayerActions
	{
		private InputActions m_Wrapper;

		public InputAction MoveKeyboard => null;

		public InputAction MovePad => null;

		public InputAction AimMouse => null;

		public InputAction AimPad => null;

		public InputAction SwapPingEmojiPad => null;

		public InputAction WheelSelect => null;

		public InputAction PingEmojiPad => null;

		public InputAction EmojiKeyboard => null;

		public InputAction PingKeyboard => null;

		public InputAction PointMinimap => null;

		public InputAction Escape => null;

		public bool enabled => false;

		public PlayerActions(InputActions wrapper)
		{
			m_Wrapper = null;
		}

		public InputActionMap Get()
		{
			return null;
		}

		public void Enable()
		{
		}

		public void Disable()
		{
		}

		public static implicit operator InputActionMap(PlayerActions set)
		{
			return null;
		}

		public void AddCallbacks(IPlayerActions instance)
		{
		}

		private void UnregisterCallbacks(IPlayerActions instance)
		{
		}

		public void RemoveCallbacks(IPlayerActions instance)
		{
		}

		public void SetCallbacks(IPlayerActions instance)
		{
		}
	}

	public struct PlayerFuncActions
	{
		private InputActions m_Wrapper;

		public InputAction Run => null;

		public InputAction Interact => null;

		public InputAction SwapWeapon => null;

		public InputAction UseItem => null;

		public InputAction SkillBasicAttack => null;

		public InputAction SkillBasicAttackPadStick => null;

		public InputAction SkillOne => null;

		public InputAction SkillTwo => null;

		public InputAction SkillThree => null;

		public InputAction SkillFour => null;

		public InputAction SkillEquipment => null;

		public InputAction SkillDash => null;

		public bool enabled => false;

		public PlayerFuncActions(InputActions wrapper)
		{
			m_Wrapper = null;
		}

		public InputActionMap Get()
		{
			return null;
		}

		public void Enable()
		{
		}

		public void Disable()
		{
		}

		public static implicit operator InputActionMap(PlayerFuncActions set)
		{
			return null;
		}

		public void AddCallbacks(IPlayerFuncActions instance)
		{
		}

		private void UnregisterCallbacks(IPlayerFuncActions instance)
		{
		}

		public void RemoveCallbacks(IPlayerFuncActions instance)
		{
		}

		public void SetCallbacks(IPlayerFuncActions instance)
		{
		}
	}

	public struct InGameActions
	{
		private InputActions m_Wrapper;

		public InputAction Minimap => null;

		public InputAction MinimapEscape => null;

		public InputAction ChangeViewTarget => null;

		public InputAction ToggleKeyGuide => null;

		public InputAction Chat => null;

		public InputAction Menu => null;

		public InputAction Inventory => null;

		public bool enabled => false;

		public InGameActions(InputActions wrapper)
		{
			m_Wrapper = null;
		}

		public InputActionMap Get()
		{
			return null;
		}

		public void Enable()
		{
		}

		public void Disable()
		{
		}

		public static implicit operator InputActionMap(InGameActions set)
		{
			return null;
		}

		public void AddCallbacks(IInGameActions instance)
		{
		}

		private void UnregisterCallbacks(IInGameActions instance)
		{
		}

		public void RemoveCallbacks(IInGameActions instance)
		{
		}

		public void SetCallbacks(IInGameActions instance)
		{
		}
	}

	public struct InGameKeyboardActions
	{
		private InputActions m_Wrapper;

		public bool enabled => false;

		public InGameKeyboardActions(InputActions wrapper)
		{
			m_Wrapper = null;
		}

		public InputActionMap Get()
		{
			return null;
		}

		public void Enable()
		{
		}

		public void Disable()
		{
		}

		public static implicit operator InputActionMap(InGameKeyboardActions set)
		{
			return null;
		}

		public void AddCallbacks(IInGameKeyboardActions instance)
		{
		}

		private void UnregisterCallbacks(IInGameKeyboardActions instance)
		{
		}

		public void RemoveCallbacks(IInGameKeyboardActions instance)
		{
		}

		public void SetCallbacks(IInGameKeyboardActions instance)
		{
		}
	}

	public struct InGamePadActions
	{
		private InputActions m_Wrapper;

		public InputAction QuickSlotMoveLeft => null;

		public InputAction QuickSlotMoveRight => null;

		public InputAction QuickSlotUse => null;

		public bool enabled => false;

		public InGamePadActions(InputActions wrapper)
		{
			m_Wrapper = null;
		}

		public InputActionMap Get()
		{
			return null;
		}

		public void Enable()
		{
		}

		public void Disable()
		{
		}

		public static implicit operator InputActionMap(InGamePadActions set)
		{
			return null;
		}

		public void AddCallbacks(IInGamePadActions instance)
		{
		}

		private void UnregisterCallbacks(IInGamePadActions instance)
		{
		}

		public void RemoveCallbacks(IInGamePadActions instance)
		{
		}

		public void SetCallbacks(IInGamePadActions instance)
		{
		}
	}

	public struct UiActions
	{
		private InputActions m_Wrapper;

		public InputAction Toggle => null;

		public InputAction Escape => null;

		public InputAction Organize => null;

		public InputAction Switch => null;

		public InputAction NextFlow => null;

		public InputAction Skip => null;

		public InputAction MarkGoods => null;

		public InputAction ToggleQuestHud => null;

		public InputAction InventorySelectOne => null;

		public InputAction ShopMaxQuantity => null;

		public InputAction CloseMenu => null;

		public InputAction UseItem => null;

		public InputAction ShowDialog => null;

		public InputAction ShowNote => null;

		public InputAction TooltipMoveToCrafting => null;

		public bool enabled => false;

		public UiActions(InputActions wrapper)
		{
			m_Wrapper = null;
		}

		public InputActionMap Get()
		{
			return null;
		}

		public void Enable()
		{
		}

		public void Disable()
		{
		}

		public static implicit operator InputActionMap(UiActions set)
		{
			return null;
		}

		public void AddCallbacks(IUiActions instance)
		{
		}

		private void UnregisterCallbacks(IUiActions instance)
		{
		}

		public void RemoveCallbacks(IUiActions instance)
		{
		}

		public void SetCallbacks(IUiActions instance)
		{
		}
	}

	public struct UiPadActions
	{
		private InputActions m_Wrapper;

		public InputAction Submit => null;

		public InputAction QuickSubmit => null;

		public InputAction MovePanel => null;

		public InputAction ScrollRect => null;

		public InputAction MoveScroll => null;

		public InputAction TabLeft => null;

		public InputAction TabRight => null;

		public InputAction OptionLeft => null;

		public InputAction OptionRight => null;

		public InputAction DragAndDrop => null;

		public InputAction DropGoods => null;

		public InputAction InsertGoods => null;

		public InputAction Restore => null;

		public InputAction Delete => null;

		public InputAction MuteToggle => null;

		public InputAction IncreaseQuantityLarge => null;

		public InputAction DecreaseQuantityLarge => null;

		public bool enabled => false;

		public UiPadActions(InputActions wrapper)
		{
			m_Wrapper = null;
		}

		public InputActionMap Get()
		{
			return null;
		}

		public void Enable()
		{
		}

		public void Disable()
		{
		}

		public static implicit operator InputActionMap(UiPadActions set)
		{
			return null;
		}

		public void AddCallbacks(IUiPadActions instance)
		{
		}

		private void UnregisterCallbacks(IUiPadActions instance)
		{
		}

		public void RemoveCallbacks(IUiPadActions instance)
		{
		}

		public void SetCallbacks(IUiPadActions instance)
		{
		}
	}

	public struct UiKeyboardActions
	{
		private InputActions m_Wrapper;

		public InputAction SwapWeapon => null;

		public InputAction SetQuickSlot => null;

		public InputAction InventorySelectHalf => null;

		public InputAction Slider => null;

		public InputAction InventoryMoveToStashAll => null;

		public InputAction Confirm => null;

		public InputAction Cancel => null;

		public bool enabled => false;

		public UiKeyboardActions(InputActions wrapper)
		{
			m_Wrapper = null;
		}

		public InputActionMap Get()
		{
			return null;
		}

		public void Enable()
		{
		}

		public void Disable()
		{
		}

		public static implicit operator InputActionMap(UiKeyboardActions set)
		{
			return null;
		}

		public void AddCallbacks(IUiKeyboardActions instance)
		{
		}

		private void UnregisterCallbacks(IUiKeyboardActions instance)
		{
		}

		public void RemoveCallbacks(IUiKeyboardActions instance)
		{
		}

		public void SetCallbacks(IUiKeyboardActions instance)
		{
		}
	}

	public struct CutsceneActions
	{
		private InputActions m_Wrapper;

		public InputAction Skip => null;

		public bool enabled => false;

		public CutsceneActions(InputActions wrapper)
		{
			m_Wrapper = null;
		}

		public InputActionMap Get()
		{
			return null;
		}

		public void Enable()
		{
		}

		public void Disable()
		{
		}

		public static implicit operator InputActionMap(CutsceneActions set)
		{
			return null;
		}

		public void AddCallbacks(ICutsceneActions instance)
		{
		}

		private void UnregisterCallbacks(ICutsceneActions instance)
		{
		}

		public void RemoveCallbacks(ICutsceneActions instance)
		{
		}

		public void SetCallbacks(ICutsceneActions instance)
		{
		}
	}

	public interface IPlayerActions
	{
		void OnMoveKeyboard(InputAction.CallbackContext context);

		void OnMovePad(InputAction.CallbackContext context);

		void OnAimMouse(InputAction.CallbackContext context);

		void OnAimPad(InputAction.CallbackContext context);

		void OnSwapPingEmojiPad(InputAction.CallbackContext context);

		void OnWheelSelect(InputAction.CallbackContext context);

		void OnPingEmojiPad(InputAction.CallbackContext context);

		void OnEmojiKeyboard(InputAction.CallbackContext context);

		void OnPingKeyboard(InputAction.CallbackContext context);

		void OnPointMinimap(InputAction.CallbackContext context);

		void OnEscape(InputAction.CallbackContext context);
	}

	public interface IPlayerFuncActions
	{
		void OnRun(InputAction.CallbackContext context);

		void OnInteract(InputAction.CallbackContext context);

		void OnSwapWeapon(InputAction.CallbackContext context);

		void OnUseItem(InputAction.CallbackContext context);

		void OnSkillBasicAttack(InputAction.CallbackContext context);

		void OnSkillBasicAttackPadStick(InputAction.CallbackContext context);

		void OnSkillOne(InputAction.CallbackContext context);

		void OnSkillTwo(InputAction.CallbackContext context);

		void OnSkillThree(InputAction.CallbackContext context);

		void OnSkillFour(InputAction.CallbackContext context);

		void OnSkillEquipment(InputAction.CallbackContext context);

		void OnSkillDash(InputAction.CallbackContext context);
	}

	public interface IInGameActions
	{
		void OnMinimap(InputAction.CallbackContext context);

		void OnMinimapEscape(InputAction.CallbackContext context);

		void OnChangeViewTarget(InputAction.CallbackContext context);

		void OnToggleKeyGuide(InputAction.CallbackContext context);

		void OnChat(InputAction.CallbackContext context);

		void OnMenu(InputAction.CallbackContext context);

		void OnInventory(InputAction.CallbackContext context);
	}

	public interface IInGameKeyboardActions
	{
	}

	public interface IInGamePadActions
	{
		void OnQuickSlotMoveLeft(InputAction.CallbackContext context);

		void OnQuickSlotMoveRight(InputAction.CallbackContext context);

		void OnQuickSlotUse(InputAction.CallbackContext context);
	}

	public interface IUiActions
	{
		void OnToggle(InputAction.CallbackContext context);

		void OnEscape(InputAction.CallbackContext context);

		void OnOrganize(InputAction.CallbackContext context);

		void OnSwitch(InputAction.CallbackContext context);

		void OnNextFlow(InputAction.CallbackContext context);

		void OnSkip(InputAction.CallbackContext context);

		void OnMarkGoods(InputAction.CallbackContext context);

		void OnToggleQuestHud(InputAction.CallbackContext context);

		void OnInventorySelectOne(InputAction.CallbackContext context);

		void OnShopMaxQuantity(InputAction.CallbackContext context);

		void OnCloseMenu(InputAction.CallbackContext context);

		void OnUseItem(InputAction.CallbackContext context);

		void OnShowDialog(InputAction.CallbackContext context);

		void OnShowNote(InputAction.CallbackContext context);

		void OnTooltipMoveToCrafting(InputAction.CallbackContext context);
	}

	public interface IUiPadActions
	{
		void OnSubmit(InputAction.CallbackContext context);

		void OnQuickSubmit(InputAction.CallbackContext context);

		void OnMovePanel(InputAction.CallbackContext context);

		void OnScrollRect(InputAction.CallbackContext context);

		void OnMoveScroll(InputAction.CallbackContext context);

		void OnTabLeft(InputAction.CallbackContext context);

		void OnTabRight(InputAction.CallbackContext context);

		void OnOptionLeft(InputAction.CallbackContext context);

		void OnOptionRight(InputAction.CallbackContext context);

		void OnDragAndDrop(InputAction.CallbackContext context);

		void OnDropGoods(InputAction.CallbackContext context);

		void OnInsertGoods(InputAction.CallbackContext context);

		void OnRestore(InputAction.CallbackContext context);

		void OnDelete(InputAction.CallbackContext context);

		void OnMuteToggle(InputAction.CallbackContext context);

		void OnIncreaseQuantityLarge(InputAction.CallbackContext context);

		void OnDecreaseQuantityLarge(InputAction.CallbackContext context);
	}

	public interface IUiKeyboardActions
	{
		void OnSwapWeapon(InputAction.CallbackContext context);

		void OnSetQuickSlot(InputAction.CallbackContext context);

		void OnInventorySelectHalf(InputAction.CallbackContext context);

		void OnSlider(InputAction.CallbackContext context);

		void OnInventoryMoveToStashAll(InputAction.CallbackContext context);

		void OnConfirm(InputAction.CallbackContext context);

		void OnCancel(InputAction.CallbackContext context);
	}

	public interface ICutsceneActions
	{
		void OnSkip(InputAction.CallbackContext context);
	}

	private readonly InputActionMap m_Player;

	private List<IPlayerActions> m_PlayerActionsCallbackInterfaces;

	private readonly InputAction m_Player_MoveKeyboard;

	private readonly InputAction m_Player_MovePad;

	private readonly InputAction m_Player_AimMouse;

	private readonly InputAction m_Player_AimPad;

	private readonly InputAction m_Player_SwapPingEmojiPad;

	private readonly InputAction m_Player_WheelSelect;

	private readonly InputAction m_Player_PingEmojiPad;

	private readonly InputAction m_Player_EmojiKeyboard;

	private readonly InputAction m_Player_PingKeyboard;

	private readonly InputAction m_Player_PointMinimap;

	private readonly InputAction m_Player_Escape;

	private readonly InputActionMap m_PlayerFunc;

	private List<IPlayerFuncActions> m_PlayerFuncActionsCallbackInterfaces;

	private readonly InputAction m_PlayerFunc_Run;

	private readonly InputAction m_PlayerFunc_Interact;

	private readonly InputAction m_PlayerFunc_SwapWeapon;

	private readonly InputAction m_PlayerFunc_UseItem;

	private readonly InputAction m_PlayerFunc_SkillBasicAttack;

	private readonly InputAction m_PlayerFunc_SkillBasicAttackPadStick;

	private readonly InputAction m_PlayerFunc_SkillOne;

	private readonly InputAction m_PlayerFunc_SkillTwo;

	private readonly InputAction m_PlayerFunc_SkillThree;

	private readonly InputAction m_PlayerFunc_SkillFour;

	private readonly InputAction m_PlayerFunc_SkillEquipment;

	private readonly InputAction m_PlayerFunc_SkillDash;

	private readonly InputActionMap m_InGame;

	private List<IInGameActions> m_InGameActionsCallbackInterfaces;

	private readonly InputAction m_InGame_Minimap;

	private readonly InputAction m_InGame_MinimapEscape;

	private readonly InputAction m_InGame_ChangeViewTarget;

	private readonly InputAction m_InGame_ToggleKeyGuide;

	private readonly InputAction m_InGame_Chat;

	private readonly InputAction m_InGame_Menu;

	private readonly InputAction m_InGame_Inventory;

	private readonly InputActionMap m_InGameKeyboard;

	private List<IInGameKeyboardActions> m_InGameKeyboardActionsCallbackInterfaces;

	private readonly InputActionMap m_InGamePad;

	private List<IInGamePadActions> m_InGamePadActionsCallbackInterfaces;

	private readonly InputAction m_InGamePad_QuickSlotMoveLeft;

	private readonly InputAction m_InGamePad_QuickSlotMoveRight;

	private readonly InputAction m_InGamePad_QuickSlotUse;

	private readonly InputActionMap m_Ui;

	private List<IUiActions> m_UiActionsCallbackInterfaces;

	private readonly InputAction m_Ui_Toggle;

	private readonly InputAction m_Ui_Escape;

	private readonly InputAction m_Ui_Organize;

	private readonly InputAction m_Ui_Switch;

	private readonly InputAction m_Ui_NextFlow;

	private readonly InputAction m_Ui_Skip;

	private readonly InputAction m_Ui_MarkGoods;

	private readonly InputAction m_Ui_ToggleQuestHud;

	private readonly InputAction m_Ui_InventorySelectOne;

	private readonly InputAction m_Ui_ShopMaxQuantity;

	private readonly InputAction m_Ui_CloseMenu;

	private readonly InputAction m_Ui_UseItem;

	private readonly InputAction m_Ui_ShowDialog;

	private readonly InputAction m_Ui_ShowNote;

	private readonly InputAction m_Ui_TooltipMoveToCrafting;

	private readonly InputActionMap m_UiPad;

	private List<IUiPadActions> m_UiPadActionsCallbackInterfaces;

	private readonly InputAction m_UiPad_Submit;

	private readonly InputAction m_UiPad_QuickSubmit;

	private readonly InputAction m_UiPad_MovePanel;

	private readonly InputAction m_UiPad_ScrollRect;

	private readonly InputAction m_UiPad_MoveScroll;

	private readonly InputAction m_UiPad_TabLeft;

	private readonly InputAction m_UiPad_TabRight;

	private readonly InputAction m_UiPad_OptionLeft;

	private readonly InputAction m_UiPad_OptionRight;

	private readonly InputAction m_UiPad_DragAndDrop;

	private readonly InputAction m_UiPad_DropGoods;

	private readonly InputAction m_UiPad_InsertGoods;

	private readonly InputAction m_UiPad_Restore;

	private readonly InputAction m_UiPad_Delete;

	private readonly InputAction m_UiPad_MuteToggle;

	private readonly InputAction m_UiPad_IncreaseQuantityLarge;

	private readonly InputAction m_UiPad_DecreaseQuantityLarge;

	private readonly InputActionMap m_UiKeyboard;

	private List<IUiKeyboardActions> m_UiKeyboardActionsCallbackInterfaces;

	private readonly InputAction m_UiKeyboard_SwapWeapon;

	private readonly InputAction m_UiKeyboard_SetQuickSlot;

	private readonly InputAction m_UiKeyboard_InventorySelectHalf;

	private readonly InputAction m_UiKeyboard_Slider;

	private readonly InputAction m_UiKeyboard_InventoryMoveToStashAll;

	private readonly InputAction m_UiKeyboard_Confirm;

	private readonly InputAction m_UiKeyboard_Cancel;

	private readonly InputActionMap m_Cutscene;

	private List<ICutsceneActions> m_CutsceneActionsCallbackInterfaces;

	private readonly InputAction m_Cutscene_Skip;

	private int m_PCSchemeIndex;

	private int m_GamePadSchemeIndex;

	public InputActionAsset asset { get; }

	public InputBinding? bindingMask
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ReadOnlyArray<InputDevice>? devices
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ReadOnlyArray<InputControlScheme> controlSchemes => default;

	public IEnumerable<InputBinding> bindings => null;

	public PlayerActions Player => default;

	public PlayerFuncActions PlayerFunc => default;

	public InGameActions InGame => default;

	public InGameKeyboardActions InGameKeyboard => default;

	public InGamePadActions InGamePad => default;

	public UiActions Ui => default;

	public UiPadActions UiPad => default;

	public UiKeyboardActions UiKeyboard => default;

	public CutsceneActions Cutscene => default;

	public InputControlScheme PCScheme => default;

	public InputControlScheme GamePadScheme => default;

	public InputActions()
	{
	}

	~InputActions()
	{
	}

	public void Dispose()
	{
	}

	public bool Contains(InputAction action)
	{
		return false;
	}

	public IEnumerator<InputAction> GetEnumerator()
	{
		return null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return null;
	}

	public void Enable()
	{
	}

	public void Disable()
	{
	}

	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return null;
	}

	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		action = null;
		return 0;
	}

	public InputActions(InputActionAsset sharedAsset)
	{
	}
}
