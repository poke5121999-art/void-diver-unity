using NemoLib.Mvp;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactDealBackOutPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private GameObject placeHolder;

	[SerializeField]
	private SkeletonGraphic spine;

	[SerializeField]
	private GameObject textBubble;

	[SerializeField]
	private Image textBubble_BG;

	[SerializeField]
	private Image textBubble_Tail;

	[SerializeField]
	private TextMeshProUGUI textBubble_Text;

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Title => title;

	public GameObject PlaceHolder => placeHolder;

	public SkeletonGraphic Spine => spine;

	public GameObject TextBubble => textBubble;

	public Image TextBubble_BG => textBubble_BG;

	public Image TextBubble_Tail => textBubble_Tail;

	public TextMeshProUGUI TextBubble_Text => textBubble_Text;
}
