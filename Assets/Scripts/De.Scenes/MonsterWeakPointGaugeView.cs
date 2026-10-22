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

	public GameObject Anchor => anchor;

	public Image BackImage => backImage;

	public Image FrontImage => frontImage;
}
