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

	public float CurrentPlayingDuration => PlayingEndTime - PlayingStartTime;

	protected virtual void OnDestroy()
	{
		if (CurrentTween != null)
		{
			CurrentTween.Kill();
		}
	}

	protected void PlayRange(float start, float end)
	{
		if (CurrentTween != null)
		{
			CurrentTween.Kill();
		}
		IsPlayingRange = true;
		Timeline.time = start;
		Timeline.Play();
		PlayingStartTime = start;
		PlayingEndTime = end;
		CurrentTween = DOTween.To(() => Timeline.time, delegate(double x)
		{
			Timeline.time = x;
		}, end, PlayingEndTime - PlayingStartTime).SetEase(Ease.Linear).OnComplete(delegate
		{
			Timeline.time = end;
			Timeline.Pause();
			IsPlayingRange = false;
			OnRangeComplete();
		}).OnKill(delegate
		{
			IsPlayingRange = false;
		});
	}

	protected virtual void OnRangeComplete()
	{
	}

	public void StopPlayingAnimation()
	{
		if (CurrentTween != null)
		{
			CurrentTween.Kill();
		}
		Timeline.Pause();
		IsPlayingRange = false;
	}
}
