using UniRx;

public class PlayerSlotModel : BaseSlotModel<PlayerSlotModel>
{
	public int PlayerSlotIndex;

	public readonly Subject<PlayerSlotModel> OnDeleteClick = new Subject<PlayerSlotModel>();
}
