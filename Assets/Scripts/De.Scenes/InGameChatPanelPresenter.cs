using System.Linq;
using DG.Tweening;
using GameCommon.Table;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Popup;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;

public class InGameChatPanelPresenter : BasePanelPresenter
{
	[View]
	private InGameChatPanelView _view;

	[InjectModel]
	private InGameChatPanelModel _model;

	[Inject]
	private GameChatManager _gameChatManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private SeriLogger _logger;

	private float _textRemainEndTime;

	private ObservableEventTrigger _viewResizeTrigger;

	private float _mouseDragStartPosY;

	private ReactiveProperty<float> _rxHeightRatio = new ReactiveProperty<float>();

	private const float _minRatio = 0.3f;

	private const float _maxRatio = 0.9f;

	private const float _defaultRatio = 0.373f;

	private bool _isDraggingResizer;

	private bool _isPointerOver;

	private Sequence _bgFadeSequence;

	private bool _isChatMode;

	private bool _isSubmitting;

	private int _enterChatFrame = -1;

	private RectTransform _inputFieldRect;

	private Camera _worldCamera;

	private readonly Vector3[] _corners = new Vector3[4];

	protected override BasePanelModel PanelModel => _model;

	private TMP_InputField InputField => _view.InputField;

	private GameObject ViewResizer => _view.ViewResizer.gameObject;

	private InputActions.InGameActions InGameActions => _inputManager.InGameActions;

	private bool ShouldShowResizer
	{
		get
		{
			if (!_isChatMode && !_isPointerOver)
			{
				return _isDraggingResizer;
			}
			return true;
		}
	}

	private void SetChatMode(bool value)
	{
		_isChatMode = value;
		RefreshResizer();
	}

	private void Start()
	{
		_inputFieldRect = _view.InputField.GetComponent<RectTransform>();
		_worldCamera = _inputFieldRect.GetComponentInParent<Canvas>().rootCanvas.worldCamera;
		_gameChatManager.RxChatRecords.ObserveChange().Subscribe(delegate
		{
			OnChatRecordsChanged();
		}).AddTo(this);
		_view.InputField.onSelect.AsObservable().Subscribe(OnSelectInputField).AddTo(this);
		_view.InputField.onDeselect.AsObservable().Subscribe(OnDeselectInputField).AddTo(this);
		_view.InputField.onSubmit.AsObservable().Subscribe(OnSubmitInputField).AddTo(this);
		_viewResizeTrigger = _view.ViewResizer.gameObject.AddComponent<ObservableEventTrigger>();
		_viewResizeTrigger.OnPointerEnterAsObservable().Subscribe(delegate
		{
			OnPointerEnter();
		}).AddTo(this);
		_viewResizeTrigger.OnPointerExitAsObservable().Subscribe(delegate
		{
			OnPointerExit();
		}).AddTo(this);
		_viewResizeTrigger.OnBeginDragAsObservable().Subscribe(OnBeginDrag).AddTo(this);
		_viewResizeTrigger.OnDragAsObservable().Subscribe(OnDrag).AddTo(this);
		_viewResizeTrigger.OnEndDragAsObservable().Subscribe(OnEndDrag).AddTo(this);
		_rxHeightRatio.Value = PlayerPrefs.GetFloat("ChatPanel_YPos_Ratio_WithMaxPos", 0.373f);
		_rxHeightRatio.Subscribe(OnHeightRatioChanged).AddTo(this);
	}

	private void OnEnable()
	{
		_inputManager.InGameActions.Chat.performed += OnInputEnter;
		RefreshResizer();
	}

	private void OnDisable()
	{
		_inputManager.InGameActions.Chat.performed -= OnInputEnter;
		if (_isChatMode)
		{
			_inputManager.UnblockInput();
			_isChatMode = false;
			RefreshResizer();
		}
		_bgFadeSequence?.Kill();
	}

	private void Update()
	{
		if (_view.InputFieldAnchor.activeSelf && !_isChatMode && Time.time > _textRemainEndTime)
		{
			_view.InputField.text = string.Empty;
			_view.InputFieldAnchor.SetActive(value: false);
		}
	}

	private void LateUpdate()
	{
		UpdateImeCompositionCursor();
	}

	private void UpdateImeCompositionCursor()
	{
		if (_view.InputField.isFocused)
		{
			_inputFieldRect.GetWorldCorners(_corners);
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(_worldCamera, _corners[2]);
			Input.compositionCursorPos = new Vector2(vector.x + 5f, (float)Screen.height - vector.y);
		}
	}

	private void OnChatRecordsChanged()
	{
		ChatScrollerModel chatScrollerModel = new ChatScrollerModel();
		chatScrollerModel.ChatInfoList = _gameChatManager.RxChatRecords.ToList();
		_view.Scroller.GetComponent<ChatScrollerPresenter>().Inject(chatScrollerModel);
	}

	private void OnChatModeChanged()
	{
		RefreshResizer();
	}

	private void OnHeightRatioChanged(float ratio)
	{
		float height = _view.GetComponent<RectTransform>().rect.height;
		RectTransform component = _view.ScrollerView.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(component.sizeDelta.x, height * ratio);
		_view.ViewResizer_TriangleAnchor_Down.gameObject.SetActive(ratio > 0.315f);
		_view.ViewResizer_TriangleAnchor_Up.gameObject.SetActive(ratio < 0.855f);
	}

	private void OnPointerEnter()
	{
		_isPointerOver = true;
		RefreshResizer();
	}

	private void OnPointerExit()
	{
		_isPointerOver = false;
		RefreshResizer();
	}

	private void OnBeginDrag(PointerEventData eventData)
	{
		_mouseDragStartPosY = eventData.position.y;
		_isDraggingResizer = true;
		RefreshResizer();
	}

	private void OnDrag(PointerEventData eventData)
	{
		float y = eventData.position.y;
		float mouseDragStartPosY = _mouseDragStartPosY;
		_mouseDragStartPosY = eventData.position.y;
		float height = _view.GetComponent<RectTransform>().rect.height;
		float num = (_view.ScrollerView.GetComponent<RectTransform>().sizeDelta.y + (y - mouseDragStartPosY)) / height;
		_rxHeightRatio.Value = Mathf.Clamp(num, 0.3f, 0.9f);
	}

	private void OnEndDrag(PointerEventData eventData)
	{
		_isDraggingResizer = false;
		PlayerPrefs.SetFloat("ChatPanel_YPos_Ratio_WithMaxPos", _rxHeightRatio.Value);
		RefreshResizer();
	}

	private void OnInputEnter(InputAction.CallbackContext context)
	{
		EnterChat();
	}

	private void OnSubmitInputField(string text)
	{
		if (string.IsNullOrEmpty(Input.compositionString) && Time.frameCount > _enterChatFrame + 1)
		{
			_isSubmitting = true;
			Observable.NextFrame().Subscribe(delegate
			{
				_isSubmitting = false;
				if (!string.IsNullOrEmpty(_view.InputField.text))
				{
					if (UIInputUtility.ValidateText(_view.InputField.text))
					{
						if (_gameChatManager.TrySendUserChat(_view.InputField.text))
						{
							_view.InputField.text = string.Empty;
							_view.InputFieldAnchor.SetActive(value: false);
						}
						else
						{
							_toastManager.NoticeFail(TLocalizedText.ChatFailBlocked.LocalText);
						}
					}
					else
					{
						_toastManager.NoticeFail(TLocalizedText.ContainsProhibitedWords.LocalText);
					}
				}
				_isChatMode = false;
				RefreshResizer();
				_inputManager.UnblockInput();
				_view.InputField.text = string.Empty;
				_view.InputField.DeactivateInputField();
			}).AddTo(this);
		}
	}

	private void OnSelectInputField(string text)
	{
		if (!_isChatMode)
		{
			EnterChat();
		}
	}

	private void OnDeselectInputField(string text)
	{
		if (_isChatMode && !_isSubmitting)
		{
			ExitChat(resetText: false);
		}
	}

	private void EnterChat()
	{
		_view.InputFieldAnchor.SetActive(value: true);
		_inputManager.BlockInput();
		_isChatMode = true;
		RefreshResizer();
		_view.InputField.ActivateInputField();
		_enterChatFrame = Time.frameCount;
	}

	private void ExitChat(bool resetText)
	{
		_isChatMode = false;
		RefreshResizer();
		_inputManager.UnblockInput();
		if (resetText || string.IsNullOrEmpty(_view.InputField.text))
		{
			_view.InputField.text = string.Empty;
		}
		else
		{
			_textRemainEndTime = Time.time + 5f;
		}
		_view.InputField.DeactivateInputField();
	}

	private bool TrySubmitInput()
	{
		if (string.IsNullOrEmpty(_view.InputField.text))
		{
			return false;
		}
		TLocalizedText tLocalizedText;
		if (UIInputUtility.ValidateText(_view.InputField.text))
		{
			if (_gameChatManager.TrySendUserChat(_view.InputField.text))
			{
				return true;
			}
			tLocalizedText = TLocalizedText.ChatFailBlocked;
		}
		else
		{
			tLocalizedText = TLocalizedText.ContainsProhibitedWords;
		}
		_toastManager.NoticeFail(tLocalizedText.LocalText);
		return false;
	}

	private void RefreshResizer()
	{
		if (ShouldShowResizer)
		{
			ShowResizer();
		}
		else
		{
			HideResizer();
		}
	}

	private void ShowResizer()
	{
		_bgFadeSequence?.Kill();
		_view.BgCanvas.alpha = 1f;
		_view.ViewResizer_Line.alpha = 1f;
		_view.ViewResizer_TriangleAnchor.SetActive(value: true);
	}

	private void HideResizer()
	{
		_view.ViewResizer_TriangleAnchor.SetActive(value: false);
		StartFadeOutSequence();
	}

	private void SetActiveResizeArrows(bool isActive)
	{
		_view.ViewResizer_TriangleAnchor.SetActive(isActive);
	}

	private void StartFadeOutSequence()
	{
		_bgFadeSequence?.Kill();
		_bgFadeSequence = DOTween.Sequence();
		_bgFadeSequence.AppendInterval(3f);
		_bgFadeSequence.Append(_view.BgCanvas.DOFade(0f, 0.5f));
		_bgFadeSequence.Join(_view.ViewResizer_Line.DOFade(0f, 0.5f));
		_bgFadeSequence.SetUpdate(isIndependentUpdate: true);
	}
}
