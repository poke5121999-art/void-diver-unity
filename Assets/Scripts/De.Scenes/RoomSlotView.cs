using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSlotView : View
{
	[SerializeField]
	private Image normal;

	[SerializeField]
	private GameObject[] ping;

	[SerializeField]
	private TextMeshProUGUI textCapacity;

	[SerializeField]
	private TextMeshProUGUI textDifficulty;

	[SerializeField]
	private TextMeshProUGUI textHostLevel;

	[SerializeField]
	private TextMeshProUGUI textName;

	[SerializeField]
	private TextMeshProUGUI textSteamId;

	[SerializeField]
	private TextMeshProUGUI textTitle;

	public Image Normal => null;

	public GameObject[] Ping => null;

	public TextMeshProUGUI TextCapacity => null;

	public TextMeshProUGUI TextDifficulty => null;

	public TextMeshProUGUI TextHostLevel => null;

	public TextMeshProUGUI TextName => null;

	public TextMeshProUGUI TextSteamId => null;

	public TextMeshProUGUI TextTitle => null;
}
