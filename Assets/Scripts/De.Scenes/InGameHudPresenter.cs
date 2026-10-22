using GameCommon.Enum;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class InGameHudPresenter : Presenter
{
	[View]
	private InGameHudView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private GamePingManager _pingManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameMapManager _mapManager;

	private void Start()
	{
	}

	private void OnPingPressed(bool isPressed)
	{
	}

	private void OnEmojiPressed(bool isPressed)
	{
	}

	private void OnPingTriggered((EPingType pingType, Vector2 inputPosition) data)
	{
	}
}
