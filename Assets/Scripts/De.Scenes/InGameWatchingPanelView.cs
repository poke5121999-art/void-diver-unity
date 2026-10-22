using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class InGameWatchingPanelView : View
{
	[SerializeField]
	private GameObject anchor;

	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject keyGuideText;

	[SerializeField]
	private GameObject observing;

	public GameObject Anchor => null;

	public Button Background => null;

	public GameObject KeyGuideText => null;

	public GameObject Observing => null;
}
