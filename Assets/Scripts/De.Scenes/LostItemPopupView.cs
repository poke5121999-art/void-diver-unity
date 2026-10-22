using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LostItemPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	[SerializeField]
	private LostItemScrollerPresenter scroller;

	[SerializeField]
	private GameObject btns;

	[SerializeField]
	private Button btns_BtnList;

	[SerializeField]
	private Button btns_BtnRefresh;

	[SerializeField]
	private Image btns_BtnRefresh_Normal;

	[SerializeField]
	private GameObject btns_BtnRefresh_Gold;

	[SerializeField]
	private TextMeshProUGUI btns_BtnRefresh_Gold_Value;

	[SerializeField]
	private Image btns_BtnRefresh_Line;

	[SerializeField]
	private TextMeshProUGUI btns_BtnRefresh_Text;

	[SerializeField]
	private GameObject decoAni;

	[SerializeField]
	private TextMeshProUGUI textEmpty;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject top_CurrencyLayout;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Info => null;

	public GoodsTooltipPresenter GoodsTooltip => null;

	public LostItemScrollerPresenter Scroller => null;

	public GameObject Btns => null;

	public Button Btns_BtnList => null;

	public Button Btns_BtnRefresh => null;

	public Image Btns_BtnRefresh_Normal => null;

	public GameObject Btns_BtnRefresh_Gold => null;

	public TextMeshProUGUI Btns_BtnRefresh_Gold_Value => null;

	public Image Btns_BtnRefresh_Line => null;

	public TextMeshProUGUI Btns_BtnRefresh_Text => null;

	public GameObject DecoAni => null;

	public TextMeshProUGUI TextEmpty => null;

	public GameObject Top => null;

	public GameObject Top_CurrencyLayout => null;
}
