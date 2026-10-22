using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using Febucci.UI;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

public class CharacterHudPresenter : BaseUnitHudPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CProcessParadoxQueueAsync_003Ed__65 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CharacterHudPresenter _003C_003E4__this;

		private Animator _003Canimator_003E5__2;

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
	private struct _003CShowSkillReadyEffectAsync_003Ed__66 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CharacterHudPresenter _003C_003E4__this;

		public int skillId;

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
	private struct _003CTryShowToastMessageAsync_003Ed__52 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CharacterHudPresenter _003C_003E4__this;

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

	private const float TOAST_HEIGHT_DEFAULT = 2f;

	private const float TOAST_HEIGHT_FINISH = 2.5f;

	private const float TOAST_DURATION = 1.2f;

	[SerializeField]
	private CharacterModel _characterModel;

	[View]
	private CharacterHudView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private GameChatManager _chatManager;

	[Inject]
	private ToastManager _toastManager;

	private CancellationTokenSource _toastCts;

	private CompositeDisposable _disposable;

	private int _currentToastPrefabIdx;

	private int _currentToastTextIndex;

	private readonly Queue<string> _toastMessageQueue;

	private Tween _consumeGaugeTween;

	private IDisposable _gaugeSkillIdDisposable;

	private CompositeDisposable _viewTargetDisposable;

	private float _targetStaminaRatio;

	private float _staminaGaugeVelocity;

	private readonly Queue<(int id, bool isAdded)> _paradoxQueue;

	private bool _isProcessingParadox;

	private CancellationTokenSource _paradoxCts;

	private CompositeDisposable _skillReadyDisposable;

	private readonly Dictionary<int, bool> _prevSkillUsable;

	private float _skillReadyCheckTimer;

	private const float SKILL_READY_CHECK_INTERVAL = 0.1f;

	private const string PARADOX_ANIM_IN = "In";

	private const string PARADOX_ANIM_OUT = "Out";

	private const float PARADOX_ANIM_IN_DURATION = 0.9f;

	private const float PARADOX_ANIM_OUT_DURATION = 0.9f;

	private GameObject _currentEmoji;

	protected override GameObject Bubble => null;

	protected override TypewriterByCharacter BubbleTextWriter => null;

	protected override TextMeshProUGUI Monologue => null;

	private StressLineController StressLineController => null;

	private bool IsMyCharacter => false;

	private void Start()
	{
	}

	private void OnCharacterSpawned(CharacterController characterController)
	{
	}

	protected override void OnDestroy()
	{
	}

	private void Update()
	{
	}

	private void UpdateSkillReadyNotification()
	{
	}

	private void UpdateCharacterName()
	{
	}

	private void UpdateHpUi()
	{
	}

	private void ShowConsumeGauge(int itemId, float duration)
	{
	}

	private void OnUpdateMaxHp()
	{
	}

	[AsyncStateMachine(typeof(_003CTryShowToastMessageAsync_003Ed__52))]
	private UniTask TryShowToastMessageAsync()
	{
		return default;
	}

	private void AddToastMessage(string message)
	{
	}

	private void OnStateChanged(EActionState state)
	{
	}

	private void OnViewTargetChanged(ulong clientId)
	{
	}

	private void OnShowEmoji(int emojiId)
	{
	}

	private void OnStressDamage(NtfStressDamage message)
	{
	}

	private void OnUpdateStressState(EStressState state)
	{
	}

	private void OnUpdateStress(float stress)
	{
	}

	private void OnUpdateState(EActionState state)
	{
	}

	private void ShowSelfReviveText(string text)
	{
	}

	private void OnUpdateParadox(IList<int> previousIds, IList<int> currentIds)
	{
	}

	private void EnqueueParadox(int id, bool added)
	{
	}

	[AsyncStateMachine(typeof(_003CProcessParadoxQueueAsync_003Ed__65))]
	private UniTask ProcessParadoxQueueAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CShowSkillReadyEffectAsync_003Ed__66))]
	private UniTask ShowSkillReadyEffectAsync(int skillId)
	{
		return default;
	}

	private bool IsDespairBuff(int buffId)
	{
		return false;
	}

	private void OnDespairBuffAdded(int buffId)
	{
	}

	private void OnDespairBuffRemoved(int buffId)
	{
	}

	private void OnEquipmentBroken(NtfEquipmentBroken message)
	{
	}
}
