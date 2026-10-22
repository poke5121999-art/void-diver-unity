using System.Collections.Generic;
using GameCommon.Enum;
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

	private readonly Dictionary<ulong, EAction> _currentActions;

	private float _giveStressAvailableTime;

	private float TimeNow => 0f;

	public NetworkBase NetworkBase => null;

	public Vector3 Position => default;

	public EActionState State => EActionState.None;

	public EOutlineType OutlineType => EOutlineType.None;

	public string HoldingSfx => null;

	public string InteractionSfx => null;

	public float HoldingTime => 0f;

	private EAction AvailableAction => EAction.None;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private bool CanInteract(UnitModel sourceModel)
	{
		return false;
	}

	bool IInteractiveObject.CanInteract(UnitModel sourceModel)
	{
		return false;
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

	public void Execute(UnitModel sourceModel)
	{
	}
}
