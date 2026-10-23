using NemoLib.Mvp;
using UniRx;
using VContainer;

public class InGameKeyGuidePanelPresenter : Presenter
{
	[View]
	private InGameKeyGuidePanelView _view;

	[Inject]
	private GameInputManager _inputManager;

	private InputActions.InGameActions InGameActions => _inputManager.InGameActions;

	private void Start()
	{
		_inputManager.InGameActions.ToggleKeyGuide.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			ToggleKeyGuide();
		}).AddTo(this);
	}

	private void ToggleKeyGuide()
	{
		_view.Toggle.SetActive(!_view.Toggle.activeSelf);
	}
}
