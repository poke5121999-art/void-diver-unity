using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class TalentSlotView : View
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private Image active;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private Image locked;

	[SerializeField]
	private Image outline;

	[SerializeField]
	private GameObject unlockableTag;

	public Button Button => null;

	public Image Active => null;

	public Image Normal => null;

	public Image Icon => null;

	public Image Locked => null;

	public Image Outline => null;

	public GameObject UnlockableTag => null;
}
