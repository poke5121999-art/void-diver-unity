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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Info => info;

	public GameObject Left => left;

	public GameObject Scroller => scroller;

	public GameObject Right => right;

	public GameObject Empty => empty;

	public QuestInfoPanelPresenter QuestInfoPanel => questInfoPanel;

	public GameObject Top => top;

	public TextMeshProUGUI Title => title;

	public TextMeshProUGUI TitleSub => titleSub;

	public GameObject KeyGuide => keyGuide;

	public GameObject KeyGuide_ShowDialog => keyGuide_ShowDialog;
}
