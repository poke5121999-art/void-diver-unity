using System.Collections.Generic;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;
using VContainer;

public class CharacterBody : MonoBase, IInteractiveObject
{
	private enum EAction
	{
		None = 0,
		Revive = 1,
		AbsorbStress = 2
	}

	[SerializeField]
	private CharacterModel _characterModel;

	[SerializeField]
	private string _holdingSfx;

	[SerializeField]
	private string _interactionSfx;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameObjectManager _objectManager;

	private readonly Dictionary<ulong, EAction> _currentActions = new Dictionary<ulong, EAction>();

	private float _giveStressAvailableTime;

	private float TimeNow => _gameManager.TimeNow;

	public NetworkBase NetworkBase => _characterModel;

	public Vector3 Position => base.transform.position;

	public EActionState State => _characterModel.State;

	public EOutlineType OutlineType => EOutlineType.Default;

	public string HoldingSfx => _holdingSfx;

	public string InteractionSfx => _interactionSfx;

	public float HoldingTime
	{
		get
		{
			return AvailableAction switch
			{
				EAction.Revive => TConst.ReviveInteractionTime.FloatValue,
				EAction.AbsorbStress => TConst.AbsorbStressInteractionTime.FloatValue,
				_ => 0f,
			};
		}
	}

	private EAction AvailableAction
	{
		get
		{
			switch (_characterModel.State)
			{
			case EActionState.Dying:
			case EActionState.Escaped:
			case EActionState.StageReady:
			case EActionState.Cutscene:
				return EAction.None;
			case EActionState.Dead:
				return EAction.Revive;
			default:
				return (_characterModel.StressState > EStressState.Alert) ? EAction.AbsorbStress : EAction.None;
			}
		}
	}

	private void Start()
	{
		_objectManager.OnInteractiveObjectSpawned.OnNext(this);
	}

	private void OnDestroy()
	{
		_objectManager.OnInteractiveObjectDespawned.OnNext(this);
	}

	private bool CanInteract(UnitModel sourceModel)
	{
		switch (AvailableAction)
		{
		case EAction.Revive:
			return true;
		default:
			return false;
		case EAction.AbsorbStress:
			if ((float)TConst.AbsorbStressLimit.IntValue < sourceModel.Stress)
			{
				return false;
			}
			return TimeNow > _giveStressAvailableTime;
		}
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		return CanInteract(sourceModel);
	}

	void IInteractiveObject.OnStartHolding(ulong sourceClientId)
	{
		_currentActions[sourceClientId] = AvailableAction;
	}

	void IInteractiveObject.OnStartInteraction(ulong sourceClientId)
	{
	}

	void IInteractiveObject.OnEndInteraction(ulong sourceClientId)
	{
		if (_currentActions.GetValueOrDefault(sourceClientId) == EAction.AbsorbStress)
		{
			_giveStressAvailableTime = TimeNow + TConst.AbsorbStressCoolTime.FloatValue;
		}
	}

	public void Execute(UnitModel sourceModel)
	{
		_gameManager.AssertIfNotHost();
		if (!(sourceModel is CharacterModel characterModel))
		{
			return;
		}
		EAction value = _currentActions.GetValueOrDefault(characterModel.ClientId);
		if (value == EAction.None || value != AvailableAction || !CanInteract(sourceModel))
		{
			return;
		}
		switch (value)
		{
		case EAction.Revive:
			_characterModel.GetComponent<UnitController>().HandleUnitEvent(new ReviveEvent
			{
				SourceModel = sourceModel
			});
			break;
		case EAction.AbsorbStress:
		{
			int intValue = TConst.AbsorbStressAmount.IntValue;
			_characterModel.GetComponent<UnitController>().HandleUnitEvent(new StressRecoveryEvent
			{
				IsFixed = true,
				Amount = intValue
			});
			sourceModel.GetComponent<UnitController>().HandleUnitEvent(new StressDamageEvent
			{
				SourceModel = sourceModel,
				Amount = intValue,
				HitPosition = sourceModel.Position,
				IsFixed = true
			});
			break;
		}
		}
	}
}
