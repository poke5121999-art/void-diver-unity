using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BgmSlotView : View
{
	[SerializeField]
	private Image normal;

	[SerializeField]
	private GameObject focused;

	[SerializeField]
	private Image line;

	[SerializeField]
	private GameObject locked;

	[SerializeField]
	private GameObject pressed;

	[SerializeField]
	private GameObject selected;

	[SerializeField]
	private TextMeshProUGUI text;

	public Image Normal => null;

	public GameObject Focused => null;

	public Image Line => null;

	public GameObject Locked => null;

	public GameObject Pressed => null;

	public GameObject Selected => null;

	public TextMeshProUGUI Text => null;
}
