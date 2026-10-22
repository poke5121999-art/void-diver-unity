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

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject FilterZone => filterZone;

	public GameObject[] Toggles => toggles;

	public GameObject Scroller => scroller;

	public GameObject Craftable => craftable;

	public Toggle Craftable_Toggle => craftable_Toggle;

	public GameObject Deco => deco;

	public GameObject Info => info;

	public CraftingMaterialPanelPresenter CraftingMaterialPanel => craftingMaterialPanel;

	public GameObject KeyGuide => keyGuide;

	public GameObject Tooltips => tooltips;

	public GameObject Available => available;

	public BagTooltipPresenter BagTooltip => bagTooltip;

	public EquipmentTooltipPresenter EquipmentTooltip => equipmentTooltip;

	public ItemTooltipPresenter ItemTooltip => itemTooltip;

	public GameObject Top => top;

	public GameObject CurrencyLayout => currencyLayout;

	public CurrencyPanelPresenter CurrencyPanel => currencyPanel;

	public RealCoinPanelPresenter RealCoinPanel => realCoinPanel;
}
