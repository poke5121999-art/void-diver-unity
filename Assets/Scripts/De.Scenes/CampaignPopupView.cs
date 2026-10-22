using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class CampaignPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private CampaignDetailPanelPresenter campaignDetailPanel;

	[SerializeField]
	private CampaignListPanelPresenter campaignListPanel;

	[SerializeField]
	private RawImage detailBackground;

	[SerializeField]
	private RawImage listBackground;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject categoryCommon;

	[SerializeField]
	private ToggleGroup categoryCommon_Toggle;

	[SerializeField]
	private Toggle categoryCommon_Toggle_Normal;

	[SerializeField]
	private Image categoryCommon_Toggle_Normal_NewTag;

	[SerializeField]
	private Toggle categoryCommon_Toggle_Story;

	[SerializeField]
	private Image categoryCommon_Toggle_Story_NewTag;

	[SerializeField]
	private GameObject categoryTutorial;

	[SerializeField]
	private GameObject goldGroup;

	[SerializeField]
	private GameObject renderDataSwapper;

	public GameObject Contents => contents;

	public CampaignDetailPanelPresenter CampaignDetailPanel => campaignDetailPanel;

	public CampaignListPanelPresenter CampaignListPanel => campaignListPanel;

	public RawImage DetailBackground => detailBackground;

	public RawImage ListBackground => listBackground;

	public GameObject Top => top;

	public GameObject CategoryCommon => categoryCommon;

	public ToggleGroup CategoryCommon_Toggle => categoryCommon_Toggle;

	public Toggle CategoryCommon_Toggle_Normal => categoryCommon_Toggle_Normal;

	public Image CategoryCommon_Toggle_Normal_NewTag => categoryCommon_Toggle_Normal_NewTag;

	public Toggle CategoryCommon_Toggle_Story => categoryCommon_Toggle_Story;

	public Image CategoryCommon_Toggle_Story_NewTag => categoryCommon_Toggle_Story_NewTag;

	public GameObject CategoryTutorial => categoryTutorial;

	public GameObject GoldGroup => goldGroup;

	public GameObject RenderDataSwapper => renderDataSwapper;
}
