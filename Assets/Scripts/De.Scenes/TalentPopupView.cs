using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalentPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private TalentInfoPanelPresenter infoPanel;

	[SerializeField]
	private ScrollRect treeScroll;

	[SerializeField]
	private Image viewport;

	[SerializeField]
	private GameObject lineRoot;

	[SerializeField]
	private GameObject[] slotPoints;

	[SerializeField]
	private GameObject keyGuide;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject currencyLayout;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private TextMeshProUGUI title_Text;

	public GameObject Contents => null;

	public GameObject Body => null;

	public TalentInfoPanelPresenter InfoPanel => null;

	public ScrollRect TreeScroll => null;

	public Image Viewport => null;

	public GameObject LineRoot => null;

	public GameObject[] SlotPoints => null;

	public GameObject KeyGuide => null;

	public GameObject Top => null;

	public GameObject CurrencyLayout => null;

	public GameObject Title => null;

	public TextMeshProUGUI Title_Text => null;
}
