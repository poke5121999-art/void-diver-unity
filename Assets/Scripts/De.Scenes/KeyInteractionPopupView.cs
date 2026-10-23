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

	public Button Background => background;

	public GameObject Content => content;

	public GameObject Body => body;

	public DraggingGoodsSlotPresenter DraggingGoodsSlot => draggingGoodsSlot;

	public GameObject Left => left;

	public PlayerInventoryPanelPresenter MyInventory => myInventory;

	public GameObject Right => right;

	public KeyInteractionPanelPresenter KeyInteractionPanel => keyInteractionPanel;
}
