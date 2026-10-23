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

	public Image ImageRoot => imageRoot;

	public GameObject IndicatorCanvas => indicatorCanvas;

	public GameObject MainPlayerMarker => mainPlayerMarker;

	public Image MinimapOuter => minimapOuter;

	public GameObject MonsterMarkerRoot => monsterMarkerRoot;

	public GameObject ObjectMarkerRoot => objectMarkerRoot;

	public GameObject PlayerMarkerRoot => playerMarkerRoot;

	public GameObject TextMarkerRoot => textMarkerRoot;

	public MinimapOuterImage MinimapOuterImage => minimapOuterImage;

	public MinimapSightMaskApplier MinimapSightMaskApplier => minimapSightMaskApplier;
}
