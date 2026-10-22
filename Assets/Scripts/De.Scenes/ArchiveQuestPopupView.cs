using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveQuestPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private GameObject scroller;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private GameObject empty;

	[SerializeField]
	private QuestInfoPanelPresenter questInfoPanel;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private TextMeshProUGUI title;

	[SerializeField]
	private TextMeshProUGUI titleSub;

	[SerializeField]
	private GameObject keyGuide;

	[SerializeField]
	private GameObject keyGuide_ShowDialog;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Info => null;

	public GameObject Left => null;

	public GameObject Scroller => null;

	public GameObject Right => null;

	public GameObject Empty => null;

	public QuestInfoPanelPresenter QuestInfoPanel => null;

	public GameObject Top => null;

	public TextMeshProUGUI Title => null;

	public TextMeshProUGUI TitleSub => null;

	public GameObject KeyGuide => null;

	public GameObject KeyGuide_ShowDialog => null;
}
