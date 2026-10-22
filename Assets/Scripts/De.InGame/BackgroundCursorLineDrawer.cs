using UnityEngine;
using VContainer;

[DefaultExecutionOrder(200)]
public class BackgroundCursorLineDrawer : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private PlayerInputController _playerInputController;

	[Inject]
	private GameCutsceneManager _gameCutsceneManager;

	[SerializeField]
	private float _playerRadius;

	[SerializeField]
	private Vector3 _correctionOffset;

	[SerializeField]
	private GameObject _cursorLineHolder;

	[SerializeField]
	private GameObject _playerQuad;

	[SerializeField]
	private GameObject _lineQuad;

	[SerializeField]
	private GameObject _pointQuad;

	private Vector3 _cursorWorldPosition;

	private Plane _xzPlane;

	private Camera MainCamera => null;

	private Vector3 PlayerPosition => default;

	private bool IsCursorLineValid => false;

	private Vector2 ScreenCursorPosition => default;

	private void LateUpdate()
	{
	}
}
