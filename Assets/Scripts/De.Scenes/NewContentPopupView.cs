using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewContentPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject normal;

	[SerializeField]
	private GameObject bottom;

	[SerializeField]
	private TextMeshProUGUI text;

	public Button Background => null;

	public GameObject Normal => null;

	public GameObject Bottom => null;

	public TextMeshProUGUI Text => null;
}
