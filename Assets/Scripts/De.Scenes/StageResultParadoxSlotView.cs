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

	public Image Clear => null;

	public Image Line => null;

	public TextMeshProUGUI ParadoxNameNegetive => null;

	public TextMeshProUGUI ParadoxNamePositive => null;
}
