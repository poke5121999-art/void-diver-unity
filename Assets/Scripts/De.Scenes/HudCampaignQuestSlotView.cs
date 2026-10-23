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

	public TextMeshProUGUI Name => name;

	public TextMeshProUGUI Easy => easy;

	public TextMeshProUGUI Hard => hard;

	public TextMeshProUGUI Insane => insane;

	public TextMeshProUGUI Normal => normal;

	public GameObject[] Tasks => tasks;
}
