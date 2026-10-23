using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlotView : View
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private GameObject delete;

	[SerializeField]
	private Button delete_Button;

	[SerializeField]
	private GameObject focused;

	[SerializeField]
	private TextMeshProUGUI levelText;

	[SerializeField]
	private TextMeshProUGUI palyerName;

	[SerializeField]
	private TextMeshProUGUI playTimeText;

	[SerializeField]
	private GameObject pressed;

	[SerializeField]
	private TextMeshProUGUI saveTimeText;

	[SerializeField]
	private GameObject selected;

	[SerializeField]
	private TextMeshProUGUI slotNumberText;

	[SerializeField]
	private TextMeshProUGUI version;

	public Button Button => button;

	public Image Normal => normal;

	public GameObject Delete => delete;

	public Button Delete_Button => delete_Button;

	public GameObject Focused => focused;

	public TextMeshProUGUI LevelText => levelText;

	public TextMeshProUGUI PalyerName => palyerName;

	public TextMeshProUGUI PlayTimeText => playTimeText;

	public GameObject Pressed => pressed;

	public TextMeshProUGUI SaveTimeText => saveTimeText;

	public GameObject Selected => selected;

	public TextMeshProUGUI SlotNumberText => slotNumberText;

	public TextMeshProUGUI Version => version;
}
