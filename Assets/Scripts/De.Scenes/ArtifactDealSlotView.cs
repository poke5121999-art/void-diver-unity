using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactDealSlotView : View
{
	[SerializeField]
	private Button slot;

	[SerializeField]
	private GameObject cancel;

	[SerializeField]
	private Image frameCategory;

	[SerializeField]
	private Image frameCategory_Outline;

	[SerializeField]
	private TextMeshProUGUI frameCategory_Text;

	[SerializeField]
	private Image frameNegativePrefix;

	[SerializeField]
	private GameObject frameNegativePrefix_ApearFx;

	[SerializeField]
	private GameObject frameNegativePrefix_negative;

	[SerializeField]
	private Image frameNegativePrefix_negative_Bg;

	[SerializeField]
	private TextMeshProUGUI frameNegativePrefix_negative_TextNum;

	[SerializeField]
	private Image frameNegativePrefix_Outline;

	[SerializeField]
	private Image frameNegativePrefix_Outline_Matched;

	[SerializeField]
	private GameObject frameNegativePrefix_positive;

	[SerializeField]
	private Image frameNegativePrefix_positive_Bg;

	[SerializeField]
	private TextMeshProUGUI frameNegativePrefix_positive_TextNum;

	[SerializeField]
	private TextMeshProUGUI frameNegativePrefix_Text;

	[SerializeField]
	private Image framePositivePrefix;

	[SerializeField]
	private GameObject framePositivePrefix_ApearFx;

	[SerializeField]
	private GameObject framePositivePrefix_negative;

	[SerializeField]
	private Image framePositivePrefix_negative_Bg;

	[SerializeField]
	private TextMeshProUGUI framePositivePrefix_negative_TextNum;

	[SerializeField]
	private Image framePositivePrefix_Outline;

	[SerializeField]
	private Image framePositivePrefix_Outline_Matched;

	[SerializeField]
	private GameObject framePositivePrefix_positive;

	[SerializeField]
	private Image framePositivePrefix_positive_Bg;

	[SerializeField]
	private TextMeshProUGUI framePositivePrefix_positive_TextNum;

	[SerializeField]
	private TextMeshProUGUI framePositivePrefix_Text;

	public Button Slot => slot;

	public GameObject Cancel => cancel;

	public Image FrameCategory => frameCategory;

	public Image FrameCategory_Outline => frameCategory_Outline;

	public TextMeshProUGUI FrameCategory_Text => frameCategory_Text;

	public Image FrameNegativePrefix => frameNegativePrefix;

	public GameObject FrameNegativePrefix_ApearFx => frameNegativePrefix_ApearFx;

	public GameObject FrameNegativePrefix_negative => frameNegativePrefix_negative;

	public Image FrameNegativePrefix_negative_Bg => frameNegativePrefix_negative_Bg;

	public TextMeshProUGUI FrameNegativePrefix_negative_TextNum => frameNegativePrefix_negative_TextNum;

	public Image FrameNegativePrefix_Outline => frameNegativePrefix_Outline;

	public Image FrameNegativePrefix_Outline_Matched => frameNegativePrefix_Outline_Matched;

	public GameObject FrameNegativePrefix_positive => frameNegativePrefix_positive;

	public Image FrameNegativePrefix_positive_Bg => frameNegativePrefix_positive_Bg;

	public TextMeshProUGUI FrameNegativePrefix_positive_TextNum => frameNegativePrefix_positive_TextNum;

	public TextMeshProUGUI FrameNegativePrefix_Text => frameNegativePrefix_Text;

	public Image FramePositivePrefix => framePositivePrefix;

	public GameObject FramePositivePrefix_ApearFx => framePositivePrefix_ApearFx;

	public GameObject FramePositivePrefix_negative => framePositivePrefix_negative;

	public Image FramePositivePrefix_negative_Bg => framePositivePrefix_negative_Bg;

	public TextMeshProUGUI FramePositivePrefix_negative_TextNum => framePositivePrefix_negative_TextNum;

	public Image FramePositivePrefix_Outline => framePositivePrefix_Outline;

	public Image FramePositivePrefix_Outline_Matched => framePositivePrefix_Outline_Matched;

	public GameObject FramePositivePrefix_positive => framePositivePrefix_positive;

	public Image FramePositivePrefix_positive_Bg => framePositivePrefix_positive_Bg;

	public TextMeshProUGUI FramePositivePrefix_positive_TextNum => framePositivePrefix_positive_TextNum;

	public TextMeshProUGUI FramePositivePrefix_Text => framePositivePrefix_Text;
}
