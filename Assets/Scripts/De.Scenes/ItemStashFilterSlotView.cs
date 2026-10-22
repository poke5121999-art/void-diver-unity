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

	public Button Button => null;

	public Image BackLine => null;

	public Image Normal => null;

	public Image Line => null;

	public TextMeshProUGUI Text => null;
}
