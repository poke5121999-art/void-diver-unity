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

	public GameObject Pivot => pivot;

	public GameObject BaseKeyGuideTextHold => baseKeyGuideTextHold;

	public TextMeshProUGUI DescText => descText;

	public GameObject Keyboard => keyboard;

	public Image BG => bG;

	public Image Gauge => gauge;

	public Image GaugeFill => gaugeFill;

	public TextMeshProUGUI HoldKeyText => holdKeyText;

	public TextMeshProUGUI TitleText => titleText;

	public GameObject GoodsCount => goodsCount;

	public TextMeshProUGUI GoodsCount_Text => goodsCount_Text;
}
