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

	public TextMeshProUGUI Desc => null;

	public Image Normal => null;

	public GameObject Cost => null;

	public GameObject Cost_Info => null;

	public Image Cost_Icon => null;

	public TextMeshProUGUI Cost_Value => null;

	public Image LockIcon => null;

	public GameObject Paradox => null;

	public Image Paradox_Icon => null;

	public TextMeshProUGUI Paradox_Level => null;

	public TextMeshProUGUI Paradox_Name => null;
}
