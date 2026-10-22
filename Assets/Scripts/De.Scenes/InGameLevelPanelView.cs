using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameLevelPanelView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private Slider expSlider;

	[SerializeField]
	private TextMeshProUGUI expText;

	[SerializeField]
	private Image iconClassic;

	[SerializeField]
	private Image iconJunior;

	[SerializeField]
	private Image iconStory;

	[SerializeField]
	private Image iconVeteran;

	[SerializeField]
	private TextMeshProUGUI levelText;

	[SerializeField]
	private GameObject levelUpTag;

	public GameObject Contents => contents;

	public Slider ExpSlider => expSlider;

	public TextMeshProUGUI ExpText => expText;

	public Image IconClassic => iconClassic;

	public Image IconJunior => iconJunior;

	public Image IconStory => iconStory;

	public Image IconVeteran => iconVeteran;

	public TextMeshProUGUI LevelText => levelText;

	public GameObject LevelUpTag => levelUpTag;
}
