using NemoLib.Mvp;
using UnityEngine;

public class ArtifactDealPanelView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject scroller;

	public GameObject Content => content;

	public GameObject Body => body;

	public GameObject Scroller => scroller;
}
