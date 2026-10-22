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

	public Button Button => button;

	public Image Active => active;

	public Image Normal => normal;

	public Image Icon => icon;

	public Image Locked => locked;

	public Image Outline => outline;

	public GameObject UnlockableTag => unlockableTag;
}
