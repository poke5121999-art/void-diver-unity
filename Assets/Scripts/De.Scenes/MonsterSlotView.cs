using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSlotView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private Button slot;

	[SerializeField]
	private Image backLine;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private Image line;

	public GameObject Content => content;

	public Button Slot => slot;

	public Image BackLine => backLine;

	public Image Normal => normal;

	public Image Icon => icon;

	public Image Line => line;
}
