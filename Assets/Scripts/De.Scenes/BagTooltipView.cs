using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagTooltipView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject description;

	[SerializeField]
	private TextMeshProUGUI description_Text;

	[SerializeField]
	private Image newTag;

	[SerializeField]
	private Image owned;

	[SerializeField]
	private GameObject price;

	[SerializeField]
	private GameObject price_GoldText;

	[SerializeField]
	private Image price_GoldText_Icon;

	[SerializeField]
	private TextMeshProUGUI price_GoldText_Value;

	[SerializeField]
	private GameObject recordComplete;

	[SerializeField]
	private GameObject stashAmount;

	[SerializeField]
	private TextMeshProUGUI stashAmount_Value;

	[SerializeField]
	private Image title;

	[SerializeField]
	private TextMeshProUGUI title_GradeText;

	[SerializeField]
	private Image title_Mark;

	[SerializeField]
	private TextMeshProUGUI title_Name;

	[SerializeField]
	private Image title_TypeIcon;

	[SerializeField]
	private GameObject frame;

	[SerializeField]
	private GameObject baseFrame;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Description => null;

	public TextMeshProUGUI Description_Text => null;

	public Image NewTag => null;

	public Image Owned => null;

	public GameObject Price => null;

	public GameObject Price_GoldText => null;

	public Image Price_GoldText_Icon => null;

	public TextMeshProUGUI Price_GoldText_Value => null;

	public GameObject RecordComplete => null;

	public GameObject StashAmount => null;

	public TextMeshProUGUI StashAmount_Value => null;

	public Image Title => null;

	public TextMeshProUGUI Title_GradeText => null;

	public Image Title_Mark => null;

	public TextMeshProUGUI Title_Name => null;

	public Image Title_TypeIcon => null;

	public GameObject Frame => null;

	public GameObject BaseFrame => null;
}
