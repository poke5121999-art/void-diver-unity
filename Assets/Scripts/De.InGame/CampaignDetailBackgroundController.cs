using System;
using System.Collections.Generic;
using GameCommon.Enum;
using UnityEngine;

public class CampaignDetailBackgroundController : CampaignBackgroundController
{
	[Serializable]
	public struct ThemeTrackEntry
	{
		public EThemeType ThemeType;

		public string TrackName;
	}

	[Header("Theme Track Mapping")]
	[SerializeField]
	private List<ThemeTrackEntry> _themeTrackEntries;

	[SerializeField]
	private string _fallbackTrackName;

	[Header("Time Ranges: Entry Menu")]
	public float EntryStart;

	public float EntryEnd;

	[Header("Time Ranges: Exit Menu")]
	public float ExitStart;

	public float ExitEnd;

	[Header("Time Ranges: Begin Campaign")]
	public float BeginStart;

	public float BeginEnd;

	protected override void OnDestroy()
	{
	}

	public void ChangeAndPlayThemeAnimation(EThemeType theme)
	{
	}

	private void ApplyTheme(EThemeType theme)
	{
	}

	private string ResolveTrackName(EThemeType theme)
	{
		return null;
	}

	private bool IsThemeTrack(string trackName)
	{
		return false;
	}

	[ContextMenu("PlayEntryDetail")]
	public void PlayEntryDetail()
	{
	}

	[ContextMenu("PlayExitDetail")]
	public void PlayExitDetail()
	{
	}

	[ContextMenu("PlayBeginCampaign")]
	public void PlayBeginCampaign()
	{
	}
}
