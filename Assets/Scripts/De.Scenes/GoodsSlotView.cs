using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoodsSlotView : View
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
	private GameObject missionMark;

	[SerializeField]
	private GameObject questMark;

	[SerializeField]
	private Image subIcon;

	public GameObject Contents => contents;

	public Button Slot => slot;

	public Image BackLine => backLine;

	public Image BackSelect => backSelect;

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

	public GameObject MissionMark => missionMark;

	public GameObject QuestMark => questMark;

	public Image SubIcon => subIcon;
}
