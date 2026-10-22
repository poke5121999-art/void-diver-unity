using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameQuestPanelView : View
{
	[SerializeField]
	private Image bG;

	[SerializeField]
	private GameObject campaignAndLoungeRoot;

	[SerializeField]
	private GameObject hudCampaignQuestSlot;

	[SerializeField]
	private GameObject hudLoungeQuestSlot;

	[SerializeField]
	private GameObject hudMissionQuestSlot;

	[SerializeField]
	private Image mission;

	[SerializeField]
	private TextMeshProUGUI mission_Caption;

	[SerializeField]
	private GameObject mission_Root;

	public Image BG => bG;

	public GameObject CampaignAndLoungeRoot => campaignAndLoungeRoot;

	public GameObject HudCampaignQuestSlot => hudCampaignQuestSlot;

	public GameObject HudLoungeQuestSlot => hudLoungeQuestSlot;

	public GameObject HudMissionQuestSlot => hudMissionQuestSlot;

	public Image Mission => mission;

	public TextMeshProUGUI Mission_Caption => mission_Caption;

	public GameObject Mission_Root => mission_Root;
}
