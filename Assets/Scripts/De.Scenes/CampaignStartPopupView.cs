using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class CampaignStartPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private TextMeshProUGUI campaignName;

	[SerializeField]
	private TextMeshProUGUI categoryText;

	[SerializeField]
	private TextMeshProUGUI difficultyText;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public TextMeshProUGUI CampaignName => campaignName;

	public TextMeshProUGUI CategoryText => categoryText;

	public TextMeshProUGUI DifficultyText => difficultyText;
}
