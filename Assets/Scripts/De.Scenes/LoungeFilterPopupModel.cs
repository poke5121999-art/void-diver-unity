using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using NemoLib.Mvp;
using UnityEngine;

public class LoungeFilterPopupModel : Model
{
	public bool BlockOtherAreas;

	public HashSet<ERoomDifficulty> BlockedDifficulties = new HashSet<ERoomDifficulty>();

	public int MinHostLevel = 1;

	public ERoomPingFilterType MinPingFilterType = ERoomPingFilterType.All;

	public bool ShowPlayingLounge;

	public LoungeFilterPopupModel Clone()
	{
		LoungeFilterPopupModel loungeFilterPopupModel = new LoungeFilterPopupModel();
		loungeFilterPopupModel.BlockOtherAreas = BlockOtherAreas;
		loungeFilterPopupModel.BlockedDifficulties = new HashSet<ERoomDifficulty>(BlockedDifficulties);
		loungeFilterPopupModel.MinHostLevel = MinHostLevel;
		loungeFilterPopupModel.MinPingFilterType = MinPingFilterType;
		loungeFilterPopupModel.ShowPlayingLounge = ShowPlayingLounge;
		return loungeFilterPopupModel;
	}

	public void Save()
	{
		PlayerPrefs.SetInt("LoungeFilter_BlockOtherAreas", BlockOtherAreas ? 1 : 0);
		PlayerPrefs.SetString("LoungeFilter_BlockedDifficulties", string.Join("|", BlockedDifficulties.Select((ERoomDifficulty d) => d.ToString())));
		PlayerPrefs.SetInt("LoungeFilter_MinHostLevel", MinHostLevel);
		PlayerPrefs.SetString("LoungeFilter_MinPingFilterType", MinPingFilterType.ToString());
		PlayerPrefs.SetInt("LoungeFilter_ShowPlayingLounge", ShowPlayingLounge ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void Load()
	{
		BlockOtherAreas = PlayerPrefs.GetInt("LoungeFilter_BlockOtherAreas", 0) == 1;
		string @string = PlayerPrefs.GetString("LoungeFilter_BlockedDifficulties", "");
		BlockedDifficulties.Clear();
		if (!string.IsNullOrEmpty(@string))
		{
			string[] array = @string.Split("|");
			for (int i = 0; i < array.Length; i++)
			{
				if (Enum.TryParse<ERoomDifficulty>(array[i], out var result))
				{
					BlockedDifficulties.Add(result);
				}
			}
		}
		MinHostLevel = PlayerPrefs.GetInt("LoungeFilter_MinHostLevel", 1);
		MinPingFilterType = (Enum.TryParse<ERoomPingFilterType>(PlayerPrefs.GetString("LoungeFilter_MinPingFilterType", ERoomPingFilterType.All.ToString()), out var result2) ? result2 : ERoomPingFilterType.All);
		ShowPlayingLounge = PlayerPrefs.GetInt("LoungeFilter_ShowPlayingLounge", 0) == 1;
	}
}
