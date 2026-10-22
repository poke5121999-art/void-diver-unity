using NemoLib.Mvp;
using VContainer;

public class CutscenePanelPresenter : Presenter
{
	[View]
	private CutscenePanelView _view;

	[Inject]
	private GameCutsceneManager _cutsceneManager;

	private void Start()
	{
	}
}
