using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraggingGoodsSlotView : View
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

	public GameObject Contents => null;

	public Button Slot => null;

	public Image BackLine => null;

	public Image BackSelect => null;

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

	public GameObject MissionMark => null;

	public GameObject QuestMark => null;

	public Image SubIcon => null;
}
