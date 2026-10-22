using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.EventSystems;
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

	private int _selectedIndex;

	private Vector2 _cachedPadScreenPoint;

	private float _openTime;

	private const float _longPressThreshold = 0.3f;

	public int SelectedIndex => 0;

	private bool IsCanceled => false;

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

	private void DisplayMenu()
	{
	}

	private int GetSelectedIndex(PointerEventData eventData)
	{
		return 0;
	}
}
