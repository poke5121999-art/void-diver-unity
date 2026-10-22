using GameCommon.Data;

public readonly struct ArtifactDealCardResolvedEvent : IArtifactDealEvent
{
	public int CardId { get; }

	public bool IsSuccess { get; }

	public EArtifactDealSuccessRateTier SuccessTier { get; }

	public ArtifactDealCardEffect[] Effects { get; }

	public ArtifactDealCardResolvedEvent(int cardId, bool isSuccess, EArtifactDealSuccessRateTier successTier, ArtifactDealCardEffect[] effects)
	{
		CardId = cardId;
		Effects = effects;
		IsSuccess = isSuccess;
		SuccessTier = successTier;
	}
}
