using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignSlotView : View
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private TextMeshProUGUI boss;

	[SerializeField]
	private TextMeshProUGUI chapter;

	[SerializeField]
	private Image completedMark;

	[SerializeField]
	private Image iconAnomaly;

	[SerializeField]
	private GameObject levelLayout;

	[SerializeField]
	private TextMeshProUGUI levelLayout_Text;

	[SerializeField]
	private TextMeshProUGUI levelLayout_TextLevel;

	[SerializeField]
	private Image lockIcon;

	[SerializeField]
	private Image newTag;

	[SerializeField]
	private TextMeshProUGUI sector;

	[SerializeField]
	private TextMeshProUGUI text;

	public Button Button => null;

	public Image Normal => null;

	public TextMeshProUGUI Boss => null;

	public TextMeshProUGUI Chapter => null;

	public Image CompletedMark => null;

	public Image IconAnomaly => null;

	public GameObject LevelLayout => null;

	public TextMeshProUGUI LevelLayout_Text => null;

	public TextMeshProUGUI LevelLayout_TextLevel => null;

	public Image LockIcon => null;

	public Image NewTag => null;

	public TextMeshProUGUI Sector => null;

	public TextMeshProUGUI Text => null;
}
