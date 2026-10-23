using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameQuickSlotView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject cooltime;

	[SerializeField]
	private Image cooltime_Image;

	[SerializeField]
	private TextMeshProUGUI cooltime_Text;

	[SerializeField]
	private GoodsSlotPresenter goodsSlot;

	[SerializeField]
	private GameObject goodsSlot_Contents;

	[SerializeField]
	private Button goodsSlot_Slot;

	[SerializeField]
	private Image goodsSlot_BackLine;

	[SerializeField]
	private Image goodsSlot_BackSelect;

	[SerializeField]
	private Image goodsSlot_Normal;

	[SerializeField]
	private CanvasGroup goodsSlot_Artifact;

	[SerializeField]
	private GameObject[] goodsSlot_Artifact_Group;

	[SerializeField]
	private Image goodsSlot_Count;

	[SerializeField]
	private TextMeshProUGUI goodsSlot_Count_Text;

	[SerializeField]
	private Image goodsSlot_Icon;

	[SerializeField]
	private Image goodsSlot_ItemMark;

	[SerializeField]
	private Image goodsSlot_LevelBg;

	[SerializeField]
	private Image goodsSlot_Line;

	[SerializeField]
	private Image goodsSlot_LineFX;

	[SerializeField]
	private GameObject goodsSlot_MissionMark;

	[SerializeField]
	private GameObject goodsSlot_QuestMark;

	[SerializeField]
	private Image goodsSlot_SubIcon;

	[SerializeField]
	private GameObject keyPrompt;

	public GameObject Contents => contents;

	public GameObject Cooltime => cooltime;

	public Image Cooltime_Image => cooltime_Image;

	public TextMeshProUGUI Cooltime_Text => cooltime_Text;

	public GoodsSlotPresenter GoodsSlot => goodsSlot;

	public GameObject GoodsSlot_Contents => goodsSlot_Contents;

	public Button GoodsSlot_Slot => goodsSlot_Slot;

	public Image GoodsSlot_BackLine => goodsSlot_BackLine;

	public Image GoodsSlot_BackSelect => goodsSlot_BackSelect;

	public Image GoodsSlot_Normal => goodsSlot_Normal;

	public CanvasGroup GoodsSlot_Artifact => goodsSlot_Artifact;

	public GameObject[] GoodsSlot_Artifact_Group => goodsSlot_Artifact_Group;

	public Image GoodsSlot_Count => goodsSlot_Count;

	public TextMeshProUGUI GoodsSlot_Count_Text => goodsSlot_Count_Text;

	public Image GoodsSlot_Icon => goodsSlot_Icon;

	public Image GoodsSlot_ItemMark => goodsSlot_ItemMark;

	public Image GoodsSlot_LevelBg => goodsSlot_LevelBg;

	public Image GoodsSlot_Line => goodsSlot_Line;

	public Image GoodsSlot_LineFX => goodsSlot_LineFX;

	public GameObject GoodsSlot_MissionMark => goodsSlot_MissionMark;

	public GameObject GoodsSlot_QuestMark => goodsSlot_QuestMark;

	public Image GoodsSlot_SubIcon => goodsSlot_SubIcon;

	public GameObject KeyPrompt => keyPrompt;
}
