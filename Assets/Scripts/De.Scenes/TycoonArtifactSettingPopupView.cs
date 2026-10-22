using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class TycoonArtifactSettingPopupView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private CanvasGroup body;

	[SerializeField]
	private Button btnEmptyArtifact;

	[SerializeField]
	private Image exhibitionStand;

	[SerializeField]
	private Image imgArtifact;

	[SerializeField]
	private PlayerInventoryPanelPresenter inventoryPanel;

	[SerializeField]
	private StashPanelPresenter stashPanel;

	public GameObject Content => null;

	public CanvasGroup Body => null;

	public Button BtnEmptyArtifact => null;

	public Image ExhibitionStand => null;

	public Image ImgArtifact => null;

	public PlayerInventoryPanelPresenter InventoryPanel => null;

	public StashPanelPresenter StashPanel => null;
}
