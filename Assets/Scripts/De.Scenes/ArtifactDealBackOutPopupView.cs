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

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Title => null;

	public GameObject PlaceHolder => null;

	public SkeletonGraphic Spine => null;

	public GameObject TextBubble => null;

	public Image TextBubble_BG => null;

	public Image TextBubble_Tail => null;

	public TextMeshProUGUI TextBubble_Text => null;
}
