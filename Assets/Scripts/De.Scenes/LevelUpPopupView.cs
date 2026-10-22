using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject root;

	[SerializeField]
	private TextMeshProUGUI desc;

	[SerializeField]
	private Image iconLevel;

	[SerializeField]
	private TextMeshProUGUI level;

	[SerializeField]
	private Image levelUpFrame01;

	[SerializeField]
	private Image levelUpFrame02;

	[SerializeField]
	private Image levelUpFrame03;

	[SerializeField]
	private Image levelUpFrame04;

	[SerializeField]
	private GameObject particleStar;

	[SerializeField]
	private TextMeshProUGUI title;

	[SerializeField]
	private TextMeshProUGUI tMPDecoText;

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Root => root;

	public TextMeshProUGUI Desc => desc;

	public Image IconLevel => iconLevel;

	public TextMeshProUGUI Level => level;

	public Image LevelUpFrame01 => levelUpFrame01;

	public Image LevelUpFrame02 => levelUpFrame02;

	public Image LevelUpFrame03 => levelUpFrame03;

	public Image LevelUpFrame04 => levelUpFrame04;

	public GameObject ParticleStar => particleStar;

	public TextMeshProUGUI Title => title;

	public TextMeshProUGUI TMPDecoText => tMPDecoText;
}
