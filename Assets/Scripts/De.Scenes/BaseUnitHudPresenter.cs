using System;
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

	private readonly HashSet<string> _recentBubbleTexts = new HashSet<string>();

	private readonly HashSet<string> _recentMonologueTexts = new HashSet<string>();

	private CompositeDisposable _monologueDisposable;

	private readonly Queue<string> _monologueQueue = new Queue<string>();

	private bool _isMonologuePlaying;

	protected abstract GameObject Bubble { get; }

	protected abstract TypewriterByCharacter BubbleTextWriter { get; }

	protected abstract TextMeshProUGUI Monologue { get; }

	protected virtual void OnDestroy()
	{
		_bubbleTextDisposable?.Dispose();
		_monologueDisposable?.Dispose();
		_recentBubbleTexts.Clear();
		_monologueQueue.Clear();
		_isMonologuePlaying = false;
	}

	protected void ShowBubbleText(string text)
	{
		if (string.IsNullOrEmpty(text) || _recentBubbleTexts.Contains(text))
		{
			return;
		}
		_recentBubbleTexts.Add(text);
		Observable.Timer(TimeSpan.FromSeconds(2.0)).Subscribe(delegate
		{
			_recentBubbleTexts.Remove(text);
		}).AddTo(this);
		_bubbleTextDisposable?.Dispose();
		_bubbleTextDisposable = new CompositeDisposable();
		Bubble.SetActive(value: true);
		if (TrySetWriterText(BubbleTextWriter, text))
		{
			BubbleTextWriter.onTextShowed.AsObservable().Subscribe(delegate
			{
				HideBubbleTextAfterDelay();
			}).AddTo(_bubbleTextDisposable);
		}
		else
		{
			Bubble.SetActive(value: false);
		}
	}

	private bool TrySetWriterText(TypewriterByCharacter writer, string text)
	{
		if (writer == null || !writer.isActiveAndEnabled)
		{
			return false;
		}
		writer.ShowText(text);
		writer.StartShowingText(restart: true);
		return true;
	}

	private void HideBubbleTextAfterDelay()
	{
		Observable.Timer(TimeSpan.FromSeconds(BUBBLE_DELAY_AFTER_WRITER_END)).Subscribe(delegate
		{
			Bubble.SetActive(value: false);
			if (_bubbleTextDisposable != null)
			{
				_bubbleTextDisposable.Dispose();
				_bubbleTextDisposable = null;
			}
		}).AddTo(_bubbleTextDisposable);
	}

	protected void ShowMonologueText(string text)
	{
		if (!string.IsNullOrEmpty(text) && !_recentMonologueTexts.Contains(text))
		{
			AddRecentMonologueText(text);
			_monologueQueue.Enqueue(text);
			if (!_isMonologuePlaying)
			{
				PlayNextMonologue();
			}
		}
	}

	private void AddRecentMonologueText(string text)
	{
		_recentMonologueTexts.Add(text);
		Observable.Timer(TimeSpan.FromSeconds(2.0)).Subscribe(delegate
		{
			_recentMonologueTexts.Remove(text);
		}).AddTo(this);
	}

	private void PlayNextMonologue()
	{
		Monologue.gameObject.SetActive(value: false);
		Monologue.text = string.Empty;
		if (_monologueQueue.Count == 0)
		{
			_isMonologuePlaying = false;
			return;
		}
		string text = _monologueQueue.Dequeue();
		_isMonologuePlaying = true;
		_monologueDisposable?.Dispose();
		_monologueDisposable = new CompositeDisposable();
		Monologue.text = text;
		TypewriterByCharacter component = Monologue.gameObject.GetComponent<TypewriterByCharacter>();
		if (component != null && component.isActiveAndEnabled)
		{
			component.StopShowingText();
			TrySetWriterText(component, text);
		}
		Monologue.gameObject.SetActive(value: true);
		Observable.Timer(TimeSpan.FromSeconds(MONOLOGUE_DURATION)).Subscribe(delegate
		{
			_monologueDisposable.Dispose();
			_monologueDisposable = null;
			PlayNextMonologue();
		}).AddTo(_monologueDisposable);
	}
}
