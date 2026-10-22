using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class PingPanelView : View
{
	[SerializeField]
	private Image background;

	[SerializeField]
	private GameObject menu;

	[SerializeField]
	private Image frame;

	[SerializeField]
	private GameObject cancel;

	[SerializeField]
	private Image cancel_Area;

	[SerializeField]
	private GameObject cursor;

	[SerializeField]
	private Image cursor_Blue;

	[SerializeField]
	private Image cursor_Red;

	[SerializeField]
	private GameObject[] pings;

	public Image Background => null;

	public GameObject Menu => null;

	public Image Frame => null;

	public GameObject Cancel => null;

	public Image Cancel_Area => null;

	public GameObject Cursor => null;

	public Image Cursor_Blue => null;

	public Image Cursor_Red => null;

	public GameObject[] Pings => null;
}
