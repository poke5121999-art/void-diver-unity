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

	public Button Background => null;

	public GameObject Contents => null;

	public Button BackButton => null;

	public GameObject Info => null;

	public GameObject Left => null;

	public GameObject Scroller => null;

	public GameObject Right => null;

	public GameObject Empty => null;

	public GameObject Glossary => null;

	public ScrollRect Glossary_Scroll => null;

	public Image Glossary_Viewport => null;

	public GameObject Glossary_Content => null;

	public TextMeshProUGUI Glossary_Text => null;

	public GameObject Glossary_Title => null;

	public GameObject Top => null;

	public TextMeshProUGUI Title => null;

	public TextMeshProUGUI TitleSub => null;
}
