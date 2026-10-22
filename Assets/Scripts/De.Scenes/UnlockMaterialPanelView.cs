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

	public GameObject Body => null;

	public GameObject Condition => null;

	public GameObject[] Condition_Texts => null;

	public GameObject Material => null;

	public TextMeshProUGUI Material_Caption => null;

	public GameObject[] Material_Slots => null;

	public GameObject NeedCoin => null;

	public GameObject Coin => null;

	public TextMeshProUGUI CoinText => null;

	public GameObject NeedCurrency => null;

	public GameObject Gold => null;

	public TextMeshProUGUI GoldText => null;

	public GameObject NeedRealCoin => null;

	public GameObject RealCoin => null;

	public TextMeshProUGUI RealCoinText => null;
}
