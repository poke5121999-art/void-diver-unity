using UnityEngine;
using UnityEngine.UI;
using VContainer;

public abstract class CursorBase : MonoBase
{
	[SerializeField]
	protected Image _cursorIcon;

	[SerializeField]
	protected Sprite _defaultCursor;

	[SerializeField]
	protected Sprite _gameCursor;

	[SerializeField]
	protected Sprite _targetCursor;

	[SerializeField]
	protected Sprite _pingCursor;

	[Inject]
	protected CameraManager _cameraManager;

	[Inject]
	protected PlayerInputController _inputController;

	[Inject]
	protected GameManager _gameManager;

	protected abstract Vector2 GetCursorPosition();

	protected abstract bool IsCursorVisible();

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	private void SetCursor(Sprite sprite)
	{
	}
}
