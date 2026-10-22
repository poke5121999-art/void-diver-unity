using DG.Tweening;
using System;
using System.Collections.Generic;
using GameCommon.Enum;
using UnityEngine;
using UnityEngine.Timeline;

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

	public float EntryEnd = 1f;

	[Header("Time Ranges: Exit Menu")]
	public float ExitStart = 1.18f;

	public float ExitEnd = 1.5f;

	[Header("Time Ranges: Begin Campaign")]
	public float BeginStart = 1.7f;

	public float BeginEnd = 2.8f;

	protected override void OnDestroy()
	{
		if (CurrentTween != null)
		{
			CurrentTween.Kill();
		}
	}

	public void ChangeAndPlayThemeAnimation(EThemeType theme)
	{
		ApplyTheme(theme);
		PlayRange(EntryStart, EntryEnd);
	}

	private void ApplyTheme(EThemeType theme)
	{
		if (Timeline == null)
		{
			Debug.LogWarning("[CampaignDetailBg] ApplyTheme abort. Timeline null");
			return;
		}
		TimelineAsset timelineAsset = Timeline.playableAsset as TimelineAsset;
		if (timelineAsset == null)
		{
			Debug.LogWarning(string.Format("[CampaignDetailBg] ApplyTheme abort. playableAsset is not TimelineAsset (actual={0})", Timeline.playableAsset));
			return;
		}
		string text = ResolveTrackName(theme);
		foreach (TrackAsset rootTrack in timelineAsset.GetRootTracks())
		{
			if (IsThemeTrack(rootTrack.name))
			{
				rootTrack.muted = rootTrack.name != text;
			}
		}
		Timeline.RebuildGraph();
	}

	private string ResolveTrackName(EThemeType theme)
	{
		for (int i = 0; i < _themeTrackEntries.Count; i++)
		{
			if (_themeTrackEntries[i].ThemeType == theme)
			{
				return _themeTrackEntries[i].TrackName;
			}
		}
		return _fallbackTrackName;
	}

	private bool IsThemeTrack(string trackName)
	{
		if (trackName == _fallbackTrackName)
		{
			return true;
		}
		for (int i = 0; i < _themeTrackEntries.Count; i++)
		{
			if (_themeTrackEntries[i].TrackName == trackName)
			{
				return true;
			}
		}
		return false;
	}

	[ContextMenu("PlayEntryDetail")]
	public void PlayEntryDetail()
	{
		PlayRange(EntryStart, EntryEnd);
	}

	[ContextMenu("PlayExitDetail")]
	public void PlayExitDetail()
	{
		PlayRange(ExitStart, ExitEnd);
	}

	[ContextMenu("PlayBeginCampaign")]
	public void PlayBeginCampaign()
	{
		PlayRange(BeginStart, BeginEnd);
	}
}
