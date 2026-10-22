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

	public Canvas Canvas => canvas;

	public InGameHudPresenter InGamePanel => inGamePanel;

	public Image LoungeLoadingPanel => loungeLoadingPanel;

	public StageEnterTimerPanelPresenter StageEnterTimerPanel => stageEnterTimerPanel;

	public GameObject CutsceneCanvas => cutsceneCanvas;

	public CutscenePanelPresenter CutscenePanel => cutscenePanel;
}
