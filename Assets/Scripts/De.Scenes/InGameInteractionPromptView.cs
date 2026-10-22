using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameInteractionPromptView : View
{
	[SerializeField]
	private GameObject pivot;

	[SerializeField]
	private GameObject baseKeyGuideTextHold;

	[SerializeField]
	private TextMeshProUGUI descText;

	[SerializeField]
	private GameObject keyboard;

	[SerializeField]
	private Image bG;

	[SerializeField]
	private Image gauge;

	[SerializeField]
	private Image gaugeFill;

	[SerializeField]
	private TextMeshProUGUI holdKeyText;

	[SerializeField]
	private TextMeshProUGUI titleText;

	[SerializeField]
	private GameObject goodsCount;

	[SerializeField]
	private TextMeshProUGUI goodsCount_Text;

	public GameObject Pivot => null;

	public GameObject BaseKeyGuideTextHold => null;

	public TextMeshProUGUI DescText => null;

	public GameObject Keyboard => null;

	public Image BG => null;

	public Image Gauge => null;

	public Image GaugeFill => null;

	public TextMeshProUGUI HoldKeyText => null;

	public TextMeshProUGUI TitleText => null;

	public GameObject GoodsCount => null;

	public TextMeshProUGUI GoodsCount_Text => null;
}
