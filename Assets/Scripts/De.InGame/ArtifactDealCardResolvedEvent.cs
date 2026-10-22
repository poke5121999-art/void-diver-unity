using GameCommon.Data;

public readonly struct ArtifactDealCardResolvedEvent : IArtifactDealEvent
{
	public int CardId { get; }

	public bool IsSuccess { get; }

	public EArtifactDealSuccessRateTier SuccessTier { get; }

	public ArtifactDealCardEffect[] Effects { get; }

	public ArtifactDealCardResolvedEvent(int cardId, bool isSuccess, EArtifactDealSuccessRateTier successTier, ArtifactDealCardEffect[] effects)
	{
		CardId = 0;
		IsSuccess = false;
		SuccessTier = EArtifactDealSuccessRateTier.High;
		Effects = null;
	}
}
