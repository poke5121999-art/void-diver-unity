using NemoLib.Mvp;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactDealPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private CanvasGroup body;

	[SerializeField]
	private GameObject deal;

	[SerializeField]
	private GameObject cardPlayZone;

	[SerializeField]
	private GameObject cardPlayZone_Animation;

	[SerializeField]
	private GameObject[] cardPlayZone_Animation_Cards;

	[SerializeField]
	private GameObject cardPlayZone_Reroll;

	[SerializeField]
	private Button cardPlayZone_Reroll_Button;

	[SerializeField]
	private Image cardPlayZone_Reroll_Button_Normal;

	[SerializeField]
	private Image cardPlayZone_Reroll_Button_Icon;

	[SerializeField]
	private Image cardPlayZone_Reroll_Button_Line;

	[SerializeField]
	private TextMeshProUGUI cardPlayZone_Reroll_Button_Text;

	[SerializeField]
	private TextMeshProUGUI cardPlayZone_Reroll_Button_TextSub;

	[SerializeField]
	private GameObject dealButtonLayout;

	[SerializeField]
	private Button dealButtonLayout_Button;

	[SerializeField]
	private GameObject dealingArtifact;

	[SerializeField]
	private InventoryGoodsSlotPresenter dealingArtifact_GoodsSlot;

	[SerializeField]
	private GameObject dealingArtifact_Top;

	[SerializeField]
	private GoodsTooltipPresenter dealingArtifactTooltip;

	[SerializeField]
	private GameObject dealingPrice;

	[SerializeField]
	private GameObject dealingPrice_Contents;

	[SerializeField]
	private GameObject dealingPrice_Body;

	[SerializeField]
	private GameObject dealingPrice_Body_Change;

	[SerializeField]
	private TextMeshProUGUI dealingPrice_Body_Change_TextChangeDown;

	[SerializeField]
	private TextMeshProUGUI dealingPrice_Body_Change_TextChangeUp;

	[SerializeField]
	private GameObject dealingPrice_Body_Coin;

	[SerializeField]
	private TextMeshProUGUI dealingPrice_Body_Coin_TextBalanceValue;

	[SerializeField]
	private Image dealingPrice_Body_Coin_TextBalanceValue_BalanceDown;

	[SerializeField]
	private Image dealingPrice_Body_Coin_TextBalanceValue_BalanceNone;

	[SerializeField]
	private Image dealingPrice_Body_Coin_TextBalanceValue_BalanceUp;

	[SerializeField]
	private TextMeshProUGUI dealingPrice_Body_Coin_TextBalanceValue_TextValue;

	[SerializeField]
	private GameObject dealingPrice_Body_DealingPriceBubble;

	[SerializeField]
	private GameObject dealingPrice_Body_DealingPriceBubble_Normal;

	[SerializeField]
	private Image dealingPrice_Body_DealingPriceBubble_Normal_BG;

	[SerializeField]
	private Image dealingPrice_Body_DealingPriceBubble_Normal_BG_Tail;

	[SerializeField]
	private TextMeshProUGUI dealingPrice_Body_DealingPriceBubble_Normal_BG_Text;

	[SerializeField]
	private TextMeshProUGUI dealingPrice_Body_DealingPriceBubble_Normal_BG_TextTagMatch;

	[SerializeField]
	private GameObject dealingPrice_Body_Exp;

	[SerializeField]
	private TextMeshProUGUI dealingPrice_Body_Exp_TextExp;

	[SerializeField]
	private TextMeshProUGUI dealingPrice_Body_Exp_TextExp_TextValue;

	[SerializeField]
	private GameObject giveupButtonLayout;

	[SerializeField]
	private Button giveupButtonLayout_Button;

	[SerializeField]
	private GameObject other;

	[SerializeField]
	private Button other_Button;

	[SerializeField]
	private GameObject placeHolder;

	[SerializeField]
	private GameObject dealedFX;

	[SerializeField]
	private SkeletonGraphic spine;

	[SerializeField]
	private TextMeshProUGUI remainPlayCount;

	[SerializeField]
	private TextMeshProUGUI remainPlayCount_Count;

	[SerializeField]
	private Image remainPlayCountLine;

	[SerializeField]
	private GameObject textBubble;

	[SerializeField]
	private Image textBubble_BG;

	[SerializeField]
	private Image textBubble_Tail;

	[SerializeField]
	private TextMeshProUGUI textBubble_Text;

	[SerializeField]
	private Image requirement;

	[SerializeField]
	private GameObject requirement_Body;

	[SerializeField]
	private GameObject[] requirement_Body_Categories;

	[SerializeField]
	private GameObject requirement_Body_DislikeLine;

	[SerializeField]
	private GameObject[] requirement_Body_DislikePrefixes;

	[SerializeField]
	private GameObject[] requirement_Body_PreferedPrefixes;

	[SerializeField]
	private GameObject requirement_Body_PrefredLine;

	[SerializeField]
	private GameObject requirement_TagMatchedFX;

	[SerializeField]
	private Image requirement_TagMatchedFX_Tier1;

	[SerializeField]
	private Image requirement_TagMatchedFX_Tier2;

	[SerializeField]
	private Image requirement_TagMatchedFX_Tier3;

	[SerializeField]
	private Image requirement_TagMatchedFX_Tier4;

	[SerializeField]
	private Image requirement_TagMatchedFX_Tier5;

	[SerializeField]
	private GameObject select;

	[SerializeField]
	private PlayerInventoryPanelPresenter inventoryPanel;

	[SerializeField]
	private StashPanelPresenter stashPanel;

	[SerializeField]
	private GameObject dealStart;

	[SerializeField]
	private CanvasGroup dealStart_Normal;

	[SerializeField]
	private TextMeshProUGUI dealStart_Normal_TextDesc;

	[SerializeField]
	private GameObject dealSuggestFail;

	[SerializeField]
	private CanvasGroup dealSuggestFail_Normal;

	[SerializeField]
	private TextMeshProUGUI dealSuggestFail_Normal_TextDesc;

	[SerializeField]
	private TextMeshProUGUI dealSuggestFail_Normal_TextFail;

	[SerializeField]
	private GameObject dealSuggestSuccess;

	[SerializeField]
	private CanvasGroup dealSuggestSuccess_Normal;

	[SerializeField]
	private GameObject dealSuggestSuccess_Normal_ParticleImage_Star;

	[SerializeField]
	private TextMeshProUGUI dealSuggestSuccess_Normal_TextDesc;

	[SerializeField]
	private TextMeshProUGUI dealSuggestSuccess_Normal_TextSuccess;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private CanvasGroup top_CurrencyLayout;

	public Button Background => null;

	public GameObject Content => null;

	public CanvasGroup Body => null;

	public GameObject Deal => null;

	public GameObject CardPlayZone => null;

	public GameObject CardPlayZone_Animation => null;

	public GameObject[] CardPlayZone_Animation_Cards => null;

	public GameObject CardPlayZone_Reroll => null;

	public Button CardPlayZone_Reroll_Button => null;

	public Image CardPlayZone_Reroll_Button_Normal => null;

	public Image CardPlayZone_Reroll_Button_Icon => null;

	public Image CardPlayZone_Reroll_Button_Line => null;

	public TextMeshProUGUI CardPlayZone_Reroll_Button_Text => null;

	public TextMeshProUGUI CardPlayZone_Reroll_Button_TextSub => null;

	public GameObject DealButtonLayout => null;

	public Button DealButtonLayout_Button => null;

	public GameObject DealingArtifact => null;

	public InventoryGoodsSlotPresenter DealingArtifact_GoodsSlot => null;

	public GameObject DealingArtifact_Top => null;

	public GoodsTooltipPresenter DealingArtifactTooltip => null;

	public GameObject DealingPrice => null;

	public GameObject DealingPrice_Contents => null;

	public GameObject DealingPrice_Body => null;

	public GameObject DealingPrice_Body_Change => null;

	public TextMeshProUGUI DealingPrice_Body_Change_TextChangeDown => null;

	public TextMeshProUGUI DealingPrice_Body_Change_TextChangeUp => null;

	public GameObject DealingPrice_Body_Coin => null;

	public TextMeshProUGUI DealingPrice_Body_Coin_TextBalanceValue => null;

	public Image DealingPrice_Body_Coin_TextBalanceValue_BalanceDown => null;

	public Image DealingPrice_Body_Coin_TextBalanceValue_BalanceNone => null;

	public Image DealingPrice_Body_Coin_TextBalanceValue_BalanceUp => null;

	public TextMeshProUGUI DealingPrice_Body_Coin_TextBalanceValue_TextValue => null;

	public GameObject DealingPrice_Body_DealingPriceBubble => null;

	public GameObject DealingPrice_Body_DealingPriceBubble_Normal => null;

	public Image DealingPrice_Body_DealingPriceBubble_Normal_BG => null;

	public Image DealingPrice_Body_DealingPriceBubble_Normal_BG_Tail => null;

	public TextMeshProUGUI DealingPrice_Body_DealingPriceBubble_Normal_BG_Text => null;

	public TextMeshProUGUI DealingPrice_Body_DealingPriceBubble_Normal_BG_TextTagMatch => null;

	public GameObject DealingPrice_Body_Exp => null;

	public TextMeshProUGUI DealingPrice_Body_Exp_TextExp => null;

	public TextMeshProUGUI DealingPrice_Body_Exp_TextExp_TextValue => null;

	public GameObject GiveupButtonLayout => null;

	public Button GiveupButtonLayout_Button => null;

	public GameObject Other => null;

	public Button Other_Button => null;

	public GameObject PlaceHolder => null;

	public GameObject DealedFX => null;

	public SkeletonGraphic Spine => null;

	public TextMeshProUGUI RemainPlayCount => null;

	public TextMeshProUGUI RemainPlayCount_Count => null;

	public Image RemainPlayCountLine => null;

	public GameObject TextBubble => null;

	public Image TextBubble_BG => null;

	public Image TextBubble_Tail => null;

	public TextMeshProUGUI TextBubble_Text => null;

	public Image Requirement => null;

	public GameObject Requirement_Body => null;

	public GameObject[] Requirement_Body_Categories => null;

	public GameObject Requirement_Body_DislikeLine => null;

	public GameObject[] Requirement_Body_DislikePrefixes => null;

	public GameObject[] Requirement_Body_PreferedPrefixes => null;

	public GameObject Requirement_Body_PrefredLine => null;

	public GameObject Requirement_TagMatchedFX => null;

	public Image Requirement_TagMatchedFX_Tier1 => null;

	public Image Requirement_TagMatchedFX_Tier2 => null;

	public Image Requirement_TagMatchedFX_Tier3 => null;

	public Image Requirement_TagMatchedFX_Tier4 => null;

	public Image Requirement_TagMatchedFX_Tier5 => null;

	public GameObject Select => null;

	public PlayerInventoryPanelPresenter InventoryPanel => null;

	public StashPanelPresenter StashPanel => null;

	public GameObject DealStart => null;

	public CanvasGroup DealStart_Normal => null;

	public TextMeshProUGUI DealStart_Normal_TextDesc => null;

	public GameObject DealSuggestFail => null;

	public CanvasGroup DealSuggestFail_Normal => null;

	public TextMeshProUGUI DealSuggestFail_Normal_TextDesc => null;

	public TextMeshProUGUI DealSuggestFail_Normal_TextFail => null;

	public GameObject DealSuggestSuccess => null;

	public CanvasGroup DealSuggestSuccess_Normal => null;

	public GameObject DealSuggestSuccess_Normal_ParticleImage_Star => null;

	public TextMeshProUGUI DealSuggestSuccess_Normal_TextDesc => null;

	public TextMeshProUGUI DealSuggestSuccess_Normal_TextSuccess => null;

	public GameObject Top => null;

	public CanvasGroup Top_CurrencyLayout => null;
}
