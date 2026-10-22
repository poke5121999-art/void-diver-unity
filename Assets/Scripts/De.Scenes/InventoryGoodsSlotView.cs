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

	public GameObject Contents => null;

	public Button Slot => null;

	public Image BackLine => null;

	public Image BackSelect => null;

	public GameObject Highlight => null;

	public Image Normal => null;

	public CanvasGroup Artifact => null;

	public GameObject[] Artifact_Group => null;

	public Image Count => null;

	public TextMeshProUGUI Count_Text => null;

	public Image Icon => null;

	public Image ItemMark => null;

	public Image LevelBg => null;

	public Image Line => null;

	public Image LineFX => null;

	public GameObject Locked => null;

	public GameObject MissionMark => null;

	public GameObject QuestMark => null;

	public Image SubIcon => null;

	public GameObject Revealing => null;

	public Image Revealing_Normal => null;

	public GameObject SlotCategoryIcon => null;

	public GameObject AccessoryIcon => null;

	public GameObject ArtifactIcon => null;

	public GameObject WeaponIcon => null;

	public GameObject Unrevealed => null;
}
