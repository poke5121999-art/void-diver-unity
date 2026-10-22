using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveDialogPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private Image scrollView;

	[SerializeField]
	private Image viewport;

	[SerializeField]
	private GameObject messageRoot;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private Image top_Bg;

	[SerializeField]
	private TextMeshProUGUI top_QuestType;

	[SerializeField]
	private TextMeshProUGUI top_Title;

	[SerializeField]
	private GameObject playerMessageSlotTemplate;

	public Button Background => background;

	public GameObject Content => content;

	public GameObject Body => body;

	public Image ScrollView => scrollView;

	public Image Viewport => viewport;

	public GameObject MessageRoot => messageRoot;

	public GameObject Top => top;

	public Image Top_Bg => top_Bg;

	public TextMeshProUGUI Top_QuestType => top_QuestType;

	public TextMeshProUGUI Top_Title => top_Title;

	public GameObject PlayerMessageSlotTemplate => playerMessageSlotTemplate;
}
