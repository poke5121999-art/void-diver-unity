using GameCommon.Table;
using System.Text;
namespace GameCommon.Data
{
	public class SummonMonsterInfo
	{
		public int Id;

		public int LifeTime = -1;

		public int Probability = 1;

		public bool DieOnSummonerDeath;

		public bool Validate(out string errorMessage)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool isValid = TMonster.Table.ContainsKey(Id);
			if (!isValid)
			{
				stringBuilder.AppendLine(string.Format("{0} [{1}] is not exist in  {2}", "Id", Id, "TMonster"));
			}
			errorMessage = stringBuilder.ToString();
			return isValid;
		}
	}
}
