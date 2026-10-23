using System;
using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class HelpPopupPresenter : GamePopupPresenter
{
	[View]
	private HelpPopupView _view;

	[Inject]
	private SoundManager _soundManager;

	private int _currentPageIndex;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private int PageCount => _view.Pages.Length;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	public override string OpenSfx => "Paper";

	public override string CloseSfx => "Paper";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		for (int i = 0; i < _view.PageSlots.Length; i++)
		{
			if (i < _view.Pages.Length)
			{
				_view.PageSlots[i].GetComponent<Presenter>().Inject(new HelpPopupPageSlotModel
				{
					PageNumber = i + 1
				});
			}
			else
			{
				_view.PageSlots[i].SetActive(false);
			}
		}
		_currentPageIndex = 0;
		OnPageChanged(0);
		_view.Background.onClick.AsObservable().Subscribe(delegate
		{
			TryNextPage();
		}).AddTo(this);
		_view.NextPageButton.onClick.AsObservable().Subscribe(delegate
		{
			TryNextPage();
		}).AddTo(this);
		_view.PrevPageButton.onClick.AsObservable().Subscribe(delegate
		{
			TryPrevPage();
		}).AddTo(this);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager.UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		GameInputManager.UiActions.Skip.OnClickAsObservable().Subscribe(delegate
		{
			TryNextPage();
		}).AddTo(InputDisposable);
		GameInputManager.UiPadActions.TabLeft.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnTabLeft();
		}).AddTo(InputDisposable);
		GameInputManager.UiPadActions.TabRight.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnTabRight();
		}).AddTo(InputDisposable);
	}

	private void OnTabLeft()
	{
		if (_view.Pages.Length > 0 && _currentPageIndex > 0)
		{
			_currentPageIndex--;
			OnPageChanged(_currentPageIndex);
		}
	}

	private void OnTabRight()
	{
		if (_view.Pages.Length > 0 && _currentPageIndex < _view.Pages.Length - 1)
		{
			_currentPageIndex++;
			OnPageChanged(_currentPageIndex);
		}
	}

	private void TryNextPage()
	{
		if (_view.Pages.Length > 0)
		{
			if (_currentPageIndex >= _view.Pages.Length - 1)
			{
				Close();
				return;
			}
			_currentPageIndex++;
			OnPageChanged(_currentPageIndex);
		}
	}

	private void TryPrevPage()
	{
		if (_view.Pages.Length > 0 && _currentPageIndex > 0)
		{
			_currentPageIndex--;
			OnPageChanged(_currentPageIndex);
		}
	}

	protected void OnPageChanged(int page)
	{
		_soundManager.PlaySfx("Paper");
		if (_view.Pages.Length == 0)
		{
			return;
		}
		page = ClampPage(page);
		for (int i = 0; i < _view.Pages.Length; i++)
		{
			GameObject gameObject = _view.Pages[i];
			if (gameObject != null && gameObject.activeSelf != (i == page))
			{
				gameObject.SetActive(i == page);
			}
		}
		for (int j = 0; j < _view.PageSlots.Length; j++)
		{
			GameObject gameObject2 = _view.PageSlots[j];
			if (gameObject2 != null)
			{
				gameObject2.GetComponent<HelpPopupPageSlotPresenter>().SetSelected(j == page);
			}
		}
		_view.PrevPageButton.gameObject.SetActive(page > 0);
		_view.NextPageButton.gameObject.SetActive(page < _view.Pages.Length - 1);
	}

	private int ClampPage(int page)
	{
		if (_view.Pages.Length < 1 || page < 0)
		{
			return 0;
		}
		if (page < _view.Pages.Length)
		{
			return page;
		}
		return _view.Pages.Length - 1;
	}
}
