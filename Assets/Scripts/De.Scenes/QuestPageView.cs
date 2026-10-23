using NemoLib.Mvp;
using UnityEngine;

public class QuestPageView : View
{
	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GameObject empty;

	[SerializeField]
	private QuestInfoPanelPresenter questInfoPanel;

	[SerializeField]
	private GameObject list;

	[SerializeField]
	private QuestScrollerPresenter scroller;

	[SerializeField]
	private GameObject keyGuide;

	[SerializeField]
	private GameObject keyGuide_DisplayHud;

	[SerializeField]
	private GameObject keyGuide_ShowDialog;

	public GameObject Body => body;

	public GameObject Info => info;

	public GameObject Empty => empty;

	public QuestInfoPanelPresenter QuestInfoPanel => questInfoPanel;

	public GameObject List => list;

	public QuestScrollerPresenter Scroller => scroller;

	public GameObject KeyGuide => keyGuide;

	public GameObject KeyGuide_DisplayHud => keyGuide_DisplayHud;

	public GameObject KeyGuide_ShowDialog => keyGuide_ShowDialog;
}
