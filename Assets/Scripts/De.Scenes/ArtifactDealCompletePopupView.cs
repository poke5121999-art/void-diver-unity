using NemoLib.Mvp;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactDealCompletePopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject coinAttraction;

	[SerializeField]
	private GameObject coinAttraction_Locate;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private GameObject frame;

	[SerializeField]
	private GameObject dealDetail;

	[SerializeField]
	private Image dim;

	[SerializeField]
	private GameObject exp;

	[SerializeField]
	private TextMeshProUGUI exp_TextExp;

	[SerializeField]
	private TextMeshProUGUI exp_TextExp_TextValue;

	[SerializeField]
	private GameObject gold;

	[SerializeField]
	private TextMeshProUGUI gold_TextBalanceValue;

	[SerializeField]
	private Image gold_TextBalanceValue_BalanceDown;

	[SerializeField]
	private Image gold_TextBalanceValue_BalanceNone;

	[SerializeField]
	private Image gold_TextBalanceValue_BalanceUp;

	[SerializeField]
	private TextMeshProUGUI gold_TextBalanceValue_TextValue;

	[SerializeField]
	private InventoryGoodsSlotPresenter goodsSlot;

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

	public GameObject CoinAttraction => null;

	public GameObject CoinAttraction_Locate => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Title => null;

	public GameObject Frame => null;

	public GameObject DealDetail => null;

	public Image Dim => null;

	public GameObject Exp => null;

	public TextMeshProUGUI Exp_TextExp => null;

	public TextMeshProUGUI Exp_TextExp_TextValue => null;

	public GameObject Gold => null;

	public TextMeshProUGUI Gold_TextBalanceValue => null;

	public Image Gold_TextBalanceValue_BalanceDown => null;

	public Image Gold_TextBalanceValue_BalanceNone => null;

	public Image Gold_TextBalanceValue_BalanceUp => null;

	public TextMeshProUGUI Gold_TextBalanceValue_TextValue => null;

	public InventoryGoodsSlotPresenter GoodsSlot => null;

	public GameObject PlaceHolder => null;

	public SkeletonGraphic Spine => null;

	public GameObject TextBubble => null;

	public Image TextBubble_BG => null;

	public Image TextBubble_Tail => null;

	public TextMeshProUGUI TextBubble_Text => null;
}
