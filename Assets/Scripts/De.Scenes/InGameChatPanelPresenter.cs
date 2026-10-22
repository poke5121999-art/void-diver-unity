using DG.Tweening;
using NemoLib.Log;
using NemoLib.Mvp;
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

	private ReactiveProperty<float> _rxHeightRatio;

	private const float _minRatio = 0.3f;

	private const float _maxRatio = 0.9f;

	private const float _defaultRatio = 0.373f;

	private bool _isDraggingResizer;

	private bool _isPointerOver;

	private Sequence _bgFadeSequence;

	private bool _isChatMode;

	private bool _isSubmitting;

	private int _enterChatFrame;

	private RectTransform _inputFieldRect;

	private Camera _worldCamera;

	private readonly Vector3[] _corners;

	protected override BasePanelModel PanelModel => null;

	private TMP_InputField InputField => null;

	private GameObject ViewResizer => null;

	private InputActions.InGameActions InGameActions => default;

	private bool ShouldShowResizer => false;

	private void SetChatMode(bool value)
	{
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
	}

	private void UpdateImeCompositionCursor()
	{
	}

	private void OnChatRecordsChanged()
	{
	}

	private void OnChatModeChanged()
	{
	}

	private void OnHeightRatioChanged(float ratio)
	{
	}

	private void OnPointerEnter()
	{
	}

	private void OnPointerExit()
	{
	}

	private void OnBeginDrag(PointerEventData eventData)
	{
	}

	private void OnDrag(PointerEventData eventData)
	{
	}

	private void OnEndDrag(PointerEventData eventData)
	{
	}

	private void OnInputEnter(InputAction.CallbackContext context)
	{
	}

	private void OnSubmitInputField(string text)
	{
	}

	private void OnSelectInputField(string text)
	{
	}

	private void OnDeselectInputField(string text)
	{
	}

	private void EnterChat()
	{
	}

	private void ExitChat(bool resetText)
	{
	}

	private bool TrySubmitInput()
	{
		return false;
	}

	private void RefreshResizer()
	{
	}

	private void ShowResizer()
	{
	}

	private void HideResizer()
	{
	}

	private void SetActiveResizeArrows(bool isActive)
	{
	}

	private void StartFadeOutSequence()
	{
	}
}
