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

	public GameObject NoParadox => noParadox;

	public GameObject[] Slots => slots;

	public GameObject Top => top;

	public TextMeshProUGUI CountText => countText;

	public Image Icon => icon;
}
