using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotView : View
{
	[SerializeField]
	private Image backLine;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private Image iconLock;

	[SerializeField]
	private GameObject iconSkill;

	[SerializeField]
	private GameObject keyGuide;

	public Image BackLine => null;

	public Image Normal => null;

	public Image Icon => null;

	public Image IconLock => null;

	public GameObject IconSkill => null;

	public GameObject KeyGuide => null;
}
