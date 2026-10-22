using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParadoxPanelView : View
{
	[SerializeField]
	private GameObject noParadox;

	[SerializeField]
	private GameObject[] slots;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private TextMeshProUGUI countText;

	[SerializeField]
	private Image icon;

	public GameObject NoParadox => null;

	public GameObject[] Slots => null;

	public GameObject Top => null;

	public TextMeshProUGUI CountText => null;

	public Image Icon => null;
}
