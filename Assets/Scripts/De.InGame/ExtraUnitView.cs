using GameCommon.Enum;
using UnityEngine;

public class ExtraUnitView : UnitView
{
	private ExtraUnitModel ExtraUnitModel => null;

	public int ExtraUnitId => 0;

	public Vector3 SpawnPosition { get; private set; }

	protected override string GetFootstepSfx(EGroundType groundType, bool isLeft)
	{
		return null;
	}

	public override void Init()
	{
	}

	protected override void OnStateChanged(EActionState state)
	{
	}

	protected override void PlayAnimationByActionState(EActionState state)
	{
	}

	protected override void OnUsingSkillIdChanged(int skillId)
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

	public override float GetMoveAnimationSpeed()
	{
		return 0f;
	}

	protected virtual void HideObject()
	{
	}
}
