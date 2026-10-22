using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BgmPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private ScrollRect baseScrollView;

	[SerializeField]
	private BgmSlotScrollerPresenter scroller;

	[SerializeField]
	private GameObject keyGuide;

	[SerializeField]
	private GameObject model;

	[SerializeField]
	private GameObject textBubble;

	[SerializeField]
	private Image bG;

	[SerializeField]
	private TextMeshProUGUI bgmName;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject currencyLayout;

	public GameObject Contents => contents;

	public ScrollRect BaseScrollView => baseScrollView;

	public BgmSlotScrollerPresenter Scroller => scroller;

	public GameObject KeyGuide => keyGuide;

	public GameObject Model => model;

	public GameObject TextBubble => textBubble;

	public Image BG => bG;

	public TextMeshProUGUI BgmName => bgmName;

	public GameObject Top => top;

	public GameObject CurrencyLayout => currencyLayout;
}
