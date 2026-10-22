using NemoLib.Mvp;
using UnityEngine;

public class InGameKeyGuidePanelView : View
{
	[SerializeField]
	private GameObject controlGuide;

	[SerializeField]
	private GameObject toggle;

	public GameObject ControlGuide => controlGuide;

	public GameObject Toggle => toggle;
}
