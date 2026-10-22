using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMaterialPanelView : View
{
	[SerializeField]
	private GameObject body;

	[SerializeField]
	private TextMeshProUGUI caption;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	[SerializeField]
	private TextMeshProUGUI craftableCount;

	[SerializeField]
	private GameObject[] materialSlots;

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

	[SerializeField]
	private GameObject guide;

	[SerializeField]
	private GameObject keyboard;

	public GameObject Body => body;

	public TextMeshProUGUI Caption => caption;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public TextMeshProUGUI CraftableCount => craftableCount;

	public GameObject[] MaterialSlots => materialSlots;

	public GameObject NeedCoin => needCoin;

	public GameObject Coin => coin;

	public TextMeshProUGUI CoinText => coinText;

	public GameObject NeedCurrency => needCurrency;

	public GameObject Gold => gold;

	public TextMeshProUGUI GoldText => goldText;

	public GameObject NeedRealCoin => needRealCoin;

	public GameObject RealCoin => realCoin;

	public TextMeshProUGUI RealCoinText => realCoinText;

	public GameObject Guide => guide;

	public GameObject Keyboard => keyboard;
}
