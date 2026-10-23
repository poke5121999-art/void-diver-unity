using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class RealCoinPanelView : View
{
	[SerializeField]
	private GameObject realCoin;

	[SerializeField]
	private GameObject realCoin_Empty;

	[SerializeField]
	private TextMeshProUGUI realCoin_Value;

	public GameObject RealCoin => realCoin;

	public GameObject RealCoin_Empty => realCoin_Empty;

	public TextMeshProUGUI RealCoin_Value => realCoin_Value;
}
