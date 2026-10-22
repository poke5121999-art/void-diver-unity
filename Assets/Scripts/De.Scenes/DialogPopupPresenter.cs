using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Febucci.UI;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public class DialogPopupPresenter : GamePopupPresenter
{
	private enum EState
	{
		None = 0,
		Started = 1,
		Ended = 2
	}

	[InjectModel]
	private DialogPopupModel _model;

	[View]
	private DialogPopupView _view;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameManager _gameManager;

	private TypewriterByCharacter _typeWriter;

	private bool _isWritingText;

	private readonly Queue<DialogEntry> _dialogQueue = new Queue<DialogEntry>();

	private DialogEntry _currentEntry;

	private bool _canClose;

	private bool _needSelect;

	private const float _defaultBgFadeDuration = 0.5f;

	private EState _state;

	private CancellationTokenSource _delayCts;

	private bool _isDelayActive;

	private int _selectionPageIndex;

	private bool _focusLastAfterRefresh;

	private CompositeDisposable _selectionDisposable;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => GameInputManager.UiKeyboardActions;

	private bool HasSelection => _currentEntry?.SelectableMessageDatas != null && _currentEntry.SelectableMessageDatas.Length > 0;

	private Animator Animator => base.gameObject.GetComponent<Animator>();

	private Animator FrameAnimator => _view.Frame.GetComponent<Animator>();

	private int SelectionPageCount
	{
		get
		{
			if (_currentEntry?.SelectableMessageDatas == null)
			{
				return 0;
			}
			return Mathf.CeilToInt((float)_currentEntry.SelectableMessageDatas.Length / (float)_view.Selections.Length);
		}
	}

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		IsBlurEffect = _gameManager.IsInLounge;
		_view.Background.OnClick().Where((Unit _) => !_needSelect).Subscribe(delegate
		{
			OnNext(playSfx: true);
		})
			.AddTo(this);
		_view.PageButton_PrevPageButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnSelectionPageChange(-1);
		}).AddTo(this);
		_view.PageButton_NextPageButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnSelectionPageChange(1);
		}).AddTo(this);
		_typeWriter = _view.Message.GetComponent<TypewriterByCharacter>();
		_typeWriter.onTextShowed.AsObservable().Subscribe(delegate
		{
			OnTextWriterEnd();
		}).AddTo(this);
		if (_model.DisplayPlayerInfo)
		{
			_view.UserInfo.SetActive(value: true);
			_playerManager.RxProfile.Subscribe(OnProfileChanged).AddTo(this);
		}
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		if (GameInputManager != null)
		{
			UiActions.Skip.OnClickAsObservable().Where((InputAction.CallbackContext _) => !_needSelect).Subscribe(delegate
			{
				OnNext(playSfx: true);
			})
				.AddTo(InputDisposable);
			UiActions.Escape.OnClickAsObservable().Where((InputAction.CallbackContext _) => _currentEntry != null && _currentEntry.AllowEscapeClose && _currentEntry.SelectableMessageDatas != null && _currentEntry.SelectableMessageDatas.Length > 0 && _currentEntry.SelectableMessageDatas.Length >= 1).Subscribe(delegate
			{
				OnSelectButtonClick(_currentEntry.SelectableMessageDatas.Length - 1);
			})
				.AddTo(InputDisposable);
			UiKeyboardActions.Slider.OnClickAsObservable().Where((InputAction.CallbackContext _) => HasSelection && SelectionPageCount > 1).Subscribe(delegate(InputAction.CallbackContext ctx)
			{
				Vector2 vector = ctx.ReadValue<Vector2>();
				if (vector.y > 0f)
				{
					OnSelectionPageChange(-1);
				}
				else if (vector.y < 0f)
				{
					OnSelectionPageChange(1);
				}
			})
				.AddTo(InputDisposable);
			UiActions.NextFlow.OnPressStartAsObservable().SelectMany((InputAction.CallbackContext _) => Observable.Interval(TimeSpan.FromSeconds(0.05)).TakeUntil(UiActions.NextFlow.OnPressEndAsObservable())).Where((long _) => !_needSelect)
				.Subscribe(delegate
				{
					OnNext(playSfx: false);
				})
				.AddTo(InputDisposable);
		}
	}

	private void OnProfileChanged(Profile profile)
	{
		_view.Currency_Text.text = profile.Gold.ToNumberString();
		bool flag = profile.Level == TConst.LevelMax.IntValue;
		long needExp = (flag ? 0 : TLevel.Table[profile.Level + 1].NeedExp);
		_view.Exp_Text.text = (flag ? "Max" : string.Format("{0} / {1}", profile.Exp, needExp));
	}

	private void OnNext(bool playSfx)
	{
		if (_isDelayActive)
		{
			_delayCts?.Cancel();
		}
		else if (!(_typeWriter == null))
		{
			if (_isWritingText)
			{
				_typeWriter.SkipTypewriter();
			}
			else
			{
				MoveToNext(playSfx);
			}
		}
	}

	private void OnSelectButtonClick(int actualIndex)
	{
		if (_currentEntry != null)
		{
			_currentEntry.OnCompleted.OnNext(actualIndex);
			_currentEntry = null;
			_needSelect = false;
		}
	}

	private void OnScrollInput(InputAction.CallbackContext ctx)
	{
		Vector2 vector = ctx.ReadValue<Vector2>();
		if (vector.y > 0f)
		{
			OnSelectionPageChange(-1);
		}
		else if (vector.y < 0f)
		{
			OnSelectionPageChange(1);
		}
	}

	private void OnSelectionPageChange(int direction)
	{
		if (_currentEntry?.SelectableMessageDatas != null && _currentEntry.SelectableMessageDatas.Length > 0)
		{
			int num = _selectionPageIndex + direction;
			if (num >= 0 && num < SelectionPageCount)
			{
				_selectionPageIndex = num;
				_soundManager.PlaySfx("select");
				UpdateSelectionDisplay();
			}
		}
	}

	protected override void OnNavOutOfBounds(ENavDir dir)
	{
		if (_currentEntry?.SelectableMessageDatas == null || _currentEntry.SelectableMessageDatas.Length <= 0 || SelectionPageCount <= 1)
		{
			return;
		}
		if (dir == ENavDir.Up)
		{
			if (_selectionPageIndex >= 1)
			{
				_focusLastAfterRefresh = true;
				OnSelectionPageChange(-1);
			}
		}
		else if (dir == ENavDir.Down && _selectionPageIndex < SelectionPageCount - 1)
		{
			_focusLastAfterRefresh = false;
			OnSelectionPageChange(1);
		}
	}

	private void OnTextWriterEnd()
	{
		_isWritingText = false;
	}

	public void AppendDialogText(DialogEntry entry)
	{
		_dialogQueue.Enqueue(entry);
		if (_state == EState.None)
		{
			_state = EState.Started;
			Animator.SetTrigger("Start");
		}
		if (_dialogQueue.Count == 1)
		{
			ShowNextDialog();
		}
	}

	private void MoveToNext(bool playSfx)
	{
		if (_state == EState.Ended)
		{
			return;
		}
		if (!_needSelect && _currentEntry != null)
		{
			_currentEntry.OnCompleted.OnNext(-1);
			_currentEntry = null;
			if (playSfx)
			{
				_soundManager.PlaySfx("DialogNext");
			}
		}
		if (_dialogQueue.Count > 0)
		{
			ShowNextDialog();
		}
		else if (_canClose)
		{
			_state = EState.Ended;
			Animator.SetTrigger("End");
		}
	}

	public void SetClosable()
	{
		_canClose = true;
		if (!_needSelect)
		{
			MoveToNext(playSfx: false);
		}
	}

	private void ShowNextDialog()
	{
		_currentEntry = _dialogQueue.Dequeue();
		DialogSpeakerEntry dialogSpeakerEntry = _currentEntry.FocusedSpeaker;
		string text = ((dialogSpeakerEntry == null) ? _playerManager.Name : dialogSpeakerEntry.Speaker.GetSpeakerName(dialogSpeakerEntry.TableId, _playerManager.Name, _playerManager.SelectedCharacter.Id));
		_view.Name.text = text;
		_view.NamePlayer.text = text;
		bool flag = dialogSpeakerEntry == null || dialogSpeakerEntry.Speaker == ESpeaker.Player;
		_view.Name.gameObject.SetActive(!flag);
		_view.NamePlayer.gameObject.SetActive(flag);
		SetPortraitActive(_view.Left_Portrait, _currentEntry.Speakers[0]);
		SetPortraitActive(_view.Center_Portrait, _currentEntry.Speakers[1]);
		SetPortraitActive(_view.Right_Portrait, _currentEntry.Speakers[2]);
		_needSelect = _currentEntry.HasSelection;
		_selectionPageIndex = 0;
		UpdateSelectionDisplay();
		PlaySpeakerAnimation(_currentEntry.SpeakerIndex);
		_isWritingText = true;
		_typeWriter.ShowText(_currentEntry.Message);
		_typeWriter.StartShowingText(restart: true);
	}

	public void SetDialogImmediate(DialogEntry entry)
	{
		_currentEntry = entry;
		DialogSpeakerEntry dialogSpeakerEntry = _currentEntry.FocusedSpeaker;
		string text = ((dialogSpeakerEntry == null) ? _playerManager.Name : dialogSpeakerEntry.Speaker.GetSpeakerName(dialogSpeakerEntry.TableId, _playerManager.Name, _playerManager.SelectedCharacter.Id));
		_view.Name.text = text;
		_view.NamePlayer.text = text;
		bool flag = dialogSpeakerEntry == null || dialogSpeakerEntry.Speaker == ESpeaker.Player;
		_view.Name.gameObject.SetActive(!flag);
		_view.NamePlayer.gameObject.SetActive(flag);
		SetPortraitActive(_view.Left_Portrait, _currentEntry.Speakers[0]);
		SetPortraitActive(_view.Center_Portrait, _currentEntry.Speakers[1]);
		SetPortraitActive(_view.Right_Portrait, _currentEntry.Speakers[2]);
		_needSelect = false;
		for (int i = 0; i < _view.Selections.Length; i++)
		{
			_view.Selections[i].SetActive(value: false);
		}
		_view.PageButton.SetActive(value: false);
		PlaySpeakerAnimation(_currentEntry.SpeakerIndex);
		_isWritingText = true;
		_typeWriter.ShowText(_currentEntry.Message);
		_typeWriter.StartShowingText(restart: true);
	}

	private void SetPortraitActive(Image image, DialogSpeakerEntry entry)
	{
		if (entry == null || entry.Speaker == ESpeaker.None || string.IsNullOrEmpty(entry.ImageName))
		{
			image.gameObject.SetActive(value: false);
			return;
		}
		image.gameObject.SetActive(value: true);
		if (!(image.sprite == null) && !(image.sprite.name != entry.ImageName))
		{
			return;
		}
		image.SetSprite(_assetManager.GetTexturePortraitAsync(entry.Speaker, entry.ImageName));
	}

	public void RefreshSelections(SelectableMessageData[] selectableMessageDatas)
	{
		_currentEntry.SelectableMessageDatas = selectableMessageDatas;
		_selectionPageIndex = 0;
		_focusLastAfterRefresh = false;
		UpdateSelectionDisplay();
	}

	private void UpdateSelectionDisplay()
	{
		_selectionDisposable?.Dispose();
		_selectionDisposable = new CompositeDisposable();
		SelectableMessageData[] array = _currentEntry?.SelectableMessageDatas;
		int num = _view.Selections.Length;
		int num2 = num * _selectionPageIndex;
		for (int i = 0; i < num; i++)
		{
			int num3 = num2 + i;
			GameObject gameObject = _view.Selections[i];
			SelectableMessageData selectableMessageData = ((array != null && num3 < array.Length) ? array[num3] : null);
			bool flag = !string.IsNullOrEmpty(selectableMessageData?.Message);
			gameObject.SetActive(flag);
			if (flag)
			{
				DialogSelectionSlotModel dialogSelectionSlotModel = new DialogSelectionSlotModel
				{
					MessageData = selectableMessageData,
					Index = num3
				};
				dialogSelectionSlotModel.OnClick.Subscribe(delegate(DialogSelectionSlotModel m)
				{
					OnSelectButtonClick(m.Index);
				}).AddTo(_selectionDisposable);
				gameObject.GetComponent<DialogSelectionSlotPresenter>().Inject(dialogSelectionSlotModel);
			}
		}
		int selectionPageCount = SelectionPageCount;
		_view.PageButton.SetActive(selectionPageCount > 1);
		if (selectionPageCount > 1)
		{
			_view.PageButton_PrevPageButton.interactable = _selectionPageIndex > 0;
			_view.PageButton_NextPageButton.interactable = _selectionPageIndex < SelectionPageCount - 1;
		}
		RefreshSelectionNavAsync().Forget();
	}

	private async UniTaskVoid RefreshSelectionNavAsync()
	{
		await UniTask.Yield();
		ClearNav();
		if (!_needSelect)
		{
			return;
		}
		List<Button> list = _view.Selections.Where((GameObject sel) => sel.activeSelf).Select((GameObject sel) => sel.GetComponent<DialogSelectionSlotView>().Button).ToList();
		if (list.Count != 0)
		{
			SetNavVertical(list);
			Selectable defaultNav = (_focusLastAfterRefresh ? list[list.Count - 1] : list[0]);
			_focusLastAfterRefresh = false;
			SetDefaultNav(defaultNav);
		}
	}

	private void PlaySpeakerAnimation(int index)
	{
		string trigger = index switch
		{
			0 => "LeftTrigger",
			1 => "CenterTrigger",
			2 => "RightTrigger",
			_ => "NoneTrigger",
		};
		_view.Speakers.GetComponent<Animator>().SetTrigger(trigger);
	}

	public void SetBackgroundAlpha(float alpha)
	{
		Image component = _view.Background_GrayDimmer.GetComponent<Image>();
		Image component2 = _view.Background_Gradient.GetComponent<Image>();
		if (!(component == null))
		{
			Color color = component.color;
			color.a = alpha;
			component.color = color;
			Color color2 = component2.color;
			color2.a = alpha;
			component2.color = color2;
		}
	}

	public async UniTask SetCustomImageAsync(string imageName, bool isFront, int index)
	{
		GameObject[] array = (isFront ? _view.FrontImages : _view.BackImages);
		if (array == null || array.Length == 0)
		{
			return;
		}
		if (index < 0 || index >= array.Length)
		{
			Logger.Warning(string.Format("Background index out of range: {0}", index));
			return;
		}
		float duration = 0.5f;
		Image targetImg = array[index].GetComponent<Image>();
		if (targetImg == null)
		{
			return;
		}
		Sprite sprite = await _assetManager.GetDialogImageAsync(imageName);
		if (!(this == null) && !(targetImg == null))
		{
			targetImg.sprite = sprite;
			targetImg.gameObject.SetActive(value: true);
			targetImg.DOKill();
			Color color = targetImg.color;
			color.a = 0f;
			targetImg.color = color;
			await targetImg.DOFade(1f, duration).AsyncWaitForCompletion();
			if (!(this == null))
			{
				UpdateDimActive();
			}
		}
	}

	public async UniTask ClearCustomImageAsync(bool isFront, int index)
	{
		GameObject[] array = (isFront ? _view.FrontImages : _view.BackImages);
		if (array == null || array.Length == 0)
		{
			return;
		}
		if (index < 0 || index >= array.Length)
		{
			Logger.Warning(string.Format("Background index out of range: {0}", index));
			return;
		}
		Image img = array[index].GetComponent<Image>();
		if (img == null)
		{
			return;
		}
		if (img.gameObject.activeSelf && img.color.a > 0f)
		{
			img.DOKill();
			await img.DOFade(0f, 0.5f).AsyncWaitForCompletion();
			if (this == null)
			{
				return;
			}
		}
		img.gameObject.SetActive(value: false);
		UpdateDimActive();
	}

	public async UniTask ClearAllCustomImagesAsync()
	{
		float duration = 0.5f;
		Image[] allImages = (_view.FrontImages ?? Array.Empty<GameObject>()).Concat(_view.BackImages ?? Array.Empty<GameObject>()).Select((GameObject go) => (!(go != null)) ? null : go.GetComponent<Image>()).Where((Image img) => img != null)
			.ToArray();
		UniTask[] array = allImages.Where((Image img) => img.gameObject.activeSelf && img.color.a > 0f).Select((Image img) =>
		{
			img.DOKill();
			return img.DOFade(0f, duration).AsyncWaitForCompletion().AsUniTask(useCurrentSynchronizationContext: true);
		}).ToArray();
		if (array.Length != 0)
		{
			await UniTask.WhenAll(array);
		}
		if (this == null)
		{
			return;
		}
		Image[] array2 = allImages;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].gameObject.SetActive(value: false);
		}
		UpdateDimActive();
	}

	private void OnAnimationEndCompleted()
	{
		Close();
	}

	private void UpdateDimActive()
	{
		_view.Dim.SetActive(HasAnyCustomImageActive());
	}

	private bool HasAnyCustomImageActive()
	{
		return (_view.FrontImages ?? Array.Empty<GameObject>()).Concat(_view.BackImages ?? Array.Empty<GameObject>()).Select((GameObject go) => (!(go != null)) ? null : go.GetComponent<Image>()).Any((Image img) => img != null && img.gameObject.activeSelf && img.color.a > 0f);
	}

	public void FadeOutFrame()
	{
		FrameAnimator.SetBool("FadeOut", value: true);
	}

	public void FadeInFrame()
	{
		FrameAnimator.SetBool("FadeOut", value: false);
	}

	public async UniTask DelayDialogAsync(int milliseconds)
	{
		if (milliseconds <= 0)
		{
			return;
		}
		if (_isDelayActive)
		{
			_delayCts?.Cancel();
			_delayCts?.Dispose();
		}
		_delayCts = new CancellationTokenSource();
		CancellationToken token = _delayCts.Token;
		_isDelayActive = true;
		try
		{
			await UniTask.Delay(milliseconds, ignoreTimeScale: false, PlayerLoopTiming.Update, token);
		}
		finally
		{
			if (_delayCts != null && _delayCts.Token == token)
			{
				_isDelayActive = false;
				_delayCts.Dispose();
				_delayCts = null;
			}
		}
	}
}
