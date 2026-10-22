using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject filterZone;

	[SerializeField]
	private GameObject[] toggles;

	[SerializeField]
	private GameObject scroller;

	[SerializeField]
	private GameObject craftable;

	[SerializeField]
	private Toggle craftable_Toggle;

	[SerializeField]
	private GameObject deco;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private CraftingMaterialPanelPresenter craftingMaterialPanel;

	[SerializeField]
	private GameObject keyGuide;

	[SerializeField]
	private GameObject tooltips;

	[SerializeField]
	private GameObject available;

	[SerializeField]
	private BagTooltipPresenter bagTooltip;

	[SerializeField]
	private EquipmentTooltipPresenter equipmentTooltip;

	[SerializeField]
	private ItemTooltipPresenter itemTooltip;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject currencyLayout;

	[SerializeField]
	private CurrencyPanelPresenter currencyPanel;

	[SerializeField]
	private RealCoinPanelPresenter realCoinPanel;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject FilterZone => null;

	public GameObject[] Toggles => null;

	public GameObject Scroller => null;

	public GameObject Craftable => null;

	public Toggle Craftable_Toggle => null;

	public GameObject Deco => null;

	public GameObject Info => null;

	public CraftingMaterialPanelPresenter CraftingMaterialPanel => null;

	public GameObject KeyGuide => null;

	public GameObject Tooltips => null;

	public GameObject Available => null;

	public BagTooltipPresenter BagTooltip => null;

	public EquipmentTooltipPresenter EquipmentTooltip => null;

	public ItemTooltipPresenter ItemTooltip => null;

	public GameObject Top => null;

	public GameObject CurrencyLayout => null;

	public CurrencyPanelPresenter CurrencyPanel => null;

	public RealCoinPanelPresenter RealCoinPanel => null;
}
