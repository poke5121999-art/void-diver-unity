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

	public Image Frame => null;

	public GameObject Info => null;

	public GameObject[] StatusCards => null;

	public Mask Mask => null;

	public Image Background => null;

	public GameObject Viewport => null;

	public InGameMiniMapPresenter MinMap => null;
}
