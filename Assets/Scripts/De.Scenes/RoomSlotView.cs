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

	public Image Normal => normal;

	public GameObject[] Ping => ping;

	public TextMeshProUGUI TextCapacity => textCapacity;

	public TextMeshProUGUI TextDifficulty => textDifficulty;

	public TextMeshProUGUI TextHostLevel => textHostLevel;

	public TextMeshProUGUI TextName => textName;

	public TextMeshProUGUI TextSteamId => textSteamId;

	public TextMeshProUGUI TextTitle => textTitle;
}
