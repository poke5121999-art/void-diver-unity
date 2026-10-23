using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInventoryPanelView : View
{
	[SerializeField]
	private GameObject[] accessorySlots;

	[SerializeField]
	private GameObject[] artifactSlots;

	[SerializeField]
	private GameObject[] equipmentSlots;

	[SerializeField]
	private GameObject subWeapon;

	[SerializeField]
	private GameObject subWeapon_Normal;

	[SerializeField]
	private Image subWeapon_Dim;

	[SerializeField]
	private Image subWeapon_IconSwap;

	[SerializeField]
	private GameObject subWeapon_KeyPrompt;

	[SerializeField]
	private InventoryGoodsSlotPresenter subWeapon_WeaponSlot;

	[SerializeField]
	private InventoryGoodsSlotPresenter weaponSlot;

	public GameObject[] AccessorySlots => accessorySlots;

	public GameObject[] ArtifactSlots => artifactSlots;

	public GameObject[] EquipmentSlots => equipmentSlots;

	public GameObject SubWeapon => subWeapon;

	public GameObject SubWeapon_Normal => subWeapon_Normal;

	public Image SubWeapon_Dim => subWeapon_Dim;

	public Image SubWeapon_IconSwap => subWeapon_IconSwap;

	public GameObject SubWeapon_KeyPrompt => subWeapon_KeyPrompt;

	public InventoryGoodsSlotPresenter SubWeapon_WeaponSlot => subWeapon_WeaponSlot;

	public InventoryGoodsSlotPresenter WeaponSlot => weaponSlot;
}
