using System.Linq;
using DG.Tweening;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using SomeNet.Utility;
using Spine.Unity;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class NpcObject : NetworkBase, IInteractiveObject
{
	public class Args
	{
		public bool ShowSpawnVfx;
	}

	[SerializeField]
	private int _npcId;

	[SerializeField]
	[Range(0f, 5f)]
	private float _holdingTime = 0.5f;

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[SerializeField]
	private UnitDissolveVisualEffect _dissolveEffect;

	[SerializeField]
	private GroundNavigation _navigation;

	private Args _args;

	public readonly ReactiveProperty<int> RxEmployeeId = new ReactiveProperty<int>();

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameVfxManager _vfxManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameNpcDialogManager _npcDialogManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	protected PlayerManager PlayerManager;

	[Inject]
	private SeriLogger _logger;

	private NetworkList<int> _syncNpcFunctionIds;

	private bool _isNavigationActive;

	private bool _isInteractableIndividually = true;

	private readonly NetworkVariable<bool> _syncShowSpawnVfx = new NetworkVariable<bool>();

	private Tweener _tweener;

	private SkeletonAnimation _skeletonAnimation;

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public float HoldingTime => _holdingTime;

	public EOutlineType OutlineType => EOutlineType.Default;

	public string HoldingSfx
	{
		get
		{
			if (!string.IsNullOrEmpty(_holdingSfx))
			{
				return _holdingSfx;
			}
			return "NPC_default";
		}
	}

	public string InteractionSfx => _interactionSfx;

	public int NpcId => _npcId;

	public bool HasFunction
	{
		get
		{
			int[] npcFunctionIds = NpcFunctionIds;
			if (npcFunctionIds != null)
			{
				return npcFunctionIds.Length > 0;
			}
			return false;
		}
	}

	public int[] NpcFunctionIds
	{
		get
		{
			if (_syncNpcFunctionIds.Count < 1)
			{
				return null;
			}
			return _syncNpcFunctionIds.ToArray();
		}
	}

	public ReactiveCollection<int> RxNpcFunctionIds { get; } = new ReactiveCollection<int>();

	public ReactiveProperty<ENpcState> RxNpcState { get; } = new ReactiveProperty<ENpcState>();

	public ENpcState NpcState => RxNpcState.Value;

	public Subject<string> OnShowBubbleText { get; } = new Subject<string>();

	public Subject<string> OnShowMonologueText { get; } = new Subject<string>();

	private bool ShowSpawnVfx => _syncShowSpawnVfx.Value;

	private bool HasDialog => _npcDialogManager.HasDialog(_npcId);

	ulong IInteractiveObject.NetworkObjectId => base.NetworkObjectId;

	public void Init(Args args)
	{
		AssertIfNotHost();
		_args = args;
	}

	protected override void Awake()
	{
		base.Awake();
		_syncNpcFunctionIds = new NetworkList<int>();
	}

	public override void OnDestroy()
	{
		if (_tweener != null)
		{
			_tweener.Kill();
		}
		if (base.Disposable == null)
		{
			_logger.Error(string.Format("NpcId : {0}", _npcId));
		}
		base.OnDestroy();
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		if (_args == null)
		{
			Debug.LogError(string.Format("[NpcObject] OnHostSpawn: _args is null (npcId={0}). Init() must be called before Spawn().", _npcId));
			return;
		}
		_syncShowSpawnVfx.Value = _args.ShowSpawnVfx;
		if (!TNpcFunction.GroupTable.TryGetValue(_npcId, out var _))
		{
			_eventSystem.OnEventOccured.Subscribe(OnEventOccured).AddTo(this);
		}
	}

	public override void OnNetworkSpawn()
	{
		float tweenValue = 0f;
		base.OnNetworkSpawn();
		RegisterListEvents();
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
		_objectManager.OnNpcSpawned.OnNext(this);
		_gameManager.OnCharacterSpawned.Subscribe(OnCharacterSpawned).AddTo(this);
		base.gameObject.name = string.Format("[Npc] {0}", _npcId);
		if (!_syncShowSpawnVfx.Value)
		{
			if (_dissolveEffect != null)
			{
				_dissolveEffect.DissolveIntensity = 0f;
			}
			return;
		}
		_vfxManager.SpawnVfx("Common/Npc_Spawn", 2f, base.transform.position);
		if (_dissolveEffect != null)
		{
			tweenValue = 1f;
			_tweener = DOTween.To(() => tweenValue, delegate(float x)
			{
				tweenValue = x;
				_dissolveEffect.DissolveIntensity = x;
			}, 0f, 1.2f).SetEase(Ease.InOutQuad);
		}
	}

	public override void OnNetworkDespawn()
	{
		UnregisterListEvents();
		_objectManager.OnInteractiveObjectDespawned.OnNext(this);
		_objectManager.OnNpcDespawned.OnNext(this);
		base.OnNetworkDespawn();
	}

	private void RegisterListEvents()
	{
		RxNpcFunctionIds.Clear();
		foreach (int syncNpcFunctionId in _syncNpcFunctionIds)
		{
			RxNpcFunctionIds.Add(syncNpcFunctionId);
		}
		_syncNpcFunctionIds.OnListChanged += OnSyncNpcFunctionIdsChanged;
	}

	private void UnregisterListEvents()
	{
		_syncNpcFunctionIds.OnListChanged -= OnSyncNpcFunctionIdsChanged;
	}

	private void OnSyncNpcFunctionIdsChanged(NetworkListEvent<int> changeEvent)
	{
		switch (changeEvent.Type)
		{
		case NetworkListEvent<int>.EventType.Add:
			RxNpcFunctionIds.Add(changeEvent.Value);
			break;
		case NetworkListEvent<int>.EventType.Insert:
			RxNpcFunctionIds.Insert(changeEvent.Index, changeEvent.Value);
			break;
		case NetworkListEvent<int>.EventType.Remove:
			RxNpcFunctionIds.RemoveAt(changeEvent.Index);
			break;
		case NetworkListEvent<int>.EventType.Value:
			RxNpcFunctionIds[changeEvent.Index] = changeEvent.Value;
			break;
		case NetworkListEvent<int>.EventType.Clear:
			RxNpcFunctionIds.Clear();
			break;
		}
	}

	private void OnEventOccured(IBaseEvent baseEvent)
	{
		if (baseEvent is NpcFunctionUnlockEvent npcFunctionUnlockEvent)
		{
			OnNpcUnlock(npcFunctionUnlockEvent);
		}
		void OnNpcUnlock(NpcFunctionUnlockEvent npcFunctionUnlockEvent2)
		{
			npcFunctionUnlockEvent2.NpcFunctionIds.Select((int _) => TNpcFunction.Table[_]).Where((TNpcFunction _) => _.NpcId == _npcId).Where((TNpcFunction _) => RandomUtility.GetRandomBoolFromPercentage(_.Percent))
				.ToList()
				.ForEach(delegate(TNpcFunction _)
				{
					_syncNpcFunctionIds.Add(_.Id);
				});
		}
	}

	public void SetNavigationActive(bool isActive)
	{
		_isNavigationActive = isActive;
		_navigation.gameObject.SetActive(isActive);
		if (isActive)
		{
			_navigation.StartPoint = _gameManager.MyCharacterController?.transform;
			_navigation.EndPoint = base.transform;
		}
	}

	private void OnCharacterSpawned(CharacterController characterController)
	{
		if (characterController.ClientId == base.NetworkManager.LocalClientId && _isNavigationActive)
		{
			_isNavigationActive = true;
			_navigation.gameObject.SetActive(value: true);
			_navigation.StartPoint = _gameManager.MyCharacterController?.transform;
			_navigation.EndPoint = base.transform;
		}
	}

	public void DisableInteraction()
	{
		_isInteractableIndividually = false;
	}

	public void EnableInteraction()
	{
		_isInteractableIndividually = true;
	}

	public void SetSkin(string skinName)
	{
		if (!(_skeletonAnimation == null) && _skeletonAnimation.Skeleton.Data.FindSkin(skinName) != null)
		{
			_skeletonAnimation.Skeleton.SetSkin(skinName);
			_skeletonAnimation.Skeleton.SetSlotsToSetupPose();
		}
	}

	public void SetEmployeeId(int employeeId)
	{
		RxEmployeeId.Value = employeeId;
		string text = employeeId.ToString();
		if (!(_skeletonAnimation == null) && _skeletonAnimation.Skeleton.Data.FindSkin(text) != null)
		{
			_skeletonAnimation.Skeleton.SetSkin(text);
			_skeletonAnimation.Skeleton.SetSlotsToSetupPose();
		}
	}

	public void PlayAnimation(string animationName, bool isLoop)
	{
		if (!(_skeletonAnimation == null))
		{
			_skeletonAnimation.state.SetAnimation(0, animationName, isLoop);
		}
	}

	public void SetSkeletonScaleX(float scaleX)
	{
		if (!(_skeletonAnimation == null))
		{
			_skeletonAnimation.skeleton.ScaleX = scaleX;
		}
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		if (!_isInteractableIndividually)
		{
			return false;
		}
		if (_gameManager.IsHost)
		{
			if (HasFunction)
			{
				return true;
			}
		}
		else
		{
			int[] npcFunctionIds = NpcFunctionIds;
			if (npcFunctionIds != null && npcFunctionIds.Any((int id) => TNpcFunction.Table[id].GuestInteractable))
			{
				return true;
			}
		}
		if (_npcDialogManager.HasDialog(_npcId))
		{
			return true;
		}
		return RxNpcState.Value != ENpcState.None;
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
	}
}
