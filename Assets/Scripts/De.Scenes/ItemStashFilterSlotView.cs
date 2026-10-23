using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStashFilterSlotView : View
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private Image backLine;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private Image line;

	[SerializeField]
	private TextMeshProUGUI text;

	public Button Button => button;

	public Image BackLine => backLine;

	public Image Normal => normal;

	public Image Line => line;

	public TextMeshProUGUI Text => text;
}
