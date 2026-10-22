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

	public GameObject Contents => null;

	public Slider ExpSlider => null;

	public TextMeshProUGUI ExpText => null;

	public Image IconClassic => null;

	public Image IconJunior => null;

	public Image IconStory => null;

	public Image IconVeteran => null;

	public TextMeshProUGUI LevelText => null;

	public GameObject LevelUpTag => null;
}
