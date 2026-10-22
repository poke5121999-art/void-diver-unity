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

	public Button Background => null;

	public GameObject Content => null;

	public CanvasGroup Body => null;

	public PlayerInventoryPanelPresenter InventoryPanel => null;

	public StashPanelPresenter StashPanel => null;

	public GameObject CoinAttraction => null;

	public GameObject Top => null;
}
