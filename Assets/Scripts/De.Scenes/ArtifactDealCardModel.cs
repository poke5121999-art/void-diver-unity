using GameCommon.Enum;
using GameCommon.Table;
using UniRx;

public class ArtifactDealCardModel : BaseSlotModel<ArtifactDealCardModel>
{
	public int Id;

	public readonly ReactiveProperty<bool> RxIsPressed = new ReactiveProperty<bool>();

	public EArtifactDealCardRarityType Rarity => TArtifactDealCard.Table[Id].RarityType;
}
