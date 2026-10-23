using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParadoxTooltipView : View
{
	[SerializeField]
	private Image backLine;

	[SerializeField]
	private TextMeshProUGUI desc;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private GameObject paradox;

	[SerializeField]
	private Image paradox_Icon;

	[SerializeField]
	private TextMeshProUGUI paradox_Level;

	[SerializeField]
	private Image paradox_LockIcon;

	[SerializeField]
	private TextMeshProUGUI paradox_Name;

	public Image BackLine => backLine;

	public TextMeshProUGUI Desc => desc;

	public Image Normal => normal;

	public GameObject Paradox => paradox;

	public Image Paradox_Icon => paradox_Icon;

	public TextMeshProUGUI Paradox_Level => paradox_Level;

	public Image Paradox_LockIcon => paradox_LockIcon;

	public TextMeshProUGUI Paradox_Name => paradox_Name;
}
