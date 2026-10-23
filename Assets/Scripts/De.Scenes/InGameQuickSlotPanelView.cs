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

	public GameObject Contents => contents;

	public GameObject[] Slots => slots;

	public GameObject SubWeapon => subWeapon;

	public GameObject SubWeapon_Normal => subWeapon_Normal;

	public Image SubWeapon_Dim => subWeapon_Dim;

	public Image SubWeapon_IconSwap => subWeapon_IconSwap;

	public InGameWeaponSwapSlotPresenter SubWeapon_WeaponSwapSlot => subWeapon_WeaponSwapSlot;

	public InGameWeaponSwapSlotPresenter WeaponSwapSlot => weaponSwapSlot;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;

	public Image SelectionIndicator => selectionIndicator;
}
