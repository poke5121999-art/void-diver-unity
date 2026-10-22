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

	public GameObject Contents => contents;

	public GameObject Body => body;

	public TalentInfoPanelPresenter InfoPanel => infoPanel;

	public ScrollRect TreeScroll => treeScroll;

	public Image Viewport => viewport;

	public GameObject LineRoot => lineRoot;

	public GameObject[] SlotPoints => slotPoints;

	public GameObject KeyGuide => keyGuide;

	public GameObject Top => top;

	public GameObject CurrencyLayout => currencyLayout;

	public GameObject Title => title;

	public TextMeshProUGUI Title_Text => title_Text;
}
