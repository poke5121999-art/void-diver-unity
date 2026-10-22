using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoragePopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject desc;

	[SerializeField]
	private TextMeshProUGUI stashSlotCountText;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	[SerializeField]
	private StorageSlotScrollerPresenter scroller;

	[SerializeField]
	private GameObject keyGuide;

	[SerializeField]
	private GameObject top;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Desc => null;

	public TextMeshProUGUI StashSlotCountText => null;

	public GameObject Info => null;

	public GoodsTooltipPresenter GoodsTooltip => null;

	public StorageSlotScrollerPresenter Scroller => null;

	public GameObject KeyGuide => null;

	public GameObject Top => null;
}
