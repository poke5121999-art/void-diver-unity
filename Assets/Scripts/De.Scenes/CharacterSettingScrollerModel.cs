using UniRx;

public class CharacterSettingScrollerModel : BaseScrollerModel<CharacterSettingSlotModel>
{
	public Character[] Characters;

	public ReactiveProperty<int> RxSelectedCharacterId;
}
