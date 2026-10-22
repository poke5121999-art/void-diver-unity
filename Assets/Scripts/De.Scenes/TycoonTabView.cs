using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TycoonTabView : View
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
	private Image locked;

	[SerializeField]
	private TextMeshProUGUI text;

	public Button Button => null;

	public Image BackLine => null;

	public Image Normal => null;

	public Image Line => null;

	public Image Locked => null;

	public TextMeshProUGUI Text => null;
}
