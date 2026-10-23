using GameCommon.Table;

public static class UserExtensions
{
	public static bool CanLevelUp(this Profile profile)
	{
		int nextLevel = profile.Level + 1;
		if (TConst.LevelMax.IntValue < nextLevel)
		{
			return false;
		}
		TLevel level = TLevel.Table[nextLevel];
		if (profile.Gold < level.NeedGold)
		{
			return false;
		}
		if (profile.Coin < level.NeedCoin)
		{
			return false;
		}
		if (profile.Exp < level.NeedExp)
		{
			return false;
		}
		return true;
	}
}
