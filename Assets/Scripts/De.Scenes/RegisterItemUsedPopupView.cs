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

	public Button Background => background;

	public GameObject Content => content;

	public GameObject Body => body;

	public DraggingGoodsSlotPresenter DraggingGoodsSlot => draggingGoodsSlot;

	public GameObject Left => left;

	public PlayerInventoryPanelPresenter MyInventory => myInventory;

	public StashPanelPresenter StashPanel => stashPanel;

	public GameObject Right => right;

	public RegisterItemUsedPanelPresenter RegisterItemUsedPanel => registerItemUsedPanel;

	public GameObject Top => top;

	public GameObject Title => title;

	public TextMeshProUGUI Title_TitleText => title_TitleText;
}
