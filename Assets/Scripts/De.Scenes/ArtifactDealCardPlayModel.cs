using GameCommon.Enum;
using NemoLib.Mvp;
using GameCommon.Table;
using UniRx;

public class ArtifactDealCardPlayModel : Model
{
	public int Id;

	public readonly Subject<ArtifactDealCardPlayModel> OnClick = new Subject<ArtifactDealCardPlayModel>();

	public readonly ReactiveProperty<bool> RxIsPressed = new ReactiveProperty<bool>();

	public EArtifactDealCardRarityType Rarity => TArtifactDealCard.Table[Id].RarityType;
}
