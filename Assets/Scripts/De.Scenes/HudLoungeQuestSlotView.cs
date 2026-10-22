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

	public TextMeshProUGUI Name => null;

	public GameObject MainIcon => null;

	public GameObject SubIcon => null;

	public GameObject[] Tasks => null;
}
