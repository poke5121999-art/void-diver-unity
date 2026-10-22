using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class InGameMiniMapView : View
{
	[SerializeField]
	private Image imageRoot;

	[SerializeField]
	private GameObject indicatorCanvas;

	[SerializeField]
	private GameObject mainPlayerMarker;

	[SerializeField]
	private Image minimapOuter;

	[SerializeField]
	private GameObject monsterMarkerRoot;

	[SerializeField]
	private GameObject objectMarkerRoot;

	[SerializeField]
	private GameObject playerMarkerRoot;

	[SerializeField]
	private GameObject textMarkerRoot;

	[SerializeField]
	private MinimapOuterImage minimapOuterImage;

	[SerializeField]
	private MinimapSightMaskApplier minimapSightMaskApplier;

	public Image ImageRoot => null;

	public GameObject IndicatorCanvas => null;

	public GameObject MainPlayerMarker => null;

	public Image MinimapOuter => null;

	public GameObject MonsterMarkerRoot => null;

	public GameObject ObjectMarkerRoot => null;

	public GameObject PlayerMarkerRoot => null;

	public GameObject TextMarkerRoot => null;

	public MinimapOuterImage MinimapOuterImage => null;

	public MinimapSightMaskApplier MinimapSightMaskApplier => null;
}
