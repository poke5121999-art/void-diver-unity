using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveGlossaryPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private Button backButton;

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
	private GameObject glossary;

	[SerializeField]
	private ScrollRect glossary_Scroll;

	[SerializeField]
	private Image glossary_Viewport;

	[SerializeField]
	private GameObject glossary_Content;

	[SerializeField]
	private TextMeshProUGUI glossary_Text;

	[SerializeField]
	private GameObject glossary_Title;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private TextMeshProUGUI title;

	[SerializeField]
	private TextMeshProUGUI titleSub;

	public Button Background => background;

	public GameObject Contents => contents;

	public Button BackButton => backButton;

	public GameObject Info => info;

	public GameObject Left => left;

	public GameObject Scroller => scroller;

	public GameObject Right => right;

	public GameObject Empty => empty;

	public GameObject Glossary => glossary;

	public ScrollRect Glossary_Scroll => glossary_Scroll;

	public Image Glossary_Viewport => glossary_Viewport;

	public GameObject Glossary_Content => glossary_Content;

	public TextMeshProUGUI Glossary_Text => glossary_Text;

	public GameObject Glossary_Title => glossary_Title;

	public GameObject Top => top;

	public TextMeshProUGUI Title => title;

	public TextMeshProUGUI TitleSub => titleSub;
}
