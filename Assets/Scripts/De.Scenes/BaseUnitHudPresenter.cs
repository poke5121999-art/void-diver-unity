using System.Collections.Generic;
using Febucci.UI;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;

public abstract class BaseUnitHudPresenter : Presenter
{
	private const float BUBBLE_DELAY_AFTER_WRITER_END = 1f;

	private const float MONOLOGUE_DURATION = 3f;

	private CompositeDisposable _bubbleTextDisposable;

	private readonly HashSet<string> _recentBubbleTexts;

	private readonly HashSet<string> _recentMonologueTexts;

	private CompositeDisposable _monologueDisposable;

	private readonly Queue<string> _monologueQueue;

	private bool _isMonologuePlaying;

	protected abstract GameObject Bubble { get; }

	protected abstract TypewriterByCharacter BubbleTextWriter { get; }

	protected abstract TextMeshProUGUI Monologue { get; }

	protected virtual void OnDestroy()
	{
	}

	protected void ShowBubbleText(string text)
	{
	}

	private bool TrySetWriterText(TypewriterByCharacter writer, string text)
	{
		return false;
	}

	private void HideBubbleTextAfterDelay()
	{
	}

	protected void ShowMonologueText(string text)
	{
	}

	private void AddRecentMonologueText(string text)
	{
	}

	private void PlayNextMonologue()
	{
	}
}
