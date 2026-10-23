using GameCommon.Data;

public class PlayingCrisisEvent : IBaseEvent
{
	public EPlayCat PlayCat { get; }

	public string UPlayId { get; }

	public string Stg { get; }

	public int Round { get; }

	public EPlayWin Pwin { get; }

	public int Ptm { get; }

	public string UseChar { get; }

	public int PlayerCount { get; }

	public int? AnomalyId { get; }

	public string UseSkill { get; }

	public string UseEquipment { get; }

	public PlayingCrisisEvent(EPlayCat playCat, string uPlayId, string stg, int round, EPlayWin pwin, int ptm, string useChar = null, int playerCount = 1, int? anomalyId = null, string useSkill = null, string useEquipment = null)
	{
		PlayCat = playCat;
		UPlayId = uPlayId;
		Stg = stg;
		Round = round;
		Pwin = pwin;
		Ptm = ptm;
		UseChar = useChar;
		PlayerCount = playerCount;
		AnomalyId = anomalyId;
		UseSkill = useSkill;
		UseEquipment = useEquipment;
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return currentProgress;
	}
}
