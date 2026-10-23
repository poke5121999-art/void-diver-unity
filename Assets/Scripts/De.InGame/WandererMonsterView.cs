using De.InGame.Unit;

public class WandererMonsterView : MonsterView
{
	public override string GetAggroAnimation()
	{
		return "aggro_start";
	}

	public override string GetReceiveAggroSpreadAnimation()
	{
		return "aggro_start";
	}

	public override string GetLostTargetAnimation()
	{
		return "aggro_end";
	}

	public override string GetIdleAnimation()
	{
		return base.MonsterModel.HasTarget ? "idle_2" : "idle_1";
	}

	public override string GetMoveAnimation()
	{
		return base.MonsterModel.HasTarget ? "walk_2" : "walk_1";
	}
}
