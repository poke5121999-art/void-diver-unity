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

	public Button Button => button;

	public Image BackLine => backLine;

	public Image Normal => normal;

	public Image Line => line;

	public Image Locked => locked;

	public TextMeshProUGUI Text => text;
}
