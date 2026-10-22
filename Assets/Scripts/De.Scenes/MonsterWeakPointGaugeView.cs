using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class MonsterWeakPointGaugeView : View
{
	[SerializeField]
	private GameObject anchor;

	[SerializeField]
	private Image backImage;

	[SerializeField]
	private Image frontImage;

	public GameObject Anchor => null;

	public Image BackImage => null;

	public Image FrontImage => null;
}
