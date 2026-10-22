using NemoLib.Mvp;
using VContainer;

public class InGameKeyGuidePanelPresenter : Presenter
{
	[View]
	private InGameKeyGuidePanelView _view;

	[Inject]
	private GameInputManager _inputManager;

	private InputActions.InGameActions InGameActions => default;

	private void Start()
	{
	}

	private void ToggleKeyGuide()
	{
	}
}
