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

	public GameObject Body => null;

	public TextMeshProUGUI Caption => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public TextMeshProUGUI CraftableCount => null;

	public GameObject[] MaterialSlots => null;

	public GameObject NeedCoin => null;

	public GameObject Coin => null;

	public TextMeshProUGUI CoinText => null;

	public GameObject NeedCurrency => null;

	public GameObject Gold => null;

	public TextMeshProUGUI GoldText => null;

	public GameObject NeedRealCoin => null;

	public GameObject RealCoin => null;

	public TextMeshProUGUI RealCoinText => null;

	public GameObject Guide => null;

	public GameObject Keyboard => null;
}
