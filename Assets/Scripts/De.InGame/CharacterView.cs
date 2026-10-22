using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DarkTonic.MasterAudio;
using GameCommon.Enum;
using GameCommon.Table;
using Spine.Unity;
using UniRx;
using UniRx.Triggers;
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

	private readonly List<IInteractiveObject> _interactiveObjectList = new List<IInteractiveObject>();

	private GamePlayer _player;

	private PlaneSightUnit _planeSightUnit;

	private bool _cachedIsSuper;

	private PlaySoundResult _currentSkillSoundResult;

	public ReactiveProperty<IInteractiveObject> RxInteractiveObject { get; } = new ReactiveProperty<IInteractiveObject>();

	protected CharacterModel CharacterModel => UnitModel as CharacterModel;

	public string PlayerName => CharacterModel.RxPlayerName.Value;

	public ulong ClientId => CharacterModel.SyncClientId.Value;

	public int CharacterId => CharacterModel.CharacterId;

	private TCharacter CharacterData => TCharacter.Table[CharacterModel.CharacterId];

	private bool IsGamePlaying => GameManager.StageState == EStageState.Playing;

	private PlaneSightUnit PlaneSightUnit
	{
		get
		{
			if (_planeSightUnit == null)
			{
				_planeSightUnit = GetComponentInChildren<PlaneSightUnit>();
			}
			return _planeSightUnit;
		}
	}

	protected override string GetFootstepSfx(EGroundType groundType, bool isLeft)
	{
		TCharacterGroundMoveSfx tCharacterGroundMoveSfx = TCharacterGroundMoveSfx.Table[groundType];
		string text = ((!CharacterModel.TryRun) ? (isLeft ? tCharacterGroundMoveSfx.WalkLeft : tCharacterGroundMoveSfx.WalkRight) : (isLeft ? tCharacterGroundMoveSfx.RunLeft : tCharacterGroundMoveSfx.RunRight));
		return text.HasValue() ? text : null;
	}

	public void Init(GamePlayer player)
	{
		base.Init();
		_player = player;
		bool isLocal = CharacterModel.ClientId == GameManager.LocalClientId;
		directionIndicator.Init(UnitModel, new Color(isLocal ? 0f : 1f, isLocal ? 1f : 0f, 0f, 1f));
		GameManager.RxViewTargetClientId.Subscribe(OnViewTargetClientIdChanged).AddTo(this);
		RxInteractiveObject.Pairwise().Subscribe(delegate(Pair<IInteractiveObject> pair)
		{
			IInteractiveObject previous = pair.Previous;
			IInteractiveObject current = pair.Current;
			if (CharacterModel.ClientId != GameManager.LocalClientId)
			{
				return;
			}
			if ((UnityEngine.Object)previous != null)
			{
				OutlineObject componentInChildren = ((Component)previous).GetComponentInChildren<OutlineObject>();
				if (componentInChildren != null)
				{
					componentInChildren.SetColor(previous.OutlineColor);
					componentInChildren.SetStroke(previous.Stroke);
				}
			}
			if ((UnityEngine.Object)current != null)
			{
				OutlineObject componentInChildren2 = ((Component)current).GetComponentInChildren<OutlineObject>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.SetColor(EOutlineType.Interaction.ToColor());
					componentInChildren2.SetStroke(EOutlineType.Interaction.ToStroke());
				}
			}
		}).AddTo(this);
		interactionCollider = GetComponent<Collider>();
		interactionCollider.OnTriggerEnterAsObservable().Subscribe(OnEnter).AddTo(this);
		interactionCollider.OnTriggerExitAsObservable().Subscribe(OnExit).AddTo(this);
		SkeletonAnimation[] skeletonAnimations = SkeletonAnimations;
		foreach (SkeletonAnimation skeletonAnimation in skeletonAnimations)
		{
			skeletonAnimation.gameObject.SetActive(skeletonAnimation == CurrentSkeletonAnimation);
		}
		CurrentSkeletonAnimation.gameObject.SetActive(value: true);
		UpdateSkins();
		player.RxCharacter.Pairwise().Subscribe(delegate(Pair<Character> pair)
		{
			if (pair.Previous.Id == pair.Current.Id && pair.Previous.SkinPresetId == pair.Current.SkinPresetId && pair.Previous.WeaponId == pair.Current.WeaponId)
			{
				return;
			}
			UpdateSkins(_cachedIsSuper);
		}).AddTo(this);
	}

	protected override void Start()
	{
		base.Start();
		UnitModel.RxDeltaAimPos.Subscribe(OnAimPosChanged).AddTo(this);
		CharacterModel.RxIsLightOn.Subscribe(OnLightOn).AddTo(this);
		CharacterModel.RxTryRun.Where((bool _) => UnitModel.State == EActionState.Move).Subscribe(delegate
		{
			PlayAnimationByActionState(EActionState.Move);
		}).AddTo(this);
		GameManager.RxStageState.Subscribe(OnStageStateChanged).AddTo(this);
		GameChannel.Receive<NtfSkillIndicator>().Subscribe(OnDisplaySkillIndicator).AddTo(this);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		RxInteractiveObject.Value = GetClosestInteractableObject();
	}

	private void OnStageStateChanged(EStageState state)
	{
		OnStateChanged(UnitModel.State);
	}

	protected override bool GetIsVisible(EActionState state)
	{
		if (state == EActionState.Escaped || state == EActionState.StageReady || state == EActionState.Cutscene)
		{
			return false;
		}
		return GameManager.StageState == EStageState.Playing;
	}

	protected override void OnStateChanged(EActionState state)
	{
		bool isVisible = GetIsVisible(state);
		_uiAnchor.SetActive(isVisible);
		_modelAnchor.SetActive(isVisible);
		base.OnStateChanged(state);
		if (isVisible)
		{
			PlayAnimationByActionState(state);
			UpdateFowRange();
		}
	}

	protected override void PlayAnimationByActionState(EActionState state)
	{
		string text;
		switch (state)
		{
		case EActionState.Idle:
		case EActionState.CrowdControlled:
			text = GetIdleAnimation();
			break;
		case EActionState.Move:
			text = GetMoveAnimation();
			break;
		case EActionState.Dying:
			text = GetDeathAnimation();
			break;
		case EActionState.Revive:
			text = GetReviveAnimation();
			break;
		case EActionState.Interact:
			text = GetInteractionAnimation();
			break;
		default:
			text = string.Empty;
			break;
		}
		bool isLoop;
		float defaultSpeed = 1f;
		switch (state)
		{
		case EActionState.Idle:
			isLoop = true;
			break;
		case EActionState.Move:
			isLoop = true;
			defaultSpeed = GetMoveAnimationSpeed();
			break;
		case EActionState.CrowdControlled:
			isLoop = true;
			if (UnitModel.IsCrowdControlled(ECrowdControlType.Freeze))
			{
				defaultSpeed = 0f;
			}
			break;
		default:
			isLoop = state == EActionState.Interact;
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			PlayAnimationAsync(text, isLoop, 0f, null, defaultSpeed).Forget();
		}
	}

	private void OnInteractiveObjectChanged(IInteractiveObject previous, IInteractiveObject current)
	{
		if (CharacterModel.ClientId != GameManager.LocalClientId)
		{
			return;
		}
		if ((UnityEngine.Object)previous != null)
		{
			OutlineObject componentInChildren = ((Component)previous).GetComponentInChildren<OutlineObject>();
			if (componentInChildren != null)
			{
				componentInChildren.SetColor(previous.OutlineColor);
				componentInChildren.SetStroke(previous.Stroke);
			}
		}
		if ((UnityEngine.Object)current != null)
		{
			OutlineObject componentInChildren2 = ((Component)current).GetComponentInChildren<OutlineObject>();
			if (componentInChildren2 != null)
			{
				componentInChildren2.SetColor(EOutlineType.Interaction.ToColor());
				componentInChildren2.SetStroke(EOutlineType.Interaction.ToStroke());
			}
		}
	}

	private void OnViewTargetClientIdChanged(ulong viewTargetId)
	{
		PlaneSightUnit?.SetIsMine(CharacterModel.ClientId == viewTargetId);
		if (PlaneSightUnit != null && PlaneSightUnit.enabled)
		{
			TCharacter tCharacter = TCharacter.Table[CharacterModel.CharacterId];
			PlaneSightUnit.SetOptions(tCharacter.SightRange, tCharacter.SightBackRange, tCharacter.SightAngle);
		}
		LineTrajectoryIndicator.SkinnedMeshRenderer.enabled = CharacterModel.ClientId == viewTargetId;
		CircleEffectIndicator.SkinnedMeshRenderer.enabled = CharacterModel.ClientId == viewTargetId;
		CircleRangeIndicator.SkinnedMeshRenderer.enabled = CharacterModel.ClientId == viewTargetId;
		ConeEffectIndicator.SkinnedMeshRenderer.enabled = CharacterModel.ClientId == viewTargetId;
	}

	private void UpdateFowRange()
	{
		if (GameManager.RxViewTargetClientId.Value != CharacterModel.ClientId)
		{
			return;
		}
		TCharacter tCharacter = TCharacter.Table[CharacterModel.CharacterId];
		if (UnitModel.State == EActionState.Interact)
		{
			// TODO(decomp): range reads TConst.InteractSightAngle (same getter as the angle), InteractSightRange may be intended @0x18069beaf
			PlaneSightUnit.SetOptions(TConst.InteractSightAngle.FloatValue, TConst.InteractSightBackRange.FloatValue, TConst.InteractSightAngle.FloatValue);
		}
		else
		{
			PlaneSightUnit.SetOptions(tCharacter.SightRange, tCharacter.SightBackRange, tCharacter.SightAngle);
		}
	}

	private void OnAimPosChanged(Vector3 value)
	{
	}

	private void OnLightOn(bool isLightOn)
	{
		SoundManager.PlaySfx3D("FlashLight_OnOff", base.transform);
	}

	protected override void OnUsingSkillIdChanged(int skillId)
	{
		if (skillId == 0)
		{
			return;
		}
		TSkill tSkill = TSkill.Table[skillId];
		if (_currentSkillSoundResult != null)
		{
			_currentSkillSoundResult.StopSound();
			_currentSkillSoundResult = null;
		}
		if (!string.IsNullOrEmpty(tSkill.ActionSfx))
		{
			_currentSkillSoundResult = SoundManager.PlaySfx3D(tSkill.ActionSfx, base.transform);
		}
	}

	protected virtual void OnEnter(Collider other)
	{
		if (other.TryGetComponent<IInteractiveObject>(out var component))
		{
			_interactiveObjectList.Add(component);
			if (!HasObstacleBetween(component))
			{
				RxInteractiveObject.Value = GetClosestInteractableObject();
			}
		}
		if (GameManager.IsHost && other.TryGetComponent<ICollisionObject>(out var component2) && component2.CanTrigger(CharacterModel.ClientId))
		{
			GameHost.SendToAll(new NtfCollisionTriggerEnter
			{
				SourceClientId = CharacterModel.ClientId,
				TargetNetworkObjectId = component2.NetworkObjectId
			});
		}
	}

	protected virtual void OnExit(Collider other)
	{
		if (other.TryGetComponent<IInteractiveObject>(out var component) && _interactiveObjectList.Contains(component))
		{
			_interactiveObjectList.Remove(component);
		}
	}

	private IInteractiveObject GetClosestInteractableObject()
	{
		if (UnitModel.State != EActionState.Interact && UnitModel.State != EActionState.StageReady)
		{
			if (_interactiveObjectList.Count == 0)
			{
				return null;
			}
			_interactiveObjectList.RemoveAll((IInteractiveObject obj) => (obj as UnityEngine.Object) == null);
			Vector3 aimPos = base.transform.position + new Vector3(UnitModel.DeltaAimPos.x, 0f, UnitModel.DeltaAimPos.z);
			IOrderedEnumerable<IInteractiveObject> orderedEnumerable = _interactiveObjectList.Where((IInteractiveObject obj) => ObjectManager.InteractiveObjects.ContainsKey(obj.NetworkObjectId)).Where((IInteractiveObject obj) => !HasObstacleBetween(obj)).Where((IInteractiveObject obj) => obj.CanInteract(UnitModel))
				.OrderBy((IInteractiveObject obj) => Vector3.Distance(aimPos, obj.Position));
			return orderedEnumerable.Where((IInteractiveObject obj) => Vector3.Distance(aimPos, obj.Position) <= TConst.AimInteractPriorityRange.FloatValue).OrderBy(GetInteractivePriority).ThenBy((IInteractiveObject obj) => Vector3.Distance(aimPos, obj.Position))
				.FirstOrDefault() ?? orderedEnumerable.FirstOrDefault();
		}
		IInteractiveObject value = RxInteractiveObject.Value;
		return ((UnityEngine.Object)value == null) ? null : value;
		static int GetInteractivePriority(IInteractiveObject obj)
		{
			if (obj is CharacterBody)
			{
				return 0;
			}
			if (obj is MonsterBody)
			{
				return 2;
			}
			return 1;
		}
	}

	private bool HasObstacleBetween(IInteractiveObject obj)
	{
		if (PhysicsUtility.Raycast(CharacterModel.Position, obj.Position, Const.LayerMask.HIGH_OBSTACLE, out var hit))
		{
			return (hit.point - obj.Position).magnitude > 0.1f;
		}
		return false;
	}

	public IInteractiveObject GetInteractableObject(ulong networkObjectId)
	{
		return _interactiveObjectList.Where((IInteractiveObject obj) => obj != null).FirstOrDefault((IInteractiveObject obj) => obj.NetworkObjectId == networkObjectId);
	}

	private void OnDisplaySkillIndicator(NtfSkillIndicator ntfSkillIndicator)
	{
		if (ntfSkillIndicator.ClientId != CharacterModel.ClientId)
		{
			return;
		}
		switch (ntfSkillIndicator.IndicatorType)
		{
		case ESkillIndicatorType.None:
			LineTrajectoryIndicator.gameObject.SetActive(value: false);
			CircleRangeIndicator.gameObject.SetActive(value: false);
			CircleEffectIndicator.gameObject.SetActive(value: false);
			ConeEffectIndicator.gameObject.SetActive(value: false);
			break;
		case ESkillIndicatorType.SelectCastInRange:
			CircleRangeIndicator.Set(ntfSkillIndicator.CastRange);
			CircleEffectIndicator.Set(ntfSkillIndicator.EffectRange, ntfSkillIndicator.CastRange);
			break;
		case ESkillIndicatorType.LineTrajectory:
			LineTrajectoryIndicator.Set(ntfSkillIndicator.Length);
			break;
		case ESkillIndicatorType.Cone:
			CircleRangeIndicator.Set(ntfSkillIndicator.CastRange);
			ConeEffectIndicator.Set(ntfSkillIndicator.EffectRange, ntfSkillIndicator.Angle);
			break;
		}
	}

	private void OnCharacterModified(Character oldCharacter, Character newCharacter)
	{
		if (oldCharacter.Id == newCharacter.Id && oldCharacter.SkinPresetId == newCharacter.SkinPresetId && oldCharacter.WeaponId == newCharacter.WeaponId)
		{
			return;
		}
		UpdateSkins(_cachedIsSuper);
	}

	public void UpdateSkins(bool isSuper = false)
	{
		_cachedIsSuper = isSuper;
		int skinPresetId = _player.Character.SkinPresetId;
		int weaponId = CharacterModel.WeaponId;
		SkeletonAnimation[] skeletonAnimations = SkeletonAnimations;
		for (int i = 0; i < skeletonAnimations.Length; i++)
		{
			skeletonAnimations[i].skeleton.UpdateCharacterSkin(skinPresetId, weaponId, isSuper);
		}
	}

	public override string GetIdleAnimation()
	{
		return UnitModel.IsNonCombat ? "default/idle" : "battle/idle";
	}

	public override string GetMoveAnimation()
	{
		if (CharacterModel.TryRun)
		{
			return UnitModel.IsNonCombat ? "default/run" : "battle/run";
		}
		return UnitModel.IsNonCombat ? "default/walk" : "battle/walk";
	}

	public override string GetDeathAnimation()
	{
		return "battle/death";
	}

	public override string GetReviveAnimation()
	{
		return "default/idle";
	}

	public string GetInteractionAnimation()
	{
		IInteractiveObject value = RxInteractiveObject.Value;
		if (value == null)
		{
			return "default/idle";
		}
		if (value is RewardBox || value is MimicObject || value is InteractiveTrigger)
		{
			return "battle/search";
		}
		return "default/idle";
	}

	public override float GetMoveAnimationSpeed()
	{
		float moveSpeed = UnitModel.MoveSpeed;
		if (moveSpeed >= TConst.MoveSpeedThresholdForRun.FloatValue)
		{
			return UnitModel.MoveSpeed / TConst.MoveSpeedThresholdForRun.FloatValue;
		}
		float num = UnitModel.MoveSpeed / TCharacter.Table[CharacterModel.CharacterId].WalkAnimation1xSpeed;
		float floatValue = TConst.MinWalkAnimationSpeed.FloatValue;
		float floatValue2 = TConst.MaxWalkAnimationSpeed.FloatValue;
		if (num < floatValue)
		{
			return floatValue;
		}
		if (num > floatValue2)
		{
			return floatValue2;
		}
		return num;
	}
}
