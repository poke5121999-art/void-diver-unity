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

	public CanvasGroup Contents => contents;

	public GameObject Restore => restore;

	public Button Restore_Button => restore_Button;

	public GameObject[] SaveSlots => saveSlots;
}
