using System.Collections.Generic;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class PlayerInputController : MonoBase
{
	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private NetworkManager _networkManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GamePlaySettingService _playSettingService;

	[Inject]
	private PlayerManager _playerManager;

	private readonly Plane _groundPlane = new Plane(Vector3.up, Vector3.zero);

	public ECompositeInputState CompositeInputState;

	private Vector2 _wheelSelectDir;

	[Inject]
	private GameObjectManager _gameObjectManager;

	private Vector2 _forward;

	private Vector2 _aimPadTarget;

	private Vector2 _aimPadDir;

	private Vector2 _aimMouseScreenPos;

	private bool _inputDirty;

	private bool _isAimPadActive;

	private bool _wasPlayerInputEnabled;

	private int _lastSentTick = -1;

	private int _nearestEnemyCacheTick = -1;

	private Vector3? _nearestEnemyCache;

	private CancellationTokenSource _aimPadDeactivateCts;

	private const float AIM_PAD_SMOOTH_SPEED_MAX = 10f;

	private const float AIM_PAD_DEACTIVATE_DELAY = 0.15f;

	private const float _padStickAttackMagnitudeThreshold = 0.99f;

	private bool _isPadStickAttackActive;

	private readonly HashSet<InputAction> _pressedSkillActions = new HashSet<InputAction>();

	private InputActions.PlayerFuncActions PlayerFuncActions => _inputManager.PlayerFuncActions;

	private InputActions.PlayerActions PlayerActions => _inputManager.PlayerActions;

	private CharacterModel CharacterModel => _gameManager.MyCharacterModel;

	public ReactiveProperty<bool> RxIsPingPressed { get; } = new ReactiveProperty<bool>(false);

	public bool IsPingPressed => RxIsPingPressed.Value;

	public ReactiveProperty<bool> RxIsEmojiPressed { get; } = new ReactiveProperty<bool>(false);

	public bool IsEmojiPressed => RxIsEmojiPressed.Value;

	public Vector2 WheelSelectDir => _wheelSelectDir;

	public Subject<Unit> OnPointMinimapPressed { get; } = new Subject<Unit>();

	private bool IsGamePad => _inputManager.RxControlDevice.Value == EControlDevice.GamePad;

	public bool IsAiming
	{
		get
		{
			if (_inputManager.RxControlDevice.Value == EControlDevice.GamePad)
			{
				return _aimPadDir.sqrMagnitude >= 0.01f;
			}
			return true;
		}
	}

	private float AimPadSmoothSpeed => (float)_playSettingService.PadSensitivity / 100f * 10f;

	private bool IsPadMoveBlocked
	{
		get
		{
			if (_inputManager.RxControlDevice.Value != EControlDevice.GamePad)
			{
				return false;
			}
			if (CompositeInputState != ECompositeInputState.Ping)
			{
				return CompositeInputState == ECompositeInputState.Emoji;
			}
			return true;
		}
	}

	private bool CanSendInput
	{
		get
		{
			if (!_gameManager.IsParticipant)
			{
				return false;
			}
			return _gameManager.MyCharacterModel != null;
		}
	}

	private void Start()
	{
		BindMove();
		BindSkill();
		BindItem();
		BindEtc();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void OnDestroy()
	{
		DisposeMove();
		DisposeSkillButtons();
		DisposeItemBtn();
		DisposeEtc();
	}

	private void BindEtc()
	{
		PlayerFuncActions.Interact.started += OnStartInteractionHold;
		PlayerFuncActions.Interact.canceled += OnCancelInteractionHold;
		PlayerActions.PingKeyboard.performed += OnPingPerformed;
		PlayerActions.PingKeyboard.canceled += OnPingCanceled;
		PlayerActions.EmojiKeyboard.performed += OnEmojiPerformed;
		PlayerActions.EmojiKeyboard.canceled += OnEmojiCanceled;
		PlayerActions.PingEmojiPad.performed += OnPingEmojiPadPerformed;
		PlayerActions.PingEmojiPad.canceled += OnPingEmojiPadCanceled;
		PlayerFuncActions.SwapWeapon.performed += OnSwapWeaponPerformed;
		PlayerActions.SwapPingEmojiPad.performed += OnSwapPingEmojiPad;
		PlayerActions.WheelSelect.performed += OnWheelSelectPerformed;
		PlayerActions.WheelSelect.canceled += OnWheelSelectCanceled;
		PlayerActions.PointMinimap.performed += PointMinimapPerformed;
	}

	private void DisposeEtc()
	{
		PlayerFuncActions.Interact.started -= OnStartInteractionHold;
		PlayerFuncActions.Interact.canceled -= OnCancelInteractionHold;
		PlayerActions.PingKeyboard.performed -= OnPingPerformed;
		PlayerActions.PingKeyboard.canceled -= OnPingCanceled;
		PlayerActions.EmojiKeyboard.performed -= OnEmojiPerformed;
		PlayerActions.EmojiKeyboard.canceled -= OnEmojiCanceled;
		PlayerActions.PingEmojiPad.performed -= OnPingEmojiPadPerformed;
		PlayerActions.PingEmojiPad.canceled -= OnPingEmojiPadCanceled;
		PlayerFuncActions.SwapWeapon.performed -= OnSwapWeaponPerformed;
		PlayerActions.SwapPingEmojiPad.performed -= OnSwapPingEmojiPad;
		PlayerActions.WheelSelect.performed -= OnWheelSelectPerformed;
		PlayerActions.WheelSelect.canceled -= OnWheelSelectCanceled;
		PlayerActions.PointMinimap.performed -= PointMinimapPerformed;
	}

	private void OnStartInteractionHold(InputAction.CallbackContext context)
	{
		if (CompositeInputState == ECompositeInputState.None)
		{
			ulong interactiveObjectId = GetInteractiveObjectId();
			if (interactiveObjectId != 0L)
			{
				_gameChannel.SendToHost(new ReqInputTryInteract
				{
					NetworkObjectId = interactiveObjectId
				});
			}
		}
	}

	private void OnCancelInteractionHold(InputAction.CallbackContext context)
	{
		if (_gameManager.MyCharacterModel == null || _gameManager.MyCharacterModel.IsWaitingInteractionFinish || !_gameManager.MyCharacterModel.CanCancelInputInteraction)
		{
			return;
		}
		ulong interactiveObjectId = GetInteractiveObjectId();
		if (interactiveObjectId != 0L)
		{
			_gameChannel.SendToHost(new ReqInputCancelInteract
			{
				NetworkObjectId = interactiveObjectId
			});
		}
	}

	private ulong GetInteractiveObjectId()
	{
		IInteractiveObject interactiveObject = _gameManager.MyCharacterView?.RxInteractiveObject.Value;
		if (interactiveObject == null)
		{
			return 0uL;
		}
		return interactiveObject.NetworkObjectId;
	}

	private void OnPingPerformed(InputAction.CallbackContext context)
	{
		if (CompositeInputState == ECompositeInputState.None)
		{
			RxIsPingPressed.Value = true;
			CompositeInputState = ECompositeInputState.Ping;
		}
	}

	private void OnPingCanceled(InputAction.CallbackContext context)
	{
		if (CompositeInputState == ECompositeInputState.Ping)
		{
			CompositeInputState = ECompositeInputState.None;
		}
		RxIsPingPressed.Value = false;
	}

	private void OnEmojiPerformed(InputAction.CallbackContext context)
	{
		if (CompositeInputState == ECompositeInputState.None)
		{
			RxIsEmojiPressed.Value = true;
			CompositeInputState = ECompositeInputState.Emoji;
		}
	}

	private void OnEmojiCanceled(InputAction.CallbackContext context)
	{
		if (CompositeInputState == ECompositeInputState.Emoji)
		{
			CompositeInputState = ECompositeInputState.None;
		}
		RxIsEmojiPressed.Value = false;
	}

	private void OnPingEmojiPadPerformed(InputAction.CallbackContext context)
	{
		if (CompositeInputState == ECompositeInputState.None)
		{
			CompositeInputState = ECompositeInputState.Ping;
			RxIsPingPressed.Value = true;
		}
	}

	private void OnPingEmojiPadCanceled(InputAction.CallbackContext context)
	{
		if (CompositeInputState == ECompositeInputState.Ping || CompositeInputState == ECompositeInputState.Emoji)
		{
			CompositeInputState = ECompositeInputState.None;
		}
		RxIsPingPressed.Value = false;
		RxIsEmojiPressed.Value = false;
	}

	private void OnSwapWeaponPerformed(InputAction.CallbackContext context)
	{
		if (CompositeInputState == ECompositeInputState.None && _gameManager.IsParticipant)
		{
			_gameChannel.SendToHost(new ReqSwapWeapon());
		}
	}

	private void OnSwapPingEmojiPad(InputAction.CallbackContext context)
	{
		if (CompositeInputState == ECompositeInputState.Ping)
		{
			CompositeInputState = ECompositeInputState.Emoji;
			RxIsPingPressed.Value = false;
			RxIsEmojiPressed.Value = true;
		}
		else if (CompositeInputState == ECompositeInputState.Emoji)
		{
			CompositeInputState = ECompositeInputState.Ping;
			RxIsEmojiPressed.Value = false;
			RxIsPingPressed.Value = true;
		}
	}

	private void OnWheelSelectPerformed(InputAction.CallbackContext context)
	{
		_wheelSelectDir = context.ReadValue<Vector2>();
	}

	private void OnWheelSelectCanceled(InputAction.CallbackContext context)
	{
		_wheelSelectDir = Vector2.zero;
	}

	private void PointMinimapPerformed(InputAction.CallbackContext context)
	{
		if (CompositeInputState == ECompositeInputState.None)
		{
			OnPointMinimapPressed.OnNext(Unit.Default);
		}
	}

	private void BindItem()
	{
		PlayerFuncActions.UseItem.started += OnUseItemStart;
		PlayerFuncActions.UseItem.canceled += OnUseItemEnd;
	}

	private void DisposeItemBtn()
	{
		PlayerFuncActions.UseItem.started -= OnUseItemStart;
		PlayerFuncActions.UseItem.canceled -= OnUseItemEnd;
	}

	private void OnUseItemStart(InputAction.CallbackContext context)
	{
		if (!CanSendInput)
		{
			return;
		}
		InputAction action = context.action;
		if (action != null)
		{
			int num = action.controls.IndexOf((InputControl c) => c == context.control);
			if (num < 0)
			{
				_logger.Warning("OnUseSkillStart: ctx.control not found in action controls.");
			}
			else
			{
				TrySendInputUseItem(_playerManager.QuickSlotItemIds[num], ESkillInputEvent.ItemSkillDown);
			}
		}
	}

	private void OnUseItemEnd(InputAction.CallbackContext context)
	{
		if (!CanSendInput)
		{
			return;
		}
		InputAction action = context.action;
		if (action != null)
		{
			int num = action.controls.IndexOf((InputControl c) => c == context.control);
			if (num < 0)
			{
				_logger.Warning("OnUseSkillStart: ctx.control not found in action controls.");
			}
			else
			{
				TrySendInputUseItem(_playerManager.QuickSlotItemIds[num], ESkillInputEvent.ItemSkillUp);
			}
		}
	}

	private bool TrySendInputUseItem(int itemId, ESkillInputEvent inputEvent)
	{
		if (_gameManager.MyCharacterModel.HasBuffEffect(EBuffEffectType.ItemBlocked) || itemId < 1)
		{
			return false;
		}
		_gameChannel.SendToHost(new ReqInputUseItem
		{
			ItemId = itemId,
			InputEvent = inputEvent
		});
		return true;
	}

	private void BindMove()
	{
		PlayerActions.MoveKeyboard.performed += OnMove;
		PlayerActions.MoveKeyboard.canceled += OnMove;
		PlayerActions.MovePad.performed += OnMove;
		PlayerActions.MovePad.canceled += OnMove;
		PlayerFuncActions.Run.performed += OnRunStart;
		PlayerFuncActions.Run.canceled += OnRunEnd;
		PlayerActions.AimMouse.performed += OnAimMouse;
		PlayerActions.AimPad.performed += OnAimPad;
		PlayerActions.AimPad.canceled += OnAimPad;
	}

	private void DisposeMove()
	{
		PlayerActions.MoveKeyboard.performed -= OnMove;
		PlayerActions.MoveKeyboard.canceled -= OnMove;
		PlayerActions.MovePad.performed -= OnMove;
		PlayerActions.MovePad.canceled -= OnMove;
		PlayerFuncActions.Run.performed -= OnRunStart;
		PlayerFuncActions.Run.canceled -= OnRunEnd;
		PlayerActions.AimMouse.performed -= OnAimMouse;
		PlayerActions.AimPad.performed -= OnAimPad;
		PlayerActions.AimPad.canceled -= OnAimPad;
	}

	private void FixedUpdate()
	{
		if (!CanSendInput && CompositeInputState != ECompositeInputState.None)
		{
			CompositeInputState = ECompositeInputState.None;
			_isPadStickAttackActive = false;
		}
		bool enabled = PlayerActions.enabled;
		if (enabled && !_wasPlayerInputEnabled)
		{
			RefreshAim();
		}
		_wasPlayerInputEnabled = enabled;
		float t = Mathf.Clamp01(Time.fixedDeltaTime * AimPadSmoothSpeed);
		_aimPadDir = Vector2.Lerp(_aimPadDir, _aimPadTarget, t);
		int num = _networkManager?.NetworkTickSystem?.LocalTime.Tick ?? (-1);
		if (num < _lastSentTick)
		{
			_lastSentTick = -1;
		}
		if (_lastSentTick < num)
		{
			OnUpdateNetworkTick(num);
		}
	}

	private void OnUpdateNetworkTick(int networkTick)
	{
		_lastSentTick = networkTick;
		FlushInput();
		void FlushInput()
		{
			if (_isAimPadActive || _forward.sqrMagnitude >= 0.01f)
			{
				_inputDirty = true;
			}
			else if (IsGamePad && _playSettingService.AimSupportOn)
			{
				// TODO(decomp): this aim-support branch was dropped by Ghidra; rebuilt from the disassembly @0x180735A10
				if (_nearestEnemyCacheTick != _lastSentTick)
				{
					_nearestEnemyCacheTick = _lastSentTick;
					_nearestEnemyCache = FindNearestEnemyInSight();
				}
				if (_nearestEnemyCache.HasValue)
				{
					_inputDirty = true;
				}
			}
			if (_inputDirty)
			{
				Vector2 forward = ((IsGamePad && (CompositeInputState == ECompositeInputState.Ping || CompositeInputState == ECompositeInputState.Emoji)) ? Vector2.zero : _forward);
				Vector3 aim = CalculateWorldAimPoint();
				_gameChannel.SendToHost(new ReqInputMoveAim
				{
					PackedForward = InputPacking.PackForward(forward),
					PackedAim = InputPacking.PackAim(aim)
				});
				_inputDirty = false;
			}
		}
	}

	private Vector3 CalculateWorldAimPoint()
	{
		Vector2 aimScreenPoint = GetAimScreenPoint(includeMoveDir: true);
		if (aimScreenPoint == Vector2.zero)
		{
			return Vector3.zero;
		}
		Ray ray = _cameraManager.MainCamera.ScreenPointToRay(aimScreenPoint);
		if (_groundPlane.Raycast(ray, out var enter))
		{
			return ray.GetPoint(enter);
		}
		return Vector3.zero;
	}

	private void OnMove(InputAction.CallbackContext context)
	{
		Vector2 forward = _forward;
		_forward = context.ReadValue<Vector2>();
		_inputDirty = true;
		if (!_playSettingService.RunToggleOn && !(_forward.sqrMagnitude < 0.01f) && !(forward.sqrMagnitude >= 0.01f) && PlayerFuncActions.Run.IsPressed())
		{
			_gameChannel.SendToHost(new ReqInputRun
			{
				Run = true
			});
		}
	}

	private void OnRunStart(InputAction.CallbackContext context)
	{
		if (!(_gameManager.MyCharacterModel == null))
		{
			ReqInputRun reqInputRun = ((!_playSettingService.RunToggleOn) ? new ReqInputRun
			{
				Run = true
			} : new ReqInputRun
			{
				Run = !_gameManager.MyCharacterModel.TryRun
			});
			_gameChannel.SendToHost(reqInputRun);
		}
	}

	private void OnRunEnd(InputAction.CallbackContext context)
	{
		if (!_playSettingService.RunToggleOn)
		{
			_gameChannel.SendToHost(new ReqInputRun
			{
				Run = false
			});
		}
	}

	private void OnAimMouse(InputAction.CallbackContext context)
	{
		_aimMouseScreenPos = context.ReadValue<Vector2>();
		_inputDirty = true;
	}

	private void RefreshAim()
	{
		if (!IsGamePad && Mouse.current != null)
		{
			_aimMouseScreenPos = Mouse.current.position.ReadValue();
		}
		_inputDirty = true;
	}

	public Vector2 GetAimScreenPoint(bool includeMoveDir = false)
	{
		if (!IsGamePad)
		{
			return _aimMouseScreenPos;
		}
		float num = (float)Screen.width * 0.5f;
		float num2 = (float)Screen.height * 0.5f;
		if (_isAimPadActive && !(_aimPadDir.sqrMagnitude < 0.01f))
		{
			return new Vector2(_aimPadDir.x * num + num, _aimPadDir.y * num2 + num2);
		}
		if (_playSettingService.AimSupportOn)
		{
			Vector3? nearestEnemyInSightCached = GetNearestEnemyInSightCached();
			if (nearestEnemyInSightCached.HasValue)
			{
				Vector3 vector = _cameraManager.MainCamera.WorldToScreenPoint(nearestEnemyInSightCached.Value);
				return new Vector2(vector.x, vector.y);
			}
		}
		if (!includeMoveDir)
		{
			return new Vector2(num, num2);
		}
		if (_forward.sqrMagnitude < 0.01f)
		{
			return Vector2.zero;
		}
		return new Vector2(_forward.x * num + num, _forward.y * num2 + num2);
	}

	private bool HasAimSupportTarget()
	{
		if (IsGamePad && _playSettingService.AimSupportOn)
		{
			return GetNearestEnemyInSightCached().HasValue;
		}
		return false;
	}

	private Vector3? GetNearestEnemyInSightCached()
	{
		if (_nearestEnemyCacheTick != _lastSentTick)
		{
			_nearestEnemyCacheTick = _lastSentTick;
			_nearestEnemyCache = FindNearestEnemyInSight();
		}
		return _nearestEnemyCache;
	}

	private Vector3? FindNearestEnemyInSight()
	{
		if (_gameManager.MyCharacterModel == null)
		{
			return null;
		}
		Vector3 position = _gameManager.MyCharacterModel.Position;
		Vector3? result = null;
		float num = float.MaxValue;
		foreach (MonsterModel item in _gameObjectManager.MonstersInLocalSight)
		{
			if (!(item == null))
			{
				float sqrMagnitude = (item.Position - position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					result = item.Position;
					num = sqrMagnitude;
				}
			}
		}
		return result;
	}

	public void OnAimPad(InputAction.CallbackContext context)
	{
		Vector2 vector = context.ReadValue<Vector2>();
		if (vector.magnitude > 0.01f)
		{
			_aimPadDeactivateCts?.Cancel();
			_aimPadDeactivateCts?.Dispose();
			_aimPadDeactivateCts = null;
			float num = Mathf.Clamp01(vector.magnitude);
			Vector2 normalized = vector.normalized;
			float num2 = Mathf.Cos(num * (float)Math.PI * 0.5f);
			_inputDirty = true;
			_isAimPadActive = true;
			_aimPadTarget = normalized * (1f - num2);
		}
		else
		{
			_aimPadTarget = Vector2.zero;
			DeactivateAimPadDelayed().Forget();
			_inputDirty = true;
		}
	}

	private async UniTaskVoid DeactivateAimPadDelayed()
	{
		_aimPadDeactivateCts?.Cancel();
		_aimPadDeactivateCts?.Dispose();
		_aimPadDeactivateCts = new CancellationTokenSource();
		await UniTask.Delay(TimeSpan.FromSeconds(0.15), ignoreTimeScale: false, PlayerLoopTiming.Update, _aimPadDeactivateCts.Token);
		_inputDirty = true;
		_isAimPadActive = false;
	}

	private bool TryMarkSkillPressed(InputAction.CallbackContext ctx)
	{
		return _pressedSkillActions.Add(ctx.action);
	}

	private bool TryMarkSkillReleased(InputAction.CallbackContext ctx)
	{
		return _pressedSkillActions.Remove(ctx.action);
	}

	private void OnUseSkillEnd(InputAction.CallbackContext context)
	{
		if (!CanSendInput)
		{
			return;
		}
		InputAction action = context.action;
		if (action == null)
		{
			return;
		}
		int num = action.controls.IndexOf((InputControl c) => c == context.control);
		if (num < 0)
		{
			_logger.Warning("OnUseSkillEnd: ctx.control not found in action controls.");
			return;
		}
		ESkillInputEvent inputEvent;
		switch (num)
		{
		case 0:
			inputEvent = ESkillInputEvent.AttackUp;
			break;
		case 1:
			inputEvent = ESkillInputEvent.Skill1Up;
			break;
		case 2:
			inputEvent = ESkillInputEvent.DashUp;
			break;
		case 3:
			inputEvent = ESkillInputEvent.Skill2Up;
			break;
		case 4:
			inputEvent = ESkillInputEvent.Skill3Up;
			break;
		case 5:
			inputEvent = ESkillInputEvent.Skill4Up;
			break;
		default:
			inputEvent = ESkillInputEvent.None;
			break;
		}
		_gameChannel.SendToHost(new ReqInputSkill
		{
			InputEvent = inputEvent
		});
	}

	public InputAction GetSkillInputAction(int skillId)
	{
		switch (_gameManager.MyCharacterModel.GetActiveSkillIds().IndexOf(skillId))
		{
		case 0:
			return PlayerFuncActions.SkillBasicAttack;
		case 1:
			return PlayerFuncActions.SkillOne;
		case 2:
			return PlayerFuncActions.SkillDash;
		case 3:
			return PlayerFuncActions.SkillTwo;
		case 4:
			return PlayerFuncActions.SkillThree;
		case 5:
			return PlayerFuncActions.SkillFour;
		case 6:
			return PlayerFuncActions.SkillEquipment;
		default:
			return null;
		}
	}

	private void BindSkill()
	{
		PlayerFuncActions.SkillBasicAttack.started += OnBasicAttackStart;
		PlayerFuncActions.SkillBasicAttack.canceled += OnBasicAttackEnd;
		PlayerFuncActions.SkillBasicAttackPadStick.performed += OnBasicAttackPadStickPerformed;
		PlayerFuncActions.SkillBasicAttackPadStick.canceled += OnBasicAttackPadStickEnd;
		PlayerFuncActions.SkillOne.started += OnSkillOneClickStart;
		PlayerFuncActions.SkillOne.canceled += OnSkillOneClickEnd;
		PlayerFuncActions.SkillTwo.started += OnSkillTwoClickStart;
		PlayerFuncActions.SkillTwo.canceled += OnSkillTwoClickEnd;
		PlayerFuncActions.SkillThree.started += OnSkillThreeClickStart;
		PlayerFuncActions.SkillThree.canceled += OnSkillThreeClickEnd;
		PlayerFuncActions.SkillFour.started += OnSkillFourClickStart;
		PlayerFuncActions.SkillFour.canceled += OnSkillFourClickEnd;
		PlayerFuncActions.SkillEquipment.started += OnEquipmentSkillClickStart;
		PlayerFuncActions.SkillEquipment.canceled += OnEquipmentSkillClickEnd;
		PlayerFuncActions.SkillDash.started += OnDashSkillStart;
		PlayerFuncActions.SkillDash.canceled += OnDashSkillEnd;
	}

	private void DisposeSkillButtons()
	{
		PlayerFuncActions.SkillBasicAttack.started -= OnBasicAttackStart;
		PlayerFuncActions.SkillBasicAttack.canceled -= OnBasicAttackEnd;
		PlayerFuncActions.SkillBasicAttackPadStick.performed -= OnBasicAttackPadStickPerformed;
		PlayerFuncActions.SkillBasicAttackPadStick.canceled -= OnBasicAttackPadStickEnd;
		PlayerFuncActions.SkillOne.started -= OnSkillOneClickStart;
		PlayerFuncActions.SkillOne.canceled -= OnSkillOneClickEnd;
		PlayerFuncActions.SkillTwo.started -= OnSkillTwoClickStart;
		PlayerFuncActions.SkillTwo.canceled -= OnSkillTwoClickEnd;
		PlayerFuncActions.SkillThree.started -= OnSkillThreeClickStart;
		PlayerFuncActions.SkillThree.canceled -= OnSkillThreeClickEnd;
		PlayerFuncActions.SkillFour.started -= OnSkillFourClickStart;
		PlayerFuncActions.SkillFour.canceled -= OnSkillFourClickEnd;
		PlayerFuncActions.SkillEquipment.started -= OnEquipmentSkillClickStart;
		PlayerFuncActions.SkillEquipment.canceled -= OnEquipmentSkillClickEnd;
		PlayerFuncActions.SkillDash.started -= OnDashSkillStart;
		PlayerFuncActions.SkillDash.canceled -= OnDashSkillEnd;
	}

	private void SendSkillPressed(InputAction.CallbackContext ctx, ESkillInputEvent inputEvent)
	{
		if (CanSendInput && TryMarkSkillPressed(ctx) && (CompositeInputState == ECompositeInputState.None || CompositeInputState == ECompositeInputState.Skill))
		{
			_gameChannel.SendToHost(new ReqInputSkill
			{
				InputEvent = inputEvent
			});
			CompositeInputState = ECompositeInputState.Skill;
		}
	}

	private void SendSkillReleased(InputAction.CallbackContext ctx, ESkillInputEvent inputEvent)
	{
		if (CanSendInput && TryMarkSkillReleased(ctx) && (CompositeInputState == ECompositeInputState.None || CompositeInputState == ECompositeInputState.Skill))
		{
			_gameChannel.SendToHost(new ReqInputSkill
			{
				InputEvent = inputEvent
			});
			CompositeInputState = ECompositeInputState.None;
		}
	}

	private void OnSkillTwoClickStart(InputAction.CallbackContext ctx)
	{
		SendSkillPressed(ctx, ESkillInputEvent.Skill2Down);
	}

	private void OnSkillTwoClickEnd(InputAction.CallbackContext ctx)
	{
		SendSkillReleased(ctx, ESkillInputEvent.Skill2Up);
	}

	private void OnSkillThreeClickStart(InputAction.CallbackContext ctx)
	{
		SendSkillPressed(ctx, ESkillInputEvent.Skill3Down);
	}

	private void OnSkillThreeClickEnd(InputAction.CallbackContext ctx)
	{
		SendSkillReleased(ctx, ESkillInputEvent.Skill3Up);
	}

	private void OnSkillFourClickStart(InputAction.CallbackContext ctx)
	{
		SendSkillPressed(ctx, ESkillInputEvent.Skill4Down);
	}

	private void OnSkillFourClickEnd(InputAction.CallbackContext ctx)
	{
		SendSkillReleased(ctx, ESkillInputEvent.Skill4Up);
	}

	private void OnEquipmentSkillClickStart(InputAction.CallbackContext ctx)
	{
		SendSkillPressed(ctx, ESkillInputEvent.EquipmentSkillDown);
	}

	private void OnEquipmentSkillClickEnd(InputAction.CallbackContext ctx)
	{
		SendSkillReleased(ctx, ESkillInputEvent.EquipmentSkillUp);
	}

	private void OnDashSkillStart(InputAction.CallbackContext ctx)
	{
		SendSkillPressed(ctx, ESkillInputEvent.DashDown);
	}

	private void OnDashSkillEnd(InputAction.CallbackContext ctx)
	{
		SendSkillReleased(ctx, ESkillInputEvent.DashUp);
	}

	private void OnBasicAttackStart(InputAction.CallbackContext ctx)
	{
		if (CanSendInput && TryMarkSkillPressed(ctx) && (CompositeInputState == ECompositeInputState.None || CompositeInputState == ECompositeInputState.Skill))
		{
			_gameChannel.SendToHost(new ReqInputSkill
			{
				InputEvent = ESkillInputEvent.AttackDown,
				ContinuousAttack = _playSettingService.AutoAttackOn
			});
			CompositeInputState = ECompositeInputState.Skill;
		}
	}

	private void OnBasicAttackEnd(InputAction.CallbackContext ctx)
	{
		SendSkillReleased(ctx, ESkillInputEvent.AttackUp);
	}

	private void OnBasicAttackPadStickPerformed(InputAction.CallbackContext ctx)
	{
		if (CanSendInput && _playSettingService.RstickAttackOn)
		{
			SetPadStickAttackActive(ctx.ReadValue<Vector2>().magnitude >= 0.99f);
		}
	}

	private void OnBasicAttackPadStickEnd(InputAction.CallbackContext ctx)
	{
		if (CanSendInput && _playSettingService.RstickAttackOn)
		{
			SetPadStickAttackActive(active: false);
		}
	}

	private void SetPadStickAttackActive(bool active)
	{
		if (_isPadStickAttackActive == active || (CompositeInputState != ECompositeInputState.None && CompositeInputState != ECompositeInputState.Skill))
		{
			return;
		}
		ECompositeInputState compositeInputState;
		if (active)
		{
			_gameChannel.SendToHost(new ReqInputSkill
			{
				InputEvent = ESkillInputEvent.AttackDown,
				ContinuousAttack = _playSettingService.AutoAttackOn
			});
			compositeInputState = ECompositeInputState.Skill;
		}
		else
		{
			_gameChannel.SendToHost(new ReqInputSkill
			{
				InputEvent = ESkillInputEvent.AttackUp
			});
			compositeInputState = ECompositeInputState.None;
		}
		CompositeInputState = compositeInputState;
		_isPadStickAttackActive = active;
	}

	private void OnSkillOneClickStart(InputAction.CallbackContext ctx)
	{
		SendSkillPressed(ctx, ESkillInputEvent.Skill1Down);
	}

	private void OnSkillOneClickEnd(InputAction.CallbackContext ctx)
	{
		SendSkillReleased(ctx, ESkillInputEvent.Skill1Up);
	}
}
