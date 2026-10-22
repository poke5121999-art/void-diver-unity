namespace Unity.Netcode
{
	public static class Extensions
	{
		private const double SERVER_BUFFER_SEC = 0.04;

		private const double LOCAL_BUFFER_SEC = 0.02;

		private const double HARD_RESET_THRESHOLD_SEC = 0.4;

		private const double ADJUSTMENT_RATIO = 0.01;

		public static void InitConfig(this NetworkManager networkManager)
		{
			networkManager.NetworkTimeSystem.ServerBufferSec = 0.04;
			networkManager.NetworkTimeSystem.LocalBufferSec = 0.02;
			networkManager.NetworkTimeSystem.HardResetThresholdSec = 0.4;
			networkManager.NetworkTimeSystem.AdjustmentRatio = 0.01;
		}
	}
}
