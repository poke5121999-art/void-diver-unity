using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageResultParadoxSlotView : View
{
	[SerializeField]
	private Image clear;

	[SerializeField]
	private Image line;

	[SerializeField]
	private TextMeshProUGUI paradoxNameNegetive;

	[SerializeField]
	private TextMeshProUGUI paradoxNamePositive;

	public Image Clear => clear;

	public Image Line => line;

	public TextMeshProUGUI ParadoxNameNegetive => paradoxNameNegetive;

	public TextMeshProUGUI ParadoxNamePositive => paradoxNamePositive;
}
