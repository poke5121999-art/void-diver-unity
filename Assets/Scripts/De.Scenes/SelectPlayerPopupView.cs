using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerPopupView : View
{
	[SerializeField]
	private CanvasGroup contents;

	[SerializeField]
	private GameObject restore;

	[SerializeField]
	private Button restore_Button;

	[SerializeField]
	private GameObject[] saveSlots;

	public CanvasGroup Contents => null;

	public GameObject Restore => null;

	public Button Restore_Button => null;

	public GameObject[] SaveSlots => null;
}
