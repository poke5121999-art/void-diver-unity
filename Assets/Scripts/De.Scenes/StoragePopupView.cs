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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Desc => desc;

	public TextMeshProUGUI StashSlotCountText => stashSlotCountText;

	public GameObject Info => info;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;

	public StorageSlotScrollerPresenter Scroller => scroller;

	public GameObject KeyGuide => keyGuide;

	public GameObject Top => top;
}
