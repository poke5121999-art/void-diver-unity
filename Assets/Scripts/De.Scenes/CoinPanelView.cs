using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class CoinPanelView : View
{
	[SerializeField]
	private GameObject coin;

	[SerializeField]
	private TextMeshProUGUI coin_Text;

	public GameObject Coin => null;

	public TextMeshProUGUI Coin_Text => null;
}
