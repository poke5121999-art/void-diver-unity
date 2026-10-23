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

	public Button Background => background;

	public GameObject Contents => contents;

	public TextMeshProUGUI Empty => empty;

	public GameObject Normal => normal;

	public GameObject[] Slots => slots;
}
