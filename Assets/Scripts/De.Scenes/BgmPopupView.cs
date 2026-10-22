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

	public GameObject Contents => null;

	public ScrollRect BaseScrollView => null;

	public BgmSlotScrollerPresenter Scroller => null;

	public GameObject KeyGuide => null;

	public GameObject Model => null;

	public GameObject TextBubble => null;

	public Image BG => null;

	public TextMeshProUGUI BgmName => null;

	public GameObject Top => null;

	public GameObject CurrencyLayout => null;
}
