using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterItemUsedPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private DraggingGoodsSlotPresenter draggingGoodsSlot;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private PlayerInventoryPanelPresenter myInventory;

	[SerializeField]
	private StashPanelPresenter stashPanel;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private RegisterItemUsedPanelPresenter registerItemUsedPanel;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private TextMeshProUGUI title_TitleText;

	public Button Background => null;

	public GameObject Content => null;

	public GameObject Body => null;

	public DraggingGoodsSlotPresenter DraggingGoodsSlot => null;

	public GameObject Left => null;

	public PlayerInventoryPanelPresenter MyInventory => null;

	public StashPanelPresenter StashPanel => null;

	public GameObject Right => null;

	public RegisterItemUsedPanelPresenter RegisterItemUsedPanel => null;

	public GameObject Top => null;

	public GameObject Title => null;

	public TextMeshProUGUI Title_TitleText => null;
}
