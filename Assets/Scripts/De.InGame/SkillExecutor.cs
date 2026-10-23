using System;
using System.Linq;
using GameCommon.Table;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class SkillExecutor : NetworkBase, IInteractiveObject, IActionExecutable, IActionStats
{
	public class Args
	{
		public int SkillExecutorId;
	}

	private Args _args;

	[SerializeField]
	[Range(0f, 5f)]
	private float _holdingTime = 0.5f;

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private SeriLogger _logger;

	private OutlineObject _outlineObject;

	private readonly NetworkVariable<int> _syncSkillExecutorId = new NetworkVariable<int>();

	private readonly NetworkVariable<int> _syncRemainCount = new NetworkVariable<int>();

	private CompositeDisposable _disposable;

	public NetworkBase NetworkBase => this;

	public Vector3 Position => base.transform.position;

	public float HoldingTime => _holdingTime;

	public EOutlineType OutlineType => (_syncRemainCount.Value > 0) ? EOutlineType.Default : EOutlineType.None;

	public string HoldingSfx => _holdingSfx;

	public string InteractionSfx => _interactionSfx;

	public GameObjectManager ObjectManager => _objectManager;

	public GameObject GameObject => base.gameObject;

	public int SkillExecutorId => _syncSkillExecutorId.Value;

	public int RemainCount => _syncRemainCount.Value;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 1f;

	ulong IInteractiveObject.NetworkObjectId => base.NetworkObjectId;

	ulong IActionExecutable.NetworkObjectId => base.NetworkObjectId;

	public bool CheckStressCondition(EStressState stressState)
	{
		return TSkillExecutor.Table[_syncSkillExecutorId.Value].InteractStressConditions.Contains(stressState);
	}

	protected override void Awake()
	{
		base.Awake();
		_outlineObject = GetComponent<OutlineObject>();
	}

	public void Init(Args args)
	{
		if (base.IsSpawned)
		{
			_logger.Error("WaveExecutor.Init() should be called before spawning the object.");
			return;
		}
		_args = args;
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		_syncSkillExecutorId.Value = _args.SkillExecutorId;
		_syncRemainCount.Value = TSkillExecutor.Table[_syncSkillExecutorId.Value].UseLimit;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		_syncRemainCount.OnValueChanged += OnRemainCountChanged;
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
		base.gameObject.name = string.Format("[SkillExecutor] {0}", _syncSkillExecutorId.Value);
	}

	public override void OnNetworkDespawn()
	{
		_syncRemainCount.OnValueChanged -= OnRemainCountChanged;
		_objectManager.OnInteractiveObjectDespawned.OnNext(this);
		_disposable?.Dispose();
		base.OnNetworkDespawn();
	}

	private void OnRemainCountChanged(int oldValue, int newValue)
	{
		if (newValue <= 0)
		{
			EOutlineType eOutlineType = ((_syncRemainCount.Value > 0) ? EOutlineType.Default : EOutlineType.None);
			_outlineObject.SetColor(eOutlineType.ToColor());
			_outlineObject.SetStroke(eOutlineType.ToStroke());
		}
	}

	public void Execute(CharacterModel casterModel)
	{
		AssertIfNotHost();
		if (_syncRemainCount.Value < 1)
		{
			return;
		}
		_syncRemainCount.Value--;
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		HitLayerInfo hitLayerInfo = casterModel.GetHitLayerInfo();
		ActionEvent[] actionEvents = TSkillExecutor.Table[_syncSkillExecutorId.Value].ActionEvents;
		if (actionEvents != null)
		{
			actionEvents.ForEach(delegate(ActionEvent actionEvent)
			{
				ExecuteActionEvent(actionEvent, hitLayerInfo);
			});
		}
	}

	private void ExecuteActionEvent(ActionEvent actionEvent, HitLayerInfo hitLayerInfo)
	{
		Observable.Timer(TimeSpan.FromSeconds(actionEvent.startTime)).Subscribe(delegate
		{
			this.ExecuteAction(actionEvent, hitLayerInfo);
		}).AddTo(_disposable);
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		return _syncRemainCount.Value > 0;
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

	public float GetStat(EStatType statType)
	{
		return 0f;
	}



}
