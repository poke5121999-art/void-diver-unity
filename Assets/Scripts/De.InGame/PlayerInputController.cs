using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class PlayerInputController : MonoBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CDeactivateAimPadDelayed_003Ed__93 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public PlayerInputController _003C_003E4__this;

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

	private readonly Plane _groundPlane;

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

	private int _lastSentTick;

	private int _nearestEnemyCacheTick;

	private Vector3? _nearestEnemyCache;

	private CancellationTokenSource _aimPadDeactivateCts;

	private const float AIM_PAD_SMOOTH_SPEED_MAX = 10f;

	private const float AIM_PAD_DEACTIVATE_DELAY = 0.15f;

	private const float _padStickAttackMagnitudeThreshold = 0.99f;

	private bool _isPadStickAttackActive;

	private readonly HashSet<InputAction> _pressedSkillActions;

	private InputActions.PlayerFuncActions PlayerFuncActions => default;

	private InputActions.PlayerActions PlayerActions => default;

	private CharacterModel CharacterModel => null;

	public ReactiveProperty<bool> RxIsPingPressed { get; }

	public bool IsPingPressed => false;

	public ReactiveProperty<bool> RxIsEmojiPressed { get; }

	public bool IsEmojiPressed => false;

	public Vector2 WheelSelectDir => default;

	public Subject<Unit> OnPointMinimapPressed { get; }

	private bool IsGamePad => false;

	public bool IsAiming => false;

	private float AimPadSmoothSpeed => 0f;

	private bool IsPadMoveBlocked => false;

	private bool CanSendInput => false;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void BindEtc()
	{
	}

	private void DisposeEtc()
	{
	}

	private void OnStartInteractionHold(InputAction.CallbackContext context)
	{
	}

	private void OnCancelInteractionHold(InputAction.CallbackContext context)
	{
	}

	private void OnPingPerformed(InputAction.CallbackContext context)
	{
	}

	private void OnPingCanceled(InputAction.CallbackContext context)
	{
	}

	private void OnEmojiPerformed(InputAction.CallbackContext context)
	{
	}

	private void OnEmojiCanceled(InputAction.CallbackContext context)
	{
	}

	private void OnPingEmojiPadPerformed(InputAction.CallbackContext context)
	{
	}

	private void OnPingEmojiPadCanceled(InputAction.CallbackContext context)
	{
	}

	private void OnSwapWeaponPerformed(InputAction.CallbackContext context)
	{
	}

	private void OnSwapPingEmojiPad(InputAction.CallbackContext context)
	{
	}

	private void OnWheelSelectPerformed(InputAction.CallbackContext context)
	{
	}

	private void OnWheelSelectCanceled(InputAction.CallbackContext context)
	{
	}

	private void PointMinimapPerformed(InputAction.CallbackContext context)
	{
	}

	private void BindItem()
	{
	}

	private void DisposeItemBtn()
	{
	}

	private void OnUseItemStart(InputAction.CallbackContext context)
	{
	}

	private void OnUseItemEnd(InputAction.CallbackContext context)
	{
	}

	private bool TrySendInputUseItem(int itemId, ESkillInputEvent inputEvent)
	{
		return false;
	}

	private void BindMove()
	{
	}

	private void DisposeMove()
	{
	}

	private void FixedUpdate()
	{
	}

	private void OnUpdateNetworkTick(int networkTick)
	{
	}

	private Vector3 CalculateWorldAimPoint()
	{
		return default;
	}

	private void OnMove(InputAction.CallbackContext context)
	{
	}

	private void OnRunStart(InputAction.CallbackContext context)
	{
	}

	private void OnRunEnd(InputAction.CallbackContext context)
	{
	}

	private void OnAimMouse(InputAction.CallbackContext context)
	{
	}

	private void RefreshAim()
	{
	}

	public Vector2 GetAimScreenPoint(bool includeMoveDir = false)
	{
		return default;
	}

	private bool HasAimSupportTarget()
	{
		return false;
	}

	private Vector3? GetNearestEnemyInSightCached()
	{
		return null;
	}

	private Vector3? FindNearestEnemyInSight()
	{
		return null;
	}

	public void OnAimPad(InputAction.CallbackContext context)
	{
	}

	[AsyncStateMachine(typeof(_003CDeactivateAimPadDelayed_003Ed__93))]
	private UniTaskVoid DeactivateAimPadDelayed()
	{
		return default;
	}

	private bool TryMarkSkillPressed(InputAction.CallbackContext ctx)
	{
		return false;
	}

	private bool TryMarkSkillReleased(InputAction.CallbackContext ctx)
	{
		return false;
	}

	private void OnUseSkillEnd(InputAction.CallbackContext context)
	{
	}

	public InputAction GetSkillInputAction(int skillId)
	{
		return null;
	}

	private void BindSkill()
	{
	}

	private void DisposeSkillButtons()
	{
	}

	private void OnSkillTwoClickStart(InputAction.CallbackContext ctx)
	{
	}

	private void OnSkillTwoClickEnd(InputAction.CallbackContext ctx)
	{
	}

	private void OnSkillThreeClickStart(InputAction.CallbackContext ctx)
	{
	}

	private void OnSkillThreeClickEnd(InputAction.CallbackContext ctx)
	{
	}

	private void OnSkillFourClickStart(InputAction.CallbackContext ctx)
	{
	}

	private void OnSkillFourClickEnd(InputAction.CallbackContext ctx)
	{
	}

	private void OnEquipmentSkillClickStart(InputAction.CallbackContext ctx)
	{
	}

	private void OnEquipmentSkillClickEnd(InputAction.CallbackContext ctx)
	{
	}

	private void OnDashSkillStart(InputAction.CallbackContext ctx)
	{
	}

	private void OnDashSkillEnd(InputAction.CallbackContext ctx)
	{
	}

	private void OnBasicAttackStart(InputAction.CallbackContext ctx)
	{
	}

	private void OnBasicAttackEnd(InputAction.CallbackContext ctx)
	{
	}

	private void OnBasicAttackPadStickPerformed(InputAction.CallbackContext ctx)
	{
	}

	private void OnBasicAttackPadStickEnd(InputAction.CallbackContext ctx)
	{
	}

	private void SetPadStickAttackActive(bool active)
	{
	}

	private void OnSkillOneClickStart(InputAction.CallbackContext ctx)
	{
	}

	private void OnSkillOneClickEnd(InputAction.CallbackContext ctx)
	{
	}
}
