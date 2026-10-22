using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParadoxPurifyPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private TextMeshProUGUI empty;

	[SerializeField]
	private GameObject normal;

	[SerializeField]
	private GameObject[] slots;

	public Button Background => null;

	public GameObject Contents => null;

	public TextMeshProUGUI Empty => null;

	public GameObject Normal => null;

	public GameObject[] Slots => null;
}
