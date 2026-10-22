using NemoLib.Mvp;

public class MenuPopupModel : Model
{
	public MenuPopupPresenter.EPageCategory PageCategory { get; set; }

	public InventoryManagementPagePresenter.ELayout InventoryManagementLayout { get; set; }

	public LootingInventory LootingInventory { get; set; }
}
