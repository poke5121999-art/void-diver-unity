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

	public GameObject[] AccessorySlots => null;

	public GameObject[] ArtifactSlots => null;

	public GameObject[] EquipmentSlots => null;

	public GameObject SubWeapon => null;

	public GameObject SubWeapon_Normal => null;

	public Image SubWeapon_Dim => null;

	public Image SubWeapon_IconSwap => null;

	public GameObject SubWeapon_KeyPrompt => null;

	public InventoryGoodsSlotPresenter SubWeapon_WeaponSlot => null;

	public InventoryGoodsSlotPresenter WeaponSlot => null;
}
