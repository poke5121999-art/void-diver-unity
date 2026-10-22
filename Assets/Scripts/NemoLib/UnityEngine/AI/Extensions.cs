namespace UnityEngine.AI
{
	public static class Extensions
	{
		public static float GetTotalLength(this NavMeshPath path, float valueIfError)
		{
			if (path == null)
			{
				return valueIfError;
			}
			if (path.corners == null || path.corners.Length < 2)
			{
				return valueIfError;
			}
			if (path.status != NavMeshPathStatus.PathComplete)
			{
				return valueIfError;
			}
			float num = 0f;
			for (int i = 1; i < path.corners.Length; i++)
			{
				num += Vector3.Distance(path.corners[i], path.corners[i - 1]);
			}
			return num;
		}
	}
}
