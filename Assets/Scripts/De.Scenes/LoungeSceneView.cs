using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class LoungeSceneView : View
{
	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private InGameHudPresenter inGamePanel;

	[SerializeField]
	private Image loungeLoadingPanel;

	[SerializeField]
	private StageEnterTimerPanelPresenter stageEnterTimerPanel;

	[SerializeField]
	private GameObject cutsceneCanvas;

	[SerializeField]
	private CutscenePanelPresenter cutscenePanel;

	public Canvas Canvas => null;

	public InGameHudPresenter InGamePanel => null;

	public Image LoungeLoadingPanel => null;

	public StageEnterTimerPanelPresenter StageEnterTimerPanel => null;

	public GameObject CutsceneCanvas => null;

	public CutscenePanelPresenter CutscenePanel => null;
}
