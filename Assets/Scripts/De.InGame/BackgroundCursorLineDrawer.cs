using GameCommon.Enum;
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
	private float _playerRadius = 0.5f;

	[SerializeField]
	private Vector3 _correctionOffset = new Vector3(-0f, 0f, 0f);

	[SerializeField]
	private GameObject _cursorLineHolder;

	[SerializeField]
	private GameObject _playerQuad;

	[SerializeField]
	private GameObject _lineQuad;

	[SerializeField]
	private GameObject _pointQuad;

	private Vector3 _cursorWorldPosition;

	private Plane _xzPlane = new Plane(Vector3.up, Vector3.zero);

	private Camera MainCamera
	{
		get
		{
			if (!_cameraManager)
			{
				return null;
			}
			return _cameraManager.MainCamera;
		}
	}

	private Vector3 PlayerPosition
	{
		get
		{
			if (!_gameManager)
			{
				return Vector3.zero;
			}
			return _gameManager.MyCharacterController.transform.position;
		}
	}

	private bool IsCursorLineValid
	{
		get
		{
			if (!Shader.IsKeywordEnabled("_CURSOR_LINE_ENABLE"))
			{
				return false;
			}
			if (_playerInputController == null || _gameManager == null)
			{
				return false;
			}
			if (!_gameManager.IsParticipant)
			{
				return false;
			}
			if (_gameManager.StageState == EStageState.Intro)
			{
				return false;
			}
			if (_gameManager.MyCharacterModel == null)
			{
				return false;
			}
			if (_gameManager.MyCharacterModel.State == EActionState.StageReady)
			{
				return false;
			}
			if (_gameCutsceneManager == null)
			{
				return false;
			}
			return !_gameCutsceneManager.RxIsPlaying.Value;
		}
	}

	private Vector2 ScreenCursorPosition => _playerInputController.GetAimScreenPoint();

	private void LateUpdate()
	{
		if (_cursorLineHolder == null)
		{
			return;
		}
		if (!IsCursorLineValid || MainCamera == null || _playerQuad == null || _lineQuad == null || _pointQuad == null)
		{
			_cursorLineHolder.SetActive(value: false);
			return;
		}
		if (!_cursorLineHolder.activeSelf)
		{
			_cursorLineHolder.SetActive(value: true);
		}
		Camera mainCamera = MainCamera;
		Vector2 aimScreenPoint = _playerInputController.GetAimScreenPoint();
		Ray ray = mainCamera.ScreenPointToRay(aimScreenPoint);
		if (_xzPlane.Raycast(ray, out var enter))
		{
			_cursorWorldPosition = ray.GetPoint(enter);
		}
		Vector3 vector = PlayerPosition + _correctionOffset;
		Vector3 vector2 = _cursorWorldPosition + _correctionOffset;
		_playerQuad.transform.position = vector;
		_pointQuad.transform.position = vector2;
		Vector3 vector3 = new Vector3(vector2.x - vector.x, 0f, vector2.z - vector.z);
		float num = vector3.magnitude - _playerRadius;
		if (num < _playerRadius || !_playerInputController.IsAiming)
		{
			num = 0f;
			_lineQuad.SetActive(value: false);
			_pointQuad.SetActive(value: false);
		}
		else
		{
			_lineQuad.SetActive(value: true);
			_pointQuad.SetActive(value: true);
		}
		Vector3 normalized = vector3.normalized;
		_lineQuad.transform.forward = normalized;
		_lineQuad.transform.position = (vector + vector2) * 0.5f + normalized * (_playerRadius * 0.5f);
		_lineQuad.transform.localScale = new Vector3(1f, 1f, num);
		FlattenLocalY(_playerQuad.transform);
		FlattenLocalY(_pointQuad.transform);
		FlattenLocalY(_lineQuad.transform);
	}

	private static void FlattenLocalY(Transform t)
	{
		Vector3 localPosition = t.localPosition;
		t.localPosition = localPosition - Vector3.up * t.localPosition.y;
	}
}
