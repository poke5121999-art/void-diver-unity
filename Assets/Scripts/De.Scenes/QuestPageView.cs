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

	public GameObject Body => null;

	public GameObject Info => null;

	public GameObject Empty => null;

	public QuestInfoPanelPresenter QuestInfoPanel => null;

	public GameObject List => null;

	public QuestScrollerPresenter Scroller => null;

	public GameObject KeyGuide => null;

	public GameObject KeyGuide_DisplayHud => null;

	public GameObject KeyGuide_ShowDialog => null;
}
