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

	public GameObject Content => content;

	public CanvasGroup Body => body;

	public Button BtnEmptyArtifact => btnEmptyArtifact;

	public Image ExhibitionStand => exhibitionStand;

	public Image ImgArtifact => imgArtifact;

	public PlayerInventoryPanelPresenter InventoryPanel => inventoryPanel;

	public StashPanelPresenter StashPanel => stashPanel;
}
