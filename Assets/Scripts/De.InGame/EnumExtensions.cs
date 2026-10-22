using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;

public static class EnumExtensions
{
	public static bool IsHitable(this EHitTarget hitTarget, HitLayerInfo ownerInfo, HitLayerInfo hitInfo, int targetHitLayer)
	{
		int ownerGroup = ownerInfo.HitLayer / 1000;
		int hitGroup = hitInfo.HitLayer / 1000;
		bool ownerHasFaction = ownerInfo.FactionId > 0;
		bool hitHasFaction = hitInfo.FactionId > 0;
		bool sameOrAllyFaction;
		if (ownerHasFaction && hitHasFaction)
		{
			int[] allyFactionIds = TFaction.Table[ownerInfo.FactionId].AllyFactionIds;
			sameOrAllyFaction = (allyFactionIds != null && allyFactionIds.Contains(hitInfo.FactionId)) || ownerInfo.FactionId == hitInfo.FactionId;
		}
		else
		{
			sameOrAllyFaction = false;
		}
		switch (hitTarget)
		{
		case EHitTarget.Owner:
			return ownerInfo.HitLayer == hitInfo.HitLayer;
		case EHitTarget.Ally:
			if (ownerGroup == hitGroup)
			{
				return !ownerHasFaction || !hitHasFaction || sameOrAllyFaction;
			}
			break;
		case EHitTarget.AllyExceptOwner:
			if (ownerGroup == hitGroup && (!ownerHasFaction || !hitHasFaction || sameOrAllyFaction))
			{
				return ownerInfo.HitLayer != hitInfo.HitLayer;
			}
			break;
		case EHitTarget.Enemy:
			if (ownerGroup == hitGroup)
			{
				if (ownerHasFaction && hitHasFaction)
				{
					return !sameOrAllyFaction;
				}
				return false;
			}
			return true;
		case EHitTarget.All:
			return true;
		case EHitTarget.AllExceptOwner:
			return ownerInfo.HitLayer != hitInfo.HitLayer;
		case EHitTarget.TargetOnly:
			return targetHitLayer == hitInfo.HitLayer;
		}
		return false;
	}
}
