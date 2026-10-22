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

	public GameObject RealCoin => null;

	public GameObject RealCoin_Empty => null;

	public TextMeshProUGUI RealCoin_Value => null;
}
