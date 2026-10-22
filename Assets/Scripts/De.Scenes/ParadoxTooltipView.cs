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

	public Image BackLine => null;

	public TextMeshProUGUI Desc => null;

	public Image Normal => null;

	public GameObject Paradox => null;

	public Image Paradox_Icon => null;

	public TextMeshProUGUI Paradox_Level => null;

	public Image Paradox_LockIcon => null;

	public TextMeshProUGUI Paradox_Name => null;
}
