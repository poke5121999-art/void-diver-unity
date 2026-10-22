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

	public GameObject Contents => null;

	public CampaignDetailPanelPresenter CampaignDetailPanel => null;

	public CampaignListPanelPresenter CampaignListPanel => null;

	public RawImage DetailBackground => null;

	public RawImage ListBackground => null;

	public GameObject Top => null;

	public GameObject CategoryCommon => null;

	public ToggleGroup CategoryCommon_Toggle => null;

	public Toggle CategoryCommon_Toggle_Normal => null;

	public Image CategoryCommon_Toggle_Normal_NewTag => null;

	public Toggle CategoryCommon_Toggle_Story => null;

	public Image CategoryCommon_Toggle_Story_NewTag => null;

	public GameObject CategoryTutorial => null;

	public GameObject GoldGroup => null;

	public GameObject RenderDataSwapper => null;
}
