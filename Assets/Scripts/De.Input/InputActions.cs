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

		public InputAction MoveKeyboard => m_Wrapper.m_Player_MoveKeyboard;

		public InputAction MovePad => m_Wrapper.m_Player_MovePad;

		public InputAction AimMouse => m_Wrapper.m_Player_AimMouse;

		public InputAction AimPad => m_Wrapper.m_Player_AimPad;

		public InputAction SwapPingEmojiPad => m_Wrapper.m_Player_SwapPingEmojiPad;

		public InputAction WheelSelect => m_Wrapper.m_Player_WheelSelect;

		public InputAction PingEmojiPad => m_Wrapper.m_Player_PingEmojiPad;

		public InputAction EmojiKeyboard => m_Wrapper.m_Player_EmojiKeyboard;

		public InputAction PingKeyboard => m_Wrapper.m_Player_PingKeyboard;

		public InputAction PointMinimap => m_Wrapper.m_Player_PointMinimap;

		public InputAction Escape => m_Wrapper.m_Player_Escape;

		public bool enabled => Get().enabled;

		public PlayerActions(InputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_Player;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(PlayerActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IPlayerActions instance)
		{
			if (instance != null && !m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
				MoveKeyboard.started += instance.OnMoveKeyboard;
				MoveKeyboard.performed += instance.OnMoveKeyboard;
				MoveKeyboard.canceled += instance.OnMoveKeyboard;
				MovePad.started += instance.OnMovePad;
				MovePad.performed += instance.OnMovePad;
				MovePad.canceled += instance.OnMovePad;
				AimMouse.started += instance.OnAimMouse;
				AimMouse.performed += instance.OnAimMouse;
				AimMouse.canceled += instance.OnAimMouse;
				AimPad.started += instance.OnAimPad;
				AimPad.performed += instance.OnAimPad;
				AimPad.canceled += instance.OnAimPad;
				SwapPingEmojiPad.started += instance.OnSwapPingEmojiPad;
				SwapPingEmojiPad.performed += instance.OnSwapPingEmojiPad;
				SwapPingEmojiPad.canceled += instance.OnSwapPingEmojiPad;
				WheelSelect.started += instance.OnWheelSelect;
				WheelSelect.performed += instance.OnWheelSelect;
				WheelSelect.canceled += instance.OnWheelSelect;
				PingEmojiPad.started += instance.OnPingEmojiPad;
				PingEmojiPad.performed += instance.OnPingEmojiPad;
				PingEmojiPad.canceled += instance.OnPingEmojiPad;
				EmojiKeyboard.started += instance.OnEmojiKeyboard;
				EmojiKeyboard.performed += instance.OnEmojiKeyboard;
				EmojiKeyboard.canceled += instance.OnEmojiKeyboard;
				PingKeyboard.started += instance.OnPingKeyboard;
				PingKeyboard.performed += instance.OnPingKeyboard;
				PingKeyboard.canceled += instance.OnPingKeyboard;
				PointMinimap.started += instance.OnPointMinimap;
				PointMinimap.performed += instance.OnPointMinimap;
				PointMinimap.canceled += instance.OnPointMinimap;
				Escape.started += instance.OnEscape;
				Escape.performed += instance.OnEscape;
				Escape.canceled += instance.OnEscape;
			}
		}

		private void UnregisterCallbacks(IPlayerActions instance)
		{
			MoveKeyboard.started -= instance.OnMoveKeyboard;
			MoveKeyboard.performed -= instance.OnMoveKeyboard;
			MoveKeyboard.canceled -= instance.OnMoveKeyboard;
			MovePad.started -= instance.OnMovePad;
			MovePad.performed -= instance.OnMovePad;
			MovePad.canceled -= instance.OnMovePad;
			AimMouse.started -= instance.OnAimMouse;
			AimMouse.performed -= instance.OnAimMouse;
			AimMouse.canceled -= instance.OnAimMouse;
			AimPad.started -= instance.OnAimPad;
			AimPad.performed -= instance.OnAimPad;
			AimPad.canceled -= instance.OnAimPad;
			SwapPingEmojiPad.started -= instance.OnSwapPingEmojiPad;
			SwapPingEmojiPad.performed -= instance.OnSwapPingEmojiPad;
			SwapPingEmojiPad.canceled -= instance.OnSwapPingEmojiPad;
			WheelSelect.started -= instance.OnWheelSelect;
			WheelSelect.performed -= instance.OnWheelSelect;
			WheelSelect.canceled -= instance.OnWheelSelect;
			PingEmojiPad.started -= instance.OnPingEmojiPad;
			PingEmojiPad.performed -= instance.OnPingEmojiPad;
			PingEmojiPad.canceled -= instance.OnPingEmojiPad;
			EmojiKeyboard.started -= instance.OnEmojiKeyboard;
			EmojiKeyboard.performed -= instance.OnEmojiKeyboard;
			EmojiKeyboard.canceled -= instance.OnEmojiKeyboard;
			PingKeyboard.started -= instance.OnPingKeyboard;
			PingKeyboard.performed -= instance.OnPingKeyboard;
			PingKeyboard.canceled -= instance.OnPingKeyboard;
			PointMinimap.started -= instance.OnPointMinimap;
			PointMinimap.performed -= instance.OnPointMinimap;
			PointMinimap.canceled -= instance.OnPointMinimap;
			Escape.started -= instance.OnEscape;
			Escape.performed -= instance.OnEscape;
			Escape.canceled -= instance.OnEscape;
		}

		public void RemoveCallbacks(IPlayerActions instance)
		{
			if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IPlayerActions instance)
		{
			foreach (IPlayerActions item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
			{
				UnregisterCallbacks(item);
			}
			m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct PlayerFuncActions
	{
		private InputActions m_Wrapper;

		public InputAction Run => m_Wrapper.m_PlayerFunc_Run;

		public InputAction Interact => m_Wrapper.m_PlayerFunc_Interact;

		public InputAction SwapWeapon => m_Wrapper.m_PlayerFunc_SwapWeapon;

		public InputAction UseItem => m_Wrapper.m_PlayerFunc_UseItem;

		public InputAction SkillBasicAttack => m_Wrapper.m_PlayerFunc_SkillBasicAttack;

		public InputAction SkillBasicAttackPadStick => m_Wrapper.m_PlayerFunc_SkillBasicAttackPadStick;

		public InputAction SkillOne => m_Wrapper.m_PlayerFunc_SkillOne;

		public InputAction SkillTwo => m_Wrapper.m_PlayerFunc_SkillTwo;

		public InputAction SkillThree => m_Wrapper.m_PlayerFunc_SkillThree;

		public InputAction SkillFour => m_Wrapper.m_PlayerFunc_SkillFour;

		public InputAction SkillEquipment => m_Wrapper.m_PlayerFunc_SkillEquipment;

		public InputAction SkillDash => m_Wrapper.m_PlayerFunc_SkillDash;

		public bool enabled => Get().enabled;

		public PlayerFuncActions(InputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_PlayerFunc;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(PlayerFuncActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IPlayerFuncActions instance)
		{
			if (instance != null && !m_Wrapper.m_PlayerFuncActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_PlayerFuncActionsCallbackInterfaces.Add(instance);
				Run.started += instance.OnRun;
				Run.performed += instance.OnRun;
				Run.canceled += instance.OnRun;
				Interact.started += instance.OnInteract;
				Interact.performed += instance.OnInteract;
				Interact.canceled += instance.OnInteract;
				SwapWeapon.started += instance.OnSwapWeapon;
				SwapWeapon.performed += instance.OnSwapWeapon;
				SwapWeapon.canceled += instance.OnSwapWeapon;
				UseItem.started += instance.OnUseItem;
				UseItem.performed += instance.OnUseItem;
				UseItem.canceled += instance.OnUseItem;
				SkillBasicAttack.started += instance.OnSkillBasicAttack;
				SkillBasicAttack.performed += instance.OnSkillBasicAttack;
				SkillBasicAttack.canceled += instance.OnSkillBasicAttack;
				SkillBasicAttackPadStick.started += instance.OnSkillBasicAttackPadStick;
				SkillBasicAttackPadStick.performed += instance.OnSkillBasicAttackPadStick;
				SkillBasicAttackPadStick.canceled += instance.OnSkillBasicAttackPadStick;
				SkillOne.started += instance.OnSkillOne;
				SkillOne.performed += instance.OnSkillOne;
				SkillOne.canceled += instance.OnSkillOne;
				SkillTwo.started += instance.OnSkillTwo;
				SkillTwo.performed += instance.OnSkillTwo;
				SkillTwo.canceled += instance.OnSkillTwo;
				SkillThree.started += instance.OnSkillThree;
				SkillThree.performed += instance.OnSkillThree;
				SkillThree.canceled += instance.OnSkillThree;
				SkillFour.started += instance.OnSkillFour;
				SkillFour.performed += instance.OnSkillFour;
				SkillFour.canceled += instance.OnSkillFour;
				SkillEquipment.started += instance.OnSkillEquipment;
				SkillEquipment.performed += instance.OnSkillEquipment;
				SkillEquipment.canceled += instance.OnSkillEquipment;
				SkillDash.started += instance.OnSkillDash;
				SkillDash.performed += instance.OnSkillDash;
				SkillDash.canceled += instance.OnSkillDash;
			}
		}

		private void UnregisterCallbacks(IPlayerFuncActions instance)
		{
			Run.started -= instance.OnRun;
			Run.performed -= instance.OnRun;
			Run.canceled -= instance.OnRun;
			Interact.started -= instance.OnInteract;
			Interact.performed -= instance.OnInteract;
			Interact.canceled -= instance.OnInteract;
			SwapWeapon.started -= instance.OnSwapWeapon;
			SwapWeapon.performed -= instance.OnSwapWeapon;
			SwapWeapon.canceled -= instance.OnSwapWeapon;
			UseItem.started -= instance.OnUseItem;
			UseItem.performed -= instance.OnUseItem;
			UseItem.canceled -= instance.OnUseItem;
			SkillBasicAttack.started -= instance.OnSkillBasicAttack;
			SkillBasicAttack.performed -= instance.OnSkillBasicAttack;
			SkillBasicAttack.canceled -= instance.OnSkillBasicAttack;
			SkillBasicAttackPadStick.started -= instance.OnSkillBasicAttackPadStick;
			SkillBasicAttackPadStick.performed -= instance.OnSkillBasicAttackPadStick;
			SkillBasicAttackPadStick.canceled -= instance.OnSkillBasicAttackPadStick;
			SkillOne.started -= instance.OnSkillOne;
			SkillOne.performed -= instance.OnSkillOne;
			SkillOne.canceled -= instance.OnSkillOne;
			SkillTwo.started -= instance.OnSkillTwo;
			SkillTwo.performed -= instance.OnSkillTwo;
			SkillTwo.canceled -= instance.OnSkillTwo;
			SkillThree.started -= instance.OnSkillThree;
			SkillThree.performed -= instance.OnSkillThree;
			SkillThree.canceled -= instance.OnSkillThree;
			SkillFour.started -= instance.OnSkillFour;
			SkillFour.performed -= instance.OnSkillFour;
			SkillFour.canceled -= instance.OnSkillFour;
			SkillEquipment.started -= instance.OnSkillEquipment;
			SkillEquipment.performed -= instance.OnSkillEquipment;
			SkillEquipment.canceled -= instance.OnSkillEquipment;
			SkillDash.started -= instance.OnSkillDash;
			SkillDash.performed -= instance.OnSkillDash;
			SkillDash.canceled -= instance.OnSkillDash;
		}

		public void RemoveCallbacks(IPlayerFuncActions instance)
		{
			if (m_Wrapper.m_PlayerFuncActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IPlayerFuncActions instance)
		{
			foreach (IPlayerFuncActions item in m_Wrapper.m_PlayerFuncActionsCallbackInterfaces)
			{
				UnregisterCallbacks(item);
			}
			m_Wrapper.m_PlayerFuncActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct InGameActions
	{
		private InputActions m_Wrapper;

		public InputAction Minimap => m_Wrapper.m_InGame_Minimap;

		public InputAction MinimapEscape => m_Wrapper.m_InGame_MinimapEscape;

		public InputAction ChangeViewTarget => m_Wrapper.m_InGame_ChangeViewTarget;

		public InputAction ToggleKeyGuide => m_Wrapper.m_InGame_ToggleKeyGuide;

		public InputAction Chat => m_Wrapper.m_InGame_Chat;

		public InputAction Menu => m_Wrapper.m_InGame_Menu;

		public InputAction Inventory => m_Wrapper.m_InGame_Inventory;

		public bool enabled => Get().enabled;

		public InGameActions(InputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_InGame;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(InGameActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IInGameActions instance)
		{
			if (instance != null && !m_Wrapper.m_InGameActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_InGameActionsCallbackInterfaces.Add(instance);
				Minimap.started += instance.OnMinimap;
				Minimap.performed += instance.OnMinimap;
				Minimap.canceled += instance.OnMinimap;
				MinimapEscape.started += instance.OnMinimapEscape;
				MinimapEscape.performed += instance.OnMinimapEscape;
				MinimapEscape.canceled += instance.OnMinimapEscape;
				ChangeViewTarget.started += instance.OnChangeViewTarget;
				ChangeViewTarget.performed += instance.OnChangeViewTarget;
				ChangeViewTarget.canceled += instance.OnChangeViewTarget;
				ToggleKeyGuide.started += instance.OnToggleKeyGuide;
				ToggleKeyGuide.performed += instance.OnToggleKeyGuide;
				ToggleKeyGuide.canceled += instance.OnToggleKeyGuide;
				Chat.started += instance.OnChat;
				Chat.performed += instance.OnChat;
				Chat.canceled += instance.OnChat;
				Menu.started += instance.OnMenu;
				Menu.performed += instance.OnMenu;
				Menu.canceled += instance.OnMenu;
				Inventory.started += instance.OnInventory;
				Inventory.performed += instance.OnInventory;
				Inventory.canceled += instance.OnInventory;
			}
		}

		private void UnregisterCallbacks(IInGameActions instance)
		{
			Minimap.started -= instance.OnMinimap;
			Minimap.performed -= instance.OnMinimap;
			Minimap.canceled -= instance.OnMinimap;
			MinimapEscape.started -= instance.OnMinimapEscape;
			MinimapEscape.performed -= instance.OnMinimapEscape;
			MinimapEscape.canceled -= instance.OnMinimapEscape;
			ChangeViewTarget.started -= instance.OnChangeViewTarget;
			ChangeViewTarget.performed -= instance.OnChangeViewTarget;
			ChangeViewTarget.canceled -= instance.OnChangeViewTarget;
			ToggleKeyGuide.started -= instance.OnToggleKeyGuide;
			ToggleKeyGuide.performed -= instance.OnToggleKeyGuide;
			ToggleKeyGuide.canceled -= instance.OnToggleKeyGuide;
			Chat.started -= instance.OnChat;
			Chat.performed -= instance.OnChat;
			Chat.canceled -= instance.OnChat;
			Menu.started -= instance.OnMenu;
			Menu.performed -= instance.OnMenu;
			Menu.canceled -= instance.OnMenu;
			Inventory.started -= instance.OnInventory;
			Inventory.performed -= instance.OnInventory;
			Inventory.canceled -= instance.OnInventory;
		}

		public void RemoveCallbacks(IInGameActions instance)
		{
			if (m_Wrapper.m_InGameActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IInGameActions instance)
		{
			foreach (IInGameActions item in m_Wrapper.m_InGameActionsCallbackInterfaces)
			{
				UnregisterCallbacks(item);
			}
			m_Wrapper.m_InGameActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct InGameKeyboardActions
	{
		private InputActions m_Wrapper;

		public bool enabled => Get().enabled;

		public InGameKeyboardActions(InputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_InGameKeyboard;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(InGameKeyboardActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IInGameKeyboardActions instance)
		{
			if (instance != null && !m_Wrapper.m_InGameKeyboardActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_InGameKeyboardActionsCallbackInterfaces.Add(instance);
			}
		}

		private void UnregisterCallbacks(IInGameKeyboardActions instance)
		{
		}

		public void RemoveCallbacks(IInGameKeyboardActions instance)
		{
			if (m_Wrapper.m_InGameKeyboardActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IInGameKeyboardActions instance)
		{
			foreach (IInGameKeyboardActions item in m_Wrapper.m_InGameKeyboardActionsCallbackInterfaces)
			{
				UnregisterCallbacks(item);
			}
			m_Wrapper.m_InGameKeyboardActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct InGamePadActions
	{
		private InputActions m_Wrapper;

		public InputAction QuickSlotMoveLeft => m_Wrapper.m_InGamePad_QuickSlotMoveLeft;

		public InputAction QuickSlotMoveRight => m_Wrapper.m_InGamePad_QuickSlotMoveRight;

		public InputAction QuickSlotUse => m_Wrapper.m_InGamePad_QuickSlotUse;

		public bool enabled => Get().enabled;

		public InGamePadActions(InputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_InGamePad;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(InGamePadActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IInGamePadActions instance)
		{
			if (instance != null && !m_Wrapper.m_InGamePadActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_InGamePadActionsCallbackInterfaces.Add(instance);
				QuickSlotMoveLeft.started += instance.OnQuickSlotMoveLeft;
				QuickSlotMoveLeft.performed += instance.OnQuickSlotMoveLeft;
				QuickSlotMoveLeft.canceled += instance.OnQuickSlotMoveLeft;
				QuickSlotMoveRight.started += instance.OnQuickSlotMoveRight;
				QuickSlotMoveRight.performed += instance.OnQuickSlotMoveRight;
				QuickSlotMoveRight.canceled += instance.OnQuickSlotMoveRight;
				QuickSlotUse.started += instance.OnQuickSlotUse;
				QuickSlotUse.performed += instance.OnQuickSlotUse;
				QuickSlotUse.canceled += instance.OnQuickSlotUse;
			}
		}

		private void UnregisterCallbacks(IInGamePadActions instance)
		{
			QuickSlotMoveLeft.started -= instance.OnQuickSlotMoveLeft;
			QuickSlotMoveLeft.performed -= instance.OnQuickSlotMoveLeft;
			QuickSlotMoveLeft.canceled -= instance.OnQuickSlotMoveLeft;
			QuickSlotMoveRight.started -= instance.OnQuickSlotMoveRight;
			QuickSlotMoveRight.performed -= instance.OnQuickSlotMoveRight;
			QuickSlotMoveRight.canceled -= instance.OnQuickSlotMoveRight;
			QuickSlotUse.started -= instance.OnQuickSlotUse;
			QuickSlotUse.performed -= instance.OnQuickSlotUse;
			QuickSlotUse.canceled -= instance.OnQuickSlotUse;
		}

		public void RemoveCallbacks(IInGamePadActions instance)
		{
			if (m_Wrapper.m_InGamePadActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IInGamePadActions instance)
		{
			foreach (IInGamePadActions item in m_Wrapper.m_InGamePadActionsCallbackInterfaces)
			{
				UnregisterCallbacks(item);
			}
			m_Wrapper.m_InGamePadActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct UiActions
	{
		private InputActions m_Wrapper;

		public InputAction Toggle => m_Wrapper.m_Ui_Toggle;

		public InputAction Escape => m_Wrapper.m_Ui_Escape;

		public InputAction Organize => m_Wrapper.m_Ui_Organize;

		public InputAction Switch => m_Wrapper.m_Ui_Switch;

		public InputAction NextFlow => m_Wrapper.m_Ui_NextFlow;

		public InputAction Skip => m_Wrapper.m_Ui_Skip;

		public InputAction MarkGoods => m_Wrapper.m_Ui_MarkGoods;

		public InputAction ToggleQuestHud => m_Wrapper.m_Ui_ToggleQuestHud;

		public InputAction InventorySelectOne => m_Wrapper.m_Ui_InventorySelectOne;

		public InputAction ShopMaxQuantity => m_Wrapper.m_Ui_ShopMaxQuantity;

		public InputAction CloseMenu => m_Wrapper.m_Ui_CloseMenu;

		public InputAction UseItem => m_Wrapper.m_Ui_UseItem;

		public InputAction ShowDialog => m_Wrapper.m_Ui_ShowDialog;

		public InputAction ShowNote => m_Wrapper.m_Ui_ShowNote;

		public InputAction TooltipMoveToCrafting => m_Wrapper.m_Ui_TooltipMoveToCrafting;

		public bool enabled => Get().enabled;

		public UiActions(InputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_Ui;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(UiActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IUiActions instance)
		{
			if (instance != null && !m_Wrapper.m_UiActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_UiActionsCallbackInterfaces.Add(instance);
				Toggle.started += instance.OnToggle;
				Toggle.performed += instance.OnToggle;
				Toggle.canceled += instance.OnToggle;
				Escape.started += instance.OnEscape;
				Escape.performed += instance.OnEscape;
				Escape.canceled += instance.OnEscape;
				Organize.started += instance.OnOrganize;
				Organize.performed += instance.OnOrganize;
				Organize.canceled += instance.OnOrganize;
				Switch.started += instance.OnSwitch;
				Switch.performed += instance.OnSwitch;
				Switch.canceled += instance.OnSwitch;
				NextFlow.started += instance.OnNextFlow;
				NextFlow.performed += instance.OnNextFlow;
				NextFlow.canceled += instance.OnNextFlow;
				Skip.started += instance.OnSkip;
				Skip.performed += instance.OnSkip;
				Skip.canceled += instance.OnSkip;
				MarkGoods.started += instance.OnMarkGoods;
				MarkGoods.performed += instance.OnMarkGoods;
				MarkGoods.canceled += instance.OnMarkGoods;
				ToggleQuestHud.started += instance.OnToggleQuestHud;
				ToggleQuestHud.performed += instance.OnToggleQuestHud;
				ToggleQuestHud.canceled += instance.OnToggleQuestHud;
				InventorySelectOne.started += instance.OnInventorySelectOne;
				InventorySelectOne.performed += instance.OnInventorySelectOne;
				InventorySelectOne.canceled += instance.OnInventorySelectOne;
				ShopMaxQuantity.started += instance.OnShopMaxQuantity;
				ShopMaxQuantity.performed += instance.OnShopMaxQuantity;
				ShopMaxQuantity.canceled += instance.OnShopMaxQuantity;
				CloseMenu.started += instance.OnCloseMenu;
				CloseMenu.performed += instance.OnCloseMenu;
				CloseMenu.canceled += instance.OnCloseMenu;
				UseItem.started += instance.OnUseItem;
				UseItem.performed += instance.OnUseItem;
				UseItem.canceled += instance.OnUseItem;
				ShowDialog.started += instance.OnShowDialog;
				ShowDialog.performed += instance.OnShowDialog;
				ShowDialog.canceled += instance.OnShowDialog;
				ShowNote.started += instance.OnShowNote;
				ShowNote.performed += instance.OnShowNote;
				ShowNote.canceled += instance.OnShowNote;
				TooltipMoveToCrafting.started += instance.OnTooltipMoveToCrafting;
				TooltipMoveToCrafting.performed += instance.OnTooltipMoveToCrafting;
				TooltipMoveToCrafting.canceled += instance.OnTooltipMoveToCrafting;
			}
		}

		private void UnregisterCallbacks(IUiActions instance)
		{
			Toggle.started -= instance.OnToggle;
			Toggle.performed -= instance.OnToggle;
			Toggle.canceled -= instance.OnToggle;
			Escape.started -= instance.OnEscape;
			Escape.performed -= instance.OnEscape;
			Escape.canceled -= instance.OnEscape;
			Organize.started -= instance.OnOrganize;
			Organize.performed -= instance.OnOrganize;
			Organize.canceled -= instance.OnOrganize;
			Switch.started -= instance.OnSwitch;
			Switch.performed -= instance.OnSwitch;
			Switch.canceled -= instance.OnSwitch;
			NextFlow.started -= instance.OnNextFlow;
			NextFlow.performed -= instance.OnNextFlow;
			NextFlow.canceled -= instance.OnNextFlow;
			Skip.started -= instance.OnSkip;
			Skip.performed -= instance.OnSkip;
			Skip.canceled -= instance.OnSkip;
			MarkGoods.started -= instance.OnMarkGoods;
			MarkGoods.performed -= instance.OnMarkGoods;
			MarkGoods.canceled -= instance.OnMarkGoods;
			ToggleQuestHud.started -= instance.OnToggleQuestHud;
			ToggleQuestHud.performed -= instance.OnToggleQuestHud;
			ToggleQuestHud.canceled -= instance.OnToggleQuestHud;
			InventorySelectOne.started -= instance.OnInventorySelectOne;
			InventorySelectOne.performed -= instance.OnInventorySelectOne;
			InventorySelectOne.canceled -= instance.OnInventorySelectOne;
			ShopMaxQuantity.started -= instance.OnShopMaxQuantity;
			ShopMaxQuantity.performed -= instance.OnShopMaxQuantity;
			ShopMaxQuantity.canceled -= instance.OnShopMaxQuantity;
			CloseMenu.started -= instance.OnCloseMenu;
			CloseMenu.performed -= instance.OnCloseMenu;
			CloseMenu.canceled -= instance.OnCloseMenu;
			UseItem.started -= instance.OnUseItem;
			UseItem.performed -= instance.OnUseItem;
			UseItem.canceled -= instance.OnUseItem;
			ShowDialog.started -= instance.OnShowDialog;
			ShowDialog.performed -= instance.OnShowDialog;
			ShowDialog.canceled -= instance.OnShowDialog;
			ShowNote.started -= instance.OnShowNote;
			ShowNote.performed -= instance.OnShowNote;
			ShowNote.canceled -= instance.OnShowNote;
			TooltipMoveToCrafting.started -= instance.OnTooltipMoveToCrafting;
			TooltipMoveToCrafting.performed -= instance.OnTooltipMoveToCrafting;
			TooltipMoveToCrafting.canceled -= instance.OnTooltipMoveToCrafting;
		}

		public void RemoveCallbacks(IUiActions instance)
		{
			if (m_Wrapper.m_UiActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IUiActions instance)
		{
			foreach (IUiActions item in m_Wrapper.m_UiActionsCallbackInterfaces)
			{
				UnregisterCallbacks(item);
			}
			m_Wrapper.m_UiActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct UiPadActions
	{
		private InputActions m_Wrapper;

		public InputAction Submit => m_Wrapper.m_UiPad_Submit;

		public InputAction QuickSubmit => m_Wrapper.m_UiPad_QuickSubmit;

		public InputAction MovePanel => m_Wrapper.m_UiPad_MovePanel;

		public InputAction ScrollRect => m_Wrapper.m_UiPad_ScrollRect;

		public InputAction MoveScroll => m_Wrapper.m_UiPad_MoveScroll;

		public InputAction TabLeft => m_Wrapper.m_UiPad_TabLeft;

		public InputAction TabRight => m_Wrapper.m_UiPad_TabRight;

		public InputAction OptionLeft => m_Wrapper.m_UiPad_OptionLeft;

		public InputAction OptionRight => m_Wrapper.m_UiPad_OptionRight;

		public InputAction DragAndDrop => m_Wrapper.m_UiPad_DragAndDrop;

		public InputAction DropGoods => m_Wrapper.m_UiPad_DropGoods;

		public InputAction InsertGoods => m_Wrapper.m_UiPad_InsertGoods;

		public InputAction Restore => m_Wrapper.m_UiPad_Restore;

		public InputAction Delete => m_Wrapper.m_UiPad_Delete;

		public InputAction MuteToggle => m_Wrapper.m_UiPad_MuteToggle;

		public InputAction IncreaseQuantityLarge => m_Wrapper.m_UiPad_IncreaseQuantityLarge;

		public InputAction DecreaseQuantityLarge => m_Wrapper.m_UiPad_DecreaseQuantityLarge;

		public bool enabled => Get().enabled;

		public UiPadActions(InputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_UiPad;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(UiPadActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IUiPadActions instance)
		{
			if (instance != null && !m_Wrapper.m_UiPadActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_UiPadActionsCallbackInterfaces.Add(instance);
				Submit.started += instance.OnSubmit;
				Submit.performed += instance.OnSubmit;
				Submit.canceled += instance.OnSubmit;
				QuickSubmit.started += instance.OnQuickSubmit;
				QuickSubmit.performed += instance.OnQuickSubmit;
				QuickSubmit.canceled += instance.OnQuickSubmit;
				MovePanel.started += instance.OnMovePanel;
				MovePanel.performed += instance.OnMovePanel;
				MovePanel.canceled += instance.OnMovePanel;
				ScrollRect.started += instance.OnScrollRect;
				ScrollRect.performed += instance.OnScrollRect;
				ScrollRect.canceled += instance.OnScrollRect;
				MoveScroll.started += instance.OnMoveScroll;
				MoveScroll.performed += instance.OnMoveScroll;
				MoveScroll.canceled += instance.OnMoveScroll;
				TabLeft.started += instance.OnTabLeft;
				TabLeft.performed += instance.OnTabLeft;
				TabLeft.canceled += instance.OnTabLeft;
				TabRight.started += instance.OnTabRight;
				TabRight.performed += instance.OnTabRight;
				TabRight.canceled += instance.OnTabRight;
				OptionLeft.started += instance.OnOptionLeft;
				OptionLeft.performed += instance.OnOptionLeft;
				OptionLeft.canceled += instance.OnOptionLeft;
				OptionRight.started += instance.OnOptionRight;
				OptionRight.performed += instance.OnOptionRight;
				OptionRight.canceled += instance.OnOptionRight;
				DragAndDrop.started += instance.OnDragAndDrop;
				DragAndDrop.performed += instance.OnDragAndDrop;
				DragAndDrop.canceled += instance.OnDragAndDrop;
				DropGoods.started += instance.OnDropGoods;
				DropGoods.performed += instance.OnDropGoods;
				DropGoods.canceled += instance.OnDropGoods;
				InsertGoods.started += instance.OnInsertGoods;
				InsertGoods.performed += instance.OnInsertGoods;
				InsertGoods.canceled += instance.OnInsertGoods;
				Restore.started += instance.OnRestore;
				Restore.performed += instance.OnRestore;
				Restore.canceled += instance.OnRestore;
				Delete.started += instance.OnDelete;
				Delete.performed += instance.OnDelete;
				Delete.canceled += instance.OnDelete;
				MuteToggle.started += instance.OnMuteToggle;
				MuteToggle.performed += instance.OnMuteToggle;
				MuteToggle.canceled += instance.OnMuteToggle;
				IncreaseQuantityLarge.started += instance.OnIncreaseQuantityLarge;
				IncreaseQuantityLarge.performed += instance.OnIncreaseQuantityLarge;
				IncreaseQuantityLarge.canceled += instance.OnIncreaseQuantityLarge;
				DecreaseQuantityLarge.started += instance.OnDecreaseQuantityLarge;
				DecreaseQuantityLarge.performed += instance.OnDecreaseQuantityLarge;
				DecreaseQuantityLarge.canceled += instance.OnDecreaseQuantityLarge;
			}
		}

		private void UnregisterCallbacks(IUiPadActions instance)
		{
			Submit.started -= instance.OnSubmit;
			Submit.performed -= instance.OnSubmit;
			Submit.canceled -= instance.OnSubmit;
			QuickSubmit.started -= instance.OnQuickSubmit;
			QuickSubmit.performed -= instance.OnQuickSubmit;
			QuickSubmit.canceled -= instance.OnQuickSubmit;
			MovePanel.started -= instance.OnMovePanel;
			MovePanel.performed -= instance.OnMovePanel;
			MovePanel.canceled -= instance.OnMovePanel;
			ScrollRect.started -= instance.OnScrollRect;
			ScrollRect.performed -= instance.OnScrollRect;
			ScrollRect.canceled -= instance.OnScrollRect;
			MoveScroll.started -= instance.OnMoveScroll;
			MoveScroll.performed -= instance.OnMoveScroll;
			MoveScroll.canceled -= instance.OnMoveScroll;
			TabLeft.started -= instance.OnTabLeft;
			TabLeft.performed -= instance.OnTabLeft;
			TabLeft.canceled -= instance.OnTabLeft;
			TabRight.started -= instance.OnTabRight;
			TabRight.performed -= instance.OnTabRight;
			TabRight.canceled -= instance.OnTabRight;
			OptionLeft.started -= instance.OnOptionLeft;
			OptionLeft.performed -= instance.OnOptionLeft;
			OptionLeft.canceled -= instance.OnOptionLeft;
			OptionRight.started -= instance.OnOptionRight;
			OptionRight.performed -= instance.OnOptionRight;
			OptionRight.canceled -= instance.OnOptionRight;
			DragAndDrop.started -= instance.OnDragAndDrop;
			DragAndDrop.performed -= instance.OnDragAndDrop;
			DragAndDrop.canceled -= instance.OnDragAndDrop;
			DropGoods.started -= instance.OnDropGoods;
			DropGoods.performed -= instance.OnDropGoods;
			DropGoods.canceled -= instance.OnDropGoods;
			InsertGoods.started -= instance.OnInsertGoods;
			InsertGoods.performed -= instance.OnInsertGoods;
			InsertGoods.canceled -= instance.OnInsertGoods;
			Restore.started -= instance.OnRestore;
			Restore.performed -= instance.OnRestore;
			Restore.canceled -= instance.OnRestore;
			Delete.started -= instance.OnDelete;
			Delete.performed -= instance.OnDelete;
			Delete.canceled -= instance.OnDelete;
			MuteToggle.started -= instance.OnMuteToggle;
			MuteToggle.performed -= instance.OnMuteToggle;
			MuteToggle.canceled -= instance.OnMuteToggle;
			IncreaseQuantityLarge.started -= instance.OnIncreaseQuantityLarge;
			IncreaseQuantityLarge.performed -= instance.OnIncreaseQuantityLarge;
			IncreaseQuantityLarge.canceled -= instance.OnIncreaseQuantityLarge;
			DecreaseQuantityLarge.started -= instance.OnDecreaseQuantityLarge;
			DecreaseQuantityLarge.performed -= instance.OnDecreaseQuantityLarge;
			DecreaseQuantityLarge.canceled -= instance.OnDecreaseQuantityLarge;
		}

		public void RemoveCallbacks(IUiPadActions instance)
		{
			if (m_Wrapper.m_UiPadActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IUiPadActions instance)
		{
			foreach (IUiPadActions item in m_Wrapper.m_UiPadActionsCallbackInterfaces)
			{
				UnregisterCallbacks(item);
			}
			m_Wrapper.m_UiPadActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct UiKeyboardActions
	{
		private InputActions m_Wrapper;

		public InputAction SwapWeapon => m_Wrapper.m_UiKeyboard_SwapWeapon;

		public InputAction SetQuickSlot => m_Wrapper.m_UiKeyboard_SetQuickSlot;

		public InputAction InventorySelectHalf => m_Wrapper.m_UiKeyboard_InventorySelectHalf;

		public InputAction Slider => m_Wrapper.m_UiKeyboard_Slider;

		public InputAction InventoryMoveToStashAll => m_Wrapper.m_UiKeyboard_InventoryMoveToStashAll;

		public InputAction Confirm => m_Wrapper.m_UiKeyboard_Confirm;

		public InputAction Cancel => m_Wrapper.m_UiKeyboard_Cancel;

		public bool enabled => Get().enabled;

		public UiKeyboardActions(InputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_UiKeyboard;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(UiKeyboardActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(IUiKeyboardActions instance)
		{
			if (instance != null && !m_Wrapper.m_UiKeyboardActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_UiKeyboardActionsCallbackInterfaces.Add(instance);
				SwapWeapon.started += instance.OnSwapWeapon;
				SwapWeapon.performed += instance.OnSwapWeapon;
				SwapWeapon.canceled += instance.OnSwapWeapon;
				SetQuickSlot.started += instance.OnSetQuickSlot;
				SetQuickSlot.performed += instance.OnSetQuickSlot;
				SetQuickSlot.canceled += instance.OnSetQuickSlot;
				InventorySelectHalf.started += instance.OnInventorySelectHalf;
				InventorySelectHalf.performed += instance.OnInventorySelectHalf;
				InventorySelectHalf.canceled += instance.OnInventorySelectHalf;
				Slider.started += instance.OnSlider;
				Slider.performed += instance.OnSlider;
				Slider.canceled += instance.OnSlider;
				InventoryMoveToStashAll.started += instance.OnInventoryMoveToStashAll;
				InventoryMoveToStashAll.performed += instance.OnInventoryMoveToStashAll;
				InventoryMoveToStashAll.canceled += instance.OnInventoryMoveToStashAll;
				Confirm.started += instance.OnConfirm;
				Confirm.performed += instance.OnConfirm;
				Confirm.canceled += instance.OnConfirm;
				Cancel.started += instance.OnCancel;
				Cancel.performed += instance.OnCancel;
				Cancel.canceled += instance.OnCancel;
			}
		}

		private void UnregisterCallbacks(IUiKeyboardActions instance)
		{
			SwapWeapon.started -= instance.OnSwapWeapon;
			SwapWeapon.performed -= instance.OnSwapWeapon;
			SwapWeapon.canceled -= instance.OnSwapWeapon;
			SetQuickSlot.started -= instance.OnSetQuickSlot;
			SetQuickSlot.performed -= instance.OnSetQuickSlot;
			SetQuickSlot.canceled -= instance.OnSetQuickSlot;
			InventorySelectHalf.started -= instance.OnInventorySelectHalf;
			InventorySelectHalf.performed -= instance.OnInventorySelectHalf;
			InventorySelectHalf.canceled -= instance.OnInventorySelectHalf;
			Slider.started -= instance.OnSlider;
			Slider.performed -= instance.OnSlider;
			Slider.canceled -= instance.OnSlider;
			InventoryMoveToStashAll.started -= instance.OnInventoryMoveToStashAll;
			InventoryMoveToStashAll.performed -= instance.OnInventoryMoveToStashAll;
			InventoryMoveToStashAll.canceled -= instance.OnInventoryMoveToStashAll;
			Confirm.started -= instance.OnConfirm;
			Confirm.performed -= instance.OnConfirm;
			Confirm.canceled -= instance.OnConfirm;
			Cancel.started -= instance.OnCancel;
			Cancel.performed -= instance.OnCancel;
			Cancel.canceled -= instance.OnCancel;
		}

		public void RemoveCallbacks(IUiKeyboardActions instance)
		{
			if (m_Wrapper.m_UiKeyboardActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(IUiKeyboardActions instance)
		{
			foreach (IUiKeyboardActions item in m_Wrapper.m_UiKeyboardActionsCallbackInterfaces)
			{
				UnregisterCallbacks(item);
			}
			m_Wrapper.m_UiKeyboardActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
		}
	}

	public struct CutsceneActions
	{
		private InputActions m_Wrapper;

		public InputAction Skip => m_Wrapper.m_Cutscene_Skip;

		public bool enabled => Get().enabled;

		public CutsceneActions(InputActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_Cutscene;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(CutsceneActions set)
		{
			return set.Get();
		}

		public void AddCallbacks(ICutsceneActions instance)
		{
			if (instance != null && !m_Wrapper.m_CutsceneActionsCallbackInterfaces.Contains(instance))
			{
				m_Wrapper.m_CutsceneActionsCallbackInterfaces.Add(instance);
				Skip.started += instance.OnSkip;
				Skip.performed += instance.OnSkip;
				Skip.canceled += instance.OnSkip;
			}
		}

		private void UnregisterCallbacks(ICutsceneActions instance)
		{
			Skip.started -= instance.OnSkip;
			Skip.performed -= instance.OnSkip;
			Skip.canceled -= instance.OnSkip;
		}

		public void RemoveCallbacks(ICutsceneActions instance)
		{
			if (m_Wrapper.m_CutsceneActionsCallbackInterfaces.Remove(instance))
			{
				UnregisterCallbacks(instance);
			}
		}

		public void SetCallbacks(ICutsceneActions instance)
		{
			foreach (ICutsceneActions item in m_Wrapper.m_CutsceneActionsCallbackInterfaces)
			{
				UnregisterCallbacks(item);
			}
			m_Wrapper.m_CutsceneActionsCallbackInterfaces.Clear();
			AddCallbacks(instance);
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

	private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();

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

	private List<IPlayerFuncActions> m_PlayerFuncActionsCallbackInterfaces = new List<IPlayerFuncActions>();

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

	private List<IInGameActions> m_InGameActionsCallbackInterfaces = new List<IInGameActions>();

	private readonly InputAction m_InGame_Minimap;

	private readonly InputAction m_InGame_MinimapEscape;

	private readonly InputAction m_InGame_ChangeViewTarget;

	private readonly InputAction m_InGame_ToggleKeyGuide;

	private readonly InputAction m_InGame_Chat;

	private readonly InputAction m_InGame_Menu;

	private readonly InputAction m_InGame_Inventory;

	private readonly InputActionMap m_InGameKeyboard;

	private List<IInGameKeyboardActions> m_InGameKeyboardActionsCallbackInterfaces = new List<IInGameKeyboardActions>();

	private readonly InputActionMap m_InGamePad;

	private List<IInGamePadActions> m_InGamePadActionsCallbackInterfaces = new List<IInGamePadActions>();

	private readonly InputAction m_InGamePad_QuickSlotMoveLeft;

	private readonly InputAction m_InGamePad_QuickSlotMoveRight;

	private readonly InputAction m_InGamePad_QuickSlotUse;

	private readonly InputActionMap m_Ui;

	private List<IUiActions> m_UiActionsCallbackInterfaces = new List<IUiActions>();

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

	private List<IUiPadActions> m_UiPadActionsCallbackInterfaces = new List<IUiPadActions>();

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

	private List<IUiKeyboardActions> m_UiKeyboardActionsCallbackInterfaces = new List<IUiKeyboardActions>();

	private readonly InputAction m_UiKeyboard_SwapWeapon;

	private readonly InputAction m_UiKeyboard_SetQuickSlot;

	private readonly InputAction m_UiKeyboard_InventorySelectHalf;

	private readonly InputAction m_UiKeyboard_Slider;

	private readonly InputAction m_UiKeyboard_InventoryMoveToStashAll;

	private readonly InputAction m_UiKeyboard_Confirm;

	private readonly InputAction m_UiKeyboard_Cancel;

	private readonly InputActionMap m_Cutscene;

	private List<ICutsceneActions> m_CutsceneActionsCallbackInterfaces = new List<ICutsceneActions>();

	private readonly InputAction m_Cutscene_Skip;

	private int m_PCSchemeIndex = -1;

	private int m_GamePadSchemeIndex = -1;

	public InputActionAsset asset { get; }

	public InputBinding? bindingMask
	{
		get
		{
			return asset.bindingMask;
		}
		set
		{
			asset.bindingMask = value;
		}
	}

	public ReadOnlyArray<InputDevice>? devices
	{
		get
		{
			return asset.devices;
		}
		set
		{
			asset.devices = value;
		}
	}

	public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

	public IEnumerable<InputBinding> bindings => asset.bindings;

	public PlayerActions Player => new PlayerActions(this);

	public PlayerFuncActions PlayerFunc => new PlayerFuncActions(this);

	public InGameActions InGame => new InGameActions(this);

	public InGameKeyboardActions InGameKeyboard => new InGameKeyboardActions(this);

	public InGamePadActions InGamePad => new InGamePadActions(this);

	public UiActions Ui => new UiActions(this);

	public UiPadActions UiPad => new UiPadActions(this);

	public UiKeyboardActions UiKeyboard => new UiKeyboardActions(this);

	public CutsceneActions Cutscene => new CutsceneActions(this);

	public InputControlScheme PCScheme
	{
		get
		{
			if (m_PCSchemeIndex == -1)
			{
				m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
			}
			return asset.controlSchemes[m_PCSchemeIndex];
		}
	}

	public InputControlScheme GamePadScheme
	{
		get
		{
			if (m_GamePadSchemeIndex == -1)
			{
				m_GamePadSchemeIndex = asset.FindControlSchemeIndex("GamePad");
			}
			return asset.controlSchemes[m_GamePadSchemeIndex];
		}
	}

	public InputActions()
	{
		asset = InputActionAsset.FromJson(@"{
    ""version"": 1,
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""0d149e0b-67bd-4657-afc6-c5141db8dd67"",
            ""actions"": [
                {
                    ""name"": ""MoveKeyboard"",
                    ""type"": ""PassThrough"",
                    ""id"": ""57a2cc14-9872-483e-856e-790808249c86"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MovePad"",
                    ""type"": ""Value"",
                    ""id"": ""7bec82e4-752d-4351-950f-dd46342cdf62"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""AimMouse"",
                    ""type"": ""PassThrough"",
                    ""id"": ""db9a9f6e-38ba-445a-ba29-2996709848b8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AimPad"",
                    ""type"": ""Value"",
                    ""id"": ""827184cd-f5d2-40f7-a8cb-a660b2d87811"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SwapPingEmojiPad"",
                    ""type"": ""Button"",
                    ""id"": ""c2a8a112-ddfd-4193-865f-14aa9dcc3537"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WheelSelect"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e4dd7565-52b8-4d45-8c5d-470a8e82b56a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PingEmojiPad"",
                    ""type"": ""Button"",
                    ""id"": ""6388b0a3-e784-4ca8-99ab-26285365e439"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EmojiKeyboard"",
                    ""type"": ""Button"",
                    ""id"": ""2e170ebc-0826-4d3c-bcad-46e900c49639"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PingKeyboard"",
                    ""type"": ""Button"",
                    ""id"": ""c989a137-9c9b-4bfe-9806-fda2e4218996"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PointMinimap"",
                    ""type"": ""Button"",
                    ""id"": ""602ba3d4-47cb-453e-a3c9-7ab5f6476085"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""3fc4b622-12ac-44bc-8500-ebbc7bd6b4ca"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1fa10d2d-25d4-40a3-b7d3-ccc077e1f903"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapPingEmojiPad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""b5420fec-347b-408f-a2ec-3443c365b25e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveKeyboard"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c7e60da1-a6cc-4e4f-965a-5a0774535ac2"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""MoveKeyboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""cfd04126-eb3a-47f6-a9b2-972326635060"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""MoveKeyboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b3f87575-51ac-44c6-91eb-22879de238bc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""MoveKeyboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""25e3753b-436b-4da7-8b0d-4ed145e1fd89"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""MoveKeyboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""18fc3818-766d-40a8-9d92-c1b10ca99b76"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""AimPad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5519293-17d0-4bf3-8e74-31307fb316e4"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""AimMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96b6acd7-d1a8-44ba-9550-fe8b118813ac"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""MovePad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0da34b2-ae9f-4c3a-92bb-193415487eeb"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""638eb5e9-76a1-4388-a3d9-83edaf6e591e"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c02a1669-3439-4484-bf06-92d414387c35"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""WheelSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d510502-d4c4-4e40-8483-64cce05a102b"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""PingEmojiPad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5243c152-42f1-49ca-ba81-c007b4239159"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""EmojiKeyboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0291190-b7c2-4add-aa46-fc209c81f4aa"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""PingKeyboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34d5a2a9-dc50-48e5-a8e4-771884398491"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""PointMinimap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45de6b19-e625-44ab-8104-aa2be3e2520b"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""PointMinimap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerFunc"",
            ""id"": ""daa500a5-058f-4caf-b2fd-de42d2ec109b"",
            ""actions"": [
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""76c34ad0-b9fa-40a7-ba67-a49fa2b52670"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""766e1b2f-3970-42ae-9093-b939a70b9d8e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwapWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""164d9977-9b36-4a99-87b7-ea2e70d5263e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UseItem"",
                    ""type"": ""Button"",
                    ""id"": ""7b270369-01f4-4711-af4f-9e6884a6833f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SkillBasicAttack"",
                    ""type"": ""Button"",
                    ""id"": ""eb18fea7-cde7-491e-b39d-765febd3bf66"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SkillBasicAttackPadStick"",
                    ""type"": ""Value"",
                    ""id"": ""8a5d8d9a-acd4-41f1-bf04-b3f4f8b7495e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SkillOne"",
                    ""type"": ""Button"",
                    ""id"": ""9a9df329-4463-4813-a647-993cc032b130"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SkillTwo"",
                    ""type"": ""Button"",
                    ""id"": ""487e605f-4735-48aa-8e15-375c1eee0dd9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SkillThree"",
                    ""type"": ""Button"",
                    ""id"": ""1e8cdf2f-4931-4b06-9915-82d0f95315fa"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SkillFour"",
                    ""type"": ""Button"",
                    ""id"": ""e2a1b035-dfce-40fa-b19a-0b7a35bb9f4c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SkillEquipment"",
                    ""type"": ""Button"",
                    ""id"": ""f42a33bc-baed-4f6a-b299-3cf8c00465a5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SkillDash"",
                    ""type"": ""Button"",
                    ""id"": ""dc482456-f470-45b9-9d28-dd49265393d0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""dd3c12f7-4c81-4e0c-8aec-4e1a86a1dc3d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa000001-0000-4000-8000-000000000003"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6a750e9-b355-47f2-91c9-0777e19f9997"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b62086d4-14b5-42cf-8831-45d210721c27"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""200266cb-e7cb-4d23-b2a6-a4fedd514e86"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""SwapWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87400c45-b75e-4726-ac97-4f90e5d1236d"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e2c4247-1b87-4418-b83a-4e8e8d1d9ec4"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d19c2953-c896-4779-92b7-26ad15f61939"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0e8eee1-7af6-431f-89af-4e23fe0743de"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""285b14a1-1d67-4987-b0cc-d64e124516fc"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6fbce961-b7b4-4b3e-a135-3d1cb901e9be"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SkillBasicAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa000001-0000-4000-8000-000000000007"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""SkillBasicAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3895ea79-6b16-410b-95c2-c1fa05835323"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SkillOne"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa000001-0000-4000-8000-000000000009"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""SkillOne"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f3850eac-4074-4518-b198-ae5e8e0c68fe"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SkillTwo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa000001-0000-4000-8000-000000000010"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""SkillTwo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d62e1b1f-52ec-4eb6-a3a5-b9127983b5d9"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SkillThree"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa000001-0000-4000-8000-000000000011"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""SkillThree"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83ed7021-b4e7-4087-8755-bee4a895b313"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SkillFour"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa000001-0000-4000-8000-000000000012"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""SkillFour"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea081d63-173b-41a9-937b-3b87d1a88e4a"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SkillEquipment"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c5b62c05-9449-47f3-a051-fb083b2d932a"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""SkillEquipment"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae8630c9-dae9-4e1d-a9b9-d7902394c91f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SkillDash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aac823d5-4692-4734-a318-423131befd0c"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""SkillDash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b6782de-9989-4adb-9420-2246d1296310"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.95,max=1)"",
                    ""groups"": "";GamePad"",
                    ""action"": ""SkillBasicAttackPadStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""InGame"",
            ""id"": ""e09ad9e1-67ea-4b1e-9881-c210ff534ae6"",
            ""actions"": [
                {
                    ""name"": ""Minimap"",
                    ""type"": ""Button"",
                    ""id"": ""1fea5f24-1023-43ee-b37e-744100869f0b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MinimapEscape"",
                    ""type"": ""Button"",
                    ""id"": ""9ef95164-ebd0-4365-b466-813745557db1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeViewTarget"",
                    ""type"": ""Button"",
                    ""id"": ""38bfaac6-9b64-4d51-af15-7e188c160ac5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleKeyGuide"",
                    ""type"": ""Button"",
                    ""id"": ""ce0adf9c-309d-4541-a1a7-e6ca5b282a6a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Chat"",
                    ""type"": ""Button"",
                    ""id"": ""f5f735a9-14d3-445b-b75c-6ffea44fdd2d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""ab75f2f1-9e31-4ba4-b4d1-7a8f7400caa3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""e3d19456-cebe-418e-82c2-7923903d8ffb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c61c4c9d-f70b-48fa-9e38-7ca7bd2a4deb"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Minimap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa000001-0000-4000-8000-000000000026"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Minimap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb789917-6c60-4ee7-ae60-5d8c38f46c0a"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""ChangeViewTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa000001-0000-4000-8000-000000000027"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""ChangeViewTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7c08335-8a88-4efc-a836-02b15a9921a6"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""ToggleKeyGuide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3f34ff8-fc3c-4852-9b4e-ab16adbfe380"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Chat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""587818d4-6406-475e-87d4-26e70554442f"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1958781-8dd6-4015-992b-01c18642a95f"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dbd95cb8-dd60-4e2a-a98d-c141fcb52ca2"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""MinimapEscape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ec67148-ee3a-4196-905a-efcf743b977c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""MinimapEscape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b6f42ed-3abb-4603-8e8d-b52f595b2dbc"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1827b0e-77b7-419c-9f55-017f9f520ac1"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""InGameKeyboard"",
            ""id"": ""09918070-fd01-46f2-8b64-a72f4a916127"",
            ""actions"": [],
            ""bindings"": []
        },
        {
            ""name"": ""InGamePad"",
            ""id"": ""f158c924-ff85-48dd-882f-2da3d498af9d"",
            ""actions"": [
                {
                    ""name"": ""QuickSlotMoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""44fb4a27-0393-4e88-b328-57b93c2046e9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""QuickSlotMoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""150a2790-e2ce-459a-8710-a2ec1b367f44"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""QuickSlotUse"",
                    ""type"": ""Button"",
                    ""id"": ""0f890f0a-a4b7-4dbc-bc06-746548b09f4b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2786b25e-49dc-4ad0-bcfa-ea60c7a18b0d"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""QuickSlotUse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""444e3b4c-a889-4f06-9e46-191459a469e1"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""QuickSlotMoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e863a003-dc01-4d34-95ac-ddb846f7ba0f"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""QuickSlotMoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Ui"",
            ""id"": ""06e2a173-044c-4e0b-829d-393b235e8ec7"",
            ""actions"": [
                {
                    ""name"": ""Toggle"",
                    ""type"": ""Button"",
                    ""id"": ""99cfb70b-3515-4610-80db-4299e588c87a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""474a473e-8998-48b2-80cd-a0f0aa542b65"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Organize"",
                    ""type"": ""Button"",
                    ""id"": ""42b890b8-2482-45eb-9f39-a9f0656c4a2f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Switch"",
                    ""type"": ""Button"",
                    ""id"": ""5e844679-91f1-48c3-9967-6bc8761502bf"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""NextFlow"",
                    ""type"": ""Button"",
                    ""id"": ""df1f5065-136a-47e2-82ac-b824a64d34ff"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skip"",
                    ""type"": ""Button"",
                    ""id"": ""598c949d-7b89-4bd3-b436-341270d11af7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MarkGoods"",
                    ""type"": ""Button"",
                    ""id"": ""ed2807b5-1c97-496d-9e86-b30b1de88ca2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleQuestHud"",
                    ""type"": ""Button"",
                    ""id"": ""23b0b2f3-d53b-45b8-9144-9c55081b77c6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InventorySelectOne"",
                    ""type"": ""Button"",
                    ""id"": ""a1b2c3d4-1111-2222-3333-aabbccddeeff"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ShopMaxQuantity"",
                    ""type"": ""Button"",
                    ""id"": ""b2c3d4e5-1111-2222-3333-aabbccddeef0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CloseMenu"",
                    ""type"": ""Button"",
                    ""id"": ""40e1e15c-232b-44ce-99e7-c5717a4cf1e6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UseItem"",
                    ""type"": ""Button"",
                    ""id"": ""6eaffd8f-000e-4d8a-a829-aab9780ff0c4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ShowDialog"",
                    ""type"": ""Button"",
                    ""id"": ""d211f645-b5d6-4d54-8553-338911842e2b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ShowNote"",
                    ""type"": ""Button"",
                    ""id"": ""4bcbc861-ec34-48d7-a306-013184a310f5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TooltipMoveToCrafting"",
                    ""type"": ""Button"",
                    ""id"": ""48ff6c09-dd74-4d50-9cc4-8af31e6f7463"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""097e7abb-0e85-4019-8fc9-1fb20299b69a"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""MarkGoods"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa000001-0000-4000-8000-000000000030"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""MarkGoods"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""667c447c-a089-4b20-8110-42fe26930d36"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Toggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""536fa91f-6461-49c3-a645-d62c51e55bb5"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Toggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ce97c2e-9c49-4ec3-bcbe-8c97d371fe06"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b87c52d8-7d49-46d6-b40a-9855130aabc3"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""075e8072-56ed-4c7c-b9a2-175309ca562d"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""045224c2-637a-4f53-ba3d-4a2711e2522f"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Organize"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35dcc7f6-ab9d-4589-a9b0-31d28312d283"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Organize"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1a4e29ae-07f9-47de-997c-9b6378f441fe"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Switch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e673190-713d-4e46-b401-e454cad42677"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Switch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""566c59d7-8dde-4d24-a403-eac22c1204ff"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""NextFlow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""296cc33d-462d-40ee-adc5-018960ae7c25"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""NextFlow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abd7a4c6-1558-49f8-a029-dbadcf746e94"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Skip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66e8ebbd-486a-445b-837d-18b8f90d75f1"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Skip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41e445e6-e4bb-4c60-9c0c-f9bcb7b50fe3"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Skip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d495172-b52b-4cca-845e-d5ca882d5b14"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""707a699f-f373-4f52-86be-aea992f69fbb"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""67801497-6bfe-48a6-90b1-dff2983e0462"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""ShowDialog"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00ec1fb6-e7da-4dca-9e84-b7aa0ca8c164"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""ShowDialog"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd1f4b54-7f2b-42c6-8ee3-cc6c35001fdc"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""ShowNote"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d601098-3846-495a-bd4e-2eb408dcebae"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""ShowNote"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f686ef76-53d1-4b84-8808-71d5812567d7"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""ToggleQuestHud"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fbd91bb-a337-4873-abda-84848f887018"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""ToggleQuestHud"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3d4e5f6-1111-2222-3333-aabbccddeef1"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""InventorySelectOne"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d4e5f6a7-1111-2222-3333-aabbccddeef2"",
                    ""path"": ""<Keyboard>/rightCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""InventorySelectOne"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa000001-0000-4000-8000-000000000031"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""InventorySelectOne"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5f6a7b8-1111-2222-3333-aabbccddeef3"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""ShopMaxQuantity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6a7b8c9-1111-2222-3333-aabbccddeef4"",
                    ""path"": ""<Keyboard>/rightCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""ShopMaxQuantity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa000001-0000-4000-8000-000000000032"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""ShopMaxQuantity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""729c0d9a-9052-4ddb-b253-6b5b3cd3cd54"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""CloseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3dc0b1d3-1530-4756-a2b0-2f455bef6df4"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""CloseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91113144-325b-4253-b203-12d54fd0a9f2"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TooltipMoveToCrafting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fc2c3cb-4500-4e68-af77-d907dcbb835b"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TooltipMoveToCrafting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UiPad"",
            ""id"": ""7662cba8-54e6-4e71-9d93-c9f5ae283e0d"",
            ""actions"": [
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""731562a1-843d-46b1-a36e-a06cb73177dc"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""QuickSubmit"",
                    ""type"": ""Button"",
                    ""id"": ""0be6eda3-c20f-4022-b0f2-de3939802ef8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MovePanel"",
                    ""type"": ""Value"",
                    ""id"": ""a6ae8c27-377e-41d9-9ce5-38073b7c4dd6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ScrollRect"",
                    ""type"": ""Value"",
                    ""id"": ""12db549b-b1c2-418c-99fe-0c05a01ad82a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveScroll"",
                    ""type"": ""Value"",
                    ""id"": ""c7ef7c71-97b1-4c54-b2a8-5467b0cee26a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""TabLeft"",
                    ""type"": ""Button"",
                    ""id"": ""daceda5b-c150-485d-9a7e-0d16a5fd5dce"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TabRight"",
                    ""type"": ""Button"",
                    ""id"": ""964870df-8c7b-4468-a95c-ff6d8753befd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OptionLeft"",
                    ""type"": ""Button"",
                    ""id"": ""a1b2c3d4-e5f6-7890-abcd-ef1234567891"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OptionRight"",
                    ""type"": ""Button"",
                    ""id"": ""b2c3d4e5-f6a7-8901-bcde-f12345678902"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DragAndDrop"",
                    ""type"": ""Button"",
                    ""id"": ""eb5244c3-fd46-4e6e-a50f-bedd64e9a486"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DropGoods"",
                    ""type"": ""Button"",
                    ""id"": ""e19d06a5-4ea6-4bc5-9a6a-ddf65e900643"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InsertGoods"",
                    ""type"": ""Button"",
                    ""id"": ""4c7b5730-c09b-4676-9a37-20545b7a7d04"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Restore"",
                    ""type"": ""Button"",
                    ""id"": ""dd3e2ead-5c23-4479-9b36-ff58c05bdfac"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Delete"",
                    ""type"": ""Button"",
                    ""id"": ""3c5721b9-7ad3-40b7-8b01-fee8985413fd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MuteToggle"",
                    ""type"": ""Button"",
                    ""id"": ""5f79f455-c044-4799-a062-68281482235c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""IncreaseQuantityLarge"",
                    ""type"": ""Button"",
                    ""id"": ""5fb894d5-8923-4e33-b6ee-e2d1241328b1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DecreaseQuantityLarge"",
                    ""type"": ""Button"",
                    ""id"": ""f964ddcf-6e1d-4b63-bcfd-fbd2d4543a07"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""06f5d85e-71fc-4195-9b4e-3f59d6809f2a"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb5a248c-ae3a-4820-b2fc-7bfe23fc9106"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""Restore"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a80c472-b9cb-4279-8776-d1e7f7ea0890"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""DragAndDrop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a071885-dc46-410e-a154-e2570b8f24f0"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""MoveScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09746d6a-3a25-4c18-b1df-f5923de03403"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.25,max=0.925)"",
                    ""groups"": "";GamePad"",
                    ""action"": ""MoveScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3d4e5f6-a7b8-9012-cdef-012345678903"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""OptionLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d4e5f6a7-b8c9-0123-def0-123456789004"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""OptionLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5f6a7b8-c9d0-1234-ef01-234567890005"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""OptionRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6a7b8c9-d0e1-2345-f012-345678900006"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""OptionRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c734a67-1771-4364-89cf-80b8d92fb4f4"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""TabLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad92c525-b451-4d49-9902-59950ce9c4b5"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""TabRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd915137-82f8-4f6d-a7af-bbda1a42b1fa"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.25,max=0.925)"",
                    ""groups"": "";GamePad"",
                    ""action"": ""MovePanel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8e9f4f8-2b56-49ae-b0fa-2f3b3e380fa0"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.25,max=0.925)"",
                    ""groups"": "";GamePad"",
                    ""action"": ""ScrollRect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a35c7688-babe-44b6-9778-06441b323bf7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f205ecc6-d93f-4a24-939b-85789ae4461a"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""DropGoods"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56ea6d68-6bcc-47f1-9a0d-f987a220d957"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""QuickSubmit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a14dce15-32c5-43f5-ae94-c6b927554b49"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""InsertGoods"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e14f5078-72d1-4269-b2d4-3198065819ae"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""MuteToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c03c61d5-282e-4288-8e57-4064a16ee983"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""IncreaseQuantityLarge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ef5abc6-2828-42e8-80f1-002139920183"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";GamePad"",
                    ""action"": ""DecreaseQuantityLarge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UiKeyboard"",
            ""id"": ""1529079f-ceb5-4293-8fe5-4d479b42ae81"",
            ""actions"": [
                {
                    ""name"": ""SwapWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""966aebc4-dcde-448a-a2d2-a97df5f00931"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SetQuickSlot"",
                    ""type"": ""Button"",
                    ""id"": ""a07045e8-2420-4830-a7fd-45d913709009"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InventorySelectHalf"",
                    ""type"": ""Button"",
                    ""id"": ""0881f409-092f-48a3-b1e0-de4979086519"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Slider"",
                    ""type"": ""Value"",
                    ""id"": ""63cb593d-d857-4043-989a-302dcc50e645"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InventoryMoveToStashAll"",
                    ""type"": ""Button"",
                    ""id"": ""5770878d-01ad-4be6-9b04-57222c6ed10c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""4acfb488-8bc8-48a8-b9e4-642135ef40fe"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""ed483649-d5e5-47a6-9ebf-fdd4c9a6bbe2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""36fa298d-cbee-48e3-b6bc-77eeacbeaeb5"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""SwapWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""882749a2-63ec-40a8-86b4-7f2a833d63e3"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InventoryMoveToStashAll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56d2df02-c7f3-4927-ad3a-477d7159f415"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SetQuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""934cfc2f-98f1-4665-b32f-58496da293c0"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SetQuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bbbdcce1-47a3-498c-a0ef-5e8d020c9665"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SetQuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00eee8bd-391e-4beb-9b1d-cd5a6bfddb26"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SetQuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7173a56-b11a-4c44-932f-9c6a6d79f6ea"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""SetQuickSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc9fa084-337c-429e-a07d-0004ec076b63"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InventorySelectHalf"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""610806e7-c312-4d63-b681-00ab3edd6be6"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slider"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7feb7b0-6731-46b9-a988-62c7e8cf7a15"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4baa05f3-6c35-4132-8a0d-e8cfb502cb99"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Cutscene"",
            ""id"": ""73229dc4-4390-401a-b8ec-d065fdff775d"",
            ""actions"": [
                {
                    ""name"": ""Skip"",
                    ""type"": ""Button"",
                    ""id"": ""ac309194-9127-46d6-803b-44cfcfe59bdc"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""68d434b5-538d-47dc-8f82-93a84ae513c3"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Skip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81f055d0-8d3c-4185-bf98-da637136f6ed"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Skip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": []
        },
        {
            ""name"": ""GamePad"",
            ""bindingGroup"": ""GamePad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
		m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
		m_Player_MoveKeyboard = m_Player.FindAction("MoveKeyboard", throwIfNotFound: true);
		m_Player_MovePad = m_Player.FindAction("MovePad", throwIfNotFound: true);
		m_Player_AimMouse = m_Player.FindAction("AimMouse", throwIfNotFound: true);
		m_Player_AimPad = m_Player.FindAction("AimPad", throwIfNotFound: true);
		m_Player_SwapPingEmojiPad = m_Player.FindAction("SwapPingEmojiPad", throwIfNotFound: true);
		m_Player_WheelSelect = m_Player.FindAction("WheelSelect", throwIfNotFound: true);
		m_Player_PingEmojiPad = m_Player.FindAction("PingEmojiPad", throwIfNotFound: true);
		m_Player_EmojiKeyboard = m_Player.FindAction("EmojiKeyboard", throwIfNotFound: true);
		m_Player_PingKeyboard = m_Player.FindAction("PingKeyboard", throwIfNotFound: true);
		m_Player_PointMinimap = m_Player.FindAction("PointMinimap", throwIfNotFound: true);
		m_Player_Escape = m_Player.FindAction("Escape", throwIfNotFound: true);
		m_PlayerFunc = asset.FindActionMap("PlayerFunc", throwIfNotFound: true);
		m_PlayerFunc_Run = m_PlayerFunc.FindAction("Run", throwIfNotFound: true);
		m_PlayerFunc_Interact = m_PlayerFunc.FindAction("Interact", throwIfNotFound: true);
		m_PlayerFunc_SwapWeapon = m_PlayerFunc.FindAction("SwapWeapon", throwIfNotFound: true);
		m_PlayerFunc_UseItem = m_PlayerFunc.FindAction("UseItem", throwIfNotFound: true);
		m_PlayerFunc_SkillBasicAttack = m_PlayerFunc.FindAction("SkillBasicAttack", throwIfNotFound: true);
		m_PlayerFunc_SkillBasicAttackPadStick = m_PlayerFunc.FindAction("SkillBasicAttackPadStick", throwIfNotFound: true);
		m_PlayerFunc_SkillOne = m_PlayerFunc.FindAction("SkillOne", throwIfNotFound: true);
		m_PlayerFunc_SkillTwo = m_PlayerFunc.FindAction("SkillTwo", throwIfNotFound: true);
		m_PlayerFunc_SkillThree = m_PlayerFunc.FindAction("SkillThree", throwIfNotFound: true);
		m_PlayerFunc_SkillFour = m_PlayerFunc.FindAction("SkillFour", throwIfNotFound: true);
		m_PlayerFunc_SkillEquipment = m_PlayerFunc.FindAction("SkillEquipment", throwIfNotFound: true);
		m_PlayerFunc_SkillDash = m_PlayerFunc.FindAction("SkillDash", throwIfNotFound: true);
		m_InGame = asset.FindActionMap("InGame", throwIfNotFound: true);
		m_InGame_Minimap = m_InGame.FindAction("Minimap", throwIfNotFound: true);
		m_InGame_MinimapEscape = m_InGame.FindAction("MinimapEscape", throwIfNotFound: true);
		m_InGame_ChangeViewTarget = m_InGame.FindAction("ChangeViewTarget", throwIfNotFound: true);
		m_InGame_ToggleKeyGuide = m_InGame.FindAction("ToggleKeyGuide", throwIfNotFound: true);
		m_InGame_Chat = m_InGame.FindAction("Chat", throwIfNotFound: true);
		m_InGame_Menu = m_InGame.FindAction("Menu", throwIfNotFound: true);
		m_InGame_Inventory = m_InGame.FindAction("Inventory", throwIfNotFound: true);
		m_InGameKeyboard = asset.FindActionMap("InGameKeyboard", throwIfNotFound: true);
		m_InGamePad = asset.FindActionMap("InGamePad", throwIfNotFound: true);
		m_InGamePad_QuickSlotMoveLeft = m_InGamePad.FindAction("QuickSlotMoveLeft", throwIfNotFound: true);
		m_InGamePad_QuickSlotMoveRight = m_InGamePad.FindAction("QuickSlotMoveRight", throwIfNotFound: true);
		m_InGamePad_QuickSlotUse = m_InGamePad.FindAction("QuickSlotUse", throwIfNotFound: true);
		m_Ui = asset.FindActionMap("Ui", throwIfNotFound: true);
		m_Ui_Toggle = m_Ui.FindAction("Toggle", throwIfNotFound: true);
		m_Ui_Escape = m_Ui.FindAction("Escape", throwIfNotFound: true);
		m_Ui_Organize = m_Ui.FindAction("Organize", throwIfNotFound: true);
		m_Ui_Switch = m_Ui.FindAction("Switch", throwIfNotFound: true);
		m_Ui_NextFlow = m_Ui.FindAction("NextFlow", throwIfNotFound: true);
		m_Ui_Skip = m_Ui.FindAction("Skip", throwIfNotFound: true);
		m_Ui_MarkGoods = m_Ui.FindAction("MarkGoods", throwIfNotFound: true);
		m_Ui_ToggleQuestHud = m_Ui.FindAction("ToggleQuestHud", throwIfNotFound: true);
		m_Ui_InventorySelectOne = m_Ui.FindAction("InventorySelectOne", throwIfNotFound: true);
		m_Ui_ShopMaxQuantity = m_Ui.FindAction("ShopMaxQuantity", throwIfNotFound: true);
		m_Ui_CloseMenu = m_Ui.FindAction("CloseMenu", throwIfNotFound: true);
		m_Ui_UseItem = m_Ui.FindAction("UseItem", throwIfNotFound: true);
		m_Ui_ShowDialog = m_Ui.FindAction("ShowDialog", throwIfNotFound: true);
		m_Ui_ShowNote = m_Ui.FindAction("ShowNote", throwIfNotFound: true);
		m_Ui_TooltipMoveToCrafting = m_Ui.FindAction("TooltipMoveToCrafting", throwIfNotFound: true);
		m_UiPad = asset.FindActionMap("UiPad", throwIfNotFound: true);
		m_UiPad_Submit = m_UiPad.FindAction("Submit", throwIfNotFound: true);
		m_UiPad_QuickSubmit = m_UiPad.FindAction("QuickSubmit", throwIfNotFound: true);
		m_UiPad_MovePanel = m_UiPad.FindAction("MovePanel", throwIfNotFound: true);
		m_UiPad_ScrollRect = m_UiPad.FindAction("ScrollRect", throwIfNotFound: true);
		m_UiPad_MoveScroll = m_UiPad.FindAction("MoveScroll", throwIfNotFound: true);
		m_UiPad_TabLeft = m_UiPad.FindAction("TabLeft", throwIfNotFound: true);
		m_UiPad_TabRight = m_UiPad.FindAction("TabRight", throwIfNotFound: true);
		m_UiPad_OptionLeft = m_UiPad.FindAction("OptionLeft", throwIfNotFound: true);
		m_UiPad_OptionRight = m_UiPad.FindAction("OptionRight", throwIfNotFound: true);
		m_UiPad_DragAndDrop = m_UiPad.FindAction("DragAndDrop", throwIfNotFound: true);
		m_UiPad_DropGoods = m_UiPad.FindAction("DropGoods", throwIfNotFound: true);
		m_UiPad_InsertGoods = m_UiPad.FindAction("InsertGoods", throwIfNotFound: true);
		m_UiPad_Restore = m_UiPad.FindAction("Restore", throwIfNotFound: true);
		m_UiPad_Delete = m_UiPad.FindAction("Delete", throwIfNotFound: true);
		m_UiPad_MuteToggle = m_UiPad.FindAction("MuteToggle", throwIfNotFound: true);
		m_UiPad_IncreaseQuantityLarge = m_UiPad.FindAction("IncreaseQuantityLarge", throwIfNotFound: true);
		m_UiPad_DecreaseQuantityLarge = m_UiPad.FindAction("DecreaseQuantityLarge", throwIfNotFound: true);
		m_UiKeyboard = asset.FindActionMap("UiKeyboard", throwIfNotFound: true);
		m_UiKeyboard_SwapWeapon = m_UiKeyboard.FindAction("SwapWeapon", throwIfNotFound: true);
		m_UiKeyboard_SetQuickSlot = m_UiKeyboard.FindAction("SetQuickSlot", throwIfNotFound: true);
		m_UiKeyboard_InventorySelectHalf = m_UiKeyboard.FindAction("InventorySelectHalf", throwIfNotFound: true);
		m_UiKeyboard_Slider = m_UiKeyboard.FindAction("Slider", throwIfNotFound: true);
		m_UiKeyboard_InventoryMoveToStashAll = m_UiKeyboard.FindAction("InventoryMoveToStashAll", throwIfNotFound: true);
		m_UiKeyboard_Confirm = m_UiKeyboard.FindAction("Confirm", throwIfNotFound: true);
		m_UiKeyboard_Cancel = m_UiKeyboard.FindAction("Cancel", throwIfNotFound: true);
		m_Cutscene = asset.FindActionMap("Cutscene", throwIfNotFound: true);
		m_Cutscene_Skip = m_Cutscene.FindAction("Skip", throwIfNotFound: true);
	}

	~InputActions()
	{
	}

	public void Dispose()
	{
		UnityEngine.Object.Destroy(asset);
	}

	public bool Contains(InputAction action)
	{
		return asset.Contains(action);
	}

	public IEnumerator<InputAction> GetEnumerator()
	{
		return asset.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Enable()
	{
		asset.Enable();
	}

	public void Disable()
	{
		asset.Disable();
	}

	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return asset.FindAction(actionNameOrId, throwIfNotFound);
	}

	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		return asset.FindBinding(bindingMask, out action);
	}

	public InputActions(InputActionAsset sharedAsset)
	{
		asset = sharedAsset;
		m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
		m_Player_MoveKeyboard = m_Player.FindAction("MoveKeyboard", throwIfNotFound: true);
		m_Player_MovePad = m_Player.FindAction("MovePad", throwIfNotFound: true);
		m_Player_AimMouse = m_Player.FindAction("AimMouse", throwIfNotFound: true);
		m_Player_AimPad = m_Player.FindAction("AimPad", throwIfNotFound: true);
		m_Player_SwapPingEmojiPad = m_Player.FindAction("SwapPingEmojiPad", throwIfNotFound: true);
		m_Player_WheelSelect = m_Player.FindAction("WheelSelect", throwIfNotFound: true);
		m_Player_PingEmojiPad = m_Player.FindAction("PingEmojiPad", throwIfNotFound: true);
		m_Player_EmojiKeyboard = m_Player.FindAction("EmojiKeyboard", throwIfNotFound: true);
		m_Player_PingKeyboard = m_Player.FindAction("PingKeyboard", throwIfNotFound: true);
		m_Player_PointMinimap = m_Player.FindAction("PointMinimap", throwIfNotFound: true);
		m_Player_Escape = m_Player.FindAction("Escape", throwIfNotFound: true);
		m_PlayerFunc = asset.FindActionMap("PlayerFunc", throwIfNotFound: true);
		m_PlayerFunc_Run = m_PlayerFunc.FindAction("Run", throwIfNotFound: true);
		m_PlayerFunc_Interact = m_PlayerFunc.FindAction("Interact", throwIfNotFound: true);
		m_PlayerFunc_SwapWeapon = m_PlayerFunc.FindAction("SwapWeapon", throwIfNotFound: true);
		m_PlayerFunc_UseItem = m_PlayerFunc.FindAction("UseItem", throwIfNotFound: true);
		m_PlayerFunc_SkillBasicAttack = m_PlayerFunc.FindAction("SkillBasicAttack", throwIfNotFound: true);
		m_PlayerFunc_SkillBasicAttackPadStick = m_PlayerFunc.FindAction("SkillBasicAttackPadStick", throwIfNotFound: true);
		m_PlayerFunc_SkillOne = m_PlayerFunc.FindAction("SkillOne", throwIfNotFound: true);
		m_PlayerFunc_SkillTwo = m_PlayerFunc.FindAction("SkillTwo", throwIfNotFound: true);
		m_PlayerFunc_SkillThree = m_PlayerFunc.FindAction("SkillThree", throwIfNotFound: true);
		m_PlayerFunc_SkillFour = m_PlayerFunc.FindAction("SkillFour", throwIfNotFound: true);
		m_PlayerFunc_SkillEquipment = m_PlayerFunc.FindAction("SkillEquipment", throwIfNotFound: true);
		m_PlayerFunc_SkillDash = m_PlayerFunc.FindAction("SkillDash", throwIfNotFound: true);
		m_InGame = asset.FindActionMap("InGame", throwIfNotFound: true);
		m_InGame_Minimap = m_InGame.FindAction("Minimap", throwIfNotFound: true);
		m_InGame_MinimapEscape = m_InGame.FindAction("MinimapEscape", throwIfNotFound: true);
		m_InGame_ChangeViewTarget = m_InGame.FindAction("ChangeViewTarget", throwIfNotFound: true);
		m_InGame_ToggleKeyGuide = m_InGame.FindAction("ToggleKeyGuide", throwIfNotFound: true);
		m_InGame_Chat = m_InGame.FindAction("Chat", throwIfNotFound: true);
		m_InGame_Menu = m_InGame.FindAction("Menu", throwIfNotFound: true);
		m_InGame_Inventory = m_InGame.FindAction("Inventory", throwIfNotFound: true);
		m_InGameKeyboard = asset.FindActionMap("InGameKeyboard", throwIfNotFound: true);
		m_InGamePad = asset.FindActionMap("InGamePad", throwIfNotFound: true);
		m_InGamePad_QuickSlotMoveLeft = m_InGamePad.FindAction("QuickSlotMoveLeft", throwIfNotFound: true);
		m_InGamePad_QuickSlotMoveRight = m_InGamePad.FindAction("QuickSlotMoveRight", throwIfNotFound: true);
		m_InGamePad_QuickSlotUse = m_InGamePad.FindAction("QuickSlotUse", throwIfNotFound: true);
		m_Ui = asset.FindActionMap("Ui", throwIfNotFound: true);
		m_Ui_Toggle = m_Ui.FindAction("Toggle", throwIfNotFound: true);
		m_Ui_Escape = m_Ui.FindAction("Escape", throwIfNotFound: true);
		m_Ui_Organize = m_Ui.FindAction("Organize", throwIfNotFound: true);
		m_Ui_Switch = m_Ui.FindAction("Switch", throwIfNotFound: true);
		m_Ui_NextFlow = m_Ui.FindAction("NextFlow", throwIfNotFound: true);
		m_Ui_Skip = m_Ui.FindAction("Skip", throwIfNotFound: true);
		m_Ui_MarkGoods = m_Ui.FindAction("MarkGoods", throwIfNotFound: true);
		m_Ui_ToggleQuestHud = m_Ui.FindAction("ToggleQuestHud", throwIfNotFound: true);
		m_Ui_InventorySelectOne = m_Ui.FindAction("InventorySelectOne", throwIfNotFound: true);
		m_Ui_ShopMaxQuantity = m_Ui.FindAction("ShopMaxQuantity", throwIfNotFound: true);
		m_Ui_CloseMenu = m_Ui.FindAction("CloseMenu", throwIfNotFound: true);
		m_Ui_UseItem = m_Ui.FindAction("UseItem", throwIfNotFound: true);
		m_Ui_ShowDialog = m_Ui.FindAction("ShowDialog", throwIfNotFound: true);
		m_Ui_ShowNote = m_Ui.FindAction("ShowNote", throwIfNotFound: true);
		m_Ui_TooltipMoveToCrafting = m_Ui.FindAction("TooltipMoveToCrafting", throwIfNotFound: true);
		m_UiPad = asset.FindActionMap("UiPad", throwIfNotFound: true);
		m_UiPad_Submit = m_UiPad.FindAction("Submit", throwIfNotFound: true);
		m_UiPad_QuickSubmit = m_UiPad.FindAction("QuickSubmit", throwIfNotFound: true);
		m_UiPad_MovePanel = m_UiPad.FindAction("MovePanel", throwIfNotFound: true);
		m_UiPad_ScrollRect = m_UiPad.FindAction("ScrollRect", throwIfNotFound: true);
		m_UiPad_MoveScroll = m_UiPad.FindAction("MoveScroll", throwIfNotFound: true);
		m_UiPad_TabLeft = m_UiPad.FindAction("TabLeft", throwIfNotFound: true);
		m_UiPad_TabRight = m_UiPad.FindAction("TabRight", throwIfNotFound: true);
		m_UiPad_OptionLeft = m_UiPad.FindAction("OptionLeft", throwIfNotFound: true);
		m_UiPad_OptionRight = m_UiPad.FindAction("OptionRight", throwIfNotFound: true);
		m_UiPad_DragAndDrop = m_UiPad.FindAction("DragAndDrop", throwIfNotFound: true);
		m_UiPad_DropGoods = m_UiPad.FindAction("DropGoods", throwIfNotFound: true);
		m_UiPad_InsertGoods = m_UiPad.FindAction("InsertGoods", throwIfNotFound: true);
		m_UiPad_Restore = m_UiPad.FindAction("Restore", throwIfNotFound: true);
		m_UiPad_Delete = m_UiPad.FindAction("Delete", throwIfNotFound: true);
		m_UiPad_MuteToggle = m_UiPad.FindAction("MuteToggle", throwIfNotFound: true);
		m_UiPad_IncreaseQuantityLarge = m_UiPad.FindAction("IncreaseQuantityLarge", throwIfNotFound: true);
		m_UiPad_DecreaseQuantityLarge = m_UiPad.FindAction("DecreaseQuantityLarge", throwIfNotFound: true);
		m_UiKeyboard = asset.FindActionMap("UiKeyboard", throwIfNotFound: true);
		m_UiKeyboard_SwapWeapon = m_UiKeyboard.FindAction("SwapWeapon", throwIfNotFound: true);
		m_UiKeyboard_SetQuickSlot = m_UiKeyboard.FindAction("SetQuickSlot", throwIfNotFound: true);
		m_UiKeyboard_InventorySelectHalf = m_UiKeyboard.FindAction("InventorySelectHalf", throwIfNotFound: true);
		m_UiKeyboard_Slider = m_UiKeyboard.FindAction("Slider", throwIfNotFound: true);
		m_UiKeyboard_InventoryMoveToStashAll = m_UiKeyboard.FindAction("InventoryMoveToStashAll", throwIfNotFound: true);
		m_UiKeyboard_Confirm = m_UiKeyboard.FindAction("Confirm", throwIfNotFound: true);
		m_UiKeyboard_Cancel = m_UiKeyboard.FindAction("Cancel", throwIfNotFound: true);
		m_Cutscene = asset.FindActionMap("Cutscene", throwIfNotFound: true);
		m_Cutscene_Skip = m_Cutscene.FindAction("Skip", throwIfNotFound: true);
	}
}
