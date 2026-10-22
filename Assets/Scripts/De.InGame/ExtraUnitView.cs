using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

public class ExtraUnitView : UnitView
{
	private ExtraUnitModel ExtraUnitModel => base.UnitModel as ExtraUnitModel;

	public int ExtraUnitId => ExtraUnitModel.SyncExtraUnitId.Value;

	public Vector3 SpawnPosition { get; private set; }

	protected override string GetFootstepSfx(EGroundType groundType, bool isLeft)
	{
		return null;
	}

	public override void Init()
	{
		base.Init();
		SpawnPosition = base.gameObject.transform.position;
	}

	protected override void OnStateChanged(EActionState state)
	{
		base.OnStateChanged(state);
		PlayAnimationByActionState(state);
		switch (state)
		{
		case EActionState.Dying:
			SoundManager.PlaySfx3D(TExtraUnit.Table[ExtraUnitModel.ExtraUnitId].DyingSfx, base.transform.position);
			break;
		case EActionState.ReservedDespawn:
			HideObject();
			break;
		}
	}

	protected override void PlayAnimationByActionState(EActionState state)
	{
		string text;
		switch (state)
		{
		case EActionState.Idle:
		case EActionState.CrowdControlled:
		case EActionState.Pause:
			text = GetIdleAnimation();
			break;
		case EActionState.Move:
		case EActionState.MoveToTarget:
		case EActionState.Wander:
		case EActionState.Flee:
		case EActionState.Avoid:
			text = GetMoveAnimation();
			break;
		case EActionState.Dying:
			text = GetDeathAnimation();
			break;
		default:
			text = string.Empty;
			break;
		}
		bool isLoop = state == EActionState.Idle || state == EActionState.Move || state == EActionState.CrowdControlled || state == EActionState.MoveToTarget || state == EActionState.Wander || state == EActionState.Flee;
		if (!string.IsNullOrEmpty(text))
		{
			PlayAnimationAsync(text, isLoop, 0f, null).Forget();
		}
	}

	protected override void OnUsingSkillIdChanged(int skillId)
	{
		if (skillId != 0 && !string.IsNullOrEmpty(TSkill.Table[skillId].ActionSfx))
		{
			SoundManager.PlaySfx3D(TSkill.Table[skillId].ActionSfx, base.transform);
		}
	}

	public override string GetIdleAnimation()
	{
		return "idle";
	}

	public override string GetMoveAnimation()
	{
		return "walk";
	}

	public override string GetDeathAnimation()
	{
		return "death";
	}

	public override string GetReviveAnimation()
	{
		return "revive";
	}

	public override float GetMoveAnimationSpeed()
	{
		return 1f;
	}

	protected virtual void HideObject()
	{
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(false);
		}
	}
}
