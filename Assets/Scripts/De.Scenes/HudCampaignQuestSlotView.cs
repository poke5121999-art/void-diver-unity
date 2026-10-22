using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class HudCampaignQuestSlotView : View
{
	[SerializeField]
	private new TextMeshProUGUI name;

	[SerializeField]
	private TextMeshProUGUI easy;

	[SerializeField]
	private TextMeshProUGUI hard;

	[SerializeField]
	private TextMeshProUGUI insane;

	[SerializeField]
	private TextMeshProUGUI normal;

	[SerializeField]
	private GameObject[] tasks;

	public TextMeshProUGUI Name => null;

	public TextMeshProUGUI Easy => null;

	public TextMeshProUGUI Hard => null;

	public TextMeshProUGUI Insane => null;

	public TextMeshProUGUI Normal => null;

	public GameObject[] Tasks => null;
}
