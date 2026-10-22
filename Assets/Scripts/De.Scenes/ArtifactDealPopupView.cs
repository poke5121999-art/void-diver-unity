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

	public Button Background => background;

	public GameObject Content => content;

	public CanvasGroup Body => body;

	public GameObject Deal => deal;

	public GameObject CardPlayZone => cardPlayZone;

	public GameObject CardPlayZone_Animation => cardPlayZone_Animation;

	public GameObject[] CardPlayZone_Animation_Cards => cardPlayZone_Animation_Cards;

	public GameObject CardPlayZone_Reroll => cardPlayZone_Reroll;

	public Button CardPlayZone_Reroll_Button => cardPlayZone_Reroll_Button;

	public Image CardPlayZone_Reroll_Button_Normal => cardPlayZone_Reroll_Button_Normal;

	public Image CardPlayZone_Reroll_Button_Icon => cardPlayZone_Reroll_Button_Icon;

	public Image CardPlayZone_Reroll_Button_Line => cardPlayZone_Reroll_Button_Line;

	public TextMeshProUGUI CardPlayZone_Reroll_Button_Text => cardPlayZone_Reroll_Button_Text;

	public TextMeshProUGUI CardPlayZone_Reroll_Button_TextSub => cardPlayZone_Reroll_Button_TextSub;

	public GameObject DealButtonLayout => dealButtonLayout;

	public Button DealButtonLayout_Button => dealButtonLayout_Button;

	public GameObject DealingArtifact => dealingArtifact;

	public InventoryGoodsSlotPresenter DealingArtifact_GoodsSlot => dealingArtifact_GoodsSlot;

	public GameObject DealingArtifact_Top => dealingArtifact_Top;

	public GoodsTooltipPresenter DealingArtifactTooltip => dealingArtifactTooltip;

	public GameObject DealingPrice => dealingPrice;

	public GameObject DealingPrice_Contents => dealingPrice_Contents;

	public GameObject DealingPrice_Body => dealingPrice_Body;

	public GameObject DealingPrice_Body_Change => dealingPrice_Body_Change;

	public TextMeshProUGUI DealingPrice_Body_Change_TextChangeDown => dealingPrice_Body_Change_TextChangeDown;

	public TextMeshProUGUI DealingPrice_Body_Change_TextChangeUp => dealingPrice_Body_Change_TextChangeUp;

	public GameObject DealingPrice_Body_Coin => dealingPrice_Body_Coin;

	public TextMeshProUGUI DealingPrice_Body_Coin_TextBalanceValue => dealingPrice_Body_Coin_TextBalanceValue;

	public Image DealingPrice_Body_Coin_TextBalanceValue_BalanceDown => dealingPrice_Body_Coin_TextBalanceValue_BalanceDown;

	public Image DealingPrice_Body_Coin_TextBalanceValue_BalanceNone => dealingPrice_Body_Coin_TextBalanceValue_BalanceNone;

	public Image DealingPrice_Body_Coin_TextBalanceValue_BalanceUp => dealingPrice_Body_Coin_TextBalanceValue_BalanceUp;

	public TextMeshProUGUI DealingPrice_Body_Coin_TextBalanceValue_TextValue => dealingPrice_Body_Coin_TextBalanceValue_TextValue;

	public GameObject DealingPrice_Body_DealingPriceBubble => dealingPrice_Body_DealingPriceBubble;

	public GameObject DealingPrice_Body_DealingPriceBubble_Normal => dealingPrice_Body_DealingPriceBubble_Normal;

	public Image DealingPrice_Body_DealingPriceBubble_Normal_BG => dealingPrice_Body_DealingPriceBubble_Normal_BG;

	public Image DealingPrice_Body_DealingPriceBubble_Normal_BG_Tail => dealingPrice_Body_DealingPriceBubble_Normal_BG_Tail;

	public TextMeshProUGUI DealingPrice_Body_DealingPriceBubble_Normal_BG_Text => dealingPrice_Body_DealingPriceBubble_Normal_BG_Text;

	public TextMeshProUGUI DealingPrice_Body_DealingPriceBubble_Normal_BG_TextTagMatch => dealingPrice_Body_DealingPriceBubble_Normal_BG_TextTagMatch;

	public GameObject DealingPrice_Body_Exp => dealingPrice_Body_Exp;

	public TextMeshProUGUI DealingPrice_Body_Exp_TextExp => dealingPrice_Body_Exp_TextExp;

	public TextMeshProUGUI DealingPrice_Body_Exp_TextExp_TextValue => dealingPrice_Body_Exp_TextExp_TextValue;

	public GameObject GiveupButtonLayout => giveupButtonLayout;

	public Button GiveupButtonLayout_Button => giveupButtonLayout_Button;

	public GameObject Other => other;

	public Button Other_Button => other_Button;

	public GameObject PlaceHolder => placeHolder;

	public GameObject DealedFX => dealedFX;

	public SkeletonGraphic Spine => spine;

	public TextMeshProUGUI RemainPlayCount => remainPlayCount;

	public TextMeshProUGUI RemainPlayCount_Count => remainPlayCount_Count;

	public Image RemainPlayCountLine => remainPlayCountLine;

	public GameObject TextBubble => textBubble;

	public Image TextBubble_BG => textBubble_BG;

	public Image TextBubble_Tail => textBubble_Tail;

	public TextMeshProUGUI TextBubble_Text => textBubble_Text;

	public Image Requirement => requirement;

	public GameObject Requirement_Body => requirement_Body;

	public GameObject[] Requirement_Body_Categories => requirement_Body_Categories;

	public GameObject Requirement_Body_DislikeLine => requirement_Body_DislikeLine;

	public GameObject[] Requirement_Body_DislikePrefixes => requirement_Body_DislikePrefixes;

	public GameObject[] Requirement_Body_PreferedPrefixes => requirement_Body_PreferedPrefixes;

	public GameObject Requirement_Body_PrefredLine => requirement_Body_PrefredLine;

	public GameObject Requirement_TagMatchedFX => requirement_TagMatchedFX;

	public Image Requirement_TagMatchedFX_Tier1 => requirement_TagMatchedFX_Tier1;

	public Image Requirement_TagMatchedFX_Tier2 => requirement_TagMatchedFX_Tier2;

	public Image Requirement_TagMatchedFX_Tier3 => requirement_TagMatchedFX_Tier3;

	public Image Requirement_TagMatchedFX_Tier4 => requirement_TagMatchedFX_Tier4;

	public Image Requirement_TagMatchedFX_Tier5 => requirement_TagMatchedFX_Tier5;

	public GameObject Select => select;

	public PlayerInventoryPanelPresenter InventoryPanel => inventoryPanel;

	public StashPanelPresenter StashPanel => stashPanel;

	public GameObject DealStart => dealStart;

	public CanvasGroup DealStart_Normal => dealStart_Normal;

	public TextMeshProUGUI DealStart_Normal_TextDesc => dealStart_Normal_TextDesc;

	public GameObject DealSuggestFail => dealSuggestFail;

	public CanvasGroup DealSuggestFail_Normal => dealSuggestFail_Normal;

	public TextMeshProUGUI DealSuggestFail_Normal_TextDesc => dealSuggestFail_Normal_TextDesc;

	public TextMeshProUGUI DealSuggestFail_Normal_TextFail => dealSuggestFail_Normal_TextFail;

	public GameObject DealSuggestSuccess => dealSuggestSuccess;

	public CanvasGroup DealSuggestSuccess_Normal => dealSuggestSuccess_Normal;

	public GameObject DealSuggestSuccess_Normal_ParticleImage_Star => dealSuggestSuccess_Normal_ParticleImage_Star;

	public TextMeshProUGUI DealSuggestSuccess_Normal_TextDesc => dealSuggestSuccess_Normal_TextDesc;

	public TextMeshProUGUI DealSuggestSuccess_Normal_TextSuccess => dealSuggestSuccess_Normal_TextSuccess;

	public GameObject Top => top;

	public CanvasGroup Top_CurrencyLayout => top_CurrencyLayout;
}
