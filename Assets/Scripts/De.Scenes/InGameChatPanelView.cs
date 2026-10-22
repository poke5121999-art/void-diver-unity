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

	public GameObject Contents => null;

	public GameObject Layout => null;

	public GameObject Enter => null;

	public GameObject InputFieldAnchor => null;

	public TMP_InputField InputField => null;

	public GameObject TextArea => null;

	public TextMeshProUGUI TextArea_Placeholder => null;

	public TextMeshProUGUI TextArea_Text => null;

	public GameObject ScrollerView => null;

	public CanvasGroup BgCanvas => null;

	public ChatScrollerPresenter Scroller => null;

	public GameObject Content => null;

	public TextMeshProUGUI PreviewText => null;

	public Image ViewResizer => null;

	public CanvasGroup ViewResizer_Line => null;

	public GameObject ViewResizer_TriangleAnchor => null;

	public Image ViewResizer_TriangleAnchor_Down => null;

	public Image ViewResizer_TriangleAnchor_Up => null;
}
