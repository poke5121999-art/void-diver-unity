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

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Root => null;

	public TextMeshProUGUI Desc => null;

	public Image IconLevel => null;

	public TextMeshProUGUI Level => null;

	public Image LevelUpFrame01 => null;

	public Image LevelUpFrame02 => null;

	public Image LevelUpFrame03 => null;

	public Image LevelUpFrame04 => null;

	public GameObject ParticleStar => null;

	public TextMeshProUGUI Title => null;

	public TextMeshProUGUI TMPDecoText => null;
}
