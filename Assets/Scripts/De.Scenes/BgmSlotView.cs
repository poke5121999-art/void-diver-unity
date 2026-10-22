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

	public Image Normal => normal;

	public GameObject Focused => focused;

	public Image Line => line;

	public GameObject Locked => locked;

	public GameObject Pressed => pressed;

	public GameObject Selected => selected;

	public TextMeshProUGUI Text => text;
}
