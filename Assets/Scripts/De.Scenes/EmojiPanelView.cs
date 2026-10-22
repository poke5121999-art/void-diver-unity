using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class EmojiPanelView : View
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

	public Image Background => background;

	public GameObject Menu => menu;

	public Image Frame => frame;

	public GameObject Cancel => cancel;

	public Image Cancel_Area => cancel_Area;

	public GameObject Cursor => cursor;

	public Image Cursor_Blue => cursor_Blue;

	public Image Cursor_Red => cursor_Red;

	public GameObject[] Pings => pings;
}
