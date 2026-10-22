using UniRx;

public class MenuBuffScrollerModel : BaseScrollerModel<MenuBuffSlotModel>
{
	public CharacterModel CharacterModel;

	public ReactiveProperty<int> RxVisibleCount;
}
