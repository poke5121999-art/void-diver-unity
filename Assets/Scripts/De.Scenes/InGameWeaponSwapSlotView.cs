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

	public GameObject Contents => contents;

	public GameObject KeyPrompt => keyPrompt;

	public Button Slot => slot;

	public Image Normal => normal;

	public Image Icon => icon;

	public Image LevelBg => levelBg;

	public GameObject SwapedFX => swapedFX;
}
