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

	public Image BG => null;

	public GameObject CampaignAndLoungeRoot => null;

	public GameObject HudCampaignQuestSlot => null;

	public GameObject HudLoungeQuestSlot => null;

	public GameObject HudMissionQuestSlot => null;

	public Image Mission => null;

	public TextMeshProUGUI Mission_Caption => null;

	public GameObject Mission_Root => null;
}
