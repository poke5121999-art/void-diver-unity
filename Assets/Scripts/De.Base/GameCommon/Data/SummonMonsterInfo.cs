namespace GameCommon.Data
{
	public class SummonMonsterInfo
	{
		public int Id;

		public int LifeTime;

		public int Probability;

		public bool DieOnSummonerDeath;

		public bool Validate(out string errorMessage)
		{
			errorMessage = null;
			return false;
		}
	}
}
