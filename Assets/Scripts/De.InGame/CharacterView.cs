using System.Collections.Generic;
using DarkTonic.MasterAudio;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using UnityEngine;
using VContainer;

public class CharacterView : UnitView
{
	[SerializeField]
	private GameObject _modelAnchor;

	[SerializeField]
	private GameObject _uiAnchor;

	[SerializeField]
	private PlayerDirection directionIndicator;

	[SerializeField]
	private Collider interactionCollider;

	[Inject]
	private GameContext _gameContext;

	private readonly List<IInteractiveObject> _interactiveObjectList;

	private GamePlayer _player;

	private PlaneSightUnit _planeSightUnit;

	private bool _cachedIsSuper;

	private PlaySoundResult _currentSkillSoundResult;

	public ReactiveProperty<IInteractiveObject> RxInteractiveObject { get; }

	protected CharacterModel CharacterModel => null;

	public string PlayerName => null;

	public ulong ClientId => 0uL;

	public int CharacterId => 0;

	private TCharacter CharacterData => null;

	private bool IsGamePlaying => false;

	private PlaneSightUnit PlaneSightUnit => null;

	protected override string GetFootstepSfx(EGroundType groundType, bool isLeft)
	{
		return null;
	}

	public void Init(GamePlayer player)
	{
	}

	protected override void Start()
	{
	}

	protected override void FixedUpdate()
	{
	}

	private void OnStageStateChanged(EStageState state)
	{
	}

	protected override bool GetIsVisible(EActionState state)
	{
		return false;
	}

	protected override void OnStateChanged(EActionState state)
	{
	}

	protected override void PlayAnimationByActionState(EActionState state)
	{
	}

	private void OnInteractiveObjectChanged(IInteractiveObject previous, IInteractiveObject current)
	{
	}

	private void OnViewTargetClientIdChanged(ulong viewTargetId)
	{
	}

	private void UpdateFowRange()
	{
	}

	private void OnAimPosChanged(Vector3 value)
	{
	}

	private void OnLightOn(bool isLightOn)
	{
	}

	protected override void OnUsingSkillIdChanged(int skillId)
	{
	}

	protected virtual void OnEnter(Collider other)
	{
	}

	protected virtual void OnExit(Collider other)
	{
	}

	private IInteractiveObject GetClosestInteractableObject()
	{
		return null;
	}

	private bool HasObstacleBetween(IInteractiveObject obj)
	{
		return false;
	}

	public IInteractiveObject GetInteractableObject(ulong networkObjectId)
	{
		return null;
	}

	private void OnDisplaySkillIndicator(NtfSkillIndicator ntfSkillIndicator)
	{
	}

	private void OnCharacterModified(Character oldCharacter, Character newCharacter)
	{
	}

	public void UpdateSkins(bool isSuper = false)
	{
	}

	public override string GetIdleAnimation()
	{
		return null;
	}

	public override string GetMoveAnimation()
	{
		return null;
	}

	public override string GetDeathAnimation()
	{
		return null;
	}

	public override string GetReviveAnimation()
	{
		return null;
	}

	public string GetInteractionAnimation()
	{
		return null;
	}

	public override float GetMoveAnimationSpeed()
	{
		return 0f;
	}
}
