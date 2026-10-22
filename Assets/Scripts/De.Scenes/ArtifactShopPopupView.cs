using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactShopPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private CanvasGroup body;

	[SerializeField]
	private PlayerInventoryPanelPresenter inventoryPanel;

	[SerializeField]
	private StashPanelPresenter stashPanel;

	[SerializeField]
	private GameObject coinAttraction;

	[SerializeField]
	private GameObject top;

	public Button Background => background;

	public GameObject Content => content;

	public CanvasGroup Body => body;

	public PlayerInventoryPanelPresenter InventoryPanel => inventoryPanel;

	public StashPanelPresenter StashPanel => stashPanel;

	public GameObject CoinAttraction => coinAttraction;

	public GameObject Top => top;
}
