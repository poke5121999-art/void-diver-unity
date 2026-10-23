using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class PingPanelPresenter : Presenter, IWheelMenuPanel
{
	private enum Layout
	{
		None = 0,
		NoticePing = 1,
		AlertPing = 2
	}

	[View]
	private PingPanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GamePingManager _pingManager;

	[Inject]
	private GameInputManager _inputManager;

	private Layout _layout;

	private Vector2 _inputPosition;

	private EPingType[] _pingTypes;

	private EPingType _selectedPingType;

	private Vector2 _cachedPadScreenPoint;

	private float _openTime;

	private const float _longPressThreshold = 0.3f;

	public EPingType SelectedPingType => _selectedPingType;

	private bool IsGamePad => InputManager.IsGamePad;

	private void Start()
	{
		_view.Background.OnPointerDownAsObservable().Subscribe(OnPointerDown).AddTo(this);
		_view.Background.OnPointerUpAsObservable().Subscribe(OnPointerUp).AddTo(this);
		_view.Background.OnDragAsObservable().Subscribe(OnDrag).AddTo(this);
	}

	public void Open(Vector2 cachedPadScreenPoint)
	{
		_cachedPadScreenPoint = cachedPadScreenPoint;
		_openTime = Time.time;
		_view.Background.gameObject.SetActive(value: true);
		SetLayout(Layout.None);
	}

	public void Cancel()
	{
		SetLayout(Layout.None);
	}

	private void Update()
	{
		if (IsGamePad && Time.time - _openTime >= 0.3f)
		{
			if (_layout == Layout.None)
			{
				_inputPosition = _cachedPadScreenPoint;
				SetLayout(Layout.NoticePing);
				DisplayMenu();
			}
			int selectedIndexFromStick = GetSelectedIndexFromStick(_inputController.WheelSelectDir);
			_selectedPingType = GetPingTypeFromIndex(selectedIndexFromStick);
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
		if (_layout == Layout.None)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				SetLayout(Layout.NoticePing);
				_selectedPingType = EPingType.Notice;
			}
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				SetLayout(Layout.AlertPing);
				_selectedPingType = EPingType.Alert;
			}
			_inputPosition = eventData.position;
		}
		else
		{
			_toastManager.Notice(TLocalizedText.PingCancelled.LocalText);
			SetLayout(Layout.None);
		}
	}

	private void OnPointerUp(PointerEventData eventData)
	{
		if (_layout != Layout.None)
		{
			EPingType selectedPingType = _selectedPingType;
			Vector2 inputPosition = _inputPosition;
			SetLayout(Layout.None);
			if (inputPosition != Vector2.zero)
			{
				if (selectedPingType == EPingType.Cancel)
				{
					_toastManager.Notice(TLocalizedText.PingCancelled.LocalText);
				}
				else
				{
					_pingManager.TriggerPing(selectedPingType, inputPosition);
				}
			}
		}
	}

	private void OnDrag(PointerEventData eventData)
	{
		if (_layout == Layout.NoticePing)
		{
			float magnitude = (eventData.position - _inputPosition).magnitude;
			if (!_view.Menu.activeSelf && magnitude > 100f)
			{
				DisplayMenu();
			}
			int selectedIndex = GetSelectedIndex(eventData);
			_selectedPingType = GetPingTypeFromIndex(selectedIndex);
			SetAnimation(selectedIndex);
		}
	}

	private void SetAnimation(int index)
	{
		if (_view.Menu.activeSelf)
		{
			_view.Menu.GetComponent<Animator>().SetInteger("Index", index);
		}
	}

	private void SetLayout(Layout layout)
	{
		_layout = layout;
		if (layout == Layout.None)
		{
			_view.Menu.SetActive(value: false);
			_inputPosition = Vector2.zero;
			_selectedPingType = EPingType.None;
		}
		else
		{
			if (layout != Layout.NoticePing)
			{
				return;
			}
			_pingTypes = Enumerable.Range(_gameManager.IsInLounge ? 1 : 11, 8).Select((int i) => (EPingType)i).ToArray();
			_view.Cursor_Blue.gameObject.SetActive(_gameManager.IsInLounge);
			_view.Cursor_Red.gameObject.SetActive(!_gameManager.IsInLounge);
			for (int j = 0; j < 8; j++)
			{
				_view.Pings[j].GetComponent<Image>().sprite = _assetManager.GetIconPing(_pingTypes[j]);
				_view.Pings[j].FindChildComponent<TextMeshProUGUI>("Text").text = _pingTypes[j].ToLocalText();
			}
		}
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
			RectTransformUtility.ScreenPointToLocalPointInRectangle(component, eventData.position, null, out var localPoint);
			return localPoint.magnitude <= component.rect.width * 0.5f;
		}
	}

	private EPingType GetPingTypeFromIndex(int index)
	{
		if (index < 0)
		{
			return EPingType.Cancel;
		}
		return _pingTypes[index];
	}
}
