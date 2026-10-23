using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParadoxSlotView : View
{
	[SerializeField]
	private TextMeshProUGUI desc;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private GameObject cost;

	[SerializeField]
	private GameObject cost_Info;

	[SerializeField]
	private Image cost_Icon;

	[SerializeField]
	private TextMeshProUGUI cost_Value;

	[SerializeField]
	private Image lockIcon;

	[SerializeField]
	private GameObject paradox;

	[SerializeField]
	private Image paradox_Icon;

	[SerializeField]
	private TextMeshProUGUI paradox_Level;

	[SerializeField]
	private TextMeshProUGUI paradox_Name;

	public TextMeshProUGUI Desc => desc;

	public Image Normal => normal;

	public GameObject Cost => cost;

	public GameObject Cost_Info => cost_Info;

	public Image Cost_Icon => cost_Icon;

	public TextMeshProUGUI Cost_Value => cost_Value;

	public Image LockIcon => lockIcon;

	public GameObject Paradox => paradox;

	public Image Paradox_Icon => paradox_Icon;

	public TextMeshProUGUI Paradox_Level => paradox_Level;

	public TextMeshProUGUI Paradox_Name => paradox_Name;
}
