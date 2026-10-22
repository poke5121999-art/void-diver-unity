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

	public Button Button => null;

	public Image Normal => null;

	public GameObject Delete => null;

	public Button Delete_Button => null;

	public GameObject Focused => null;

	public TextMeshProUGUI LevelText => null;

	public TextMeshProUGUI PalyerName => null;

	public TextMeshProUGUI PlayTimeText => null;

	public GameObject Pressed => null;

	public TextMeshProUGUI SaveTimeText => null;

	public GameObject Selected => null;

	public TextMeshProUGUI SlotNumberText => null;

	public TextMeshProUGUI Version => null;
}
