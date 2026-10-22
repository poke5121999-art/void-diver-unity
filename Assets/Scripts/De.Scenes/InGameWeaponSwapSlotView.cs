using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class InGameWeaponSwapSlotView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject keyPrompt;

	[SerializeField]
	private Button slot;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private Image levelBg;

	[SerializeField]
	private GameObject swapedFX;

	public GameObject Contents => null;

	public GameObject KeyPrompt => null;

	public Button Slot => null;

	public Image Normal => null;

	public Image Icon => null;

	public Image LevelBg => null;

	public GameObject SwapedFX => null;
}
