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

	public Button Background => null;

	public GameObject Content => null;

	public GameObject Body => null;

	public Image ScrollView => null;

	public Image Viewport => null;

	public GameObject MessageRoot => null;

	public GameObject Top => null;

	public Image Top_Bg => null;

	public TextMeshProUGUI Top_QuestType => null;

	public TextMeshProUGUI Top_Title => null;

	public GameObject PlayerMessageSlotTemplate => null;
}
