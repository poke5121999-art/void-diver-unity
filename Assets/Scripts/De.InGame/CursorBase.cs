using GameCommon.Enum;
using De.InGame.Unit;
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
		SetCursor(_defaultCursor);
	}

	protected virtual void Update()
	{
		_cursorIcon.enabled = IsCursorVisible();
		if (!_cursorIcon.enabled)
		{
			return;
		}
		Vector2 cursorPosition = GetCursorPosition();
		_cursorIcon.rectTransform.position = new Vector3(cursorPosition.x, cursorPosition.y, 0f);
		if (!_gameManager.IsParticipant)
		{
			SetCursor(_defaultCursor);
			return;
		}
		Ray ray = _cameraManager.MainCamera.ScreenPointToRay(new Vector3(cursorPosition.x, cursorPosition.y, 0f));
		if (_inputController.CompositeInputState == ECompositeInputState.Emoji || _inputController.CompositeInputState == ECompositeInputState.Ping)
		{
			SetCursor(_pingCursor);
		}
		else if (Physics.Raycast(ray, out var hitInfo, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Unit")) && hitInfo.collider.gameObject.HasComponent<MonsterView>())
		{
			SetCursor(_targetCursor);
		}
		else
		{
			SetCursor(_gameCursor);
		}
	}

	private void SetCursor(Sprite sprite)
	{
		if (!(sprite == _cursorIcon.sprite))
		{
			_cursorIcon.sprite = sprite;
			_cursorIcon.enabled = sprite != null;
			if (sprite != null)
			{
				_cursorIcon.SetNativeSize();
			}
		}
	}
}
