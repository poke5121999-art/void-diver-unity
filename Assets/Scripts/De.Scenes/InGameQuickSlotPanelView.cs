using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class InGameQuickSlotPanelView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject[] slots;

	[SerializeField]
	private GameObject subWeapon;

	[SerializeField]
	private GameObject subWeapon_Normal;

	[SerializeField]
	private Image subWeapon_Dim;

	[SerializeField]
	private Image subWeapon_IconSwap;

	[SerializeField]
	private InGameWeaponSwapSlotPresenter subWeapon_WeaponSwapSlot;

	[SerializeField]
	private InGameWeaponSwapSlotPresenter weaponSwapSlot;

	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	[SerializeField]
	private Image selectionIndicator;

	public GameObject Contents => null;

	public GameObject[] Slots => null;

	public GameObject SubWeapon => null;

	public GameObject SubWeapon_Normal => null;

	public Image SubWeapon_Dim => null;

	public Image SubWeapon_IconSwap => null;

	public InGameWeaponSwapSlotPresenter SubWeapon_WeaponSwapSlot => null;

	public InGameWeaponSwapSlotPresenter WeaponSwapSlot => null;

	public GoodsTooltipPresenter GoodsTooltip => null;

	public Image SelectionIndicator => null;
}
