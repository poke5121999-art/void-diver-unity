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

	public Button Background => background;

	public GameObject CoinAttraction => coinAttraction;

	public GameObject CoinAttraction_Locate => coinAttraction_Locate;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Title => title;

	public GameObject Frame => frame;

	public GameObject DealDetail => dealDetail;

	public Image Dim => dim;

	public GameObject Exp => exp;

	public TextMeshProUGUI Exp_TextExp => exp_TextExp;

	public TextMeshProUGUI Exp_TextExp_TextValue => exp_TextExp_TextValue;

	public GameObject Gold => gold;

	public TextMeshProUGUI Gold_TextBalanceValue => gold_TextBalanceValue;

	public Image Gold_TextBalanceValue_BalanceDown => gold_TextBalanceValue_BalanceDown;

	public Image Gold_TextBalanceValue_BalanceNone => gold_TextBalanceValue_BalanceNone;

	public Image Gold_TextBalanceValue_BalanceUp => gold_TextBalanceValue_BalanceUp;

	public TextMeshProUGUI Gold_TextBalanceValue_TextValue => gold_TextBalanceValue_TextValue;

	public InventoryGoodsSlotPresenter GoodsSlot => goodsSlot;

	public GameObject PlaceHolder => placeHolder;

	public SkeletonGraphic Spine => spine;

	public GameObject TextBubble => textBubble;

	public Image TextBubble_BG => textBubble_BG;

	public Image TextBubble_Tail => textBubble_Tail;

	public TextMeshProUGUI TextBubble_Text => textBubble_Text;
}
