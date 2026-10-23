using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NoteSystemPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private NoteSystemPopupModel _model;

	[View]
	private NoteSystemPopupView _view;

	private NoteSystemPanelPresenter _panelPresenter;

	private readonly Queue<string> _pendingKeys = new Queue<string>();

	private CancellationToken _ct;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override string OpenSfx => string.Format("Open{0}", _model.NoteType);

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_ct = this.GetCancellationTokenOnDestroy();
		if (_view.BaseBgDimmer != null)
		{
			// TODO(decomp): GetComponent<T> type argument not recoverable (a Graphic with virtual color), Image assumed @0x180831d0f
			Image component = _view.BaseBgDimmer.GetComponent<Image>();
			if (component != null)
			{
				Color color = component.color;
				color.a = Mathf.Clamp01(_model.BackGroundAlpha);
				component.color = color;
			}
		}
		_view.BaseBgDimmer.OnClick().Subscribe(delegate
		{
			OnAdvanceOrCancel();
		}).AddTo(this);
		_panelPresenter = InjectPanel();
		if (_model.Texts == null || _model.Texts.Length < 1)
		{
			Close();
			return;
		}
		for (int i = 1; i < _model.Texts.Length; i++)
		{
			if (!string.IsNullOrEmpty(_model.Texts[i]))
			{
				_pendingKeys.Enqueue(_model.Texts[i]);
			}
		}
		if (_panelPresenter != null)
		{
			_panelPresenter.PlayMessageAsync(_ct, _model.Texts[0]).Forget();
		}
	}

	private void PlayNextIfAny()
	{
		string text;
		do
		{
			if (_panelPresenter == null || _pendingKeys.Count == 0)
			{
				Close();
				return;
			}
			text = _pendingKeys.Dequeue();
		}
		while (string.IsNullOrEmpty(text));
		_panelPresenter.PlayMessageAsync(_ct, text).Forget();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnAdvanceOrCancel();
		}).AddTo(InputDisposable);
		UiActions.Skip.OnClickAsObservable().Subscribe(delegate
		{
			OnAdvanceOrCancel();
		}).AddTo(InputDisposable);
		UiActions.NextFlow.OnPressStartAsObservable().SelectMany((InputAction.CallbackContext _) => Observable.Interval(TimeSpan.FromSeconds(0.05f)).TakeUntil(UiActions.NextFlow.OnPressEndAsObservable())).Subscribe(delegate
		{
			OnAdvanceOrCancel();
		}).AddTo(InputDisposable);
	}

	private NoteSystemPanelPresenter InjectPanel()
	{
		GameObject[] notes = _view.Notes;
		if (notes == null || notes.Length == 0)
		{
			return null;
		}
		int num = (int)(_model.NoteType - 1);
		if (num < 0 || num >= notes.Length)
		{
			num = 0;
		}
		for (int i = 0; i < notes.Length; i++)
		{
			notes[i].SetActive(i == num);
		}
		NoteSystemPanelPresenter component = notes[num].GetComponent<NoteSystemPanelPresenter>();
		if (component == null)
		{
			return null;
		}
		string[] texts = _model.Texts;
		component.Inject(new NoteSystemPanelModel
		{
			NoteText = ((texts == null || texts.Length < 1) ? string.Empty : texts[0])
		});
		return component;
	}

	private void OnAdvanceOrCancel()
	{
		if (_panelPresenter == null)
		{
			Close();
		}
		else if (!_panelPresenter.IsWriting)
		{
			if (_pendingKeys.Count > 0)
			{
				PlayNextIfAny();
			}
			else
			{
				Close();
			}
		}
		else
		{
			_panelPresenter.SkipTypewriter();
		}
	}

	private void OnConfirm()
	{
		Close();
	}
}
