using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class CurrencyPanelView : View
{
	[SerializeField]
	private GameObject gold;

	[SerializeField]
	private TextMeshProUGUI gold_Text;

	public GameObject Gold => gold;

	public TextMeshProUGUI Gold_Text => gold_Text;
}
