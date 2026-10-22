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

	public GameObject Content => null;

	public Button Slot => null;

	public Image BackLine => null;

	public Image Normal => null;

	public Image Icon => null;

	public Image Line => null;
}
