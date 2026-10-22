using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class NoteSystemPopupView : View
{
	[SerializeField]
	private Button baseBgDimmer;

	[SerializeField]
	private GameObject[] notes;

	public Button BaseBgDimmer => baseBgDimmer;

	public GameObject[] Notes => notes;
}
