public enum EArtifactDealState
{
	None = 0,
	AwaitSelectArtifact = 1,
	AwaitSwapArtifact = 2,
	AwaitAction = 3,
	AwaitActionWithNoCount = 4,
	PlayCard = 5,
	PlayReroll = 6,
	PlayDealSetup = 7,
	PlaySwapSetup = 8,
	StopBeforeDeal = 9,
	GiveUpDeal = 10,
	AcceptDeal = 11
}
