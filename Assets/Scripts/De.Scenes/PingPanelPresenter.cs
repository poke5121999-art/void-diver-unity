using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.EventSystems;
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

	public EPingType SelectedPingType => EPingType.None;

	private bool IsGamePad => false;

	private void Start()
	{
	}

	public void Open(Vector2 cachedPadScreenPoint)
	{
	}

	public void Cancel()
	{
	}

	private void Update()
	{
	}

	private int GetSelectedIndexFromStick(Vector2 stick)
	{
		return 0;
	}

	private void OnPointerDown(PointerEventData eventData)
	{
	}

	private void OnPointerUp(PointerEventData eventData)
	{
	}

	private void OnDrag(PointerEventData eventData)
	{
	}

	private void SetAnimation(int index)
	{
	}

	private void SetLayout(Layout layout)
	{
	}

	private void DisplayMenu()
	{
	}

	private int GetSelectedIndex(PointerEventData eventData)
	{
		return 0;
	}

	private EPingType GetPingTypeFromIndex(int index)
	{
		return EPingType.None;
	}
}
