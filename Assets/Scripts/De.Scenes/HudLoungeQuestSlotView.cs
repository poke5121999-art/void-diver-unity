using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class HudLoungeQuestSlotView : View
{
	[SerializeField]
	private new TextMeshProUGUI name;

	[SerializeField]
	private GameObject mainIcon;

	[SerializeField]
	private GameObject subIcon;

	[SerializeField]
	private GameObject[] tasks;

	public TextMeshProUGUI Name => name;

	public GameObject MainIcon => mainIcon;

	public GameObject SubIcon => subIcon;

	public GameObject[] Tasks => tasks;
}
