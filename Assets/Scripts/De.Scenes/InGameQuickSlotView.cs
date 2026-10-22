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

	public GameObject Contents => null;

	public GameObject Cooltime => null;

	public Image Cooltime_Image => null;

	public TextMeshProUGUI Cooltime_Text => null;

	public GoodsSlotPresenter GoodsSlot => null;

	public GameObject GoodsSlot_Contents => null;

	public Button GoodsSlot_Slot => null;

	public Image GoodsSlot_BackLine => null;

	public Image GoodsSlot_BackSelect => null;

	public Image GoodsSlot_Normal => null;

	public CanvasGroup GoodsSlot_Artifact => null;

	public GameObject[] GoodsSlot_Artifact_Group => null;

	public Image GoodsSlot_Count => null;

	public TextMeshProUGUI GoodsSlot_Count_Text => null;

	public Image GoodsSlot_Icon => null;

	public Image GoodsSlot_ItemMark => null;

	public Image GoodsSlot_LevelBg => null;

	public Image GoodsSlot_Line => null;

	public Image GoodsSlot_LineFX => null;

	public GameObject GoodsSlot_MissionMark => null;

	public GameObject GoodsSlot_QuestMark => null;

	public Image GoodsSlot_SubIcon => null;

	public GameObject KeyPrompt => null;
}
