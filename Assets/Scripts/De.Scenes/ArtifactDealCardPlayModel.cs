using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;

public class ArtifactDealCardPlayModel : Model
{
	public int Id;

	public readonly Subject<ArtifactDealCardPlayModel> OnClick;

	public readonly ReactiveProperty<bool> RxIsPressed;

	public EArtifactDealCardRarityType Rarity => EArtifactDealCardRarityType.None;
}
