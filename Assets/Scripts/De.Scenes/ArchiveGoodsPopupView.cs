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

	public GameObject Contents => contents;

	public GameObject Left => left;

	public GameObject Scroller => scroller;

	public GameObject Right => right;

	public GameObject Empty => empty;

	public EquipmentInfoPanelPresenter EquipmentInfoPanel => equipmentInfoPanel;

	public ItemInfoPanelPresenter ItemInfoPanel => itemInfoPanel;

	public GameObject Unknown => unknown;

	public GameObject Top => top;

	public TextMeshProUGUI Title => title;

	public TextMeshProUGUI TitleSub => titleSub;

	public GameObject KeyGuide => keyGuide;

	public GameObject KeyGuide_Note => keyGuide_Note;
}
