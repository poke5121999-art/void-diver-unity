using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameChatPanelView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject layout;

	[SerializeField]
	private GameObject enter;

	[SerializeField]
	private GameObject inputFieldAnchor;

	[SerializeField]
	private TMP_InputField inputField;

	[SerializeField]
	private GameObject textArea;

	[SerializeField]
	private TextMeshProUGUI textArea_Placeholder;

	[SerializeField]
	private TextMeshProUGUI textArea_Text;

	[SerializeField]
	private GameObject scrollerView;

	[SerializeField]
	private CanvasGroup bgCanvas;

	[SerializeField]
	private ChatScrollerPresenter scroller;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private TextMeshProUGUI previewText;

	[SerializeField]
	private Image viewResizer;

	[SerializeField]
	private CanvasGroup viewResizer_Line;

	[SerializeField]
	private GameObject viewResizer_TriangleAnchor;

	[SerializeField]
	private Image viewResizer_TriangleAnchor_Down;

	[SerializeField]
	private Image viewResizer_TriangleAnchor_Up;

	public GameObject Contents => contents;

	public GameObject Layout => layout;

	public GameObject Enter => enter;

	public GameObject InputFieldAnchor => inputFieldAnchor;

	public TMP_InputField InputField => inputField;

	public GameObject TextArea => textArea;

	public TextMeshProUGUI TextArea_Placeholder => textArea_Placeholder;

	public TextMeshProUGUI TextArea_Text => textArea_Text;

	public GameObject ScrollerView => scrollerView;

	public CanvasGroup BgCanvas => bgCanvas;

	public ChatScrollerPresenter Scroller => scroller;

	public GameObject Content => content;

	public TextMeshProUGUI PreviewText => previewText;

	public Image ViewResizer => viewResizer;

	public CanvasGroup ViewResizer_Line => viewResizer_Line;

	public GameObject ViewResizer_TriangleAnchor => viewResizer_TriangleAnchor;

	public Image ViewResizer_TriangleAnchor_Down => viewResizer_TriangleAnchor_Down;

	public Image ViewResizer_TriangleAnchor_Up => viewResizer_TriangleAnchor_Up;
}
