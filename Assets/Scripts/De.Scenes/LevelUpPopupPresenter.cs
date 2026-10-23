using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class LevelUpPopupPresenter : GamePopupPresenter
{
	[View]
	private LevelUpPopupView _view;

	[InjectModel]
	private LevelUpPopupModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	private bool _isAnimationCompleted;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override string OpenSfx => "LevelUp";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Level.text = string.Format("{0}", _model.Level);
		_view.Background.OnClick().Subscribe(delegate
		{
			TryClose();
		}).AddTo(this);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			TryClose();
		}).AddTo(InputDisposable);
	}

	private bool TryClose()
	{
		if (_isAnimationCompleted)
		{
			Close();
			return true;
		}
		Animator component = GetComponent<Animator>();
		component.Play(component.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 1f);
		_isAnimationCompleted = true;
		return false;
	}

	private void OnAnimationCompleted()
	{
		_isAnimationCompleted = true;
	}
}
