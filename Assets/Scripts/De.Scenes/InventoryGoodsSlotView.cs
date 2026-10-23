using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGoodsSlotView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private Button slot;

	[SerializeField]
	private Image backLine;

	[SerializeField]
	private Image backSelect;

	[SerializeField]
	private GameObject highlight;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private CanvasGroup artifact;

	[SerializeField]
	private GameObject[] artifact_Group;

	[SerializeField]
	private Image count;

	[SerializeField]
	private TextMeshProUGUI count_Text;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private Image itemMark;

	[SerializeField]
	private Image levelBg;

	[SerializeField]
	private Image line;

	[SerializeField]
	private Image lineFX;

	[SerializeField]
	private GameObject locked;

	[SerializeField]
	private GameObject missionMark;

	[SerializeField]
	private GameObject questMark;

	[SerializeField]
	private Image subIcon;

	[SerializeField]
	private GameObject revealing;

	[SerializeField]
	private Image revealing_Normal;

	[SerializeField]
	private GameObject slotCategoryIcon;

	[SerializeField]
	private GameObject accessoryIcon;

	[SerializeField]
	private GameObject artifactIcon;

	[SerializeField]
	private GameObject weaponIcon;

	[SerializeField]
	private GameObject unrevealed;

	public GameObject Contents => contents;

	public Button Slot => slot;

	public Image BackLine => backLine;

	public Image BackSelect => backSelect;

	public GameObject Highlight => highlight;

	public Image Normal => normal;

	public CanvasGroup Artifact => artifact;

	public GameObject[] Artifact_Group => artifact_Group;

	public Image Count => count;

	public TextMeshProUGUI Count_Text => count_Text;

	public Image Icon => icon;

	public Image ItemMark => itemMark;

	public Image LevelBg => levelBg;

	public Image Line => line;

	public Image LineFX => lineFX;

	public GameObject Locked => locked;

	public GameObject MissionMark => missionMark;

	public GameObject QuestMark => questMark;

	public Image SubIcon => subIcon;

	public GameObject Revealing => revealing;

	public Image Revealing_Normal => revealing_Normal;

	public GameObject SlotCategoryIcon => slotCategoryIcon;

	public GameObject AccessoryIcon => accessoryIcon;

	public GameObject ArtifactIcon => artifactIcon;

	public GameObject WeaponIcon => weaponIcon;

	public GameObject Unrevealed => unrevealed;
}
