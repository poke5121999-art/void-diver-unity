using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class KeyInteractionPopupView : View
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
	private GameObject right;

	[SerializeField]
	private KeyInteractionPanelPresenter keyInteractionPanel;

	public Button Background => null;

	public GameObject Content => null;

	public GameObject Body => null;

	public DraggingGoodsSlotPresenter DraggingGoodsSlot => null;

	public GameObject Left => null;

	public PlayerInventoryPanelPresenter MyInventory => null;

	public GameObject Right => null;

	public KeyInteractionPanelPresenter KeyInteractionPanel => null;
}
