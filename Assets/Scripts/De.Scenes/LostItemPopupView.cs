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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Info => info;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;

	public LostItemScrollerPresenter Scroller => scroller;

	public GameObject Btns => btns;

	public Button Btns_BtnList => btns_BtnList;

	public Button Btns_BtnRefresh => btns_BtnRefresh;

	public Image Btns_BtnRefresh_Normal => btns_BtnRefresh_Normal;

	public GameObject Btns_BtnRefresh_Gold => btns_BtnRefresh_Gold;

	public TextMeshProUGUI Btns_BtnRefresh_Gold_Value => btns_BtnRefresh_Gold_Value;

	public Image Btns_BtnRefresh_Line => btns_BtnRefresh_Line;

	public TextMeshProUGUI Btns_BtnRefresh_Text => btns_BtnRefresh_Text;

	public GameObject DecoAni => decoAni;

	public TextMeshProUGUI TextEmpty => textEmpty;

	public GameObject Top => top;

	public GameObject Top_CurrencyLayout => top_CurrencyLayout;
}
