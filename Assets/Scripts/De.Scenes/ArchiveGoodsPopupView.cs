using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class ArchiveGoodsPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private GameObject scroller;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private GameObject empty;

	[SerializeField]
	private EquipmentInfoPanelPresenter equipmentInfoPanel;

	[SerializeField]
	private ItemInfoPanelPresenter itemInfoPanel;

	[SerializeField]
	private GameObject unknown;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private TextMeshProUGUI title;

	[SerializeField]
	private TextMeshProUGUI titleSub;

	[SerializeField]
	private GameObject keyGuide;

	[SerializeField]
	private GameObject keyGuide_Note;

	public GameObject Contents => null;

	public GameObject Left => null;

	public GameObject Scroller => null;

	public GameObject Right => null;

	public GameObject Empty => null;

	public EquipmentInfoPanelPresenter EquipmentInfoPanel => null;

	public ItemInfoPanelPresenter ItemInfoPanel => null;

	public GameObject Unknown => null;

	public GameObject Top => null;

	public TextMeshProUGUI Title => null;

	public TextMeshProUGUI TitleSub => null;

	public GameObject KeyGuide => null;

	public GameObject KeyGuide_Note => null;
}
