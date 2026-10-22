using System.Collections.Generic;
using GameCommon.Enum;
using NemoLib.Mvp;

public class LoungeFilterPopupModel : Model
{
	public bool BlockOtherAreas;

	public HashSet<ERoomDifficulty> BlockedDifficulties;

	public int MinHostLevel;

	public ERoomPingFilterType MinPingFilterType;

	public bool ShowPlayingLounge;

	public LoungeFilterPopupModel Clone()
	{
		return null;
	}

	public void Save()
	{
	}

	public void Load()
	{
	}
}
