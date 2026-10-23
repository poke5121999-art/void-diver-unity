using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class InGameMiniMapPanelView : View
{
	[SerializeField]
	private Image frame;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GameObject[] statusCards;

	[SerializeField]
	private Mask mask;

	[SerializeField]
	private Image background;

	[SerializeField]
	private GameObject viewport;

	[SerializeField]
	private InGameMiniMapPresenter minMap;

	public Image Frame => frame;

	public GameObject Info => info;

	public GameObject[] StatusCards => statusCards;

	public Mask Mask => mask;

	public Image Background => background;

	public GameObject Viewport => viewport;

	public InGameMiniMapPresenter MinMap => minMap;
}
