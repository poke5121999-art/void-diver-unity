using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

public class CampaignBackgroundController : MonoBase
{
	[Header("Timeline")]
	public PlayableDirector Timeline;

	protected bool IsPlayingRange;

	protected Tween CurrentTween;

	protected float PlayingStartTime;

	protected float PlayingEndTime;

	public float CurrentPlayingDuration => 0f;

	protected virtual void OnDestroy()
	{
	}

	protected void PlayRange(float start, float end)
	{
	}

	protected virtual void OnRangeComplete()
	{
	}

	public void StopPlayingAnimation()
	{
	}
}
