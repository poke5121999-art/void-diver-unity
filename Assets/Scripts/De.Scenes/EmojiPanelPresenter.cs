using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class EmojiPanelPresenter : Presenter, IWheelMenuPanel
{
	[View]
	private EmojiPanelView _view;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private GameChannel _gameChannel;

	private Vector2 _inputPosition;

	private int _selectedIndex = -1;

	private Vector2 _cachedPadScreenPoint;

	private float _openTime;

	private const float _longPressThreshold = 0.3f;

	public int SelectedIndex => _selectedIndex;

	private bool IsCanceled => _inputPosition == Vector2.zero;

	private bool IsGamePad => InputManager.IsGamePad;

	private void Start()
	{
		_view.Background.OnPointerDownAsObservable().Subscribe(OnPointerDown).AddTo(this);
		_view.Background.OnPointerUpAsObservable().Subscribe(OnPointerUp).AddTo(this);
		_view.Background.OnDragAsObservable().Subscribe(OnDrag).AddTo(this);
		for (int i = 0; i < _view.Pings.Length; i++)
		{
			_view.Pings[i].GetComponent<Image>().sprite = _assetManager.GetIconEmoji(i);
		}
	}

	public void Open(Vector2 cachedPadScreenPoint)
	{
		_cachedPadScreenPoint = cachedPadScreenPoint;
		_openTime = Time.time;
		_view.Background.gameObject.SetActive(value: true);
		_view.Menu.SetActive(value: false);
		_inputPosition = cachedPadScreenPoint;
		_selectedIndex = -1;
	}

	public void Cancel()
	{
		_view.Menu.SetActive(value: false);
		_inputPosition = Vector2.zero;
		_selectedIndex = -1;
	}

	private void Update()
	{
		if (IsGamePad && Time.time - _openTime >= 0.3f)
		{
			if (!_view.Menu.activeSelf)
			{
				_inputPosition = _cachedPadScreenPoint;
				DisplayMenu();
			}
			int selectedIndexFromStick = GetSelectedIndexFromStick(_inputController.WheelSelectDir);
			_selectedIndex = selectedIndexFromStick;
			SetAnimation(selectedIndexFromStick);
		}
	}

	private int GetSelectedIndexFromStick(Vector2 stick)
	{
		if (stick.magnitude < 0.3f)
		{
			return -1;
		}
		float num = Mathf.Atan2(stick.y, stick.x) * 57.29578f;
		float num2 = (360f - num) % 360f;
		float num3 = (num2 + 90f + 22.5f) % 360f;
		return (int)(num3 / 45f) % 8;
	}

	private void OnPointerDown(PointerEventData eventData)
	{
		if (!IsCanceled && eventData.button == PointerEventData.InputButton.Right)
		{
			_toastManager.Notice(TLocalizedText.EmojiCancelled.LocalText);
			Cancel();
		}
		else if (eventData.button == PointerEventData.InputButton.Left)
		{
			_selectedIndex = -1;
			_inputPosition = eventData.position;
		}
	}

	private void OnPointerUp(PointerEventData eventData)
	{
		if (IsCanceled)
		{
			return;
		}
		if (_selectedIndex < 0)
		{
			_toastManager.Notice((_view.Menu.activeSelf ? TLocalizedText.EmojiCancelled : TLocalizedText.EmojiClickedWithoutDrag).LocalText);
		}
		else
		{
			_gameChannel.SendToHost(new ReqEmoji
			{
				EmojiId = _selectedIndex
			});
		}
		Cancel();
	}

	private void OnDrag(PointerEventData eventData)
	{
		if (!IsCanceled)
		{
			float magnitude = (eventData.position - _inputPosition).magnitude;
			if (!_view.Menu.activeSelf && magnitude > 25f)
			{
				DisplayMenu();
			}
			int selectedIndex = GetSelectedIndex(eventData);
			_selectedIndex = selectedIndex;
			SetAnimation(selectedIndex);
		}
	}

	private void SetAnimation(int index)
	{
		_view.Menu.GetComponent<Animator>().SetInteger("Index", index);
	}

	private void DisplayMenu()
	{
		_view.Menu.SetActive(value: true);
		RectTransform component = _view.Menu.GetComponent<RectTransform>();
		RectTransformUtility.ScreenPointToLocalPointInRectangle(component.parent as RectTransform, _inputPosition, _cameraManager.UiCamera, out var localPoint);
		component.anchoredPosition = localPoint;
	}

	private int GetSelectedIndex(PointerEventData eventData)
	{
		if (IsInCancelArea())
		{
			return -1;
		}
		Vector2 normalized = (eventData.position - _inputPosition).normalized;
		float num = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
		float num2 = (360f - num) % 360f;
		float num3 = (num2 + 90f + 22.5f) % 360f;
		return (int)(num3 / 45f) % 8;

		bool IsInCancelArea()
		{
			RectTransform component = _view.Cancel_Area.GetComponent<RectTransform>();
			Vector2 b = RectTransformUtility.WorldToScreenPoint(_cameraManager.UiCamera, component.position);
			float width = component.rect.width;
			return Vector2.Distance(eventData.position, b) <= width * 0.5f;
		}
	}
}
