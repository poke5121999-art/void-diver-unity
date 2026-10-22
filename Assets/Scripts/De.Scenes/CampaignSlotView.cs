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

	public Button Button => button;

	public Image Normal => normal;

	public TextMeshProUGUI Boss => boss;

	public TextMeshProUGUI Chapter => chapter;

	public Image CompletedMark => completedMark;

	public Image IconAnomaly => iconAnomaly;

	public GameObject LevelLayout => levelLayout;

	public TextMeshProUGUI LevelLayout_Text => levelLayout_Text;

	public TextMeshProUGUI LevelLayout_TextLevel => levelLayout_TextLevel;

	public Image LockIcon => lockIcon;

	public Image NewTag => newTag;

	public TextMeshProUGUI Sector => sector;

	public TextMeshProUGUI Text => text;
}
