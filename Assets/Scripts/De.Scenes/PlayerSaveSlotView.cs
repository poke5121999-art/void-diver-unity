using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSaveSlotView : View
{
	[SerializeField]
	private Image normal;

	[SerializeField]
	private TextMeshProUGUI playerName;

	[SerializeField]
	private TextMeshProUGUI levelText;

	[SerializeField]
	private TextMeshProUGUI slotNumber;

	public Image Normal => null;

	public TextMeshProUGUI PlayerName => null;

	public TextMeshProUGUI LevelText => null;

	public TextMeshProUGUI SlotNumber => null;
}
