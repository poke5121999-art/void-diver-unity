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

	public GameObject Contents => null;

	public GameObject Body => null;

	public TextMeshProUGUI CampaignName => null;

	public TextMeshProUGUI CategoryText => null;

	public TextMeshProUGUI DifficultyText => null;
}
