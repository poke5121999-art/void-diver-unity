using GameCommon.Enum;
using UniRx;

public class ArtifactDealCardModel : BaseSlotModel<ArtifactDealCardModel>
{
	public int Id;

	public readonly ReactiveProperty<bool> RxIsPressed;

	public EArtifactDealCardRarityType Rarity => EArtifactDealCardRarityType.None;
}
