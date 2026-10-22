using UnityEngine.InputSystem;
using UniRx;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using VContainer;

public class InGameWatchingPanelPresenter : Presenter
{
	[View]
	private InGameWatchingPanelView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameInputManager _inputManager;

	private bool _isWatching;

	private InputActions.InGameActions InGameActions => _inputManager.InGameActions;

	private void Start()
	{
		_view.Background.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnChangeViewTarget();
		}).AddTo(this);
		if (_gameManager.MyCharacterModel != null)
		{
			_gameManager.MyCharacterModel.RxState.Subscribe(OnMyCharacterStateChanged).AddTo(this);
		}
		else
		{
			_gameManager.OnMyCharacterSpawned.Subscribe(SubscribeCharacterState).AddTo(this);
		}
		_gameManager.RxStageState.Where((EStageState state) => state == EStageState.Ended).Subscribe(delegate
		{
			OnStageEnded();
		}).AddTo(this);
		_inputManager.InGameActions.ChangeViewTarget.OnClickAsObservable().Where((InputAction.CallbackContext _) => _isWatching).WithSfx("ButtonClick")
			.Subscribe(delegate
			{
				OnChangeViewTarget();
			})
			.AddTo(this);
	}

	private void SubscribeCharacterState(CharacterModel characterModel)
	{
		characterModel.RxState.Subscribe(OnMyCharacterStateChanged).AddTo(this);
	}

	private void OnMyCharacterStateChanged(EActionState state)
	{
		if (!_gameManager.IsAllDeadOrEscaped())
		{
			_isWatching = state == EActionState.Dead || state == EActionState.Escaped;
			_view.Anchor.SetActive(_isWatching);
		}
	}

	private void OnStageEnded()
	{
		_view.Anchor.SetActive(false);
	}

	private void OnChangeViewTarget()
	{
		_gameManager.ChangeViewTarget();
	}
}
