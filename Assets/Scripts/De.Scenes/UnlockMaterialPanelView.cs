using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class UnlockMaterialPanelView : View
{
	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject condition;

	[SerializeField]
	private GameObject[] condition_Texts;

	[SerializeField]
	private GameObject material;

	[SerializeField]
	private TextMeshProUGUI material_Caption;

	[SerializeField]
	private GameObject[] material_Slots;

	[SerializeField]
	private GameObject needCoin;

	[SerializeField]
	private GameObject coin;

	[SerializeField]
	private TextMeshProUGUI coinText;

	[SerializeField]
	private GameObject needCurrency;

	[SerializeField]
	private GameObject gold;

	[SerializeField]
	private TextMeshProUGUI goldText;

	[SerializeField]
	private GameObject needRealCoin;

	[SerializeField]
	private GameObject realCoin;

	[SerializeField]
	private TextMeshProUGUI realCoinText;

	public GameObject Body => body;

	public GameObject Condition => condition;

	public GameObject[] Condition_Texts => condition_Texts;

	public GameObject Material => material;

	public TextMeshProUGUI Material_Caption => material_Caption;

	public GameObject[] Material_Slots => material_Slots;

	public GameObject NeedCoin => needCoin;

	public GameObject Coin => coin;

	public TextMeshProUGUI CoinText => coinText;

	public GameObject NeedCurrency => needCurrency;

	public GameObject Gold => gold;

	public TextMeshProUGUI GoldText => goldText;

	public GameObject NeedRealCoin => needRealCoin;

	public GameObject RealCoin => realCoin;

	public TextMeshProUGUI RealCoinText => realCoinText;
}
