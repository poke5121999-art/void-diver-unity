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

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Description => description;

	public TextMeshProUGUI Description_Text => description_Text;

	public Image NewTag => newTag;

	public Image Owned => owned;

	public GameObject Price => price;

	public GameObject Price_GoldText => price_GoldText;

	public Image Price_GoldText_Icon => price_GoldText_Icon;

	public TextMeshProUGUI Price_GoldText_Value => price_GoldText_Value;

	public GameObject RecordComplete => recordComplete;

	public GameObject StashAmount => stashAmount;

	public TextMeshProUGUI StashAmount_Value => stashAmount_Value;

	public Image Title => title;

	public TextMeshProUGUI Title_GradeText => title_GradeText;

	public Image Title_Mark => title_Mark;

	public TextMeshProUGUI Title_Name => title_Name;

	public Image Title_TypeIcon => title_TypeIcon;

	public GameObject Frame => frame;

	public GameObject BaseFrame => baseFrame;
}
